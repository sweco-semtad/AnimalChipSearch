using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using SKKSearchAPI;
using SKKRegisterSok;

namespace AnimalChipSearch.ViewModels
{
    public class AnimalViewModel : ViewModelBase
    {
        protected MainWindowViewModel _mainViewModel;

        protected AnimalViewModel()
        {
        }

        public AnimalViewModel(MainWindowViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        private Animal _animal;
        public Animal Animal
        {
            get { return _animal; }
            set
            {
                _animal = value;
                RaisePropertyChanged("Animal");
                RaisePropertyChanged("TatueringsId");
                RaisePropertyChanged("ChipId");
                RaisePropertyChanged("RegId");
                RaisePropertyChanged("Namn");
                RaisePropertyChanged("Djurslag");
                RaisePropertyChanged("Ras");
                RaisePropertyChanged("Kon");
                RaisePropertyChanged("Saknad");
                RaisePropertyChanged("Harlag");
                RaisePropertyChanged("Farg");
                RaisePropertyChanged("Fodelsedatum");
                RaisePropertyChanged("Kastrerad");
                RaisePropertyChanged("OwnerNamn");
                RaisePropertyChanged("OwnerAdress");
                RaisePropertyChanged("OwnerEpost");
                RaisePropertyChanged("OwnerTelHem");
                RaisePropertyChanged("OwnerTelArbete");
                RaisePropertyChanged("OwnerTelMobil");
                RaisePropertyChanged("Url");
            }
        }

        public String TatueringsId
        {
            get { return _animal != null ? _animal.TatueringsId : "-"; }
        }

        public String ChipId
        {
            get { return _animal != null ? _animal.ChipId : "-"; }
        }

        public String RegId
        {
            get { return _animal != null ? _animal.RegId : "-"; }
        }

        public String Namn
        {
            get { return _animal != null ? _animal.Namn : "-"; }
        }

        public String Djurslag
        {
            get
            {

                String returnString = "-";
                if (_animal != null)
                {
                    switch (_animal.Species)
                    {
                        case SKKSearchAPI.Djurslag.Hund:
                            returnString = "Hund";
                            break;
                        case SKKSearchAPI.Djurslag.Katt:
                            returnString = "Katt";
                            break;
                    }
                }
                return returnString;
            }
        }

        public String Ras
        {
            get { return _animal != null ? _animal.Ras : "-"; }
        }

        public String Kon
        {
            get
            {
                String returnString = "-";
                if (_animal != null) {
                switch (_animal.Kon) {
                        case SKKSearchAPI.Kon.Hane:
                            returnString = "Hane";
                            break;
                        case SKKSearchAPI.Kon.Tik:
                            returnString = "Tik";
                            break;
                        case SKKSearchAPI.Kon.Hona:
                            returnString = "Hona";
                            break;
                        case SKKSearchAPI.Kon.Hund:
                            returnString = "Hund";
                            break;
                    }
                }
                return returnString;
            }
        }

        public String Saknad
        {
            get { return _animal != null && _animal.Saknad ? "Ja" : "Nej"; }
        }

        public String Harlag
        {
            get { return _animal != null ? _animal.Harlag : "-"; }
        }

        public String Farg
        {
            get { return _animal != null ? _animal.Farg : "-"; }
        }

        public String Fodelsedatum
        {
            get { return _animal != null ? _animal.Fodelsedatum : "-"; }
        }

        public String Kastrerad
        {
            get { return _animal != null && _animal.Kastrerad ? "Ja" : "Nej"; }
        }

        public String linkNum
        {
            get { return _animal != null ? _animal.linkNum : "-"; }
        }

        public String OwnerNamn
        {
            get { return _animal != null ? _animal.Agare.Namn : "-"; }
        }

        public String OwnerAdress
        {
            get { return _animal != null ? _animal.Agare.Adress : "-"; }
        }

        public String OwnerEpost
        {
            get { return _animal != null ? _animal.Agare.Epost : "-"; }
        }

        public String OwnerTelHem
        {
            get { return _animal != null ? _animal.Agare.TelHem : "-"; }
        }

        public String OwnerTelArbete
        {
            get { return _animal != null ? _animal.Agare.TelArbete : "-"; }
        }

        public String OwnerTelMobil
        {
            get { return _animal != null ? _animal.Agare.TelMobil : "-"; }
        }

        public String Url
        {
            get
            {
                if (_animal != null && _animal.DbId != null)
                {
                    return _animal.Species == SKKSearchAPI.Djurslag.Hund ?
                        SKKUrls.DogUrl + _animal.DbId :
                        SKKUrls.CatUrl + _animal.DbId;
                }

                return "";
            }
        }

        #region Commands
        void BackToList()
        {
            _mainViewModel.ShowList();
        }

        bool CanExecuteSearch()
        {
            return true;
        }

        public ICommand BackToListCommand { get { return new RelayCommand(BackToList, CanExecuteSearch); } }

        #endregion
    }
}
