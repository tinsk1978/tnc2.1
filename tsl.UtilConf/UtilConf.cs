using System.IO;
using Microsoft.Extensions.Configuration;

namespace tsl {
    /// <summary>
    /// 实用程序配置类
    /// </summary>
    public class UtilConf {
        /*
         * appsetting.json
         * {
         *  "ConnectionStrings": {
         *      "con": "connection string"
         *  }
         * }
         * 
         * config.GetConnectionString("con");
         * 
         */

        private static IConfiguration config;
        public static IConfiguration Configuration//加载配置文件
        {
            get {
                if(config!=null) return config;
                config=new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json",optional: true,reloadOnChange: true)
                    .Build();
                return config;
            }
            set => config=value;
        }
        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <returns></returns>
        public static string GetConnectionString(string name) {
            return config.GetConnectionString(name);
        }
    }
}
