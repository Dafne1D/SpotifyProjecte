using System.Windows;
using System.Windows.Controls;


namespace AppSpotifyWPF.Screens.Songs
{
    public partial class PagDeleteSong : Page
    {
        public PagDeleteSong()
        {
            InitializeComponent();
        }

        private void BackToHome_Click(object sender, RoutedEventArgs e)
        {
            // Obtener la ventana principal (HomeScreen)
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow is HomeScreen home)
            {
                // Oculta el Frame y vuelve a mostrar el contenido principal
                home.MainFrame.Visibility = Visibility.Collapsed;
                home.HomeContent.Visibility = Visibility.Visible;
            }
        }
    }
}

