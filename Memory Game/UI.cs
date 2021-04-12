//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;

//namespace Memory_Game
//{
   
//    internal class UI
//    {
//        public static void Run()
//        {
//            string firstPlayerName = UI.requestName();
//            string secondPlayerName;
//            eGameMode gameMode = UI.setGameMode(firstPlayerName, out secondPlayerName);
//            GameLogic game = new GameLogic(firstPlayerName, secondPlayerName, gameMode);
//            if (gameMode == eGameMode.Solo)
//            {
//                UI.determineAIDifficulty(game);
//            }

//            startGame(game);
//        }

//        private static void determineAIDifficulty(GameLogic i_Game)
//        {
//            int difficulty;

//            Console.WriteLine("Please select difficulty from 1 to 5, 1 is easiest, 5 is the hardest");
//            string userString = Console.ReadLine();
//            do
//            {
//                while (int.TryParse(userString, out difficulty) == false)
//                {
//                    Console.WriteLine("Wrong input");
//                    userString = Console.ReadLine();
//                }

//            } while (difficulty < 1 || difficulty > 5);

//            i_Game.SetAILogic(difficulty * 2);
//        }

//        private static void startGame(GameLogic i_Game)
//        {
//            int row;
//            int col;

//            i_Game.GameStatus = eGameStatus.FirstPlayerTurn;
//            while (i_Game.GameStatus != eGameStatus.UserWantToQuit)
//            {
              
//                UI.requestDimensions(out row, out col);
//                i_Game.SetGameTable(row, col);
//                do
//                {
                   
//                    UI.makeTurn(i_Game);                    

//                } while (i_Game.GameStatus != eGameStatus.GameOver);

             
//                UI.askUserToContuniueAndDetermineGameStatus(i_Game);
//            }

//            terminateGame();
//        }

//        // $G$ DSN-999 (-5) this method is to long - should be divided to several methods
//        private static void cpuTurn(GameLogic i_Game)
//        {
//            string firstCoordinate;
//            string secondCoordinate;

       
//            i_Game.PrintGameBoard();
//            Console.WriteLine("CPU's Turn");
//            if (i_Game.ExistsMemorizedPair(out  firstCoordinate, out secondCoordinate) == true)
//            {
//                Console.WriteLine(Environment.NewLine);
//                Console.WriteLine("Using memorized pair of coordiantes");
//                Thread.Sleep(2000);
//                Console.WriteLine(Environment.NewLine);
//                i_Game.RevealSquare(firstCoordinate, m_game.GameStatus);
//                i_Game.RevealSquare(secondCoordinate);
              
//                i_Game.PrintGameBoard();
//                Console.WriteLine("CPU's Turn");
//                Thread.Sleep(2000);
//            }
//            else
//            {
//                Console.WriteLine(Environment.NewLine);
//                Console.WriteLine("Picking first coordinate");
//                firstCoordinate = i_Game.RandomizeCoordinate();
//                Thread.Sleep(2000);
//                i_Game.RevealSquare(firstCoordinate);
              
//                i_Game.PrintGameBoard();
//                Console.WriteLine("CPU's Turn");
//                Console.WriteLine(Environment.NewLine);
//                Console.WriteLine(firstCoordinate);
//                Thread.Sleep(2000);
              
//                i_Game.PrintGameBoard();
//                Console.WriteLine("CPU's Turn");
//                Console.WriteLine(Environment.NewLine);
//                Console.WriteLine("Picking second coordinate");
//                Thread.Sleep(2000);
//                secondCoordinate = i_Game.CheckIfSquareValueInMemory(firstCoordinate);
//                i_Game.RevealSquare(secondCoordinate);
            
//                i_Game.PrintGameBoard();
//                Console.WriteLine("CPU's Turn");
//                Console.WriteLine(Environment.NewLine);
//                Console.WriteLine(secondCoordinate);
//                Thread.Sleep(2000);
//            }
         
//            if (i_Game.IsAMatch(firstCoordinate, secondCoordinate))
//            {
//                i_Game.DeletePairFromAIMemeory(firstCoordinate);              
//                i_Game.AdvanceScore(i_Game.GameStatus);
              
//                if (i_Game.IsGameOver())
//                {
//                    UI.announceWinner(i_Game);
//                }

//            }
//            else
//            {        
//                i_Game.HidePair(firstCoordinate, secondCoordinate);
//                i_Game.GameStatus = eGameStatus.FirstPlayerTurn;
//            }

//        }

//        private static void announceWinner(GameLogic i_Game)
//        {
    
