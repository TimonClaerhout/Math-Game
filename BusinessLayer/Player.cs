namespace BusinessLayer
{
    public class Player
    {
        /// <summary>
        /// Non-default constructor that inserts the username of the player inside the class
        /// </summary>
        /// <param name="name">Username of the user that is playing the math game</param>
        /// This is done by setting the string name equal to the private struct settings which has an username attribute.
        public Player(string name)
        {
            Username = name;
        }
        /// <summary>
        /// Set the math game on level easy
        /// </summary>
        /// This is done by setting the boolean easyLevel of the Settings struct on true, but also the other levels to false.
        /// Otherwise when another level is clicked by the ComboBox in the LoginWindow, multiple booleans will be set to true instead of one.
        public void SetEasyLevel()
        {
            easyLevel = true;
            normalLevel = false;
            hardLevel = false;
        }
        /// <summary>
        /// Set the math game on level normal
        /// </summary>
        /// This is done by setting the boolean normalLevel of the Settings struct on true, but also the other levels to false.
        /// Otherwise when another level is clicked by the ComboBox in the LoginWindow, multiple booleans will be set to true instead of one.
        public void SetNormalLevel()
        {
            easyLevel = false;
            normalLevel = true;
            hardLevel = false;
        }
        /// <summary>
        /// Set the math game on level hard
        /// </summary>
        /// This is done by setting the boolean hardLevel of the Settings struct on true, but also the other levels to false.
        /// Otherwise when another level is clicked by the ComboBox in the LoginWindow, multiple booleans will be set to true instead of one.
        public void SetHardLevel()
        {
            easyLevel = false;
            normalLevel = false;
            hardLevel = true;
        }
        /// <returns>Boolean if normal level is selected by the user</returns>
        public bool IsNormalSelected()
        {
            return normalLevel;
        }
        /// <returns>Boolean if hard level is selected by the user</returns>
        public bool IsHardSelected()
        {
            return hardLevel;
        }
        /// <summary>
        /// The name of the user that is playing the Math Game
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Time that the user needed to solve the Math Challenges
        /// </summary>
        public double Time { get; set; }

        // Attributes

        /// <summary>
        /// Boolean if level easy is selected of the Math Game
        /// </summary>
        private bool easyLevel;
        /// <summary>
        /// Boolean if level normal is selected of the Math Game
        /// </summary>
        private bool normalLevel;
        /// <summary>
        /// Boolean if level hard is selected of the Math Game
        /// </summary>
        private bool hardLevel;
    }
}