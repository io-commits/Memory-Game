using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory_Game
{
    internal class AILogic
    {
        private readonly int r_DifficultyLevel;
        private readonly List<Coordinate> r_Coordinates; 

        public AILogic(int i_GameDifficulty)
        {
            r_DifficultyLevel = i_GameDifficulty;
            r_Coordinates = new List<Coordinate>(r_DifficultyLevel);
        }

        public void AddCoordinate(Coordinate i_Coordinate)
        {
            bool additionValid = true;
            foreach (Coordinate coordinate in r_Coordinates)
            {
                if (coordinate.Coordiante.Equals(i_Coordinate.Coordiante))
                {
                    additionValid = false;
                    break;
                }
            }

            if (additionValid == true)
            {
                r_Coordinates.Insert(0, i_Coordinate);
            }

            if (r_Coordinates.Count > r_DifficultyLevel)
            {
                r_Coordinates.RemoveAt(r_DifficultyLevel);
            }    
            
        }

        public bool SearchValue(string i_GivenCoordinate, char i_Value, out string io_Coordiante)
        {
            bool returnValue = false;
            io_Coordiante = null;
            foreach (var cord in r_Coordinates)
            {
                if (cord.Value.Equals(i_Value) && cord.Coordiante.Equals(i_GivenCoordinate) == false)
                {
                    io_Coordiante = cord.Coordiante;
                    returnValue = true;
                }
            }

            return returnValue;
        }
        
        public bool CheckForMemorizedPair(out string i_FirstCoordinate, out string i_SecondCoordiante)
        {
            bool returnedValue = false;
            bool foundFlag = false;
            i_FirstCoordinate = null;
            i_SecondCoordiante = null;
            foreach (Coordinate coordinate in r_Coordinates)
            {
                i_FirstCoordinate = coordinate.Coordiante;
                foreach (Coordinate innerCoordinate in r_Coordinates)
                {
                    if (innerCoordinate.Value == coordinate.Value && i_FirstCoordinate.Equals(innerCoordinate.Coordiante) == false)
                    {
                        i_SecondCoordiante = innerCoordinate.Coordiante;
                        returnedValue = true;                       
                        foundFlag = true;
                        break;
                    }  
                    
                }

                if (foundFlag == true)
                {
                    break;
                }

            }

            return returnedValue;
        }
    
        public void DeletePair(string i_FirstCoordinate)
        {
            Coordinate first = new Coordinate();
            Coordinate second = new Coordinate();
            foreach (Coordinate firstCoordinateIterator in r_Coordinates)
            {
                if (firstCoordinateIterator.Coordiante == i_FirstCoordinate)
                {
                    first = firstCoordinateIterator;
                }

            }
            foreach (Coordinate secondCoordinateIterator in r_Coordinates)
            {
                if (first.Coordiante != secondCoordinateIterator.Coordiante && first.Value == secondCoordinateIterator.Value)
                {
                    second = secondCoordinateIterator;
                }

            }
            r_Coordinates.Remove(first);
            r_Coordinates.Remove(second);
        }
    
    }
}
