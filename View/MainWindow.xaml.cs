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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kompetenzcheck.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly VehicleController _controller;

        public MainWindow()
        {
            InitializeComponent();
            _controller = new VehicleController();
            LoadVehicles();
            InitializeFilters();
        }

        private async void LoadVehicles()
        {
            try
            {
                var vehicles = await _controller.GetAllVehiclesAsync();
                vehicleDataGrid.ItemsSource = vehicles;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading vehicles: {ex.Message}");
            }
        }

        private void BtnAddVehicle_Click(object sender, RoutedEventArgs e)
        {
            var detailsWindow = new Details(null);
            if (detailsWindow.ShowDialog() == true)
            {
                LoadVehicles(); // Refresh the list
            }
        }

        // For editing existing vehicle
        private void VehicleDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (vehicleDataGrid.SelectedItem is Fahrzeug selectedVehicle)
            {
                var detailsWindow = new Details(selectedVehicle);
                if (detailsWindow.ShowDialog() == true)
                {
                    LoadVehicles(); // Refresh the list
                }
            }
        }

        private async void InitializeFilters()
        {
            try
            {
                var vehicles = await _controller.GetAllVehiclesAsync();
                var categories = vehicles.Select(v => new { v.Kategorie }).Distinct().ToList();
                categoryFilter.ItemsSource = categories;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading filters: {ex.Message}");
            }
        }
        private async void BtnApplyFilters_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var vehicles = await _controller.GetAllVehiclesAsync();

                if (!string.IsNullOrEmpty(categoryFilter.Text))
                    vehicles = await _controller.GetVehiclesByCategoryAsync(categoryFilter.Text);

                if (float.TryParse(priceMinFilter.Text, out float min) &&
                    float.TryParse(priceMaxFilter.Text, out float max))
                    vehicles = await _controller.GetVehiclesByPriceRangeAsync(min, max);

                vehicleDataGrid.ItemsSource = vehicles;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
