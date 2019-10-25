﻿using System;
using System.Security.Cryptography;
using System.Text;
/// <summary>
/// 加密解密类
/// </summary>
namespace tsl.Crypto {
    /// <summary>
    /// 此类获取md5加密值
    /// </summary>
    public class MD5Comm {
        /// <summary>
        /// 通过创建哈希字符串适用于任何 MD5 哈希函数 （在任何平台） 上创建 32 个字符的十六进制格式哈希字符串        
        /// </summary>
        /// <param name="source">需要加密的明文</param>
        /// <returns></returns>
        public static string Get32MD5One(string source) {
            using(MD5 md5Hash = MD5.Create()) {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(source));
                StringBuilder sBuilder = new StringBuilder();
                for(int i = 0;i<data.Length;i++) {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();                
            }
        }
        /// <summary>
        /// 获取16位md5加密
        /// </summary>
        /// <param name="source">需要加密的明文</param>
        /// <returns></returns>
        public static string Get16MD5One(string source) {
            using(MD5 md5Hash = MD5.Create()) {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(source));
                //转换成字符串，并取9到25位
                string sBuilder = BitConverter.ToString(data,4,8);
                //BitConverter转换出来的字符串会在每个字符中间产生一个分隔符，需要去除掉
                sBuilder=sBuilder.Replace("-","");
                return sBuilder.ToString();
            }
        }
        /// <summary>
        ///返回32位加密结果，该结果取32位加密结果的第9位到25位
        /// </summary>
        /// <param name="strSource">需要加密的明文</param>
        /// <returns>返回32位加密结果，该结果取32位加密结果的第9位到25位</returns>
        public static string Get32MD5Two(string source) {
            MD5 md5 = new MD5CryptoServiceProvider();
            //获取密文字节数组
            byte[] bytResult = md5.ComputeHash(Encoding.Default.GetBytes(source));
            //转换成字符串，32位
            string strResult = BitConverter.ToString(bytResult);
            //BitConverter转换出来的字符串会在每个字符中间产生一个分隔符，需要去除掉
            return strResult.Replace("-","");            
        }
        /// <summary>
        /// 返回16位加密结果，该结果取32位加密结果的第9位到25位
        /// </summary>
        /// <param name="strSource">需要加密的明文</param>
        /// <returns>返回16位加密结果，该结果取32位加密结果的第9位到25位</returns>
        public static string Get16MD5Two(string source) {
            MD5 md5 = new MD5CryptoServiceProvider();
            //获取密文字节数组
            byte[] bytResult = md5.ComputeHash(Encoding.Default.GetBytes(source));
            //转换成字符串，并取9到25位
            string strResult = BitConverter.ToString(bytResult,4,8);
            //BitConverter转换出来的字符串会在每个字符中间产生一个分隔符，需要去除掉
            return strResult.Replace("-","");            
        }
    }
}
