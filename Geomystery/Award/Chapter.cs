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
        public string cover { get; set; }
        public ObservableCollection<Level> Levels { get; set; }
        public int unlocked { get; set; }
        public Chapter(int id=0)
        {
            ID = id;
            name = discribe = "";
            Levels = new ObservableCollection<Level>();
            unlocked = 0;
        }
        public static ObservableCollection<Chapter> getChapters()
        {
            ObservableCollection<Chapter> k = new ObservableCollection<Chapter>();
            k.Add(new Chapter(1) );
            k.Add(new Chapter(2) );
            k.Add(new Chapter(3) );
            foreach (var x in k)
            {
                x.name = AppResources.GetString("CN" + x.ID.ToString()); 
                x.discribe = AppResources.GetString("CD" + x.ID.ToString()) ;
                x.cover = "ms-appx:///Pictures/Chapters/" + x.ID.ToString() + ".png";
                int hvdone = (APPDATA.app_data.HAVEDONE-1)/ 9;
                if (x.ID - 1 <= hvdone) x.unlocked = 1;
            }
            return k;
        }
    }
}
