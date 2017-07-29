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
using Geomystery.Award;


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
            ScreenHeight = Window.Current.Bounds.Height;
            ScreenWidth = Window.Current.Bounds.Width;
           
            backG.Width = ScreenWidth * 2;
            backG.Height = ScreenHeight * 2;
            var x = backG.RenderTransform as CompositeTransform;
            try
            {
                if (APPDATA.app_data.CURRENT_PAGE == AppPage.AchievementPage) x.TranslateX = -ScreenWidth;
                if (APPDATA.app_data.CURRENT_PAGE == AppPage.OptionPage|| APPDATA.app_data.CURRENT_PAGE == AppPage.AboutPage) x.TranslateY = -ScreenHeight;
            }
            catch { return;  }
            backG.RenderTransform = x;
        }

        public async void init()
        {
            MainFrame = this.myFrame;
            debugTxt = this.debugT;
            BgaP = bgaPlayer;
            BgmP = bgmPlayer;
            mytitle.Visibility = Visibility.Collapsed;
            startFrame.Navigate(typeof(SplashScreen));
            init_gamesetting();
            View = new ViewModel.ViewModel();
            if (!APPDATA.app_data.Views.Contains(View))
            {
                APPDATA.app_data.Views.Add(View);
            }
            
            init_music();
            if (APPDATA.app_data.ISMUTE)
            {
                MuteButton.Content = CONST.mute;
            }
            else
            {
                MuteButton.Content = CONST.volume2;
            }

            BackButton.Visibility = Visibility.Collapsed;

            myFrame.Navigate(typeof(HomePage));
            optionFrame.Navigate(typeof(Option));
            achievementFrame.Navigate(typeof(Achievement));

            await SplashScreen.goback();
            mytitle.Visibility = Visibility.Visible;
            startFrame.Visibility = Visibility.Collapsed;
            if (!APPDATA.app_data.ISFULLSCREEN)
                ApplicationView.GetForCurrentView().ExitFullScreenMode();       //退出全屏
        }
        public void init_gamesetting()
        {
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;    //启动设置全屏
            APPDATA.app_data = new APPDATA();
            APPDATA.LOAD();
            APPDATA.app_data.BACKBUTTON = this.BackButton;
            APPDATA.app_data.MAINGRID = backG;
            APPDATA.app_data.Chapters = Chapter.getChapters();
        }
        public void init_music()
        {
            BGMPlayer.getInstance();
            backG.Children.Add(BGMPlayer.MusicPlayer);
            backG.Children.Add(BGMPlayer.BgmPlayer);
            BGMPlayer.MusicPlayer.Visibility = Visibility.Collapsed;
            BGMPlayer.BgmPlayer.Visibility = Visibility.Collapsed;
            BGMPlayer.PlayBgm();
        }
        

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            switch (APPDATA.app_data.CURRENT_PAGE)
            {
                case AppPage.HomePage: return;
                case AppPage.AchievementPage:
                case AppPage.OptionPage: 
                case AppPage.FreeStylePage:
                case AppPage.SelectChapterPage: APPDATA.app_data.MoveTo(AppPage.HomePage); break;

                case AppPage.GamePage: APPDATA.app_data.MoveTo(AppPage.SelectGamePage,SelectGame.localChapter); break;
                case AppPage.SelectGamePage: APPDATA.app_data.MoveTo(AppPage.SelectChapterPage); break;
                case AppPage.AboutPage: optionFrame.Navigate(typeof(Option));APPDATA.app_data.MoveTo(AppPage.OptionPage); break;
            }
            BGMPlayer.PlayButton();
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
            ScreenHeight = Window.Current.Bounds.Height;
            ScreenWidth = Window.Current.Bounds.Width;
            backG.Width = ScreenWidth * 2;
            backG.Height = ScreenHeight * 2;
        }

    }
}
