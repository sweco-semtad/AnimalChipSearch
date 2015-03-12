using System;
using System.IO;
using System.IO.Ports;
using System.Management;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using System.Threading;
using System.Threading.Tasks;

using LibUsbDotNet;
using LibUsbDotNet.Main;
using LibUsbDotNet.Info;
using LibUsbDotNet.DeviceNotify;
using LibUsbDotNet.LudnMonoLibUsb;

namespace com.kit.RfidUsbLib
{
    public class UsbReaderWriter
    {
        private static IDeviceNotifier UsbDeviceNotifier = DeviceNotifier.OpenDeviceNotifier();
        
        private SerialPort _serialPort;

        private bool _deviceConnected = false;

        private string _chipId;

        private RFIDChipIdReceiver _receiver;

        /// <summary>
        /// Constructor
        /// </summary>
        public UsbReaderWriter(RFIDChipIdReceiver receiver)
        {
            _receiver = receiver;

            // Hook the device notifier event
            UsbDeviceNotifier.OnDeviceNotify += OnDeviceNotifyEvent;

            if (IsRFIDReaderConnected())
            {
                String portName = GetPortNameAsync();

                if (portName != null)
                    ConnectToDevice(portName);
            }
        }

        /// <summary>
        /// Connect to device
        /// </summary>
        /// <param name="portName"></param>
        public void ConnectToDevice(String portName)
        {
            // Create a new SerialPort object with default settings.
            _serialPort = new SerialPort();

            if (portName == null)
            {
                // No device connected
                return;
            }

            // Allow the user to set the appropriate properties.
            _serialPort.PortName = portName;
            _serialPort.BaudRate = 9600;
            _serialPort.Parity = Parity.None;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;

            // Set the read/write timeouts
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;

            _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            _serialPort.Open();

            _deviceConnected = true;

            if (_receiver != null)
            {
                _receiver.DeviceConnectionEvent(_deviceConnected);
            }
        }

        private string GetPortNameAsync()
        {
            String portName = null;
            Task t = Task.Run(() =>
            {
                try
                {
                    //WMISearcher.Options.ReturnImmediately = True
                    ManagementObjectSearcher searcher =
                        new ManagementObjectSearcher("root\\WMI",
                        "SELECT * FROM MSSerial_PortName");
                    searcher.Options.ReturnImmediately = false;

                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        //If the serial port's instance name contains USB 
                        //it must be a USB to serial device
                        if (queryObj["InstanceName"].ToString().Contains("FTDI"))
                        {
                            portName = queryObj["PortName"].ToString();
                        }
                    }
                }
                catch (ManagementException e)
                {
                    //throw new Exception("An error occurred while querying for WMI data", e);
                }
            });
            t.Wait();
            return portName;
        }

        /// <summary>
        /// Data received from RFID device.
        /// Sometimes we get more than one event and the chip id is
        /// broken up between the events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e) {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();

            if (indata.Contains("\r"))
            {
                if (!string.IsNullOrEmpty(_chipId))
                    _chipId += indata.Replace("\r", "");
                else
                    _chipId = indata.Replace("\r", "");

                if (_receiver != null)
                {
                    //var countryCode = _chipId.Substring(0, 3);
                    //if (countryCode != "941" && _receiver != null)
                    //{
                    //    _receiver.ReadError("Chipet har en landskod som inte är Svensk. (" + countryCode + ")");
                    //    return;
                    //}

                    // Remove the countrycode
                    var chipId = _chipId.Replace("_", "");
                    // Notify the receiver
                    _receiver.ChipIdRead(chipId);
                }

                // Reset the id
                _chipId = string.Empty;
            }
            else
            {
                _chipId += indata;
            }
        }

        /// <summary>
        /// Is the RFID reader writer connected?
        /// </summary>
        /// <returns></returns>
        private bool IsRFIDReaderConnected()
        {
            UsbRegDeviceList allDevices = UsbDevice.AllDevices;
            foreach (UsbRegistry usbRegistry in allDevices)
            {
                UsbDeviceInfo deviceInfo = usbRegistry.Device.Info;
                return IsThisRFIDReader(deviceInfo.Descriptor.VendorID, deviceInfo.Descriptor.ProductID);
            }

            return false;
        }

        /// <summary>
        /// Validate that the connected device is the RFIDRW-E-232
        /// </summary>
        /// <param name="vendorId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        private bool IsThisRFIDReader(int vendorId, int productId)
        {
            return vendorId == 1027 && productId == 24577;
        }

        /// <summary>
        /// Event fired when USB devices are disconnected and connected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDeviceNotifyEvent(object sender, DeviceNotifyEventArgs e)
        {
            // A Device system-level event has occured
            if (IsThisRFIDReader(e.Device.IdVendor, e.Device.IdProduct))
            {
                if (e.EventType == EventType.DeviceArrival)
                {
                    String portName = GetPortNameAsync();

                    if (portName != null)
                        ConnectToDevice(portName);
                }
                else if (e.EventType == EventType.DeviceRemoveComplete)
                {
                    DeviceDisconnecting();
                }
            }
        }

        /// <summary>
        /// Send a command on the connected com port
        /// </summary>
        /// <param name="command"></param>
        private void SendCommand(String command)
        {
            if (_deviceConnected)
            {
                _serialPort.Write(command + "\r");
            }
            else
            {
                throw new Exception("Device not connected");
            }
        }

        /// <summary>
        /// Device is disconnected. House cleaning
        /// </summary>
        private void DeviceDisconnecting()
        {
            if (_serialPort != null)
                _serialPort.Close();

            _deviceConnected = false;

            // Notify the reciever
            if (_receiver != null)
            {
                _receiver.DeviceConnectionEvent(_deviceConnected);
            }
        }

        public void Dispose()
        {
            // Close connections
            DeviceDisconnecting();

            // Unregister for USB events
            UsbDeviceNotifier.OnDeviceNotify -= OnDeviceNotifyEvent;

            // Suppress finalization.
            GC.SuppressFinalize(this);
        }
    }

    public interface RFIDChipIdReceiver
    {
        void ChipIdRead(String chipId);

        void DeviceConnectionEvent(bool deviceConnected);

        void ReadError(string message);
    }
}
