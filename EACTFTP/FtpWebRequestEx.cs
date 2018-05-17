using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using WinSCP;

namespace TOOL
{
    /// <summary>
    /// FtpWebRequest拓展
    /// </summary>
    public class FtpWebRequestEx
    {
        NetworkCredential _networkCredential;
        string _ftpUri = string.Empty;
        Encoding _encoding = Encoding.Default;
        string _host;
        int _port;
        public void CloseFTP()
        {

        }

        /// <summary>
        /// 远程路径
        /// </summary>
        public string ServerDirectory { get; protected set; }

        string GetURI(string remote)
        {
            string uri = string.Empty;
            if (remote == "/")
            {
                uri = _ftpUri;
            }
            else
            {
                uri = Combine(_ftpUri, remote);
            }
            return uri;
        }

        public FtpWebRequestEx(string FtpServerIP, int FtpServerPort, string FtpRemotePath, string FtpUser, string FtpPassword)
        {
            ServerDirectory = "/";
            if (!string.IsNullOrEmpty(FtpRemotePath)) 
            {
                ServerDirectory = FtpRemotePath;
            }
            _networkCredential = new NetworkCredential(FtpUser, FtpPassword);
            _ftpUri = "ftp://" + string.Format("{0}:{1}", FtpServerIP, FtpServerPort);
            _host = FtpServerIP;
            _port = FtpServerPort;
        }

        private FtpWebResponse GetFtpResponse(string uri, string requestMethod,Action<FtpWebRequest> action=null)
        {
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(uri);
            request.Credentials = _networkCredential;
            request.KeepAlive = false;
            request.UseBinary = true;
            request.Method = requestMethod;

            if (action != null) 
            {
                action(request);
            }
            
            return GetFtpResponse(request);
        }

        private FtpWebResponse GetFtpResponse(FtpWebRequest request)
        {
            FtpWebResponse response = null;
            var info = new StringBuilder();
            info.AppendFormat("url={0}  method={1} ", request.RequestUri, request.Method);
            try
            {
                response = (FtpWebResponse)request.GetResponse();
                info.Append("验证完毕，服务器回应信息：[" + response.WelcomeMessage + "]");
                info.Append("正在连接：[ " + response.BannerMessage + "]");
                info.Append("连接成功，服务器返回的是：" + response.StatusCode + " " + response.StatusDescription);
                EACTSYSLOG.EACTSYSLOG.EactTrace("Response", info.ToString());
            }
            catch (WebException ex)
            {
                info.AppendFormat("发送错误。返回信息为={0}   {1}",ex.Status,ex.Message);
                EACTSYSLOG.EACTSYSLOG.EactTrace("Response", info.ToString());
                response = null;
                throw ex;
            }

            return response;
        }

        public string Combine(string remotePath, string RemoteFile)
        {
            return remotePath.TrimEnd('/') + "/" + RemoteFile.TrimStart('/');
        }

        /// <summary>
        /// 获取当前目录下明细(包含文件和文件夹)
        /// </summary>
        public List<RemoteFileInfo> GetFilesDetailList(string local)
        {
            try
            {
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Ftp,
                    HostName = _host,
                    PortNumber = _port,
                    UserName = _networkCredential.UserName,
                    Password = _networkCredential.Password,
                };

                using (var _ftpConnection = new Session())
                {
                    _ftpConnection.Open(sessionOptions);
                    var remotePath = Combine(_ftpConnection.HomePath, local);
                    var msg = new StringBuilder();
                    msg.AppendLine(remotePath);
                    var list = _ftpConnection.ListDirectory(remotePath).Files.ToList();
                    list.ForEach(u => {
                        msg.AppendLine(string.Format("{0} IsDirectory={1} IsParentDirectory={2} IsThisDirectory={3} FullName={4}", u.Name, u.IsDirectory, u.IsParentDirectory, u.IsThisDirectory, u.FullName));
                    });
                    EACTSYSLOG.EACTSYSLOG.EactTrace("GetFilesDetailList", msg.ToString());
                    return list;
                }
                
            }
            catch
            {
                return new List<RemoteFileInfo> { };
            }
            
        }


