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
using Windows.Media.Core;


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
        public static TextBlock debugTxt = new TextBlock();
        public static Frame MainFrame=new Frame();
        public static MediaPlayerElement BgmP = new MediaPlayerElement();
        public static MediaPlayerElement BgaP =new MediaPlayerElement();
        private ViewModel.ViewModel View;

        public MainPage()
        {
            this.InitializeComponent();
            this.Name = this.GetType().ToString()+"P";
            myFrame.Navigated += MyFrame_Navigated;
            Application.Current.Suspending += new SuspendingEventHandler(App_Suspending);
            Window.Current.SizeChanged += Current_SizeChanged;
            init();
            debugT.Text = this.Name;
        }

        public void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            ApplicationView cview = ApplicationView.GetForCurrentView();
            APPDATA.app_data.ISFULLSCREEN = cview.IsFullScreenMode;
            APPDATA.app_data.MAINGRID = backG;
            ScreenHeight = Window.Current.Bounds.Height;
            ScreenWidth = Window.Current.Bounds.Width;
            backG.Width = ScreenWidth * 2;
            backG.Height = ScreenHeight * 2;
        }

        public void init()
        {
            MainFrame = this.myFrame;
            debugTxt = this.debugT;
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
            APPDATA.app_data.BACKBUTTON = this.BackButton;
            //debugT.Text = APPDATA.app_data.show();
            BackButton.Visibility = Visibility.Collapsed;
            myFrame.Navigate(typeof(HomePage));
            optionFrame.Navigate(typeof(Option));
            achievementFrame.Navigate(typeof(Achievement));
            init_music();
        }
        public void init_music()
        {
            BGMPlayer.getInstance();
            BGMPlayer.MusicPlayer.Name = "MusicPlayer";
            backG.Children.Add(BGMPlayer.MusicPlayer);
            BGMPlayer.MusicPlayer.Visibility = Visibility.Collapsed;
            BGMPlayer.MusicPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/button1.wav"));
            BGMPlayer.MusicPlayer.MediaPlayer.IsLoopingEnabled = false;
            BGMPlayer.MusicPlayer.MediaPlayer.AutoPlay = false;
            BGMPlayer.MusicPlayer.MediaPlayer.Volume = 100;
        }
        

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (myFrame.CanGoBack)
            {
                myFrame.GoBack();
            }
            if(APPDATA.app_data.CURRENT_PAGE!=AppPage.HomePage)
            {
                APPDATA.app_data.MoveTo(AppPage.HomePage);
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
            backG.Width = ScreenWidth * 2;
            backG.Height = ScreenHeight * 2;
        }

        private void MyFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (!myFrame.CanGoBack)
            {
                BackButton.Visibility = Visibility.Collapsed;
            }
            else BackButton.Visibility = Visibility.Visible;
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
