using System;
using System.Text;
using System.Security.Cryptography;
/// <summary>
/// 加密解密类
/// </summary>
namespace tsl.Crypto {
    public class AES {
        //默认密钥向量
        private static readonly byte[] Keys = { 0x41,0x72,0x65,0x79,0x6F,0x75,0x6D,0x79,0x53,0x6E,0x6F,0x77,0x6D,0x61,0x6E,0x3F };
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="encryptString">明文</param>
        /// <param name="encryptKey">密匙</param>
        /// <returns></returns>
        public static string Encrypt(string encryptString,string encryptKey) {
            try {
                if(string.IsNullOrEmpty(encryptString)) return "";
                encryptKey=GetSubString(encryptKey,0,32,"");
                encryptKey=encryptKey.PadRight(32,' ');
                RijndaelManaged rijndaelProvider = new RijndaelManaged();
                rijndaelProvider.Key =Encoding.UTF8.GetBytes(encryptKey.Substring(0,32));
                rijndaelProvider.IV  =Keys;
                ICryptoTransform rijndaelEncrypt = rijndaelProvider.CreateEncryptor();

                byte[] inputData     = Encoding.UTF8.GetBytes(encryptString);
                byte[] encryptedData = rijndaelEncrypt.TransformFinalBlock(inputData,0,inputData.Length);

                return Convert.ToBase64String(encryptedData);
            } catch {
                return "";
            }
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="decryptString">密文</param>
        /// <param name="decryptKey">密匙</param>
        /// <returns></returns>
        public static string Decrypt(string decryptString,string decryptKey) {
            try {
                if(string.IsNullOrEmpty(decryptString)) return "";
                decryptKey=GetSubString(decryptKey,0,32,"");
                decryptKey=decryptKey.PadRight(32,' ');

                RijndaelManaged rijndaelProvider = new RijndaelManaged();
                rijndaelProvider.Key =Encoding.UTF8.GetBytes(decryptKey);
                rijndaelProvider.IV  =Keys;
                ICryptoTransform rijndaelDecrypt = rijndaelProvider.CreateDecryptor();

                byte[] inputData     = Convert.FromBase64String(decryptString);
                byte[] decryptedData = rijndaelDecrypt.TransformFinalBlock(inputData,0,inputData.Length);

                return Encoding.UTF8.GetString(decryptedData);
            } catch {
                return "";
            }
        }
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="p_SrcString"></param>
        /// <param name="p_StartIndex"></param>
        /// <param name="p_Length"></param>
        /// <param name="p_TailString"></param>
        /// <returns></returns>
        public static string GetSubString(string p_SrcString,int p_StartIndex,int p_Length,string p_TailString) {
            string myResult  = p_SrcString;
            byte[] bComments = Encoding.UTF8.GetBytes(p_SrcString);
            foreach(char c in Encoding.UTF8.GetChars(bComments)) {    //当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
                if((c>'\u0800'&&c<'\u4e00')||(c>'\xAC00'&&c<'\xD7A3')) {
                    //当截取的起始位置超出字段串长度时
                    if(p_StartIndex>=p_SrcString.Length)
                        return "";
                    else
                        return p_SrcString.Substring(p_StartIndex,
                                                       ((p_Length+p_StartIndex)>p_SrcString.Length) 
                                                        ? (p_SrcString.Length-p_StartIndex) 
                                                        : p_Length);
                }
            }
            if(p_Length>=0) {
                byte[] bsSrcString = Encoding.Default.GetBytes(p_SrcString);
                //当字符串长度大于起始位置
                if(bsSrcString.Length>p_StartIndex) {
                    int p_EndIndex = bsSrcString.Length;

                    //当要截取的长度在字符串的有效长度范围内
                    if(bsSrcString.Length>(p_StartIndex+p_Length)) {
                        p_EndIndex=p_Length+p_StartIndex;
                    } else {   //当不在有效范围内时,只取到字符串的结尾

                        p_Length=bsSrcString.Length-p_StartIndex;
                        p_TailString="";
                    }

                    int nRealLength = p_Length;
                    int[] anResultFlag = new int[p_Length];
                    int nFlag = 0;
                    for(int i = p_StartIndex;i<p_EndIndex;i++) {
                        if(bsSrcString[i]>127) {
                            nFlag++;
                            if(nFlag==3)
                                nFlag=1;
                        } else
                            nFlag=0;

                        anResultFlag[i]=nFlag;
                    }

                    if((bsSrcString[p_EndIndex-1]>127)&&(anResultFlag[p_Length-1]==1))
                        nRealLength=p_Length+1;

                    byte[] bsResult = new byte[nRealLength];
                    Array.Copy(bsSrcString,p_StartIndex,bsResult,0,nRealLength);

                    myResult=Encoding.Default.GetString(bsResult);
                    myResult+=p_TailString;
                }
            }
            return myResult;
        }
    }
}
