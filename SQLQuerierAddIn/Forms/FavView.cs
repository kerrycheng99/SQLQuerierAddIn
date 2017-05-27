using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Collections;
using SQLQuerierAddIn.Controls;
using SQLQuerierAddIn.Common;

namespace SQLQuerierAddIn.Forms
{
    public partial class FavView : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        private FavNode _TreeClickNode; //鼠标单击的节点
        private List<TreeNode> _NodeList = new List<TreeNode>(); //记录所有的树节点  
        private TreeNode _FindNode = null;                       //记录查找到的节点  
        private TreeNode _LastSelNode = null;                    //记录最近一次选择的节点  

        private string _RootPath = string.Empty;    //收藏夹根目录路径
        private string _FileFilter = "*.sql";       //文件类型
        private string _FavTipTxt = string.Empty;
        private string _MsgText = string.Empty;
        private string _LblText = string.Empty;

        public FavView()
        {
            InitializeComponent();
            //初始化其他补足
            _FavTipTxt = btn_RootSet.ToolTipText;
            _RootPath = GetFavRootPath();
            fbdRootFloder.SelectedPath = _RootPath;
            btn_RootSet.ToolTipText = _FavTipTxt + " " + _RootPath;

            LoadTreeview();
        }

        #region GetFavRootPath : 从注册表获取SQL收藏夹根目录路径
        /// <summary>
        /// 从注册表获取SQL收藏夹根目录路径
        /// </summary>
        /// <returns></returns>
        private string GetFavRootPath()
        {
            string rootpath = ComRegistry.GetFavRootFloder();
            //注册表未设定收藏夹根目录路径 或 注册表设定的收藏夹根目录路径不存在时
            //取用户配置文件夹
            if (string.IsNullOrEmpty(rootpath) || !Directory.Exists(rootpath))
            {
                
                //var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                rootpath = Path.Combine(path, "SQLFavorites");
                if (!Directory.Exists(rootpath))
                {
                    Directory.CreateDirectory(rootpath);
                }
            }

            return rootpath;
        }
        #endregion

        #region LoadTreeview : 初始化Treeview项
        /// <summary>
        /// 初始化Treeview项
        /// </summary>
        private void LoadTreeview()
        {
            tvFav.Nodes.Clear();
            _LblText = ComFunction.GetResxString(typeof(FavView), "TXT_FAVORITES", "Favorites");
            FavNode rootNode = new FavNode(_LblText);
            rootNode.NodeType = FavNodeType.RootNode;//Root
            rootNode.FilePath = _RootPath;
            rootNode.ImageIndex = 0;//fav.gif
            rootNode.SelectedImageIndex = 0;//fav.gif
            rootNode.Expand();
            tvFav.Nodes.Add(rootNode);

            ExpendTree(_RootPath, this.tvFav.Nodes[0]);

            this.tvFav.Nodes[0].Expand();
        }
        #endregion

        #region ExpendTree ： 递归获取指定文件夹下的所有子目录及文件并生成Tree视图
        /// <summary>
        /// 递归获取指定文件夹下的所有子目录及文件，
        /// 并生成Tree视图
        /// </summary>
        /// <param name="path"></param>
        /// <param name="node"></param>
        private void ExpendTree(string path, TreeNode node)
        {
            if (!Directory.Exists(path)) return;
            try
            {
                //目录下的文件、文件夹集合
                //string[] dirArr = System.IO.Directory.GetDirectories(path, "*", System.IO.SearchOption.AllDirectories);
                string[] dirArr = System.IO.Directory.GetDirectories(path);
                string[] rootfileArr = System.IO.Directory.GetFiles(path, _FileFilter);
                //文件夹递归
                for (int i = 0; i < dirArr.Length; i++)
                {
                    //增加父节点
                    //string dirName = Path.GetFileNameWithoutExtension(dirArr[i]);
                    string dirName = Path.GetFileName(dirArr[i]);
                    FavNode subnode = new FavNode(dirName);
                    subnode.NodeID = dirName;
                    subnode.NodeType = FavNodeType.FolderNode;//Folder
                    subnode.FilePath = dirArr[i];
                    subnode.ImageIndex = 1;
                    subnode.SelectedImageIndex = 2;
                    node.Nodes.Add(subnode);

                    ExpendTree(dirArr[i], node.Nodes[i]);
                }

                //文件直接添加
                foreach (string var in rootfileArr)
                {
                    //string filename = Path.GetFileNameWithoutExtension(var);
                    string filename = Path.GetFileName(var);
                    FavNode subnode = new FavNode(filename);
                    subnode.NodeID = filename;
                    subnode.NodeType = FavNodeType.FileNode;//File
                    subnode.FilePath = var;
                    subnode.ImageIndex = 3;
                    subnode.SelectedImageIndex = 3;
                    node.Nodes.Add(subnode);
                }
            }
            catch { }
        }
        #endregion

