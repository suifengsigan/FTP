using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WinSCP;

namespace TOOL
{
    public class WinSCPEx
    {
        Session _ftpConnection;
        TransferMode TransferMode = TransferMode.Binary;
        TransferOptions _transferOptions = new TransferOptions();

        ~WinSCPEx() 
        {
            try { CloseFTP(); }
            catch { }
        }

        public void CloseFTP() 
        {
            _ftpConnection.Dispose();
        }
        public WinSCPEx(string FtpServerIP, int FtpServerPort, string FtpRemotePath, string FtpUser, string FtpPassword)
        {
            SessionOptions sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Ftp,
                HostName = FtpServerIP,
                PortNumber = FtpServerPort,
                UserName = FtpUser,
                Password = FtpPassword,
            };

            _ftpConnection = new Session();
            _ftpConnection.Open(sessionOptions);
            _transferOptions.TransferMode = TransferMode;

            NextDirectory(FtpRemotePath);
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
        public List<RemoteFileInfo> GetFilesDetailList(string local)
        {
            try
            {
                var list = _ftpConnection.ListDirectory(local).Files.ToList();
                return list;
            }
            catch
            {
                return new List<RemoteFileInfo> { };
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
                var result = new List<string>();
                GetFilesDetailList(LocaPath).ForEach(u => {
                    if (!u.IsDirectory) 
                    {
                        result.Add(u.Name);
                    }
                });
                return result.ToArray();
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
                var result = new List<string>();
                GetFilesDetailList(LocaPath).ForEach(u =>
                {
                    if (u.IsDirectory&&!u.IsParentDirectory)
                    {
                        result.Add(u.Name);
                    }
                });
                return result.ToArray();
            }
            catch
            {
                return new string[] { };
            }
        }


        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName"></param>
        public void Delete(string fileName)
        {
            _ftpConnection.RemoveFiles(fileName);
        }


        /// <summary>
        /// 下载文件
        /// </summary>
        public void DownloadFile(string localPath, string remotePath, string remoteFile)
        {
            _ftpConnection.GetFiles(Combine(remotePath, remoteFile), Path.Combine(localPath, remoteFile),false,_transferOptions).Check();
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
                    Delete(sFolderName);
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
            return _ftpConnection.FileExists(RemoteDirectoryName);
        }

        public bool FileExist(string RemotePath, string RemoteFile)
        {
            return _ftpConnection.FileExists(Combine(RemotePath,RemoteFile));
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
            _ftpConnection.CreateDirectory(DirPath);
        }


        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="dirName"></param>
        public void MakeDir(string dirName)
        {
            _ftpConnection.CreateDirectory(dirName);
        }

        /// <summary>
        /// 切换目录
        /// </summary>
        public void NextDirectory(string sFolderName)
        {
            try
            {
                //TODO
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

                _ftpConnection.PutFiles(sFileName, remoteFile, false, _transferOptions).Check();
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
            UploadFolder(sFolderPath, _ftpConnection.ExecutablePath);
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
