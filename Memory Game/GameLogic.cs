using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory_Game
{
    public class GameLogic
    {
        private readonly eGameMode r_GameMode;
        private Player m_FirstPlayer;
        private Player m_SecondPlayer;
        private AILogic m_AiLogic;
        private eGameStatus m_GameStatus;
        private Coordinate[,] m_GameBoard;
        private int m_GameBoardNumOfRows;
        private int m_GameBoardNumOfColumns;
        private Random m_Random = new Random();
     
        public GameLogic(string i_FirstPlayerString, string i_SecondPlayerString, eGameMode i_GameMode)
        {
            m_FirstPlayer = new Player(i_FirstPlayerString);
            m_SecondPlayer = new Player(i_SecondPlayerString);
            r_GameMode = i_GameMode;
        }

        public int GameBoardNumOfColumns
        {
            get
            {
                return m_GameBoardNumOfColumns;
            }
            set
            {
                m_GameBoardNumOfColumns = value;
            }
        }

        public void SetGameTable(int i_Row, int i_Col)
        {
            m_GameBoardNumOfRows = i_Row;
            m_GameBoardNumOfColumns = i_Col;
            m_FirstPlayer.PlayerScore = 0;
            m_SecondPlayer.PlayerScore = 0;
            GameBoard = generateTable();
            hideAllSquares();
        }

        public int GameBoardNumOfRows
        {
            get { return m_GameBoardNumOfRows; }
            set { m_GameBoardNumOfRows = value; }
        }

        public string RandomizeCoordinate()
        {
            string returnedValue;
            int row;
            int col;

            randomizeCoordiante(out row, out col);
            returnedValue = string.Format("{0}{1}", (char)('A' + col), (char)('1' + row));
            m_AiLogic.AddCoordinate(new Coordinate(returnedValue, getCoordianteValue(returnedValue), false));

            return returnedValue;
        }

        public void HidePair(string i_FirstCoordinate, string i_SecondCoordinate)
        {
            GameBoard[i_FirstCoordinate[1] - '1', i_FirstCoordinate[0] - 'A'].IsRevealed = false;
            GameBoard[i_SecondCoordinate[1] - '1', i_SecondCoordinate[0] - 'A'].IsRevealed = false;
        }
      
        public bool IsAMatch(string i_FirstCoordinate, string i_SecondCoordinate)
        {
            bool Match = false; 

            if (i_FirstCoordinate == null || i_SecondCoordinate == null)
            {
               Match = false;
            }         
            else if (GameBoard[i_FirstCoordinate[1] - '1', i_FirstCoordinate[0] - 'A'].Value == GameBoard[i_SecondCoordinate[1] - '1', i_SecondCoordinate[0] - 'A'].Value)
            {
                Match = true;
            }       

            return Match;
        }

        public bool IsRevealValid(string i_Coordinate)
        {
            if (GameBoard[i_Coordinate[1] - '1', i_Coordinate[0] - 'A'].IsRevealed)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public void RevealSquare(string i_Coordinate,eGameStatus i_Status)
        {
            int rowCoordinate = i_Coordinate[1] - '1';
            int colCoordinate = i_Coordinate[0] - 'A';

            GameBoard[rowCoordinate, colCoordinate].BelongsTo = i_Status;
            GameBoard[rowCoordinate, colCoordinate].IsRevealed = true;
        }

        public void AdvanceScore(eGameStatus i_GameStatus)
        {
            if (i_GameStatus == eGameStatus.FirstPlayerTurn)
            {
                m_FirstPlayer.PlayerScore++;
            }
            else
            {
                m_SecondPlayer.PlayerScore++;
            }
        }

        public void SetAILogic(int i_Difficulty)
        {
            m_AiLogic = new AILogic(i_Difficulty);
        }

        public string CheckIfSquareValueInMemory(string i_FirstCoordiante)
        {
            string secondCoordinate;

            m_AiLogic.SearchValue(i_FirstCoordiante, getCoordianteValue(i_FirstCoordiante), out secondCoordinate);
            if (secondCoordinate == null)
            {
                secondCoordinate = RandomizeCoordinate();
            }

            return secondCoordinate;
        }

        public void AddPlayerCoordinateToAI(string i_FirstCoordinate, string i_SecondCoordiante)
        {
            m_AiLogic.AddCoordinate(new Coordinate(i_FirstCoordinate, getCoordianteValue(i_FirstCoordinate),0));
            m_AiLogic.AddCoordinate(new Coordinate(i_SecondCoordiante, getCoordianteValue(i_SecondCoordiante),0));
        }

        public bool ExistsMemorizedPair(out string i_FirstCoordinate, out string i_SecondCoordinate)
        {
            bool returnedValue = false;

            if (m_AiLogic.CheckForMemorizedPair(out i_FirstCoordinate, out i_SecondCoordinate) == true)
            {
                returnedValue = true;
            }

            return returnedValue;
        }

        public void DeletePairFromAIMemeory(string i_FirstCoordiante)
        {
            m_AiLogic.DeletePair(i_FirstCoordiante);
        }
      
        internal Player FirstPlayer
        {
            get
            {
                return m_FirstPlayer;
            }
            set
            {
                m_FirstPlayer = value;
            }
        }
        
        internal eGameMode GameMode
        {
            get
            {
                return r_GameMode;
            }
        }
        
        internal Player SeconndPlayer
        {
            get
            {
                return m_SecondPlayer;
            }
            set
            {
                m_SecondPlayer = value;
            }
        }
        
        internal eGameStatus GameStatus
        {
            get
            {
                return m_GameStatus;
            }

            set
            {
                m_GameStatus = value;
            }

        }
       
        internal bool IsGameOver()
        {
            if (m_FirstPlayer.PlayerScore + m_SecondPlayer.PlayerScore == (m_GameBoardNumOfColumns * m_GameBoardNumOfRows) / 2)
            {
                m_GameStatus = eGameStatus.GameOver;
                return true;
            }
            else
            {
                return false;
            }
        }
        
        internal Coordinate[,] GameBoard
        {
            get
            {
                return m_GameBoard;
            }

            set
            {
                m_GameBoard = value;
            }
        }
        
        private Coordinate[,] generateTable()
        {
            int tempRow=0;
            int tempCol=0;
            int partnerCoordinate =0;
            int[] usedLetters = new int[m_GameBoardNumOfColumns * m_GameBoardNumOfRows / 2];

            GameBoard = new Coordinate[m_GameBoardNumOfRows, m_GameBoardNumOfColumns];
            constractCoordinates();
            for (int i = 0; i < usedLetters.Length; i++)
            {
                usedLetters[i] = 1;
                for (int j = 0; j < 2; j++)
                {
                    partnerCoordinate = tempCol + (tempRow * 10);
                    randomizeCoordiante(out tempRow, out tempCol);
                    GameBoard[tempRow, tempCol].BuildCoordinate(string.Format("{0}{1}", (char)('A' + tempCol), (char)('1' + tempRow)), (char)(65 + i),0);
                }
                GameBoard[tempRow, tempCol].PartnerCordinate = partnerCoordinate;
                GameBoard[partnerCoordinate/10, partnerCoordinate%10].PartnerCordinate = tempCol + (tempRow * 10);

            }

            return GameBoard;
        }
        
        private void randomizeCoordiante(out int io_TempRow, out int io_TempCol)
        {
            io_TempRow = m_Random.Next(0, m_GameBoardNumOfRows);
            io_TempCol = m_Random.Next(0, m_GameBoardNumOfColumns);
           

            while (GameBoard[io_TempRow, io_TempCol].IsRevealed == true)
            {
                io_TempRow = m_Random.Next(0, m_GameBoardNumOfRows);
                io_TempCol = m_Random.Next(0, m_GameBoardNumOfColumns);
            }

        }
        
        private void constractCoordinates()
        {
            for (int i = 0; i < GameBoardNumOfRows; i++)
            {

                for (int j = 0; j < GameBoardNumOfColumns; j++)
                {
                    GameBoard[i, j] = new Coordinate();

                }
            }
        }
        
        private StringBuilder getBorderString()
        {
            StringBuilder rowBorder = new StringBuilder();

            rowBorder.Append("   ");
            for (int i = 0; i < m_GameBoardNumOfColumns; i++)
            {
                rowBorder.Append("====");
            }

            rowBorder.Append("=");

            return rowBorder;
        }
        
        private void hideAllSquares()
        {
            for (int i = 0; i < m_GameBoardNumOfRows; i++)
            {
                for (int j = 0; j < m_GameBoardNumOfColumns; j++)
                {
                    GameBoard[i, j].IsRevealed = false;
                }

            }

        }

        private char getCoordianteValue(string i_Coordiante)
        {
            char returnedChar;

            returnedChar = GameBoard[i_Coordiante[1] - '1', i_Coordiante[0] - 'A'].Value;

            return returnedChar;
        }
    }
}
