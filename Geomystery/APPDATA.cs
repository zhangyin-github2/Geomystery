using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Geomystery
{
    public static class APPDATA
    {
        public static bool ISMUTE = false;
        public static bool ISNIGHT = false;
        public static bool ISFULLSCREEN;
        public static void SAVE()
        {
            string DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "APPDATA.db");
            var conn = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath);
            conn.CreateTable<option_data>();// 创建 option_data 模型对应的表，如果已存在，则忽略该操作。
            var db = conn.Table<option_data>();
            var k = new option_data();
            if (db.Count() == 0) conn.Insert(k);
            else
            {
                conn.DeleteAll(typeof(option_data));
                conn.Insert(k);
            }
        }
        public static void LOAD()
        {
            string DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "APPDATA.db");
            var conn = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath);
            conn.CreateTable<option_data>();// 创建 option_data 模型对应的表，如果已存在，则忽略该操作。
            var db = conn.Table<option_data>();
            var k = new option_data();
            if (db.Count() == 0) return;
            else
            {
                k = db.First();
                ISMUTE = k.ISMUTE;
                ISNIGHT = k.ISNIGHT;
            }
        }
    }
    public class option_data
    {
        public  bool ISMUTE { set; get; }
        public  bool ISNIGHT { set; get; }
        public option_data()
        {
            ISMUTE = APPDATA.ISMUTE;
            ISNIGHT = APPDATA.ISNIGHT;
        }
    }
}
