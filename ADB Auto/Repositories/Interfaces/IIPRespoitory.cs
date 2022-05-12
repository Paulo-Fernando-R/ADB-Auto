using System.Collections.Generic;

namespace ADB_Auto.Repositories.Interfaces
{
    public interface IIPRespository
    {
        void Remove(IList<string> ips);
        bool Save(string ip);
        IList<string> GetAll();
    }
}
