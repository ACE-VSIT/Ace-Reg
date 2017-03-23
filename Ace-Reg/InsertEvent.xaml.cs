using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace Ace_Reg
{
    /// <summary>
    /// Interaction logic for InsertEvent.xaml
    /// </summary>
    public partial class InsertEvent : Window
    {
        private readonly string dbConString = @Constants.EVENTDB;

        SQLiteConnection sqLite; string Query, tableNames;
        private string selectedTable, approvalTable;
        string ID = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 5);
       
        public InsertEvent()
        {
            InitializeComponent();
            fillCombo();            
        }

        #region Insert
        private void subButton_Click(object sender, RoutedEventArgs e)
        {
            SQLiteConnection sqLite = new SQLiteConnection(dbConString);

            ID = selectedTable.Substring(0, 3).ToUpper() + "-" + ID.ToUpper();

            if (prizeBox.Text.Equals(null) || nameBox.Text.Equals(null) || courseBox.Equals(null) || rollBox.Equals(null) || semBox.Equals(null) || collBox.Equals(null))
                MessageBox.Show("Fill all the details");
            else
            {

                try
                {
                    approvalTable = selectedTable + "_approval";                    

                    sqLite.Open();
                    string Query = "INSERT INTO '" + selectedTable + "'(EID, Name, RollNo, College, Course, Semester_Section, Prize) values('" + ID + "', '" + this.nameBox.Text + "', '" + rollBox.Text + "',  '" + collBox.Text + "',  '" + courseBox.Text + "',  '" + semBox.Text + "', '" + prizeBox.Text + "' )";
                    SQLiteCommand createCommand = new SQLiteCommand(Query, sqLite);
                    createCommand.ExecuteNonQuery();

                    Query = "INSERT INTO '" + approvalTable + "'(EID, Name, Prize) values('" + ID + "', '" + this.nameBox.Text + "', '" + prizeBox.Text + "')";
                    createCommand = new SQLiteCommand(Query, sqLite);
                    createCommand.ExecuteNonQuery();

                    MessageBox.Show("New Record Inserted");
                }
                catch (Exception exception)
                {

                    MessageBox.Show(exception.Message);
                }

                finally
                {
                    EventsDB ev = new EventsDB();
                    sqLite.Close();
                    this.Hide();
                    ev.Show();
                }
            }

        }
        #endregion
                
        #region Fill Combo 
        private void fillCombo()
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
                    if ( ! (tableNames.EndsWith("_approval")) )
                        selectEvent.Items.Add(tableNames);                    
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

            prizeBox.Items.Add("First");
            prizeBox.Items.Add("Second");
            prizeBox.Items.Add("Third");
            prizeBox.Items.Add("Event Head");
            prizeBox.Items.Add("Volunteer");
            prizeBox.Items.Add("Coordinator");
            prizeBox.Items.Add("Participation");
        }

        private void selectEvent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTable = selectEvent.SelectedValue as string;
        }

        #endregion

        #region Nav-Exit
        private void goBack_Click(object sender, RoutedEventArgs e)
        {
            EventsDB ev = new EventsDB();
            this.Hide();
            ev.Show();
        }


        private void importCSV_Click(object sender, RoutedEventArgs e)
        {
            ImportCSV im = new ImportCSV();
            this.Hide();
            im.Show();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
        #endregion
    }
}
