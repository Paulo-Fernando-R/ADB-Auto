using ADB_Auto.Models;
using System.Collections.Generic;

namespace ADB_Auto.Repositories.Interfaces
{
    public interface IIPRespository
    {
        void Remove(IList<InternetProtocol> ips);
        bool Save(InternetProtocol ip);
        IList<InternetProtocol> GetAll();
    }
}
