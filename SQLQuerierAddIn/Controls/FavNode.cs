using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SQLQuerierAddIn.Controls
{
    public class FavNode:TreeNode
    {
        private string nodeid;
        private string filepath;
        private FavNodeType nodetype;
        private string parentid;

        public string NodeID
        {
            set { nodeid = value; }
            get { return nodeid; }
        }
        public string FilePath
        {
            set { filepath = value; }
            get { return filepath; }
        }
        public FavNodeType NodeType
        {
            set { nodetype = value; }
            get { return nodetype; }
        }
        public string ParentID
        {
            set { parentid = value; }
            get { return parentid; }
        }



        public FavNode()
        { 
        }
        public FavNode(string NodeName)
        {
            Text = NodeName;
        }
       
    }

    #region SQL Favorites Node Type Enums
    /// <summary>
    /// SQL Favorites Node Type Enums
    /// <para>0.Root</para>
    /// <para>1.SQL Folder</para>
    /// <para>2.SQL File</para>
    /// </summary>
    public enum FavNodeType
    {
        RootNode = 0,
        FolderNode = 1,
        FileNode = 2
    }

    #endregion

}
