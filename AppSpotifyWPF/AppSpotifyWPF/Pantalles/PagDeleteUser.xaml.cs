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

namespace AppSpotifyWPF.Pantalles
{
    public partial class PagDeleteUser : Page
    {
        public PagDeleteUser()
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

