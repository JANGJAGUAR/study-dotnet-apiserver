namespace OmokGameClient;

partial class OmokClient
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        Button Attendance_Btn;
        Button Inventory_Btn;
        ID_Text = new Label();
        Login_Box = new GroupBox();
        Token_Label = new TextBox();
        Token_Text = new Label();
        Signup_Btn = new Button();
        Login_Btn = new Button();
        ID_Label = new TextBox();
        PW_Text = new Label();
        PW_Label = new TextBox();
        OmokBoard = new Panel();
        Server_Box = new GroupBox();
        Port_Label = new TextBox();
        Localhost_Check = new CheckBox();
        Disconnect_Btn = new Button();
        Connect_Btn = new Button();
        IP_Label = new TextBox();
        Port_Text = new Label();
        IP_Text = new Label();
        Room_Box = new GroupBox();
        Chat_List = new ListBox();
        User_List = new ListBox();
        RoomNo_Label = new TextBox();
        RoomNo_Text = new Label();
        Chat_Btn = new Button();
        Chat_Label = new TextBox();
        Log_box = new GroupBox();
        Log_List = new ListBox();
        Game_Box = new GroupBox();
        Surrender_Btn = new Button();
        Back_Btn = new Button();
        Ready_Btn = new Button();
        Reward_Box = new GroupBox();
        MailBox_Btn = new Button();
        Player_Panel = new Panel();
        PlayerNickname_Label = new TextBox();
        PlayerWin_Label = new TextBox();
        PlayerWin_Text = new Label();
        PlayerLose_Label = new TextBox();
        PlayerLose_Text = new Label();
        PlayerTime_Label = new TextBox();
        EnemyTime_Label = new TextBox();
        EnemyLose_Text = new Label();
        EnemyWin_Text = new Label();
        EnemyLose_Label = new TextBox();
        EnemyWin_Label = new TextBox();
        EnemyNickname_Label = new TextBox();
        Enemy_Panel = new Panel();
        Matching_Btn = new Button();
        PlayerColor_Label = new TextBox();
        EnemyColor_Label = new TextBox();
        Attendance_Btn = new Button();
        Inventory_Btn = new Button();
        Login_Box.SuspendLayout();
        Server_Box.SuspendLayout();
        Room_Box.SuspendLayout();
        Log_box.SuspendLayout();
        Game_Box.SuspendLayout();
        Reward_Box.SuspendLayout();
        SuspendLayout();
        // 
        // Attendance_Btn
        // 
        Attendance_Btn.Location = new Point(140, 26);
        Attendance_Btn.Name = "Attendance_Btn";
        Attendance_Btn.Size = new Size(100, 38);
        Attendance_Btn.TabIndex = 7;
        Attendance_Btn.Text = "출석부";
        Attendance_Btn.UseVisualStyleBackColor = true;
        // 
        // Inventory_Btn
        // 
        Inventory_Btn.Location = new Point(31, 26);
        Inventory_Btn.Name = "Inventory_Btn";
        Inventory_Btn.Size = new Size(100, 38);
        Inventory_Btn.TabIndex = 9;
        Inventory_Btn.Text = "인벤토리";
        Inventory_Btn.UseVisualStyleBackColor = true;
        // 
        // ID_Text
        // 
        ID_Text.AutoSize = true;
        ID_Text.Location = new Point(44, 35);
        ID_Text.Name = "ID_Text";
        ID_Text.Size = new Size(24, 20);
        ID_Text.TabIndex = 0;
        ID_Text.Text = "ID";
        // 
        // Login_Box
        // 
        Login_Box.Controls.Add(Token_Label);
        Login_Box.Controls.Add(Token_Text);
        Login_Box.Controls.Add(Signup_Btn);
        Login_Box.Controls.Add(Login_Btn);
        Login_Box.Controls.Add(ID_Label);
        Login_Box.Controls.Add(PW_Text);
        Login_Box.Controls.Add(PW_Label);
        Login_Box.Controls.Add(ID_Text);
        Login_Box.Location = new Point(12, 184);
        Login_Box.Name = "Login_Box";
        Login_Box.Size = new Size(382, 176);
        Login_Box.TabIndex = 2;
        Login_Box.TabStop = false;
        Login_Box.Text = "Login";
        // 
        // Token_Label
        // 
        Token_Label.Location = new Point(111, 125);
        Token_Label.Name = "Token_Label";
        Token_Label.ReadOnly = true;
        Token_Label.Size = new Size(125, 27);
        Token_Label.TabIndex = 8;
        // 
        // Token_Text
        // 
        Token_Text.AutoSize = true;
        Token_Text.Location = new Point(31, 128);
        Token_Text.Name = "Token_Text";
        Token_Text.Size = new Size(51, 20);
        Token_Text.TabIndex = 7;
        Token_Text.Text = "Token";
        // 
        // Signup_Btn
        // 
        Signup_Btn.Location = new Point(267, 76);
        Signup_Btn.Name = "Signup_Btn";
        Signup_Btn.Size = new Size(81, 27);
        Signup_Btn.TabIndex = 6;
        Signup_Btn.Text = "회원가입";
        Signup_Btn.UseVisualStyleBackColor = true;
        Signup_Btn.Click += Signup_Btn_Click;
        // 
        // Login_Btn
        // 
        Login_Btn.Location = new Point(267, 32);
        Login_Btn.Name = "Login_Btn";
        Login_Btn.Size = new Size(81, 27);
        Login_Btn.TabIndex = 5;
        Login_Btn.Text = "로그인";
        Login_Btn.UseVisualStyleBackColor = true;
        Login_Btn.Click += Login_Btn_Click;
        // 
        // ID_Label
        // 
        ID_Label.Location = new Point(111, 32);
        ID_Label.Name = "ID_Label";
        ID_Label.Size = new Size(125, 27);
        ID_Label.TabIndex = 4;
        // 
        // PW_Text
        // 
        PW_Text.AutoSize = true;
        PW_Text.Location = new Point(20, 79);
        PW_Text.Name = "PW_Text";
        PW_Text.Size = new Size(72, 20);
        PW_Text.TabIndex = 3;
        PW_Text.Text = "Password";
        // 
        // PW_Label
        // 
        PW_Label.Location = new Point(111, 76);
        PW_Label.Name = "PW_Label";
        PW_Label.Size = new Size(125, 27);
        PW_Label.TabIndex = 2;
        // 
        // OmokBoard
        // 
        OmokBoard.BackColor = Color.Peru;
        OmokBoard.Location = new Point(400, 12);
        OmokBoard.Name = "OmokBoard";
        OmokBoard.Size = new Size(800, 800);
        OmokBoard.TabIndex = 3;
        OmokBoard.Paint += panel1_Paint;
        // 
        // Server_Box
        // 
        Server_Box.Controls.Add(Port_Label);
        Server_Box.Controls.Add(Localhost_Check);
        Server_Box.Controls.Add(Disconnect_Btn);
        Server_Box.Controls.Add(Connect_Btn);
        Server_Box.Controls.Add(IP_Label);
        Server_Box.Controls.Add(Port_Text);
        Server_Box.Controls.Add(IP_Text);
        Server_Box.Location = new Point(13, 13);
        Server_Box.Name = "Server_Box";
        Server_Box.Size = new Size(381, 166);
        Server_Box.TabIndex = 9;
        Server_Box.TabStop = false;
        Server_Box.Text = "Server";
        // 
        // Port_Label
        // 
        Port_Label.Location = new Point(110, 115);
        Port_Label.Name = "Port_Label";
        Port_Label.Size = new Size(125, 27);
        Port_Label.TabIndex = 9;
        // 
        // Localhost_Check
        // 
        Localhost_Check.AutoSize = true;
        Localhost_Check.Location = new Point(111, 65);
        Localhost_Check.Name = "Localhost_Check";
        Localhost_Check.Size = new Size(92, 24);
        Localhost_Check.TabIndex = 10;
        Localhost_Check.Text = "localhost";
        Localhost_Check.UseVisualStyleBackColor = true;
        // 
        // Disconnect_Btn
        // 
        Disconnect_Btn.Location = new Point(267, 112);
        Disconnect_Btn.Name = "Disconnect_Btn";
        Disconnect_Btn.Size = new Size(81, 27);
        Disconnect_Btn.TabIndex = 6;
        Disconnect_Btn.Text = "연결끊기";
        Disconnect_Btn.UseVisualStyleBackColor = true;
        Disconnect_Btn.Click += Disconnect_Btn_Click;
        // 
        // Connect_Btn
        // 
        Connect_Btn.Location = new Point(267, 32);
        Connect_Btn.Name = "Connect_Btn";
        Connect_Btn.Size = new Size(81, 27);
        Connect_Btn.TabIndex = 5;
        Connect_Btn.Text = "연결하기";
        Connect_Btn.UseVisualStyleBackColor = true;
        Connect_Btn.Click += Connect_Btn_Click;
        // 
        // IP_Label
        // 
        IP_Label.Location = new Point(111, 32);
        IP_Label.Name = "IP_Label";
        IP_Label.Size = new Size(125, 27);
        IP_Label.TabIndex = 4;
        // 
        // Port_Text
        // 
        Port_Text.AutoSize = true;
        Port_Text.Location = new Point(35, 115);
        Port_Text.Name = "Port_Text";
        Port_Text.Size = new Size(37, 20);
        Port_Text.TabIndex = 3;
        Port_Text.Text = "Port";
        // 
        // IP_Text
        // 
        IP_Text.AutoSize = true;
        IP_Text.Location = new Point(44, 35);
        IP_Text.Name = "IP_Text";
        IP_Text.Size = new Size(22, 20);
        IP_Text.TabIndex = 0;
        IP_Text.Text = "IP";
        // 
        // Room_Box
        // 
        Room_Box.Controls.Add(Chat_List);
        Room_Box.Controls.Add(User_List);
        Room_Box.Controls.Add(RoomNo_Label);
        Room_Box.Controls.Add(RoomNo_Text);
        Room_Box.Controls.Add(Chat_Btn);
        Room_Box.Controls.Add(Chat_Label);
        Room_Box.Location = new Point(1206, 13);
        Room_Box.Name = "Room_Box";
        Room_Box.Size = new Size(364, 309);
        Room_Box.TabIndex = 9;
        Room_Box.TabStop = false;
        Room_Box.Text = "Room";
        // 
        // Chat_List
        // 
        Chat_List.FormattingEnabled = true;
        Chat_List.Location = new Point(20, 99);
        Chat_List.Name = "Chat_List";
        Chat_List.Size = new Size(338, 164);
        Chat_List.TabIndex = 10;
        // 
        // User_List
        // 
        User_List.FormattingEnabled = true;
        User_List.Location = new Point(138, 29);
        User_List.Name = "User_List";
        User_List.Size = new Size(220, 64);
        User_List.TabIndex = 9;
        // 
        // RoomNo_Label
        // 
        RoomNo_Label.Location = new Point(20, 33);
        RoomNo_Label.Name = "RoomNo_Label";
        RoomNo_Label.ReadOnly = true;
        RoomNo_Label.Size = new Size(62, 27);
        RoomNo_Label.TabIndex = 8;
        // 
        // RoomNo_Text
        // 
        RoomNo_Text.AutoSize = true;
        RoomNo_Text.Location = new Point(88, 36);
        RoomNo_Text.Name = "RoomNo_Text";
        RoomNo_Text.Size = new Size(44, 20);
        RoomNo_Text.TabIndex = 7;
        RoomNo_Text.Text = "번 방";
        // 
        // Chat_Btn
        // 
        Chat_Btn.Location = new Point(295, 270);
        Chat_Btn.Name = "Chat_Btn";
        Chat_Btn.Size = new Size(63, 27);
        Chat_Btn.TabIndex = 5;
        Chat_Btn.Text = "채팅";
        Chat_Btn.UseVisualStyleBackColor = true;
        Chat_Btn.Click += Chat_Btn_Click;
        // 
        // Chat_Label
        // 
        Chat_Label.Location = new Point(20, 270);
        Chat_Label.Name = "Chat_Label";
        Chat_Label.Size = new Size(269, 27);
        Chat_Label.TabIndex = 4;
        // 
        // Log_box
        // 
        Log_box.Controls.Add(Log_List);
        Log_box.Location = new Point(1206, 328);
        Log_box.Name = "Log_box";
        Log_box.Size = new Size(364, 241);
        Log_box.TabIndex = 11;
        Log_box.TabStop = false;
        Log_box.Text = "Log";
        // 
        // Log_List
        // 
        Log_List.FormattingEnabled = true;
        Log_List.Location = new Point(6, 26);
        Log_List.Name = "Log_List";
        Log_List.Size = new Size(352, 204);
        Log_List.TabIndex = 11;
        // 
        // Game_Box
        // 
        Game_Box.Controls.Add(Matching_Btn);
        Game_Box.Controls.Add(Surrender_Btn);
        Game_Box.Controls.Add(Back_Btn);
        Game_Box.Controls.Add(Ready_Btn);
        Game_Box.Location = new Point(12, 451);
        Game_Box.Name = "Game_Box";
        Game_Box.Size = new Size(382, 137);
        Game_Box.TabIndex = 12;
        Game_Box.TabStop = false;
        Game_Box.Text = "Game";
        // 
        // Surrender_Btn
        // 
        Surrender_Btn.Location = new Point(193, 80);
        Surrender_Btn.Name = "Surrender_Btn";
        Surrender_Btn.Size = new Size(156, 38);
        Surrender_Btn.TabIndex = 8;
        Surrender_Btn.Text = "기권패";
        Surrender_Btn.UseVisualStyleBackColor = true;
        Surrender_Btn.Click += Surrender_Btn_Click;
        // 
        // Back_Btn
        // 
        Back_Btn.Location = new Point(31, 80);
        Back_Btn.Name = "Back_Btn";
        Back_Btn.Size = new Size(156, 38);
        Back_Btn.TabIndex = 7;
        Back_Btn.Text = "한 수 무르기";
        Back_Btn.UseVisualStyleBackColor = true;
        Back_Btn.Click += Back_Btn_Click;
        // 
        // Ready_Btn
        // 
        Ready_Btn.BackColor = SystemColors.GradientActiveCaption;
        Ready_Btn.FlatAppearance.BorderColor = Color.FromArgb(255, 192, 192);
        Ready_Btn.Location = new Point(193, 36);
        Ready_Btn.Name = "Ready_Btn";
        Ready_Btn.Size = new Size(156, 38);
        Ready_Btn.TabIndex = 6;
        Ready_Btn.Text = "게임 시작";
        Ready_Btn.UseVisualStyleBackColor = false;
        Ready_Btn.Click += Ready_Btn_Click;
        // 
        // Reward_Box
        // 
        Reward_Box.Controls.Add(Inventory_Btn);
        Reward_Box.Controls.Add(MailBox_Btn);
        Reward_Box.Controls.Add(Attendance_Btn);
        Reward_Box.Location = new Point(12, 366);
        Reward_Box.Name = "Reward_Box";
        Reward_Box.Size = new Size(382, 79);
        Reward_Box.TabIndex = 13;
        Reward_Box.TabStop = false;
        Reward_Box.Text = "Reward";
        // 
        // MailBox_Btn
        // 
        MailBox_Btn.Location = new Point(249, 26);
        MailBox_Btn.Name = "MailBox_Btn";
        MailBox_Btn.Size = new Size(100, 38);
        MailBox_Btn.TabIndex = 8;
        MailBox_Btn.Text = "우편함";
        MailBox_Btn.UseVisualStyleBackColor = true;
        MailBox_Btn.Click += MailBox_Btn_Click;
        // 
        // Player_Panel
        // 
        Player_Panel.BackColor = SystemColors.ControlDark;
        Player_Panel.Location = new Point(32, 624);
        Player_Panel.Name = "Player_Panel";
        Player_Panel.Size = new Size(128, 128);
        Player_Panel.TabIndex = 14;
        // 
        // PlayerNickname_Label
        // 
        PlayerNickname_Label.Location = new Point(32, 758);
        PlayerNickname_Label.Name = "PlayerNickname_Label";
        PlayerNickname_Label.ReadOnly = true;
        PlayerNickname_Label.Size = new Size(128, 27);
        PlayerNickname_Label.TabIndex = 15;
        // 
        // PlayerWin_Label
        // 
        PlayerWin_Label.Location = new Point(232, 631);
        PlayerWin_Label.Name = "PlayerWin_Label";
        PlayerWin_Label.ReadOnly = true;
        PlayerWin_Label.Size = new Size(128, 27);
        PlayerWin_Label.TabIndex = 15;
        // 
        // PlayerWin_Text
        // 
        PlayerWin_Text.AutoSize = true;
        PlayerWin_Text.Location = new Point(177, 634);
        PlayerWin_Text.Name = "PlayerWin_Text";
        PlayerWin_Text.Size = new Size(39, 20);
        PlayerWin_Text.TabIndex = 17;
        PlayerWin_Text.Text = "승리";
        // 
        // PlayerLose_Label
        // 
        PlayerLose_Label.Location = new Point(232, 673);
        PlayerLose_Label.Name = "PlayerLose_Label";
        PlayerLose_Label.ReadOnly = true;
        PlayerLose_Label.Size = new Size(128, 27);
        PlayerLose_Label.TabIndex = 16;
        // 
        // PlayerLose_Text
        // 
        PlayerLose_Text.AutoSize = true;
        PlayerLose_Text.Location = new Point(177, 676);
        PlayerLose_Text.Name = "PlayerLose_Text";
        PlayerLose_Text.Size = new Size(39, 20);
        PlayerLose_Text.TabIndex = 18;
        PlayerLose_Text.Text = "패배";
        // 
        // PlayerTime_Label
        // 
        PlayerTime_Label.Font = new Font("맑은 고딕", 24F, FontStyle.Bold, GraphicsUnit.Point, 129);
        PlayerTime_Label.Location = new Point(280, 721);
        PlayerTime_Label.Name = "PlayerTime_Label";
        PlayerTime_Label.ReadOnly = true;
        PlayerTime_Label.Size = new Size(80, 61);
        PlayerTime_Label.TabIndex = 25;
        // 
        // EnemyTime_Label
        // 
        EnemyTime_Label.Font = new Font("맑은 고딕", 24F, FontStyle.Bold, GraphicsUnit.Point, 129);
        EnemyTime_Label.Location = new Point(1474, 721);
        EnemyTime_Label.Name = "EnemyTime_Label";
        EnemyTime_Label.ReadOnly = true;
        EnemyTime_Label.Size = new Size(80, 61);
        EnemyTime_Label.TabIndex = 32;
        // 
        // EnemyLose_Text
        // 
        EnemyLose_Text.AutoSize = true;
        EnemyLose_Text.Location = new Point(1371, 676);
        EnemyLose_Text.Name = "EnemyLose_Text";
        EnemyLose_Text.Size = new Size(39, 20);
        EnemyLose_Text.TabIndex = 31;
        EnemyLose_Text.Text = "패배";
        // 
        // EnemyWin_Text
        // 
        EnemyWin_Text.AutoSize = true;
        EnemyWin_Text.Location = new Point(1371, 634);
        EnemyWin_Text.Name = "EnemyWin_Text";
        EnemyWin_Text.Size = new Size(39, 20);
        EnemyWin_Text.TabIndex = 30;
        EnemyWin_Text.Text = "승리";
        // 
        // EnemyLose_Label
        // 
        EnemyLose_Label.Location = new Point(1426, 673);
        EnemyLose_Label.Name = "EnemyLose_Label";
        EnemyLose_Label.ReadOnly = true;
        EnemyLose_Label.Size = new Size(128, 27);
        EnemyLose_Label.TabIndex = 29;
        // 
        // EnemyWin_Label
        // 
        EnemyWin_Label.Location = new Point(1426, 631);
        EnemyWin_Label.Name = "EnemyWin_Label";
        EnemyWin_Label.ReadOnly = true;
        EnemyWin_Label.Size = new Size(128, 27);
        EnemyWin_Label.TabIndex = 27;
        // 
        // EnemyNickname_Label
        // 
        EnemyNickname_Label.Location = new Point(1226, 758);
        EnemyNickname_Label.Name = "EnemyNickname_Label";
        EnemyNickname_Label.ReadOnly = true;
        EnemyNickname_Label.Size = new Size(128, 27);
        EnemyNickname_Label.TabIndex = 28;
        // 
        // Enemy_Panel
        // 
        Enemy_Panel.BackColor = SystemColors.ControlDark;
        Enemy_Panel.Location = new Point(1226, 624);
        Enemy_Panel.Name = "Enemy_Panel";
        Enemy_Panel.Size = new Size(128, 128);
        Enemy_Panel.TabIndex = 26;
        // 
        // Matching_Btn
        // 
        Matching_Btn.BackColor = SystemColors.GradientActiveCaption;
        Matching_Btn.FlatAppearance.BorderColor = Color.FromArgb(255, 192, 192);
        Matching_Btn.Location = new Point(31, 36);
        Matching_Btn.Name = "Matching_Btn";
        Matching_Btn.Size = new Size(156, 38);
        Matching_Btn.TabIndex = 9;
        Matching_Btn.Text = "매칭 상대 찾기";
        Matching_Btn.UseVisualStyleBackColor = false;
        Matching_Btn.Click += Matching_Btn_Click;
        // 
        // PlayerColor_Label
        // 
        PlayerColor_Label.Font = new Font("맑은 고딕", 24F, FontStyle.Bold, GraphicsUnit.Point, 129);
        PlayerColor_Label.Location = new Point(177, 721);
        PlayerColor_Label.Name = "PlayerColor_Label";
        PlayerColor_Label.ReadOnly = true;
        PlayerColor_Label.Size = new Size(52, 61);
        PlayerColor_Label.TabIndex = 33;
        // 
        // EnemyColor_Label
        // 
        EnemyColor_Label.Font = new Font("맑은 고딕", 24F, FontStyle.Bold, GraphicsUnit.Point, 129);
        EnemyColor_Label.Location = new Point(1371, 721);
        EnemyColor_Label.Name = "EnemyColor_Label";
        EnemyColor_Label.ReadOnly = true;
        EnemyColor_Label.Size = new Size(52, 61);
        EnemyColor_Label.TabIndex = 34;
        // 
        // OmokClient
        // 
        AutoScaleDimensions = new SizeF(9F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1582, 853);
        Controls.Add(EnemyColor_Label);
        Controls.Add(PlayerColor_Label);
        Controls.Add(EnemyTime_Label);
        Controls.Add(EnemyLose_Text);
        Controls.Add(EnemyWin_Text);
        Controls.Add(EnemyLose_Label);
        Controls.Add(EnemyWin_Label);
        Controls.Add(EnemyNickname_Label);
        Controls.Add(Enemy_Panel);
        Controls.Add(PlayerTime_Label);
        Controls.Add(PlayerLose_Text);
        Controls.Add(PlayerWin_Text);
        Controls.Add(PlayerLose_Label);
        Controls.Add(PlayerWin_Label);
        Controls.Add(PlayerNickname_Label);
        Controls.Add(Player_Panel);
        Controls.Add(Reward_Box);
        Controls.Add(Game_Box);
        Controls.Add(Log_box);
        Controls.Add(Room_Box);
        Controls.Add(Server_Box);
        Controls.Add(OmokBoard);
        Controls.Add(Login_Box);
        Name = "OmokClient";
        Text = "OmokClient";
        Load += mainForm_Load;
        Login_Box.ResumeLayout(false);
        Login_Box.PerformLayout();
        Server_Box.ResumeLayout(false);
        Server_Box.PerformLayout();
        Room_Box.ResumeLayout(false);
        Room_Box.PerformLayout();
        Log_box.ResumeLayout(false);
        Game_Box.ResumeLayout(false);
        Reward_Box.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label ID_Text;
    private GroupBox Login_Box;
    private TextBox PW_Label;
    private Panel OmokBoard;
    private Button Login_Btn;
    private TextBox ID_Label;
    private Label PW_Text;
    private Button Signup_Btn;
    private Label Token_Text;
    private TextBox Token_Label;
    private GroupBox Server_Box;
    private Button Disconnect_Btn;
    private Button Connect_Btn;
    private TextBox IP_Label;
    private Label Port_Text;
    private Label IP_Text;
    private CheckBox Localhost_Check;
    private GroupBox Room_Box;
    private TextBox RoomNo_Label;
    private Label RoomNo_Text;
    private Button Chat_Btn;
    private TextBox Chat_Label;
    private GroupBox Log_box;
    private ListBox Chat_List;
    private ListBox User_List;
    private ListBox Log_List;
    private GroupBox Game_Box;
    private Button Back_Btn;
    private Button Ready_Btn;
    private Button Surrender_Btn;
    private GroupBox Reward_Box;
    private Button MailBox_Btn;
    private Panel Player_Panel;
    private TextBox PlayerNickname_Label;
    private TextBox PlayerWin_Label;
    private Label PlayerWin_Text;
    private TextBox PlayerLose_Label;
    private Label PlayerLose_Text;
    private TextBox PlayerTime_Label;
    private TextBox EnemyTime_Label;
    private Label EnemyLose_Text;
    private Label EnemyWin_Text;
    private TextBox EnemyLose_Label;
    private TextBox EnemyWin_Label;
    private TextBox EnemyNickname_Label;
    private Panel Enemy_Panel;
    private TextBox Port_Label;
    private Button Matching_Btn;
    private TextBox PlayerColor_Label;
    private TextBox EnemyColor_Label;
}