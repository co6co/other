using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Threading;


namespace H31DHTMgr
{
    internal class TorrentServer
    {
        public string UrlFormat { get; set; }
        public int Timeout { get; set; }

        public string GetUrlStr(string hashName)
        {
            //����������
            if (UrlFormat.IndexOf("{2}", StringComparison.Ordinal) > 0) return string.Format(UrlFormat, hashName.Substring(0, 2), hashName.Substring(hashName.Length - 2, 2), hashName);
            // //����������
            else if ((UrlFormat.IndexOf("{1}", StringComparison.Ordinal) > 0)) return string.Format(UrlFormat, hashName.Substring(0, 2),    hashName);
            //��һ������
            else if ((UrlFormat.IndexOf("{0}", StringComparison.Ordinal) > 0)) return string.Format(UrlFormat, hashName);
            return UrlFormat;
        }
    }

    public class H31Down
    {
        private string pathname=string.Empty;
        public bool webgood = true;
        private int downwebpos = 0;
       
     
        private List<TorrentServer> _serverList;
        private Random _random;
        public H31Down()
        {
            _serverList=new List<TorrentServer>();
            //_serverList.Add(new TorrentServer() { Timeout = 300, UrlFormat = "https://zoink.it/torrent/{0}.torrent" });//��ʱ
            _serverList.Add(new TorrentServer() { Timeout = 500, UrlFormat = "http://bt.box.n0808.com/{0}/{1}/{2}.torrent" });
            _serverList.Add(new TorrentServer() { Timeout = 1000, UrlFormat = "http://torrage.com/torrent/{0}.torrent" });
            _serverList.Add(new TorrentServer() { Timeout = 1000, UrlFormat = "http://torcache.net/torrent/{0}.torrent" });
            _random = new Random(DateTime.Now.Millisecond);
        }

        private int GetRandomServer()
        {
            return _random.Next(0, _serverList.Count);
        }

        #region ���ص��ڴ���ֱ��ʹ��
        public byte[] DownLoadFileByHashToByte(string hashname)
        {
            byte[] res = null;
            try
            {
                //�ȼ�鱾����û���ļ��������ֱ�Ӷ�ȡ���أ�û���ٴ�����������
                string filename = string.Format("{0}//{1}.torrent", pathname, hashname);
                if (File.Exists(filename))
                {
                    System.IO.FileStream TorrentFile = new System.IO.FileStream(filename, System.IO.FileMode.Open);
                    if (TorrentFile.Length > 0)
                    {
                        res = new byte[TorrentFile.Length];
                        TorrentFile.Read(res, 0, res.Length);
                        TorrentFile.Close();
                        return res;
                    }
                }
                
                //�����ǰ��������վ�е�һ������,��Ϊǰ��������վ�ٶȿ�Щ
                downwebpos = GetRandomServer();
                //res = DownLoadFileToSaveByte(m_strURLList[downwebpos]);

                //�������������ַ˳������,��ֹ��һ����վ���ع��౻��
                res = DownLoadFileToSaveByte(_serverList[downwebpos].GetUrlStr(hashname), _serverList[downwebpos].Timeout );
                if (res == null)
                {
                    downwebpos = GetRandomServer();
                    res = DownLoadFileToSaveByte(_serverList[downwebpos].GetUrlStr(hashname), _serverList[downwebpos].Timeout); 
                }

                return res;
            }
            catch (Exception e)
            {
                H31Debug.PrintLn(e.Message);
                return null;
            }
        }
        private byte[] DownLoadFileToSaveByte(string strURL,int timeout1)
        {
            Int32 ticktime1 = System.Environment.TickCount;
            byte[] result = null;
            try
            {
                Int32 ticktime2 = 0;
                byte[] buffer = new byte[4096];

                WebRequest wr = WebRequest.Create(strURL);
                wr.ContentType = "application/x-bittorrent";
                wr.Timeout = timeout1;
                WebResponse response = wr.GetResponse();
                int readsize = 0;
                {
                    bool gzip = response.Headers["Content-Encoding"] == "gzip";
                    Stream responseStream = gzip ? new GZipStream(response.GetResponseStream(), CompressionMode.Decompress) : response.GetResponseStream();

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        //responseStream.ReadTimeout = timeout1*2;
                        int count = 0;
                        do
                        {
                            count = responseStream.Read(buffer, 0, buffer.Length);
                            memoryStream.Write(buffer, 0, count);
                            readsize += count;
                        } while (count != 0);
                        ticktime2 = System.Environment.TickCount;

                        Thread.Sleep(10);
                        result = memoryStream.ToArray();
                    }
                    Int32 ticktime3 = System.Environment.TickCount;
                    //H31Debug.PrintLn("���سɹ�" + strURL + ":" + readsize.ToString() + ":" + (ticktime2 - ticktime1).ToString() + "-" + (ticktime3 - ticktime2).ToString());
                }
                wr.Abort();
                return result;
            }
            catch (Exception e)
            {
                 Int32 ticktime3 = System.Environment.TickCount;
                 H31Debug.PrintLn("����ʧ��" + strURL + ":" +  (ticktime3 - ticktime1).ToString()+"err:"+e.Message);
                return null;
            }
        }
        #endregion

