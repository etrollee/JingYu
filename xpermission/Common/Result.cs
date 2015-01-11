using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public static class Result
    {
        /// <summary>
        /// 获取字符串类型的主键
        /// </summary>
        /// <returns></returns>
        public static string GetNewId()
        {
            return CreateNewId();
        }

        /// <summary>
        /// 创建不重复的Id
        /// </summary>
        /// <returns></returns>
        private static string CreateNewId()
        {
            string id = DateTime.Now.ToString("yyyyMMddHHmmssfffffff");
            string guid = Guid.NewGuid().ToString().Replace("-","");
            id += guid.Substring(0,10);
            return id;
        }
    }
}
