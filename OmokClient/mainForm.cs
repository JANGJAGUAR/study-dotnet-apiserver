using System;
using System.Collections.Concurrent;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms;
using MemoryPack;
using MessagePack;
using OmokClient.CS;

// using PKTReqLogin = OmokClient.CS.PKTReqLogin;

namespace OmokClient
{
    // REQ 처리
    [SupportedOSPlatform("windows10.0.177630")]
    public partial class mainForm : Form
    {
        ClientSimpleTcp Network = new ClientSimpleTcp();

        bool IsNetworkThreadRunning = false;
        bool IsBackGroundProcessRunning = false;

        System.Threading.Thread NetworkReadThread = null;
        System.Threading.Thread NetworkSendThread = null;

        PacketBufferManager PacketBuffer = new PacketBufferManager();
        ConcurrentQueue<byte[]> RecvPacketQueue = new ConcurrentQueue<byte[]>();
        ConcurrentQueue<byte[]> SendPacketQueue = new ConcurrentQueue<byte[]>();

        System.Windows.Forms.Timer dispatcherUITimer = new();


        public mainForm()
        {
            InitializeComponent();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            PacketBuffer.Init((8096 * 10), MemoryPackPacketHeadInfo.HeadSize, 2048);

            IsNetworkThreadRunning = true;
            NetworkReadThread = new System.Threading.Thread(this.NetworkReadProcess);
            NetworkReadThread.Start();
            NetworkSendThread = new System.Threading.Thread(this.NetworkSendProcess);
            NetworkSendThread.Start();

            IsBackGroundProcessRunning = true;
            dispatcherUITimer.Tick += new EventHandler(BackGroundProcess);
            dispatcherUITimer.Interval = 100;
            dispatcherUITimer.Start();

            btnDisconnect.Enabled = false;

            SetPacketHandler();
            DevLog.Write("프로그램 시작 !!!", LOG_LEVEL.INFO);
        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsNetworkThreadRunning = false;
            IsBackGroundProcessRunning = false;

            Network.Close();
        }

        private void btn_Connect_Click(object sender, EventArgs e)
        {
            string address = textBoxIP.Text;

            if (checkBoxLocalHostIP.Checked)
            {
                address = "127.0.0.1";
            }

            int port = Convert.ToInt32(textBoxPort.Text);

            if (Network.Connect(address, port))
            {
                labelStatus.Text = string.Format("{0}. 서버에 접속 중", DateTime.Now);
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;

                DevLog.Write($"서버에 접속 중", LOG_LEVEL.INFO);
            }
            else
            {
                labelStatus.Text = string.Format("{0}. 서버에 접속 실패", DateTime.Now);
            }

            PacketBuffer.Clear();
        }

        private void btn_Disconnect_Click(object sender, EventArgs e)
        {
            SetDisconnectd();
            Network.Close();
        }

        

        void NetworkReadProcess()
        {
            while (IsNetworkThreadRunning)
            {
                if (Network.IsConnected() == false)
                {
                    System.Threading.Thread.Sleep(1);
                    continue;
                }

                var recvData = Network.Receive();

                if (recvData != null)
                {
                    PacketBuffer.Write(recvData.Item2, 0, recvData.Item1);

                    while (true)
                    {
                        var data = PacketBuffer.Read();
                        if (data == null)
                        {
                            break;
                        }
                        
                        RecvPacketQueue.Enqueue(data);
                    }
                    //DevLog.Write($"받은 데이터: {recvData.Item2}", LOG_LEVEL.INFO);
                }
                else
                {
                    Network.Close();
                    SetDisconnectd();
                    DevLog.Write("서버와 접속 종료 !!!", LOG_LEVEL.INFO);
                }
            }
        }

        void NetworkSendProcess()
        {
            while (IsNetworkThreadRunning)
            {
                System.Threading.Thread.Sleep(1);

                if (Network.IsConnected() == false)
                {
                    continue;
                }

                
                if (SendPacketQueue.TryDequeue(out var packet))
                {
                    Network.Send(packet);
                }
            }
        }


        void BackGroundProcess(object sender, EventArgs e)
        {
            ProcessLog();

            try
            {
                byte[] packet = null;

                if(RecvPacketQueue.TryDequeue(out packet))
                {
                    PacketProcess(packet);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("BackGroundProcess. error:{0}", ex.Message));
            }
        }

        private void ProcessLog()
        {
            // 너무 이 작업만 할 수 없으므로 일정 작업 이상을 하면 일단 패스한다.
            int logWorkCount = 0;

            while (IsBackGroundProcessRunning)
            {
                System.Threading.Thread.Sleep(1);

                string msg;

                if (DevLog.GetLog(out msg))
                {
                    ++logWorkCount;

                    if (listBoxLog.Items.Count > 512)
                    {
                        listBoxLog.Items.Clear();
                    }

                    listBoxLog.Items.Add(msg);
                    listBoxLog.SelectedIndex = listBoxLog.Items.Count - 1;
                }
                else
                {
                    break;
                }

                if (logWorkCount > 8)
                {
                    break;
                }
            }
        }


