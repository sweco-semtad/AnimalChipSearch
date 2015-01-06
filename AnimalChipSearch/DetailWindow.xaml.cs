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
using SKKSearchAPI;

namespace AnimalChipSearch
{
    /// <summary>
    /// Interaction logic for DetailWindow.xaml
    /// </summary>
    public partial class DetailWindow : Window
    {
        private Animal _animal;

        SKKRegisterSok.SKKSearch skkSearch = new SKKRegisterSok.SKKSearch();

        public DetailWindow(Animal animalDetails, Window parent)
        {
            Owner = parent;
            InitializeComponent();
            Update(animalDetails);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        internal void Update(Animal animalDetails)
        {
            _animal = animalDetails;

            txtName.Text = animalDetails.Namn;
            txtRas.Text = animalDetails.Ras;
            txtFarg.Text = animalDetails.Farg;
            txtRegnr.Text = animalDetails.RegId;
            txtChipNr.Text = animalDetails.ChipId;
            txtTatoo.Text = animalDetails.TatueringsId;

            // Agare
            txtAgareNamn.Text = animalDetails.Agare.Namn;
            txtAgareAdress.Text = animalDetails.Agare.Adress;
            txtAgareEpost.Text = animalDetails.Agare.Epost;
            txtAgareTelArb.Text = animalDetails.Agare.TelArbete;
            txtAgareTelHem.Text = animalDetails.Agare.TelHem;
            txtAgareTelMob.Text = animalDetails.Agare.TelMobil;
        }
    }
}
