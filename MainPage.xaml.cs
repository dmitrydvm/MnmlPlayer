using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using MauiIcons.Core;
using MauiIcons.SegoeFluent;
using System.Collections.ObjectModel;

namespace MnmlPlayer
{
    public partial class MainPage : ContentPage
    {
        private readonly string[] fileTypes = [".mp3", ".flac"];
        private readonly ObservableCollection<FileResult> _tracks = new();
        public MainPage()
        {
            InitializeComponent();



            Playlist.ItemsSource = _tracks;
        }



        private async void Add_Clicked(object sender, EventArgs e)
        {
            var options = new PickOptions()
            {
                PickerTitle = "Please select a music",
                FileTypes = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, fileTypes }, // file extension
                    { DevicePlatform.macOS, fileTypes }, // UTType values
                })
            };

            var result = (await FilePicker.PickMultipleAsync(options)).ToList();
            foreach (var file in result)
            {
                _tracks.Add(file);
            }

            Player.Source = MediaSource.FromFile(result[0].FullPath);

            return;
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            _tracks.Clear();
        }


        private void Play_Clicked(object sender, EventArgs e)
        {
            if (Player.CurrentState == MediaElementState.Stopped || Player.CurrentState == MediaElementState.Paused)
            {
                Player.Play();
                Play.Icon(SegoeFluentIcons.Pause);
            }
            if (Player.CurrentState == MediaElementState.Playing)
            {
                Player.Pause();
                Play.Icon(SegoeFluentIcons.Play);
            }

        }

        private void Stop_Clicked(object sender, EventArgs e)
        {
            Player.Stop();
        }

        private void Player_PositionChanged(object sender, MediaPositionChangedEventArgs e)
        {
            Seeker.Value = e.Position.TotalSeconds;
        }

        private void Seeker_DragCompleted(object sender, EventArgs e)
        {
            var seconds = ((Slider)sender).Value;

            Player.SeekTo(TimeSpan.FromSeconds(seconds));
            Player.Play();
        }

        private void Seeker_DragStarted(object sender, EventArgs e)
        {
            Player.Pause();
        }
    }

}
