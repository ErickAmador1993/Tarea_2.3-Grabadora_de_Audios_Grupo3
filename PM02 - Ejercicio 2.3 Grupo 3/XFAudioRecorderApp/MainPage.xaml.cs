using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.AudioRecorder;
using Xamarin.Essentials;
using MediaManager;
using System.IO;
using XFAudioRecorderApp.Models;
using XFAudioRecorderApp.Controllers;
using XFAudioRecorderApp.View;

namespace XFAudioRecorderApp
{
    public partial class MainPage : ContentPage
    {
        public string AudioPath, fileName;

        private readonly AudioRecorderService audioRecorderService = new AudioRecorderService
        {
            StopRecordingOnSilence = true, //will stop recording after 2 seconds (default)
            StopRecordingAfterTimeout = true,  //stop recording after a max timeout (defined below)
            TotalAudioTimeout = TimeSpan.FromSeconds(180) //audio will stop recording after 3 minutes
        };
        
        private readonly AudioPlayer audioPlayer = new AudioPlayer();
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var status = await Permissions.RequestAsync<Permissions.Microphone>();

            if (status != PermissionStatus.Granted)
                return;

            if (audioRecorderService.IsRecording)
            {
                await audioRecorderService.StopRecording();
                //audioPlayer.Play(audioRecorderService.GetAudioFilePath());
                //await DisplayAlert("Alerta", audioRecorderService.TotalAudioTimeout.ToString(), "cancel");
                getRecord();
            }
            else
            {
                await audioRecorderService.StartRecording();
            }
            
        }

        private async void getRecord()
        {

            //var audioFile = await recorder;
            if (audioRecorderService.FilePath != null) //non-null audioFile indicates audio was successfully recorded
            {
                //do something with the file
                //var path = audioRecorderService.FilePath;
                //await CrossMediaManager.Current.Play("file://" + path);

                var stream = audioRecorderService.GetAudioFileStream();

                //string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DateTime.Now.ToString("dd_MM_yyyy_mm_ss") + "_sample.wav");
                fileName = Path.Combine(FileSystem.CacheDirectory, DateTime.Now.ToString("ddMMyyyymmss") + "_VoiceNote.wav");

                using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    stream.CopyTo(fileStream);
                }

                await DisplayAlert("Alerta", fileName, "cancel");

                AudioPath = fileName;

            }

        }

        private async void saveAudio_Clicked(object sender, EventArgs e)
        {
            var audios = new Models.AudioModel
            {
                AudioUrl = AudioPath,
                AudioDescripcion = txtDescripcion.Text
            };

            var resultado = await App.BaseDatos.GrabarAudio(audios);

            if (resultado == 1)
            {
                await DisplayAlert("", "El audio " + fileName + " guardado en la ruta " + AudioPath, "ok");
                txtDescripcion.Text = "";
            }
            else
            {
                await DisplayAlert("Error", "No se pudo Guardar", "ok");
            }
        }

        private async void btnShowAudioList_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageAudioListView());
        }

        private void btnSalir_Clicked(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
        }
    }
}
