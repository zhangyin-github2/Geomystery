using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Models.Geometry
{
    public abstract class Geometry
    {
        //所在坐标系
        public Coordinate coord { get; set; }
    }
}
