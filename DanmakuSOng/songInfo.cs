using System.Text;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.WebSockets;

namespace DanmakuSong
{
    public class songInfo
    {
        public songInfo()
        {
            this.songName = null;
            this.songID = null;
            this.lastSecond = 0;
            this.songURL = null;
            this.by = null;
        }

        public songInfo(string id, string commentUser)
        {
            string url = "http://music.163.com/api/song/detail/?id=" + id + "&ids=%5B" + id + "%5D&csrf_token=Method=GET";
            WebClient w = new WebClient();
            Stream sc = w.OpenRead(url);
            StreamReader sr = new StreamReader(sc, Encoding.UTF8);
            string json = sr.ReadToEnd();
            sr.Close();
            sc.Close();

            if (json == "{\"songs\":[],\"equalizers\":{},\"code\":200}")
            {
                err = true;
                return;
            }

            #region

            JObject jo = JObject.Parse(json);
            string mp3Url = jo["songs"]["mp3Url"].ToString();
            string song_Name = jo["songs"]["name"].ToString();
            int duration = System.Convert.ToInt32(jo["songs"]["duration"]);

            #endregion//使用Json处理

            #region
            /*
            //songInfo s = (songInfo)JsonConvert.DeserializeObject(json, typeof(songInfo));

            string tmpUrl = json.Substring(json.IndexOf("\"mp3Url\":\"") + 10);
            string mp3Url = tmpUrl.Substring(0, tmpUrl.IndexOf("\""));

            string tmpName = json.Substring(json.IndexOf("\"name\":\"") + 8);
            string song_Name = tmpName.Substring(0, tmpName.IndexOf("\""));

            //暂时用字符串处理代替json
            */
            #endregion //之前的字符串处理

            this.songID = id;
            this.songURL = mp3Url;
            this.songName = song_Name;
            this.by = commentUser;
            //持续时间暂时不用
            this.lastSecond = duration;

            //Debug
            /*
            if (id == "2333")
            {
                songName = "11";
                songID = "2333";
                songURL = "http://m2.music.126.net/I4zKjuKE-8RTVUFEW_GrUw==/7997847583068395.mp3";
                lastSecond = 222;
            }
            */

        }

        public string getSongName()
        {
            return songName;
        }

        public string getSongID()
        {
            return songID;
        }

        public long getLastSecond()
        {
            return lastSecond;
        }

        public string getSongURL()
        {
            return songURL;
        }

        public string getBy()
        {
            return by;
        }

        private string songName;
        private string songID;
        private long lastSecond;
        private string songURL;
        private string by;
        public bool err = false;

        public songInfo(string id, string json, string commentUser)
        {
            if (json == "{\"message\":\"no resource\",\"code\":404}")
            {
                err = true;
                return;
            }

            #region

            JObject jo = JObject.Parse(json);
            string mp3Url = jo["songs"]["mp3Url"].ToString();
            string song_Name = jo["songs"]["name"].ToString();
            int duration = System.Convert.ToInt32(jo["songs"]["duration"]);

            #endregion//使用Json处理

            #region
            /*
            //songInfo s = (songInfo)JsonConvert.DeserializeObject(json, typeof(songInfo));

            string tmpUrl = json.Substring(json.IndexOf("\"mp3Url\":\"") + 10);
            string mp3Url = tmpUrl.Substring(0, tmpUrl.IndexOf("\""));

            string tmpName = json.Substring(json.IndexOf("\"name\":\"") + 8);
            string song_Name = tmpName.Substring(0, tmpName.IndexOf("\""));

            //暂时用字符串处理代替json
            */
            #endregion //之前的字符串处理

            this.songID = id;
            this.songURL = mp3Url;
            this.songName = song_Name;
            this.by = commentUser;
            //持续时间暂时不用
            this.lastSecond = duration;
        }

        public bool songListError(string json)
        {
            err = false;
            if (json == "{\"message\":\"no resource\",\"code\":404}")
            {
                err = true;
            }

            return err;
        }

        public songInfo(string id, string name, long last, string Url, string by)
        {
            this.songID = id;
            this.songName = name;
            this.lastSecond = last;
            this.songURL = Url;
            this.by = by;
            this.err = false;
        }

        public void HttpDownloadFile(string path)
        {
            // 设置参数
            HttpWebRequest request = WebRequest.Create(this.songURL) as HttpWebRequest;
            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream responseStream = response.GetResponseStream();
            //创建本地文件写入流
            Stream stream = new FileStream(path, FileMode.Create);
            byte[] bArr = new byte[1024];
            int size = responseStream.Read(bArr, 0, (int)bArr.Length);
            while (size > 0)
            {
                stream.Write(bArr, 0, size);
                size = responseStream.Read(bArr, 0, (int)bArr.Length);
            }
            stream.Close();
            responseStream.Close();
        }
    }

}
