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

namespace GestorPersones.View
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dgrEmpleats.ItemsSource = Empleat.GetEmpleats();
            dgrEmpleats.Columns[3].Visibility = Visibility.Collapsed;
            dgrEmpleats.Columns[4].Visibility = Visibility.Collapsed;
        }



        public Empleat EmpleatSeleccionat
        {
            get { return (Empleat)GetValue(EmpleatSeleccionatProperty); }
            set { SetValue(EmpleatSeleccionatProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EmpleatSeleccionat.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EmpleatSeleccionatProperty =
            DependencyProperty.Register("EmpleatSeleccionat", typeof(Empleat), typeof(MainWindow), new PropertyMetadata(null));

        private void dgrEmpleats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Empleat emp = (Empleat) dgrEmpleats.SelectedValue;
            // Treballarem sobre una còpia, amb tota la tranquilitat del món.
            EmpleatSeleccionat = emp.Clonar();
        }

        private void btnDeleteProj_Click(object sender, RoutedEventArgs e)
        {
            if (dgrProjectes.SelectedValue != null)
            {
                EmpleatSeleccionat.RemoveProjecte((Projecte)dgrProjectes.SelectedValue);
            }
            
        }

        private void btnAddProj_Click(object sender, RoutedEventArgs e)
        {
            EmpleatSeleccionat.AddProjecte((Projecte)cbProjectes.SelectedValue);
        }
    }
}
