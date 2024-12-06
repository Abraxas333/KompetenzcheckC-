using Kompetenzcheck.Controller;
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

namespace Kompetenzcheck.View
{
    /// <summary>
    /// Interaction logic for Details.xaml
    /// </summary>
    public partial class Details : Window
    {
        private Fahrzeug _vehicle;
        private readonly VehicleController _controller;

        public Details(Fahrzeug vehicle = null)
        {
            InitializeComponent();
            _controller = new VehicleController();
            _vehicle = vehicle ?? new Fahrzeug();
            LoadVehicleData();
        }

        private void LoadVehicleData()
        {
            txtBezeichnung.Text = _vehicle.Bezeichnung;
            txtBaujahr.Text = _vehicle.Baujahr.ToString();
            txtPreis.Text = _vehicle.Preis.ToString();
            txtKategorie.Text = _vehicle.Kategorie;
            txtHerstellerID.Text = _vehicle.HerstellerID.ToString();
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _vehicle.Bezeichnung = txtBezeichnung.Text;
                _vehicle.Baujahr = int.Parse(txtBaujahr.Text);
                _vehicle.Preis = float.Parse(txtPreis.Text);
                _vehicle.Kategorie = txtKategorie.Text;
                _vehicle.HerstellerID = int.Parse(txtHerstellerID.Text);

                await _controller.SaveVehicleAsync(_vehicle);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving vehicle: {ex.Message}");
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
