using System.Windows;
using System.Windows.Input;
using HelloWorldWpf.Commands;
using HelloWorldWpf.ViewModels;

namespace HelloWorldWpf
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            DataContext = viewModel;
            ClearSearchCommand = new RelayCommand(ClearSearch);
        }

        public ICommand ClearSearchCommand { get; }

        private void ClearSearch()
        {
            viewModel.SearchText = string.Empty;
        }
    }
}
