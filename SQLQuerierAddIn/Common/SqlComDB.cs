using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;

namespace SQLQuerierAddIn.Common
{
    public class SqlComDB
    {
        #region : コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SqlComDB(string ConString)
        {
            //コネクション インスタンスの作成
            _cn = new SqlConnection();
            //コマンド インスタンスの作成
            _cm = new SqlCommand();

            //web.configよりコネクションストリングを取得
            //SqlCon sql = new SqlCon();
            _cn.ConnectionString = ConString;
        }
        #endregion

        #region : フィールド
        /// <summary>
        /// フィールド
        /// </summary>
        protected SqlConnection _cn;
        protected SqlCommand _cm;
        protected SqlDataReader _dr;
        protected SqlDataAdapter _da;
        protected SqlTransaction _trans;

        protected string _strErr;
        protected string _expmsg;
        #endregion

        #region : プロパティ
        /// <summary>
        /// プロパティ
        /// </summary>
        public string strErr
        {
            get { return _strErr; }
            set { _strErr = value; }
        }

        public string expmsg
        {
            get { return _expmsg; }
            set { _expmsg = value; }
        }
        #endregion

        #region : データーベース接続
        /// <summary>
        /// データーベース接続
        /// 
        /// 接続Test使用
        /// </summary>
        public int DbConnection()
        {
            try
            {
                _cn.Open();
                //return _cn;
                return 0;

            }
            catch (Exception e)
            {
                _expmsg = e.Message;
                _strErr = e.ToString();
                System.Console.Out.Write(_strErr);
                //return _cn;
                return -1;
            }

        }
        #endregion

        #region : データーベース接続の終了
        /// <summary>
        /// データーベース接続の終了
        /// </summary>
        public Boolean DbClose()
        {
            try
            {
                if (_trans != null)  // 2012/03/02
                {
                    try
                    {
                        _trans.Rollback();
                    }
                    catch { }
                    _trans = null;  //ADD 2009/3/18
                }
                _cn.Close();
                _cn.Dispose();

                return true;

            }
            catch (Exception e)
            {
                String strErr;

                strErr = e.ToString();
                System.Console.Out.Write(strErr);
                return false;
            }
        }
        #endregion

        #region: コネクション状態
        /// <summary>
        /// コネクション状態
        /// </summary>
        public ConnectionState State()
        {
            return _cn.State;
        }
        #endregion

        #region: トランザクション開始
        /// <summary>
        /// トランザクション開始
        /// </summary>
        public void DbBeginTrans()
        {

            try
            {
                if (_cn.State != ConnectionState.Open)
                    _cn.Open();
                _trans = _cn.BeginTransaction(IsolationLevel.ReadCommitted);
                _cm.Transaction = _trans;
            }
            catch (Exception e)
            {
                _expmsg = e.Message;
                _strErr = e.ToString();
            }
        }
        #endregion

        #region : コミット
        /// <summary>
        /// コミット
        /// </summary>
        public void DbCommit()
        {
            try
            {
                _trans.Commit();
                _trans = null;  //ADD 2009/3/18
                _cn.Close();
                _cn.Dispose();  //ADD 2012/3/2
            }
            catch (Exception e)
            {
                string strErr;

                strErr = e.ToString();
                System.Console.Out.Write(strErr);
            }
        }
        #endregion

        #region : ロールバック
        /// <summary>
        /// ロールバック
        /// </summary>
        public void DbRollback()
        {
            try
            {
                _trans.Rollback();
                _trans = null;  //ADD 2009/3/18
                _cn.Close();
                _cn.Dispose();  //ADD 2012/3/2
            }
            catch (Exception e)
            {
                _expmsg = e.Message;
                _strErr = e.ToString();
            }
        }
        #endregion

        #region : SQLの実行
        /// <summary>
        /// SQLの実行
        /// </summary>
        public int DbExecute(string strSql)
        {
            _cm.CommandType = CommandType.Text;
            try
            {
                int rtn;
                if (_cn.State != ConnectionState.Open)
                    _cn.Open();

                _cm.Connection = _cn;
                _cm.Transaction = _trans;   //ADD 2008/12/22
                _cm.CommandText = strSql;
                _cm.CommandTimeout = 0;     // 2010.09.24 add 0は無限

                rtn = _cm.ExecuteNonQuery();

                return rtn;
            }
            catch (Exception e)
            {
                _expmsg = e.Message;
                _strErr = e.ToString();
                return ComConst.FAILED;
            }
        }
        #endregion

        #region : データリーダ
        public int DbExecuteReader(string strSql)
        {
            try
            {
                _expmsg = "";
                _strErr = "";
                if (_cn.State != ConnectionState.Open)
                    _cn.Open();

                _cm.CommandType = CommandType.Text;
                _cm.Connection = _cn;
                _cm.CommandText = strSql;
                _cm.CommandTimeout = 0;     // 2010.09.24 add 0は無限
                //            _cm.Connection.Open();

                _dr = _cm.ExecuteReader();

                if (_dr.Read())
                {
                    return 0;
                }
                else
                {
                    return ComConst.FAILED;
                }
            }
            catch (Exception e)
            {
                _expmsg = e.Message;
                _strErr = e.ToString();
                return -1;
            }
        }
        public string Row(string p_field_name)
        {
            return _dr[p_field_name].ToString();
        }
        public int DbCloseReader()
        {
            try
            {
                _expmsg = "";
                _strErr = "";
                _dr.Close();
            }
            catch
            {
            }
            return 0;
        }
        #endregion

        public DataSet DbDataSet(string strSql, string tblName)
        {
            try
            {
                DataSet ds = new DataSet();
                _da = new SqlDataAdapter();
                _cm.CommandType = CommandType.Text;
                _cm.Connection = _cn;
                _cm.CommandTimeout = 0;     // 2010.09.24 add 0は無限
                _da.SelectCommand = _cm;

                //--- Connection Open
                if (_cn.State != ConnectionState.Open)
                    _cn.Open();

                //'--- Fill
                _cm.CommandText = strSql;
                _da.Fill(ds, tblName);

                return ds;
            }
            catch (Exception e)
            {
                _expmsg = e.Message;
                _strErr = e.ToString();
                return null;
            }
        }

        public DataTable DbDataTable(string strSql, string tblName)
        {
            try
            {
                DataTable dt = new DataTable(tblName);
                _da = new SqlDataAdapter();
                _cm.CommandType = CommandType.Text;
                _cm.Connection = _cn;
                _cm.CommandTimeout = 0;     // 2010.09.24 add 0は無限
                _da.SelectCommand = _cm;

                //--- Connection Open
                if (_cn.State != ConnectionState.Open)
                    _cn.Open();

                //'--- Fill
                _cm.CommandText = strSql;
                _da.Fill(dt);

                return dt;
            }
            catch (SqlException e)
            {
                //e.g: 消息 208，级别 16，状态 1，第 1 行
                string sqlmsg = "Error Number:" + e.Number.ToString() + "," +
                    "Class:" + e.Class.ToString() + "," +
                    "State:" + e.State.ToString() + "," +
                    "Line Number:" + e.LineNumber.ToString() + "\r\n" +
                    e.Message;
                //_expmsg = e.Message;
                _expmsg = sqlmsg;
                _strErr = e.ToString();
                return null;
            }
        }


    }
}
