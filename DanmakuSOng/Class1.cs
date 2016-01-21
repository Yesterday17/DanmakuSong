using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.IO;

namespace DanmakuSong
{
    public class Class1 : BilibiliDM_PluginFramework.DMPlugin
    {
        frmSongList l_frmSongList = new frmSongList();
        public Class1()
        {
            this.ReceivedDanmaku += Class1_ReceivedDanmaku;
            this.PluginAuth = "昨日的十七号";
            this.PluginName = "DanmakuSong";
            this.PluginDesc = "通过简单的弹幕文字识别实现点歌的功能，音乐源网易云音乐。";
            this.PluginCont = "yesterday17@yesterday17.cn";
            this.PluginVer = "v0.0.1";
        }

        //接受弹幕 估计是整个程序的核心了吧
        private void Class1_ReceivedDanmaku(object sender, BilibiliDM_PluginFramework.ReceivedDanmakuArgs e)
        {
            string d_txt = e.Danmaku.CommentText;
            string d_by = e.Danmaku.CommentUser;

            if (d_txt == null) return;
            if (d_txt.Substring(0, 2) == "点歌")
            {
                string id = d_txt.Substring(3);
                songInfo newsong = new songInfo(id, d_by);
                if (!newsong.err)
                {
                    l_frmSongList.addSong(newsong);
                }
                else
                {
                    AddDM(d_by + "输入的歌曲id：" + id + "有误！");
                }
            }
            if (d_txt.Substring(0, 2) == "歌单")
            {
                //原有歌曲数量
                int oldnum = l_frmSongList.getSongNum();

                if (e.Danmaku.isAdmin == true)
                {
                    string id = d_txt.Substring(3);

                    string url = "http://music.163.com/api/playlist/detail?id=" + id;
                    WebClient w = new WebClient();
                    Stream sc = w.OpenRead(url);
                    StreamReader sr = new StreamReader(sc, Encoding.UTF8);
                    string json = sr.ReadToEnd();
                    sr.Close();
                    sc.Close();

                    Newtonsoft.Json.Linq.JObject jo = Newtonsoft.Json.Linq.JObject.Parse(json);
                    int i = 0;

                    while (i < jo["result"]["tracks"].Count())
                    {
                        string name = jo["result"]["tracks"][i]["name"].ToString();
                        string m_url = jo["result"]["tracks"][i]["mp3Url"].ToString();
                        long last = Convert.ToInt32(jo["result"]["tracks"][i]["duration"]);
                        songInfo newsong = new songInfo(id, name, last, m_url, d_by);

                        l_frmSongList.addSong(newsong);

                        i++;
                    }
                    #region

                    /*
                    json = json.Substring(json.IndexOf("popularity") + 10);

                    while (json.IndexOf("mp3Url") != -1 && json.IndexOf("popularity") != -1)
                    {
                        songInfo newsong = new songInfo(id, json, d_by);
                        if (!newsong.err)
                        {
                            l_frmSongList.addSong(newsong);
                            json = json.Substring(json.IndexOf("popularity") + 10);
                        }
                        else
                        {
                            AddDM(d_by + "输入的歌单id：" + id + "有误！");
                            break;
                        }
                    }

                    //补充处理
                    l_frmSongList.addSong(new songInfo(id, json, d_by));

                    //l_frmSongList.delSong(oldnum+1);
                    */
                    #endregion
                }
                else
                {
                    AddDM("只有房管才能点歌单！");
                }
            }
            //throw new NotImplementedException();
        }

        //管理插件
        public override void Admin()
        {
            string d_txt = Input.InputBox.ShowInputBox("请输入需要发送的弹幕！", string.Empty);
            string d_by = "DS";

            if (d_txt.Substring(0, 2) == "点歌")
            {
                d_by = d_by + " - 单曲";
                string id = d_txt.Substring(3);
                songInfo newsong = new songInfo(id, d_by);
                if (!newsong.err)
                {
                    l_frmSongList.addSong(newsong);
                }
                else
                {
                    AddDM(d_by + "输入的歌曲id：" + id + "有误！");
                }
            }
            if (d_txt.Substring(0, 2) == "歌单")
            {
                d_by = d_by + " - 歌单";

                string id = d_txt.Substring(3);
                string url = "http://music.163.com/api/playlist/detail?id=" + id;
                WebClient w = new WebClient();
                Stream sc = w.OpenRead(url);
                StreamReader sr = new StreamReader(sc, Encoding.UTF8);
                string json = sr.ReadToEnd();
                sr.Close();
                sc.Close();

                Newtonsoft.Json.Linq.JObject jo = Newtonsoft.Json.Linq.JObject.Parse(json);
                int i = 0;

                while (i < jo["result"]["tracks"].Count())
                {
                    string name = jo["result"]["tracks"][i]["name"].ToString();
                    string m_url = jo["result"]["tracks"][i]["mp3Url"].ToString();
                    long last = Convert.ToInt32(jo["result"]["tracks"][i]["duration"]);
                    songInfo newsong = new songInfo(id, name, last, m_url, d_by);
                    l_frmSongList.addSong(newsong);
                    i++;
                }
            }
            else
            {
                AddDM(d_txt);
            }
            base.Admin();
        }

        //停止插件使用
        public override void Stop()
        {
            l_frmSongList.Hide();
            AddDM("点歌插件已禁止使用。");
            base.Stop();
            //請勿使用任何阻塞方法
        }

        //启动插件使用
        public override void Start()
        {
            //初始化歌曲列表

            l_frmSongList.Show();
            AddDM("点歌插件已经启动，在弹幕中输入 点歌 [歌曲ID] 就可以在播放列表中加入您想要的歌曲！");
            base.Start();
            //請勿使用任何阻塞方法
        }
    }
}
