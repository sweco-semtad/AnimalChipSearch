using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKKSearchAPI;
using System.Windows.Input;
using System.Windows;

namespace AnimalChipSearch.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
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

        public String TxtbChipId { get; set; }

        public String TxtbInkId { get; set; }

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

        public MainWindowViewModel()
        {
            // Initialize the combobox for djurslag
            DjurslagList = new ObservableCollection<DjurslagViewModel>();
            DjurslagList.Add(new DjurslagViewModel { Djurslag = Djurslag.Hund });
            DjurslagList.Add(new DjurslagViewModel { Djurslag = Djurslag.Katt });
            _selectedDjurslag = DjurslagList[0];

            IsUIEnabled = true;

            // Create view models
            SearchListViewModel = new SearchListViewModel(this);
            EmptyControlViewModel = new EmptyControlViewModel { Text = GetString("info") };
            DogViewModel = new DogViewModel(this);
            CatViewModel = new CatViewModel(this);

            // Set the current view
            CurrentView = EmptyControlViewModel;
        }

        private DjurslagViewModel _selectedDjurslag;
        public DjurslagViewModel SelectedDjurslag
        {
            get { return _selectedDjurslag; }
            set {
                _selectedDjurslag = value;
                RaisePropertyChanged("SelectedDjurslag");
            }
        }

        private ObservableCollection<DjurslagViewModel> _djurslagList;
        public ObservableCollection<DjurslagViewModel> DjurslagList
        {
            get { return _djurslagList; }
            set { 
                _djurslagList = value;
                RaisePropertyChanged("DjurslagList");
            }
        }

        private async void SearchAnimals()
        {
            try
            {
                DisableUI();
                EmptyControlViewModel.Text = "Söker...";
                CurrentView = EmptyControlViewModel;

                var djurslag = _selectedDjurslag.Djurslag;

                AnimalList responseObject = null;

                await Task.Run(() =>
                {
                    if (djurslag == Djurslag.Hund)
                    {
                        responseObject = skkSearch.SearchDogs(TxtbInkId, TxtbChipId);
                    }
                    else
                    {
                        responseObject = skkSearch.SearchCats(TxtbInkId, TxtbChipId);
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

        bool CanExecuteSearch()
        {
            return true;
        }

        public ICommand InitSearchCommand { get { return new RelayCommand(InitSearch, CanExecuteSearch); } }

        #endregion

    }
}
