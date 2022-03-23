using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XFAudioRecorderApp.Controllers;
using XFAudioRecorderApp.Models;
using Plugin.AudioRecorder;

namespace XFAudioRecorderApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageAudioListView : ContentPage
    {

        private readonly AudioPlayer audioPlayer = new AudioPlayer();

        public PageAudioListView()
        {
            InitializeComponent();
            loadList();
        }

        private async void loadList()
        {
            var lista = await App.BaseDatos.ObtenerListaAudios();
            listViewAudios.ItemsSource = lista;
        }

        private async void listViewAudios_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //await DisplayAlert("", "", "");
        }

        private async void listViewAudios_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            listViewAudios.SelectedItem = null;
            try
            {
                Models.AudioModel item = (Models.AudioModel)e.Item;

                var uri = item.AudioUrl;
                audioPlayer.Play(uri);

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message.ToString(), "Ok");
            }
        }

        private async void SwipeItem_Invoked(object sender, EventArgs e)
        {
            SwipeItem item = sender as SwipeItem;
            AudioModel model = item.BindingContext as AudioModel;
            //await Navigation.PushAsync(new RecordVideoPage(model));
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}