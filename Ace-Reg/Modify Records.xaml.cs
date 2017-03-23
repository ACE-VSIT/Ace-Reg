using System;
using System.Data;
using System.Windows;
using System.Data.SQLite;


namespace Ace_Reg
{
    /// <summary>
    /// Interaction logic for Modify_Records.xaml
    /// </summary>
    public partial class Modify_Records : Window
    {
        private readonly string dbConString = @Constants.MEMDB;
        SQLiteConnection sqLite;   String Query;

        public Modify_Records()
        {
            InitializeComponent();
        }
   
        #region Search
        private void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            sqLite = new SQLiteConnection(dbConString);            

            try
            {
                sqLite.Open();
                sqLite.ChangePassword("simonLikesApples");
                Query = "SELECT * FROM MemberData WHERE  EnrolmentNo='" + SearchingBox.Text + "' OR Name='" + SearchingBox.Text + "' ORDER BY Name";
                buttonHelper();
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

        #region Delete 
        private void delAll_Click(object sender, RoutedEventArgs e)
        {
            sqLite = new SQLiteConnection(dbConString);

            if (MessageBox.Show("Delete all entries?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                MessageBox.Show("Deletion Process Canceled");                
            }

            else
            {
               
                try
                {
                    sqLite.Open();
                 //   sqLite.ChangePassword("simonLikesApples");
                    Query = "DELETE FROM MemberData";
                    buttonHelper();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }

                finally
                {
                    sqLite.Close();
                    MessageBox.Show("Entries Deleted");
                }
            }
            
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            sqLite = new SQLiteConnection(dbConString);

            if (MessageBox.Show("Delete selected entries?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                MessageBox.Show("Deletion Process Canceled");
            }

            else
            {
                try
                {
                    sqLite.Open();
                    //   sqLite.ChangePassword("simonLikesApples");


                    if (SearchingBox.Text.Equals(null) || SearchingBox.Text.Equals("") || SearchingBox.Text.Equals(" "))
                    {
                        var selected = recordsTable.SelectedItems;

                        foreach (var selectedRows in selected)
                        {
                            var rowView = (DataRowView)selectedRows;
                            Query = "DELETE FROM MemberData WHERE EnrolmentNo='" + rowView["EnrolmentNo"] + "'";
                            buttonHelper();                            
                        }
                    }

                    else
                    {
                        Query = "DELETE FROM MemberData WHERE EnrolmentNo='" + SearchingBox.Text + "' OR Name='" + SearchingBox.Text + "'";
                        buttonHelper();
                    }
                    
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }

                finally
                {
                    sqLite.Close();
                    MessageBox.Show("Selected Entry Deleted");
                }
            }
        }
        #endregion

        #region Helper Method
        private void buttonHelper()
        {
            SQLiteCommand createCommand = new SQLiteCommand(Query, sqLite);
            createCommand.ExecuteNonQuery();

            SQLiteDataAdapter Adapt = new SQLiteDataAdapter(createCommand);
            DataTable data = new DataTable("MemberData");
            Adapt.Fill(data);
            recordsTable.ItemsSource = data.DefaultView;
            Adapt.Update(data);
        }
        #endregion

        #region Nav-Exit
        private void goBack_Click(object sender, RoutedEventArgs e)
        {
            Options op = new Options();
            this.Hide();
            op.Show();
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            Update_Record up = new Update_Record();
            this.Hide();
            up.Show();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        #endregion

    }
}
