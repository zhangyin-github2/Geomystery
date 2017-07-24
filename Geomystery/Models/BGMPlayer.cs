using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Geomystery.Models
{
    class BGMPlayer
    {
        public static MediaPlayerElement MusicPlayer;
        public static MediaPlayerElement getInstance()
        {
            if (MusicPlayer == null)
            {
                MusicPlayer = new MediaPlayerElement();
            }
            return MusicPlayer;
        }
        public static void setMute()
        {
            BGMPlayer.MusicPlayer.MediaPlayer.IsMuted = !BGMPlayer.MusicPlayer.MediaPlayer.IsMuted;
        }
    }
}
