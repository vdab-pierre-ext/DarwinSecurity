using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarwinSecurity.Model
{
    public class Gebruiker:Bibliotheek.Abstract.IGebruiker
    {
        public string UserName { get; set; }
        public string Password { get; set; }
     }
}