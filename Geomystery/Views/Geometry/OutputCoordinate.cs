using Geomystery.Models.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Views.Geometry
{
    //显示坐标系
    public class OutputCoordinate
    {
        //被显示的坐标系
        public Coordinate coordinate { get; set; }

        //被显示的几何实体列表
        public List<OutputGeometry> GeometryList { get; set; }

        //（视图，显示，展示）坐标系构造函数
        public OutputCoordinate(Coordinate coordinate)
        {
            this.coordinate = coordinate;
        }
    }
}
