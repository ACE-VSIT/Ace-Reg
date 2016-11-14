using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace Ace_Reg
{
    /// <summary>
    /// Interaction logic for EventApprovalStatus.xaml
    /// </summary>
    public partial class EventApprovalStatus : Window
    {
        private readonly string dbConString = @"Data Source=Events.db;Version=3;Password=simonLikesApples;";

        SQLiteConnection sqLite;

        string apTable, Query, tableNames;

        public EventApprovalStatus()
        {
            InitializeComponent();
            fillCombo();
        }


        #region Nav & Exit
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void goBack_Click(object sender, RoutedEventArgs e)
        {
            EventsDB ev = new EventsDB();
            this.Hide();
            ev.Show();
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
                    if (tableNames.EndsWith("_approval"))
                        approvalBox.Items.Add(tableNames);
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
        #endregion        

        private void approvalBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            apTable = approvalBox.SelectedValue as string;
            sqLite = new SQLiteConnection(dbConString);

            try
            {
                sqLite.Open();
                Query = "SELECT * FROM '" + apTable + "' Order by Name";
                buttonHelp();
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

        private void personSelected_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            personSelected.Text = "";
        }

        #region Helper Method
        private void buttonHelp()
        {
            SQLiteCommand createCommand = new SQLiteCommand(Query, sqLite);
            createCommand.ExecuteNonQuery();

            SQLiteDataAdapter Adapt = new SQLiteDataAdapter(createCommand);
            DataTable data = new DataTable(apTable);
            Adapt.Fill(data);
            approvalGrid.ItemsSource = data.DefaultView;
            Adapt.Update(data);
        }
        #endregion

        private void updateData_Click(object sender, RoutedEventArgs e)
        {


            sqLite = new SQLiteConnection(dbConString);

            
            if (MessageBox.Show("Update Status?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                MessageBox.Show("Updation Process Canceled");
            }

            else
            {                

                try
                {
                    sqLite.Open();
                   
                    Query = "UPDATE '" + apTable + "'SET Status = '" + statusCheck.IsChecked.ToString() + "' WHERE EID='" + personSelected.Text + "' OR NAME = '" + personSelected.Text + "' ";
                    SQLiteCommand createCommand = new SQLiteCommand(Query, sqLite);
                    createCommand.ExecuteNonQuery();


                    Query = "UPDATE '" + apTable + "' SET SeatNo = '" + seatNum.Text + "' WHERE EID='" + personSelected.Text + "' OR NAME = '" + personSelected.Text + "' ";
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
                    EventsDB e1 = new EventsDB();
                    sqLite.Close();
                    this.Hide();
                    e1.Show();
                }
            }

        }
    }
}
