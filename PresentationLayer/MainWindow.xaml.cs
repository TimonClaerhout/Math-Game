using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using System.Diagnostics;
using System.Media;
using BusinessLayer;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //attributes
        /// <summary>
        /// The time that the user gets to solve the MathChallenge
        /// </summary>
        private int time = 6;
        /// <summary>
        /// Used for exeption handling if the user entered entered something that wasn't a number
        /// </summary>
        private int valid = 0;
        /// <summary>
        /// Holds the statistics of the current player that is playing
        /// </summary>
        private Player Player = null;
        /// <summary>
        /// Game logic of the Math Game
        /// </summary>
        private Game Game = null;
        /// <summary>
        /// Gives math challenge with random numbers and operand
        /// </summary>
        private MathChallenge CurrentChallenge = null;
        /// <summary>
        /// Timer that will give the user 5 seconds to solve the MathChallenge in level normal and hard
        /// </summary>
        private DispatcherTimer Timer;
        /// <summary>
        /// Stopwatch that will keep track of how much time the user needed to solve the MathChallenges
        /// </summary>
        private Stopwatch Sw;
        /// <summary>
        /// Sound "Correct.wav" will be heared when the user solved the MathChallenge correctly
        /// </summary>
        private SoundPlayer Correct = new SoundPlayer("Correct.wav");
        /// <summary>
        /// Sound "Wrong.wav" will be heared when the user solved the MathChallenge incorrectly
        /// </summary>
        private SoundPlayer Wrong = new SoundPlayer("Wrong.wav");

        /// <summary>
        /// Import insertPlayer of the C++ DLL "DataLayer.dll"<br></br>
        /// It inserts the current player with his statistics inside the leaderboard.db database
        /// </summary>
        /// <param name="table">Which table of the database (normal/hard)</param>
        /// <param name="username">Name of the user that has played the MathGame</param>
        /// <param name="score">Total score of the user</param>
        /// <param name="time">Time that the user needed to solve all challenges</param>
        [DllImport(@"..\..\..\..\x64\Debug\DataLayer.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void insertPlayer(string table, string username, int score, double time);

        /// <summary>
        /// Non-default constructor that will inialise the components, sounds, timer, stopwatch and player info
        /// </summary>
        /// <param name="Player">Holds the statistics of the current player that is playing</param>
        public MainWindow(Player Player)
        {
            this.Player = Player;// Load player statistics inside the private class Player
            Correct.LoadAsync(); // Load "Correct.wav" sound asynchronous with a new thread 
            Wrong.LoadAsync();   // Load "Wrong.wav"   sound asynchronous with a new thread 
            InitializeComponent();
            Welcome.Text = $"Welcome {Player.Username}. Press start to play the game. Good Luck!";
            if (Player.IsNormalSelected() || Player.IsHardSelected())
            {
                SetTimer();
            }
        }
        /// <summary>
        /// Inialise the DispatcherTimer and Stopwatch
        /// </summary>
        private void SetTimer()
        {
            //Setting the timer wich calls the function Timer_Tick.
            Timer = new DispatcherTimer();
            Sw = new Stopwatch();
            // Each second is a Tick generated inside the Timer
            Timer.Interval = new TimeSpan(0, 0, 1);
            // When the Tick is generated (after 1 second) then the function TimerTick is called
            Timer.Tick += TimerTick;
        }
        /// <summary>
        /// Countdown of the MathChallenge in level normal/hard, when user hasn't solved his challenge before the time expired <br></br>
        /// Then go to the next MathChallenge
        /// </summary>
        private void TimerTick(object sender, EventArgs e)
        {
            //Count down of timer
            if (time > 0)
            {
                time--;
                TBCountDown.Text = string.Format("0{0}:0{1}", time / 60, time % 60);
                //if the time goes under 3 seconds color changes to red.
                if (time <= 3)
                {
                    TBCountDown.Foreground = Brushes.Red;
                }
                //otherwise the timer color is black.
                if (time == 5)
                {
                    TBCountDown.Foreground = Brushes.Black;
                }
            }
            else
            {
                /* If the game isn't finished yet and the player ran out of time.
                Then will the timer set again to 5 seconds and prepare the next challenge. */
                if (!Game.IsFinished())
                {
                    PrepareNextChallenge();
                }
                /* If the game is finished and the player didn't answer the last challenge, we dont need
                 to give him another challenge. The method End() is used to view the score in the EndWindow and 
                To stop the timer so that TimerTick method isn't called again. */
                else
                {
                    UpdateChallengeProgress();
                    End();
                }
            }
        }

        /// <summary>
        /// If startgame button is clicked the next challenge and the progress bar is showed
        /// </summary>
        private void StartGameClick(object sender, RoutedEventArgs e)
        {
            Explenation.Visibility = Visibility.Hidden;
            Game = new Game();
            StartGame.Visibility = Visibility.Hidden;
            progress.Maximum = Game.NumberOfChallenges();
            PrepareNextChallenge();
        }

        /// <summary>
        /// It gives the left and right number of the MathChallenge inside the TextBox that the user has to solve<br></br>
        /// If level hard is selected we are generating larger numbers.
        /// </summary>
        private void GiveNumbers()
        {
            if (CurrentChallenge != null)
            {
                if (Player.IsHardSelected())
                {
                    CurrentChallenge.GenerateHardNumbers();
                }
                LeftValue.Text = CurrentChallenge.Left.ToString();
                RightValue.Text = CurrentChallenge.Right.ToString();
                Operand.Text = CurrentChallenge.Operand().ToString();
            }
        }
        /// <summary>
        /// Gives next MathChallenge by setting the timer, play the sound, update the challenge progress and give new numbers
        /// </summary>
        private void PrepareNextChallenge()
        {
            if (Player.IsNormalSelected() || Player.IsHardSelected())
            {
                // Set time to 6 because when the WPF is updated with the new MathChallenge only number 5 will be seen
                time = 6;
                Timer.Start(); // Start the DispatcherTimer
                Sw.Start();    // Start the Stopwatch
            }
            UpdateChallengeProgress();
            CurrentChallenge = Game.NextChallenge();
            GiveNumbers();
            Solution.Text = "";
            Solution.Focus();
        }
        /// <summary>
        /// Updates the progess bar and play a specific sound if the player solved a challenge
        /// </summary>
        private void UpdateChallengeProgress()
        {
            progress.Value = Game.NumberOfSolvedChallenge();
            ProgressLabel.Text = $"Solved {Game.NumberOfSolvedChallenge()} of {Game.NumberOfChallenges()} challenges";
            if (CurrentChallenge != null)
            {
                // When Math Challenge is correclty solved
                if (CurrentChallenge.IsCorrectlySolved())
                {
                    Correct.Play();
                    progress.Foreground = new SolidColorBrush(Colors.Green);
                    Welcome.Text = $"Well done {Player.Username}!";
                }
                // When Math Challenge is incorreclty solved
                else
                {
                    Wrong.Play();
                    progress.Foreground = new SolidColorBrush(Colors.Red);
                    Welcome.Text = $"Next time better {Player.Username}.";
                }
            }
        }
        /// <summary>
        /// When the MathGame has ended then can't the player type any value, the challenge bar is updated and the MainWindow is closed<br></br>
        /// After that it opens the EndWindow with an overview and statistics of the player
        /// </summary>
        private void End()
        {
            if (!Game.IsFinished())
            {
                PrepareNextChallenge();
            }
            else
            {
                if (Player.IsNormalSelected() || Player.IsHardSelected())
                {
                    Timer.Stop();
                    Sw.Stop();
                    Player.Time = Sw.ElapsedMilliseconds / 1000.0;
                    if (Player.IsNormalSelected())
                    {
                        insertPlayer("Normal_Leaderboard", Player.Username, Game.Score(), Player.Time);
                    }
                    else
                    {
                        insertPlayer("Hard_Leaderboard", Player.Username, Game.Score(), Player.Time);
                    }
                }
                UpdateChallengeProgress();
                //Makes sure that the player cant type anything in the box.
                Solution.IsEnabled = false;
                //It gives the points of the player to the EndWindow.
                EndWindow EndWindow = new EndWindow(Game.Overview(), Player);
                EndWindow.Show();
                this.Close();
            }
        }
        /// <summary>
        /// Make sure the game doesn't crash if the player enters a letter or something else instead of an number<br></br>
        /// Thereby is exeption handling used (with try & catch)
        /// </summary>
        private void SolutionKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Solution.Text != "")
            {
                do
                {
                    try
                    {
                        int userSolution = Convert.ToInt32(Solution.Text);
                        CurrentChallenge.Solve(userSolution);
                        valid = 1;
                    }
                    catch (SystemException)
                    {
                        Solution.Text = "";
                        valid = 0;
                        return;
                    }
                }
                while (valid < 1 && Solution.Text != "");
                End();
            }
        }
        /// <summary>
        /// If the player wants some more instructions of how to play the game at the selected level this MessageBox will be shown
        /// </summary>
        private void ExplenationClick(object sender, RoutedEventArgs e)
        {
            if (Player.IsHardSelected())
            {
                MessageBox.Show("If you press the button:'start game' 2 big numbers with an operand will pop at your screen. \n" +
                "You have to solve it but you have only 5 seconds, so be quick! \n" +
                "A timer will be counting down above the math challenge.\n" +
                "If you didn't enter a number within that time, you won't score a point and it will move on to the next challenge. \n" +
                "The challenges contains different oparandi: +, -, * and /. \n" +
                "There are 10 challenges in total that you have to solve. \n" +
                "The time you needed to solve those 10 challenges will be saved in the hard leaderboard.\n" +
                "Good luck with getting that number one spot!", "The Math Game");
            }
            else if (Player.IsNormalSelected())
            {
                MessageBox.Show("If you press the button:'start game' 2 numbers with an operand will pop at your screen. \n" +
                "You have to solve it but you have only 5 seconds, so be quick! \n" +
                "A timer will be counting down above the math challenge.\n" +
                "If you didn't enter a number within that time, you won't score a point and it will move on to the next challenge. \n" +
                "The challenges contains different oparandi: +, -, * and /. \n" +
                "There are 10 challenges in total that you have to solve. \n" +
                "The time you needed to solve those 10 challenges will be saved in the normal leaderboard.\n" +
                "Good luck with getting that number one spot!", "The Math Game");
            }
            else
            {
                MessageBox.Show("If you press the button:'start game' 2 numbers with an operand will pop at your screen. \n" +
                "You can solve the challenges at your own time just make sure the solution is right. \n" +
                "The challenges contains different oparandi: +, -, * and /. \n" +
                "There are 10 challenges in total that you have to solve, good luck!", "The Math Game");
            }
        }
    }
}