using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;

namespace tsl.mysql {
    public class Parameter {
        private readonly List<MySqlParameter> li;

        #region ** 初始化 **
        /// <summary>
        /// 构造函数
        /// </summary>
        public Parameter() {
            li=new List<MySqlParameter>();
        }
        /// <summary>
        /// 单个参数的构造函数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        public Parameter(string name,object value) {
            li=new List<MySqlParameter>();
            this.Add(name,value);
        }
        #endregion

        #region ** 属性 **         
        /// <summary>
        ///长度 
        /// </summary>
        public int Length {
            get { return li.Count; }
        }
        /// <summary>
        ///索引 
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public MySqlParameter this[int k] {
            get {
                if(li.Contains(li[k])) {
                    MySqlParameter parm = li[k];
                    return parm;
                } else {
                    return null;
                }
            }
        }
        /// <summary>
        /// 参数数组
        /// </summary>
        public MySqlParameter[] Param {
            get {
                int count = li.Count;
                if(count>0) {
                    MySqlParameter[] p=new MySqlParameter[count];
                    li.CopyTo(p);
                    return p;
                } else {
                    return null;
                }
            }
        }
        #endregion

        #region ** 添加参数 **
        /// <summary>
        ///添加 Input 类型参数 
        /// </summary>
        /// <param name="sName">参数名称</param>
        /// <param name="sValue">参数值</param>
        public void Add(string sName,object sValue) {
            li.Add(new MySqlParameter() {
                ParameterName =sName.Trim(),
                Value         =sValue??DBNull.Value,
                Direction     =ParameterDirection.Input,
            });
        }
        /// <summary>
        /// 添加 Input 类型参数
        /// </summary>
        /// <param name="sName">参数名称</param>
        /// <param name="sDbType">参数类型</param>
        /// <param name="size">参数大小</param>
        /// <param name="sValue">参数值</param>
        public void Add(string sName,MySqlDbType sDbType,int size,object sValue) {
            MySqlParameter param = size>0
                ? new MySqlParameter(sName.Trim(),sDbType,size)
                : new MySqlParameter(sName.Trim(),sDbType);
            param.Value     =sValue??DBNull.Value;
            param.Direction =ParameterDirection.Input;
            li.Add(param);
        }
        /// <summary>
        ///添加 @Result的Output 类型参数，整形
        /// </summary>
        public void AddOut() {
            AddOut("@Result",MySqlDbType.Int32,4);
        }
        /// <summary>
        /// 添加 Output 类型参数
        /// </summary>
        /// <param name="sName">参数名称</param>
        /// <param name="sDbType">参数类型</param>
        /// <param name="iSize">参数大小</param>
        public void AddOut(string sName,MySqlDbType sDbType,int iSize) {
            li.Add(new MySqlParameter() {
                ParameterName =sName,
                MySqlDbType   =sDbType,
                Size          =iSize,
                Direction     =ParameterDirection.Output,
            });
        }
        /// <summary>
        /// 添加 InputOutput 类型参数
        /// </summary>
        /// <param name="sName">名称</param>
        public void AddInputOutput(string sName) {
            li.Add(new MySqlParameter() {
                ParameterName =sName,
                Direction     =ParameterDirection.InputOutput,
            });
        }
        /// <summary>
        /// 添加 InputOutput 类型参数
        /// </summary>
        /// <param name="sName">名称</param>
        /// <param name="sDbType">数据类型</param>
        /// <param name="iSize">大小</param>
        public void AddInputOutput(string sName,MySqlDbType sDbType,int iSize) {
            li.Add(new MySqlParameter() {
                ParameterName =sName,
                MySqlDbType   =sDbType,
                Size          =iSize,
                Direction     =ParameterDirection.InputOutput,
            });
        }
        #endregion      

        #region ** 清空参数集合 **
        /// <summary>
        /// 清空参数集合
        /// </summary>
        public void Clear() {
            li.Clear();
        }
        #endregion

    }
}
