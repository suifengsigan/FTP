using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WinSCP;

namespace TOOL
{
    public class WinSCPFTP : IEACTFTP
    {
        Session _session;
        ~WinSCPFTP()
        {
            try
            {
                using (_session)
                {
                }
            }
            catch (Exception ex)
            {
                WriteLog("~WinSCPFTP", ex.Message);
            }
        }

        public void CloseFTP() { }

        public WinSCPFTP(string FtpServerIP, string FtpRemotePath, string FtpUserID, string FtpPassword, bool SSL)
        {
            var strs = FtpServerIP.Split(':');
            // Setup session options
            var sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Ftp,
                PortNumber= int.Parse(strs[1]),
                HostName = strs[0],
                UserName = FtpUserID,
                Password = FtpPassword
                //,SshHostKeyFingerprint = "ssh-rsa 2048 xx:xx:xx:xx:xx:xx:xx:xx:..."
            };
            _session = new Session();
            // Connect
            _session.Open(sessionOptions);
        }
        public WinSCPFTP(string FtpServerIP, int FtpServerPort, string FtpRemotePath, string FtpUser, string FtpPassword)
        {
            // Setup session options
            var sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Ftp,
                PortNumber= FtpServerPort,
                HostName = FtpServerIP,
                UserName = FtpUser,
                Password = FtpPassword
                //,SshHostKeyFingerprint = "ssh-rsa 2048 xx:xx:xx:xx:xx:xx:xx:xx:..."
            };

            _session = new Session();
            // Connect
            _session.Open(sessionOptions);
        }

        public void Delete(string fileName)
        {
            _session.RemoveFiles(fileName);
        }

        public void DeleteFtpDirWithAll(string sFolderName, bool isDeleteFolder = true)
        {
            try
            {
                var fileInfos = _session.ListDirectory(sFolderName).Files.ToList();
                foreach (var fileInfo in fileInfos)
                {
                    if (fileInfo.Name != "." && fileInfo.Name != "..")
                    {
                        if (fileInfo.IsDirectory)
                            DeleteFtpDirWithAll(sFolderName + "/" + fileInfo.Name, true);
                        else
                            Delete(sFolderName + "/" + fileInfo.Name);
                    }
                }
                if (isDeleteFolder)
                {
                    Delete(sFolderName);
                }

            }
            catch
            {
                throw;
            }
        }

        public bool DirectoryExist(string RemoteDirectoryName)
        {
            return _session.FileExists(RemoteDirectoryName);
        }

        public bool DirectoryExist(string LocaPath, string RemoteDirectoryName)
        {
            try
            {
                var dirList = GetDirectoryList(LocaPath);
                foreach (var str in dirList)
                {
                    if (str == RemoteDirectoryName)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public void DownloadFile(string localPath, string remotePath, string remoteFile)
        {
            try
            {
                //var array = ftpConnection.DownloadByteArray(remotePath + "/" + remoteFile);
                //File.WriteAllBytes(Path.Combine(localPath, remoteFile), array);
                //_session.GetFiles()
            }
            catch
            {
                throw;
            }
        }

        public void DownloadFolder(string sFolderPath, string remotePath, string sFolderName)
        {

        }

        public string[] GetDirectoryList(string LocaPath)
        {
            var result = new List<string> { };
            var files = _session.ListDirectory(LocaPath);
            files.Files.ToList().ForEach(u => {
                if (u.IsDirectory && u.Name != "..")
                {
                    result.Add(u.Name);
                }
            });
            return result.ToArray();
        }

        public string[] GetFileList(string LocaPath)
        {
            var result = new List<string> { };
            var files = _session.ListDirectory(LocaPath);
            files.Files.ToList().ForEach(u => {
                if (!u.IsDirectory)
                {
                    result.Add(u.Name);
                }
            });
            return result.ToArray();
        }

        public void MakeDir(string dirName)
        {
            try
            {
                _session.CreateDirectory(dirName);
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
            catch
            {
                throw;
            }
        }

        public void NextDirectory(string sFolderName)
        {
        }

        public void UpLoadFile(string sFileName, string remotePath)
        {
            FileInfo fileInf = new FileInfo(sFileName);
            var remoteFile = Combine(remotePath, fileInf.Name);
            //首先定位有没有这个文件
            if (FileExist(remoteFile))
            {
                //如果当前路径下有这个文件夹，删除服务器上的文件
                Delete(remoteFile);
            }
            _UploadFile(fileInf.FullName, remotePath);
        }

      
        public void UpLoadFile(string sFileName)
        {
            try
            {
                FileInfo fileInf = new FileInfo(sFileName);
                //首先定位有没有这个文件
                if (FileExist(fileInf.Name))
                {
                    //如果当前路径下有这个文件夹，删除服务器上的文件
                    Delete(fileInf.Name);
                }
                _UploadFile(fileInf.FullName,_session.HomePath);
            }
            catch
            {
                throw;
            }
        }

        public void UploadFolder(string sFolderPath)
        {
            UploadFolder(sFolderPath, _session.HomePath);
        }

        public void UploadFolder(string localFolderPath, string remotePath)
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

        /// <summary>
        /// 上传文件夹（指定远程目录）
        /// </summary>
        /// <param name="localFolderPath">本地文件夹路径</param>
        /// <param name="remotePath">远程目录</param>
        public void _UploadFolder(string localFolderPath, string remotePath)
        {
            try
            {
                string ex = remotePath + "/";
                DirectoryInfo dir = new DirectoryInfo(localFolderPath);
                //创建目录
                _session.CreateDirectory(ex + dir.Name);
                //在服务器上新建这个文件
                FileInfo[] allFile = dir.GetFiles();
                foreach (FileInfo fi in allFile)
                {
                    //上传文件
                    _UploadFile(fi.FullName, ex + dir.Name);
                }
                DirectoryInfo[] allDir = dir.GetDirectories();
                foreach (DirectoryInfo d in allDir)
                {
                    _UploadFolder(d.FullName, ex + dir.Name);
                }
            }
            catch
            {
                throw;
            }
        }



        public bool FileExist(string RemoteFileName)
        {
            try
            {
                return _session.FileExists(RemoteFileName);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="sFileName">上传文件的路径</param>
        /// <param name="sFTPURI">FTP服务器目标地址</param>
        /// <param name="backgroundWorker"></param>
        private void _UploadFile(string sFileName, string remotePath)
        {
            try
            {
                // Upload files
                TransferOptions transferOptions = new TransferOptions();
                transferOptions.TransferMode = TransferMode.Binary;

                TransferOperationResult transferResult;
                transferResult =
                    _session.PutFiles(sFileName, Combine(remotePath, "/*.*"), false, transferOptions);

                // Throw on any error
                transferResult.Check();

                // Print results
                foreach (TransferEventArgs transfer in transferResult.Transfers)
                {
                    var msg = string.Format("Upload of {0} succeeded", transfer.FileName);
                    WriteLog("UpLoadFile", msg);
                }
            }
            catch
            {
                throw;
            }
        }


        public string Combine(string remotePath, string RemoteFile)
        {
            return remotePath.TrimEnd('/') + "/" + RemoteFile.TrimStart('/');
        }

        void WriteLog(string name, string msg)
        {
            Console.WriteLine("{0}:{1}", name, msg);
            EACTSYSLOG.EACTSYSLOG.EactTrace(name, msg);
        }


    }
}
