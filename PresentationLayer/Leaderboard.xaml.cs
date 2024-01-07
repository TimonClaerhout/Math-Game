using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for Leaderboard.xaml
    /// </summary>
    /// 
    public partial class Leaderboard : Window
    {
        /// <summary>
        /// Used to convert the unmanaged memory block containing the copied ANSI string of the imported C++ DLL <br></br>
        /// To a managed Unicode String object
        /// </summary>
        private IntPtr IntPtr;

        /// <summary>
        /// Import showLeaderboard of the C++ DLL "DataLayer.dll"<br></br>
        /// This will show the entire leaderboard of the specific level inside the leaderboard.db database
        /// </summary>
        [DllImport(@"..\..\..\..\x64\Debug\DataLayer.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr showLeaderboard(string table);

        /// <summary>
        /// Non-default constructor that shows the entire leaderboard of the specific level
        /// </summary>
        /// <param name="level">Level that the user played the MathGame on that has the leaderboard (normal/hard)</param>
        public Leaderboard(string level)
        {
            InitializeComponent();
            IntPtr = showLeaderboard(level);
            GlobalLeaderboard.Text = $"{Marshal.PtrToStringAnsi(IntPtr)}";
        }
        /// <summary>
        /// When the button Close is clicked then close the leaderboard window
        /// </summary>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
