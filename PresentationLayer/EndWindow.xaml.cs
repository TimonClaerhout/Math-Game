using BusinessLayer;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for EndWindow.xaml
    /// </summary>
    public partial class EndWindow : Window
    {
        /// <summary>
        /// Variable used for the DispatcherTimer
        /// </summary>
        private int time = 10;
        /// <summary>
        /// Calls the TimerTick method each second with the time variable
        /// </summary>
        private DispatcherTimer Timer;
        /// <summary>
        /// Holds the statistics of the current player that has played 
        /// </summary>
        private Player Player;
        /// <summary>
        /// Used to convert the unmanaged memory block containing the copied ANSI string of the imported C++ DLL <br></br>
        /// To a managed Unicode String object
        /// </summary>
        private IntPtr IntPtr;

        /// <summary>
        /// Import showTop3 of the C++ DLL "DataLayer.dll"<br></br>
        /// Shows the top 3 players with their statistics at the specific level inside the leaderboard.db database
        /// </summary>
        [DllImport(@"..\..\..\..\x64\Debug\DataLayer.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr showTop3(string table);

        /// <summary>
        /// Import showLeaderboardUser of the C++ DLL "DataLayer.dll"<br></br>
        /// Shows the place that the user ended on in the leaderboard.db database 
        /// </summary>
        [DllImport(@"..\..\..\..\x64\Debug\DataLayer.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr showLeaderboardUser(string table, string username, double time);

        /// <summary>
        /// Shows the result of the player with the given string Result of MainWindow. 
        /// </summary>
        /// <param name="result">Overview of all MathChallenges that the user tried to solve</param>
        /// <param name="Player">All statistics of the player at the end of the game</param>
        public EndWindow(string result, Player Player)
        {
            this.Player = Player;
            InitializeComponent();
            // Show overview of all challenges the user tried to solve with correct answer + total score
            Results.Text = $"{result}";
            if (Player.IsNormalSelected())
            {
                // Show what place the current user ended on the normal leaderboard
                IntPtr = showLeaderboardUser("Normal_Leaderboard", Player.Username, Player.Time);
                LeaderboardUser.Text = $"{Marshal.PtrToStringAnsi(IntPtr)}";

                // Show the top 3 players of the normal leaderboard
                IntPtr = showTop3("Normal_Leaderboard");
                LeaderboardTop3.Text = $"{Marshal.PtrToStringAnsi(IntPtr)}";
            }
            else if (Player.IsHardSelected())
            {
                // Show what place the current user ended on the hard leaderboard
                IntPtr = showLeaderboardUser("Hard_Leaderboard", Player.Username, Player.Time);
                LeaderboardUser.Text = $"{Marshal.PtrToStringAnsi(IntPtr)}";

                // Show the top 3 players of the hard leaderboard
                IntPtr = showTop3("Hard_Leaderboard");
                LeaderboardTop3.Text = $"{Marshal.PtrToStringAnsi(IntPtr)}";
            }
            else
            {
                LeaderboardUser.Visibility = Visibility.Collapsed;
                LeaderboardTop3.Visibility = Visibility.Collapsed;
                Show_leaderboard.Visibility = Visibility.Collapsed;
            }
            SetTimer();
            Timer.Start();
        }
        /// <summary>
        /// Inialize the DispatcherTimer class and call the TimerTick method each second
        /// </summary>
        private void SetTimer()
        {
            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Tick += TimerTick;
        }
        /// <summary>
        /// Every second this method is called to switch colours of the play again button
        /// </summary>
        private void TimerTick(object sender, EventArgs e)
        {
            //Count down of timer
            if (time > 0)
            {
                time--;
                //if the time is even the background of the button Playagain will change to yellow.
                if (time % 2 == 1)
                {
                    PlayAgain.Background = Brushes.Yellow;
                }
                //if the time is uneven the background of the button Playagain will change to LightGreen.
                else if (time % 1 == 0)
                {
                    PlayAgain.Background = Brushes.LightGreen;
                }
                //So will the timer never stop if the user dont press a button.
                if (time == 0)
                {
                    time = 10;
                }
            }
        }
        /// <summary>
        /// If the player wants to play again than it closes the EndWindow and opens The LoginWindow.
        /// </summary>
        private void PlayAgainClick(object sender, RoutedEventArgs e)
        {
            LoginWindow LoginWindow = new LoginWindow(Player.Username);
            LoginWindow.Show();
            this.Close();
        }

        /// <summary>
        /// If the player wants to exit the game, it closes the EndWindow and the game will stop running.
        /// </summary>
        private void ExitClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// When the user hovers over the Exit button than "Are you sure?" is displayed on the button
        /// </summary>
        private void ExitMouseEnter(object sender, MouseEventArgs e)
        {
            Exit.Content = "Are you sure?";
        }
        /// <summary>
        /// When the user leaves the button then "exit" is displayed on the button
        /// </summary>
        private void ExitMouseLeave(object sender, MouseEventArgs e)
        {
            Exit.Content = "exit";
        }
        /// <summary>
        /// Show the entire leaderboard of the MathGame at the correct level (normal/hard)
        /// </summary>
        private void ShowLeaderboardClick(object sender, RoutedEventArgs e)
        {
            if (Player.IsNormalSelected())
            {
                Leaderboard NormalLeaderboard = new Leaderboard("Normal_Leaderboard");
                NormalLeaderboard.Show();
            } else
            {
                Leaderboard HardLeaderboard = new Leaderboard("Hard_Leaderboard");
                HardLeaderboard.Show();
            }
        }
    }
}
