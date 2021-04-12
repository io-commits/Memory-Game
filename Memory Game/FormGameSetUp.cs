using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Memory_Game
{
    public partial class GameSetUp : Form
    {

        public GameSetUp()
        {
            InitializeComponent();
        }

        private void OpponentButton_Click(object sender, EventArgs e)
        {
            changeOpponent();            
        }

        private void changeOpponent()
        {
            if(SecondPlayerTextBox.Enabled)
            {
                OpponentButton.Text = "Against a Friend";
                SecondPlayerTextBox.Text = "- computer -";
                SecondPlayerTextBox.Enabled = false;
                
            }
            else
            {
                OpponentButton.Text = "Against a Computer";
                SecondPlayerTextBox.Enabled = true;
                SecondPlayerTextBox.Text = "";
            }
        }

        public string FirstPlayerName
        {
            get
            {
                return FirstPlayerTextBox.Text;
            }
        }

        public string SecondlayerName
        {
            get
            {
                return SecondPlayerTextBox.Text;
            }
        }

        public bool GameMode
        {
            get
            {
                return SecondPlayerTextBox.Enabled;
            }
        }

        public string BoardSize
        {
            get
            {
                return BoardSizeButton.Text; 
            }
        }



        private void StartButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BoardSizeButton_Click(object sender, EventArgs e)
        {
            switch (BoardSizeButton.Text)
            {
                case "4 x 4":
                    BoardSizeButton.Text = "4 x 5";
                    break;
                case "4 x 5":
                    BoardSizeButton.Text = "4 x 6";
                    break;
                case "4 x 6":
                    BoardSizeButton.Text = "5 x 4";
                    break;
                case "5 x 4":
                    BoardSizeButton.Text = "5 x 6";
                    break;
                case "5 x 6":
                    BoardSizeButton.Text = "6 x 4";
                    break;
                case "6 x 4":
                    BoardSizeButton.Text = "6 x 5";
                    break;
                case "6 x 5":
                    BoardSizeButton.Text = "6 x 6";
                    break;
                case "6 x 6":
                    BoardSizeButton.Text = "4 x 4";
                    break;
                default:                   
                    break;
            }

        }
       

        
    }
}
