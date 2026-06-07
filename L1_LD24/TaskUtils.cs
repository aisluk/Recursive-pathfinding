using System;
using System.Collections.Generic;
using System.Linq;

namespace L1_LD24
{
    public partial class TaskUtils : System.Web.UI.Page
    {
        /// <summary>
        /// finds all coordinates of a specific character in the City two-dimensional array
        /// </summary>
        /// <param name="City"></param>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        private static List<Coordinates> LocateSpecificChars(char[,] City, int n, int m, char character)
        {
            List<Coordinates> Chars = new List<Coordinates>();
            for (int i = 0; i < n; i++) // y axis
            {
                for (int j = 0; j < m; j++) // x axis
                {
                    if (City[j, i].Equals(character))
                    {                       
                        Coordinates coord = new Coordinates(j, i);
                        Chars.Add(coord);
                    }
                }
            }
            return Chars;
        }

        /// <summary>
        /// redos the array coordinates format to print coordinates format (they start at index 1 from bottom left corner)
        /// </summary>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        public static List<Coordinates> ChangeCoordinatesToResult(int n, int m, List<Coordinates> Coordinates)
        {
            List<Coordinates> Changed = new List<Coordinates>();
            for (int i = 0; i < Coordinates.Count; i++)
            {
                Changed.Add(ChangeCoordinateToResult(m, n, Coordinates[i]));
            }
            return Changed;
        }

        /// <summary>
        /// redos one array's coordinates format to print coordinates format
        /// </summary>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public static Coordinates ChangeCoordinateToResult(int m, int n, Coordinates coordinate)
        {
            int resultsColumn = coordinate.GetColumn() + 1;
            int resultsLine = n - coordinate.GetLine();
            Coordinates changed = new Coordinates(resultsColumn, resultsLine);
            return changed;
        }

        /// <summary>
        /// checks if the given coordinate is valid and has been visited
        /// </summary>
        /// <param name="city"></param>
        /// <param name="queue"></param>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <param name="visited"></param>
        /// <param name="path"></param>
        private static void Visit(char[,] city, Queue<Tuple<Coordinates, int>> queue, int x, int y, int n, int m, bool[,] visited, int path)
        {
            if (x >= 0 && y >= 0 && y < n && x < m)
            {
                if (!visited[x, y] && (city[x, y] == '.' || city[x, y] == 'S' || city[x, y] == 'D' || city[x, y] == 'P')) // if passable tile
                {
                    queue.Enqueue(Tuple.Create(new Coordinates(x, y), path)); // adds current coordinates to queue, since it's valid to keep the search going
                    visited[x, y] = true;
                }
            }
        }

