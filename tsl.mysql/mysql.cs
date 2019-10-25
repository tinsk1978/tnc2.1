using System;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace tsl.mysql {
    /// <summary>
    /// MySql 类库
    /// </summary>
    public class Helper {
        #region --定义变量--
        /// <summary>
        /// 数据库连接串
        /// </summary>
        public string dsn;
        /// <summary>
        ///默认实例
        /// </summary>
        public static Helper SqlDSN { get { return new Helper(); } }
        #endregion

        #region --构造函数--
        /// <summary>
        /// 构造函数
        /// </summary>
        public Helper() : this("SqlDSN") { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="strDSN">数据库连接串名</param>
        public Helper(string strDSN) {
            dsn=UtilConf.GetConnectionString(strDSN);
            string encrypt = UtilConf.GetConnectionString("encrypt");
            if(encrypt=="true") {//解密
                dsn=Crypto.DES.Decrypt(dsn);
            }
        }
        #endregion

        #region ** 打开/关闭链接 **
        /// <summary>
        /// 打开链接
        /// </summary>
        /// <param name="comd"></param>
        private void ConnOpen(ref MySqlCommand comd) {
            if(comd.Connection.State==ConnectionState.Closed)
                comd.Connection.Open();
        }
        /// <summary>
        /// 关闭链接
        /// </summary>
        /// <param name="comd"></param>
        private void ConnClose(ref MySqlCommand comd) {
            if(comd.Connection.State==ConnectionState.Open) {
                comd.Connection.Close();
            }
            comd.Dispose();
        }
        #endregion

        #region ** 创建 MySqlCommand 对象 **
        /// <summary>
        /// 根据存储过程名,生成MySqlCommand对象
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <returns></returns>
        public MySqlCommand ProcComd(string spName) {
            try {
                MySqlCommand comd;
                using(MySqlConnection conn = new MySqlConnection(dsn)) {
                    comd=conn.CreateCommand();
                }
                comd.CommandText=spName;
                comd.CommandType=CommandType.StoredProcedure;
                return comd;
            } catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 根据存储过程名和参数，生成MySqlCommand对象
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="p">过程参数</param>
        /// <returns></returns>
        public MySqlCommand ProcComd(string spName,Parameter p) {
            try {
                MySqlCommand comd = ProcComd(spName);
                //comd.Parameters.AddRange(p.Param);
                int len = p.Length;
                if(len>0) {
                    for(int i = 0;i<len;i++) {
                        comd.Parameters.Add(p[i]);
                    }
                }
                return comd;
            } catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 根据SQL语句，生成MySqlCommand对象
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns></returns>
        public MySqlCommand SqlComd(string strSql) {
            try {
                MySqlCommand comd;
                using(MySqlConnection conn = new MySqlConnection(dsn)) {
                    comd=conn.CreateCommand();
                }
                comd.CommandText=strSql;
                comd.CommandType=CommandType.Text;
                return comd;
            } catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 根据SQL语句和参数，生成MySqlCommand对象
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="p">参数</param>
        /// <returns></returns>
        public MySqlCommand SqlComd(string strSql,Parameter p) {
            try {
                MySqlCommand comd = SqlComd(strSql);

                int len = p.Length;
                if(len>0) {
                    for(int i = 0;i<len;i++) {
                        comd.Parameters.Add(p[i]);
                    }
                }
                return comd;
            } catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region ** 创建 SqlDataAdapter 对象 **
        /// <summary>
        /// 根据存储过程名，生成MySqlDataAdapter对象
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <returns></returns>
        public MySqlDataAdapter ProcAdapter(string spName) {
            try {
                MySqlConnection conn = new MySqlConnection(dsn);
                MySqlDataAdapter comdAdapter = new MySqlDataAdapter(spName,conn);
                comdAdapter.SelectCommand.CommandType=CommandType.StoredProcedure;
                return comdAdapter;
            } catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 根据存储过程名和参数，生成MySqlDataAdapter对象
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="p">过程参数</param>
        public MySqlDataAdapter ProcAdapter(string spName,Parameter p) {
            try {
                MySqlDataAdapter comdAdapter = ProcAdapter(spName);

                int len = p.Length;
                if(len>0) {
                    for(int i = 0;i<len;i++) {
                        comdAdapter.SelectCommand.Parameters.Add(p[i]);
                    }
                }
                return comdAdapter;
            } catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 根据SQL语句,生成MyDataAdapter对象
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        public MySqlDataAdapter SqlAdapter(string strSql) {
            try {
                MySqlConnection conn = new MySqlConnection(dsn);
                MySqlDataAdapter apter = new MySqlDataAdapter(strSql,conn);
                apter.SelectCommand.CommandType=CommandType.Text;
                return apter;
            } catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 根据SQL语句和参数,生成MyDataAdapter对象
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <param name="p">参数</param>
        public MySqlDataAdapter SqlAdapter(string strSql,Parameter p) {
            try {
                MySqlDataAdapter apter = SqlAdapter(strSql);
                int len = p.Length;
                if(len>0) {
                    for(int i = 0;i<len;i++) {
                        apter.SelectCommand.Parameters.Add(p[i]);
                    }
                }
                return apter;
            } catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 根据SQL语句,生成用于更新的MyDataAdapter对象
        /// </summary>
        /// <param name="sqlUpdate">Sql语句</param>
        public MySqlDataAdapter UpdateAdapter(string sqlUpdate) {
            try {
                MySqlConnection conn = new MySqlConnection(dsn);
                MySqlDataAdapter apter = new MySqlDataAdapter {
                    UpdateBatchSize=0,
                    UpdateCommand=new MySqlCommand(sqlUpdate,conn) {
                        CommandType=CommandType.Text,
                        UpdatedRowSource=UpdateRowSource.None
                    }
                };
                return apter;
            } catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 根据SQL语句和参数,生成用于更新的MyDataAdapter对象
        /// </summary>
        /// <param name="sqlUpdate">Sql语句</param>
        /// <param name="p">参数</param>
        public MySqlDataAdapter UpdateAdapter(string sqlUpdate,Parameter p) {
            try {
                MySqlDataAdapter apter = UpdateAdapter(sqlUpdate);
                int len = p.Length;
                if(len>0) {
                    for(int i = 0;i<len;i++) {
                        apter.UpdateCommand.Parameters.Add(p[i]);
                    }
                }
                return apter;
            } catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 根据SQL语句,生成用于插入的MyDataAdapter对象
        /// </summary>
        /// <param name="sqlInsert">Sql语句</param>
        public MySqlDataAdapter InsertAdapter(string sqlInsert) {
            try {
                MySqlConnection conn = new MySqlConnection(dsn);
                MySqlDataAdapter apter = new MySqlDataAdapter {
                    UpdateBatchSize=0,
                    InsertCommand=new MySqlCommand(sqlInsert,conn) {
                        UpdatedRowSource=UpdateRowSource.None,
                        CommandType=CommandType.Text
                    }
                };
                return apter;
            } catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 根据SQL语句和参数,生成用于插入的MyDataAdapter对象
        /// </summary>
        /// <param name="sqlInsert">Sql语句</param>
        /// <param name="p">参数</param>
        public MySqlDataAdapter InsertAdapter(string sqlInsert,Parameter p) {
            try {
                MySqlDataAdapter apter = InsertAdapter(sqlInsert);
                int len = p.Length;
                if(len>0) {
                    for(int i = 0;i<len;i++) {
                        apter.InsertCommand.Parameters.Add(p[i]);
                    }
                }
                return apter;
            } catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 根据SQL语句,生成用于删除的MyDataAdapter对象
        /// </summary>
        /// <param name="sqlDelete">Sql语句</param>
        public MySqlDataAdapter DeleteAdapter(string sqlDelete) {
            try {
                MySqlConnection conn = new MySqlConnection(dsn);
                MySqlDataAdapter apter = new MySqlDataAdapter {
                    UpdateBatchSize=0,
                    DeleteCommand=new MySqlCommand(sqlDelete,conn) {
                        UpdatedRowSource=UpdateRowSource.None,
                        CommandType=CommandType.Text
                    }
                };
                return apter;
            } catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 根据SQL语句和参数,生成用于删除的MyDataAdapter对象
        /// </summary>
        /// <param name="sqlDelete">Sql语句</param>
        /// <param name="p">参数</param>
        public MySqlDataAdapter DeleteAdapter(string sqlDelete,Parameter p) {
            try {
                MySqlDataAdapter apter = DeleteAdapter(sqlDelete);
                int len = p.Length;
                if(len>0) {
                    for(int i = 0;i<len;i++) {
                        _=apter.DeleteCommand.Parameters.Add(p[i]);
                    }
                }
                return apter;
            } catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region ** 创建 DataReader 对象 **
        /// <summary>
        /// 根据存储过程生成生MySqlDataReader
        /// </summary>
        /// <param name="spName">存储过程名</param>
        public MySqlDataReader ProcExecuteReader(string spName) {
            MySqlCommand comd = ProcComd(spName);
            return GetDataReader(comd);
        }
        /// <summary>
        /// 根据存储过程和参数生成SqlDataReader
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="p">过程参数</param>
        public MySqlDataReader ProcExecuteReader(string spName,Parameter p) {
            MySqlCommand comd = ProcComd(spName,p);
            return GetDataReader(comd);
        }
        /// <summary>
        /// 根据SQL语句生成SqlDataReader
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        public MySqlDataReader SqlExecuteReader(string strSql) {
            MySqlCommand comd = SqlComd(strSql);
            return GetDataReader(comd);
        }
        /// <summary>
        /// 根据SQL语句和参数生成SqlDataReader
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <param name="p">参数</param>
        public MySqlDataReader SqlExecuteReader(string strSql,Parameter p) {
            MySqlCommand comd = SqlComd(strSql,p);
            return GetDataReader(comd);
        }
        /// <summary>
        /// 获取DataReader
        /// </summary>
        /// <param name="comd"></param>
        /// <returns></returns>
        private MySqlDataReader GetDataReader(MySqlCommand comd) {
            try {
                ConnOpen(ref comd);
                return comd.ExecuteReader(CommandBehavior.CloseConnection);
            } catch(Exception ex) {
                ConnClose(ref comd);
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region ** 创建 DataTable 对象 **
        /// <summary>
        /// 根据存储过程创建 DataTable 
        /// </summary>
        /// <param name="spName">过程名称</param>
        public DataTable ProcQueryTable(string spName) {
            MySqlDataAdapter adapter = ProcAdapter(spName);
            return GetDataTable(adapter);
        }
        /// <summary>
        /// 根据存储过程和参数创建 DataTable 
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="p">过程参数</param>
        public DataTable ProcQueryTable(string spName,Parameter p) {
            MySqlDataAdapter adapter = ProcAdapter(spName,p);
            return GetDataTable(adapter);
        }
        /// <summary>
        /// 根据SQL语句,创建DataTable
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        public DataTable QueryTable(string strSql) {
            MySqlDataAdapter adapter = SqlAdapter(strSql);
            return GetDataTable(adapter);
        }
        /// <summary>
        /// 根据SQL语句和参数,创建DataTable
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="p">参数</param>
        public DataTable QueryTable(string strSql,Parameter p) {
            MySqlDataAdapter adapter = SqlAdapter(strSql,p);
            return GetDataTable(adapter);
        }
        private DataTable GetDataTable(MySqlDataAdapter adapter) {
            try {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            } catch(Exception ex) {
                throw new Exception(ex.Message);
            } finally {
                if(adapter.SelectCommand.Connection.State==ConnectionState.Open) {
                    adapter.SelectCommand.Connection.Close();
                }
                adapter.Dispose();
            }
        }
        #endregion

        #region ** 创建 DataSet 对象 **
        /// <summary>
        /// 根据存储过程创建 DataSet 
        /// </summary>
        /// <param name="spName">过程名称</param>
        public DataSet ProcQuery(string spName) {
            MySqlDataAdapter adapter = ProcAdapter(spName);
            return GetDataSet(adapter);
        }
        /// <summary>
        /// 根据存储过程和参数创建 DataSet 
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="p">过程参数</param>
        public DataSet ProcQuery(string spName,Parameter p) {
            MySqlDataAdapter adapter = ProcAdapter(spName,p);
            return GetDataSet(adapter);
        }
        /// <summary>
        /// 根据SQL语句,创建DataSet
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        public DataSet Query(string strSql) {
            MySqlDataAdapter adapter = SqlAdapter(strSql);
            return GetDataSet(adapter);
        }
        /// <summary>
        /// 根据SQL语句和参数,创建DataSet
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="p">参数</param>
        public DataSet Query(string strSql,Parameter p) {
            MySqlDataAdapter adapter = SqlAdapter(strSql,p);
            return GetDataSet(adapter);
        }
        private DataSet GetDataSet(MySqlDataAdapter adapter) {
            try {
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            } catch(Exception ex) {
                throw new Exception(ex.Message);
            } finally {
                if(adapter.SelectCommand.Connection.State==ConnectionState.Open) {
                    adapter.SelectCommand.Connection.Close();
                }
                adapter.Dispose();
            }
        }
        #endregion

        #region ** 创建 Scalar 对象 **
        /// <summary>
        /// 根据存储过程名创建 Scalar 对象,并返回结果集的第一列第一行的值
        /// </summary>
        /// <param name="spName">存储过程名</param>
        public object ProcExecuteScalar(string spName) {
            MySqlCommand comd = ProcComd(spName);
            return GetScalar(comd);
        }
        /// <summary>
        /// 根据存储过程名和参数创建 Scalar 对象,并返回结果集的第一列第一行的值
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="p">过程参数</param>
        public object ProcExecuteScalar(string spName,Parameter p) {
            MySqlCommand comd = ProcComd(spName,p);
            return GetScalar(comd);
        }
        /// <summary>
        /// 根据SQL语句，创建Scalar对象,并返回结果集的第一列第一行的值
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        public object SqlExecuteScalar(string strSql) {
            MySqlCommand comd = SqlComd(strSql);
            return GetScalar(comd);
        }
        /// <summary>
        /// 根据SQL语句和参数，创建Scalar对象,并返回结果集的第一列第一行的值
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="p">参数</param>
        public object SqlExecuteScalar(string strSql,Parameter p) {
            MySqlCommand comd = SqlComd(strSql,p);
            return GetScalar(comd);
        }
        /// <summary>
        /// 创建Scalar对象,,并返回结果集的第一列第一行的值
        /// </summary>
        /// <param name="comd"></param>
        /// <returns></returns>
        private object GetScalar(MySqlCommand comd) {
            try {
                ConnOpen(ref comd);
                object o = comd.ExecuteScalar();
                ConnClose(ref comd);
                return Equals(o,null)||Equals(o,DBNull.Value) ? null : o;
            } catch(Exception ex) {
                ConnClose(ref comd);
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region ** 执行数据库操作 - ExecuteNonQuery() **
        /// <summary>
        /// 执行数据库操作,并返回影响的记录数
        /// </summary>
        /// <param name="comd"></param>
        /// <returns>返回操作记录数</returns>
        private int ExecuteNonQuery(MySqlCommand comd) {
            try {
                ConnOpen(ref comd);
                int iOk = comd.ExecuteNonQuery();
                ConnClose(ref comd);
                return iOk;
            } catch(Exception ex) {
                ConnClose(ref comd);
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region ** 仅执行，不返回输出参数 **
        /// <summary>
        /// 运行存储过程,返回影响的记录数
        /// </summary>
        /// <param name="spName">存储过程名</param>
        public int RunProc(string spName) {
            MySqlCommand comd = ProcComd(spName);
            return ExecuteNonQuery(comd);
        }
        /// <summary>
        /// 运行带参数的存储过程,返回影响的记录数
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="p">过程参数</param>
        public int RunProc(string spName,Parameter p) {
            MySqlCommand comd = ProcComd(spName,p);
            return ExecuteNonQuery(comd);
        }
        /// <summary> 
        /// 执行sql语句,返回影响的记录数
        /// </summary> 
        /// <param name="sql">sql语句</param>
        public int ExecuteSql(string sql) {
            MySqlCommand comd = SqlComd(sql);
            return ExecuteNonQuery(comd);
        }
        /// <summary>
        /// 执行带参数的SQL语句,返回影响的记录数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="p">参数</param>
        /// <returns></returns>
        public int ExecuteSql(string sql,Parameter p) {
            MySqlCommand comd = SqlComd(sql,p);
            return ExecuteNonQuery(comd);
        }
        /// <summary> 
        /// 判断是否存在,执行SQL语句
        /// </summary>
        /// <param name="sql">sql语句</param>        
        public bool Exist(string sql) {
            MySqlCommand comd = SqlComd(sql);
            if(int.TryParse(GetScalar(comd).ToString(),out int iOk)) {
                return iOk>0 ? true : false;
            } else {
                return false;
            }
        }
        /// <summary> 
        /// 判断是否存在,执行带参数的SQL语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="p">参数</param>
        public bool Exist(string sql,Parameter p) {
            MySqlCommand comd = SqlComd(sql,p);
            if(int.TryParse(GetScalar(comd).ToString(),out int iOk)) {
                return iOk>0 ? true : false;
            } else {
                return false;
            }
        }
        /// <summary>
        /// 执行SQL语句,并返回第一行第一列的值字符型
        /// </summary>
        /// <param name="sql">sql语句</param>        
        /// <returns>字符型</returns>
        public string RunSql(string sql) {
            try {
                object o = SqlExecuteScalar(sql);
                return (Equals(o,null)||Equals(o,DBNull.Value)) ? null : o.ToString();
            } catch {
                return null;
            }
        }
        /// <summary>
        /// 执行带参数的SQL语句,返回第一行第一列的值字符型
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="p">参数</param>
        /// <returns></returns>
        public string RunSql(string sql,Parameter p) {
            try {
                object o = SqlExecuteScalar(sql,p);
                return (Equals(o,null)||Equals(o,DBNull.Value)) ? null : o.ToString();
            } catch {
                return null;
            }
        }
        /// <summary>
        /// 执行SQL语句,并返回第一行指定列的值,当返回列索引大于列总数时，返回第一列的值
        /// </summary>
        /// <param name="sql">sql语句</param>        
        /// <param name="index">指定列索引</param>
        /// <returns>字符型</returns>
        public string RunSql(string sql,int index) {
            try {
                using(MySqlDataReader dr = SqlExecuteReader(sql)) {
                    if(dr!=null&&dr.HasRows) {
                        dr.Read();
                        object o = dr[index>dr.FieldCount ? 0 : index];
                        dr.Close();
                        return o?.ToString();
                    } else {
                        dr.Close();
                        return null;
                    }
                }
            } catch {
                return null;
            }
        }
        /// <summary>
        /// 执行带参数的SQL语句,返回第一行指定列的值,当返回列索引大于列总数时，返回第一列的值
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="p">参数</param>
        /// <param name="index">指定列索引</param>
        /// <returns></returns>
        public string RunSql(string sql,Parameter p,int index) {
            try {
                using(MySqlDataReader dr = SqlExecuteReader(sql,p)) {
                    if(dr!=null&&dr.HasRows) {
                        dr.Read();
                        object o = dr[index>dr.FieldCount ? 0 : index];
                        dr.Close();
                        return o?.ToString();
                    } else {
                        dr.Close();
                        return null;
                    }
                }
            } catch {
                return null;
            }
        }
        /// <summary>
        /// 执行的SQL语句,返回第一行指定列集的值字符型(从0开始计数)
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="p">参数</param>
        /// <param name="count">指定列数</param>
        /// <param name="values">返回的值</param>
        public void RunSql(string sql,int count,out string[] values) {
            try {
                using(MySqlDataReader dr = SqlExecuteReader(sql)) {
                    if(dr!=null&&dr.HasRows) {
                        int length = count>dr.FieldCount ? dr.FieldCount : count;
                        values=new string[length];
                        dr.Read();
                        for(int i = 0;i<length;i++) {
                            values[i]=(Equals(dr[i],null)||Equals(dr[i],DBNull.Value)) ? "" : dr[i].ToString();
                        }
                    } else {
                        values=null;
                    }
                    dr.Close();
                }
            } catch {
                values=null;
            }
        }
        /// <summary>
        /// 执行带参数的SQL语句,返回第一行指定列集的值字符型(从0开始计数)
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="p">参数</param>
        /// <param name="count">指定列数</param>
        /// <param name="values">返回的值</param>
        public void RunSql(string sql,Parameter p,int count,out string[] values) {
            try {
                using(MySqlDataReader dr = SqlExecuteReader(sql,p)) {
                    if(dr!=null&&dr.HasRows) {
                        int length = count>dr.FieldCount ? dr.FieldCount : count;
                        values=new string[length];
                        dr.Read();
                        for(int i = 0;i<length;i++) {
                            values[i]=(Equals(dr[i],null)||Equals(dr[i],DBNull.Value)) ? "" : dr[i].ToString();
                        }
                    } else {
                        values=null;
                    }
                    dr.Close();
                }
            } catch {
                values=null;
            }
        }
        /// <summary>
        /// 多语句的运行事务        
        /// </summary>       
        /// <param name="comTexts">Sql语句数组</param>    
        /// <returns></returns>      
        public bool ExcuteCommandByTran(List<string> comTexts) {
            using(MySqlConnection con = new MySqlConnection(dsn)) {
                con.Open();
                MySqlTransaction tran = con.BeginTransaction();
                MySqlCommand cmd = new MySqlCommand {
                    Connection=con,
                    Transaction=tran
                };
                try {
                    foreach(string comText in comTexts) {
                        cmd.CommandText=comText;
                        cmd.ExecuteNonQuery();
                    }
                    tran.Commit();
                    con.Close();
                    return true;
                } catch {
                    tran.Rollback();
                    con.Close();
                    return false;
                }
            }
        }
        #endregion

        #region ** 执行存储过程并返回输出参数的值 **
        /// <summary>
        /// 执行存储过程并返回输出参数的值
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="p">过程参数</param>
        /// <param name="outParamName">返回参数名</param>
        public string ExecuteProcOut(string spName,Parameter p,string outParamName) {
            MySqlCommand comd = ProcComd(spName,p);
            try {
                ConnOpen(ref comd);
                comd.ExecuteNonQuery();
                object o = comd.Parameters[outParamName].Value;
                ConnClose(ref comd);
                return o?.ToString();
            } catch(Exception ex) {
                ConnClose(ref comd);
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 执行存储过程并返回输出参数的值：默认输出参数 @Result Varchar(50)
        /// </summary>
        /// <param name="spName">存储过程名称</param>
        /// <param name="p">过程参数</param>
        public string ExecuteProcOut(string spName,Parameter p) {
            p.AddOut("@Result",MySqlDbType.VarChar,50);
            return ExecuteProcOut(spName,p,"@Result");
        }
        #endregion

        #region ** 执行并返回输出参数的值 **
        /// <summary>
        /// 执行存储过程，并返回输出参数的值
        /// </summary>
        /// <param name="spName">存储过程名称</param>
        /// <param name="p">过程参数</param>
        /// <param name="retParam">返回参数</param>
        /// <param name="size">返回参数大小</param>
        public string ExecuteProcReturn(string spName,Parameter p,string retParam,int size = 50) {
            MySqlCommand comd = ProcComd(spName,p);
            comd.Parameters.Add(new MySqlParameter(retParam,MySqlDbType.VarChar,size));
            comd.Parameters[retParam].Direction=ParameterDirection.ReturnValue;
            try {
                ConnOpen(ref comd);
                comd.ExecuteNonQuery();
                object o = comd.Parameters[retParam].Value;
                ConnClose(ref comd);
                return o?.ToString();
            } catch(Exception ex) {
                ConnClose(ref comd);
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        ///  执行存储过程，并返回输出参数的值,默认输出参数 @ReturnValue Varchar(50)
        /// </summary>
        /// <param name="spName">存储过程名称</param>
        /// <param name="p">过程参数</param>
        /// <returns></returns>
        public string ExecuteProcReturn(string spName,Parameter p) {
            return ExecuteProcReturn(spName,p,"ReturnValue");
        }
        /// <summary> 
        /// 执行Sql语句，并返回指定参数的值
        /// </summary> 
        /// <param name="sql">Sql语句</param>
        /// <param name="p">参数</param>
        /// <param name="retParam">返回参数</param>
        /// <param name="size">返回参数大小</param>
        public string ExecuteSqlReturn(string sql,Parameter p,string retParam,int size = 50) {
            MySqlCommand comd = SqlComd(sql,p);
            comd.Parameters.Add(new MySqlParameter(retParam,MySqlDbType.VarChar,size));
            comd.Parameters[retParam].Direction=ParameterDirection.ReturnValue;
            try {
                ConnOpen(ref comd);
                comd.ExecuteNonQuery();
                object o = comd.Parameters[retParam].Value;
                ConnClose(ref comd);
                return o?.ToString();
            } catch(Exception ex) {
                ConnClose(ref comd);
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 根据Sql语句执行,默认返回参数的值 @ReturnValue VarChar（50）
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="p">参数</param>
        public string ExecuteSqlReturn(string sql,Parameter p) {
            return ExecuteSqlReturn(sql,p,"ExecuteSqlReturn");
        }
        #endregion

        #region ** 大批量导入数据 **
        /// <summary>
        /// 大批量数据插入,返回成功插入行数,通过 MySqlBulkLoader 实现
        /// </summary>
        /// <param name="table">数据源</param>
        /// <param name="tableName">数据库存放数据的表名</param>
        /// <returns>返回成功插入行数</returns>
        public int BulkInsert(DataTable table,string tableName) {
            if(table.Rows.Count==0) return 0;
            int insertCount = 0;
            string tmpPath  = System.IO.Path.GetTempFileName();
            File.Csv.DataTableToCsvFile(table,tmpPath);
            using(MySqlConnection conn = new MySqlConnection(dsn)) {
                try {
                    conn.Open();
                    MySqlBulkLoader bulk = new MySqlBulkLoader(conn) {
                        FieldTerminator         =",",
                        FieldQuotationCharacter ='"',
                        EscapeCharacter         ='"',
                        LineTerminator          ="\r\n",
                        FileName                =tmpPath,
                        NumberOfLinesToSkip     =0,
                        TableName               =tableName,
                    };
                    bulk.Columns.AddRange(table.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToList());
                    insertCount=bulk.Load();
                } catch {
                    insertCount=0;
                } finally {
                    System.IO.File.Delete(tmpPath);
                    conn.Close();
                }
            }
            return insertCount;
        }
        /// <summary>
        /// DataTable批量插入MYSQL数据库，通过拼接实现
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <param name="tableName">数据库存放数据的表名</param>
        /// <returns>返回成功插入行数</returns>
        public int InsertByDataTable(DataTable dataTable,string tableName) {
            int result = 0;
            if(null==dataTable||dataTable.Rows.Count<=0) {
                return 0;
            }
            // 构建INSERT语句
            StringBuilder sb = new StringBuilder();
            _=sb.Append("INSERT INTO "+tableName+"(");
            for(int i = 0;i<dataTable.Columns.Count;i++) {
                _=sb.Append(dataTable.Columns[i].ColumnName+",");
            }
            _=sb.Remove(sb.ToString().LastIndexOf(','),1);
            _=sb.Append(") VALUES ");
            for(int i = 0;i<dataTable.Rows.Count;i++) {
                _=sb.Append("(");
                for(int j = 0;j<dataTable.Columns.Count;j++) {
                    _=sb.Append("'"+dataTable.Rows[i][j]+"',");
                }
                _=sb.Remove(sb.ToString().LastIndexOf(','),1);
                _=sb.Append("),");
            }
            _=sb.Remove(sb.ToString().LastIndexOf(','),1);
            _=sb.Append(";");
            using(MySqlConnection con = new MySqlConnection(dsn)) {
                con.Open();
                using(MySqlCommand cmd = new MySqlCommand(sb.ToString(),con)) {
                    try {
                        result=cmd.ExecuteNonQuery();
                    } catch {
                        result=0;
                    }
                }
            }
            return result;
        }
        #endregion
    }
}