        #region CreatPopMenu : 创建treeview 右键弹出菜单

        private void CreatPopMenu(FavNodeType NodeType)
        {
            switch (NodeType)
            {
                case FavNodeType.RootNode:
                    tsmiOpen.Visible = false;
                    tsmiNew.Visible = true;
                    tsmiDelete.Visible = false;
                    tsmiRename.Visible = false;
                    break;
                case FavNodeType.FolderNode:
                    tsmiOpen.Visible = false;
                    tsmiNew.Visible = true;
                    tsmiDelete.Visible = true;
                    tsmiRename.Visible = true;
                    break;
                case FavNodeType.FileNode:
                    tsmiOpen.Visible = true;
                    tsmiNew.Visible = false;
                    tsmiDelete.Visible = true;
                    tsmiRename.Visible = true;
                    break;
            }
        }

        #endregion
        #region OpenNodeFile : 打开TreeView节点项所对应的文件
        /// <summary>
        /// 打开TreeView节点项所对应的文件
        /// </summary>
        private void OpenNodeFile()
        {
            try
            {
                FavNode SelNode = (FavNode)this.tvFav.SelectedNode;
                FavNodeType NodeType = SelNode.NodeType;

                string selstr = SelNode.Text;
                string filepath = SelNode.FilePath;
                if (filepath.Trim() != "")
                {
                    FavSQLBase sqleditorfrm = (FavSQLBase)Application.OpenForms["FavSQLBase"];

                    if (sqleditorfrm != null)
                    {
                        
                        //打开当前节点文件
                        if (NodeType != FavNodeType.FileNode)
                        {
                            sqleditorfrm.EmptyContents();
                            sqleditorfrm.FileNotSave = false;
                            sqleditorfrm.NodeType = FavNodeType.FolderNode;
                            sqleditorfrm.Text = ComFunction.GetResxString(typeof(FavView), "TXT_SQL_EDITOR", "SQL Editor");
                            sqleditorfrm.ToolTipText = sqleditorfrm.Text;
                            return;
                        }
                        sqleditorfrm.DisplaySQLFile(filepath);
                        sqleditorfrm.FilePath = filepath;
                        sqleditorfrm.FileName = selstr;
                        sqleditorfrm.FileNotSave = false;
                        sqleditorfrm.NodeType = FavNodeType.FileNode;
                        sqleditorfrm.Text = selstr;
                        sqleditorfrm.ToolTipText = sqleditorfrm.Text;
                        //sqleditorfrm.Activate();
                    }
                    else
                    {
                        _MsgText = ComFunction.GetResxString(typeof(FavView), "TXT_NOT_OPEN_SQL_WINDOW", "Not open SQL Editor window.");
                        MessageBox.Show(_MsgText, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                }
                else
                {
                    _MsgText = ComFunction.GetResxString(typeof(FavView), "TXT_FILE_NOT_EXIST", "The selected file does not exist.");
                    MessageBox.Show(_MsgText, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (System.Exception ex)
            {
                _MsgText = ComFunction.GetResxString(typeof(FavView), "TXT_PLS_REF_FAVORITES", "Please refresh favorites and try again.");
                _MsgText = ex.Message + _MsgText;
                MessageBox.Show(_MsgText,this.Text,MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }
        #endregion
        #region CreateNewNodeFolder : 在TreeView当前节点创建子节点并创建对应的新目录
        /// <summary>
        /// 在TreeView当前节点创建子节点并创建对应的新目录
        /// </summary>
        private bool CreateNewNodeFolder()
        {
            try
            {
                FavNode SelNode = (FavNode)this.tvFav.SelectedNode;
                string nodeid = SelNode.NodeID;

                int orderid = 1;
                if (SelNode != null)
                {
                    orderid = SelNode.Nodes.Count + 1;
                }

                _LblText = ComFunction.GetResxString(typeof(FavView), "TXT_NEW_FOLDER", "New Folder");

                string text = SelNode.Text;
                string dirpath = SelNode.FilePath;
                FavNodeType nodetype = SelNode.NodeType;
                string newNodeid = text + "_SUB";  //GetMaxNodeID(ds.Tables[0]);
                string folderKeyWords = _LblText; //"新建文件夹";
                //string newFolderName = folderKeyWords + "{0}";
                //int cnt = System.IO.Directory.GetDirectories(dirpath, folderKeyWords + "*").Length;
                //if (cnt == 0)
                //    newFolderName = string.Format(newFolderName, "");
                //else
                //    newFolderName = string.Format(newFolderName, " (" + (cnt + 1) + ")");
                string newFolderName = folderKeyWords;
                string newFolderPath = Path.Combine(dirpath, newFolderName);
                if (Directory.Exists(newFolderPath))
                {
                    for (int i = 2; i < ushort.MaxValue; i++)//65535
                    {
                        newFolderName = folderKeyWords + " (" + i + ")";
                        newFolderPath = Path.Combine(dirpath, newFolderName);
                        if (!Directory.Exists(newFolderPath)) break;
                    }
                }

                //新建文件夹
                //if (!Directory.Exists(newFolderPath)) Directory.CreateDirectory(newFolderPath);
                Directory.CreateDirectory(newFolderPath);

                //在treeview上增加一个节点
                FavNode node = new FavNode(newFolderName);
                node.NodeID = newNodeid;
                node.ParentID = nodeid;
                node.NodeType = FavNodeType.FolderNode;
                node.FilePath = newFolderPath;
                node.ImageIndex = 1;
                node.SelectedImageIndex = 2;
                SelNode.Nodes.Add(node);
                SelNode.Expand();
                int idx = SelNode.Nodes.Count - 1;
                SelNode.Nodes[idx].Checked = true;
                tvFav.SelectedNode = SelNode.Nodes[idx];//选中
                   
                return true;
            }
            catch (System.Exception ex)
            {
                //MessageBox.Show(ex.Message);
                _MsgText = ComFunction.GetResxString(typeof(FavView), "TXT_NOT_SEL_NODE", "Please select a TreeView node.");
                MessageBox.Show(_MsgText, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

        }
        #endregion
        #region CreateNewNodeFile : 在TreeView当前节点创建新子节点并创建新SQL脚本文档
        /// <summary>
        /// 在TreeView当前节点创建新子节点并创建新SQL脚本文档
        /// </summary>
        private bool CreateNewNodeFile()
        {
            try
            {
                FavNode SelNode = (FavNode)this.tvFav.SelectedNode;
                string nodeid = SelNode.NodeID;

                int orderid = 1;
                if (SelNode != null)
                {
                    orderid = SelNode.Nodes.Count + 1;
                }

                _LblText = ComFunction.GetResxString(typeof(FavView), "TXT_NEW_SQL_DOC", "New SQL Document");

                string text = SelNode.Text;
                string dirpath = SelNode.FilePath;
                FavNodeType nodetype = SelNode.NodeType;
                string newNodeid = text + "_FILE";  //GetMaxNodeID(ds.Tables[0]);
                string fileKeyWords = _LblText; //"新建SQL文档";
                //string newFileName = fileKeyWords + "{0}.sql";
                //int cnt = System.IO.Directory.GetFiles(dirpath, fileKeyWords + "*.sql").Length;
                //if (cnt == 0)
                //    newFileName = string.Format(newFileName, "");
                //else
                //    newFileName = string.Format(newFileName, " (" + (cnt + 1) + ")");
                string newFileName = fileKeyWords + ".sql";
                string newFilePath = Path.Combine(dirpath, newFileName);

                if (File.Exists(newFilePath))
                {
                    for (int i = 2; i < ushort.MaxValue; i++)//65535
                    {
                        newFileName = fileKeyWords + " (" + i + ")" + ".sql";
                        newFilePath = Path.Combine(dirpath, newFileName);
                        if (!File.Exists(newFilePath)) break;
                    }
                }
                
                //创建新建文件
                //if (!File.Exists(newFilePath)) File.Create(newFilePath);
                File.Create(newFilePath).Close();

                //在treeview上增加一个节点
                FavNode node = new FavNode(newFileName);
                node.NodeID = newNodeid;
                node.ParentID = nodeid;
                node.NodeType = FavNodeType.FileNode;
                node.FilePath = newFilePath;
                node.ImageIndex = 3;
                node.SelectedImageIndex = 3;
                SelNode.Nodes.Add(node);
                SelNode.Expand();
                int idx = SelNode.Nodes.Count - 1;
                SelNode.Nodes[idx].Checked = true;
                tvFav.SelectedNode = SelNode.Nodes[idx];//选中
                    
                return true;
            }
            catch (System.Exception ex)
            {
                //MessageBox.Show(ex.Message);
                _MsgText = ComFunction.GetResxString(typeof(FavView), "TXT_NOT_SEL_NODE", "Please select a TreeView node.");
                MessageBox.Show(_MsgText, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
                return false;
            }
        }
        #endregion
        #region BeginNodeRename : TreeView当前节点重命名开始从命名及对应目录（SQL文档）重命名
        /// <summary>
        /// TreeView当前节点重命名开始
        /// </summary>
        private void BeginNodeRename()
        {
            try
            {
                FavNode SelNode = (FavNode)this.tvFav.SelectedNode;
                if (SelNode != null && SelNode.Parent != null)
                {
                    tvFav.SelectedNode = SelNode;
                    tvFav.LabelEdit = true;
                    if (!SelNode.IsEditing)
                    {
                        SelNode.BeginEdit();
                    }
                }
                else
                {
                    _MsgText = ComFunction.GetResxString(typeof(FavView), "TXT_NOT_SEL_NODE_OR_IS_ROOT", "Not selected node or the node is the root node.");
                    MessageBox.Show(_MsgText, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message,this.Text,MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }
        #endregion
        #region EndNodeRename : TreeView当前节点重命名开始结束及对应目录（SQL文档）重命名
        /// <summary>
        /// TreeView当前节点重命名开始结束及对应目录（SQL文档）重命名
        /// </summary>
        private void EndNodeRename(NodeLabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                if (e.Label.Length > 0)
                {
                    if (e.Label.IndexOfAny(new char[] { '\\', '/', ':', '*', '?', '\"', '<', '>', '|' }) == -1)
                    {
                        e.Node.EndEdit(false);
                        FavNode SelNode = (FavNode)e.Node;
                        string nodeid = SelNode.NodeID;
                        FavNodeType nodetype = SelNode.NodeType;
                        string newNodetext = e.Label;
                        string filepath = SelNode.FilePath;
                        string newfilepath = filepath;
                        string origNodeText = SelNode.Text;

                        try
                        {
                            #region SQL文档重命名
                            if (nodetype == FavNodeType.FileNode)
                            {
                                if (newNodetext.Length >= 4)
                                {
                                    string fileExtension = newNodetext.Substring(newNodetext.Length - 4, 4).ToLower();
                                    if (fileExtension != ".sql") newNodetext += ".sql";
                                }
                                else
                                {
                                    newNodetext += ".sql";
                                }

                                int end = filepath.LastIndexOf("\\");
                                newfilepath = filepath.Substring(0, end + 1) + newNodetext;
                                newNodetext = newfilepath.Substring(end + 1, newfilepath.Length - end - 1);
                                SelNode.Text = newNodetext;
                                SelNode.ToolTipText = newNodetext;
                                SelNode.FilePath = newfilepath;

                                File.Move(filepath, newfilepath);
                            }
                            #endregion

                            #region 目录重命名
                            if (nodetype == FavNodeType.FolderNode)
                            {
                                int end = filepath.LastIndexOf("\\");
                                newfilepath = filepath.Substring(0, end + 1) + newNodetext;
                                SelNode.Text = newNodetext;
                                SelNode.ToolTipText = newNodetext;
                                SelNode.FilePath = newfilepath;

                                Directory.Move(filepath, newfilepath);
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            _MsgText = e.Label + "\r\n" + ex.Message;
                            MessageBox.Show(_MsgText, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            SelNode.Text = origNodeText;
                            SelNode.ToolTipText = origNodeText;
                            SelNode.FilePath = filepath;
                            SelNode.Checked = true;
                            tvFav.SelectedNode = SelNode;
                            e.CancelEdit = true;
                        }
                    }
                    else
                    {
                        _MsgText = ComFunction.GetResxString(typeof(FavView), "TXT_INVALID_NODE_OR_CHAR", "Invalid node or invalid characters:");
                        _MsgText += " \\ / : * ? \"  < > |";
                        MessageBox.Show(_MsgText, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.CancelEdit = true;
                        e.Node.BeginEdit();
                    }
                }
                else
                {
                    _MsgText = ComFunction.GetResxString(typeof(FavView), "TXT_INVALID_NODE_OR_EMPTY", "Invalid node or node name can not be empty.");
                    MessageBox.Show(_MsgText, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.CancelEdit = true;
                    e.Node.BeginEdit();
                }
                this.tvFav.LabelEdit = false;
            }
        }
        #endregion
        #region RemoveNode : TreeView当前节点移除并删除对应SQL文档（目录）
        /// <summary>
        /// TreeView当前节点移除并删除对应SQL文档（目录）
        /// </summary>
        private void RemoveNode()
        {
            try
            {
                FavNode ParNode = (FavNode)this.tvFav.SelectedNode.Parent;
                FavNode SelNode = (FavNode)this.tvFav.SelectedNode;

                if (SelNode != null && SelNode.Parent != null)
                {
                    string nodeid = SelNode.NodeID;
                    string text = SelNode.Text;
                    string filepath = SelNode.FilePath;
                    FavNodeType nodetype = SelNode.NodeType;
                    //SQL文档删除
                    if (nodetype == FavNodeType.FileNode)
                    {
                        if (File.Exists(filepath))
                        {
                            File.Delete(filepath);
                        }
                    }
                    //目录删除
                    if (nodetype == FavNodeType.FolderNode)
                    {
                        if (Directory.Exists(filepath))
                        {
                            Directory.Delete(filepath, true);
                        }
                    }

                    //删除节点
                    ParNode.Checked = true;
                    tvFav.SelectedNode = ParNode;//选中父节点
                    tvFav.Nodes.Remove(SelNode);
                }
                else
                {
                    _MsgText = ComFunction.GetResxString(typeof(FavView), "TXT_NOT_SEL_NODE_OR_IS_ROOT","Not selected node or the node is the root node.");
                    MessageBox.Show(_MsgText, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message,this.Text,MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region tvFav TreeView操作

        private void tvFav_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            //
            //上一个文件修改后有无保存
            //
            FavSQLBase sqleditorfrm = (FavSQLBase)Application.OpenForms["FavSQLBase"];
            if (sqleditorfrm != null)
            {
                //上一个文件修改后尚未保存
                if (sqleditorfrm.FileNotSave)
                {
                    _MsgText = ComFunction.GetResxString(typeof(FavSQLBase), "TXT_FILE_NOT_SAVE", "File \"{0}\" has not been saved, save it?");
                    _MsgText = string.Format(_MsgText, sqleditorfrm.FileName);
                    DialogResult diaRes = MessageBox.Show(_MsgText, this.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (diaRes == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        return;
                    }

                    if (diaRes == DialogResult.Yes)
                    {
                        sqleditorfrm.tecSQLEdit.SaveFile(sqleditorfrm.FilePath);
                        sqleditorfrm.FileNotSave = false;
                    }
                }
            }

            e.Node.ForeColor = Color.Blue;
            //e.Node.NodeFont = new Font("微软雅黑", 9, FontStyle.Underline);
            e.Node.NodeFont = new Font(tvFav.Font.FontFamily.Name, tvFav.Font.Size, FontStyle.Underline);
            if (_LastSelNode != null)
            {
                _LastSelNode.ForeColor = SystemColors.WindowText;
                //_LastSelNode.NodeFont = new Font("微软雅黑", 9, FontStyle.Regular);   .
                _LastSelNode.NodeFont = new Font(tvFav.Font.FontFamily.Name, tvFav.Font.Size, FontStyle.Regular);
            }

        }

        private void tvFav_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //顶部菜单调整
            FavNode SelNode = (FavNode)e.Node;
            switch (SelNode.NodeType)
            {
                case FavNodeType.RootNode:
                case FavNodeType.FolderNode:
                    btn_NewFolder.Visible = true;
                    btn_NewFile.Visible = true;
                    toolStripSeparator2.Visible = true;
                    break;
                case FavNodeType.FileNode:
                    btn_NewFolder.Visible = false;
                    btn_NewFile.Visible = false;
                    toolStripSeparator2.Visible = false;
                    break;
            }

            if (this.tvFav.SelectedNode != null)
            {
                _LastSelNode = tvFav.SelectedNode;
            }                   

            OpenNodeFile();

        }
        private void tvFav_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //_TreeClickNode = (FavNode)e.Node;
            //this.tvFav.SelectedNode = _TreeClickNode;
            //if (e.Button == MouseButtons.Left && _TreeClickNode.NodeType == "FOLDER")
            //{
            //    if (_TreeClickNode.IsExpanded)
            //        _TreeClickNode.Collapse();
            //    else
            //        _TreeClickNode.Expand();
            //}
        }

        private void tvFav_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                Point mpt = new Point(e.X, e.Y);
                _TreeClickNode = (FavNode)this.tvFav.GetNodeAt(mpt);
                //this.tvFav.SelectedNode = _TreeClickNode;
                if (_TreeClickNode != null)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        this.tvFav.SelectedNode = _TreeClickNode;
                        CreatPopMenu(_TreeClickNode.NodeType);
                        contextMenuStrip1.Show(tvFav, mpt);
                    }
                    
                }
            }
            catch
            {
            }
        }


        private void tvFav_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            EndNodeRename(e);
        }
        /// <summary>
        /// 定义TreeView快捷键，F2：重命名、F5：刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvFav_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                //Node重命名
                FavNode SelNode = (FavNode)this.tvFav.SelectedNode;
                if (SelNode != null && SelNode.Parent != null)
                {
                    BeginNodeRename();
                }
            }

            if (e.KeyCode == Keys.F5)
            {
                //TreeView刷新
                LoadTreeview();
            }
        }
        private void tvFav_DragDrop(object sender, DragEventArgs e)
        {

        }

        private void tvFav_ItemDrag(object sender, ItemDragEventArgs e)
        {

        }

        #endregion

        #region 右键弹出菜单事件相关
        /// <summary>
        /// 右键弹出菜单-打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiOpen_Click(object sender, EventArgs e)
        {
            //打开文档
            OpenNodeFile();
        }

        /// <summary>
        /// 右键弹出菜单-新建文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiNewFolder_Click(object sender, EventArgs e)
        {
            //新建文件夹
            if (CreateNewNodeFolder())
                BeginNodeRename();
        }

        /// <summary>
        /// 右键弹出菜单-新建SQL文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiNewFile_Click(object sender, EventArgs e)
        {
            //新建SQL文件
            if (CreateNewNodeFile())
                BeginNodeRename();
        }
        /// <summary>
        /// 右键弹出菜单-删除节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiDelete_Click(object sender, EventArgs e)
        {
            //删除节点
            RemoveNode();
        }
        /// <summary>
        /// 右键弹出菜单-刷新TreeView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiRefrush_Click(object sender, EventArgs e)
        {
            //TreeView刷新
            LoadTreeview();

        }

        private void tmsiRename_Click(object sender, EventArgs e)
        {
            //TreeView节点重命名
            BeginNodeRename();

        }
        #endregion

        #region 顶部菜单事件相关
        private void btn_RootSet_Click(object sender, EventArgs e)
        {
            if (fbdRootFloder.ShowDialog() == DialogResult.OK)
            {
                _RootPath = fbdRootFloder.SelectedPath;
                
                //保存SQL收藏夹根目录路径,将当前选择的收藏夹目录信息写入注册表
                ComRegistry.SetFavRootFloder(_RootPath);
                
                //TreeView刷新
                btn_RootSet.ToolTipText = _FavTipTxt + " " + _RootPath;
                FavSQLBase sqleditorfrm = (FavSQLBase)Application.OpenForms["FavSQLBase"];
                if (sqleditorfrm != null)
                {
                    sqleditorfrm.EmptyContents();
                }
                LoadTreeview();
            }
        }
        

        private void btn_Refrush_Click(object sender, EventArgs e)
        {
            LoadTreeview();
        }

        private void btn_NewFolder_Click(object sender, EventArgs e)
        {
            //新建文件夹
            if (CreateNewNodeFolder())
                BeginNodeRename();
        }

        private void btn_NewFile_Click(object sender, EventArgs e)
        {
            //新建SQL文件
            if (CreateNewNodeFile())
                BeginNodeRename();
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            //删除节点
            RemoveNode();
        }
        #endregion

        private void txtSrch_Enter(object sender, EventArgs e)
        {
            BeginInvoke((Action)delegate
            {
                txtSrch.SelectAll();
            });            
        }

        private void txtSrch_KeyDown(object sender, KeyEventArgs e)
        {
            //回车时查找TreeView
            if (e.KeyCode == Keys.Enter)
            {
                int idx = FindNodePartDown(txtSrch.Text, 0);
                if (idx > -1)
                {
                    _FindNode = _NodeList[idx];
                    _FindNode.Expand();
                    tvFav.SelectedNode = _FindNode;        //选中查找到的节点  
                    tvFav.Focus();  
                }
            }
        }

        /// <summary>  
        /// 模糊匹配(向下查找)  
        /// </summary>  
        /// <param name="inputText"></param>  
        /// <returns></returns>  
        private int FindNodePartDown(string inputText, int startCount)
        {
            _NodeList.Clear();
            GetAllNodes(tvFav.Nodes, _NodeList);
            for (int i = startCount; i < _NodeList.Count; i++)
            {
                if (_NodeList[i].Text.ToLower().Contains(inputText.ToLower()))
                {
                    return i;
                }
            }

            return -1;
        } 

        /// <summary>
        /// 遍历树节点，并将节点存入List<TreeNode>集合中
        /// </summary>
        /// <param name="nodeCollection"></param>
        /// <param name="nodeList"></param>
        private void GetAllNodes(TreeNodeCollection nodeCollection, List<TreeNode> nodeList)
        {
            foreach (TreeNode itemNode in nodeCollection)
            {
                nodeList.Add(itemNode);
                GetAllNodes(itemNode.Nodes, nodeList);
            }
        }


    }



}
