using Geomystery.Controllers.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Award
{
    public class LevelLoader
    {
        public static LevelLoader LL;
        public static Controllers.Geometry.Controllers GetLevel(int index)
        {
            Controllers.Geometry.Controllers controller = new Controllers.Geometry.Controllers(1);

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
                        geoList.Add("p,1,n,true,-2,2");
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
                        geoList.Add("p,2,n,true,-14,-4");
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
                        geoList.Add("p,1,n,false,-2,2");
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
                        conList.Add("i,false,7,4,5,0");
                        conList.Add("i,false,8,5,6,0");
                        conList.Add("i,false,9,4,6,0");

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
                        conList.Add("dc,false,5,1,2");
                        conList.Add("dc,false,6,2,1");
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
                        geoList.Add("p,1,n,true,0,0");
                        geoList.Add("p,2,n,true,10,6");
                        geoList.Add("p,3,n,true,10,-6");
                        geoList.Add("l,4,n,true,1,2");
                        geoList.Add("l,5,n,true,1,3");

                        controller.useAnotherCondition = true;
                        controller.anotherConditionsList = new AnotherConditionsList();
                        controller.anotherConditionsList.reachedConditions = new List<AnotherCondition>();
                        controller.anotherConditionsList.unmetConditions = new List<AnotherCondition>();

                        //TODO 
                        controller.anotherConditionsList.unmetConditions.Add(new LineCondition() { wantX = 0, wantY = 0,slope = 0 });

                        break;
                    }
                //垂线
                case 7:
                    {
                        geoList.Add("p,1,n,true,0,0");
                        geoList.Add("p,2,n,true,0,6");
                        geoList.Add("p,3,n,true,0,-6");
                        geoList.Add("l,4,n,true,1,3");

                        controller.useAnotherCondition = true;
                        controller.anotherConditionsList = new AnotherConditionsList();
                        controller.anotherConditionsList.reachedConditions = new List<AnotherCondition>();
                        controller.anotherConditionsList.unmetConditions = new List<AnotherCondition>();

                        //TODO 
                        controller.anotherConditionsList.unmetConditions.Add(new LineCondition() { wantX = 0, wantY = 0, slope = 0 });

                        break;
                    }
                //菱形内切圆
                case 8:
                    {
                        geoList.Add("p,1,n,true,8,0");
                        geoList.Add("p,2,n,true,0,6");
                        geoList.Add("p,3,n,true,0,-6");
                        geoList.Add("p,4,n,true,-8,0");
                        geoList.Add("l,5,n,true,1,2");
                        geoList.Add("l,6,n,true,2,4");
                        geoList.Add("l,7,n,true,4,3");
                        geoList.Add("l,8,n,true,1,3");

                        controller.useAnotherCondition = true;
                        controller.anotherConditionsList = new AnotherConditionsList();
                        controller.anotherConditionsList.reachedConditions = new List<AnotherCondition>();
                        controller.anotherConditionsList.unmetConditions = new List<AnotherCondition>();

                        //TODO 
                        controller.anotherConditionsList.unmetConditions.Add(new CircleCondition() { wantX = 0, wantY = 0, radius = 5 });

                        break;
                    }
                //求圆心
                case 9:
                    {
                        geoList.Add("p,1,n,false,0,0");
                        geoList.Add("p,2,n,false,0,6");
                        geoList.Add("c,3,n,true,1,2");


                        controller.useAnotherCondition = true;
                        controller.anotherConditionsList = new AnotherConditionsList();
                        controller.anotherConditionsList.reachedConditions = new List<AnotherCondition>();
                        controller.anotherConditionsList.unmetConditions = new List<AnotherCondition>();

                        //TODO 
                        controller.anotherConditionsList.unmetConditions.Add(new PointCondition() { wantX = 0, wantY = 0 });

                        break;
                    }
                //过给定点画出给定直线的平行直线
                case 10:
                    {

                        break;
                    }
                //画出给定圆在给定点位置的切线
                case 11:
                    {

                        break;
                    }
                //在给定直线的逆时针方向上过给定点画出一个60°角
                case 12:
                    {

                        break;
                    }
                //过给定点画出给定圆的外切正三角形
                case 13:
                    {

                        break;
                    }
                //画出给定梯形的中位线
                case 14:
                    {

                        break;
                    }
                //平分矩形
                case 15:
                    {

                        break;
                    }
                //三角形的外接圆
                case 16:
                    {

                        break;
                    }
                //三角形的内切圆
                case 17:
                    {

                        break;
                    }
                //正六边形
                case 18:
                    {

                        break;
                    }
            }
            return controller;
        }
    }
}
