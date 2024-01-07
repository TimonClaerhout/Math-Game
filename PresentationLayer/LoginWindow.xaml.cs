using System.Windows;
using BusinessLayer;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        /// <summary>
        /// Holds the statistics of the current player that is playing 
        /// </summary>
        private Player Player = null;
        /// <summary>
        /// Default constructor that inializes the Player with his/her nickname
        /// </summary>
        public LoginWindow()
        {
            InitializeComponent();
            Player = new Player(Nickname.Text);
        }
        /// <summary>
        /// Non-default constructor that is used when the player has clicked play again at the end of the previous game<br></br>
        /// Thereby is the username that was previously used, used again
        /// </summary>
        public LoginWindow(string username)
        {
            InitializeComponent();
            Nickname.Text = username;
            Player = new Player(Nickname.Text);
        }

        /// <summary>
        /// Called when level "easy" is selected in the ComboBox
        /// </summary>
        private void EasySelected(object sender, RoutedEventArgs e)
        {
            // Because the ComboBox is inialised first and is set default by level "easy", the Player class is then for a short moment
            // Unialised (= NULL). To prevent this scenario, the if statement used.
            if (Player != null)
            {
                Player.SetEasyLevel();
            }
        }
        /// <summary>
        /// Called when level "normal" is selected in the ComboBox
        /// </summary>
        private void NormalSelected(object sender, RoutedEventArgs e)
        {
            Player.SetNormalLevel();
        }
        /// <summary>
        /// Called when level "hard" is selected in the ComboBox
        /// </summary>
        private void HardSelected(object sender, RoutedEventArgs e)
        {
            Player.SetHardLevel();
        }
        //It makes sure that the player enters a nickname and gives that nickname to the MainWindow
        //Together with 2 booleans wich determines the level that is selected.
        private void LoginClick(object sender, RoutedEventArgs e)
        {
            Player.Username = Nickname.Text;
            // When the nickname is filled in then create the mainwindow with the specific level and username of the user
            if (Nickname.Text != "")
            {
                MainWindow mainwindow = new MainWindow(Player);
                mainwindow.Show();
                this.Close();
            }
            else
            {
                // When the nickname is empty then show the MessageBox
                MessageBox.Show("Please enter nickname", "The Math Game");
                Nickname.Focus();
            }
        }
    }
}
