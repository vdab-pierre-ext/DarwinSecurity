using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarwinSecurity.Model
{
    public static class DarwinConfig
    {
        public static string Method
        {
            get { return "user.getByUsernameAndPassword";  }
        }
        public static int AppId { 
            get {
                return 3;    
            } 
        }
        public static string DarwinURL { get { return "http://arghun/D3Core/bulwark.php"; } }
        public static string PrivateKey { get { return "T45ua59k"; } }
    }
}
