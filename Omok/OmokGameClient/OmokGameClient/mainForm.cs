using System.Collections.Concurrent;
using MemoryPack;
using Microsoft.Extensions.Options;
using OmokShareProject;

namespace OmokGameClient;

public partial class OmokClient : Form
{
    private APIService _apiService = new APIService();
    ClientSimpleTcp Network = new ClientSimpleTcp();
    
    bool IsNetworkThreadRunning = false;
    bool IsBackGroundProcessRunning = false;
    
    System.Threading.Thread NetworkReadThread = null;
    System.Threading.Thread NetworkSendThread = null;
    
    PacketBufferManager PacketBuffer = new PacketBufferManager();
    ConcurrentQueue<byte[]> RecvPacketQueue = new ConcurrentQueue<byte[]>();
    ConcurrentQueue<byte[]> SendPacketQueue = new ConcurrentQueue<byte[]>();
    
    System.Windows.Forms.Timer dispatcherUITimer = new();
    
    public OmokClient()
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
        
        SetPacketHandler();
        DevLog.Write("프로그램 시작 !!!", LOG_LEVEL.INFO);
    }
    private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        IsNetworkThreadRunning = false;
        IsBackGroundProcessRunning = false;
        
        Network.Close();
        SetDisconnectd();
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

                if (Log_List.Items.Count > 512)
                {
                    Log_List.Items.Clear();
                }

                Log_List.Items.Add(msg);
                Log_List.SelectedIndex = Log_List.Items.Count - 1;
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

        while (true)
        {
            if (SendPacketQueue.TryDequeue(out var temp) == false)
            {
                break;
            }
        }

        Chat_List.Items.Clear();
        User_List.Items.Clear();
        Log_List.Items.Add("서버 접속이 끊어짐");
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
        
        SendPacketQueue.Enqueue(packetData);
    }
    
    // [하트 비트]
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
    
    
    // [기록 및 수정 함수]
    
    void AddRoomUserList(string userID) // Int64 userUniqueId
    {
        var msg = $"{userID}";  // {userUniqueId}: 
        User_List.Items.Add(msg);
    }

    void RemoveRoomUserList(string userID)
    {
        object removeItem = null;

        foreach( var user in User_List.Items)
        {
            var items = user.ToString();
            if( items == userID)
            {
                // 지울 사람 
                removeItem = user;
                break;
            }
        }

        if (removeItem != null)
        {
            User_List.Items.Remove(removeItem);
        }
    }
    
    // 방 메시지 박스에 msg(id, 메시지) 형태로 기록
    void AddRoomChatMessage(string userID, string msgssage)
    {
        var msg = $"{userID}:  {msgssage}";

        if (Chat_List.Items.Count > 512)
        {
            Chat_List.Items.Clear();
        }

        Chat_List.Items.Add(msg);
        Chat_List.SelectedIndex = Chat_List.Items.Count - 1;
    }
    
    // [버튼 이벤트 함수]

    // 회원가입 버튼
    private void Signup_Btn_Click(object sender, EventArgs e)
    {
        _apiService.SignupApiServer(ID_Label.Text, PW_Label.Text);
        DevLog.Write($"회원가입 요청:  {ID_Label.Text}, {PW_Label.Text}");
    }
    
    // 로그인 버튼
    private void Login_Btn_Click(object sender, EventArgs e)
    {
        _apiService.LoginApiServer(ID_Label.Text, PW_Label.Text);
         
        DevLog.Write($"로그인 요청:  {ID_Label.Text}, {PW_Label.Text}");
        
        // 타이머 작동
        //TODO: 응답 결과 이용해서, 로그인 성공일 경우 타이머 작동
        if (true)
        {
            var task = Task.Run(ClientTimer);
        }
    }
    
    // 방 채팅 요청 버튼
    private void Chat_Btn_Click(object sender, EventArgs e)
    {
        if (true)//TODO: 연결 안 되어있을 떄
        {
            MessageBox.Show("방에 입장하지 않았습니다");
            return;
        }
        
        if(Chat_Label.Text=="")
        {
            MessageBox.Show("채팅 메시지를 입력하세요");
            return;
        }

        var requestPkt = new PKTReqRoomChat();
        requestPkt.ChatMessage = Chat_Label.Text;

        var sendPacketData = MemoryPackSerializer.Serialize(requestPkt);

        PostSendPacket(PACKETID.REQ_ROOM_CHAT, sendPacketData);
        DevLog.Write($"방 채팅 요청");
    }
    
    // 항복 버튼
    private void Surrender_Btn_Click(object sender, EventArgs e)
    {
        //TODO : 항복 처리
        PostSendPacket(PACKETID.REQ_ROOM_LEAVE,  new byte[MemoryPackPacketHeadInfo.HeadSize]);
        DevLog.Write($"방 나가기 요청:  {RoomNo_Label.Text} 번 방");
    }
    
    // 매칭 요청 버튼
    private void Matching_Btn_Click(object sender, EventArgs e)
    {
        _apiService.MatchingServerFind(ID_Label.Text);
    }
    
    // 준비 완료 버튼
    private void Ready_Btn_Click(object sender, EventArgs e)
    {
        _apiService.MatchingServerCheck(ID_Label.Text);
    }
    
    
    //TODO: 아이템 관련
    private void Back_Btn_Click(object sender, EventArgs e)
    {

    }
    private void MailBox_Btn_Click(object sender, EventArgs e)
    {

    }
    
}