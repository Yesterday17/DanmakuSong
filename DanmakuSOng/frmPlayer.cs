using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DanmakuSong
{
    public partial class frmPlayer : Form
    {
        public frmPlayer()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            axWindowsMediaPlayer1.settings.autoStart = true;
        }

        public void Play(songInfo s)
        {
            /*
            WMPLib.IWMPPlaylist playlist = null;
            playlist = axWindowsMediaPlayer1.playlistCollection.newPlaylist("playlist");
            playlist.appendItem(axWindowsMediaPlayer1.newMedia(s.getSongURL()));
            axWindowsMediaPlayer1.currentPlaylist = playlist;
            axWindowsMediaPlayer1.Ctlcontrols.currentItem = playlist.Item[0];
            */
            axWindowsMediaPlayer1.URL = s.getSongURL();
        }
        public Boolean isStop()
        {
            //MessageBox.Show(axWindowsMediaPlayer1.status);
            return axWindowsMediaPlayer1.status == "已停止";
        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {
            if (this.FormBorderStyle == FormBorderStyle.None)
            {
                this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.None;
            }
        }

        private void axWindowsMediaPlayer1_ClickEvent(object sender, AxWMPLib._WMPOCXEvents_ClickEvent e)
        {
            if (this.FormBorderStyle == FormBorderStyle.None)
            {
                this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.None;
            }
        }

        private void frmPlayer_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
