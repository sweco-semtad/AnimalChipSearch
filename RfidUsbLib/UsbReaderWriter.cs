using System;
using System.IO;
using System.IO.Ports;
using System.Management;
using LibUsbDotNet;
using System.Collections.ObjectModel;
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

        private RFIDChipIdReceiver _receiver;
        public RFIDChipIdReceiver Receiver { set { _receiver = value;} }

        /// <summary>
        /// Singleton instance
        /// </summary>
        private static UsbReaderWriter _instance;
        public static UsbReaderWriter Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UsbReaderWriter();
                return _instance;
            }
        }

        /// <summary>
        /// Private constructor
        /// </summary>
        private UsbReaderWriter()
        {
            // Hook the device notifier event
            UsbDeviceNotifier.OnDeviceNotify += OnDeviceNotifyEvent;

            if (IsRFIDReaderConnected())
            {
                ConnectToDevice();
            }
        }

        /// <summary>
        /// Connect to device
        /// </summary>
        /// <param name="portName"></param>
        private void ConnectToDevice(String portName = null)
        {
            // Create a new SerialPort object with default settings.
            _serialPort = new SerialPort();

            if (portName == null)
            {
                try
                {
                    ManagementObjectSearcher searcher =
                        new ManagementObjectSearcher("root\\WMI",
                        "SELECT * FROM MSSerial_PortName");

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
                    throw new Exception("An error occurred while querying for WMI data", e);
                }
            }

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
        }

        /// <summary>
        /// Data received from RFID device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataReceivedHandler(object sender,SerialDataReceivedEventArgs e) {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            // TODO event for data recieved
            if (_receiver != null)
                _receiver.ChipIdRead(indata);
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
            if (Instance.IsThisRFIDReader(e.Device.IdVendor, e.Device.IdProduct))
            {
                if (e.EventType == EventType.DeviceArrival)
                {
                    ConnectToDevice(e.Port.Name);
                }
                else if (e.EventType == EventType.DeviceRemoveComplete)
                {
                    DeviceDisconnected();
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
        /// Device is disconnected. Hous cleaning
        /// </summary>
        private void DeviceDisconnected()
        {
            if (_serialPort != null)
                _serialPort.Close();

            _deviceConnected = false;
        }
    }

    public interface RFIDChipIdReceiver
    {
        void ChipIdRead(String chipId);
    }
}
