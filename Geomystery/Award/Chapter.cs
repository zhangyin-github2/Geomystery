using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Award
{
    public class Chapter
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string discribe { get; set; }

        public ObservableCollection<Level> Levels { get; set; }
}
