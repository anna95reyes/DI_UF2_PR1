using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        private enum Estat
        {
            SENSE_CANVIS,
            AMB_CANVIS
        }

        private Estat estat;

        private Estat EstatButton
        {
            get { return estat; }
            set
            {
                estat = value;
                estatButtons(value);
            }
        }

        private void estatButtons(Estat estat)
        {
            if (estat == Estat.SENSE_CANVIS)
            {
                btnCancel.Visibility = Visibility.Collapsed;
                btnSave.Visibility = Visibility.Collapsed;
            }
            else if (estat == Estat.AMB_CANVIS)
            {
                btnCancel.Visibility = Visibility.Visible;
                btnSave.Visibility = Visibility.Visible;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            btnDelete.IsEnabled = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            recarregaLlistaEmpleats();
            btnDeleteProj.IsEnabled = false;
            btnAddProj.IsEnabled = false;
            EstatButton = Estat.SENSE_CANVIS;
        }

        private void recarregaLlistaEmpleats()
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
            if (dgrEmpleats.SelectedValue != null)
            {
                Empleat emp = (Empleat) dgrEmpleats.SelectedValue;
                // Treballarem sobre una còpia, amb tota la tranquilitat del món.
                EmpleatSeleccionat = emp.Clonar();
                EmpleatSeleccionat.PropertyChanged += Empleat_PropertyChanged;
                btnDelete.IsEnabled = true;
            }           
        }

        private void Empleat_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Boolean nifValid = Empleat.validaNif(txbNIF.Text);
            Boolean nomValid = Empleat.validaNomICognoms(txbNom.Text);
            Boolean cognomsValid = Empleat.validaNomICognoms(txbCognoms.Text);
            Boolean DataEntradaValid = Empleat.validaDataEntrada((DateTime)dtpData.SelectedDate);

            if (nifValid && nomValid && cognomsValid && DataEntradaValid)
            {
                ActivarButtonSave();
            }
            CanviarEstat();
        }

        private void btnDeleteProj_Click(object sender, RoutedEventArgs e)
        {
            if (dgrProjectes.SelectedValue != null)
            {
                EmpleatSeleccionat.RemoveProjecte((Projecte)dgrProjectes.SelectedValue);
            }
            cbProjectes.ItemsSource = EmpleatSeleccionat.ProjectesOnNoTreballo;
            dgrProjectes.SelectedValue = null;
            btnDeleteProj.IsEnabled = false;
            CanviarEstat();
        }

        private void btnAddProj_Click(object sender, RoutedEventArgs e)
        {
            if ((Projecte)cbProjectes.SelectedValue != null)
            {
                if (!EmpleatSeleccionat.ProjectesOnTreballo.Contains((Projecte)cbProjectes.SelectedValue))
                {
                    EmpleatSeleccionat.AddProjecte((Projecte)cbProjectes.SelectedValue);
                }
                cbProjectes.ItemsSource = EmpleatSeleccionat.ProjectesOnNoTreballo;
                cbProjectes.SelectedValue = null;
                btnAddProj.IsEnabled = false;
                CanviarEstat();
            }
        }

        private void dgrProjectes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnDeleteProj.IsEnabled = true;
        }

        private void cbProjectes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnAddProj.IsEnabled = true;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            dgrEmpleats.SelectedValue = null;
            EmpleatSeleccionat = null;
            btnDelete.IsEnabled = false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            EmpleatSeleccionat = ((Empleat)dgrEmpleats.SelectedValue).Clonar();
            EstatButton = Estat.SENSE_CANVIS;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int empleatSeleccionat = dgrEmpleats.SelectedIndex;
            Empleat.GetEmpleats()[empleatSeleccionat] = EmpleatSeleccionat;
            dgrEmpleats.ItemsSource = null;
            recarregaLlistaEmpleats();
            dgrEmpleats.SelectedIndex = empleatSeleccionat;
            EstatButton = Estat.SENSE_CANVIS;
        }

        private void CanviarEstat()
        {
            if (EstatButton == Estat.SENSE_CANVIS)
            {
                EstatButton = Estat.AMB_CANVIS;
            }
        }

        private void txbNIF_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (EmpleatSeleccionat != null)
            {
                if (Empleat.validaNif(txbNIF.Text))
                {
                    txbNIF.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    BindingExpression be = txbNIF.GetBindingExpression(TextBox.TextProperty);
                    be.UpdateSource();
                }
                else
                {
                    txbNIF.BorderBrush = new SolidColorBrush(Colors.Red);
                    DesactivarButtonSave();
                }
            }
        }

        private void txbNom_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (EmpleatSeleccionat != null)
            {
                if (Empleat.validaNomICognoms(txbNom.Text))
                {
                    txbNom.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    BindingExpression be = txbNom.GetBindingExpression(TextBox.TextProperty);
                    be.UpdateSource();
                }
                else
                {
                    txbNom.BorderBrush = new SolidColorBrush(Colors.Red);
                    DesactivarButtonSave();
                }
            }
        }

        private void txbCognoms_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (EmpleatSeleccionat != null)
            {
                if (Empleat.validaNomICognoms(txbCognoms.Text))
                {
                    txbCognoms.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    BindingExpression be = txbCognoms.GetBindingExpression(TextBox.TextProperty);
                    be.UpdateSource();
                }
                else
                {
                    txbCognoms.BorderBrush = new SolidColorBrush(Colors.Red);
                    DesactivarButtonSave();
                }
            }
        }

        private void dtpData_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmpleatSeleccionat != null)
            {
                if (Empleat.validaDataEntrada((DateTime)dtpData.SelectedDate))
                {
                    dtpData.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    BindingExpression be = dtpData.GetBindingExpression(DatePicker.SelectedDateProperty);
                    be.UpdateSource();
                }
                else
                {
                    dtpData.BorderBrush = new SolidColorBrush(Colors.Red);
                    DesactivarButtonSave();
                }
            }
        }

        private void DesactivarButtonSave()
        {
            btnSave.IsEnabled = false;
        }

        private void ActivarButtonSave()
        {
            btnSave.IsEnabled = true;
        }
    }
}