//            i_Game.PrintGameBoard();
//            Console.WriteLine(Environment.NewLine);
//            Console.WriteLine("The Final score is:");
//            Console.WriteLine(i_Game.FirstPlayer.PlayerName + " Earned " + i_Game.FirstPlayer.PlayerScore + " points!");
//            Console.WriteLine(i_Game.SeconndPlayer.PlayerName + " Earned " + i_Game.SeconndPlayer.PlayerScore + " points!");
//            Console.WriteLine(Environment.NewLine);
//            if (i_Game.FirstPlayer.PlayerScore > i_Game.SeconndPlayer.PlayerScore)
//            {
//                Console.WriteLine(i_Game.FirstPlayer.PlayerName + " is the winner !!");
//                Console.WriteLine(i_Game.SeconndPlayer.PlayerName + " better luck next time!");
//            }
//            else if (i_Game.FirstPlayer.PlayerScore < i_Game.SeconndPlayer.PlayerScore)
//            {
//                Console.WriteLine(i_Game.SeconndPlayer.PlayerName + " is the winner !!");
//                Console.WriteLine(i_Game.FirstPlayer.PlayerName + " better luck next time!");
//            }
//            else
//            {
//                Console.WriteLine("It is a Tie!");
//                Console.WriteLine("Job well done !");
//            }

//            Thread.Sleep(10000);
//        }

//        private static void playerTurn(GameLogic i_Game)
//        {
//            string firstCoordinate;
//            string secondCoordinate;
//            Player player = i_Game.GameStatus == eGameStatus.FirstPlayerTurn ? i_Game.FirstPlayer : i_Game.SeconndPlayer;

         
//            i_Game.PrintGameBoard();
//            Console.WriteLine(player.PlayerName + "'s Turn");
//            Console.WriteLine("Please enter the first Coordinate for the letter you wish to reveal");
//            firstCoordinate = UI.requestCoordiante(i_Game);
//            i_Game.RevealSquare(firstCoordinate);           
//            i_Game.PrintGameBoard();
//            Console.WriteLine(player.PlayerName + "'s Turn");
//            Console.WriteLine("Please enter the second Coordinate for the letter you wish to reveal");
//            secondCoordinate = UI.requestCoordiante(i_Game);
//            i_Game.RevealSquare(secondCoordinate);           
//            i_Game.PrintGameBoard();
//            if (i_Game.IsAMatch(firstCoordinate, secondCoordinate))
//            {
//                if (i_Game.GameMode == eGameMode.Solo)
//                {
//                    i_Game.DeletePairFromAIMemeory(firstCoordinate);
//                }
               
//                i_Game.AdvanceScore(i_Game.GameStatus);
//                if (i_Game.IsGameOver())
//                {
//                    UI.announceWinner(i_Game);
//                }

//            }
//            else
//            {
//                if (i_Game.GameMode == eGameMode.Solo)
//                {
//                    i_Game.AddPlayerCoordinateToAI(firstCoordinate, secondCoordinate);
//                }

//                i_Game.GameStatus = player.Equals(i_Game.FirstPlayer) ? eGameStatus.SecondPlayerTurn : eGameStatus.FirstPlayerTurn;               
//                Thread.Sleep(2000);
//                i_Game.HidePair(firstCoordinate, secondCoordinate);
//            }

//        }

//        private static string requestName()
//        {
//            string playerName;

//            Console.WriteLine("Please type in your name");
//            playerName = Console.ReadLine();
//            while (checkName(playerName) == false || playerName == string.Empty)
//            {
//                Console.WriteLine("Try again - only letters permitted");
//                playerName = Console.ReadLine();
//                Console.WriteLine(Environment.NewLine);
//            }

//            return playerName;
//        }

//        private static bool checkName(string i_PlayerName)
//        {
//            bool result = true;
//            foreach (char c in i_PlayerName)
//            {
//                if (char.IsLetter(c) == false)
//                {
//                    result = false;
//                    break;
//                }
//            }

//            return result;
//        }

//        // $G$ CSS-999 (-3) You should have used constants\enum here.
//        // $G$ CSS-999 (-5) Out parameters should start with o_PascaleCased
//        private static void requestDimensions(out int io_Row, out int io_Col)
//        {
//            string temp;
//            bool isOdd = false;
//            Console.WriteLine("Please input the dimensions for your game board");
//            Console.WriteLine(Environment.NewLine);
//            do
//            {
//                Console.WriteLine("Please select number of rows");
//                temp = Console.ReadLine();
//                while (int.TryParse(temp, out io_Row) == false || io_Row >= 4 == false || io_Row <= 6 == false)
//                {
//                    Console.WriteLine("Wrong input! Try again");
//                    temp = Console.ReadLine();
//                }

//                Console.WriteLine("Please select number of cols");
//                temp = Console.ReadLine();
//                while (int.TryParse(temp, out io_Col) == false || io_Col >= 4 == false || io_Col <= 6 == false)
//                {
//                    Console.WriteLine("Wrong input! Try again");
//                    temp = Console.ReadLine();
//                }

//                if ((io_Row * io_Col) % 2 == 1)
//                {
//                    isOdd = true;
//                    Console.WriteLine("Wrong Input! At most ONE digit represents row OR col can be odd");
//                    Console.WriteLine(Environment.NewLine);
//                }
//                else
//                {
//                    isOdd = false;
//                }

//            } while (isOdd);
//        }

