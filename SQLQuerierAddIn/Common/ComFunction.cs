using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;

namespace SQLQuerierAddIn.Common
{
    public class ComFunction
    {

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public ComFunction()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #endregion

        /// <summary>
        /// 返回指定的 System.String 资源的值。
        /// <para>封装ComponentResourceManager.GetString</para>
        /// </summary>
        /// <param name="t">一个 System.Type，System.ComponentModel.ComponentResourceManager 将从其中派生所有用于查找资源文件的信息。</param>
        /// <param name="name">要获取的资源名。</param>
        /// <returns>针对调用方的当前区域性设置而本地化的资源的值。如果不可能有匹配项，则返回string.Empty。</returns>
        public static string GetResxString(Type t, string name)
        {
            string resxVal = "";

            try
            {
                System.ComponentModel.ComponentResourceManager rm = new System.ComponentModel.ComponentResourceManager(t);
                resxVal = rm.GetString(name);
                if (string.IsNullOrEmpty(resxVal))
                    resxVal = "No Corresponding Message."; //string.Empty;
            }
            catch (Exception)
            {
                resxVal = "No Corresponding Message."; //string.Empty;
            }
            
            return resxVal;
        }

        /// <summary>
        /// 返回指定的 System.String 资源的值。
        /// <para>封装ComponentResourceManager.GetString</para>
        /// </summary>
        /// <param name="t">一个 System.Type，System.ComponentModel.ComponentResourceManager 将从其中派生所有用于查找资源文件的信息。</param>
        /// <param name="name">要获取的资源名。</param>
        /// <param name="nullVal">资源值返回null时的替代值</param>
        /// <returns>针对调用方的当前区域性设置而本地化的资源的值。如果不可能有匹配项，则返回string.Empty。</returns>
        public static string GetResxString(Type t, string name, string nullVal)
        {
            string resxVal = "";

            try
            {
                System.ComponentModel.ComponentResourceManager rm = new System.ComponentModel.ComponentResourceManager(t);
                resxVal = rm.GetString(name);
                if (string.IsNullOrEmpty(resxVal))
                    resxVal = nullVal;
            }
            catch (Exception)
            {
                resxVal = nullVal;
            }

            return resxVal;
        }

        /// <summary>
        /// 获取程序主版本信息，格式：Vx.x
        /// </summary>
        /// <returns></returns>
        public static string GetAppEdition()
        {
            string ver = "";
            AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName(); ;
            Version version = assemblyName.Version;
            ver = "V" + version.Major.ToString() + "." + version.Minor.ToString();

            return ver;
        } 

    }
}
