using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace VoiceWav
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void btnSpeak_Click(object sender, EventArgs e)
        {
            var content = this.tbText.Text;
            int rate = Convert.ToInt32(this.cbRate.Text);
            SpeechVocie.Speak(content, rate);
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = "选择存放目录";
            if (folder.ShowDialog() == DialogResult.OK)
            {
                string basePath = folder.SelectedPath;
                var content = this.tbText.Text;

                var arr = content.Replace("\r", "").Split(new char[] { '\n' });
                int rate = Convert.ToInt32(this.cbRate.Text);
                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }

                this.btnSpeak.Enabled = false;
                this.btnExport.Enabled = false;

                this.Text = this.Text + "(正在导出请稍后)";

                var thread = new Thread(new ThreadStart(() =>
                {
                    foreach (var item in arr)
                    {
                        var savePath = Path.Combine(basePath, SafeFileName(item) + ".wav");
                        SpeechVocie.SpeakToFile(item, savePath, rate);
                        if (tbResult.InvokeRequired)
                        {
                            this.tbResult.Invoke(new Action(() => { tbResult.Text = "Speek wav " + item; }));
                        }
                    }
                    if (this.btnSpeak.InvokeRequired)
                    {
                        this.btnSpeak.Invoke(new Action(() => { btnSpeak.Enabled = true; }));
                    }

                    if (this.btnExport.InvokeRequired)
                    {
                        this.btnExport.Invoke(new Action(() => { btnExport.Enabled = true; }));
                    }
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() => { this.Text = this.Text.Replace("(正在导出请稍后)", ""); }));
                    }

                    if (this.tbResult.InvokeRequired)
                    {
                        this.tbResult.Invoke(new Action(() => { tbResult.Text = ""; }));
                    }
                }))
                { IsBackground = true };
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
        }
        string SafeFileName(string s)
        {
            //名称处理\:/*?"<>|
            s = System.Text.RegularExpressions.Regex.Replace(s, "[\\\\:/*?\"<>\r\n\\s]", "_");
            return s.Length > 10 ? s.Substring(0, 10) : s;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.cbRate.Text = "1";
        }



        private void tbText_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null)
                return;
            if (e.KeyChar == (char)1)
            {
                textBox.SelectAll();
                e.Handled = true;
            }
        }
    }
}
