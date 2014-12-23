using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SKKRegisterSok;

namespace AnimalChipSearch
{
    /// <summary>
    /// Interaction logic for DetailWindow.xaml
    /// </summary>
    public partial class DetailWindow : Window
    {
        private Animal _animal;

        public DetailWindow(Animal animalDetails)
        {
            InitializeComponent();

            _animal = animalDetails;

            LblName.Content = animalDetails.Namn;
            LblRas.Content = animalDetails.Ras;
            LblFarg.Content = animalDetails.Farg;
            LblRegnr.Content = animalDetails.RegId;
            LblChipNr.Content = animalDetails.ChipId;
            LblTatoo.Content = animalDetails.TatueringsId;
            LblRas.Content = animalDetails.Ras;

            // Agare
            LblAgareNamn.Content = animalDetails.Agare.Namn;
            LblAgareAdress.Content = animalDetails.Agare.Adress;
            LblAgareEpost.Content = animalDetails.Agare.Epost;
            LblAgareTelArb.Content = animalDetails.Agare.TelArbete;
            LblAgareTelHem.Content = animalDetails.Agare.TelHem;
            LblAgareTelMob.Content = animalDetails.Agare.TelMobil;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
