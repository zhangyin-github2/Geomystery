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
using Windows.UI.ViewManagement;
using Geomystery.Pages;
using Geomystery.Assets.music;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Geomystery
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        double ScreenHeight;
        double ScreenWidth;
        public static Frame MainFrame=new Frame();
        public static MediaPlayerElement BgmP = new MediaPlayerElement();
        public static MediaPlayerElement BgaP =new MediaPlayerElement();

        public MainPage()
        {
            this.InitializeComponent();
            Application.Current.Suspending += new SuspendingEventHandler(App_Suspending);
            init();
        }

        public void init()
        {
            MainFrame = this.myFrame;
            BgmP = bgmPlayer;
            BgaP = bgaPlayer;
            View = new ViewModel.ViewModel();   //未找到刷新解决办法
            APPDATA.LOAD();
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;    //启动设置全屏
            myFrame.Navigate(typeof(HomePage));
            MuteButton.Content = CONST.volume2;
        }

        private ViewModel.ViewModel View { set; get; } = new ViewModel.ViewModel();

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (myFrame.CanGoBack)
            {
                myFrame.GoBack();
            }
        }

        private void MuteButton_Click(object sender, RoutedEventArgs e)
        {
            APPDATA.ISMUTE = !APPDATA.ISMUTE;
            //BgmP.MediaPlayer.IsMuted = BgaP.MediaPlayer.IsMuted = isMute;

            if (APPDATA.ISMUTE)
            {
                MuteButton.Content = CONST.mute;
            }
            else
            {
                MuteButton.Content = CONST.volume2;
            }
        }

        /// <summary>
        /// 获取窗口实际长宽
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ScreenHeight = Window.Current.Bounds.Height;
            ScreenWidth = Window.Current.Bounds.Width;

        }

        /// <summary>
        /// 在程序关闭时保存一些内容
        /// </summary>
        async void App_Suspending(Object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            // TODO: This is the time to save app data in case the process is terminated
            APPDATA.SAVE();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            BgmPlayer.getInstance();
            BgmPlayer.MusicPlayer.Name = "MusicPlayer";
            Music.Children.Add(BgmPlayer.MusicPlayer);
            BgmPlayer.MusicPlayer.Visibility = Visibility.Collapsed;
            BgmPlayer.MusicPlayer.IsLooping = true;
            BgmPlayer.MusicPlayer.AutoPlay = true;
            BgmPlayer.MusicPlayer.Source = new Uri("ms-appx:///Assets/bgm.mp3");
            BgmPlayer.MusicPlayer.Play();
            BgmPlayer.MusicPlayer.Volume = 1;
        }
    }
}
