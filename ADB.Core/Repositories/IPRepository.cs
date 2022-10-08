using ADB.Core.Models;
using ADB.Core.Repositories.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ADB.Core.Repositories
{
    public class IPRepository : IIPRespository
    {
        private readonly string filePath;

        public IPRepository(string filePath)
        {
            this.filePath = filePath;
        }

        public void Remove(InternetProtocol internetProtocol)
        {
            IList<InternetProtocol> internetProtocols = GetAll();
            internetProtocols.Remove(internetProtocol);

            StreamWriter sw = new StreamWriter(filePath);

            foreach (InternetProtocol ip in internetProtocols)
            {
                sw.WriteLine(ip.IP);
            }

            sw.Close();
        }

        public InternetProtocol GetByIP(string ip)
        {
            IList<InternetProtocol> internetProtocols = GetAll();
            foreach (InternetProtocol internetProtocol in internetProtocols )
            {
                if (internetProtocol.IP.Equals(ip))
                    return internetProtocol;
            }

            return null;
        }

        public IList<InternetProtocol> GetAll()
        {
            if (!File.Exists(filePath))
            {
                return new List<InternetProtocol>();
            }

            IList<InternetProtocol> internetProtocols = File.ReadAllLines(filePath).Select(x => new InternetProtocol(x)).ToList();
            return internetProtocols;
        }

        public void RemoveRange(IList<InternetProtocol> ips)
        {
            StreamWriter sw = new StreamWriter(filePath);

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
                StreamWriter sw = new StreamWriter(filePath, true);
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
