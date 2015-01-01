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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SKKRegisterSok;

namespace AnimalChipSearch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SKKRegisterSok.SKKSearch skkSearch = new SKKRegisterSok.SKKSearch();

        AnimalList animalList;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            resultList.Items.Clear();
            resultList.Items.Add(new ListViewItem() {Content = "Söker, vänta..."});

            String chipId = chipIdTxt.Text;
            String inkId = inkIdTxt.Text;

            if (((ComboBoxItem)DjurslagCB.SelectedItem).Name == "Hund")
            {
                animalList = skkSearch.SearchDogs(inkId, chipId);
            }
            else
            {
                animalList = skkSearch.SearchCats(inkId, chipId);
            }

            if (animalList.errorMessage != null && animalList.errorMessage != String.Empty)
            {
                resultList.Items.Clear();
                resultList.Items.Add(new ListViewItem() { Content = animalList.errorMessage });
            }
            else
            {
                resultList.Items.Clear();

                foreach (SKKRegisterSok.Animal animal in animalList.animals)
                {
                    resultList.Items.Add(animal);
                }
            }

        }

        public void ListViewItemDoubleClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = sender as ListViewItem;

                Animal animal = null;
                if (animalList.Species == Djurslag.Hund)
                    animal = skkSearch.GetDogDetails((Animal)item.DataContext);
                else
                    animal = skkSearch.GetCatDetails((Animal)item.DataContext);

                DetailWindow dtWindow = new DetailWindow(animal, this);
                dtWindow.ShowDialog();
            }
            catch (Exception exc)
            {
                System.Windows.MessageBox.Show(exc.Message);
            }
        }
    }
}
