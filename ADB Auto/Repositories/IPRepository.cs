using ADB_Auto.Models;
using ADB_Auto.Repositories.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ADB_Auto.Repositories
{
    public class IPRepository : IIPRespository
    {
        private const string FILE_NAME = "SaveIPs.txt";

        public IList<InternetProtocol> GetAll()
        {
            if (!File.Exists(FILE_NAME))
            {
                return new List<InternetProtocol>();
            }

            IList<InternetProtocol> internetProtocols = File.ReadAllLines(FILE_NAME).Select(x => new InternetProtocol(x)).ToList();
            return internetProtocols;
        }

        public void Remove(IList<InternetProtocol> ips)
        {
            StreamWriter sw = new StreamWriter(FILE_NAME);

            foreach (InternetProtocol ip in ips)
            {
                sw.WriteLine(ip.IP);
            }

            sw.Close();
        }

        public bool Save(InternetProtocol ip)
        {
            try
            {
                StreamWriter sw = new StreamWriter(FILE_NAME, true);
                sw.WriteLine(ip.IP);
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
