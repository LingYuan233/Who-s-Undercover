using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using 谁是卧底.util;

namespace 谁是卧底
{
    public partial class Form1 : Form
    {
        private HttpClient hc = new HttpClient();
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string Name = textBox1.Text;

            if (string.IsNullOrWhiteSpace(Name))
            {
                textBox1.Text = "";
                return;
            }

            string url = "http://服务器地址/canEnter";
            HttpResponseMessage response = await hc.GetAsync(url);
            HttpContent content = response.Content; 
            string mycontent = await content.ReadAsStringAsync();
            if (mycontent == "Failed")
            {
                label5.Text = "加入失败";
                return;
            }

            Values.PlayerName = Name;
            Main main = new Main();
            this.Hide();
            main.ShowDialog();
            this.Dispose();
        }
    }
}