        public void SetDisconnectd()
        {
            if (btnConnect.Enabled == false)
            {
                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
            }

            while (true)
            {
                if (SendPacketQueue.TryDequeue(out var temp) == false)
                {
                    break;
                }
            }

            listBoxRoomChatMsg.Items.Clear();
            listBoxRoomUserList.Items.Clear();

            labelStatus.Text = "서버 접속이 끊어짐";
        }

        
        void PostSendPacket(PACKETID packetID, byte[] packetData)
        {
            if (Network.IsConnected() == false)
            {
                DevLog.Write("서버 연결이 되어 있지 않습니다", LOG_LEVEL.ERROR);
                return;
            }

            var header = new MemoryPackPacketHeadInfo();
            header.TotalSize = (UInt16)packetData.Length;
            header.Id = (UInt16)packetID;
            header.Type = 0;
            header.WriteHeader(packetData);

            // if (packetData != null)
            // {
            //     header.TotalSize = (UInt16)packetData.Length;
            //     
            //     header.Write(packetData);
            // }
            // else
            // {
            //     packetData = header.Write();
            // }

            SendPacketQueue.Enqueue(packetData);
        }
        //TODO 위로 쭉 이해 못함
        
        // [기록 및 수정 함수]
        
        //TODO 유니크 없어도 괜찮으면 다 없애기
        void AddRoomUserList(string userID) // Int64 userUniqueId
        {
            var msg = $"{userID}";  // {userUniqueId}: 
            listBoxRoomUserList.Items.Add(msg);
        }

        void RemoveRoomUserList(string userID)
        {
            object removeItem = null;

            foreach( var user in listBoxRoomUserList.Items)
            {
                var items = user.ToString();
                if( items == userID)
                {
                    // 지울 사람 
                    removeItem = user;
                    // TODO return; 이 왜 있었을까
                }
            }

            if (removeItem != null)
            {
                listBoxRoomUserList.Items.Remove(removeItem);
            }
        }
        
        // 방 메시지 박스에 msg(id, 메시지) 형태로 기록
        void AddRoomChatMessage(string userID, string msgssage)
        {
            var msg = $"{userID}:  {msgssage}";
    
            if (listBoxRoomChatMsg.Items.Count > 512)
            {
                listBoxRoomChatMsg.Items.Clear();
            }
    
            listBoxRoomChatMsg.Items.Add(msg);
            listBoxRoomChatMsg.SelectedIndex = listBoxRoomChatMsg.Items.Count - 1;
        }
        
        // TODO 로비 생성 후
        // 로비 메시지 박스에 msg(id, 메시지) 형태로 기록
        void AddLobbyChatMessage(string userID, string message)
        {
            listBoxLobbyChat.Items.Add($"[{userID}]: {message}");
        }
        
        
        // [버튼 이벤트 함수]
        
        // 로그인 요청 버튼
        private void btn_Login_Click(object sender, EventArgs e)
        {
            var loginReq = new PKTReqLogin();
            loginReq.UserID = textBoxUserID.Text;
            loginReq.AuthToken = textBoxUserPW.Text;
            //TODO DateTime.Now로 바로 해도 괜찮은지
            loginReq.Datetime = DateTime.Now;

            var sendPacketData = MemoryPackSerializer.Serialize(loginReq);
                        
            PostSendPacket(PACKETID.REQ_LOGIN, sendPacketData);            
            DevLog.Write($"로그인 요청:  {textBoxUserID.Text}, {textBoxUserPW.Text}");
            DevLog.Write($"로그인 요청: {string.Join(", ", sendPacketData)}");
        }

        // 방 입장 요청 버튼
        private void btn_RoomEnter_Click(object sender, EventArgs e)
        {
            var requestPkt = new PKTReqRoomEnter();
            requestPkt.RoomNumber = textBoxRoomNumber.Text.ToInt32();

            var sendPacketData = MemoryPackSerializer.Serialize(requestPkt);

            PostSendPacket(PACKETID.REQ_ROOM_ENTER, sendPacketData);
            DevLog.Write($"방 입장 요청:  {textBoxRoomNumber.Text} 번 방");
        }

        // 방 떠나기 요청 버튼
        private void btn_RoomLeave_Click(object sender, EventArgs e)
        {
            PostSendPacket(PACKETID.REQ_ROOM_LEAVE,  new byte[MemoryPackPacketHeadInfo.HeadSize]);
            //TODO 위에랑 차이가 뭐지
            DevLog.Write($"방 입장 요청:  {textBoxRoomNumber.Text} 번 방");
        }

