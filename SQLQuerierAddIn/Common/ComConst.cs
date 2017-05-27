using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLQuerierAddIn.Common
{
    public class ComConst
    {
        public const int SUCCEED = 0;
        public const int FAILED = -1;
        public const int CHECK_ERROR = -2;
        public const int TRIGGER_ERROR = -2;
        public const int ROUTE_SET_ERROR = -3;
        public const int LOOP_ERROR = -4;

        //
        // 言語の定数
        //
        #region : 言語の定数
        /// <summary>
        /// 言語の定数
        /// </summary>
        public enum LangId
        {
            Japanese = 1,
            English = 2,
            Chinese = 3,
            Local = 4
        }
        #endregion

        //
        // ＤＢ関連の定数宣言
        //
        public const int DB_SEL = 0;
        public const int DB_NEXT = 1;
        public const int DB_CLOSE = 2;
        public const int DB_SELC = 3;

        public const int DB_RTN = 0;
        public const int DB_IN = 1;
        public const int DB_INOUT = 2;
        public const int DB_OUT = 3;

        public class XLstatusbar
        {
            public const string strlangset = "Langauge set Initializing...";
            public const string strReady = "Ready";
            public const string strDataGet = "Data searching...";
            public const string strDataUpd = "Data updating...";

        }
        //
        // Login関連の定数
        //
        public const int PWD_CHG = -1;
        public const int PWD_UNMATCH = -2;
        public const int USER_LOCKED = -3;
        public const int USER_UNMATCH = -4;
        public const int PWD_NO_CHG = -5;
        public const int PWD_MIN_LEN_ERR = -6;
        public const int SYS_PARA_ERR = -7;
        public const int PWD_NO_CHG1 = -8;
        public const int PWD_NO_CHG2 = -9;
        public const int PWD_NO_CHG3 = -10;

        //
        // DB更新エラー関連の定数
        //
        public const int UNABLE_TO_DELETE = -2;


        //
        // 申請フォーム用の定数
        //
        #region : 申請フォーム用の定数
        /// <summary>
        /// コントロール種別
        /// </summary>
        public enum Control_Type
        {
            Label = 0,
            TextBox = 1,
            DropDownList = 2,
            CheckBox = 3,
            DateText = 4,
            AttachedFile = 5
        }

        /// <summary>
        /// 文字種類
        /// </summary>
        public enum Character_Type
        {
            Character = 0,
            Integer = 1,
            Decimal = 2
        }

        /// <summary>
        /// 入力タイミング
        /// </summary>
        public enum Input_Timing
        {
            Request = 0,            // 申請
            Apploval = 1,           // 承認
            Both = 2,               // 両方
            Inquiry = 3,            // 照会
            ItemMake = 9            // 申請フォーム定義
        }

        /// <summary>
        /// DropDownList編集タイプ
        /// </summary>
        public enum Ddl_Edit_Type
        {
            Fixed = 0,              // 固定値
            Key01 = 1,              // SystemParameter Key01
            ExternalDb = 2          // 外部DB定義
        }


        /// <summary>
        /// Labelタイプ
        /// </summary>
        public enum Label_Type
        {
            Caption = 0,            //見出し
            Value = 1               //値
        }

        /// <summary>
        /// コントロールのポジション種別
        /// </summary>
        public class Control_Position
        {
            public const string STATIC = "static";        //既定値：本来の位置
            public const string RELATIVE = "relative";    //本来の位置からの相対位置指定
            public const string ABSOLUTE = "absolute";    //絶対位置指定
            public const string INHERIT = "inherit";      //継承

        }

        /// <summary>
        /// スタイルプロパティの単位種別[文字列]
        /// </summary>
        public class UnitType_String
        {
            public const string PERCENTAGE = "%";       //親要素に対して相対的な比率で測定します。  
            public const string PIXEL = "px";           //ピクセル単位で測定します。  
            public const string AUTO = "auto";          //自動：既定値
            public const string INHERIT = "inherit";    //継承
        }

        /// <summary>
        /// 区切り文字
        /// </summary>
        public class Delimiter
        {
            public const string LF = "\n";
            public const string CRLF = "\r\n";
            public const string TAB = "\t";
            public const string COMMA = ",";
        }

        /// <summary>
        /// 外部DB連携：Parameter、Value設定 最大数
        /// </summary>
        public const int SQL_PARAMETER_MAX = 100;
        public const int SQL_VALUE_MAX = 100;

        /// <summary>
        /// 式定義：設定最大数
        /// </summary>
        public const int ITEM_FORMURA_NO_MAX = 5;
        public const int ITEM_FORMURA_SEQ_MAX = 100;

        /// <summary>
        /// トリガー設定：Parameter
        /// </summary>
        public const int TRIGGER_MAX = 100;
        public const int TRIGGER_PARAMETER_MAX = 100;

        public class Trigger_Type
        {
            public const int REQUEST_INPUT = 0;     //申請入力
            public const int REQUEST_CONFIRM = 1;   //申請確認
            public const int APPLOVAL = 2;          //承認
            public const int LAST_APPLOVAL = 3;     //最終承認
            public const int LAST_CIRCULAR = 4;     //最終回覧
            public const int REJECT = 5;            //却下
            public const int PASS_BACK = 6;         //差戻し
        }

        /// <summary>
        /// ＳＱＬ区分：外部ＤＢ連携
        /// </summary>
        public class Sql_Type
        {
            public const int SELECT = 0;
            public const int INSERT = 1;
            public const int UPDATE = 2;
            public const int DELETE = 3;
            public const int FUNCTION = 4;
        }

        /// <summary>
        /// 承認経路条件設定 最大数
        /// </summary>
        public const int CONDITION_PAGESIZE = 3;
        public const int CONDITION_MAX = 30;
        public const int CONDITION_LINESIZE = 5;
        public const int CONDITION_COL_MAX = 150;
        #endregion

        //
        // 申請用の定数 
        //
        #region : 申請データ用の定数
        /// <summary>
        /// 申請項目[REQUEST_ITEM.VALUE_TYPE]
        /// </summary>
        public enum Value_Type
        {
            Numeric = 0,        //数値
            Literal = 1,        //文字列
            AttachedFile = 2    //添付
        }
        /// <summary>
        /// 申請項目[REQUEST_ITEM.ATTACHED_FILE_TYPE]
        /// </summary>
        public enum Attached_File_Type
        {
            Outside = 0,        //外部ファイル
            Database = 1        //DB格納
        }
        /// <summary>
        /// DataBaseを更新する際の区分[画面遷移時・登録時で使用]
        /// 
        /// 使用画面：AppRequest、AppRequest_Detailなど
        /// 使用処理：RequestPageクラス
        /// </summary>
        public enum DataBase_UpdateMode
        {
            Insert = 0,
            Update = 1,
            Delete = 2
        }
        /// <summary>
        /// 申請ステータス定数
        /// </summary>
        public class Request_Status
        {
            public const int TEMP_SAVE = 1;             //一時保存
            public const int WAIT_FOR_APPROVAL = 10;    //承認待ち
            public const int PASS_BACK = 70;            //差戻し
            public const int PASS_BACK_MID = 75;        //途中差戻し
            public const int APPROVED = 80;             //承認
            public const int CLOSE = 90;                //Close
            public const int WITHDRAWAL = 98;           //取下げ
            public const int REJECTED = 99;             //却下
        }
        #endregion

        //
        // 承認経路用の定数
        //
        #region : 承認経路用の定数
        /// <summary>
        /// カラム位置
        /// </summary>
        public enum AppRouteDef_Col
        {
            AddRow = 0,
            Delete = 1,
            Split = 2,
            Approver_Type = 3,
            Role = 4,
            UserId = 5,
            UserName = 6,
            SeqNo = 7,
            RouteChg = 8
        }
        /// <summary>
        /// 画面処理用の定数
        /// </summary>
        public class AppRouteDef_Const
        {
            public const int MaxLevel = 100;        //最大レベル  
            public const int ColBlockSize = 9;      //1ブロックのカラム数 
            public const int RowBlockSize = 3;      //1ブロックの行数
            public const int MaxCol = 4;            //画面最大カラムブロック数
        }

        /// <summary>
        /// エラー用の定数
        /// </summary>
        public class AppRouteDef_Error
        {
            public const int INPUT_ROLE_OR_USER = -2;       //ロールまたはユーザーを指定してください  
            public const int BOTH_ROLE_AND_USER = -3;       //ロール・ユーザーの両方は指定できません  
            public const int USER_NOT_EXIST = -4;           //ユーザーが存在しません  
        }
        #endregion

        //
        // 申請時の承認経路設定用の定数
        //
        #region : 申請時の承認経路設定用の定数
        /// <summary>
        /// カラム位置
        /// </summary>
        public enum AppRoute_Col
        {
            Approver_Type = 0,
            Role = 1,
            UserName = 2,
            SeqNo = 3
        }
        /// <summary>
        /// 画面処理用の定数
        /// </summary>
        public class AppRoute_Const
        {
            public const int MaxLevel = 100;        //最大レベル  
            public const int ColBlockSize = 4;      //1ブロックのカラム数 
            public const int RowBlockSize = 3;      //1ブロックの行数
            public const int MaxCol = 4;            //画面最大カラムブロック数
        }

        #endregion

        //
        // 承認時の承認経路表示用の定数
        //
        #region : 承認時の承認経路表示用の定数
        /// <summary>
        /// カラム位置
        /// </summary>
        public enum Route_Col
        {
            Approver_Type = 0,
            Role = 1,
            UserName = 2,
            ApproveFlag = 3,
            Comment = 4
        }
        /// <summary>
        /// 画面処理用の定数
        /// </summary>
        public class Route_Const
        {
            public const int MaxLevel = 100;        //最大レベル  
            public const int ColBlockSize = 5;      //1ブロックのカラム数 
            public const int RowBlockSize = 3;      //1ブロックの行数
            public const int MaxCol = 4;            //画面最大カラムブロック数
        }

        #endregion


        //
        // メール関連の定数
        //
        public class Mail
        {
            /// <summary>
            /// エンコーディング
            /// </summary>
            public class BodyEncoding
            {
                public const string None = "";            // 指定なし＝UTF-8
                public const string Shift_jis = "shift_jis";        // 日本語 シフトJIS
                public const string Euc_jp = "euc-jp";              // 日本語 EUC
                public const string Iso_2022_jp = "iso-2022-jp";    // 日本語 JIS
                public const string CsISO2022JP = "csISO2022JP";    // 日本語 JIS(1バイトカタカナ可)
                public const string Utf_8 = "utf-8";                // UTF-8
            }
            /// <summary>
            /// フォーマット形式
            /// </summary>
            public class BodyFormat
            {
                public const string None = "";            // 指定なし
                public const string Text = "TEXT";        // TEXT
                public const string Html = "HTML";        // HTML
            }
            /// <summary>
            /// 優先順位
            /// </summary>
            public class Prioryty
            {
                public const string High = "HIGH";
                public const string Low = "LOW";
                public const string Normal = "NORMAL";
            }

        }



        public ComConst()
        {
            // 
            // TODO: コンストラクタ ロジックをここに追加してください。
            //
        }



    }
}
