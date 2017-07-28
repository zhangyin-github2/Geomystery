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

            ach.Add(new Achievements { name = "旗开得胜", picture = "Picture/旗开得胜.png", aim=1,reward_num=200, islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 2 * ff, fontsize2 = ff, fontsize3 = 1.2 * ff });
            ach.Add(new Achievements { name = "细心的人", picture = "Picture/细心的人.png", aim = 1, reward_num = 300, islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 2 * ff, fontsize2 = ff, fontsize3 = 1.2 * ff });
            ach.Add(new Achievements { name = "事不过三", picture = "Picture/事不过三.png", aim = 1, reward_num = 300, islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 2 * ff, fontsize2 = ff, fontsize3 = 1.2 * ff });
            ach.Add(new Achievements { name = "数学家", picture = "Picture/数学家.png", aim = 1, reward_num = 300, islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 2 * ff, fontsize2 = ff, fontsize3 = 1.2 * ff });
            ach.Add(new Achievements { name = "完美过关", picture = "Picture/完美过关.png", aim = 1, reward_num = 300, islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 2 * ff, fontsize2 = ff, fontsize3 = 1.2 * ff });
            ach.Add(new Achievements { name = "好事多磨", picture = "Picture/好事多磨.png", aim = 1, reward_num = 200, islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 2 * ff, fontsize2 = ff, fontsize3 = 1.2 * ff });
            ach.Add(new Achievements { name = "圆圆不断", picture = "Picture/圆圆不断.png", aim = 1, reward_num = 400, islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 2 * ff, fontsize2 = ff, fontsize3 = 1.2 * ff });
            ach.Add(new Achievements { name = "穿针引线", picture = "Picture/穿针引线.png", aim = 1, reward_num = 400, islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 2 * ff, fontsize2 = ff, fontsize3 = 1.2 * ff });
            ach.Add(new Achievements { name = "土豪", picture = "Picture/土豪.png", aim = 1, reward_num = 500, islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 2 * ff, fontsize2 = ff, fontsize3 = 1.2 * ff });
            ach.Add(new Achievements { name = " 解题狂人", picture = "Picture/ 解题狂人.png", aim = 1, reward_num = 1000, islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 2 * ff, fontsize2 = ff, fontsize3 = 1.2 * ff });
          

            ach[0].discribe = "完成了你的第一关，开启了你的解题之旅。";
            ach[1].discribe = "第3关中，画出了所有可能的答案。";
            ach[2].discribe = "第5关中使用圆工具次数少于三次直线工具次数少于三次。";
            ach[3].discribe = "第10关中，画出了所有可能的答案。";
            ach[4].discribe = "第12关中使用圆工具次数少于三次直线工具次数少于四次。";
            ach[5].discribe = "在某一关卡中尝试了10次仍未解锁。";
            ach[6].discribe = "累计使用圆工具50次。";
            ach[7].discribe = "累计使用线工具50次。";
            ach[8].discribe = "金币数量超过1000。";
            ach[9].discribe = "解锁全部关卡。";
            


            return ach;
        }
    }
}