        /// <summary>
        /// calculates distances, finds most efficient path for each friend and returns their distance's sum
        /// </summary>
        /// <param name="city"></param>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <param name="chosenMeeting"></param>
        /// <param name="chosenPizzeria"></param>
        /// <returns></returns>
        public static int BFSPaths(char[,] city, int n, int m, out List<Coordinates> allFriends, out Coordinates chosenMeeting, out Coordinates chosenPizzeria)
        {
            List<Coordinates> meetings = LocateSpecificChars(city, n, m, 'S');
            allFriends = LocateSpecificChars(city, n, m, 'D');
            List<Coordinates> allPizzerias = LocateSpecificChars(city, n, m, 'P');

            List<Coordinates> validMeetings = new List<Coordinates>();
            List<int> validMeetingPaths = new List<int>();

            List<Coordinates> validPizzerias = new List<Coordinates>();
            List<int> validPizzeriaPaths = new List<int>();
            List<Coordinates> pizzeriaMeetings = new List<Coordinates>();
                        
            int minPath = int.MaxValue;
            chosenMeeting = chosenPizzeria = null;

            foreach (Coordinates meeting in meetings) // checks if each meeting location can be reached by all friends
            {
                int currentPathSum = 0;
                int validFriendsCount = 0;
                
                foreach (Coordinates friend in allFriends)
                {
                    List<int> paths = BFSMultiple(city, friend, n, m, meeting);
                    if (paths.Count > 0)
                    {
                        currentPathSum += paths.Min();
                        validFriendsCount++;
                    }
                    else
                    {                        
                        break;
                    }
                }

                if (validFriendsCount != allFriends.Count) // if the meeting location isn't fully reachable, ends search
                {
                    return -1;
                }
                else
                {
                    validMeetingPaths.Add(currentPathSum);
                    validMeetings.Add(meeting);
                }
            }

            foreach (Coordinates pizzeria in allPizzerias) // checks if a pizzeria can be reached from each meeting location
            {
                int bestPath = int.MaxValue;
                Coordinates bestMeeting = new Coordinates();
                foreach (Coordinates meeting in validMeetings)
                {
                    List<int> tempPaths = BFSMultiple(city, pizzeria, n, m, meeting);
                    if (tempPaths.Count > 0)
                    {
                        if (bestPath > tempPaths.Min())
                        {
                            bestPath = tempPaths.Min();
                            bestMeeting = meeting;
                        }
                    }
                }

                if (bestPath == int.MaxValue) // if the pizzeria location isn't reachable, ends search
                {
                    return -1;
                }
                else
                {                    
                    validPizzeriaPaths.Add(bestPath);
                    validPizzerias.Add(pizzeria);
                    pizzeriaMeetings.Add(bestMeeting);
                }                
            }

            for (int i = 0; i < validPizzerias.Count; i++) // picks out shortest overall path
            {
                int thisTotalPath = validPizzeriaPaths[i] * allFriends.Count(); // friends go to the pizzeria
                Coordinates thisMeeting = new Coordinates();

                for (int j = 0; j < allFriends.Count; j++) // friends go back to their original position on their own
                {
                    List<int> paths = BFSMultiple(city, allFriends[j], n, m, validPizzerias[i]);
                    thisTotalPath += paths.Min();
                }
                for (int j = 0; j < validMeetings.Count; j++)
                {
                    if (validMeetings[j] == pizzeriaMeetings[i])
                    {
                        thisTotalPath += validMeetingPaths[j]; // friends go to the meeting location
                        thisMeeting = validMeetings[j];
                    }
                }
                if (minPath > thisTotalPath)
                {
                    minPath = thisTotalPath;
                    chosenMeeting = thisMeeting;
                    chosenPizzeria = validPizzerias[i];
                }
            }

            return minPath;

        }

        /// <summary>
        /// Returns all possible path lengths from the provided start point. Uses the Breath-First Search algorithm
        /// </summary>
        /// <param name="city"></param>
        /// <param name="start"></param>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <param name="goal"></param>
        /// <param name="allPaths"></param>
        /// <returns></returns>
        private static List<int> BFSMultiple(char[,] city, Coordinates start, int n, int m, Coordinates goal)
        {
            List<int> allPaths = new List<int>();
            var queue = new Queue<Tuple<Coordinates, int>>();
            bool[,] visited = new bool[m, n];

            queue.Enqueue(Tuple.Create(start, 0));

            visited[start.GetColumn(), start.GetLine()] = true;

            while (queue.Count > 0)
            {
                var (current, path) = queue.Dequeue(); // gives object at the start of the queue and removes it from the queue

                if (current.Equals(goal))
                {
                    allPaths.Add(path);
                    visited[goal.GetColumn(), goal.GetLine()] = false; // resets the goal, so multiple paths could be found to it
                }                
                Visit(city, queue, current.GetColumn(), current.GetLine() - 1, n, m, visited, path + 1);
                Visit(city, queue, current.GetColumn(), current.GetLine() + 1, n, m, visited, path + 1);
                Visit(city, queue, current.GetColumn() - 1, current.GetLine(), n, m, visited, path + 1);
                Visit(city, queue, current.GetColumn() + 1, current.GetLine(), n, m, visited, path + 1);                
            }
            return allPaths;
        }
    }
}