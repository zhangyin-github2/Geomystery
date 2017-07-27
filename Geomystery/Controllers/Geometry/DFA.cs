using Geomystery.Models.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Controllers.Geometry
{
    /// <summary>
    /// 当前工具选择栈
    /// </summary>
    public class SelectedGeometryStackItem
    {
        /// <summary>
        /// 被选择的几何元素
        /// </summary>
        public Models.Geometry.Geometry selectedGeometry { get; set; }

        /// <summary>
        /// 是否为新创建的，如果是新创建的则需要撤销
        /// </summary>
        public bool IsNew { get; set; }
    }

    /// <summary>
    /// 被选中的几何元素的需求项
    /// </summary>
    public class NeedGeometrySetItem
    {
        /// <summary>
        /// 需要的几何元素类型
        /// </summary>
        public Type type { get; set; }

        /// <summary>
        /// 需要的数量
        /// </summary>
        public int needNumber { get; set; }

        /// <summary>
        /// 实际选择栈
        /// </summary>
        public List<SelectedGeometryStackItem> selectStack;
    }

    /// <summary>
    /// 用户操作记录器，借用有限状态自动机的DFA，但本身并不是DFA
    /// </summary>
    public class DFA
    {
        /// <summary>
        /// 工具名称，用来记录当前的工具
        /// </summary>
        public UserTool userTool { get; set; }

        /// <summary>
        /// 工作模式：0 构造模式，不重复的确定数量的元素构造，1 可取消模式，可以取消上一次选择的点，2 增加模式，例如构造多边形时，重复选择将会封闭图形
        /// </summary>
        public int workingMode { get; set; }

        /// <summary>
        /// 需求序列
        /// </summary>
        public List<NeedGeometrySetItem> needList { get; set; }

        /// <summary>
        /// 轮到哪个部分了
        /// </summary>
        public int turn { get; set; }

        /// <summary>
        /// 自动机是否初始化
        /// </summary>
        public bool isInitialized { get; set; }

        /// <summary>
        /// 自动机当前状态 0 初始状态 1 工作中 2 结束状态 负数 错误状态
        /// </summary>
        public int state { get; private set; }

        /// <summary>
        /// 工作场景
        /// </summary>
        public Coordinate coordinate { get; set; }

        /// <summary>
        /// 工具使用结果
        /// </summary>
        public List<Models.Geometry.Geometry> result { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public DFA()
        {
            userTool = null;
            isInitialized = false;
            state = -1;
            turn = -1;
            //selectStack = new List<SelectedGeometryStackItem>();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userTool">使用工具</param>
        /// <param name="workmode">工作模式</param>
        /// <param name="coordinate">工作场景坐标系</param>
        public DFA(UserTool userTool, int workmode, Coordinate coordinate)
        {
            this.userTool = userTool;
            this.coordinate = coordinate;
            if (userTool != null && this.coordinate != null)
            {
                if(userTool.NeedGeometryList != null)                  //需要初始化需求序列
                {
                    needList = new List<NeedGeometrySetItem>();
                    for (int i = 0; i < userTool.NeedGeometryList.Count; i++)
                    {
                        needList.Add(new NeedGeometrySetItem()
                        {
                            type = userTool.NeedGeometryList[i].type,
                            needNumber = userTool.NeedGeometryList[i].needNumber,
                            selectStack = new List<SelectedGeometryStackItem>(),
                        });
                    }
                }
                isInitialized = true;           //经过初始化
                state = 1;                      //执行
                turn = 0;                           //填充need list 下标
                this.workingMode = workmode;        //工作模式
            }
            else
            {
                isInitialized = false;
                turn = -1;
                state = -1;
            }
            
        }

        public int Initialized(UserTool userTool, int workmode, Coordinate coordinate)
        {
            if(isInitialized)
            {
                return 1;
            }
            else if(!isInitialized && userTool != null && coordinate != null)             //还没初始化
            {
                if (userTool.NeedGeometryList != null)                  //需要初始化需求序列
                {
                    needList = new List<NeedGeometrySetItem>();
                    for (int i = 0; i < userTool.NeedGeometryList.Count; i++)
                    {
                        needList.Add(new NeedGeometrySetItem()
                        {
                            type = userTool.NeedGeometryList[i].type,
                            needNumber = userTool.NeedGeometryList[i].needNumber,
                            selectStack = new List<SelectedGeometryStackItem>(),
                        });
                    }
                }
                isInitialized = true;           //经过初始化
                state = 1;                      //执行
                turn = 0;                           //填充need list 下标
                this.workingMode = workmode;        //工作模式
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// 用户选择了一个元素（记录了用户的操作）
        /// </summary>
        /// <param name="geometry">用户选择的元素</param>
        /// <param name="newCreate">这个元素是否是新增的</param>
        /// <returns></returns>
        public int UserSelectGeomerty(Models.Geometry.Geometry geometry, bool newCreate)
        {
            if(workingMode == 0)                //不可撤销模式
            {
                if(state == 1)                          //工作状态
                {
                    foreach (SelectedGeometryStackItem selectedItem in needList[turn].selectStack)
                    {
                        if (selectedItem.selectedGeometry == geometry)          //存在重复项
                        {
                            state = -2;              //需要撤销更改
                            return -2;              //需要撤销
                        }
                    }

                    if(geometry.GetType() == needList[turn].type)               //类型匹配检查
                    {
                        if (needList[turn].needNumber == needList[turn].selectStack.Count - 1) turn++;
                        needList[turn].selectStack.Add(new SelectedGeometryStackItem() { IsNew = newCreate, selectedGeometry = geometry });
                        if (turn == needList.Count - 1 && needList[turn].selectStack.Count == needList[turn].needNumber)
                        {
                            state = 2;                      //创作完成
                            return 2;
                        }
                        //state = 1;
                    }
                    else
                    {
                        state = -2;              //需要撤销更改
                        return -2;              //需要撤销
                    }
                    
                }
            }
            else if(workingMode == 1)           //可以撤销模式
            {
                if (state == 1)                          //工作状态
                {
                    SelectedGeometryStackItem selectedItem = needList[turn].selectStack[needList[turn].selectStack.Count - 1];
                    if (selectedItem.selectedGeometry == geometry)                  //重复选择上一个元素
                    {
                        if(selectedItem.IsNew)                                  //上一个元素是新建的
                        {
                            geometry.coord.Remove(geometry);
                        }
                        needList[turn].selectStack.Remove(selectedItem);                      //撤销选择
                        if (needList[turn].selectStack.Count == 0) turn--;              //前一个
                        //state = 1;
                    }
                    else
                    {
                        if (geometry.GetType() == needList[turn].type)
                        {
                            if (needList[turn].needNumber == needList[turn].selectStack.Count - 1) turn++;
                            needList[turn].selectStack.Add(new SelectedGeometryStackItem() { IsNew = newCreate, selectedGeometry = geometry });
                            if (turn == needList.Count - 1 && needList[turn].selectStack.Count == needList[turn].needNumber)
                            {
                                state = 2;                      //创作完成
                                return 2;
                            }

                        }
                        else
                        {
                            state = -2;              //需要撤销更改
                            return -2;              //需要撤销
                        }
                            
                    }
                }
            }
            else if(workingMode ==2)            //多重选择自动封闭模式
            {

            }
            return 1;
        }

        /// <summary>
        /// 撤销本工具进行的操作
        /// </summary>
        public void Undo()
        {
            if (!isInitialized) return;               //未初始化
            if (needList == null) return;               //不存在的情况
            for(int i = turn; i >= 0; i--)
            {
                if (needList[turn].selectStack.Count == 0) continue;
                for(int j = needList[turn].selectStack.Count; j >= 0; j--)
                {
                    if (needList[turn].selectStack[j].IsNew)
                    {
                        coordinate.Remove(needList[turn].selectStack[j].selectedGeometry);
                    }
                }
                //needList[turn].selectStack.Clear();
            }
        }

        /// <summary>
        /// 重做
        /// </summary>
        public void Redo()
        {
            if (!isInitialized) return;               //未初始化
            if (needList == null) return;               //不存在的情况
            for (int i = turn; i >= 0; i--)
            {
                if (needList[turn].selectStack.Count == 0) continue;
                for (int j = needList[turn].selectStack.Count; j >= 0; j--)
                {
                    if (needList[turn].selectStack[j].IsNew)
                    {
                        coordinate.Remove(needList[turn].selectStack[j].selectedGeometry);
                    }
                }
                //needList[turn].selectStack.Clear();
            }
        }
    }
}
