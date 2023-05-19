namespace ClickCounter
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer timerRemainder = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer timerStart = new System.Windows.Forms.Timer();

        enum MaxValue { MaxSecondsStart = 3, MaxSecondsClicks = 10 };
        static int remainderSeconds = 3;
        static int remainderSecondsClicks = 10;

        static int counterClicks = 0;
        static int recordClicks = 0;

        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ClickCounter";

            // �������������� �������
            InitializeTimerRemainder();
            InitializeTimerStart();

            recordClicks = int.Parse(FileWork.ReadFile());  // ��������� ������ ������ �� �����
        }

        private void InitializeTimerRemainder()
        {
            timerRemainder.Interval = 1000;
            timerRemainder.Tick += RemainderTimer;
        }

        private void InitializeTimerStart()
        {
            timerStart.Interval = 1000;
            timerStart.Tick += ClickTimer;
        }


        private void RemainderTimer(object? sender, EventArgs e)
        {
            timerLabel.Text = $"Start via:  {--remainderSeconds}";
            if (remainderSeconds == 0)  // ���� ������ ����
            {
                timerRemainder.Stop();  // ������������� ������
                remainderSeconds = (int)MaxValue.MaxSecondsStart;  // ������������� ��������� ��������
                MessageBox.Show("Let`s go!");
                StartMainTimer();  // ��������� ������ ��� ������
            }
        }

        private void ClickTimer(object? sender, EventArgs e)
        {
            timerLabel.Text = $"End via:  {--remainderSecondsClicks}";
            if (remainderSecondsClicks == 0)  // ���� ������ ����
            {
                timerStart.Stop();  // ������������� ������
                clickButton.Visible = false;  // ������ ������ ��� ������
                remainderSecondsClicks = (int)MaxValue.MaxSecondsClicks;  // ������������� ��������� ��������
                CheckRecordClick();  // �������� �������
                counterClicks = 0;  // �������� �����
                StopClicks();
            }
        }


        private void StartRemainderTimer()
        {
            timerLabel.Visible = true;  // ���������� ����� (��� ������ ������)
            timerLabel.Text = $"Start via:  {remainderSeconds}";
            timerRemainder.Start();  // ������ �������
        }

        private void StartMainTimer()
        {
            clickButton.Visible = true;  // ���������� ������ ��� ������
            timerLabel.Text = $"End via:  {remainderSecondsClicks}";
            timerStart.Start();  // ������ �������
        }


        private void CheckRecordClick()
        {
            // ��������� ������
            if (counterClicks > recordClicks) recordClicks = counterClicks;
        }

        private void StopClicks()
        {
            var answer = MessageBox.Show("Do you want continiue?", "Questions", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            switch (answer)
            {
                case DialogResult.Yes:
                    StartRemainderTimer();  // ��������� ������
                    break;

                case DialogResult.No:
                    MessageBox.Show($"������: {recordClicks}", "Message");
                    FileWork.WriteFile(recordClicks.ToString());  // ������ ������� � ����
                    Close();  // ��������� ����
                    break;
            }
        }


        private void Start_MouseClick(object sender, MouseEventArgs e)
        {
            Controls.Remove(startButton);  // ������� ������ start
            Controls.Remove(exitButton);  // ������� ������ exit
            StartRemainderTimer();  // ������ �������
        }

        private void Click_MouseClick(object sender, MouseEventArgs e)
        {
            counterClicks++;
        }

        private void Exit_MouseClick(object sender, MouseEventArgs e)
        {
            var answer = MessageBox.Show("Do you want exit?", "Questions", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (answer == DialogResult.Yes) Close();
        }
    }
}