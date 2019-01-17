using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using EACT;
using System.IO;
using TOOL;

namespace FTPDome
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitEvent();
        }

        TOOL.WinSCPFTP EACTFTP = null;

        string GetFullPath(TreeNode node) 
        {
            string path = string.Empty;
            GetFullPath(node, ref path);
            return path;
        }

        void GetFullPath(TreeNode node, ref string path)
        {
          
            if (node != null) 
            {

                var ex =string.Empty;
                path = string.Format("{0}{1}{2}", "/",node.Text, path);
                if (node.Parent != null)
                {
                    GetFullPath(node.Parent, ref path);
                }
            }
        }

        void RefreshUI()
        {
            bool isConnect = EACTFTP != null;
            if (isConnect) 
            {
                TxtN.Text = "/NCDATA/";
                var text = TxtN.Text;
                treeView1.Nodes.Clear();
                listBox1.Items.Clear();

                EACTFTP.GetDirectoryList(text).ToList().ForEach(u => {
                    treeView1.Nodes.Add(u);
                });

                EACTFTP.GetFileList(text).ToList().ForEach(u => {
                    listBox1.Items.Add(u);
                });
            }

            button1.Enabled = isConnect;
            HttpU.Enabled = isConnect;
            button6.Enabled = isConnect;
            button4.Enabled = isConnect;
            btnDeleteFile.Enabled = isConnect;
            btnDeleteFolder.Enabled = isConnect;
            button5.Enabled = isConnect;
            button7.Enabled = isConnect;
            btnClose.Enabled = isConnect;
            treeView1.Enabled = isConnect;
            listBox1.Enabled = isConnect;
            btnConnect.Enabled = !isConnect;
        }

        void InitEvent() 
        {
            treeView1.NodeMouseDoubleClick += treeView1_NodeMouseDoubleClick;
            treeView1.AfterSelect += treeView1_AfterSelect;
        }

        void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            listBox1.Items.Clear();
            TxtN.Text = GetFullPath(treeView1.SelectedNode);
            EACTFTP.GetFileList(TxtN.Text).ToList().ForEach(u =>
            {
                listBox1.Items.Add(u);
            });
        }

        void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var path = string.Empty;
            GetFullPath(e.Node, ref path);
            e.Node.Nodes.Clear();
            EACTFTP.GetDirectoryList(path).ToList().ForEach(u => {
                e.Node.Nodes.Add(u);
            });
        }

        private void TcpU_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FB = new FolderBrowserDialog();
            FB.Description = "选择需要上传的文件夹";
            if (FB.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FTPClient FTPClient = new FTPClient(txtIP.Text.Trim(), "", txtUse.Text.Trim(), txtPW.Text.Trim(), 21);
                FTPClient.Connect();
                if (FTPClient.Connected)
                {
                    //首先进入到目标目录
                    //FTPClient.ChDir(@"MachRoders\160725");
                    //FTPClient.RmDir(new DirectoryInfo(@"F:\EACT3.0\EACT3.0\UI\bin\Debug\CNCMACHINE\10000081").Name);
                    FTPClient.Put(FB.SelectedPath, "*");
                }
                FTPClient.DisConnect();
                MessageBox.Show("OK");
            }
        }

        private void TcpD_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FB = new FolderBrowserDialog();
            FB.Description = "选择将文件夹保存到本地文件夹";
            if (FB.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FTPClient FTPClient = new FTPClient("192.168.1.30", "", "eact", "123", 21);
                FTPClient.Connect();
                if (FTPClient.Connected)
                {
                    var ee = FTPClient.List("-a");
                }
                FTPClient.DisConnect();
                MessageBox.Show("OK");
            }
        }

        private void HttpU_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog FB = new FolderBrowserDialog();
                FB.Description = "选择需要上传的文件夹";
                if (FB.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var node=treeView1.SelectedNode;
                    //Test OK
                    //EACTFTP.NextDirectory(TxtN.Text);
                    //EACTFTP.UploadFolder(FB.SelectedPath);
                    EACTFTP.UploadFolder(FB.SelectedPath, TxtN.Text);
                    MessageBox.Show("OK");
                    RefreshUI();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog FB = new FolderBrowserDialog();
                var node = treeView1.SelectedNode;
                if (node == null)
                {
                    MessageBox.Show("请选择下载目录");
                    return;
                }
                FB.Description = "选择将文件夹保存到本地文件夹";
                if (FB.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    EACTFTP.DownloadFolder(FB.SelectedPath,GetFullPath(node.Parent),node.Text);
                    MessageBox.Show("OK");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog FB = new OpenFileDialog();
                if (FB.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    EACTFTP.UpLoadFile(FB.FileName, TxtN.Text);
                    MessageBox.Show("OK");
                    RefreshUI();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog FB = new OpenFileDialog();
            FB.Multiselect = false;
            if (FB.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FTPClient FTPClient = new FTPClient(txtIP.Text.Trim(), "", txtUse.Text.Trim(), txtPW.Text.Trim(), int.Parse(txtport.Text));
                FTPClient.Connect();
                if (FTPClient.Connected)
                {
                    //首先进入到目标目录
                    //FTPClient.ChDir(@"MachRoders\160725");
                    //FTPClient.RmDir(new DirectoryInfo(@"F:\EACT3.0\EACT3.0\UI\bin\Debug\CNCMACHINE\10000081").Name);
                    FTPClient.Put(FB.FileName);
                }
                FTPClient.DisConnect();
                MessageBox.Show("OK");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FTPClient FTPClient = new FTPClient(txtIP.Text.Trim(), @"CMM\0001\0001-C403-D02", txtUse.Text.Trim(), txtPW.Text.Trim(), int.Parse(txtport.Text));
            FTPClient.Connect();
            if (FTPClient.Connected)
            {
                //首先进入到目标目录
                //FTPClient.ChDir(@"MachRoders\160725");
                //FTPClient.RmDir(new DirectoryInfo(@"F:\EACT3.0\EACT3.0\UI\bin\Debug\CNCMACHINE\10000081").Name);
                FTPClient.Get("0001-C403-D02.stp", @"C:\Users\cj\Desktop\三亚消防", "0001-C403-D02.stp");
            }
            FTPClient.DisConnect();
            MessageBox.Show("OK");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                EACTFTP EACTFTP = new EACTFTP(txtIP.Text.Trim(), int.Parse(txtport.Text.ToString()), "", txtUse.Text.Trim(), txtPW.Text.Trim());
                    listBox1.Items.Clear();
                    listBox1.Items.AddRange(EACTFTP.GetDirectoryList(EACTFTP.FtpURI + "/" + TxtN.Text));
                    MessageBox.Show("OK");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {

                if (listBox1.SelectedItem==null)
                {
                    MessageBox.Show("请选择服务器文件");
                    return;
                }
                var remotePath = TxtN.Text;
                var remoteFile = listBox1.SelectedItem.ToString();
                FolderBrowserDialog FB = new FolderBrowserDialog();
                FB.Description = "选择文件夹";
                if (FB.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    EACTFTP.NextDirectory(remotePath);
                    EACTFTP.DownloadFile(FB.SelectedPath, remotePath, remoteFile);
                    MessageBox.Show("OK");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                EACTFTP EACTFTP = new EACTFTP(txtIP.Text.Trim(), int.Parse(txtport.Text.ToString()), "", txtUse.Text.Trim(), txtPW.Text.Trim());
                listBox1.Items.Clear();
                listBox1.Items.AddRange(EACTFTP.GetFileList(TxtN.Text));
                MessageBox.Show("OK");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnDeleteFile_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("请选择服务器文件");
                return;
            }
            var remotePath = TxtN.Text;
            var remoteFile = listBox1.SelectedItem.ToString();
            EACTFTP.Delete(EACTFTP.Combine(remotePath ,remoteFile));
            MessageBox.Show("OK");
            RefreshUI();
            
        }

        private void btnDeleteFolder_Click(object sender, EventArgs e)
        {
            var dir = TxtN.Text;
            if (EACTFTP.DirectoryExist(dir)) 
            {
                EACTFTP.DeleteFtpDirWithAll(dir);
                MessageBox.Show("OK");
                RefreshUI();
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                EACTFTP = new TOOL.WinSCPFTP(txtIP.Text.Trim(), int.Parse(txtport.Text.ToString()), "", txtUse.Text.Trim(), txtPW.Text.Trim());
                RefreshUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (EACTFTP != null) 
            {
                EACTFTP.CloseFTP();
                EACTFTP = null;
            }
            RefreshUI();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var frm = new EACTSYSLOG.FrmLogShow();
            frm.Show();
            frm.LogShow();
        }
    }
}
