using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory_Game
{
    internal struct Player
    {
        private readonly string r_PlayerName;
        private uint m_PlayerScore;

        public Player(string i_PlayerName)
        {
            r_PlayerName = i_PlayerName;
            m_PlayerScore = 0;
        }

        public string PlayerName
        {
            get
            {
                return r_PlayerName;
            }
        }

        public uint PlayerScore
        {
            get
            {
                return m_PlayerScore;
            }

            set
            {
                m_PlayerScore = value;
            }

        }

    }
}
