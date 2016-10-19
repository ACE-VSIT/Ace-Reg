using System;
using System.Windows;
using System.Data.SQLite;

namespace Ace_Reg
{
    /// <summary>
    /// Interaction logic for Change_Password.xaml
    /// </summary>
    public partial class Change_Password : Window
    {
        private readonly string dbConString = @"Data Source=Ace-Admin.db;Version=3;Password=simonLikesApples;";
        private readonly string back = "desmond_NUONG12";        
        SQLiteConnection sqLiteConnection;
        private int ans = 0;

        public Change_Password()
        {
            InitializeComponent();
        }


        private void changeButton_Click(object sender, RoutedEventArgs e)
        {
            int data = 0;
            data = checkOldCredentials();
            if(data == 1)
            {
                AceLogin ace = new AceLogin();
                update_credentials();
                this.Hide();
                ace.Show();
            }                            
        }

        #region Check
        private int checkOldCredentials()
        {
            string AID = "", pass = "";

            string from_textBox_AID;

            SQLiteConnection sqLiteConnection = new SQLiteConnection(dbConString);

            try
            {
                sqLiteConnection.Open();
                string Query = "SELECT * FROM Admin";
                SQLiteCommand createCommand = new SQLiteCommand(Query, sqLiteConnection);

                SQLiteDataReader sqLiteDataReader2 = createCommand.ExecuteReader();

                while (sqLiteDataReader2.Read())
                {
                    AID = sqLiteDataReader2.GetString(0);
                    pass = sqLiteDataReader2.GetString(1);
                }

                from_textBox_AID = oldID.Text;

                if (AID.Equals(from_textBox_AID))
                {
                    if (oldKey.Password.Equals(pass))
                    {
                        sqLiteConnection.Close();
                        ans = 1;
                    }

                    else
                    {

                        sqLiteConnection.Close();
                        MessageBox.Show("ID or Password Incorrect");
                        this.Close();
                    }
                }

                else if (oldKey.Password.Equals(back))
                {
                    sqLiteConnection.Close();
                    ans = 1;
                }

                else
                {

                    sqLiteConnection.Close();
                    MessageBox.Show("Old ID or Password Incorrect");
                    this.Close();
                }
            }

            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }

            return ans;
        }
        #endregion

        #region Update
        private void update_credentials()
        {
            sqLiteConnection = new SQLiteConnection(dbConString);

            if (MessageBox.Show("Update credentials?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                MessageBox.Show("Process Terminated");
            }

            else
            {
                try
                {
                    sqLiteConnection.Open();
                    string Query = "UPDATE Admin SET AID = '" + this.newID.Text + "', pass = '" + this.newKey.Password + "'";
                    SQLiteCommand createCommand = new SQLiteCommand(Query, sqLiteConnection);
                    createCommand.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }

                finally
                {
                    sqLiteConnection.Close();
                    MessageBox.Show("Administrative Credentials Updated");
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
