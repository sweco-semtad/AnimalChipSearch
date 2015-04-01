using System;
using System.Windows;
using System.Windows.Controls;
using AnimalChipSearch.ViewModels;
//using SKKSearchAPI;
using MahApps.Metro.Controls;

namespace AnimalChipSearch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        private MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            //  We have declared the view model instance declaratively in the xaml.
            _viewModel = new MainWindowViewModel();
            this.DataContext = _viewModel;

            _viewModel.MessageBoxRequest += new EventHandler<MvvmMessageBoxEventArgs>(MyView_MessageBoxRequest);
        }

        void MyView_MessageBoxRequest(object sender, MvvmMessageBoxEventArgs e)
        {
            e.Show();
        }
    }
}
