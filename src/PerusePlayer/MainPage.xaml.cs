using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage.Pickers;
using Windows.Media.Core;
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using Windows.Data.Json;
using Windows.Storage;
using System.Xml.Serialization;
using System.Text;
// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace PerusePlayer
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<Memo> _memoData = new ObservableCollection<Memo>();

        public MainPage()
        {
            this.InitializeComponent();
            _memoList.ItemsSource = _memoData;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            OpenVideo(null, null);
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var time = _mediaPlayerElement.MediaPlayer.PlaybackSession.Position;
            if (_memoData.Any(m => m.Time == time))
            {
                var dialog = new MessageDialog("すでにこの時間は登録済みです", "警告");
                await dialog.ShowAsync();
                return;
            };
            _memoData.Add(new Memo(time));
            var tmp = new ObservableCollection<Memo>();
            foreach (var memo in _memoData.OrderBy(m => m.Time))
            {
                tmp.Add(memo);
            }
            _memoData = tmp;
            _memoList.ItemsSource = _memoData;
        }

        private void MemoList_ItemClick(object sender, ItemClickEventArgs e)
        {
            _mediaPlayerElement.MediaPlayer.PlaybackSession.Position = ((Memo)e.ClickedItem).Time;
        }

        private async void SpeedMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem item && double.TryParse(item.Text, out var speed))
            {
                _mediaPlayerElement.MediaPlayer.PlaybackSession.PlaybackRate = speed;
            }
            else
            {
                var dialog = new MessageDialog("スピード調整できなかった");
                await dialog.ShowAsync();
            }
        }

        private async void OpenVideo(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.VideosLibrary
            };
            picker.FileTypeFilter.Add(".mp4");
            picker.FileTypeFilter.Add("*");
            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                _memoData = new ObservableCollection<Memo>();
                _memoList.ItemsSource = _memoData;
                _mediaPlayerElement.Source = MediaSource.CreateFromStorageFile(file);
                _mediaPlayerElement.MediaPlayer.Play();
            }
        }
    }
}
