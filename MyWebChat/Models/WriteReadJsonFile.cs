using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MyWebChat.Models
{
    public class WriteReadJsonFile
    {
        public static bool WriteList<T>(List<T> list, string fileName1,string fileName2)
        {
            try
            {
                string json = JsonConvert.SerializeObject(list, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
                File.WriteAllText(HttpContext.Current.Server.MapPath(@"~/Files/") + "" + fileName1, json);
                File.WriteAllText(HttpContext.Current.Server.MapPath(@"~/Files/") + "" + fileName2, json);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool ReadList<T>(ref List<T> list, string fileName)
        {
            string file = HttpContext.Current.Server.MapPath(@"~/Files/") + "" + fileName;
            if (!File.Exists(file))
            {
                File.Create(file).Close();
                return true;
            }
            else
            {
                try
                {
                    string json = File.ReadAllText(HttpContext.Current.Server.MapPath(@"~/Files/") + "" + fileName);
                    list = JsonConvert.DeserializeObject<List<T>>(json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public static bool CheckExistsFile(string fileName)
        {
            string file = HttpContext.Current.Server.MapPath(@"~/Files/") + "" + fileName;
            if (!File.Exists(file))
                return false;
            else
                return true;
        }
    }
}