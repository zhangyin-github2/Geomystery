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
        public double fontsize1 { get; set; }
        public double fontsize2 { get; set; }
        public double fontsize3 { get; set; }

        public Achievements()
        {
            aim = 1;
            have_done = 0;
            islock = "未达成";
        }
        public static ObservableCollection<Achievements> GetAch(double ff = 44)
        {
            var ach = new ObservableCollection<Achievements>();

            ach.Add(new Achievements { name = "第一首歌", picture = "Picture/第一首歌.png", islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 2 * ff, fontsize2 = ff, fontsize3 = 1.2 * ff });
            ach.Add(new Achievements { name = "精确打击", picture = "Picture/精确打击.png", islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 2 * ff, fontsize2 = ff, fontsize3 = 1.2 * ff });
            ach.Add(new Achievements { name = "连击精英", picture = "Picture/连击精英.png", islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 2 * ff, fontsize2 = ff, fontsize3 = 1.2 * ff });
            ach.Add(new Achievements { name = "节奏大师", picture = "Picture/节奏大师.png", islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 2 * ff, fontsize2 = ff, fontsize3 = 1.2 * ff });
            ach.Add(new Achievements { name = "完美一战", picture = "Picture/完美一战.png", islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 2 * ff, fontsize2 = ff, fontsize3 = 1.2 * ff });
            ach.Add(new Achievements { name = "收藏家", picture = "Picture/收藏家.png", islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 2 * ff, fontsize2 = ff, fontsize3 = 1.2 * ff });
            ach.Add(new Achievements { name = "土豪", picture = "Picture/土豪.png", islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 2 * ff, fontsize2 = ff, fontsize3 = 1.2 * ff });
            ach.Add(new Achievements { name = "节奏之王", picture = "Picture/节奏之王.png", islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 2 * ff, fontsize2 = ff, fontsize3 = 1.2 * ff });

            ach[0].discribe = "完成你的第一首歌。";
            ach[1].discribe = "在一局游戏中没有Miss评分。";
            ach[2].discribe = "在一局游戏中完成所有连击。";
            ach[3].discribe = "在一局游戏中获得全部Perfect评分。";
            ach[4].discribe = "在一局游戏中得到满分。";
            ach[5].discribe = "解锁所有歌曲。";
            ach[6].discribe = "金币数量超过10000。";
            ach[7].discribe = "在所有歌曲中达成S以上评分。";

            return ach;
        }
    }
}
