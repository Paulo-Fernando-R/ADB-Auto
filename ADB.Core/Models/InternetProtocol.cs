using System.Collections.Generic;

namespace ADB.Core.Models
{
    public class InternetProtocol
    {
        public string IP { get; set; }

        public InternetProtocol(string ip)
        {
            IP = ip;
        }

        public override bool Equals(object obj)
        {
            return obj is InternetProtocol protocol&&
                   IP==protocol.IP;
        }

        public override int GetHashCode()
        {
            return -419837636+EqualityComparer<string>.Default.GetHashCode(IP);
        }

        public override string ToString()
        {
            return IP;
        }
    }
}
