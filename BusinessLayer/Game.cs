namespace BusinessLayer
{
    public class Game
    {
        /// <summary>
        /// Default constructor of Game class that creates the math challenges
        /// </summary>
        public Game()
        {
            CreateChallenges();
        }
        /// <summary>
        /// Creates 10 different MathChallenges by accesing the default constructor of the class MathChallenge <br></br>
        /// These 10 challenges are put in an MathChallenge array
        /// </summary>
        private void CreateChallenges()
        {
            for (int i = 0; i < Challenges.Length; i++)
            {
                Challenges[i] = new MathChallenge();
            }
        }
        /// <summary>
        /// If the game is not finished, return new MatChallenge
        /// </summary>
        /// <returns>Next index of array Challenges which returns a new math challenge</returns>
        public MathChallenge NextChallenge()
        {
            if (!IsFinished())
            {
                return Challenges[currentChallenge++];
            }
            else
            {
                return null;
            }
        }

        /// <returns>Boolean if all challenges are executed or not</returns>
        public bool IsFinished()
        {
            return currentChallenge >= Challenges.Length;
        }
        /// <returns>Integer with the amount of challenges that are generated</returns>
        public int NumberOfChallenges()
        {
            return Challenges.Length;
        }
        /// <returns>Integer with the amount of challenges that are executed</returns>
        public int NumberOfSolvedChallenge()
        {
            return currentChallenge;
        }
        /// <summary>
        /// Loops over the Challenges array to count the total score.
        /// </summary>
        /// <returns>Integer with the total score</returns>
        public int Score()
        {
            int score = 0;
            foreach (var challenge in Challenges)
            {
                score += challenge.Score();
            }
            return score;
        }
        /// <returns>String with an overview of the given math challenges and the total score of correctly solved challenges</returns>
        public string Overview()
        {
            string output = "Your challenge overview:\n";
            foreach (var challenge in Challenges)
            {
                output += challenge + "\n";
            }
            output += $"\nYou finished the game with a score of {Score()}";
            return output;
        }

        // Attributes

        /// <summary>
        /// Contains an array of different MathChallenges that the user has to solve
        /// </summary>
        private readonly MathChallenge[] Challenges = new MathChallenge[10];
        /// <summary>
        /// The current index inside the Challenges array that the user is trying to solve the MathChallenge
        /// </summary>
        private int currentChallenge = 0;
    }
}