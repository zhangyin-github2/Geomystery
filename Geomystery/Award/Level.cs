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
        /// 构造函数
        /// </summary>
        public Level()
        {
            ID = 0;
            name = "";
            cover = "Picture/lock.png";
            unlocked = 0;
            Passed = false;
        }
        public LevelLoader LL;
        /// 关卡通过后，修改关卡状态，修改星星数
        /// </summary>
        /// <param name="NewStars"></param>
        private void LevelPass( )
        {
            if (Passed == false)
            {
                Passed = true;
                Geo_Coin.Geo.GetGeo_Coins(1);
            }
                 
        }  
        public static ObservableCollection<Level> getLevels()
        {
            ObservableCollection<Level> cp1 = new ObservableCollection<Level>();
            cp1.Add(new Level()
            {
                ID = 1,
                name = "Draw Line",
                unlocked = 1,
                intro = "Today you need to take the first step in geometric learning,why not get started from the most simple content.Try to draw three lines to connect these three points .Line is one of the most useful tools you will use to solve problems ,you must comprehend it.",
            });

            cp1.Add(new Level()
            {
                ID = 2,
                name = "Draw Circle",
                intro = "Lines can't help you with solving all of the question when you learn geometry,so why not try to use circles ? Try to draw two concentric circles through the other two points with a given point as the center.This question is not too difficult ,is it?"
            });

            cp1.Add(new Level()
            {
                ID = 3,
                name = "Draw Point",
                intro= "It seems that you have learned to use straight lines and rounds, which is really gratifying.But sometimes you need some points to help you.Try to mark the intersection of the given three lines,and this step will be used repeatedly."
            });

            cp1.Add(new Level()
            {
                ID = 4,
                name = "Angle of 60°",
                intro= "Have you already learnt the three basic tools?It seems that you are ready to accept the first test. Try to draw a 60°angle on the counterclockwise based on the given ray, I think it's not hard for you."

            });

            cp1.Add(new Level() { ID = 5, name = "Perpendicular Bisector",
                intro = "It is a long way to lern geometry ,don't be complacent. You need to learn how to use more tools which is more complex.Try to draw the perpendicular bisector of this line. What's more ,mastered perpendicular bisectors is obligatory course if you want to learn to solve problems."
            });

            cp1.Add(new Level() { ID = 6, name = "Angle Bisector",
                intro = "Do you remember the 60 degree angle you have drawn? The line isn't the only object we have to operate, we need to learn to operate angle.Try to draw the bisector of the given angle. You will learn more about the relationship between the lines and the angles."
            });

            cp1.Add(new Level() { ID = 7, name = "Perpendicular Line",
                intro = "Have you already mastered perpendicular bisectors and angle bisectors ?I think you are ready to continue your learning in using composite tools. Try to draw the perpendicular line of the given line through the given point. Can you see the nature of this problem?"
            });

            cp1.Add(new Level() { ID = 8, name = "Circle in Diamond",
                intro = "Finished learning to use composite tool , maybe you are eager to do a quiz. Are you ready to do it?  Try to draw the inscribed circle of the given diamond. Don't forget to use the new tools you have lerant to help you."
            });

            cp1.Add(new Level() { ID = 9, name = "Circle Center",
                intro = "I think these simple problems can not stumped you, you need a final quiz before starting your test. Try to draw the center of the given circle. This question is not as simple as it looks."
            });

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

        List<string> geoList = new List<string>();
        List<string> conList = new List<string>();
        switch (index)
        {
            case 1:
                {
                    //List<string> geoList = new List<string>();
                    geoList.Add("p,1,n,true,66560,-35198");
                    geoList.Add("p,2,n,true,44797,-52078");
                    geoList.Add("p,3,n,true,81920,-54799");

                    //List<string> conList = new List<string>();
                    conList.Add("f,true,1");
                    conList.Add("f,true,2");
                    conList.Add("f,true,3");
                    conList.Add("drawline,false,4,1,2");
                    conList.Add("dl,false,5,2,3");
                    conList.Add("dl,false,6,1,3");

                    
                    /*
                    Point2 p1 = new Point2() { X = 66560, Y = -35198 };
                    Point2 p2 = new Point2() { X = 44797, Y = -52078 };
                    Point2 p3 = new Point2() { X = 81920, Y = -54799 };
                    controller.coordinate.AddPoint(p1);
                    controller.coordinate.AddPoint(p2);
                    controller.coordinate.AddPoint(p3);
                    */
                    break;
                }
            case 2:
                {
                    geoList.Add("p,1,n,true,66560,-35198");
                    geoList.Add("p,2,n,true,44797,-52078");

                    conList.Add("f,true,1");
                    conList.Add("f,true,2");
                    conList.Add("dc,false,3,1,2");

                    break;
                }
            case 3:
                {
                    /*
                    geoList.Add("p,1,n,true,66560,-35198");
                    geoList.Add("p,2,n,true,44797,-52078");
                    geoList.Add("p,3,n,true,81920,-54799");

                    conList.Add("f,true,1");
                    conList.Add("f,true,2");
                    conList.Add("f,true,3");
                    conList.Add("drawline,false,4,1,2");
                    conList.Add("dl,false,5,2,3");
                    conList.Add("dl,false,6,1,3");
                    */

                    break;
                }
            case 4:
                {
                    geoList.Add("p,1,n,true,66560,-35198");
                    geoList.Add("p,2,n,true,44797,-52078");
                    geoList.Add("l,3,n,true,1,2");

                    conList.Add("f,true,1");
                    conList.Add("f,true,2");
                    conList.Add("dl,true,3,1,2");
                    conList.Add("o,false,4,3");
                    conList.Add("dc,false,5,1,3");
                    conList.Add("dc,false,6,3,1");
                    conList.Add("i,false,7,5,6,1");
                    conList.Add("dl,false,8,7,1");
                    break;
                }
        }
        for (int i = 0; i < geoList.Count; i++)
        {
            controller.AddGeometryFromString(geoList[i]);
        }
        //复制一下代表条件列表缓存，也可以不需要模板与副本，每次关卡都要从头加载条件列表
        controller.conditionLists = new List<ConditionsList>();                             //需要达成条件
        controller.meetingconditionLists = new List<ConditionsList>();                      //复制自“需要达成条件”的正在达成条件
        controller.conditionLists.Add(new ConditionsList());
        for (int i = 0; i < conList.Count; i++)
        {
            controller.AddConditionFromString(conList[i]);
        }
        controller.meetingconditionLists.Add(controller.conditionLists[0].Copy());
        controller.coordinate.GeometryCount = 1000;

        return controller;
    }
 }