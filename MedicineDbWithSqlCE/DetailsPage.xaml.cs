using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using MedicineDbWithSqlCE.Model;
using MedicineDbWithSqlCE.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MedicineDbWithSqlCE
{
    public partial class DetailsPage : PhoneApplicationPage
    {
        MedicineDbViewModel _dbViewModel = new MedicineDbViewModel();

        public DetailsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (SelectedItemPreviewGrid.DataContext == null)
            {
                string selectedIndex = "";
                if (NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex))
                {
                    int index = int.Parse(selectedIndex);
                    SelectedItemPreviewGrid.DataContext = _dbViewModel.Medicines[index];
                    LoadRelatedLonglistSelector(_dbViewModel.Medicines[index]);
                }
            }
        }

        private void LoadRelatedLonglistSelector(Medicine medicine)
        {
            this.LongListSelector.ItemsSource = (IList) _dbViewModel.GetRelatedMedicines(medicine);
        }

        private void LongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var medicine = LongListSelector.SelectedItem as Medicine;
            SelectedItemPreviewGrid.DataContext = medicine;

        }
    }
}