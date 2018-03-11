using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 阿里云ASR录音文件识别
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        delegate string Method(string tiaojian);
        Method method;
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                textBox2.Text = "";
                method = new Method(Transcription.GetData);
                method.BeginInvoke(textBox1.Text.Trim(), CallBack, null);
            }
            else
            {
                MessageBox.Show("文件oss地址不能为空");
            }
        }
        public void CallBack(IAsyncResult result)
        {
            this.Invoke(new Action(()=> {
                textBox2.Text = method.EndInvoke(result).ToString();
                textBox1.Text = "";
            }));
        }
    }
}
