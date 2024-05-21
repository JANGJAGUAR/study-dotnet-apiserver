using OmokShareProject;

namespace OmokGameClient;

public partial class OmokClient : Form
{
    private APIService _apiService = new APIService();
    // ClientSimpleTcp Network = new ClientSimpleTcp();
    //
    // bool IsNetworkThreadRunning = false;
    // bool IsBackGroundProcessRunning = false;
    //
    // System.Threading.Thread NetworkReadThread = null;
    // System.Threading.Thread NetworkSendThread = null;
    //
    // PacketBufferManager PacketBuffer = new PacketBufferManager();
    // ConcurrentQueue<byte[]> RecvPacketQueue = new ConcurrentQueue<byte[]>();
    // ConcurrentQueue<byte[]> SendPacketQueue = new ConcurrentQueue<byte[]>();
    //
    // System.Windows.Forms.Timer dispatcherUITimer = new();
    
    public OmokClient()
    {
        InitializeComponent();
    }

    private void mainForm_Load(object sender, EventArgs e)
    {
        
        
        
        // PacketBuffer.Init((8096 * 10), MemoryPackPacketHeadInfo.HeadSize, 2048);
        //
        // IsNetworkThreadRunning = true;
        // NetworkReadThread = new System.Threading.Thread(this.NetworkReadProcess);
        // NetworkReadThread.Start();
        // NetworkSendThread = new System.Threading.Thread(this.NetworkSendProcess);
        // NetworkSendThread.Start();
        //
        // IsBackGroundProcessRunning = true;
        // dispatcherUITimer.Tick += new EventHandler(BackGroundProcess);
        // dispatcherUITimer.Interval = 100;
        // dispatcherUITimer.Start();
        //
        // SetPacketHandler();
        // DevLog.Write("프로그램 시작 !!!", LOG_LEVEL.INFO);
    }
    private void panel1_Paint(object sender, PaintEventArgs e)
    {

    }
    private void Signup_Btn_Click(object sender, EventArgs e)
    {
        _apiService.SignupApiServer(ID_Label.Text, PW_Label.Text);
    }
    private void Login_Btn_Click(object sender, EventArgs e)
    {
        _apiService.LoginApiServer(ID_Label.Text, PW_Label.Text);
    }
    private void Back_Btn_Click(object sender, EventArgs e)
    {

    }
    
    private void Surrender_Btn_Click(object sender, EventArgs e)
    {

    }
    private void MailBox_Btn_Click(object sender, EventArgs e)
    {

    }
    private void Matching_Btn_Click(object sender, EventArgs e)
    {
        _apiService.MatchingServerFind(ID_Label.Text);
    }
    private void Ready_Btn_Click(object sender, EventArgs e)
    {
        _apiService.MatchingServerCheck(ID_Label.Text);
    }
    private void Chat_Btn_Click(object sender, EventArgs e)
    {

    }
}