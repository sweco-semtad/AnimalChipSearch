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
using SKKSearchAPI;

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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                resultList.Items.Clear();
                resultList.Items.Add(new Animal() { Namn = "Söker, vänta..." });

                String chipId = chipIdTxt.Text;
                String inkId = inkIdTxt.Text;

                var djurslag = ((ComboBoxItem)DjurslagCB.SelectedItem).Name == "Hund" ? Djurslag.Hund : Djurslag.Katt;

                DisableUI();

                await Task.Run(() =>
                {
                    if (djurslag == Djurslag.Hund)
                    {
                        animalList = skkSearch.SearchDogs(inkId, chipId);
                    }
                    else
                    {
                        animalList = skkSearch.SearchCats(inkId, chipId);
                    }
                });

                if (animalList.errorMessage != null && animalList.errorMessage != String.Empty)
                {
                    resultList.Items.Clear();
                    if (animalList.errorMessage == ErrorMessages.NoDogsFound)
                    {
                        string ingaHundar = (string)Application.Current.FindResource("ingaHundar");
                        resultList.Items.Add(new Animal() { Namn = ingaHundar });
                    }
                }
                else
                {
                    resultList.Items.Clear();

                    foreach (Animal animal in animalList.animals)
                    {
                        resultList.Items.Add(animal);
                    }

                    if (animalList.HasMoreThan20)
                    {
                        string moreThan20 = (string)Application.Current.FindResource("moreThan20");
                        System.Windows.MessageBox.Show(moreThan20);
                    }
                }

                EnableUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async void ListViewItemDoubleClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = sender as ListViewItem;
                var animal = (Animal)item.DataContext;
                
                DisableUI();
                await Task.Run(() =>
                {
                    if (animalList.Species == Djurslag.Hund)
                        animal = skkSearch.GetDogDetails(animal);
                    else
                        animal = skkSearch.GetCatDetails(animal);
                });

                DetailWindow dtWindow = new DetailWindow(animal, this);
                EnableUI();
                dtWindow.ShowDialog();
            }
            catch (Exception exc)
            {
                System.Windows.MessageBox.Show(exc.Message);
            }
        }

        private void DisableUI()
        {
            SearchButton.IsEnabled = false;
            resultList.IsEnabled = false;
            chipIdTxt.IsEnabled = false;
            inkIdTxt.IsEnabled = false;
            DjurslagCB.IsEnabled = false;
        }

        private void EnableUI()
        {
            SearchButton.IsEnabled = true;
            resultList.IsEnabled = true;
            chipIdTxt.IsEnabled = true;
            inkIdTxt.IsEnabled = true;
            DjurslagCB.IsEnabled = true;
        }
    }
}
