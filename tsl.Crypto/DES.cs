using System;
using System.Security.Cryptography;
using System.Text;
/// <summary>
/// 加密解密类
/// </summary>
namespace tsl.Crypto {
    public class DES {
        #region --加密--
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text">明文</param>
        /// <returns></returns>
        public static string Encrypt(string Text) {
            return Encrypt(Text,"tianzhao");
        }
        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="Text">明文</param> 
        /// <param name="sKey">密匙</param> 
        /// <returns></returns> 
        public static string Encrypt(string Text,string sKey) {
            try {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray;
                inputByteArray=Encoding.Default.GetBytes(Text);
                string md5SKey = MD5Comm.Get32MD5One(sKey).Substring(0,8);
                des.Key=ASCIIEncoding.ASCII.GetBytes(md5SKey);
                des.IV=ASCIIEncoding.ASCII.GetBytes(md5SKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream cs = new CryptoStream(ms,des.CreateEncryptor(),CryptoStreamMode.Write);
                cs.Write(inputByteArray,0,inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                foreach(byte b in ms.ToArray()) {
                    ret.AppendFormat("{0:X2}",b);
                }
                return ret.ToString();
            } catch {
                return "";
            }
        }
        #endregion

        #region --解密--

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text">密文</param>
        /// <returns></returns>
        public static string Decrypt(string Text) {
            return Decrypt(Text,"tianzhao");
        }
        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="Text">密文</param> 
        /// <param name="sKey">密匙</param> 
        /// <returns></returns> 
        public static string Decrypt(string Text,string sKey) {
            try {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                int len                      =Text.Length/2;
                byte[] inputByteArray        = new byte[len];
                int x, i;
                for(x=0;x<len;x++) {
                    i=Convert.ToInt32(Text.Substring(x*2,2),16);
                    inputByteArray[x]=(byte)i;
                }
                string md5SKey            = MD5Comm.Get32MD5One(sKey).Substring(0,8);
                des.Key                   =ASCIIEncoding.ASCII.GetBytes(md5SKey);
                des.IV                    =ASCIIEncoding.ASCII.GetBytes(md5SKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream cs           = new CryptoStream(ms,des.CreateDecryptor(),CryptoStreamMode.Write);
                cs.Write(inputByteArray,0,inputByteArray.Length);
                cs.FlushFinalBlock();
                return Encoding.Default.GetString(ms.ToArray());
            } catch {
                return "";
            }
        }
        #endregion
    }
}
