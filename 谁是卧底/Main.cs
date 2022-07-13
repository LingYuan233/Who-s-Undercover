using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using WebSocket4Net;
using 谁是卧底.util;

namespace 谁是卧底
{
    public partial class Main : Form
    {
        private WebSocket ws = new WebSocket("ws://服务器地址");
        private bool CanPreparation = true;
        private bool CanVote = false;
        private string Self;

        public Main()
        {
            InitializeComponent();
        }
        private void Main_Load(object sender, EventArgs e)
        {
            textBox2.Text = Values.PlayerName;
            label4.Text = "玩家列表：\n";
            ws.Open();
            sendQuestion();
            SendJoinMes();
            ws.MessageReceived += new EventHandler<MessageReceivedEventArgs>(ws_message);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //send message
            string Message = textBox1.Text;
            if (string.IsNullOrWhiteSpace(Message))
            {
                return;
            }
            MessageBase mb = new MessageBase();
            mb.Name = Values.PlayerName;
            mb.Type = "message";
            mb.Message = Message;
            string json = JsonConvert.SerializeObject(mb);
            ws.Send(json);
            textBox1.Text = "";
            
        }

        private void ws_message(object sender, MessageReceivedEventArgs e)
        {
            // e.Message
            MainBase mb = JsonConvert.DeserializeObject<MainBase>(e.Message);
            if (mb.Type == "message")
            {
                richTextBox1.Text += $"【{mb.Name}】说： {mb.Message}\n";
            }
            if (mb.Type == "getAllConn")
            {
                //comboBox1.Items.Add(mb.Name);
                refreshComboList(mb);
            }
            if (mb.Type == "serverMessage")
            {
                showServerMessage(mb);
            }
            if (mb.Type == "passed")
            {
                showPassedMessage(mb);
            }
        }
        private void SendJoinMes()
        {
            MessageBase mb = new MessageBase();
            mb.Name = Values.PlayerName;
            mb.Type = "joined";
            mb.Message = "";
            string json = JsonConvert.SerializeObject(mb);
            ws.Send(json);
        }
        private void sendQuestion()
        {
            //
            MainBase mb = new MainBase();
            mb.Name = Values.PlayerName;
            mb.Type = "question";
            string json = JsonConvert.SerializeObject(mb);
            ws.Send(json);
        }
        private void refreshComboList(MainBase mb)
        {
            comboBox1.Items.Clear();
            label4.Text = "玩家列表：\n";
            for (int i=0; i<mb.Conn.Length; i++)
            {
                if (mb.Conn[i] != Values.PlayerName)
                {
                    comboBox1.Items.Add(mb.Conn[i]);
                }
                label4.Text += $"{mb.Conn[i]}\n";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //投票
            string SelectedName = comboBox1.Text;
            if (string.IsNullOrWhiteSpace(SelectedName))
            {
                return;
            }
            if (!CanVote)
            {
                return;
            }
            //向服务器发送投票信息
            MainBase mb = new MainBase();
            mb.Type = "Vote";
            mb.VoteName = SelectedName;
            string json = JsonConvert.SerializeObject(mb);
            ws.Send(json);
            CanVote = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!CanPreparation)
            {
                return;
            }
            //点击准备按钮
            checkBox1.Checked = true;
            //向服务器发送准备事件
            MainBase mb = new MainBase();
            mb.Type = "preparation";
            mb.Name = Values.PlayerName;
            mb.Message = Values.PlayerName + "已准备";
            string Json = JsonConvert.SerializeObject(mb);
            ws.Send(Json);
            CanPreparation = false;
        }

        private void showServerMessage(MainBase mb)
        {
            //显示服务器发过来的消息
            if (mb.Info == "preparation")//有玩家准备消息
            {
                richTextBox1.Text += $"【服务器消息】 玩家 {mb.Name} 已准备\n";
            }
            if (mb.Info == "startGame")//开始游戏
            {
                richTextBox1.Text += $"【服务器消息】 游戏开始，正在下放游戏关键词\n";
            }
            if (mb.Info == "sendKw")//得到关键字
            {
                KeywordsSendHandler(mb);
                richTextBox1.Text += "【服务器消息】 关键词已下发完毕，开始发言投票吧！\n";
            }
        }
        private void KeywordsSendHandler(MainBase mb)
        {
            CanVote = true;
            //显示该关键词对象的信息
            textBox3.Text = mb.KwType;
            if (mb.UndercoverName == Values.PlayerName)
            {
                //该玩家是卧底
                textBox4.Text = mb.Undercoverkw;
                Self = "Undercover";
            }
            else
            {
                //该玩家不是卧底
                textBox4.Text = mb.Ordinarykw;
                Self = "Ordinary";
            }
        }
        private void showPassedMessage(MainBase mb)
        {
            //判断被淘汰的是不是自己
            string showMessage = "\n【服务器消息】 游戏已结束，正在清算信息。\n";
            string passedName = mb.PassedName;
            if (passedName == Values.PlayerName)
            {
                //是自己
                showMessage += "你被淘汰了！\n";
            }
            else
            {
                //不是自己
                showMessage += $"玩家{mb.PassedName}被淘汰了！\n";
            }
            showMessage += $"卧底玩家是{mb.UndercoverName}";
            richTextBox1.Text += showMessage;
        }
    }
}
