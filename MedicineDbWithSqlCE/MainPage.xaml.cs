using System;
using System.Windows.Controls;
using MedicineDbWithSqlCE.Model;
using Microsoft.Phone.Controls;

namespace MedicineDbWithSqlCE
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
        private void LongListSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LongListSelector.SelectedItem == null)
                return;

            // Navigate to the new page
            var medicine = LongListSelector.SelectedItem as Medicine;
            if (medicine != null)
                NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + (medicine.MedicineId - 1), UriKind.Relative));

            // Reset selected item to null (no selection)
            LongListSelector.SelectedItem = null;
        }
    }
}