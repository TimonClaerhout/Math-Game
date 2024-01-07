using System;

namespace BusinessLayer
{

    public class MathChallenge
    {
        /// <summary>
        /// Default constructor which generates random operand with random numbers for easy and normal level.
        /// </summary>
        /// The default constructor of the MathChallenge class uses the Random class for generating random numbers.
        /// These numbers are generated for easy and normal level with a switch statement, the case number is randombly generated whereby:
        /// case 1 = '+' operand -> random numbers between 0 - 20
        /// case 2 = '-' operand -> random numbers between 0 - 20
        /// case 3 = '*' operand -> random number between 0 - 20 and 0 - 10
        /// case 4 = '/' operand -> searches for a dividable number with a do while statement
        public MathChallenge()
        {
            operand = Generator.Next(1, 5);
            switch (operand)
            {
                case 1: goto case 2;
                case 2:
                    Left = Generator.Next(0, 21);
                    Right = Generator.Next(0, 21);
                    break;
                case 3:
                    Left = Generator.Next(0, 21);
                    Right = Generator.Next(0, 11);
                    break;
                case 4:
                    Left = Generator.Next(4, 25);
                    do
                    {
                        Right = Generator.Next(1, 21);
                    } while (Left % Right != 0);
                    break;
            }
        }
        /// <summary>
        /// Generates random operand with random numbers for hard level.
        /// </summary>
        /// These numbers and operand are generated exactly the same method as explained in the constructor.
        /// But with the difference that there are larger numbers; no multiplication by 0,1 or 2; no division by 1 or by itself
        public void GenerateHardNumbers()
        {
            operand = Generator.Next(1, 5);
            switch (operand)
            {
                case 1: goto case 2;
                case 2:
                    Left = Generator.Next(0, 101);
                    Right = Generator.Next(0, 101);
                    break;
                case 3:
                    Left = Generator.Next(20, 41);
                    Right = Generator.Next(3, 21);
                    break;
                case 4:
                    Left = Generator.Next(4, 101);
                    do
                    {
                        Right = Generator.Next(2, 51);
                    } while ((Left % Right != 0) && (Left != Right));
                    break;
            }
        }
        /// <returns>Integer with the Left number of the math challenge</returns>
        public int Left { get; private set; }
        /// <returns>Integer with the Right number of the math challenge</returns>
        public int Right { get; private set; }
        /// <returns>Char with the Right operand sign of the swich statement that is used by the MathChallenge class</returns>
        public char Operand()
        {
            switch (operand)
            {
                case 1: return '+';
                case 2: return '-';
                case 3: return '*';
                case 4: return '/';
            }
            return '0';
        }
        /// <summary>
        /// Player that is trying to solve the MathChallenge with his answer
        /// </summary>
        /// This is done by setting the integer result equal to the private integer attempt of the class.
        /// That way the attempt is only accessible by the MathChallenge class.
        /// <param name="result">Integer that the user inserted for trying to solve the challenge</param>
        public void Solve(int result)
        {
            attempt = result;
            isSolved = true;
        }
        /// <returns>Integer with the correct solution of the math challenge</returns>
        /// This is done by using the same switch statement of the MathChallenge class, paired with the correct operand.
        private int Solution()
        {
            switch (operand)
            {
                case 1: return Left + Right;
                case 2: return Left - Right;
                case 3: return Left * Right;
                case 4: return Left / Right;
            }
            return 0;
        }
        /// <returns>Boolean that checks if the user has correctly solved the math challenge</returns>
        /// This is dony by comparing the correct solution of the math challenge by the attempt that the user tried to solve the challenge.
        public bool IsCorrectlySolved()
        {
            return Solution() == attempt;
        }
        /// <returns>Integer with value '1' if the math challenge is correctly solved or with value '0' if not</returns>
        /// This is done by checking if the challenge was corretly solved by the user with IsCorrectlySolved() method.
        public int Score()
        {
            if (IsCorrectlySolved())
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// Overriding base class ToString(), thereby this function is used when the MathChallenge class is used inside a string.
        /// </summary>
        /// <returns> String with the current math challenge and Right after, <br></br>
        /// When solved correctly is a 'V' displayed.<br></br>
        /// When nothing is typed is a '?' displayed <br></br>
        /// When solved incorrectly 'X' is displayed with the correctly answer.</returns>
        public override string ToString()
        {
            string output = $"{Left} {Operand()} {Right} = ";

            if (!isSolved)
            {
                output += "?";
            }
            else
            {
                if (IsCorrectlySolved())
                {
                    output += $"{attempt} [V]";
                }
                else
                {
                    output += $"{attempt} [X {Solution()}]";
                }
            }
            return output;
        }

        // Attributes

        /// <summary>
        /// Will be used in a switch statement that determines the random operand of the MathChallenge
        /// </summary>
        private int operand = 0;
        /// <summary>
        /// Holds the answer that the user has inserted, to solve the MathChallenge
        /// </summary>
        private int attempt = 0;
        /// <summary>
        /// Boolean that checks if the MathChallenge is answered by the user or not
        /// </summary>
        private bool isSolved = false;

        // Static class attribute
        /// <summary>
        /// Gives a random integer that is used for the random numbers and operand inside the MathChallenge
        /// </summary>
        private static readonly Random Generator = new Random();
    }
}