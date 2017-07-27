﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.UI.Xaml.Controls;

namespace Geomystery.Models
{
    class BGMPlayer
    {
        public static MediaPlayerElement MusicPlayer;
        public static MediaPlayerElement BgmPlayer;
        public static void getInstance()
        {
            if (MusicPlayer == null)
            {
                MusicPlayer = new MediaPlayerElement();
                MusicPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Audio/button1.wav", UriKind.Absolute));
                MusicPlayer.MediaPlayer.IsLoopingEnabled = false;
                MusicPlayer.MediaPlayer.AutoPlay = false;
                MusicPlayer.MediaPlayer.Volume = APPDATA.app_data.SFXVOLUME;
                MusicPlayer.MediaPlayer.IsMuted = APPDATA.app_data.ISMUTE;
            }
            if (BgmPlayer == null)
            {
                BgmPlayer = new MediaPlayerElement();
                BgmPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Audio/bgm1.mp3", UriKind.Absolute));
                BgmPlayer.MediaPlayer.IsLoopingEnabled = true;
                BgmPlayer.MediaPlayer.AutoPlay = true;
                BgmPlayer.MediaPlayer.Volume = APPDATA.app_data.MUSICVOLUME;
                BgmPlayer.MediaPlayer.IsMuted = APPDATA.app_data.ISMUTE;
            }
            //return MusicPlayer;
        }
        public static void PlayButton()
        {
            MusicPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Audio/button4.wav", UriKind.Absolute));
            MusicPlayer.MediaPlayer.Play();
        }
        public static void PlayBgm()
        {
            BgmPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Audio/bgm1.mp3", UriKind.Absolute));
            BgmPlayer.MediaPlayer.Play();
        }
        public static void setMute()
        {
            MusicPlayer.MediaPlayer.IsMuted = !BGMPlayer.MusicPlayer.MediaPlayer.IsMuted;
            BgmPlayer.MediaPlayer.IsMuted = !BGMPlayer.BgmPlayer.MediaPlayer.IsMuted;
        }
    }
}
