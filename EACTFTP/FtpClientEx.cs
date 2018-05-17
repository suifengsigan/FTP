using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Globalization;
using System.ComponentModel;
using System.Collections;
using System.Net.Security;
using System.Linq;

namespace TOOL
{
    public class FtpClientEx
    {
        EACT.FTPClient ftpConnection;

        ~FtpClientEx() 
        {
            try { CloseFTP(); }
            catch { }
        }

        public void CloseFTP() 
        {
            if (ftpConnection.Connected)
            {
                ftpConnection.DisConnect();
            }
        }
        public FtpClientEx(string FtpServerIP, int FtpServerPort, string FtpRemotePath, string FtpUser, string FtpPassword)
        {
            ftpConnection = new EACT.FTPClient(FtpServerIP, FtpRemotePath, FtpUser, FtpPassword, FtpServerPort);
            ftpConnection.Connect();
        }

        public string Combine(string remotePath, string RemoteFile)
        {
            return remotePath.TrimEnd('/') + "/" + RemoteFile.TrimStart('/');
        }


        #region   获取当前目录下明细(包含文件和文件夹)
        /// <summary>
        /// 获取当前目录下明细(包含文件和文件夹)
        /// </summary>
        /// <returns></returns>
        public string[] GetFilesDetailList(string local)
        {
            try
            {
                var list = ftpConnection.List(local).ToList();
                return list.ToArray();
            }
            catch
            {
                return new string[] { };
            }
        }

        #endregion

        /// <summary>
        /// 获取文件列表
        /// </summary>
        public virtual string[] GetFileList(string LocaPath)
        {
            try
            {
                string[] drectory = GetFilesDetailList(LocaPath);
                var list = new List<string>();
                foreach (string str in drectory)
                {
                    if (str.Trim().Substring(0, 1).ToUpper() == "-")
                    {
                        /*判断 Unix 风格*/
                        string dir = str.Split(' ').Where(u => !string.IsNullOrEmpty(u) && !string.IsNullOrEmpty(u.Trim())).LastOrDefault();
                        AddList(list, dir);
                    }
                    else if (str.Trim().Substring(0, 1).ToUpper() != "D" && !str.Contains("<DIR>"))
                    {
                        var temp = str.Split(' ').Where(u => !string.IsNullOrEmpty(u) && !string.IsNullOrEmpty(u.Trim()));
                        if (temp.Count() == 4)
                        {
                            var dir = temp.LastOrDefault();
                            AddList(list, dir);
                        }
                    }
                }
                return list.ToArray();
            }
            catch
            {
                return new string[] { };
            }
        }

        /// <summary>
        /// 获取文件夹列表
        /// </summary>
        public virtual string[] GetDirectoryList(string LocaPath)
        {
            try
            {
                string[] drectory = GetFilesDetailList(LocaPath);
                var list = new List<string>();
                foreach (string str in drectory)
                {
                    int dirPos = str.IndexOf("<DIR>");
                    if (dirPos > 0)
                    {
                        /*判断 Windows 风格*/
                        list.Add(str.Substring(dirPos + 5).Trim());
                    }
                    else if (str.Trim().Substring(0, 1).ToUpper() == "D")
                    {
                        /*判断 Unix 风格*/
                        string dir = str.Split(' ').Where(u => !string.IsNullOrEmpty(u) && !string.IsNullOrEmpty(u.Trim())).LastOrDefault();
                        AddList(list, dir);

                    }
                }
                return list.ToArray();
            }
            catch
            {
                return new string[] { };
            }
        }

