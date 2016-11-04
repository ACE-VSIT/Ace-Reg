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
        private readonly string dbConString = @"Data Source=Events.db;Version=3;Password=simonLikesApples;";

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
                    bool result = testIfExists(tableNames);                    
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
        private bool testIfExists(string tableName)
        {
            sqLite = new SQLiteConnection(dbConString);

            try
            {

                sqLite.Open();
                Query = "SELECT * FROM '" + tableNames + "' WHERE EID='" + checkName.Text + "' OR Name='" + checkName.Text + "' ORDER BY Name";
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
                    MessageBox.Show("Record Selected: " + testID);
                    testID = null;
                    x = 0;
                    eventsParticipated.Items.Add(tableNames);
                }
                                
                sqLite.Close();
            }

            if (x >= 1)
                return true;
            else
                return false;
        }
        #endregion

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