        // 방 채팅 요청 버튼
        private void btn_RoomChat_Click(object sender, EventArgs e)
        {
            if(textBoxRoomSendMsg.Text.IsEmpty())
            {
                MessageBox.Show("채팅 메시지를 입력하세요");
                return;
            }

            var requestPkt = new PKTReqRoomChat();
            requestPkt.ChatMessage = textBoxRoomSendMsg.Text;

            var sendPacketData = MemoryPackSerializer.Serialize(requestPkt);

            PostSendPacket(PACKETID.REQ_ROOM_CHAT, sendPacketData);
            DevLog.Write($"방 채팅 요청");
            //TODO 위처럼 쓰는게 맞나
        }

        //TODO (6주차) 매칭서버
        private void btn_Matching_Click(object sender, EventArgs e)
        {
            // PostSendPacket(PACKETID.REQ_MATCH_USER, null);
            DevLog.Write($"매칭 요청");
        }
        
        

        // 로비 입장 요청
        //TODO 로비 구현 아직 안함
        private void btn_LobbyEnter_Click(object sender, EventArgs e)
        {
            // var requestPkt = new PKTReqLobbyEnter();
            // requestPkt.LobbyNumber = textBox1.Text.ToInt32();
            // var sendPacketData = MemoryPackSerializer.Serialize(requestPkt);
            //
            // PostSendPacket(PACKETID.REQ_LOBBY_ENTER, sendPacketData);
            DevLog.Write($"로비 들어가기 요청. 로비 번호: {textBox1.Text}");
        }

        // 로비 나가기 요청
        //TODO 로비 구현 아직 안함
        private void btn_LobbyLeave_Click(object sender, EventArgs e)
        {
            // PostSendPacket(PACKETID.REQ_LOBBY_LEAVE,  new byte[MemoryPackPacketHeadInfo.HeadSize]);
            //TODO 위에랑 차이가 뭐지
            DevLog.Write($"로비 나가기 요청. 로비 번호: {textBox1.Text}");
        }

        private void listBoxRoomChatMsg_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TODO 무슨 기능이지
            //룸 채팅 드래그?
        }

        private void textBoxRelay_TextChanged(object sender, EventArgs e)
        {
            //TODO 무슨 기능이지
            //릴레이?가 뭐지
        }

        // 돌 두기 요청 버튼
        private void btn_PutStone_Click(object sender, EventArgs e)
        {
            var requestPkt = new PKTReqPutStone();
            requestPkt.xPos = xPosTextNumber.Text.ToInt32();
            requestPkt.yPos = xPosTextNumber.Text.ToInt32();

            var sendPacketData = MemoryPackSerializer.Serialize(requestPkt);
            PostSendPacket(PACKETID.REQ_PUT_STONE, sendPacketData);
            DevLog.Write($"돌 두기 버튼 요청 : x  [  {xPosTextNumber.Text}  ], y : [ {yPosTextNumber.Text} ] ");

        }

        // 게임 시작 버튼
        private void btn_GameStart_Click(object sender, EventArgs e)
        {
            var requestPkt = new PKTReqGameStart();
            requestPkt.UserID = textBoxUserID.Text;
            //TODO 적혀있는거 말고 인증된거 보내기
            var sendPacketData = MemoryPackSerializer.Serialize(requestPkt);
            PostSendPacket(PACKETID.REQ_GAME_START, sendPacketData);
            DevLog.Write($"게임시작 요청");
        }

        // 로비 채팅
        private void btn_LobbyChat_Click(object sender, EventArgs e)
        {
            // if(textBoxLobbyChat.Text.IsEmpty())
            // {
            //     MessageBox.Show("채팅 메시지를 입력하세요");
            //     return;
            // }
            //
            // var requestPkt = new PKTReqLobbyChat();
            // requestPkt.ChatMessage = textBoxLobbyChat.Text;
            //
            // var sendPacketData = MemoryPackSerializer.Serialize(requestPkt);
            //
            // PostSendPacket(PACKETID.REQ_LOBBY_CHAT, sendPacketData);
            
            DevLog.Write("로비 채팅 요청");
            //TODO 위처럼 쓰는게 맞나
        }
        
        //TODO 릴레이?
        // private void button3_Click(object sender, EventArgs e)
        // {
        //     //PostSendPacket(PACKET_ID.LOBBY_LIST_REQ, null);
        //     DevLog.Write($"방 릴레이 요청");
        // }
        
        private async Task ClientTimer()
        {
            while (true)
            {
                // 주기(1초)마다 실행
                var pktClientTimer = new PKTClientHeartBeat();
                pktClientTimer.dateTime = DateTime.Now;
            
                var sendPacket = MemoryPackSerializer.Serialize(pktClientTimer);
                PostSendPacket(PACKETID.REQ_HEART_BEAT, sendPacket);


                // 1초 대기
                await Task.Delay(1000);
            }
        }
        
    }
    
}
