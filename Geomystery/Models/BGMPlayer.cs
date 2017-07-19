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
        public static MediaElement MusicPlayer;
        public static MediaElement getInstance()
        {
            if (MusicPlayer == null)
            {
                MusicPlayer = new MediaElement();
            }
            return MusicPlayer;
        }
    }
}
