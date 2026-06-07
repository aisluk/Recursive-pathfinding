using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;


namespace L1_LD24
{
    public partial class InOutUtils : System.Web.UI.Page
    {
        /// <summary>
        /// reads the provided .txt data file, saves the city's height, width and character components
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static char[,] ReadFromTxt(string filePath, out int n, out int m)
        {
            string[] allLines = File.ReadAllLines(filePath);
            char[] punctuation = { ' ', '\t' }; // excluded into a char array so StringSplitOptions would work
            string[] citySize = allLines[0].Split(punctuation, StringSplitOptions.RemoveEmptyEntries);
            n = int.Parse(citySize[0]);
            m = int.Parse(citySize[1]);
            allLines = allLines.Skip(1).ToArray(); // skips first line

            if (n < 2 || m < 2) {// if the provided city length or width are too small                           
                return null;
            }
            else {
                char[,] City = new char[m, n]; // [X,Y]
                int YAmount = 0;
                foreach (string line in allLines) {
                    char[] lineSymbols = line.TrimEnd(' ').ToCharArray();
                    AddLineSymbols(City, m, YAmount, lineSymbols);
                    YAmount++;
                }
                return City;
            }
        }

        /// <summary>
        /// uses recursion to save each symbol from a read line to a two-dimensional array
        /// </summary>
        /// <param name="City"></param>
        /// <param name="lineNo"></param>
        /// <param name="columnNo"></param>
        /// <param name="lineSymbols"></param>
        private static void AddLineSymbols(char[,] City, int X, int Y, char[] lineSymbols)
        {
            if (X < 1) {
                return;
            }
            else {
                City[(X - 1), Y] = lineSymbols[X - 1];          // because index starts at 0
                AddLineSymbols(City, (X - 1), Y, lineSymbols);  // to add symbol from a previous column
            }
        }

        /// <summary>
        /// prints the contents of initial file to the webpage, using a table
        /// </summary>
        /// <param name="City"></param>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <param name="fileName"></param>
        public static void PrintInitialToWebpage(char[,] City, int n, int m, Table Table1, string errorMessage)
        {
            if (City.GetLength(0) < 2 || City.GetLength(1) < 0 || City.GetLength(1) > 10) {
                TableCell cell = new TableCell {
                    Text = errorMessage
                };
                TableRow row = new TableRow();
                row.Cells.Add(cell);
                Table1.Rows.Add(row);
            }
            else {
                TableCell cell = new TableCell {
                    Text = $"{n} {m}"
                };
                TableRow row = new TableRow();
                row.Cells.Add(cell);
                Table1.Rows.Add(row);

                for (int i = 0; i < n; i++) {
                    TableCell cell2 = new TableCell();
                    TableRow row2 = new TableRow();
                    string tableLine = "";
                    PrepareInitialLine(City, 0, i, m, ref tableLine);
                    cell2.Text = tableLine;
                    row2.Cells.Add(cell2);
                    Table1.Rows.Add(row2);
                }
            }
        }

        /// <summary>
        /// prints results to webpage, using a table
        /// </summary>
        /// <param name="Table2"></param>
        /// <param name="Friends"></param>
        /// <param name="MeetingLocation"></param>
        /// <param name="Pizzeria"></param>
        /// <param name="DistanceSum"></param>
        public static void PrintResultsToWebpage(Table Table2, List<Coordinates> Friends, Coordinates MeetingLocation, Coordinates Pizzeria, int distanceSum)
        {
            foreach (Coordinates friend in Friends) {
                TableCell cell = new TableCell();
                TableRow row = new TableRow();
                cell.Text = friend.ToString();
                row.Cells.Add(cell);
                Table2.Rows.Add(row);
            }
            TableCell cell2 = new TableCell();
            TableRow row2 = new TableRow();
            if (MeetingLocation == null || Pizzeria == null || distanceSum < 2) {
                cell2.Text = "Neįmanoma";
                row2.Cells.Add(cell2);
                Table2.Rows.Add(row2);
            }
            else {
                cell2.Text = $"Susitikimo vieta: {MeetingLocation.ToString()}";
                row2.Cells.Add(cell2);
                Table2.Rows.Add(row2);

                TableCell cell3 = new TableCell();
                TableRow row3 = new TableRow();
                cell3.Text = $"Picerija: {Pizzeria.ToString()}";
                row3.Cells.Add(cell3);
                Table2.Rows.Add(row3);

                TableCell cell4 = new TableCell();
                TableRow row4 = new TableRow();
                cell4.Text = $"Nueita: {distanceSum}";
                row4.Cells.Add(cell4);
                Table2.Rows.Add(row4);
            }
        }

        /// <summary>
        /// uses recursion to prepare to print each city symbol in a line
        /// </summary>
        /// <param name="City"></param>
        /// <param name="lineNo"></param>
        /// <param name="columnNo"></param>
        /// <param name="tableLine"></param>
        private static void PrepareInitialLine(char[,] City, int X, int Y, int XEnd, ref string tableLine)
        {
            if (X >= XEnd) {
                return;
            }
            else {
                tableLine += (City[X, Y].ToString());
                PrepareInitialLine(City, (X + 1), Y, XEnd, ref tableLine);
            }
        }    

        /// <summary>
        /// prints the contents of initial file to a .txt file
        /// </summary>
        /// <param name="City"></param>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <param name="fileName"></param>
        public static void PrintInitialToTxt(char[,] City, int n, int m, string filePath, string errorMessage)
        {
            using (StreamWriter writer = new StreamWriter(filePath, true)) {// allows appending            
                if (City.GetLength(0) < 2 || City.GetLength(1) < 0 || City.GetLength(1) > 10) {                
                    writer.WriteLine(errorMessage);
                    writer.Close();
                }
                else {
                    for (int i = 0; i < n; i++) {
                        string line = "";
                        PrepareInitialLine(City, 0, i, m, ref line);
                        writer.WriteLine(line);
                    }
                    writer.Close();
                }
            }
        }

        /// <summary>
        /// prints the results to a .txt file
        /// </summary>
        /// <param name="meeting"></param>
        /// <param name="pizzeria"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="errorMessage"></param>
        public static void PrintResultsToTxt(List<Coordinates> friends, Coordinates meeting, Coordinates pizzeria, int path, string filePath, string errorMessage)
        {
            using (StreamWriter writer = new StreamWriter(filePath, true)) { // allows appending
                writer.WriteLine();
                for (int i = 0; i < friends.Count; i++) {
                    writer.WriteLine(friends[i]);
                }

                if (path < 1 || meeting == null || pizzeria == null) {
                    writer.WriteLine(errorMessage);
                    writer.Close();
                }
                else {
                    for (int i = 0; i < friends.Count; i++) {
                        writer.WriteLine(friends[i]);
                    }
                    writer.WriteLine($"Susitikikimo vieta: {meeting}");
                    writer.WriteLine($"Picerija: {pizzeria}");
                    writer.WriteLine($"Nueita: {path}");
                    writer.Close();
                }
            }
        }
    }
}