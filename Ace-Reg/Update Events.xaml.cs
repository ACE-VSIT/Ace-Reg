using System;
using System.Windows;
using System.Data.SQLite;
using System.Windows.Controls;

namespace Ace_Reg
{
    /// <summary>
    /// Interaction logic for Update_Events.xaml
    /// </summary>
    public partial class Update_Events : Window
    {
        private readonly string dbConString = @"Data Source=Events.db;Version=3;Password=simonLikesApples;";

        SQLiteConnection sqLite; string Query, tableNames;
        private string selectedTable, apTable, testID; int x;


        public Update_Events()
        {
            InitializeComponent();
            fillCombo();
        }

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
                    if (!(tableNames.EndsWith("_approval")))
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

        #region Selection
        private void SButton_Click(object sender, RoutedEventArgs e)
        {
            sqLite = new SQLiteConnection(dbConString);

            try
            {

                sqLite.Open();
                Query = "SELECT * FROM '" + selectedTable + "' WHERE EID='" + SBox.Text + "' OR Name='" + SBox.Text + "' ORDER BY Name";
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
                }

                else
                    MessageBox.Show("No such record");

                sqLite.Close();
            }
        }
        #endregion

        #region Update
        private void subButton_Click(object sender, RoutedEventArgs e)
        {
            sqLite = new SQLiteConnection(dbConString);

            if (prizeBox.Text.Equals(null) || nameBox.Text.Equals(null) || courseBox.Equals(null) || rollBox.Equals(null) || semBox.Equals(null) || collBox.Equals(null))
                MessageBox.Show("Fill all the details");

            else
            {
                if (MessageBox.Show("Update Prize?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    MessageBox.Show("Updation Process Canceled");
                }

                else
                {

                    apTable = selectedTable + "_approval";

                    try
                    {
                        sqLite.Open();
                        Query = "UPDATE '" + selectedTable + "' SET Prize = '" + prizeBox.Text + "' WHERE EID='" + this.SBox.Text + "' OR NAME = '" + this.SBox.Text + "'";
                        SQLiteCommand createCommand = new SQLiteCommand(Query, sqLite);
                        createCommand.ExecuteNonQuery();

                        Query = "UPDATE '" + apTable + "' SET Prize = '" + prizeBox.Text + "' WHERE EID='" + this.SBox.Text + "' OR NAME = '" + this.SBox.Text + "'";
                        createCommand = new SQLiteCommand(Query, sqLite);
                        createCommand.ExecuteNonQuery();

                        MessageBox.Show("Record Updated");
                    }
                    catch (Exception exception)
                    {

                        MessageBox.Show(exception.Message);
                    }

                    finally
                    {
                        ShowEvents sh = new ShowEvents();
                        sqLite.Close();
                        this.Hide();
                        sh.Show();                        
                    }
                }
            }

        }

        private void upCol_Click(object sender, RoutedEventArgs e)
        {
           Query = "UPDATE '" + selectedTable + "' SET College = '" + collBox.Text + "' WHERE EID='" + this.SBox.Text + "' OR NAME = '" + this.SBox.Text + "'";

            upGen();
        }

        private void upSection_Click(object sender, RoutedEventArgs e)
        {
            Query = "UPDATE '" + selectedTable + "' SET Semester_Section = '" + semBox.Text + "' WHERE EID='" + this.SBox.Text + "' OR NAME = '" + this.SBox.Text + "'";

            upGen();
        }

        private void upCourse_Click(object sender, RoutedEventArgs e)
        {
            Query = "UPDATE '" + selectedTable + "' SET Course = '" + courseBox.Text + "'  WHERE EID='" + this.SBox.Text + "' OR NAME = '" + this.SBox.Text + "'";

            upGen();
        }

        private void upRoll_Click(object sender, RoutedEventArgs e)
        {
            Query = "UPDATE '" + selectedTable + "' SET RollNo = '" + rollBox.Text + "' WHERE EID='" + this.SBox.Text + "' OR NAME = '" + this.SBox.Text + "'";

            upGen();
        }

        private void upName_Click(object sender, RoutedEventArgs e)
        {

            apTable = selectedTable + "_approval";

            Query = "UPDATE '" + selectedTable + "' SET Name = '" + nameBox.Text + "' WHERE EID='" + this.SBox.Text + "' OR NAME = '" + this.SBox.Text + "'";

            upGen();

            Query = "UPDATE '" + apTable + "' SET Name = '" + nameBox.Text + "' WHERE EID='" + this.SBox.Text + "' OR NAME = '" + this.SBox.Text + "'";

            upGen();
        }

        private void upAll_Click(object sender, RoutedEventArgs e)
        {
            sqLite = new SQLiteConnection(dbConString);

            if (prizeBox.Text.Equals(null) || nameBox.Text.Equals(null) || courseBox.Equals(null) || rollBox.Equals(null) || semBox.Equals(null) || collBox.Equals(null))
                MessageBox.Show("Fill all the details");

            else
            {
                if (MessageBox.Show("Update All Columns?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    MessageBox.Show("Updation Process Canceled");
                }

                else
                {
                    try
                    {
                        sqLite.Open();
                        Query = "UPDATE '" + selectedTable + "' SET Name = '" + nameBox.Text + "', Course = '" + courseBox.Text + "', Semester_Section = '" + semBox.Text + "', College = '" + collBox.Text + "' ,  Prize = '" + prizeBox.Text + "', RollNo = '" + rollBox.Text + "' WHERE EID='" + this.SBox.Text + "' OR NAME = '" + this.SBox.Text + "'";
                        SQLiteCommand createCommand = new SQLiteCommand(Query, sqLite);
                        createCommand.ExecuteNonQuery();
                        MessageBox.Show("Record Updated");
                    }
                    catch (Exception exception)
                    {

                        MessageBox.Show(exception.Message);
                    }

                    finally
                    {
                        ShowEvents sh = new ShowEvents();
                        sqLite.Close();
                        this.Hide();
                        sh.Show();
                    }
                }
            }
        }
        #endregion               

        #region Nav-Exit
        private void goBack_Click(object sender, RoutedEventArgs e)
        {
            ShowEvents sh = new ShowEvents();
            this.Hide();
            sh.Show();
        }
      
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
        #endregion

        #region Generic Update
        private void upGen()
        {
            sqLite = new SQLiteConnection(dbConString);


            if (MessageBox.Show("Update Selected Column?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                MessageBox.Show("Updation Process Canceled");
            }

            else
            {                

                try
                {
                    sqLite.Open();

                    SQLiteCommand createCommand = new SQLiteCommand(Query, sqLite);
                    createCommand.ExecuteNonQuery();
                    MessageBox.Show("Record Updated");
                }

                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }

                finally
                {
                    ShowEvents sh = new ShowEvents();
                    sqLite.Close();
                    this.Hide();
                    sh.Show();
                }
            }            

        }
        #endregion

    }
}
