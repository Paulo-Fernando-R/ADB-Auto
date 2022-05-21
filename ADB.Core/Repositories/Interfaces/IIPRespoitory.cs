using ADB.Core.Models;
using System.Collections.Generic;

namespace ADB.Core.Repositories.Interfaces
{
    public interface IIPRespository
    {
        void Remove(IList<InternetProtocol> ips);
        bool Save(InternetProtocol ip);
        IList<InternetProtocol> GetAll();
    }
}
