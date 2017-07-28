using Geomystery.Controllers.Geometry;
using Geomystery.Models.Geometry;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Award
{
    enum Stars { NoStars=0, OneStar = 1, TwoStar = 2, ThreeStar = 3 };
    /// <summary>
    /// 设置一些章节相关信息与函数
    /// </summary>
    public class Level
    {
        /// <summary>
        /// 关卡编号
        /// </summary>
        public int ID;
        /// <summary>
        /// 关卡名称
        /// </summary>
        public string name;
        /// <summary>
        /// 关卡封面
        /// </summary>
        public string cover;
        /// <summary>
        /// 关卡说明
        /// </summary>
        public string intro;
        /// <summary>
        /// 是否已解锁
        /// </summary>
        public int unlocked { get; set; }
        /// <summary>
        /// 是否已通过
        /// </summary>
        private bool Passed;
        /// <summary>
        /// 获得星星数
        /// </summary>
        private Stars stars;
        /// <summary>
        /// 构造函数
        /// </summary>
        public Level()
        {
            ID = 0;
            name = "";
            cover = "Picture/lock.png";
            unlocked = 0;
            Passed = false;
            stars = Stars.NoStars;
        }
        /// <summary>
        /// 关卡初始化载入
        /// </summary>
        public void LevelStart()
        { }
        /// <summary>
        /// 关卡通过后，修改关卡状态，修改星星数
        /// </summary>
        /// <param name="NewStars"></param>
        private void LevelPass(Stars NewStars)
        {
            if (Passed == false)
            {
                Passed = true;
                Geo_Coin.Geo.GetGeo_Coins(1);
            }
            if (NewStars > stars)
            {
                for (int i = 0; i < (NewStars - stars); i++)
                {
                    Geo_Coin.Geo.GetGeo_Coins(2);
                }
                stars = NewStars;
            }     
        }  
        public static ObservableCollection<Level> getLevels()
        {
            ObservableCollection<Level> cp1 = new ObservableCollection<Level>();
            cp1.Add(new Level() { ID = 1, name = "Draw Line", unlocked = 1 });
            cp1.Add(new Level() { ID = 2, name = "Draw Circle" });
            cp1.Add(new Level() { ID = 3, name = "Draw Point" });
            cp1.Add(new Level() { ID = 4, name = "Angle of 60°" });
            cp1.Add(new Level() { ID = 5, name = "Perpendicular Bisector" });
            cp1.Add(new Level() { ID = 6, name = "Angle Bisector" });
            cp1.Add(new Level() { ID = 7, name = "Perpendicular Line" });
            cp1.Add(new Level() { ID = 8, name = "Ciricl in Diamond" });
            cp1.Add(new Level() { ID = 9, name = "Circle Center" });
            foreach(var x in cp1)
            {
                x.cover = "ms-appx:///Pictures/Levels/" + x.ID.ToString()+".png";
            }
            return cp1;
        }
    }
}

public class LevelLoader
{
    public static Controllers GetLevel(int index)
    {
        Controllers controller = new Controllers(1);

        controller.historyDfaList = new List<DFA>();
        controller.redoDfaList = new List<DFA>();
        //controller.isIniialized = true;

        switch (index)
        {
            case 1:
                {
                    Point2 p1 = new Point2() { X = 66560, Y = -35198 };
                    Point2 p2 = new Point2() { X = 44797, Y = -52078 };
                    Point2 p3 = new Point2() { X = 81920, Y = -54799 };
                    controller.coordinate.AddPoint(p1);
                    controller.coordinate.AddPoint(p2);
                    controller.coordinate.AddPoint(p3);
                    break;
                }
            case 2:
                {
                    Point2 p1 = new Point2() { X = 66560, Y = -35198 };
                    Point2 p2 = new Point2() { X = 44797, Y = -52078 };
                    Point2 p3 = new Point2() { X = 81920, Y = -54799 };
                    controller.coordinate.AddPoint(p1);
                    controller.coordinate.AddPoint(p2);
                    controller.coordinate.AddPoint(p3);
                    break;
                }
            case 3:
                {
                    Point2 p1 = new Point2() { X = 66560, Y = -35198 };
                    Point2 p2 = new Point2() { X = 44797, Y = -52078 };
                    Point2 p3 = new Point2() { X = 81920, Y = -54799 };
                    controller.coordinate.AddPoint(p1);
                    controller.coordinate.AddPoint(p2);
                    controller.coordinate.AddPoint(p3);
                    p1.resultPoint.isVisible = false;
                    p2.resultPoint.isVisible = false;
                    p3.resultPoint.isVisible = false;
                    Line l1 = new Line() { p1 = p1, p2 = p2 };
                    Line l2 = new Line() { p1 = p2, p2 = p3 };
                    Line l3 = new Line() { p1 = p3, p2 = p1 };
                    controller.coordinate.AddLine(l1);
                    controller.coordinate.AddLine(l2);
                    controller.coordinate.AddLine(l3);
                    break;
                }
            case 4:
                {

                    break;
                }
        }
        return controller;
    }
 }