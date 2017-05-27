using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Win32;
using System.Windows.Forms;

namespace SQLQuerierAddIn.Common
{
    public class ComRegistry
    {
        //注册表子项路径
        private const string SUBKEY_DBSRC = @"Software\SQLQuerierAddIn\DataSources";
        private const string SUBKEY_DBPAR = @"Software\SQLQuerierAddIn\DataSources";
        private const string SUBKEY_LASTS = @"Software\SQLQuerierAddIn";
        private const string SUBKEY_FVPAR = @"Software\SQLQuerierAddIn\SQLFavorites";

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public ComRegistry()
        {
            // 
            // TODO: 请在此处添加构造函数逻辑。
            //
        }

        #endregion

        /****************************/
        /* 基本操作					*/
        /****************************/
        #region GetDbServerList 数据源列表取得
        /// <summary>
        ///  数据源列表取得
        /// </summary>
        /// <returns>正常:数据源列表 错误:null</returns>
        public static List<string> GetDbServerList()
        {
            return GetRegSubKeys(SUBKEY_DBSRC);
        }
        #endregion

        #region GetDbSourceInfos  数据源基本信息取得
        /// <summary>
        ///  DB数据源基本信息取得
        /// </summary>
        /// <returns>正常:数据源基本信息 错误:null</returns>
        public static string GetDbSourceInfos(string SrcName)
        {
            return GetRegValue(SUBKEY_DBSRC, SrcName);
        }
        #endregion

        #region GetDbSourcePwd 数据库密码信息取得
        public static string GetDbSourcePwd(string ParKey)
        {
            //string subkey = SUBKEY_DBPAR + @"\" + ParKey + ".params";
            string subkey = SUBKEY_DBPAR + @"\" + ParKey;
            return GetRegValue(subkey, "Password");

        }
        #endregion

        #region GetLastAccessDbSource 最近一次访问的数据源取得
        public static string GetLastAccessDbSource()
        {
            return GetRegValue(SUBKEY_LASTS, "LastDbSource");

        }
        #endregion

        #region GetLastAccessDbName 最近一次访问的数据库取得
        public static string GetLastAccessDbName()
        {
            return GetRegValue(SUBKEY_LASTS, "LastDbName");

        }
        #endregion

        #region GetLastSQLQuery 最近一次查询脚本取得
        public static string GetLastSQLQuery()
        {
            return GetRegValue(SUBKEY_LASTS, "LastSQLQuery");

        }
        #endregion

        #region GetFavRootFloder SQL收藏夹根目录路径取得
        public static string GetFavRootFloder()
        {
            return GetRegValue(SUBKEY_FVPAR, "RootFloder");

        }
        #endregion


        #region SetDbSource 数据源基本信息设定
        /// <summary>
        ///  DB数据源基本信息设定
        /// </summary>
        /// <param name="SrcKey">Key</param>
        /// <param name="SrcVal">Value</param>
        /// <returns>正常:true false:例外</returns>
        public static bool SetDbSource(string SrcName, string SrcVal)
        {
            return SetRegValue(SUBKEY_DBSRC, SrcName, SrcVal);
            
        }
        #endregion

        #region SetDbSourcePwd 数据库密码信息设定
        public static bool SetDbSourcePwd(string ParKey, string ParVal)
        {
            string subkey = SUBKEY_DBPAR + @"\" + ParKey + ".params";
            return SetRegValue(subkey, "Password", ParVal);

        }
        #endregion

        #region SetLastAccessDbSource 最近一次访问的数据源设定
        public static bool SetLastAccessDbSource(string SrcVal)
        {
            return SetRegValue(SUBKEY_LASTS, "LastDbSource", SrcVal);

        }
        #endregion

        #region SetLastAccessDbName 最近一次访问的数据库设定
        public static bool SetLastAccessDbName(string SrcVal)
        {
            return SetRegValue(SUBKEY_LASTS, "LastDbName", SrcVal);

        }
        #endregion

        #region SetLastSQLQuery 最近一次查询脚本设定
        public static bool SetLastSQLQuery(string SrcVal)
        {
            return SetRegValue(SUBKEY_LASTS, "LastSQLQuery", SrcVal);

        }
        #endregion

        #region SetFavRootFloder 设定SQL收藏夹根目录路径
        public static bool SetFavRootFloder(string SrcVal)
        {
            return SetRegValue(SUBKEY_FVPAR, "RootFloder", SrcVal);

        }
        #endregion

        /****************************/
        /* 其他  					*/
        /****************************/
        #region GetRegSubKeys 注册表键关联的值名称列表取得
        /// <summary>
        /// 注册表键关联的值名称列表取得
        /// </summary>
        /// <param name="Key">注册表项</param>
        /// <returns>值名称列表</returns>
        public static List<string> GetRegSubKeys(string Key)
        {
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(Key, false);

            if (regKey == null) return null;

            string[] regstr = regKey.GetValueNames();

            regKey.Close();

            List<string> Serverlist = new List<string>(regstr);

            return Serverlist;

        }
        #endregion

        #region GetRegValue 注册表键值取得
        /// <summary>
        /// 注册表键值取得
        /// </summary>
        /// <param name="Key">注册表项</param>
        /// <param name="SubKey">子项</param>
        /// <returns>键值</returns>
        public static string GetRegValue(string Key, string SubKey)
        {
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(Key, false);

            if (regKey == null) return null;

            string regstr = (string)regKey.GetValue(SubKey);

            regKey.Close();

            return regstr;

        }
        #endregion

        #region SetRegValue 注册表键值设定
        /// <summary>
        /// 注册表键值写入
        /// </summary>
        /// <param name="Key">注册表项</param>
        /// <param name="Name">键名</param>
        /// <param name="Val">键值</param>
        public static bool SetRegValue(string Key, string Name, string Val)
        {
            bool rtn = false;

            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(Key, true);

            if (regKey == null)
            {
                regKey = Registry.CurrentUser.CreateSubKey(Key);
            }

            try
            {
                regKey.SetValue(Name, Val);

                regKey.Close();

                rtn = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return rtn;

        }
        #endregion

        #region ReplaceKeyBackslash 反斜杠替换处理
        /// <summary>
        /// 反斜杠替换处理
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string ReplaceKeyBackslash(string Key)
        {
            string newKey = Key;
            if (!string.IsNullOrEmpty(Key))
            {
                newKey = Key.Replace("\\", "/");
            }
            return newKey;
        }
        #endregion

        #region splitStringToMap 将字符串以[;][=]分割为Dictionary集合
        /// <summary>
        /// 将字符串以[;][=]分割为Dictionary集合
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Dictionary<String, String> splitStringToMap(string s)
        {
            Dictionary<String, String> map = new Dictionary<String, String>();
            String[] temp = s.Split(';'); //通过分号进行分割  
            for (int i = 0; i < temp.Length; i++)
            {
                String[] arr = temp[i].Split('='); //通过等号继续分割   
                map.Add(arr[0], arr[1]);
            }
            return map;
        }
        #endregion

    }
}
