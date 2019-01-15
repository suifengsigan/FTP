namespace FTPDome
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.HttpU = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.TcpU = new System.Windows.Forms.Button();
            this.TcpD = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.txtUse = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPW = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtN = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.ckSSL = new System.Windows.Forms.CheckBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.txtport = new System.Windows.Forms.TextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.btnDeleteFile = new System.Windows.Forms.Button();
            this.btnDeleteFolder = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.ckAscii = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // HttpU
            // 
            this.HttpU.Enabled = false;
            this.HttpU.Location = new System.Drawing.Point(166, 174);
            this.HttpU.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.HttpU.Name = "HttpU";
            this.HttpU.Size = new System.Drawing.Size(138, 33);
            this.HttpU.TabIndex = 2;
            this.HttpU.Text = "上传文件夹";
            this.HttpU.UseVisualStyleBackColor = true;
            this.HttpU.Click += new System.EventHandler(this.HttpU_Click);
            // 
            // button4
            // 
            this.button4.Enabled = false;
            this.button4.Location = new System.Drawing.Point(166, 215);
            this.button4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(138, 33);
            this.button4.TabIndex = 4;
            this.button4.Text = "下载文件夹";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // TcpU
            // 
            this.TcpU.Location = new System.Drawing.Point(691, 287);
            this.TcpU.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TcpU.Name = "TcpU";
            this.TcpU.Size = new System.Drawing.Size(117, 33);
            this.TcpU.TabIndex = 7;
            this.TcpU.Text = "TCP方式上传文件夹";
            this.TcpU.UseVisualStyleBackColor = true;
            this.TcpU.Visible = false;
            this.TcpU.Click += new System.EventHandler(this.TcpU_Click);
            // 
            // TcpD
            // 
            this.TcpD.Location = new System.Drawing.Point(691, 328);
            this.TcpD.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TcpD.Name = "TcpD";
            this.TcpD.Size = new System.Drawing.Size(117, 33);
            this.TcpD.TabIndex = 8;
            this.TcpD.Text = "TCP方式下载文件夹";
            this.TcpD.UseVisualStyleBackColor = true;
            this.TcpD.Visible = false;
            this.TcpD.Click += new System.EventHandler(this.TcpD_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 17);
            this.label1.TabIndex = 9;
            this.label1.Text = "FTP IP地址";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(115, 14);
            this.txtIP.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(99, 23);
            this.txtIP.TabIndex = 10;
            this.txtIP.Text = "127.0.0.1";
            // 
            // txtUse
            // 
            this.txtUse.Location = new System.Drawing.Point(115, 52);
            this.txtUse.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtUse.Name = "txtUse";
            this.txtUse.Size = new System.Drawing.Size(305, 23);
            this.txtUse.TabIndex = 12;
            this.txtUse.Text = "eact";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 17);
            this.label2.TabIndex = 11;
            this.label2.Text = "FTP 用户名";
            // 
            // txtPW
            // 
            this.txtPW.Location = new System.Drawing.Point(115, 91);
            this.txtPW.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPW.Name = "txtPW";
            this.txtPW.Size = new System.Drawing.Size(305, 23);
            this.txtPW.TabIndex = 14;
            this.txtPW.Text = "123";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 17);
            this.label3.TabIndex = 13;
            this.label3.Text = "FTP 密码";
            // 
            // TxtN
            // 
            this.TxtN.Location = new System.Drawing.Point(24, 333);
            this.TxtN.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TxtN.Name = "TxtN";
            this.TxtN.ReadOnly = true;
            this.TxtN.Size = new System.Drawing.Size(305, 23);
            this.TxtN.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 312);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 17);
            this.label4.TabIndex = 15;
            this.label4.Text = "FTP目录";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(444, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(364, 17);
            this.label5.TabIndex = 17;
            this.label5.Text = "修改成你设置的FTP服务器地址，注意冒号要是英文输入法下的冒号";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(444, 52);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(622, 17);
            this.label6.TabIndex = 18;
            this.label6.Text = "修改成你设置的FTP服务器可以访问的帐号，帐号要有可读写权限，在Ser_U软件设置，帐号设置后，最好重启Ser_U";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(444, 91);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(268, 17);
            this.label7.TabIndex = 19;
            this.label7.Text = "修改成你设置的FTP服务器可以访问的帐号的密码";
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(23, 173);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(137, 33);
            this.button1.TabIndex = 20;
            this.button1.Text = "上传文件";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(691, 205);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(117, 33);
            this.button2.TabIndex = 21;
            this.button2.Text = "TCP方式上传文件";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(691, 246);
            this.button3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(117, 33);
            this.button3.TabIndex = 22;
            this.button3.Text = "TCP方式下载文件";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // ckSSL
            // 
            this.ckSSL.AutoSize = true;
            this.ckSSL.Location = new System.Drawing.Point(115, 121);
            this.ckSSL.Name = "ckSSL";
            this.ckSSL.Size = new System.Drawing.Size(47, 21);
            this.ckSSL.TabIndex = 23;
            this.ckSSL.Text = "SSL";
            this.ckSSL.UseVisualStyleBackColor = true;
            this.ckSSL.Visible = false;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 17;
            this.listBox1.Location = new System.Drawing.Point(335, 303);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(314, 157);
            this.listBox1.TabIndex = 25;
            // 
            // txtport
            // 
            this.txtport.Location = new System.Drawing.Point(321, 13);
            this.txtport.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtport.Name = "txtport";
            this.txtport.Size = new System.Drawing.Size(99, 23);
            this.txtport.TabIndex = 26;
            this.txtport.Text = "21";
            // 
            // button6
            // 
            this.button6.Enabled = false;
            this.button6.Location = new System.Drawing.Point(23, 214);
            this.button6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(137, 33);
            this.button6.TabIndex = 27;
            this.button6.Text = "下载文件";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Enabled = false;
            this.button7.Location = new System.Drawing.Point(462, 216);
            this.button7.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(90, 33);
            this.button7.TabIndex = 28;
            this.button7.Text = "文件列表";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Visible = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // btnDeleteFile
            // 
            this.btnDeleteFile.Enabled = false;
            this.btnDeleteFile.Location = new System.Drawing.Point(25, 256);
            this.btnDeleteFile.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDeleteFile.Name = "btnDeleteFile";
            this.btnDeleteFile.Size = new System.Drawing.Size(137, 33);
            this.btnDeleteFile.TabIndex = 29;
            this.btnDeleteFile.Text = "删除文件";
            this.btnDeleteFile.UseVisualStyleBackColor = true;
            this.btnDeleteFile.Click += new System.EventHandler(this.btnDeleteFile_Click);
            // 
            // btnDeleteFolder
            // 
            this.btnDeleteFolder.Enabled = false;
            this.btnDeleteFolder.Location = new System.Drawing.Point(167, 256);
            this.btnDeleteFolder.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDeleteFolder.Name = "btnDeleteFolder";
            this.btnDeleteFolder.Size = new System.Drawing.Size(137, 33);
            this.btnDeleteFolder.TabIndex = 30;
            this.btnDeleteFolder.Text = "删除文件夹";
            this.btnDeleteFolder.UseVisualStyleBackColor = true;
            this.btnDeleteFolder.Click += new System.EventHandler(this.btnDeleteFolder_Click);
            // 
            // treeView1
            // 
            this.treeView1.Enabled = false;
            this.treeView1.Location = new System.Drawing.Point(25, 363);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(304, 97);
            this.treeView1.TabIndex = 31;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(447, 121);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 32;
            this.btnConnect.Text = "连接FTP";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnClose
            // 
            this.btnClose.Enabled = false;
            this.btnClose.Location = new System.Drawing.Point(549, 121);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 33;
            this.btnClose.Text = "关闭FTP";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // button5
            // 
            this.button5.Enabled = false;
            this.button5.Location = new System.Drawing.Point(366, 215);
            this.button5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(90, 33);
            this.button5.TabIndex = 24;
            this.button5.Text = "文件夹列表";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Visible = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(654, 119);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 23);
            this.button8.TabIndex = 34;
            this.button8.Text = "日志";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // ckAscii
            // 
            this.ckAscii.AutoSize = true;
            this.ckAscii.Checked = true;
            this.ckAscii.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckAscii.Location = new System.Drawing.Point(168, 119);
            this.ckAscii.Name = "ckAscii";
            this.ckAscii.Size = new System.Drawing.Size(58, 21);
            this.ckAscii.TabIndex = 35;
            this.ckAscii.Text = "ASCII";
            this.ckAscii.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1077, 480);
            this.Controls.Add(this.ckAscii);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.btnDeleteFolder);
            this.Controls.Add(this.btnDeleteFile);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.txtport);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.ckSSL);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.TxtN);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPW);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtUse);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TcpD);
            this.Controls.Add(this.TcpU);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.HttpU);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button HttpU;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button TcpU;
        private System.Windows.Forms.Button TcpD;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.TextBox txtUse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPW;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TxtN;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox ckSSL;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox txtport;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button btnDeleteFile;
        private System.Windows.Forms.Button btnDeleteFolder;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.CheckBox ckAscii;
    }
}

