using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroceryList.Model;
using GroceryList.Service;
using Xamarin.Forms;

namespace GroceryList.Page
{
    public partial class GroceryList : ContentPage
    {
        private readonly GroceryService service;
        public GroceryList()
        {
            service = new GroceryService();
            InitializeComponent();
            LoadGroceryList();
        }

        private async void LoadGroceryList()
        {
            GroceryView.ItemsSource = await service.RefreshDataAsync();
        }

        private async void Save_OnClicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Item.Text))
            {
                await DisplayAlert("Required", "Please add an item", "Ok");
                return;
            }

            GroceryItems groceryItem = new GroceryItems();
            groceryItem.GroceryItem = Item.Text;

            await service.SaveGroceryItemAsync(groceryItem);
            LoadGroceryList();
            Item.Text = string.Empty;
            Item.Unfocus();
        }

        private async void MenuItem_OnClicked(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;
            var item = (GroceryItems)mi.CommandParameter;
            DeleteItem(item.Id);
        }

        async Task DeleteItem(int id)
        {
            service.DeleteGroceryItemAsync(id);
            LoadGroceryList();
        }

        private async void GroceryView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (GroceryItems)e.SelectedItem;

            if (Device.OS != TargetPlatform.iOS && item != null)
            {
                if (Device.OS == TargetPlatform.Android)
                {
                    await DisplayAlert("Delete", "Pres long to delete", "Ok");
                }
                else
                {
                    if (await DisplayAlert("Delete", "Delete item " + item.GroceryItem, "Delete", "Cancel"))
                    {
                        await DeleteItem(item.Id);
                    }
                }
            }

            GroceryView.SelectedItem = null;

        }
    }
}
