using System;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Ace_Reg
{
    /// <summary>
    /// Interaction logic for InsertEvent.xaml
    /// </summary>
    public partial class InsertEvent : Window
    {
        private readonly string dbConString = @"Data Source=Events.db;Version=3;Password=simonLikesApples;";

        SQLiteConnection sqLite; string Query, tableNames;
        private string selectedTable;
        string ID = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 8);


        public InsertEvent()
        {
            InitializeComponent();
            fillCombo();            
        }

        #region Insert
        private void subButton_Click(object sender, RoutedEventArgs e)
        {
            SQLiteConnection sqLite = new SQLiteConnection(dbConString);

            if (prizeBox.Text.Equals(null) || nameBox.Text.Equals(null) || courseBox.Equals(null) || rollBox.Equals(null) || semBox.Equals(null) || collBox.Equals(null))
                MessageBox.Show("Fill all the details");
            else
            {

                try
                {
                    sqLite.Open();
                    string Query = "INSERT INTO '" + selectedTable + "'(EID, Name, RollNo, College, Course, Semester_Section, Prize) values('" + ID + "', '" + this.nameBox.Text + "', '" + rollBox.Text + "',  '" + collBox.Text + "',  '" + courseBox.Text + "',  '" + semBox.Text + "', '" + prizeBox.Text + "' )";
                    SQLiteCommand createCommand = new SQLiteCommand(Query, sqLite);
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

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
        #endregion
    }
}