//        private static eGameMode setGameMode(string i_FirstPlayerName, out string io_SecondPlayerName)
//        {
//            int userChoice;
//            bool validInput = false;
//            eGameMode gameMode = new eGameMode();
//            Console.WriteLine("Please select game mode, type 1 to play against pc or 2 for two player match");
//            do
//            {
//                while (int.TryParse(Console.ReadLine(), out userChoice) == false) 
//                {
//                    Console.WriteLine("Wrong input, type 1 or 2");
//                }

//                if (userChoice != 1 && userChoice != 2)
//                {
//                    Console.WriteLine("Wrong input, number must be 1 or 2");
//                }
//                else
//                {
//                    validInput = true;
//                }

//            } while (validInput == false);

//            // $G$ DSN-999 (-5) Yous hould have used enums here
//            switch (userChoice)
//            {
//                case 1:
//                    gameMode = eGameMode.Solo;
//                    break;
//                case 2:
//                    gameMode = eGameMode.Duo;
//                    break;
//            }

//            io_SecondPlayerName = gameMode == eGameMode.Solo ? "CPU" : UI.requestName();
//            while (io_SecondPlayerName == i_FirstPlayerName)
//            {
//                Console.WriteLine(Environment.NewLine);
//                Console.WriteLine("Name already taken, please select another name");
//                io_SecondPlayerName = UI.requestName();
//            }

//            return gameMode;
//        }

//        private static string requestCoordiante(GameLogic i_Game)
//        {
//            string userCoordiante;
//            bool errorFlag;
//            do
//            {
//                userCoordiante = Console.ReadLine();
//                if (userCoordiante == "Q")
//                {
//                    terminateGame();
//                }

//                errorFlag = false;
//                if (userCoordiante.Equals(string.Empty) || userCoordiante.Length > 2 || userCoordiante.Length == 1 || char.IsLower(userCoordiante[0]) == true || char.IsUpper(userCoordiante[0]) == false || char.IsDigit(userCoordiante[1]) == false)
//                {
//                    Console.WriteLine("Incorrect input format, the format should be CAPITAL LETTER-NUMBER");
//                    errorFlag = true;
//                }
//                else if (userCoordiante[0] >= ('A' + i_Game.GameBoardNumOfColumns))
//                {
//                    Console.WriteLine("Incorrect input, " + userCoordiante + " is out of bounds, please input again");
//                    errorFlag = true;
//                }
//                else if (userCoordiante[1] < '0' || userCoordiante[1] > '0' + i_Game.GameBoardNumOfRows) 
//                {
//                    Console.WriteLine("Incorrect input," + userCoordiante + " has invalid characters, please input again");
//                    errorFlag = true;
//                }
//                else if (userCoordiante[1] > ('0' + i_Game.GameBoardNumOfRows))
//                {
//                    Console.WriteLine("Incorrect input, " + userCoordiante + " is out of bounds, please input again");
//                    errorFlag = true;
//                }
//                else if (i_Game.IsRevealValid(userCoordiante) != true)
//                {
//                    Console.WriteLine(Environment.NewLine);
//                    Console.WriteLine("Invalid input! the letter is already revealed");
//                    errorFlag = true;
//                }

//            } while (errorFlag == true);

//            return userCoordiante;
//        }

//        private static void askUserToContuniueAndDetermineGameStatus(GameLogic i_Game)
//        {
//            char userChar;
//            string userString;
//            Console.WriteLine("Do you want to continue Playing ?");
//            Console.WriteLine(Environment.NewLine);
//            Console.WriteLine("Please type y to continue or n to quit");
//            userString = Console.ReadLine();
//            userChar = userString.Equals(string.Empty) == true ? '0' : userString[0];
//            while (userChar.Equals('y') == false && userChar.Equals('n') == false)
//            {
//                Console.WriteLine("Wrong input!!");
//                Console.WriteLine(Environment.NewLine);
//                Console.WriteLine("Please y to continue or n to quit");
//                Console.WriteLine(Environment.NewLine);
//                userString = Console.ReadLine();
//                userChar = userString.Equals(string.Empty) == true ? '0' : userString[0];
//            }

//            if (userChar.Equals('y') == true)
//            {
//                i_Game.GameStatus = eGameStatus.FirstPlayerTurn;
//            }
//            else
//            {
//                i_Game.GameStatus = eGameStatus.UserWantToQuit;
//            }

//        }

//        private static void makeTurn(GameLogic i_Game)
//        {
//            while (i_Game.GameStatus != eGameStatus.GameOver && i_Game.GameStatus == eGameStatus.FirstPlayerTurn)
//            {
//                UI.playerTurn(i_Game);
//            }

//            while (i_Game.GameStatus != eGameStatus.GameOver && i_Game.GameStatus == eGameStatus.SecondPlayerTurn)
//            {
//                if (i_Game.GameMode == eGameMode.Solo)
//                {
//                    UI.cpuTurn(i_Game);
//                }
//                else
//                {
//                    UI.playerTurn(i_Game);
//                }

//            }  
            
//        }

//        private static void terminateGame()
//        {
//            Console.WriteLine("The game has been terminated by user request");
//            Console.WriteLine(Environment.NewLine);
//            Console.WriteLine("See you next time!");
//            Thread.Sleep(2000);
//            Environment.Exit(0);
//        }
//    }

//}
