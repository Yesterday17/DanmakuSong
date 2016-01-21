using System.Text;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace DanmakuSong
{
    public class songInfo
    {
        public songInfo()
        {
            this.songName = "NaN";
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
            this.lastSecond = 32767;

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

            string tmpUrl = json.Substring(json.IndexOf("\"mp3Url\":\"") + 10);
            string mp3Url = tmpUrl.Substring(0, tmpUrl.IndexOf("\""));

            string tmpName = json.Substring(json.IndexOf("\"name\":\"") + 8);
            string song_Name = tmpName.Substring(0, tmpName.IndexOf("\""));

            this.songID = id;
            this.songURL = mp3Url;
            this.songName = song_Name;
            this.by = commentUser;
            //持续时间暂时不用
            this.lastSecond = 32767;
        }
    }

}
