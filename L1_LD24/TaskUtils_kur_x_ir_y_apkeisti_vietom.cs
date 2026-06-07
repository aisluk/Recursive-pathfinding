using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
// dvimaciuose masyvuose PIRMA EILUTES, PO TO STULPELIAI.  y row, x column
// GetLength(0) eilutes y
// GetLength(1) stulpeliai x

namespace L1_LD24
{
    public partial class TaskUtils : System.Web.UI.Page
    {
        /// <summary>
        /// finds all friend coordinates
        /// </summary>
        /// <param name="City"></param>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static List<Coordinates> FindFriends(char[,] City, int n, int m)
        {
            List<Coordinates> Friends = LocateSpecificChars(City, n, m, 'D');
            return Friends;
        }

        /// <summary>
        /// finds all possible meeting locations
        /// </summary>
        /// <param name="City"></param>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        private static List<Coordinates> FindMeetingLocation(char[,] City, int n, int m)
        {
            List<Coordinates> Meetings = LocateSpecificChars(City, n, m, 'S');

            return Meetings; // returns default (negative) value or found meeting locations
        }

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
                    if (City[i, j].Equals(character))
                    {
                        //Coordinates coord = new Coordinates(i, j);
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
                Changed.Add(ChangeCoordinateToResult(n, m, Coordinates[i]));
            }
            return Changed;
        }



