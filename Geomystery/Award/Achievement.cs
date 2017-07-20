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

            ach.Add(new Achievements { name = "成就新人", picture = "Picture/成就新人.png",aim=1,reward_num=100, islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 0.8 * ff, fontsize2 = ff, fontsize3 = 0.6 * ff });
            ach.Add(new Achievements { name = "成就高手", picture = "Picture/成就高手.png", aim = 1, reward_num = 200, islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 0.8 * ff, fontsize2 = ff, fontsize3 = 0.6 * ff });
            ach.Add(new Achievements { name = "成就党", picture = "Picture/成就党.png", aim = 1, reward_num = 300, islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 0.8 * ff, fontsize2 = ff, fontsize3 = 0.6 * ff });
            ach.Add(new Achievements { name = "圆圆不断", picture = "Picture/圆圆不断.png", aim = 1, reward_num = 300, islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 0.8 * ff, fontsize2 = ff, fontsize3 = 0.6 * ff });
            ach.Add(new Achievements { name = "大杀特杀", picture = "Picture/大杀特杀.png", aim = 1, reward_num = 100, islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 0.8 * ff, fontsize2 = ff, fontsize3 = 0.6 * ff });
            ach.Add(new Achievements { name = "无人能挡", picture = "Picture/无人能挡.png", aim = 1, reward_num = 200, islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 0.8 * ff, fontsize2 = ff, fontsize3 = 0.6 * ff });
            ach.Add(new Achievements { name = "接近神了", picture = "Picture/接近神了.png", aim = 1, reward_num = 300, islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 0.8 * ff, fontsize2 = ff, fontsize3 = 0.6 * ff });
            ach.Add(new Achievements { name = "超越神了", picture = "Picture/超越神了.png", aim = 1, reward_num = 400, islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 0.8 * ff, fontsize2 = ff, fontsize3 = 0.6 * ff });
            ach.Add(new Achievements { name = "人生赢家", picture = "Picture/人生赢家.png", aim = 1, reward_num = 500, islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 0.8 * ff, fontsize2 = ff, fontsize3 = 0.6 * ff });
            ach.Add(new Achievements { name = " 芽儿呦！牛的一匹", picture = "Picture/ 芽儿呦！牛的一匹.png", aim = 1, reward_num = 1000, islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 0.8 * ff, fontsize2 = ff, fontsize3 = 0.6 * ff });
            ach.Add(new Achievements { name = "怕是你翻水水喽", picture = "Picture/怕是你翻水水喽.png", aim = 1, reward_num = 300, islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 0.8 * ff, fontsize2 = ff, fontsize3 = 0.6 * ff });
            ach.Add(new Achievements { name = "666", picture = "Picture/666.png", aim = 1, reward_num = 300, islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 0.8 * ff, fontsize2 = ff, fontsize3 = 0.6 * ff });
            ach.Add(new Achievements { name = "好2好2", picture = "Picture/好2好2.png", aim = 1, reward_num = 300, islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 0.8 * ff, fontsize2 = ff, fontsize3 = 0.6 * ff });
            ach.Add(new Achievements { name = "小狒狒", picture = "Picture/小狒狒.png", aim = 1, reward_num = 100, islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 0.8 * ff, fontsize2 = ff, fontsize3 = 0.6 * ff });
            ach.Add(new Achievements { name = "大猩猩", picture = "Picture/大猩猩.png", aim = 1, reward_num = 200, islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 0.8 * ff, fontsize2 = ff, fontsize3 = 0.6 * ff });
            ach.Add(new Achievements { name = "金刚", picture = "Picture/金刚.png", aim = 1, reward_num = 300, islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 0.8 * ff, fontsize2 = ff, fontsize3 = 0.6 * ff });
            ach.Add(new Achievements { name = "那你不是好棒棒", picture = "Picture/那你不是好棒棒.png", aim = 1, reward_num = 500, islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 0.8 * ff, fontsize2 = ff, fontsize3 = 0.6 * ff });
            ach.Add(new Achievements { name = "征服之刃", picture = "Picture/征服之刃.png", aim = 1, reward_num = 200, islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 0.8 * ff, fontsize2 = ff, fontsize3 = 0.6 * ff });
            ach.Add(new Achievements { name = "征服宗师", picture = "Picture/征服宗师.png", aim = 1, reward_num = 400, islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 0.8 * ff, fontsize2 = ff, fontsize3 = 0.6 * ff });
            ach.Add(new Achievements { name = "战神", picture = "Picture/战神.png", aim = 1, reward_num = 00, islock = "未达成", col = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)), fontsize1 = 0.8 * ff, fontsize2 = ff, fontsize3 = 0.6 * ff });


            ach[0].discribe = "您已完成5个成就的解锁。";
            ach[1].discribe = "您已完成10个成就的解锁。";
            ach[2].discribe = "您已完成15个成就的解锁。";
            ach[3].discribe = "您已累计使用圆工具200次。";
            ach[4].discribe = "您已成功通过5个关卡。";
            ach[5].discribe = "您已成功通过10个关卡。";
            ach[6].discribe = "您已成功通过15个关卡。";
            ach[7].discribe = "您已成功通过20个关卡。";
            ach[8].discribe = "您已积攒10000金币。";
            ach[9].discribe = "您已所有关卡全部三星过关。";
            ach[10].discribe = "您已在某一关尝试了50次仍未过关。";
            ach[11].discribe = "您已经完成了第18关。";
            ach[12].discribe = "您已经完成了第22关。";
            ach[13].discribe = "您已累计获得20颗星星。";
            ach[14].discribe = "您已累计获得40颗星星。";
            ach[15].discribe = "您已累计获得60颗星星。";
            ach[16].discribe = "您已通过全部关卡。";
            ach[17].discribe = "您已累计在5小时内通过chapter1。";
            ach[18].discribe = "您已累计在5小时内通过chapter2。";
            ach[19].discribe = "您已累计在5小时内通过chapter3。";



            return ach;
        }
    }
}
