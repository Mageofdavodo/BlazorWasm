using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudWasm.Shared
{
    public class UserSession
    {
        public string Token { get; set; }
        public int ExpireTime { get; set; }
        public DateTime ExpiryTimeStamp { get; set; }
    }
}
