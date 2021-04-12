using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory_Game
{
    internal class Coordinate
    {
        private  string m_Coordiante;
        private  char m_Value;          
        private bool m_IsRevealed;
        private int m_PartnerCordinate;
        private eGameStatus m_BelongsTo;
        public event Action<Coordinate> WasChanged;       

        public Coordinate(string i_CoordinateString, char i_CoordianteChar, bool i_IsRevealed)
        {
            m_Coordiante = i_CoordinateString;
            m_Value = i_CoordianteChar;
            m_IsRevealed = i_IsRevealed;
            m_PartnerCordinate = 0;
        }

        public Coordinate(string i_CoordinateString, char i_CoordianteChar,int i_PartnerCoordinate)
        {
            m_Coordiante = i_CoordinateString;
            m_Value = i_CoordianteChar;
            m_IsRevealed = true;
            m_PartnerCordinate = i_PartnerCoordinate;
        }

        public void BuildCoordinate(string i_CoordinateString, char i_CoordianteChar, int i_PartnerCoordinate)
        {
            m_Coordiante = i_CoordinateString;
            m_Value = i_CoordianteChar;
            m_IsRevealed = true;
            m_PartnerCordinate = i_PartnerCoordinate;
        }

        public Coordinate()
        {
            m_IsRevealed = false;
        }

        public string Coordiante
        {
            get
            {
                return m_Coordiante;
            }
        }

        public char Value
        {
            get
            {
                return m_Value;
            }
        }

        public bool IsRevealed
        {
            get
            {
                return m_IsRevealed;
            }

            set
            {               
                m_IsRevealed = value;
                OnChange();
            }

        }

        public int PartnerCordinate
        {
            get
            {
                return m_PartnerCordinate;
            }

            set
            {
                m_PartnerCordinate = value;
            }
        }

        public eGameStatus BelongsTo
        {
            get
            {
                return m_BelongsTo;
            }

            set
            {
                m_BelongsTo = value;
            }
        }

        protected virtual void OnChange()
        {
            if (WasChanged != null)
            {
                WasChanged.Invoke(this);
            }
        }
    }
}
