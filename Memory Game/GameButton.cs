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
    internal class GameButton : Button
    {
      
        private System.Windows.Forms.PictureBox m_Picture;
        private Coordinate m_Coordinate;
        private bool m_Visit = false;

        public PictureBox Picture
        {
            get
            {
                return m_Picture;
            }

            set
            {
                m_Picture = value;
            }
        }

        public GameButton(Coordinate i_Cordinate)
             : base()
        {
            m_Coordinate = i_Cordinate; 
            m_Coordinate.WasChanged += Coordinate_WasChanged;
            m_Picture = new System.Windows.Forms.PictureBox();
            m_Picture.Size = new System.Drawing.Size(85, 85);
            m_Picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            m_Picture.WaitOnLoad = true;
        }

        public bool Visit
        {
            get
            {
                return m_Visit;
            }

            set
            {
                m_Visit = value;
            }
        }

        internal Coordinate Coordinate
        {
            get
            {
                return m_Coordinate;
            }           
        }

        protected override void OnClick(EventArgs e)
        {            
            Coordinate.IsRevealed = true;
            base.OnClick(e);
        }

        private void Coordinate_WasChanged(Coordinate i_Coordinate)
        {
            if(i_Coordinate.BelongsTo == eGameStatus.FirstPlayerTurn)
            {
                Picture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            }
            else if(i_Coordinate.BelongsTo == eGameStatus.SecondPlayerTurn)
            {
                Picture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            }

            this.Visible = !Coordinate.IsRevealed;
        }
   
    }

}
