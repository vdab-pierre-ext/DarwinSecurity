using System;
using System.Data;
using System.Configuration;
using System.Web;


using System.Security.Cryptography;
using System.Text;



/// <summary>
/// Summary description for Encryptor
/// </summary>
///
namespace Utility
{
    public class Encryptor
    {
        private Encoding _encoding;
        public Encoding Encoding { 
            get {
                if (_encoding == null) {
                    _encoding = Encoding.UTF8;
                }
                return _encoding;
            }
            set {
                _encoding = value;
            }
        }

        private string _key;
        public string Key { get { return _key; } 
            set {
                if(value.Length!=8)
                {
                    throw new Exception("Key is geen 8 karakters lang.");
                }
                _key = value;
            }
        }

        public Encryptor(string key)
        {
            this.Key = key;
        }

        public string Encrypt3DES(string strString) {
            strString= HttpUtility.HtmlEncode(strString);
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();

            DES.Key = Encoding.GetBytes(this.Key);
            DES.Mode = CipherMode.ECB;
            DES.Padding = PaddingMode.Zeros;

            ICryptoTransform DESEncrypt = DES.CreateEncryptor();

            byte[] buffer = _encoding.GetBytes(strString);

            string base64 = Convert.ToBase64String(DESEncrypt.TransformFinalBlock(buffer, 0, buffer.Length));
            string zonderSpaties = base64.Contains(" ") ? base64.Replace(' ', '+') : base64;

            return zonderSpaties;
        }

        public string Decrypt3DES(string strString) {

            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();

            DES.Key = Encoding.GetBytes(this.Key);
            DES.Mode = CipherMode.ECB;
            DES.Padding = PaddingMode.Zeros;
            
            ICryptoTransform DESDecrypt = DES.CreateDecryptor();

            strString = strString.Contains(" ") ? strString.Replace(' ', '+') : strString;
            
            byte[] buffer = Convert.FromBase64String(strString);

            return HttpUtility.HtmlDecode(UTF8Encoding.UTF8.GetString(DESDecrypt.TransformFinalBlock(buffer, 0, buffer.Length)));
        }
    }
}