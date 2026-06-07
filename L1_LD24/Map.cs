using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L1_LD24
{
    //public class Coordinates : IComparable<Coordinates>
    //{
    //    public int LineNo { get; set; }
    //    public int ColumnNo { get; set; }

    //    public Coordinates(int startingLine, int startingColumn)
    //    {
    //        this.LineNo = startingLine;
    //        this.ColumnNo = startingColumn;
    //    }

    //    public Coordinates()
    //    {
    //        this.LineNo = -1;
    //        this.ColumnNo = -1;
    //    }

    //    public override string ToString()
    //    {
    //        return $"{LineNo} {ColumnNo}";
    //    }

    //    public bool IsPossibleCoordinate()
    //    {
    //        return LineNo > -1 && ColumnNo > -1;
    //    }

    //    public bool IsPossibleCoordinate(int column, int line)
    //    {
    //        return LineNo > -1 && ColumnNo > -1 && LineNo < line && ColumnNo < column;
    //    }

    //    public bool IsPossibleCoordinate(Coordinates other)
    //    {
    //        return LineNo > -1 && ColumnNo > -1 && LineNo <= other.LineNo && ColumnNo <= other.ColumnNo;
    //    }

    //    public int GetLine()
    //    {
    //        return LineNo;
    //    }

    //    public int GetColumn()
    //    {
    //        return ColumnNo;
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        if (obj is Coordinates)
    //            return this.Equals(obj as Coordinates);
    //        else
    //            return false;
    //    }

    //    public override int GetHashCode()
    //    {
    //        return LineNo.GetHashCode() ^ ColumnNo.GetHashCode();
    //    }

    //    public int CompareTo(Coordinates other)
    //    {
    //        if (this.LineNo == other.LineNo)
    //            return this.ColumnNo.CompareTo(other.ColumnNo);
    //        else
    //            return this.LineNo.CompareTo(other.LineNo);
    //    }
    //}

    public class Map
    {
        private static void Visit(char[,] city, Queue<Tuple<Coordinates, int>> queue, int y, int x, int n, int m, bool[,] visited, int path)
        {
            if (x >= 0 && y >= 0 && x < n && y < m)
            {
                if (!visited[y, x] && (city[y, x] == '.' || city[y, x] == 'S' || city[y, x] == 'D' || city[y, x] == 'P'))
                {
                    queue.Enqueue(Tuple.Create(new Coordinates(y, x), path));
                    visited[y, x] = true;
                }
            }
        }

        public static int BFSPaths(char[,] city, int n, int m, out Coordinates chosenMeeting, out Coordinates chosenPizzeria)
        {
            /*
            List<Coordinates> meetings = new List<Coordinates>
            {
                new Coordinates(2, 2),
                new Coordinates(4, 2)
            };

            List<Coordinates> allFriends = new List<Coordinates>
            {
                new Coordinates(0, 0),
                new Coordinates(2, 0),
                new Coordinates(4, 0)
            };
            */

            List<Coordinates> meetings = TaskUtils.FindMeetingLocation(city, n, m);
            List<Coordinates> allFriends = TaskUtils.FindFriends(city, n, m);


            List<int> allValidPaths = new List<int>();
            int minPath = int.MaxValue;
            chosenMeeting = chosenPizzeria = null;

            foreach (Coordinates meeting in meetings)
            {
                List<int> friendPaths = new List<int>();
                foreach (Coordinates friend in allFriends)
                {
                    List<int> paths;
                    BFS(city, friend, n, m, 'D', out paths);
                    if (paths.Count > 0)
                    {
                        friendPaths.Add(paths.Min());
                    }
                    else
                    {
                        friendPaths.Clear();
                        break;
                    }
                }

                if (friendPaths.Count == allFriends.Count)
                {
                    allValidPaths.Add(friendPaths.Sum());
                }
            }

            if (allValidPaths.Count == 0)
            {
                return -1;
            }

            foreach (Coordinates meeting in meetings)
            {
                List<int> friendPaths = new List<int>();
                foreach (Coordinates friend in allFriends)
                {
                    List<int> paths;
                    BFS(city, friend, n, m, 'P', out paths);
                    if (paths.Count > 0)
                    {
                        friendPaths.Add(paths.Min());
                    }
                    else
                    {
                        friendPaths.Clear();
                        break;
                    }
                }

                if (friendPaths.Count == allFriends.Count)
                {
                    List<Coordinates> tempPizzerias = BFS(city, meeting, n, m, 'P', out List<int> paths);
                    if (tempPizzerias.Count == 0)
                    {
                        continue;
                    }
                    else
                    {
                        for (int i = 0; i < tempPizzerias.Count; i++)
                        {
                            //int totalPath = friendPaths.Sum() * 2 + paths[i] * 2;
                            int totalPath = friendPaths.Sum() * 2 + paths[i] * 2 * allFriends.Count;
                            if (totalPath < minPath)
                            {
                                minPath = totalPath;
                                chosenMeeting = meeting;
                                chosenPizzeria = tempPizzerias[i];
                            }
                        }
                    }
                }
            }

            return minPath;
        }

        public static List<Coordinates> BFS(char[,] city, Coordinates start, int n, int m, char goal, out List<int> allPaths)
        {
            List<Coordinates> allGoals = new List<Coordinates>();
            allPaths = new List<int>();
            var queue = new Queue<Tuple<Coordinates, int>>();
            bool[,] visited = new bool[n, m];

            queue.Enqueue(Tuple.Create(start, 0));
            visited[start.GetLine(), start.GetColumn()] = true;

            while (queue.Count > 0)
            {
                var (current, path) = queue.Dequeue();

                if (city[current.GetLine(), current.GetColumn()] == goal)
                {
                    allGoals.Add(current);
                    allPaths.Add(path);
                }

                Visit(city, queue, current.GetLine() - 1, current.GetColumn(), n, m, visited, path + 1);
                Visit(city, queue, current.GetLine() + 1, current.GetColumn(), n, m, visited, path + 1);
                Visit(city, queue, current.GetLine(), current.GetColumn() - 1, n, m, visited, path + 1);
                Visit(city, queue, current.GetLine(), current.GetColumn() + 1, n, m, visited, path + 1);
            }
            return allGoals;
        }

        public static void TestMethods()
        {
            int n = 5;
            int m = 5;

            Coordinates start = new Coordinates(2, 2);
            char goal = 'D';
            char[,] city = new char[,]
            {
                { 'D', '.', '.', '.', 'P' },
                { 'X', 'X', '.', '.', '.' },
                { 'D', '.', 'S', '.', 'P' },
                { 'X', 'X', '.', '.', '.' },
                { 'D', '.', 'S', '.', '.' }
            };

            int resultsPath = BFSPaths(city, n, m, out Coordinates chosenMeeting, out Coordinates chosenPizzeria);
        }
    }
}

/*
expected values are
resultsPath = 32
chosenMeeting = (2,2)
chosenPizzeria = (2,4)
*/