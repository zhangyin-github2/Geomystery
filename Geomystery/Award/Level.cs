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
        public string Discribe;
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
        public Level(int id=0)
        {
            ID = id;
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
        public static ObservableCollection<Level> getLevels(int start =1)
        {
            ObservableCollection<Level> levels = new ObservableCollection<Level>();
            for(int i=start;i<start+9;i++)
            {
                Level x = new Level(i);
                x.name = AppResources.GetString("L" + x.ID.ToString() + "N") ;
                x.Discribe = AppResources.GetString("L" + x.ID.ToString() + "D") ;
                x.cover = "ms-appx:///Pictures/Levels/" + x.ID.ToString()+".png";
                if (APPDATA.app_data.HAVEDONE >= x.ID - 1) x.unlocked = 1;
                levels.Add(x);
            }
            return levels;
        }
    }
}

public class LevelLoader
{
    public static LevelLoader LL;
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
            //画线
            case 1:
                {
                    //List<string> geoList = new List<string>();
                    geoList.Add("p,1,n,true,-5,5");
                    geoList.Add("p,2,n,true,-10,-10");
                    geoList.Add("p,3,n,true,10,-10");

                    //List<string> conList = new List<string>();
                    conList.Add("f,true,1");
                    conList.Add("f,true,2");
                    conList.Add("f,true,3");
                    conList.Add("drawline,false,4,1,2");
                    conList.Add("dl,false,5,2,3");
                    conList.Add("dl,false,6,1,3");

                    for (int i = 0; i < geoList.Count; i++)
                    {
                        controller.AddGeometryFromString(geoList[i]);
                    }
                    //复制一下代表条件列表缓存，也可以不需要模板与副本，每次关卡都要从头加载条件列表
                    controller.useAnotherCondition = false;
                    controller.conditionLists = new List<ConditionsList>();                             //需要达成条件
                    controller.meetingconditionLists = new List<ConditionsList>();                      //复制自“需要达成条件”的正在达成条件
                    controller.conditionLists.Add(new ConditionsList());
                    for (int i = 0; i < conList.Count; i++)
                    {
                        controller.AddConditionFromString(conList[i], 0);
                    }
                    controller.meetingconditionLists.Add(controller.conditionLists[0].Copy());
                    controller.coordinate.GeometryCount = 1000;

                    break;
                }
                //画同心圆
            case 2:
                {
                    geoList.Add("p,1,n,true,-5,5");
                    geoList.Add("p,2,n,true,-10,-10");
                    geoList.Add("p,3,n,true,10,-10");

                    //List<string> conList = new List<string>();
                    conList.Add("f,true,1");
                    conList.Add("f,true,2");
                    conList.Add("f,true,3");
                    conList.Add("dc,false,4,1,2");
                    conList.Add("dc,false,5,1,3");

                    for (int i = 0; i < geoList.Count; i++)
                    {
                        controller.AddGeometryFromString(geoList[i]);
                    }
                    //复制一下代表条件列表缓存，也可以不需要模板与副本，每次关卡都要从头加载条件列表
                    controller.useAnotherCondition = false;
                    controller.conditionLists = new List<ConditionsList>();                             //需要达成条件
                    controller.meetingconditionLists = new List<ConditionsList>();                      //复制自“需要达成条件”的正在达成条件
                    controller.conditionLists.Add(new ConditionsList());
                    for (int i = 0; i < conList.Count; i++)
                    {
                        controller.AddConditionFromString(conList[i], 0);
                    }
                    controller.meetingconditionLists.Add(controller.conditionLists[0].Copy());
                    controller.coordinate.GeometryCount = 1000;

                    break;
                }
                //求交点
            case 3:
                {
                    geoList.Add("p,1,n,false,-5,5");
                    geoList.Add("p,2,n,false,-10,-10");
                    geoList.Add("p,3,n,false,10,-10");
                    geoList.Add("l,4,n,true,1,2");
                    geoList.Add("l,5,n,true,2,3");
                    geoList.Add("l,6,n,true,1,3");

                    //List<string> conList = new List<string>();
                    conList.Add("f,true,1");
                    conList.Add("f,true,2");
                    conList.Add("f,true,3");
                    conList.Add("pl,true,4");
                    conList.Add("pl,true,5");
                    conList.Add("pl,true,6");
                    conList.Add("i,false,7,4,5");
                    conList.Add("i,false,8,5,6");
                    conList.Add("i,false,9,4,6");

                    controller.useAnotherCondition = false;
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
                        controller.AddConditionFromString(conList[i], 0);
                    }
                    controller.meetingconditionLists.Add(controller.conditionLists[0].Copy());
                    controller.coordinate.GeometryCount = 1000;

                    break;
                }
                //60度角
            case 4:
                {
                    geoList.Add("p,1,n,true,-10,-10");
                    geoList.Add("p,2,n,true,10,-10");
                    geoList.Add("l,3,n,true,1,2");

                    conList.Add("f,true,1");
                    conList.Add("f,true,2");
                    conList.Add("dl,true,3,1,2");
                    conList.Add("o,false,4,3");
                    conList.Add("dc,false,5,1,4");
                    conList.Add("dc,false,6,4,1");
                    conList.Add("i,false,7,5,6,1");
                    conList.Add("dl,false,8,7,1");

                    controller.useAnotherCondition = false;
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
                        controller.AddConditionFromString(conList[i], 0);
                    }
                    controller.meetingconditionLists.Add(controller.conditionLists[0].Copy());
                    controller.coordinate.GeometryCount = 1000;

                    break;
                }
                //垂直平分线
            case 5:
                {
                    geoList.Add("p,1,n,true,-10,-10");
                    geoList.Add("p,2,n,true,10,-10");
                    geoList.Add("l,3,n,true,1,2");

                    conList.Add("f,true,1");
                    conList.Add("f,true,2");
                    conList.Add("dl,true,3,1,2");
                    conList.Add("dc,false,5,1,2");
                    conList.Add("dc,false,6,2,1");
                    conList.Add("i,false,7,5,6,1");
                    conList.Add("i,false,8,5,6,2");
                    conList.Add("dl,false,9,8,7");

                    controller.useAnotherCondition = false;
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
                        controller.AddConditionFromString(conList[i], 0);
                    }
                    controller.meetingconditionLists.Add(controller.conditionLists[0].Copy());
                    controller.coordinate.GeometryCount = 1000;

                    break;
                }
                //角平分线
             case 6:
                {
                    controller.useAnotherCondition = true;
                    controller.anotherConditionsList = new AnotherConditionsList();
                    controller.anotherConditionsList.reachedConditions = new List<AnotherCondition>();
                    controller.anotherConditionsList.unmetConditions = new List<AnotherCondition>();

                    //TODO 
                    //controller.anotherConditionsList.unmetConditions.Add(new PointCondition() { wantX = 1, wantY = 2, id = -1, isReached = false, });

                    break;
                }
                //垂线
            case 7:
                {

                    break;
                }
                //菱形内切圆
            case 8:
                {

                    break;
                }
                //求圆心
            case 9:
                {

                    break;
                }
        }

        return controller;
    }
 }