        /// <summary>
        /// 从Windows格式中返回文件信息
        /// </summary>
        /// <param name="Record">文件信息</param>
        private FileStruct ParseFileStructFromWindowsStyleRecord(string Record)
        {
            FileStruct f = new FileStruct();
            string processstr = Record.Trim();
            string dateStr = processstr.Substring(0, 8);
            processstr = (processstr.Substring(8, processstr.Length - 8)).Trim();
            string timeStr = processstr.Substring(0, 7);
            processstr = (processstr.Substring(7, processstr.Length - 7)).Trim();
            DateTimeFormatInfo myDTFI = new CultureInfo("en-US", false).DateTimeFormat;
            myDTFI.ShortTimePattern = "t";
            f.CreateTime = DateTime.Parse(dateStr + " " + timeStr, myDTFI);
            if (processstr.Substring(0, 5) == "<DIR>")
            {
                f.IsDirectory = true;
                processstr = (processstr.Substring(5, processstr.Length - 5)).Trim();
            }
            else
            {
                string[] strs = processstr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);   // true);
                processstr = strs[1];
                f.IsDirectory = false;
            }
            f.Name = processstr;
            return f;
        }

        /// <summary>
        /// 从Unix格式中返回文件信息
        /// </summary>
        /// <param name="Record">文件信息</param>
        private FileStruct ParseFileStructFromUnixStyleRecord(string Record)
        {
            FileStruct f = new FileStruct();
            string processstr = Record.Trim();
            f.Flags = processstr.Substring(0, 10);
            f.IsDirectory = (f.Flags[0] == 'd');
            processstr = (processstr.Substring(11)).Trim();
            _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);   //跳过一部分
            f.Owner = _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);
            f.Group = _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);
            _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);   //跳过一部分
            string yearOrTime = processstr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[2];
            if (yearOrTime.IndexOf(":") >= 0)  //time
            {
                processstr = processstr.Replace(yearOrTime, DateTime.Now.Year.ToString());
            }
            f.CreateTime = DateTime.Parse(_cutSubstringFromStringWithTrim(ref processstr, ' ', 8));
            f.Name = processstr;   //最后就是名称
            return f;
        }

        /// <summary>
        /// 按照一定的规则进行字符串截取
        /// </summary>
        /// <param name="s">截取的字符串</param>
        /// <param name="c">查找的字符</param>
        /// <param name="startIndex">查找的位置</param>
        private string _cutSubstringFromStringWithTrim(ref string s, char c, int startIndex)
        {
            int pos1 = s.IndexOf(c, startIndex);
            string retString = s.Substring(0, pos1);
            s = (s.Substring(pos1)).Trim();
            return retString;
        }


        /// <summary>
        /// 判断文件列表的方式Window方式还是Unix方式
        /// </summary>
        /// <param name="recordList">文件信息列表</param>
        private FileListStyle GuessFileListStyle(string[] recordList)
        {
            foreach (string s in recordList)
            {
                if (s.Length > 10
                 && Regex.IsMatch(s.Substring(0, 10), "(-|d)(-|r)(-|w)(-|x)(-|r)(-|w)(-|x)(-|r)(-|w)(-|x)"))
                {
                    return FileListStyle.UnixStyle;
                }
                else if (s.Length > 8
                 && Regex.IsMatch(s.Substring(0, 8), "[0-9][0-9]-[0-9][0-9]-[0-9][0-9]"))
                {
                    return FileListStyle.WindowsStyle;
                }
            }
            return FileListStyle.Unknown;
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
        /// 获取文件列表
        /// </summary>
        public virtual string[] GetFileList(string LocaPath)
        {
            try
            {
                var result = new List<string>();
                GetFilesDetailList(LocaPath).ForEach(u =>
                {
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
                    if (u.IsDirectory && !u.IsParentDirectory)
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
        /// 删除目录
        /// </summary>
        public void RmDir(string remotePath)
        {
            using (FtpWebResponse response = GetFtpResponse(GetURI(remotePath), WebRequestMethods.Ftp.RemoveDirectory)) 
            {
                if (response == null)
                {
                    throw new Exception("删除目录失败...");
                }
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        public void Delete(string remoteFile)
        {
            using (FtpWebResponse response = GetFtpResponse(GetURI(remoteFile), WebRequestMethods.Ftp.DeleteFile)) 
            {
                if (response == null)
                {
                    throw new Exception("删除文件失败...");
                }
            }
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
                    RmDir(sFolderName);
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
            var result = false;
            try
            {
                var strs = RemoteDirectoryName.Split('/').ToList();
                if (strs.Count > 0)
                {
                    string preRemotePath = @"/";
                    for (int i = 0; i < strs.Count - 1; i++)
                    {
                        preRemotePath = Combine(preRemotePath, strs[i]);
                    }
                    result = GetDirectoryList(preRemotePath).Where(u => u == strs.Last()).Count() > 0;
                }
            }
            catch(Exception ex)
            {
                result = false;
            }
            
            return result;
        }

        public bool FileExist(string RemotePath, string RemoteFile)
        {
            bool result = false;
            try
            {
                result = GetFileList(RemotePath).FirstOrDefault(u => u == RemoteFile) != null;
            }
            catch
            {
                result = false;
            }
            return result ;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        public void DownloadFile(string localPath, string remotePath, string remoteFile)
        {
           using( FtpWebResponse response = GetFtpResponse(GetURI(Combine(remotePath,remoteFile)), WebRequestMethods.Ftp.DownloadFile))
           {
               if (response == null)
               {
                   throw new Exception("下载文件失败...");
               }

               using (Stream responseStream = response.GetResponseStream())
               {
                   using (FileStream filestream = File.Create(Path.Combine(localPath, remoteFile)))
                   {
                       int buflength = 8196;
                       byte[] buffer = new byte[buflength];
                       int bytesRead = 1;

                       while (bytesRead != 0)
                       {
                           bytesRead = responseStream.Read(buffer, 0, buflength);
                           filestream.Write(buffer, 0, bytesRead);
                       }
                   }

               }
           }
            
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
            using (FtpWebResponse response = GetFtpResponse(GetURI(dirName), WebRequestMethods.Ftp.MakeDirectory)) 
            {
                if (response == null) 
                {
                    throw new Exception("创建文件夹失败...");
                }
            }
        }

        /// <summary>
        /// 切换目录
        /// </summary>
        public void NextDirectory(string sFolderName)
        {
            ServerDirectory = Combine(ServerDirectory, sFolderName);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
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
                var uri = GetURI(Combine(remotePath,fileInf.Name));

                using (FtpWebResponse response = GetFtpResponse(uri, WebRequestMethods.Ftp.UploadFile
                    , (request) =>
                    {
                        request.ContentLength = fileInf.Length;
                        int buflength = 8196;
                        byte[] buffer = new byte[buflength];
                        using (FileStream filestream = fileInf.OpenRead())
                        {
                            using (Stream responseStream = request.GetRequestStream())
                            {
                                int contenlength = filestream.Read(buffer, 0, buflength);
                                while (contenlength != 0)
                                {
                                    responseStream.Write(buffer, 0, contenlength);
                                    contenlength = filestream.Read(buffer, 0, buflength);
                                }
                            }
                        }
                    }
                    ))
                {
                    //TODO
                   
                }
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
            UploadFolder(sFolderPath, ServerDirectory);
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
    
    #region 文件信息结构
    public struct FileStruct
    {
        public string Flags;
        public string Owner;
        public string Group;
        public bool IsDirectory;
        public DateTime CreateTime;
        public string Name;
    }
    public enum FileListStyle
    {
        UnixStyle,
        WindowsStyle,
        Unknown
    }
    #endregion
}
