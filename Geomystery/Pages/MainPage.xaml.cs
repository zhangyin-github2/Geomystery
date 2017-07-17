﻿using System;
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


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Geomystery
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static Frame MainFrame=new Frame();
        public static MediaPlayerElement BgmP=new MediaPlayerElement(), BgaP=new MediaPlayerElement();
        bool isMute=false;

        public MainPage()
        {
            this.InitializeComponent();
            init();
            
        }

        public void init()
        {
            MainFrame = this.myFrame;
            BgmP = bgmPlayer;
            BgaP = bgaPlayer;
            isMute = false;
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;    //启动设置全屏
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
        //    isMute = !isMute;
        //    BgmP.MediaPlayer.IsMuted = BgaP.MediaPlayer.IsMuted = isMute;
        //    if (isMute)
        //    {
        //        MuteButton.Content = CONST.mute;
        //    }
        //    else
        //    {
        //        MuteButton.Content = CONST.volume2;
        //    }
        }

    }
}