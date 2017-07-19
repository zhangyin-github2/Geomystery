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
using Geomystery.Models;
using Geomystery.ViewModel;


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
        private ViewModel.ViewModel View;

        public MainPage()
        {
            this.InitializeComponent();
            BGMPlayer.getInstance();
            BGMPlayer.MusicPlayer.Name = "MusicPlayer";
            Music.Children.Add(BGMPlayer.MusicPlayer);
            BGMPlayer.MusicPlayer.Visibility = Visibility.Collapsed;
            BGMPlayer.MusicPlayer.IsLooping = true;
            BGMPlayer.MusicPlayer.AutoPlay = true;
            BGMPlayer.MusicPlayer.Source = new Uri("ms-appx://Assets/buttonmusic.mp3");
            BGMPlayer.MusicPlayer.Volume = 100;
            Application.Current.Suspending += new SuspendingEventHandler(App_Suspending);
            init();
        }

        public void init()
        {
            MainFrame = this.myFrame;
            BgmP = bgmPlayer;
            BgaP = bgaPlayer;
            APPDATA.app_data = new APPDATA();
            APPDATA.LOAD();
            View = new ViewModel.ViewModel();
            if(!APPDATA.app_data.Views.Contains(View))
            {
                APPDATA.app_data.Views.Add(View);
            }
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;    //启动设置全屏
            if (!APPDATA.app_data.ISFULLSCREEN)
                ApplicationView.GetForCurrentView().ExitFullScreenMode();       //退出全屏
            if (APPDATA.app_data.ISMUTE)
                MuteButton.Content = CONST.mute;
            else
                MuteButton.Content = CONST.volume2;
            debugT.Text = APPDATA.app_data.show();
            myFrame.Navigate(typeof(HomePage));
        }

        

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (myFrame.CanGoBack)
            {
                myFrame.GoBack();
            }
        }

        private void MuteButton_Click(object sender, RoutedEventArgs e)
        {
            APPDATA.app_data.setMute();

            if (APPDATA.app_data.ISMUTE)
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
            
        }
    }
}
