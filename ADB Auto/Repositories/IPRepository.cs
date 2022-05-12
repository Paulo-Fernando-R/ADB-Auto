using ADB_Auto.Repositories.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ADB_Auto.Repositories
{
    public class IPRepository : IIPRespository
    {
        private const string FILE_NAME = "SaveIPs.txt";

        public IList<string> GetAll()
        {
            if (File.Exists(FILE_NAME))
            {
                return new List<string>();
            }

            IList<string> lines = File.ReadAllLines(FILE_NAME).ToList();
            return lines;
        }

        public void Remove(IList<string> ips)
        {
            StreamWriter sw = new StreamWriter(FILE_NAME);

            foreach (string ip in ips)
            {
                sw.WriteLine(ip);
            }

            sw.Close();
        }

        public bool Save(string ip)
        {
            try
            {
                StreamWriter sw = new StreamWriter(FILE_NAME, true);
                sw.WriteLine(ip);
                sw.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
