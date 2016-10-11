using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Timers;
using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.GZip;
using System.Reflection;

namespace KcptunGUI
{
    public class KcptunUtils
    {
        public static readonly string KcptunPath = Path.Combine(Path.GetTempPath(), "kcptun");
        private static readonly string KcptunReleaseURL = "https://api.github.com/repos/xtaci/kcptun/releases";
        public static readonly string KcptunClient32 = "kcptun_client_x86.exe";
        public static readonly string KcptunClient64 = "kcptun_client_x64.exe";
        public static readonly string KcptunServer32 = "kcptun_server_x86.exe";
        public static readonly string KcptunServer64 = "kcptun_server_x64.exe";
        private static readonly string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.0 Safari/537.36";

        public event EventHandler UpdateAvailable;
        public Dictionary<string, string> UpdateList { get; private set; }

        public void ExtractEmbeddedKcptunBinary()
        {
            try
            {
                if (!File.Exists(Path.Combine(KcptunPath, KcptunClient32)))
                {
                    ExtractEmbeddedKcptunBinary(KcptunClient32);
                }
                if (!File.Exists(Path.Combine(KcptunPath, KcptunClient64)))
                {
                    ExtractEmbeddedKcptunBinary(KcptunClient64);
                }
                if (!File.Exists(Path.Combine(KcptunPath, KcptunServer32)))
                {
                    ExtractEmbeddedKcptunBinary(KcptunServer32);
                }
                if (!File.Exists(Path.Combine(KcptunPath, KcptunServer64)))
                {
                    ExtractEmbeddedKcptunBinary(KcptunServer64);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ExtractEmbeddedKcptunBinary(string kcptunResource)
        {
            using (Stream st = Assembly.GetExecutingAssembly().GetManifestResourceStream("KcptunGUI.kcptun." + kcptunResource))
            {
                using (FileStream fs = new FileStream(Path.Combine(KcptunPath, kcptunResource), FileMode.Create))
                {
                    byte[] buffer = new byte[4096];
                    while (true)
                    {
                        int count = st.Read(buffer, 0, buffer.Length);
                        if (count > 0)
                        {
                            fs.Write(buffer, 0, count);
                        }
                        else
                        {
                            break;
                        }
                    }
                    fs.Close();
                    st.Close();
                }
            }
        }

        public bool CreateKcptunFolder()
        {
            try
            {
                if (!Directory.Exists(KcptunPath))
                {
                    Directory.CreateDirectory(KcptunPath);
                }
                string path = Path.Combine(KcptunPath, "tmp");
                DirectoryInfo di = new DirectoryInfo(path);
                if (di.Exists)
                {
                    di.Delete(true);
                }
                Directory.CreateDirectory(path);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void CheckKcptunUpdate(int delay)
        {
            Timer timer = new Timer(delay);
            timer.AutoReset = false;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
        }

        public bool UpdateKcptunBinary()
        {
            if (UpdateList.Count == 0)
            {
                return false;
            }
            try
            {
                foreach (var item in this.UpdateList)
                {
                    File.Delete(item.Key);
                    File.Move(item.Value, item.Key);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Timer timer = sender as Timer;
            timer.Elapsed -= Timer_Elapsed;
            timer.Enabled = false;
            timer.Dispose();
            if (await DownloadLatestKcptunRelease())
            {
                UpdateAvailable(this, new EventArgs());
            }
        }

        private async Task<bool> DownloadLatestKcptunRelease()
        {
            try
            {
                var downloadList = await GetLatestKcptunRelease();
                if (downloadList == null || downloadList.Count == 0)
                {
                    return false;
                }
                List<Task> tasks = new List<Task>();
                foreach (var item in downloadList)
                {
                    tasks.Add(DownloadKcptunBinary(Path.Combine(KcptunPath, "tmp", item.Key), item.Value));
                }
                await Task.WhenAll(tasks);
                tasks.Clear();
                foreach (var item in downloadList)
                {
                    tasks.Add(Task.Run(
                        () => ExtractKcptunFile(Path.Combine(KcptunPath, "tmp", item.Key), Path.Combine(KcptunPath, "tmp"))));
                }
                await Task.WhenAll(tasks);
                this.UpdateList = await CheckBinaryUpdate();
                if (this.UpdateList.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private async Task<Dictionary<string, string>> GetLatestKcptunRelease()
        {
            try
            {
                WebClient client = new WebClient();
                client.Headers.Add("User-Agent", UserAgent);
                client.Proxy = WebRequest.GetSystemWebProxy();
                string json = await client.DownloadStringTaskAsync(new Uri(KcptunReleaseURL));
                var releaseList = JsonConvert.DeserializeObject(json, typeof(List<KcptunRelease>)) as List<KcptunRelease>;
                KcptunRelease latestRelease = null;
                DateTime latestUpdateTime = DateTime.MinValue;
                foreach (var r in releaseList)
                {
                    if (!r.prelease && r.published_at > latestUpdateTime)
                    {
                        latestUpdateTime = r.published_at;
                        latestRelease = r;
                    }
                }
                if (latestRelease == null)
                {
                    return null;
                }
                Dictionary<string, string> downloadList = new Dictionary<string, string>();
                foreach (var asset in latestRelease.assets)
                {
                    if (asset.name.Contains("windows"))
                    {
                        downloadList.Add(asset.name, asset.browser_download_url);
                    }
                }
                return downloadList;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private Task DownloadKcptunBinary(string file, string url)
        {
            string fileFullPath = Path.Combine(KcptunPath, "tmp", file);
            WebClient client = new WebClient();
            client.Headers.Add("User-Agent", UserAgent);
            client.Proxy = WebRequest.GetSystemWebProxy();
            return client.DownloadFileTaskAsync(new Uri(url), fileFullPath);
        }

        private bool ExtractKcptunFile(string gzArchiveName, string destFolder)
        {
            try
            {
                using (Stream stream = File.OpenRead(gzArchiveName))
                {
                    using (Stream gzipStream = new GZipInputStream(stream))
                    {
                        using (TarArchive tarArchive = TarArchive.CreateInputTarArchive(gzipStream))
                        {
                            tarArchive.ExtractContents(destFolder);
                            tarArchive.Close();
                            gzipStream.Close();
                            stream.Close();
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private async Task<Dictionary<string, string>> CheckBinaryUpdate()
        {
            Dictionary<string, string> updateList = new Dictionary<string, string>();
            string downloaded = GetKcpBinaryFileName(Path.Combine(KcptunPath, "tmp"), "windows", "server", "386", "exe");
            string original = Path.Combine(KcptunPath, KcptunServer32);
            if (!await Task.Run(() => CompareSha256(downloaded, original)))
            {
                updateList.Add(original, downloaded);
            }
            downloaded = GetKcpBinaryFileName(Path.Combine(KcptunPath, "tmp"), "windows", "server", "amd64", "exe");
            original = Path.Combine(KcptunPath, KcptunServer64);
            if (!!await Task.Run(() => CompareSha256(downloaded, original)))
            {
                updateList.Add(original, downloaded);
            }
            downloaded = GetKcpBinaryFileName(Path.Combine(KcptunPath, "tmp"), "windows", "client", "386", "exe");
            original = Path.Combine(KcptunPath, KcptunClient32);
            if (!!await Task.Run(() => CompareSha256(downloaded, original)))
            {
                updateList.Add(original, downloaded);
            }
            downloaded = GetKcpBinaryFileName(Path.Combine(KcptunPath, "tmp"), "windows", "client", "amd64", "exe");
            original = Path.Combine(KcptunPath, KcptunClient64);
            if (!!await Task.Run(() => CompareSha256(downloaded, original)))
            {
                updateList.Add(original, downloaded);
            }
            return updateList;
        }

        private string GetKcpBinaryFileName(string path, params string[] expressions)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            foreach (FileInfo f in di.GetFiles())
            {
                bool flag = true;
                foreach (string str in expressions)
                {
                    if (!f.Name.Contains(str))
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    return f.FullName;
                }
            }
            return null;
        }

        private bool CompareSha256(string file1, string file2)
        {
            string tmp = GetSha256(file1);
            if (tmp != null)
            {
                return tmp.Equals(GetSha256(file2));
            }
            else
            {
                return false;
            }
        }

        private string GetSha256(string file)
        {
            try
            {
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    using (SHA256 sha256 = new SHA256Managed())
                    {
                        byte[] tmpByte = sha256.ComputeHash(fs);
                        sha256.Clear();
                        return Convert.ToBase64String(tmpByte);
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private class KcptunRelease
        {
            public bool prelease;
            public DateTime published_at;
            public List<Asset> assets;

            public class Asset
            {
                public string name;
                public string browser_download_url;
            }
        }
    }
}
