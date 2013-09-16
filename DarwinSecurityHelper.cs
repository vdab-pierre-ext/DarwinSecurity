using DarwinSecurity.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace DarwinSecurity
{
    public static class DarwinSecurityHelper
    {
        public static bool Login(string naam, string wachtwoord) {
            
            Gebruiker gebruiker = new Gebruiker{UserName=naam,Password=wachtwoord};
            
            Encryptor enc = new Encryptor(DarwinConfig.PrivateKey);
            MessageArgs msgArgs = new MessageArgs { username = gebruiker.UserName, password = gebruiker.Password };
            
            
            AanvraagMessage avMsg = new AanvraagMessage { method=DarwinConfig.Method,args=msgArgs};
            string strMsg = JsonConvert.SerializeObject(avMsg);
            string encryptedMsg = enc.Encrypt3DES(strMsg);
            
            Aanvraag deAanvraag = new Aanvraag { 
                appId=DarwinConfig.AppId,
                message=encryptedMsg
            };
            string aanvraagText = JsonConvert.SerializeObject(deAanvraag);

            //vraag sturen
            var httpwebrequest = (HttpWebRequest)WebRequest.Create(DarwinConfig.DarwinURL);
            httpwebrequest.Method = "POST";
            //een stream maken om er in te kunnen schrijven
            Stream dataStream = httpwebrequest.GetRequestStream();
            TextWriter tw = new StreamWriter(dataStream);
            tw.WriteLine("contents=" + aanvraagText);
            tw.Flush();
            tw.Close();

            //antwoord van Darwin
            HttpWebResponse response = (HttpWebResponse)httpwebrequest.GetResponse();

            //inlezen in tekst
            StreamReader sr = new StreamReader(response.GetResponseStream());
            string responseText = sr.ReadToEnd();
            
            //antwoord object uit die tekst maken
            Antwoord jsonAntwoord = JsonConvert.DeserializeObject<Antwoord>(responseText);

            //isRegisteredUser betekent dat gebruiker een "Darwin" gebruiker is
            bool isRegisteredUser = false;
            if (jsonAntwoord.status == "OK")
            {
                string plainText = enc.Decrypt3DES(jsonAntwoord.response);

                //andere manier om een json string om te zetten in een Json-object
                JObject o = JObject.Parse(plainText);
                if (o["exception"] == null && o["id"] != null)
                {
                    isRegisteredUser = true;
                }

            }
            return isRegisteredUser;
        }
    }
}