        void AddList(List<string> list, string dir)
        {
            if (dir != null && !string.IsNullOrEmpty(dir.Trim()))
            {
                dir = dir.Trim();
                if (dir != "." && dir != "..")
                {
                    list.Add(dir);
                }
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName"></param>
        public void Delete(string fileName) 
        {
            ftpConnection.Delete(fileName);
        }


        /// <summary>
        /// 下载文件
        /// </summary>
        public void DownloadFile(string localPath, string remotePath, string remoteFile)
        {
            ftpConnection.Get(Combine(remotePath, remoteFile), localPath, remoteFile);
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        public void DeleteFtpDirWithAll(string sFolderName, bool isDeleteFolder = true) 
        {
            try
            {
                EACTSYSLOG.EACTSYSLOG.EactTrace("DeleteFtpDirWithAll", sFolderName);
                GetDirectoryList(sFolderName).ToList().ForEach(u =>
                {
                    DeleteFtpDirWithAll(Combine(sFolderName, u), true);
                });

                GetFileList(sFolderName).ToList().ForEach(u =>
                {
                    var file = Combine(sFolderName, u);
                    EACTSYSLOG.EACTSYSLOG.EactTrace("DeleteFile", file);
                    Delete(file);
                });

                if (isDeleteFolder)
                {
                    EACTSYSLOG.EACTSYSLOG.EactTrace("DeleteDirectory", sFolderName);
                    ftpConnection.RmDir(sFolderName);
                }

            }
            catch
            {
                throw;
            }
        }
        public bool DirectoryExist(string LocaPath, string RemoteDirectoryName) 
        {
            return DirectoryExist(Combine(LocaPath, RemoteDirectoryName));
        }

        /// <summary>
        /// 是否存在目录
        /// </summary>
        /// <param name="RemoteDirectoryName"></param>
        /// <returns></returns>
        public bool DirectoryExist(string RemoteDirectoryName) 
        {
            var result = false ;
            var strs=RemoteDirectoryName.Split('/').ToList();
            if (strs.Count > 0)
            {
                string preRemotePath = @"/";
                for (int i = 0; i < strs.Count - 1; i++)
                {
                    preRemotePath = Combine(preRemotePath, strs[i]);
                }
                result = GetDirectoryList(preRemotePath).Where(u => u == strs.Last()).Count() > 0;
            }
            else 
            {
                result = true;
            }

            return result;
        }

        public bool FileExist(string RemotePath,string RemoteFile) 
        {
            return GetFileList(RemotePath).FirstOrDefault(u => u == RemoteFile) != null;
        }

        /// <summary>
        /// 下载文件夹
        /// </summary>
        public void DownloadFolder(string sFolderPath, string remotePath, string sFolderName) 
        {
            _DownloadFolder(sFolderPath, sFolderName, remotePath);
        }


        void _DownloadFolder(string sFolderPath, string sFolderName, string remotePath = null)
        {
            try
            {
                if (!Directory.Exists(sFolderPath))
                {
                    Directory.CreateDirectory(sFolderPath);
                }

                if (remotePath != null)
                {
                    sFolderName = Combine(remotePath, sFolderName);
                }

                var FolderList = GetDirectoryList(sFolderName);
                var FileList = GetFileList(sFolderName);
                //查询一下是否有这个目录了
                foreach (var FL in FileList)
                {
                    if (FL != "." && FL != "..")
                    {
                        DownloadFile(sFolderPath, sFolderName, FL);
                    }
                }
                foreach (var FD in FolderList)
                {
                    if (FD != "." && FD != "..")
                    {
                        _DownloadFolder(Path.Combine(sFolderPath, FD), FD, sFolderName);
                    }
                }
            }
            catch
            {
                throw;
            }
        }



        public void MakeDirPath(string DirPath)
        {
            //首先查找路径中有没有/
            try
            {
                if (DirPath.IndexOf('/') < 0)
                {
                    if (!DirectoryExist(DirPath))
                    {
                        MakeDir(DirPath);
                    }
                }
                else
                {
                    var Path = DirPath.Split('/');
                    var PathName = string.Empty;
                    foreach (var P in Path)
                    {
                        if (!string.IsNullOrEmpty(P))
                        {
                            //首先判断这个路径下有没有这个文件夹
                            if (!DirectoryExist(PathName, P))
                            {
                                PathName += P + "/";
                                MakeDir(PathName);
                            }
                            else
                            {
                                PathName += P + "/";
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="dirName"></param>
        public void MakeDir(string dirName)
        {
            try
            {
                ftpConnection.MkDir(dirName);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 切换目录
        /// </summary>
        public void NextDirectory(string sFolderName)
        {
            try
            {
                ftpConnection.ChDir(sFolderName);
            }
            catch
            {
                throw new Exception(string.Format("没有发现对应下级目录{0}, 切换失败！", sFolderName));
            }
        }
        public virtual void UpLoadFile(string sFileName, string remotePath)
        {
            try
            {
                FileInfo fileInf = new FileInfo(sFileName);
                var remoteFile = Combine(remotePath, fileInf.Name);
                if (FileExist(remotePath, fileInf.Name))
                {
                    Delete(remoteFile);
                }
                ftpConnection.ChDir(remotePath);
                ftpConnection.Put(sFileName);
                ftpConnection.ChDir("..");
            }
            catch
            {
                throw;
            }
        }
        public virtual void UploadFolder(string localFolderPath, string remotePath)
        {
            DirectoryInfo dir = new DirectoryInfo(localFolderPath);
            var remoteDir = Combine(remotePath, dir.Name);
            //首先定位有没有这个文件夹
            if (DirectoryExist(remoteDir))
            {
                //如果当前路径下有这个文件夹，删除服务器上的我呢减价
                DeleteFtpDirWithAll(remoteDir);
            }
            _UploadFolder(localFolderPath, remotePath);
        }

        public void UploadFolder(string sFolderPath)
        {
            UploadFolder(sFolderPath, ftpConnection.RemotePath);
        }

        /// <summary>
        /// 上传文件夹（指定远程目录）
        /// </summary>
        /// <param name="localFolderPath">本地文件夹路径</param>
        /// <param name="remotePath">远程目录</param>
        void _UploadFolder(string localFolderPath, string remotePath)
        {
            try
            {
                string ex = remotePath;
                DirectoryInfo dir = new DirectoryInfo(localFolderPath);
                //创建目录
                MakeDir(Combine(ex, dir.Name));
                //在服务器上新建这个文件
                FileInfo[] allFile = dir.GetFiles();
                foreach (FileInfo fi in allFile)
                {
                    //上传文件
                    UpLoadFile(fi.FullName, Combine(ex, dir.Name));
                }
                DirectoryInfo[] allDir = dir.GetDirectories();
                foreach (DirectoryInfo d in allDir)
                {
                    _UploadFolder(d.FullName, Combine(ex, dir.Name));
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
