using System;
using System.Collections;
using System.Windows.Forms;

namespace DanmakuSong
{
    public partial class frmSongList : Form
    {
        frmPlayer p = new frmPlayer();
        Queue playlist = new Queue();
        bool show_player = false;

        public frmSongList()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void frmSongList_Load(object sender, EventArgs e)
        {
            ColumnHeader play = new ColumnHeader();
            play.Text = "　";
            play.Width = 20;
            play.TextAlign = HorizontalAlignment.Left;
            this.listView1.Columns.Add(play);

            ColumnHeader songName = new ColumnHeader();
            songName.Text = "歌曲名称";
            songName.Width = 100;
            songName.TextAlign = HorizontalAlignment.Left;
            this.listView1.Columns.Add(songName);

            ColumnHeader by = new ColumnHeader();
            by.Text = "点歌人";
            by.Width = 100;
            by.TextAlign = HorizontalAlignment.Left;
            this.listView1.Columns.Add(by);
        }

        public void addSong(songInfo i)
        {
            ListViewItem l = new ListViewItem();
            l.Text = "　";
            l.SubItems.Add(i.getSongName());
            l.SubItems.Add(i.getBy());
            listView1.Items.Add(l);

            playlist.Enqueue(i);
            if (listView1.Items.IndexOf(l) == 0)
            {
                listView1.Items[0].Text = "√";
                p.Play(i);
            }
        }

        public void delSong(int i)
        {
            //暂时先缓缓
            listView1.Items.RemoveAt(i);
            //listView1.Items[i].Text = "√";
            songInfo s = playlist.Dequeue() as songInfo;
        }

        private void t_Tick(object sender, EventArgs e)
        {
            if (p.isStop())
            {
                if (playlist.Count <= 1)
                {
                    return;
                }

                listView1.Items.RemoveAt(0);
                listView1.Items[0].Text = "√";
                //删除list中第一首歌曲 给第二首打上勾
                songInfo s = playlist.Dequeue() as songInfo;
                p.Play(s);
            }
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (this.FormBorderStyle == FormBorderStyle.FixedToolWindow)
            {
                this.FormBorderStyle = FormBorderStyle.None;
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            }
        }

        private void frmSongList_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (show_player == false)
            {
                show_player = true;
                p.Show();
            }
            else
            {
                show_player = false;
                p.Hide();
            }
            e.Cancel = true;
        }
        
        public int getSongNum()
        {
            return listView1.Items.Count;
        }
    }
}
