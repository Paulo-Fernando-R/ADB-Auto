using ADB.Core.Models;
using System.Collections.Generic;

namespace ADB.Core.Repositories.Interfaces
{
    public interface IIPRespository
    {
        InternetProtocol GetByIP(string ip);
        void RemoveRange(IList<InternetProtocol> ips);
        void Remove(InternetProtocol ip);
        bool Save(InternetProtocol ip);
        IList<InternetProtocol> GetAll();
    }
}
