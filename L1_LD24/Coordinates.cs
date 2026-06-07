using System;

namespace L1_LD24
{
    /// <summary>
    /// class for saving first friend's, meeting location's or pizzeria's coordinates
    /// </summary>
    public class Coordinates : IComparable<Coordinates>
    {
        public int X { get; set; }
        public int Y { get; set; }

        /// <summary>
        /// full constructor
        /// </summary>
        /// <param name="startingY"></param>
        /// <param name="startingX"></param>
        public Coordinates(int startingX, int startingY)
        {
            this.Y = startingY;
            this.X = startingX;
        }

        /// <summary>
        /// empty constructor that adds first logically impossible value (-1) to be replaced later
        /// </summary>
        public Coordinates()
        {
            this.Y = -1;
            this.X = -1;
        }

        public override string ToString()
        {
            string line = $"{X} {Y}";
            return line;
        }

        /// <summary>
        /// gives line index
        /// </summary>
        /// <returns></returns>
        public int GetLine() => Y; // return Y        

        /// <summary>
        /// gives column index
        /// </summary>
        /// <returns></returns>
        public int GetColumn() => X;

        public override bool Equals(object obj)
        {
            if (obj is Coordinates other)
                return Y == other.Y && X == other.X;
            else
                return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Y.GetHashCode();
                hash = hash * 23 + X.GetHashCode();
                return hash;
            }
        }

        public int CompareTo(Coordinates other)
        {
            if (this.Y == other.Y)
                return this.X.CompareTo(other.X);
            else
                return this.Y.CompareTo(other.Y);
        }
    }
}