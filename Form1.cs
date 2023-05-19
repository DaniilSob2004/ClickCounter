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

            // инициализируем таймера
            InitializeTimerRemainder();
            InitializeTimerStart();

            recordClicks = int.Parse(FileWork.ReadFile());  // считываем рекорд кликов из файла
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
            if (remainderSeconds == 0)  // если таймер истЄк
            {
                timerRemainder.Stop();  // останавливаем таймер
                remainderSeconds = (int)MaxValue.MaxSecondsStart;  // устанавливаем начальное значение
                MessageBox.Show("Let`s go!");
                StartMainTimer();  // запускаем таймер дл€ кликов
            }
        }

        private void ClickTimer(object? sender, EventArgs e)
        {
            timerLabel.Text = $"End via:  {--remainderSecondsClicks}";
            if (remainderSecondsClicks == 0)  // если таймер истЄк
            {
                timerStart.Stop();  // останавливаем таймер
                clickButton.Visible = false;  // пр€чем кнопку дл€ кликов
                remainderSecondsClicks = (int)MaxValue.MaxSecondsClicks;  // устанавливаем начальное значение
                CheckRecordClick();  // проверка рекорда
                counterClicks = 0;  // обнул€ем клики
                StopClicks();
            }
        }


        private void StartRemainderTimer()
        {
            timerLabel.Visible = true;  // показываем метку (дл€ отчЄта секунд)
            timerLabel.Text = $"Start via:  {remainderSeconds}";
            timerRemainder.Start();  // запуск таймера
        }

        private void StartMainTimer()
        {
            clickButton.Visible = true;  // показываем кнопку дл€ кликов
            timerLabel.Text = $"End via:  {remainderSecondsClicks}";
            timerStart.Start();  // запуск таймера
        }


        private void CheckRecordClick()
        {
            // обновл€ем рекорд
            if (counterClicks > recordClicks) recordClicks = counterClicks;
        }

        private void StopClicks()
        {
            var answer = MessageBox.Show("Do you want continiue?", "Questions", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            switch (answer)
            {
                case DialogResult.Yes:
                    StartRemainderTimer();  // запускаем кликер
                    break;

                case DialogResult.No:
                    MessageBox.Show($"–екорд: {recordClicks}", "Message");
                    FileWork.WriteFile(recordClicks.ToString());  // запись рекорда в файл
                    Close();  // закрываем окно
                    break;
            }
        }


        private void Start_MouseClick(object sender, MouseEventArgs e)
        {
            Controls.Remove(startButton);  // удал€ем кнопку start
            Controls.Remove(exitButton);  // удал€ем кнопку exit
            StartRemainderTimer();  // запуск таймера
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