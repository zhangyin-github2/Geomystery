using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Geomystery.Award
{
    public class Achievements
    {
        public string name { get; set; }
        public string islock { get; set; }
        public SolidColorBrush col { get; set; }
        public string picture { get; set; }
        public string discribe { get; set; }
        public int aim { get; set; }
        public int have_done { get; set; }
        public int reward_num { get; set; }

        public Achievements(double ff=44)
        {
            aim = 1;
            have_done = 0;
            col = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));
            islock = "未达成";
        }
        public static ObservableCollection<Achievements> GetAch(double ff = 44)
        {
            var ach = new ObservableCollection<Achievements>();
   
            for(int i=0;i< 6;i++)
            {
                ach.Add(new Achievements());
                ach[i].name = AppResources.GetString("AC" + (i + 1).ToString() + "N");
                ach[i].discribe = AppResources.GetString("AC" +(i+1).ToString()+"D");
                string locked = APPDATA.app_data.ACHIEVEMENT[i] == '0' ? "Lock" : "Unlock";
                if(APPDATA.app_data.ACHIEVEMENT[i] == '1') ach[i].col = new SolidColorBrush(Color.FromArgb(255, 1, 139, 61));
                ach[i].islock = AppResources.GetString(locked);
                ach[i].picture = "ms-appx:///Pictures/Achievement/" + (i + 1).ToString() + ".png";
            }
            return ach;
        }
    }
}
