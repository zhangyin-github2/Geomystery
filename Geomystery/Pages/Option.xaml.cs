using Geomystery.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Geomystery
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Option : Page
    {
        private ViewModel.ViewModel View { set; get; } = new ViewModel.ViewModel();
        public Option()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += Current_SizeChanged;
            init();
        }

        void init()
        {
            View = new ViewModel.ViewModel();
            if (!APPDATA.app_data.Views.Contains(View))
            {
                APPDATA.app_data.Views.Add(View);
            }
            if (APPDATA.app_data.LANGGUAGE != "en-US")  LanguageNow.Text = "简体中文";
            else LanguageNow.Text = "English";
            SFXVolSlider.Value = APPDATA.app_data.SFXVOLUME*SFXVolSlider.Maximum;
            MusicVolSlider.Value = APPDATA.app_data.MUSICVOLUME * MusicVolSlider.Maximum;
            NightMode.IsChecked = APPDATA.app_data.ISNIGHT;
            Fullscreen.IsChecked = APPDATA.app_data.ISFULLSCREEN;
            Fullscreen.Content = APPDATA.app_data.ISFULLSCREEN ? CONST.yes : CONST.no;
            NightMode.Content = APPDATA.app_data.ISNIGHT ? CONST.yes : CONST.no;
        }

        private void NightMode_Click(object sender, RoutedEventArgs e)
        {
            BGMPlayer.PlayButton9();
            APPDATA.app_data.setNight();
            NightMode.Content = APPDATA.app_data.ISNIGHT ? CONST.yes : CONST.no;
        }

        private void Fullscreen_Click(object sender, RoutedEventArgs e)
        {
            BGMPlayer.PlayButton9();
            APPDATA.app_data.setFullScreen();
            Fullscreen.Content = APPDATA.app_data.ISFULLSCREEN ? CONST.yes : CONST.no;
        }

        private void AboutUs_Click(object sender, RoutedEventArgs e)
        {
            BGMPlayer.PlayButton1();
            APPDATA.app_data.MoveTo(AppPage.AboutPage);
            Frame.Navigate(typeof(About));
        }

        private async void ResetProgress_Click(object sender, RoutedEventArgs e)
        {
            BGMPlayer.PlayButton2();
            await APPDATA.app_data.Reset();
            init();
        }
        public void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            ApplicationView cview = ApplicationView.GetForCurrentView();
            APPDATA.app_data.ISFULLSCREEN = cview.IsFullScreenMode;
            Fullscreen.IsChecked = APPDATA.app_data.ISFULLSCREEN;
            Fullscreen.Content = APPDATA.app_data.ISFULLSCREEN ? CONST.yes : CONST.no;
        }

        private void LanRight_Click(object sender, RoutedEventArgs e)
        {
            BGMPlayer.PlayButton9();
            APPDATA.app_data.change_language();
        }

        private void LanLeft_Click(object sender, RoutedEventArgs e)
        {
            BGMPlayer.PlayButton9();
            APPDATA.app_data.change_language();
        }

        private void SFXVolSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var s = sender as Slider;
            var v = s.Value/ s.Maximum;
            APPDATA.app_data.setVolume(v);
        }

        private void MusicVolSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var s = sender as Slider;
            var v = s.Value / s.Maximum;
            APPDATA.app_data.setBgmVolume(v);
        }
    }
}
