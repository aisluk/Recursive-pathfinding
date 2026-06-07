// OP2 L1 LD_24 susitikimas. By Aistė Lukošiūtė from IFF-3/10
using System;
using System.Collections.Generic;
using System.Web;
using System.IO;

namespace L1_LD24
{
    public partial class Forma1 : System.Web.UI.Page
    {
        const string resultsFile = "U3rez.txt";

        protected void Page_Load(object sender, EventArgs e)
        {
            //Button2_Click(sender, e);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (File.Exists(HttpContext.Current.Server.MapPath($"App_Data/{resultsFile}")))
                File.Delete(HttpContext.Current.Server.MapPath($"App_Data/{resultsFile}"));

            DropDownList1_SelectedIndexChanged(sender, e, out string dataFile);

            if (!File.Exists(HttpContext.Current.Server.MapPath($"App_Data/{dataFile}")))
            {
                Label4.Text = "• Šis duomenų failas neegzistuoja.";
                Label4.Visible = true;
            }
            else
            {
                char[,] CityCoordinates = InOutUtils.ReadFromTxt(HttpContext.Current.Server.MapPath($"App_Data/{dataFile}"), out int n, out int m);
                if (CityCoordinates == null) // if there was an error in the file and coordinates didn't exist or didn't fit the requirements
                {
                    Label4.Text = "• Šiame duomenų faile trūksta duomenų.";
                    Label4.Visible = true;
                    File.AppendAllText(HttpContext.Current.Server.MapPath($"App_Data/{resultsFile}"), "Buvo pateiktas netaisyklingas duomenų failas.");
                }
                else
                {
                    Label3.Visible = true; // makes fields and label for initial data and results visible
                    Label2.Visible = true;
                    Table1.Visible = true;
                    Table2.Visible = true;
                    InOutUtils.PrintInitialToWebpage(CityCoordinates, n, m, Table1, "Duomenų filas tuščias arba yra neteisingo formato.");
                    InOutUtils.PrintInitialToTxt(CityCoordinates, n, m, HttpContext.Current.Server.MapPath($"App_Data/{resultsFile}"), "Duomenų filas tuščias arba neteisingo formato.");

                    int distance = TaskUtils.BFSPaths(CityCoordinates, n, m, out List<Coordinates> Friends, out Coordinates chosenMeeting, out Coordinates chosenPizzeria);

                    List<Coordinates> FriendsChanged = TaskUtils.ChangeCoordinatesToResult(n, m, Friends); // makes coordinates fit the printing requirements
                    FriendsChanged.Sort(); // sorts friends in coordinate ascending order

                    if (chosenPizzeria != null)
                    {
                        chosenMeeting = TaskUtils.ChangeCoordinateToResult(n, m, chosenMeeting);
                        chosenPizzeria = TaskUtils.ChangeCoordinateToResult(n, m, chosenPizzeria);
                    }
                    InOutUtils.PrintResultsToTxt(FriendsChanged, chosenMeeting, chosenPizzeria, distance, HttpContext.Current.Server.MapPath($"App_Data/{resultsFile}"), "Neįmanoma");
                    InOutUtils.PrintResultsToWebpage(Table2, FriendsChanged, chosenMeeting, chosenPizzeria, distance);
                }
            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        { 
            DropDownList1_SelectedIndexChanged(sender, e, out string dataFile);
        }
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e, out string dataFile)
        {
            dataFile = DropDownList1.SelectedValue;
        }

        /// <summary>
        /// clears results from webpage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button2_Click(object sender, EventArgs e)
        {
            if (File.Exists(resultsFile))
                File.Delete(resultsFile);
            Table1.Rows.Clear();
            Table2.Rows.Clear();
            Table2.Visible = false;
            Label3.Visible = false;
            Label4.Visible = false;
            DropDownList1.ClearSelection();
        }
    }
}