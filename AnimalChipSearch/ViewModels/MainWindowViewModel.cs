using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SKKSearchAPI;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows;
using com.kit.RfidUsbLib;

namespace AnimalChipSearch.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, RFIDChipIdReceiver
    {
        SKKRegisterSok.SKKSearch skkSearch = new SKKRegisterSok.SKKSearch();
        
        private ViewModelBase _currentView;
        public ViewModelBase CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                RaisePropertyChanged("CurrentView");
            }
        }

        public SearchListViewModel SearchListViewModel { get; set; }

        public DogViewModel DogViewModel { get; set; }

        public CatViewModel CatViewModel { get; set; }

        public EmptyControlViewModel EmptyControlViewModel { get; set; }

        public String TxtId { get; set; }

        private Boolean _isUIEnabled;
        public Boolean IsUIEnabled
        {
            get { return _isUIEnabled; }
            set
            {
                _isUIEnabled = value;
                RaisePropertyChanged("IsUIEnabled");
            }
        }

        private Boolean _catMode;
        public Boolean CatMode
        {
            get { return _catMode; }
            set
            {
                _catMode = value;
                RaisePropertyChanged("CatMode");
            }
        }

        private static String ChipId = "Chip";
        private static String TatooId = "Tatuering";

        private IdModell _idMode = IdModell.Chip;

        private String _flipButtonText;
        public String FlipButtonText
        {
            get { return _flipButtonText; }
            set
            {
                _flipButtonText = value;
                RaisePropertyChanged("FlipButtonText");
            }
        }

        private UsbReaderWriter _usbBgObject;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowViewModel() {

            IsUIEnabled = true;

            FlipButtonText = ChipId;

            // Create view models
            SearchListViewModel = new SearchListViewModel(this);
            EmptyControlViewModel = new EmptyControlViewModel { Text = GetString("info") };
            DogViewModel = new DogViewModel(this);
            CatViewModel = new CatViewModel(this);

            // Set the current view
            CurrentView = EmptyControlViewModel;

            // Initialize the USB RFID object
            _usbBgObject = new UsbReaderWriter(this);

            // Listen for shutdown event
            Application.Current.Dispatcher.ShutdownStarted += Dispatcher_ShutdownStarted;
        }

        private async void SearchAnimals()
        {
            try
            {
                DisableUI();
                EmptyControlViewModel.Text = "Söker...";
                CurrentView = EmptyControlViewModel;

                var djurslag = _catMode ? Djurslag.Katt : Djurslag.Hund;

                AnimalList responseObject = null;

                await Task.Run(() =>
                {
                    if (djurslag == Djurslag.Hund)
                    {
                        responseObject = skkSearch.SearchDogs(_idMode, TxtId);
                    }
                    else
                    {
                        responseObject = skkSearch.SearchCats(_idMode, TxtId);
                    }
                });

                if (responseObject.errorMessage != null && responseObject.errorMessage != String.Empty)
                {
                    // Got error message from service
                    if (responseObject.errorMessage == ErrorMessages.NoDogsFound)
                    {
                        EmptyControlViewModel.Text = GetString("ingaHundar");
                    }
                    else if (responseObject.errorMessage == ErrorMessages.NoCatsFound)
                    {
                        EmptyControlViewModel.Text = GetString("ingaKatter");
                    }
                    else
                    {
                        EmptyControlViewModel.Text = GetString("okäntFel");
                        // Lägg till felmeddelandet
                        EmptyControlViewModel.Text += "\n\n" + responseObject.errorMessage;
                    }

                    CurrentView = EmptyControlViewModel;
                }
                else
                {
                    // We got a result list, lets show it
                    SearchListViewModel.Update(responseObject);

                    if (responseObject.animals.Count == 1)
                        AnimalDoubleCkick(responseObject.animals.First());
                    else
                        CurrentView = SearchListViewModel;
                }

                EnableUI();
            }
            catch (Exception ex)
            {
                MessageBox_Show(null, ex.Message, "Fel", System.Windows.MessageBoxButton.OK);
                EnableUI();
            }
        }

        public void ShowList()
        {
            CurrentView = SearchListViewModel;
        }

        public void AnimalDoubleCkick(Animal animal)
        {
            GetDetails(animal);
            // Cat or dog?
            if (animal.Species == Djurslag.Hund)
                CurrentView = DogViewModel;
            else
                CurrentView = CatViewModel;
            
        }

        public async void GetDetails(Animal animal)
        {
            await Task.Run(() =>
            {
                if (animal.Species == Djurslag.Hund)
                {
                    animal = skkSearch.GetDogDetails(animal);
                    DogViewModel.Animal = animal;
                }
                else
                {
                    animal = skkSearch.GetCatDetails(animal);
                    CatViewModel.Animal = animal;
                }
            });
        }

        /// <summary>
        /// Received chip number read by RFID reader
        /// </summary>
        /// <param name="chipId"></param>
        public void ChipIdRead(String chipId)
        {
            TxtId = chipId;
            RaisePropertyChanged("TxtbChipId");
            SearchAnimals();
        }

        /// <summary>
        /// Notification that a reader is connected or disconnected
        /// </summary>
        /// <param name="deviceConnected">True: device connected, False: device disconnected</param>
        public void DeviceConnectionEvent(bool deviceConnected)
        {
            // TODO visa att läsaren är ansluten
            //MessageBox_Show(null, "Device connection change: " + deviceConnected, "ChipId", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None);
        }

        /// <summary>
        /// Read error from the RFID reader
        /// </summary>
        /// <param name="message"></param>
        public void ReadError(string message)
        {
            MessageBox_Show(null, "Läsfel: " + message, "Läsfel", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None);
        }

        private void DisableUI()
        {
            IsUIEnabled = false;
        }

        private void EnableUI()
        {
            IsUIEnabled = true;
        }

        #region Commands
        void InitSearch()
        {
            SearchAnimals();
        }

        void FlipButton()
        {
            if (_idMode == IdModell.Chip)
            {
                FlipButtonText = TatooId;
                _idMode = IdModell.Tatuering;
            }
            else
            {
                FlipButtonText = ChipId;
                _idMode = IdModell.Chip;
            }
        }

        bool CanExecuteSearch()
        {
            return true;
        }

        public ICommand InitSearchCommand { get { return new RelayCommand(InitSearch, CanExecuteSearch); } }

        public ICommand FlipButtonCommand { get { return new RelayCommand(FlipButton, CanExecuteSearch); } }


        #endregion

        private void Dispatcher_ShutdownStarted(object sender, EventArgs e)
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_usbBgObject != null)
                _usbBgObject.Dispose();

            // Suppress finalization.
            GC.SuppressFinalize(this);
        }
    }
}
