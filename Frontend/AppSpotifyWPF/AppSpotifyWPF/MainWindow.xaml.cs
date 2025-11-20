
using AppSpotifyWPF.Screens;
using AppSpotifyWPF.Screens.Songs;
using AppSpotifyWPF.Screens.Users;
using System.Windows;
using System.Windows.Controls;

namespace AppSpotifyWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new HomePage());
        }
    }
}