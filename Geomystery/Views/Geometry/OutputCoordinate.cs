using Geomystery.Models.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Views.Geometry
{
    //显示坐标系
    public class OutputCoordinate
    {
        //被显示的坐标系
        public Coordinate coordinate { get; set; }


        /*
         * 单位长度unit length
         * 逻辑坐标系的1单位长度相当于多少 DIP
         * DIP代表“器件独立像素”。这是可以与物理像素相同，大于或小于的虚拟化单元。
         * 像素和DIP之间的比例由DPI决定：pixels = dips * dpi / 96
         */
        public float UnitLength { get; set; }


        /*
         * 从逻辑坐标系到显示器坐标系的变换，可以想象：
         * 逻辑坐标系第四象限有一个矩形框，这个矩形框就是要显示在显示器上的部分
         * vector(v1, v2)即是逻辑坐标系原点指向显示器矩形框左上角的向量
         * 假设单位长度UL(unit length) = 10 dip，可以想象矩形框可以覆盖第四象限
         * （如果对于向量vector(v1, v2)，有v1 < 0 || v2 > 0，显示器会显示更大的一部分可能包括一二三象限）
         * 逻辑点p(a1, b1)在显示器上的坐标应该是( (a1 - v1) * UL , (v2 - b1) * UL )
         * 同理，用户在屏幕上点击点(p1, p2)映射到逻辑坐标系当中应该是 ( p1 / UL + v1 , v2 - p2 / UL  )
         */
        public Vector2 vector { get; set; }


        //被显示的几何实体列表
        public List<OutputGeometry> GeometryList { get; set; }

        //（视图，显示，展示）坐标系构造函数
        public OutputCoordinate(Coordinate coordinate)
        {
            this.coordinate = coordinate;
        }

        public  Vector2 ToVector2(Point2 p2)//logi-screen
        {
            float x = p2.X - vector.X;
            float y = p2.Y - vector.Y;
            x = x / UnitLength;
            y = -y / UnitLength;
            Vector2 v2 = new Vector2(x,y);
            return v2;
        }

        public Point2 ToPoint2(Vector2 v2)//screen-logi
        { 
            Point2 p2 = new Point2() { X = (v2.X*UnitLength+vector.X), Y = -(v2.Y*UnitLength+vector.Y) };
            return p2;
        }
    }
}