        /// <summary>
        /// redos one array coordinates format to print coordinates format
        /// </summary>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public static Coordinates ChangeCoordinateToResult(int n, int m, Coordinates coordinate)
        {
            Coordinates changed = new Coordinates();
            int resultsLine = 1;
            for (int i = n - 1; i >= 0; i--) // goes up based on y axis
            {
                int resultsColumn = 1;
                for (int j = 0; j < m; j++) // goes right based on x axis
                {
                    //if (coordinate.GetColumn() == i && coordinate.GetLine() == j)
                    if (coordinate.GetLine() == i && coordinate.GetColumn() == j)
                    {
                        changed = new Coordinates(resultsLine, resultsColumn);
                        return changed;
                    }
                    resultsColumn++;
                }
                resultsLine++;
            }
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
        private static void Visit(char[,] city, Queue<Tuple<Coordinates, int>> queue, int y, int x, int n, int m, bool[,] visited, int path)
        {
            //if (x >= 0 && y >= 0 && x < n && y < m && !visited[x, y] && city[x, y] != 'X' && city[x, y] != 'P') // if valid coordinate for path
            if (x >= 0 && y >= 0 && x < n && y < m) // if valid coordinate index
            {
                //if (!visited[y, x] && city[y, x] != 'X' && city[y, x] != 'P') // if coordinate is passable and not yet visited
                //if (!visited[y, x] && (city[y, x].Equals('.') || city[y, x].Equals('S') || city[y, x].Equals('D'))) // if coordinate is passable and not yet visited
                if (!visited[y, x] && (city[y, x] == '.' || city[y, x] == 'S' || city[y, x] == 'D' || city[y, x] == 'P'))
                {
                    queue.Enqueue(Tuple.Create(new Coordinates(y, x), path)); // adds current coordinates to queue, since it's valid to keep the search going
                                                                              // adds current coordinates to queue, since it's valid to keep the search goings

                    //queue.Enqueue(Tuple.Create(new Coordinates(x, y), path));
                    //visited[x, y] = true;
                    visited[y, x] = true;
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
        public static int BFSPaths(char[,] city, int n, int m, out Coordinates chosenMeeting, out Coordinates chosenPizzeria)
        {
            List<Coordinates> meetings = FindMeetingLocation(city, n, m);
            List<Coordinates> allFriends = FindFriends(city, n, m);

            List<int> allValidPaths = new List<int>();
            int minPath = int.MaxValue;
            chosenMeeting = chosenPizzeria = null;

            foreach (Coordinates meeting in meetings) // checks if each meeting location can be reached by all friends
            {
                foreach (Coordinates friend in allFriends)
                {
                    BFSMultiple(city, friend, n, m, 'D', out List<int> paths);
                    if (paths.Count > 0)
                    {
                        allValidPaths.Add(paths.Min());
                    }
                    else
                    {
                        allValidPaths.Clear();
                        break;
                    }

                    /*
                     List<Coordinates> tempFriends = BFS(city, meeting, n, m, 'D', out int paths);
                    if (tempFriends.Count == allFriends.Count)
                    {
                        allValidPaths.Add(paths);
                    }
                    */
                }

                if (allValidPaths.Count == 0) // ends search if no meeting locations are valid
                {
                    return -1;
                }
            }

            foreach (Coordinates meeting in meetings) // checks if a pizzeria can be reached from each meeting location
            {
                List<Coordinates> tempPizzerias = BFSMultiple(city, meeting, n, m, 'P', out List<int> paths);
                if (tempPizzerias.Count == 0) // if no reachable pizzerias were found, skip the meeting location
                {
                    continue;
                }
                else
                { // picks out found pizzeria with shortest path to meeting location
                    for (int i = 0; i < tempPizzerias.Count; i++)
                    {
                        //int totalPath = allValidPaths[i] * 2 + paths[i] * 2;
                        //int totalPath = allValidPaths.Sum() * 2 + paths[i] * 2;
                        int totalPath = (allValidPaths.Sum() + paths[i]) * allFriends.Count;
                        if (totalPath < minPath)
                        {
                            minPath = totalPath;                // total path is double because each friend goes to the
                            chosenMeeting = meeting;            // pizzeria and then back to their original position
                            chosenPizzeria = tempPizzerias[i];
                        }
                    }
                }
            }
            return minPath;

        }

        /// <summary>
        /// Returns one path length from the provided start point. Uses the Breath-First Search algorithm
        /// </summary>
        /// <param name="city"></param>
        /// <param name="start"></param>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <param name="goal"></param>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static List<Coordinates> BFS(char[,] city, Coordinates start, int n, int m, char goal, out int paths)
        {
            List<Coordinates> allGoals = new List<Coordinates>();
            var queue = new Queue<Tuple<Coordinates, int>>();
            bool[,] visited = new bool[n, m];
            paths = 0;

            queue.Enqueue(Tuple.Create(start, 0));
            visited[start.GetLine(), start.GetColumn()] = true;
            //visited[start.GetColumn(), start.GetLine()] = true;

            while (queue.Count > 0) // if queue has elements, go through them
            {
                var (current, path) = queue.Dequeue(); // gives object at the start of the queue and removes it from the queue

                //if (city[current.GetColumn(), current.GetLine()].Equals(goal)) // if goal is reached, return the coordinates
                if (city[current.GetLine(), current.GetColumn()] == goal) // if goal is reached, return the coordinates
                {
                    allGoals.Add(current);
                    //?
                    paths = path;
                    break;
                }

                Visit(city, queue, current.GetLine() - 1, current.GetColumn(), n, m, visited, path + 1);
                Visit(city, queue, current.GetLine() + 1, current.GetColumn(), n, m, visited, path + 1);
                Visit(city, queue, current.GetLine(), current.GetColumn() - 1, n, m, visited, path + 1);
                Visit(city, queue, current.GetLine(), current.GetColumn() + 1, n, m, visited, path + 1);
                /*
                Visit(city, queue, current.GetColumn() - 1, current.GetLine(), n, m, visited, path + 1); // checks left // kai 21 turetu pridet 20 // kai 23 prideda 31 // kai 12 prisideda 20
                Visit(city, queue, current.GetColumn() + 1, current.GetLine(), n, m, visited, path + 1); // checks right // kai 32 prisideda 24
                Visit(city, queue, current.GetColumn(), current.GetLine() - 1, n, m, visited, path + 1); // checks up // kai 2 1 pideda 02 o ne 20 // kai 02 prisideda 10
                Visit(city, queue, current.GetColumn(), current.GetLine() + 1, n, m, visited, path + 1); // checks down
                */
                // kai current 2 3 prideda i queue 3 1 kuris yra X ir nzn kodel taip daro
            }
            return allGoals;
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
        public static List<Coordinates> BFSMultiple(char[,] city, Coordinates start, int n, int m, char goal, out List<int> allPaths)
        {
            List<Coordinates> allGoals = new List<Coordinates>();
            allPaths = new List<int>();
            var queue = new Queue<Tuple<Coordinates, int>>();
            bool[,] visited = new bool[n, m];

            queue.Enqueue(Tuple.Create(start, 0));
            visited[start.GetLine(), start.GetColumn()] = true;
            //visited[start.GetColumn(), start.GetLine()] = true;

            while (queue.Count > 0)
            {
                var (current, path) = queue.Dequeue(); // gives object at the start of the queue and removes it from the queue

                if (city[current.GetLine(), current.GetColumn()] == goal) // if goal is reached, return
                                                                          //if (city[current.GetColumn(), current.GetLine()] == goal) // if goal is reached, return
                {
                    allGoals.Add(current);
                    allPaths.Add(path);
                }

                Visit(city, queue, current.GetLine() - 1, current.GetColumn(), n, m, visited, path + 1);
                Visit(city, queue, current.GetLine() + 1, current.GetColumn(), n, m, visited, path + 1);
                Visit(city, queue, current.GetLine(), current.GetColumn() - 1, n, m, visited, path + 1);
                Visit(city, queue, current.GetLine(), current.GetColumn() + 1, n, m, visited, path + 1);
                /*
                Visit(city, queue, current.GetColumn() - 1, current.GetLine(), n, m, visited, path + 1);
                Visit(city, queue, current.GetColumn() + 1, current.GetLine(), n, m, visited, path + 1);
                Visit(city, queue, current.GetColumn(), current.GetLine() - 1, n, m, visited, path + 1);
                Visit(city, queue, current.GetColumn(), current.GetLine() + 1, n, m, visited, path + 1);
                */
            }
            return allGoals;
        }
    }
}