        #region ���ص��ļ�
        public int DownLoadFileByHashToFile(string hashname)
        {
            try
            {
                if (pathname == string.Empty)
                {
                    string localfile = AppDomain.CurrentDomain.BaseDirectory;
                    pathname = Path.Combine(localfile, "Torrent");
                    if (!Directory.Exists(pathname))
                    {
                        Directory.CreateDirectory(pathname);
                        string tmpFolder = Path.Combine(pathname, "BAD");
                        if (!Directory.Exists(tmpFolder))
                        {
                            Directory.CreateDirectory(tmpFolder);
                        }

                    }
                }

                //������ļ����Ƿ����
                string pathname1 = Path.Combine(pathname ,hashname.Substring(hashname.Length - 2, 2));
                if (!Directory.Exists(pathname1))
                {
                    Directory.CreateDirectory(pathname1);
                }
                string filename = string.Format("{0}\\{1}\\{2}.torrent", pathname, hashname.Substring(hashname.Length - 2, 2), hashname);
                if (File.Exists(filename))  return 1;
               
 
                //�����һ����ַ����
                //downwebpos = (downwebpos + 1) % 2;
                //if (DownLoadFileToSaveFile(m_strURLList[downwebpos], filename) == 1)
                //    return 1;

                //�������������ַ˳������,��ֹ��һ����վ���ع��౻��
                downwebpos = (downwebpos + 1);
                //��������ַһһ��������
                foreach (var torrentServer in _serverList)
                {
                    if (1==DownLoadFileToSaveFile(torrentServer.GetUrlStr(filename), filename, torrentServer.Timeout)) return 1;
                } 
                return 0;
            }
            catch (Exception e)
            {
                H31Debug.PrintLn(e.Message);
                return -2;
            }
        }
        private int DownLoadFileToSaveFile(string strURL, string fileName,int timeout1)
        {
            Int32 ticktime1 = System.Environment.TickCount;
            try
            {
                Int32 ticktime2 = 0;
                byte[] buffer = new byte[4096];

                WebRequest wr = WebRequest.Create(strURL);
                wr.ContentType = "application/x-bittorrent";
                wr.Timeout = timeout1;
                WebResponse response = wr.GetResponse();
                int readsize = 0;
                {
                    bool gzip = response.Headers["Content-Encoding"] == "gzip";
                    Stream responseStream = gzip ? new GZipStream(response.GetResponseStream(), CompressionMode.Decompress) : response.GetResponseStream();

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        int count = 0;
                        do
                        {
                            count = responseStream.Read(buffer, 0, buffer.Length);
                            memoryStream.Write(buffer, 0, count);
                            readsize += count;
                        } while (count != 0);
                        ticktime2 = System.Environment.TickCount;

                        byte[] result = memoryStream.ToArray();
                        Thread.Sleep(10);
                        using (BinaryWriter writer = new BinaryWriter(new FileStream(fileName, FileMode.Create)))
                        {
                            writer.Write(result);
                        }
                    }
                    Int32 ticktime3 = System.Environment.TickCount;
                    //H31Debug.PrintLn("���سɹ�" + strURL + ":" + readsize.ToString() + ":" + (ticktime2 - ticktime1).ToString() + "-" + (ticktime3 - ticktime2).ToString());
                }
                return 1;
            }
            catch (WebException e)
            {
                Int32 ticktime3 = System.Environment.TickCount;
                if (e.Status == WebExceptionStatus.Timeout)//�ļ���ʱ
                {
                    return -2;
                }
                else if (e.Status == WebExceptionStatus.ProtocolError)//�ļ�������
                {
                    return -3;
                }
                else
                {
                    H31Debug.PrintLn("����ʧ��" + strURL + ":" + (ticktime3 - ticktime1).ToString() + e.Status.ToString() + e.Message);
                    return -4;
                }
            }
        }
        #endregion

    }
}
