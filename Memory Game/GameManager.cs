using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Memory_Game
{
    public class GameManager : Form
    {
        private GameButton[,] m_gameButtons;       
        private System.Windows.Forms.Label m_CurrentPlayerLabel;
        private System.Windows.Forms.Label m_FirstPlayerLabel;
        private System.Windows.Forms.Label m_SecondPlayerLabel;
        private System.Windows.Forms.Timer m_RevealTimerPlayer = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer m_RevealTimerCpu = new System.Windows.Forms.Timer();
        private string m_CpuFirstCoordinate;
        private string m_CpuSecondCoordinate;
    
        internal GameLogic m_game;        
        internal GameButton m_firstPick;
        internal GameButton m_secondPick;

        public string CpuFirstCoordinate
        {
            get
            {
                return m_CpuFirstCoordinate;
            }

            set
            {
                m_CpuFirstCoordinate = value;
            }
        }

        public string CpuSecondCoordinate
        {
            get
            {
                return m_CpuSecondCoordinate;
            }

            set
            {
                m_CpuSecondCoordinate = value;
            }
        }

        public System.Windows.Forms.Timer RevealTimerPlayer
        {
            get
            {
                return m_RevealTimerPlayer;
            }

            set
            {
                m_RevealTimerPlayer = value;
            }
        }

        public System.Windows.Forms.Timer RevealTimerCpu
        {
            get
            {
                return m_RevealTimerCpu;
            }

            set
            {
                m_RevealTimerCpu = value;
            }
        }

        public GameManager()
        {
            GameSetUp preGame = new GameSetUp();
            eGameMode gameMode;
            RevealTimerPlayer.Interval = 10;
            RevealTimerCpu.Interval = 10;
            RevealTimerPlayer.Tick += revealTimerPlayer_Tick;
            RevealTimerCpu.Tick += revealTimerCpu_Tick;
            preGame.ShowDialog();

            if (preGame.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                gameMode = setGameMode(preGame);
                m_game = setGame(preGame.FirstPlayerName, preGame.SecondlayerName, gameMode, ParseRow(preGame.BoardSize), ParseCol(preGame.BoardSize));
          
            }
                    
            InitializeBoard();

        }
        
        internal GameButton FirstPick
        {
            get
            {
                return m_firstPick;
            }

            set
            {
                m_firstPick = value;
            }
        }
        
        internal GameButton SecondPick
        {
            get
            {
                return m_secondPick;
            }

            set
            {
                m_secondPick = value;
            }
        }
        
        internal GameButton[,] GameButtons
        {
            get
            {
                return m_gameButtons;
            }

            set
            {
                m_gameButtons = value;
            }
        }

        private void revealTimerCpu_Tick(object sender, EventArgs e)
        {
           
            m_game.HidePair(CpuFirstCoordinate, CpuSecondCoordinate);
            m_game.GameStatus = eGameStatus.FirstPlayerTurn;
            playerColorUpdate();           
            Thread.Sleep(4000);
            RevealTimerCpu.Stop();
        }

        private void revealTimerPlayer_Tick(object sender, EventArgs e)
        {
            makeTurn();
           
            resetPicks();
            RevealTimerPlayer.Stop();
        }

        private GameLogic setGame(string i_firstPlayerName, string i_secondlayerName, eGameMode i_gameMode, int i_rows, int i_cols)
        {
            GameLogic game = new GameLogic(i_firstPlayerName, i_secondlayerName, i_gameMode);
            game.SetGameTable(i_rows, i_cols);
           
            if(i_gameMode == eGameMode.Solo)
            {
                game.SetAILogic(5);
            }
            return game;

        }

        private void InitializeBoard()
        {
            int RowButtonLocation = 10;
            int ColButtonLocation = 0;

            InitializeLabels();
            m_CurrentPlayerLabel.Text = "Current Player: "+ m_game.FirstPlayer.PlayerName; 
            m_FirstPlayerLabel.Text = m_game.FirstPlayer.PlayerName+' '+0;
            m_SecondPlayerLabel.Text = m_game.SeconndPlayer.PlayerName+' '+0;

            this.GameButtons = new GameButton[m_game.GameBoardNumOfRows, m_game.GameBoardNumOfColumns];

            for (int i = 0; i < m_game.GameBoardNumOfRows; i++)
            {
                ColButtonLocation = 10;

                for (int j = 0; j < m_game.GameBoardNumOfColumns; j++)
                {
                    GameButtons[i, j] = new GameButton(m_game.GameBoard[i, j]);
                    GameButtons[i, j].Size = new System.Drawing.Size(85, 85);
                    GameButtons[i, j].Location = new System.Drawing.Point(RowButtonLocation, ColButtonLocation);
                    GameButtons[i, j].Picture.Location = new System.Drawing.Point(RowButtonLocation, ColButtonLocation);
                    
                    GameButtons[i, j].TabStop = false;
                    this.Controls.Add(GameButtons[i, j]);
                    this.Controls.Add(GameButtons[i, j].Picture);

                    ColButtonLocation += 90;
                }
                RowButtonLocation += 90;
            }


            m_CurrentPlayerLabel.Location = new System.Drawing.Point(10, ColButtonLocation + 10);
            m_FirstPlayerLabel.Location = new System.Drawing.Point(10, ColButtonLocation + 35);
            m_SecondPlayerLabel.Location = new System.Drawing.Point(10, ColButtonLocation + 55);

            this.Text = "Memory Game";
            this.ClientSize = new System.Drawing.Size(RowButtonLocation + 4, m_SecondPlayerLabel.Location.Y + 26);
            this.Controls.Add(m_CurrentPlayerLabel);
            this.Controls.Add(m_FirstPlayerLabel);
            this.Controls.Add(m_SecondPlayerLabel);

            setPictures();        
            listenClicks();
            gameStart();
        }

        private void gameStart()
        {
           
            m_game.GameStatus = eGameStatus.FirstPlayerTurn;
            this.ShowDialog();
            if(m_game.GameStatus == eGameStatus.GameOver)
            {
                FormGameOver Gameover = new FormGameOver(m_game);
                Gameover.ShowDialog();
                if (Gameover.DialogResult == DialogResult.Retry)
                {
                    this.Controls.Clear();
                    m_game.SetGameTable(m_game.GameBoardNumOfRows, m_game.GameBoardNumOfColumns);
                    InitializeBoard();
                }
            }
           
        }

        private void makeTurn()
        {
                       
            playerTurn(); 
                       
            if (m_game.GameMode == eGameMode.Solo && m_game.GameStatus == eGameStatus.SecondPlayerTurn)
            {
                cpuTurn();
            }

        }

        private void listenClicks()
        {
            foreach (GameButton Button in GameButtons)
            {
                Button.Click += gameButton_Click;
            }
        }

        private void gameButton_Click(object i_sender, EventArgs e)
        {
            GameButton button = i_sender as GameButton;           
            if (FirstPick == null)
            {
                FirstPick = button;
                if (m_game.GameStatus == eGameStatus.FirstPlayerTurn)
                {                   
                    FirstPick.Picture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));                   
                }
                else
                {                  
                    FirstPick.Picture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));                   
                }
            }
            else if (SecondPick == null)
            {              
                SecondPick = button;
                
                if (m_game.GameStatus == eGameStatus.FirstPlayerTurn)
                {
                    SecondPick.Picture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
                }
                else
                {
                    SecondPick.Picture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
                }
                 RevealTimerPlayer.Start();
               
            }

        }
     
        private void setPictures()
        {
            Random random = new Random();
            int pictureID = random.Next(0,100);
            for (int i = 0; i< m_game.GameBoardNumOfRows; i++)
            {
                for(int j = 0; j< m_game.GameBoardNumOfColumns; j++)
                {
                    if (GameButtons[i, j].Visit == false)
                    {
                        GameButtons[i, j].Picture.ImageLocation = ("https://picsum.photos/id/" + pictureID + "/80");
                        GameButtons[i, j].Visit = true;

                        GameButtons[GameButtons[i,j].Coordinate.PartnerCordinate/10, GameButtons[i, j].Coordinate.PartnerCordinate % 10].Picture.ImageLocation = ("https://picsum.photos/id/" + pictureID + "/80");
                        GameButtons[GameButtons[i, j].Coordinate.PartnerCordinate / 10, GameButtons[i, j].Coordinate.PartnerCordinate % 10].Visit = true;

                        pictureID += 10;
                    }

                }

            }

        }

        private void InitializeLabels()
        {
            m_CurrentPlayerLabel = new System.Windows.Forms.Label();
            m_FirstPlayerLabel = new System.Windows.Forms.Label();
            m_SecondPlayerLabel = new System.Windows.Forms.Label();

            m_CurrentPlayerLabel.AutoSize = true;
            m_FirstPlayerLabel.AutoSize = true;
            m_SecondPlayerLabel.AutoSize = true; 

            m_FirstPlayerLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            m_CurrentPlayerLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            m_SecondPlayerLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
        }

        private void resetPicks()
        {
            FirstPick = null;
            SecondPick = null;
        }

        private void playerTurn()
        {
            Player player = m_game.GameStatus == eGameStatus.FirstPlayerTurn ? m_game.FirstPlayer : m_game.SeconndPlayer;
            if (m_game.IsAMatch(FirstPick.Coordinate.Coordiante, SecondPick.Coordinate.Coordiante))
            {
                if (m_game.GameMode == eGameMode.Solo)
                {
                    m_game.DeletePairFromAIMemeory(FirstPick.Coordinate.Coordiante);
                }
               

                m_game.AdvanceScore(m_game.GameStatus);
                if (m_game.GameStatus == eGameStatus.FirstPlayerTurn)
                {                    
                    m_FirstPlayerLabel.Text = player.PlayerName +' '+m_game.FirstPlayer.PlayerScore;
                 
                }
                else
                {
                    m_SecondPlayerLabel.Text = player.PlayerName + ' ' + m_game.SeconndPlayer.PlayerScore;     
                                  
                }
                if (m_game.IsGameOver())
                {
                    this.Close();
                }

            }
            else
            {

                if (m_game.GameMode == eGameMode.Solo)
                {
                    m_game.AddPlayerCoordinateToAI(FirstPick.Coordinate.Coordiante, SecondPick.Coordinate.Coordiante);
                }

                m_game.GameStatus = player.Equals(m_game.FirstPlayer) ? eGameStatus.SecondPlayerTurn : eGameStatus.FirstPlayerTurn;
                playerColorUpdate();
                Thread.Sleep(2000);                
                m_game.HidePair(FirstPick.Coordinate.Coordiante, SecondPick.Coordinate.Coordiante);
                
            }

        }

        private void playerColorUpdate()
        {
            if (m_game.GameStatus == eGameStatus.FirstPlayerTurn)
            {
                m_CurrentPlayerLabel.Text = "Current Player: " + m_game.FirstPlayer.PlayerName;
                m_CurrentPlayerLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            }
            else
            {
                m_CurrentPlayerLabel.Text = "Current Player: " + m_game.SeconndPlayer.PlayerName;
                m_CurrentPlayerLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            }

        }

        private void cpuTurn()
        {
           
            bool KeepCpuPlaying = true;

            playerColorUpdate();
            while (KeepCpuPlaying == true)
            {
                KeepCpuPlaying = findAMatchingPair();
            }
        }

        private bool findAMatchingPair()
        {
            bool Match = false;

            if (m_game.ExistsMemorizedPair(out m_CpuFirstCoordinate, out m_CpuSecondCoordinate) == true)
            {
                m_game.RevealSquare(CpuFirstCoordinate, m_game.GameStatus);
                m_game.RevealSquare(CpuSecondCoordinate, m_game.GameStatus);
            }
            else
            {
                CpuFirstCoordinate = m_game.RandomizeCoordinate();
                m_game.RevealSquare(CpuFirstCoordinate, m_game.GameStatus);
                CpuSecondCoordinate = m_game.CheckIfSquareValueInMemory(CpuFirstCoordinate);
                m_game.RevealSquare(CpuSecondCoordinate, m_game.GameStatus);
            }

            if (m_game.IsAMatch(CpuFirstCoordinate, CpuSecondCoordinate))
            {
                m_game.DeletePairFromAIMemeory(CpuFirstCoordinate);
                m_game.AdvanceScore(m_game.GameStatus);
                m_SecondPlayerLabel.Text = m_game.SeconndPlayer.PlayerName + ' ' + m_game.SeconndPlayer.PlayerScore;
                Match = true;
                if (m_game.IsGameOver())
                {
                    this.Close();
                    Match = false;
                }              
            }
            else
            {
               Match = false;
               RevealTimerCpu.Start();
            }

            return Match;
        }

        private int ParseCol(string boardSize)
        {
            return (int)char.GetNumericValue(boardSize[4]);

        }

        private int ParseRow(string boardSize)
        {
            return (int)char.GetNumericValue(boardSize[0]);
        }

        private eGameMode setGameMode(GameSetUp i_preGame)
        {
            eGameMode gameMode;
            if (i_preGame.GameMode == true)
            {
                gameMode = eGameMode.Duo;
            }
            else
            {
                gameMode = eGameMode.Solo;
            }
            return gameMode;
        }
    }
}
