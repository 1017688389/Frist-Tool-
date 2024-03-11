using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace ToolModel
{
    public class ConmFunction
    {
        #region 单例构造限制
        private static ConmFunction _conmmonFuction;
        private ConmFunction()
        {
        }
        public static ConmFunction GetInstance()
        {
            if(_conmmonFuction==null)
            {
                _conmmonFuction = new ConmFunction();
            }
            return _conmmonFuction;
        }
        #endregion 单例构造限制

        #region 可用参变量
        public string _tmpPath;
        public string TmpPath
        {
            get
            {
                if (string.IsNullOrEmpty(_tmpPath))
                {
                    _tmpPath = Directory.GetCurrentDirectory()+"\\WorkPlace";
                }
                return _tmpPath;
            }
            private set
            {
                _tmpPath = value;
            }
        }

        #endregion 可用参变量

        #region 常用方法

        /// <summary>
        /// 打开浏览器
        /// </summary>
        /// <param name="exeName"></param>
        /// <param name="fileUrl"></param>
        public static void CallProcessRun(string exeName, string fileUrl)
        {
            Process pro = new Process();
            pro.StartInfo.FileName = exeName;
            pro.StartInfo.Arguments = fileUrl;
            pro.Start();
        }


        #endregion
    }
}
