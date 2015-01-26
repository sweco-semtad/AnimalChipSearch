using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimalChipSearch;
using SKKSearchAPI;

namespace AnimalChipSearch.ViewModels
{
    public class SearchListViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainViewModel;

        private ObservableCollection<Animal> _animalList;
        public ObservableCollection<Animal> AnimalsList
        {
            get { return _animalList; }
            set
            {
                _animalList = value;
                RaisePropertyChanged("AnimalList");
            }
        }

        public SearchListViewModel(MainWindowViewModel mainViewModel) {
            _animalList = new ObservableCollection<Animal>();
            _mainViewModel = mainViewModel;
        }

        public void Update(AnimalList list) {

            AnimalsList.Clear();

            foreach (Animal animal in list.animals)
            {
                AnimalsList.Add(animal);
            }

            if (list.HasMoreThan20)
            {
                MessageBox_Show(null, GetString("moreThan20"), "Fel", System.Windows.MessageBoxButton.OK);
            }
        }

        public void AnimalDoubleCkick(Animal animal)
        {
            _mainViewModel.AnimalDoubleCkick(animal);
        }
    }
}
