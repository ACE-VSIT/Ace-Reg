using System;
using System.Windows;
using System.Data.SQLite;

namespace Ace_Reg
{
    /// <summary>
    /// Interaction logic for UserHistory.xaml
    /// </summary>
    public partial class UserHistory : Window
    {
        private readonly string dbConString = @"Data Source=events.db;Version=3;";

        SQLiteConnection sqLite;
        string Query, tableNames, testID;
        int x;

        public UserHistory()
        {
            InitializeComponent();
        }

        private void goBack_Click(object sender, RoutedEventArgs e)
        {
            EnhancedOptions EO = new EnhancedOptions();
            this.Hide();
            EO.Show();
        }

        private void checking_Click(object sender, RoutedEventArgs e)
        {
            eventsParticipated.Items.Clear();

            sqLite = new SQLiteConnection(dbConString);
            try
            {
                sqLite.Open();
                Query = "SELECT name FROM sqlite_master WHERE type='table';";
                SQLiteCommand createCommand = new SQLiteCommand(Query, sqLite);
                SQLiteDataReader reader = createCommand.ExecuteReader();
                while (reader.Read())
                {
                    tableNames = reader.GetString(0);
                    testIfExists(tableNames);
                }
            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }

            finally
            {
                sqLite.Close();
            }

        }

        #region Populate list
        private void testIfExists(string approveTable)
        {
            sqLite = new SQLiteConnection(dbConString);

            if(approveTable.EndsWith("_approval"))
            {
                try
                {
                    string truth = "True";
                    sqLite.Open();
                    Query = "SELECT * FROM '" + approveTable + "' WHERE EID='" + checkName.Text + "' OR Name='" + checkName.Text +
                        "' AND Status='" + truth + "' ORDER BY Name";
                    SQLiteCommand createCommand = new SQLiteCommand(Query, sqLite);
                    SQLiteDataReader dataReader = createCommand.ExecuteReader();
                    x = 0;
                    while (dataReader.Read())
                    {
                        testID = dataReader.GetString(0);
                        x++;
                    }

                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }

                finally
                {
                    if (x >= 1)
                    {
                        //MessageBox.Show("Record Selected: " + testID);
                        testID = null;
                        x = 0;

                        string[] historicData = approveTable.Split('_');

                        eventsParticipated.Items.Add(historicData[0]);
                    }

                    sqLite.Close();
                }
            }
        }
        #endregion

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
