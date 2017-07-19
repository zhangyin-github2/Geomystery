using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Controllers.Geometry
{
    /// <summary>
    /// 用户操作的方式作为自动机状态转换的条件
    /// </summary>
    public enum UserOperater
    {
        //
        // 摘要:
        //     指针进入
        PointerEntered = 0,
        //
        // 摘要:
        //     指针离开
        PointerExit = 1,
        //
        // 摘要:
        //     指针按下
        PointerPressed = 2,
        //
        // 摘要:
        //     指针弹起
        PointerReleased = 3,
        //
        // 摘要:
        //     指针移动
        PointerMoved = 4,
        //
        // 摘要:
        //     指针弹起
        KeyUp = 5,
        //
        // 摘要:
        //     指针弹起
        KeyDown = 6,
    }

    public class DFA
    {
        /// <summary>
        /// 自动机类型名（“点”“直线”“圆”）
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 自动机初始化
        /// </summary>
        public bool IsInitialized { get; set; }

        /// <summary>
        /// 自动机当前状态
        /// </summary>
        public int state { get;private set; }

        /// <summary>
        /// 终止状态集合
        /// </summary>
        public List<int> endState { get; set; }

        /// <summary>
        /// 通过名字构建DFA
        /// </summary>
        public DFA(string name)
        {
            this.name = name;
            this.IsInitialized = false;
        }

        /// <summary>
        /// 状态转换函数
        /// </summary>
        /// <param name="nowState">当前状态</param>
        /// <param name="userOp">用户操作</param>
        /// <returns></returns>
        public static int stateChange(int nowState, UserOperater userOp)
        {
            return 0;
        }

        /// <summary>
        /// 触发当前这台自动机状态变化
        /// </summary>
        /// <param name="userOp"></param>
        /// <returns></returns>
        public int realStateChangeFunction(UserOperater userOp)
        {
            //this.state = stateChange(0, UserOperater.PointerPressed);
            return 0;
        }
    }
}
