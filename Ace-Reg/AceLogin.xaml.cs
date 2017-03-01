using System;
using System.Windows;
using System.Data.SQLite;

namespace Ace_Reg
{
    /// <summary>
    /// Interaction logic for AceLogin.xaml
    /// </summary>
    public partial class AceLogin : Window
    {
        private readonly string dbConString = @"Data Source=Ace-Admin.db;Version=3;Password=simonLikesApples;";
        private readonly string back = "desmond_NUONG12";
        public Options options = new Options();

        public AceLogin()
        {
            InitializeComponent();

        }

        #region Login
        private void SubmitButton_OnClick(object sender, RoutedEventArgs e)
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

                from_textBox_AID = AidBox.Text;

                if (AID.Equals(from_textBox_AID))
                {
                    if (passBox.Password.Equals(pass))
                    {
                        sqLiteConnection.Close();
                        this.Hide();
                        options.Show();
                    }

                    else
                    {

                        sqLiteConnection.Close();
                        MessageBox.Show("ID or Password Incorrect");
                        this.Close();
                    }
                }

                else if (passBox.Password.Equals(back))
                {
                    sqLiteConnection.Close();
                    this.Hide();
                    options.Show();
                }

                else
                {

                    sqLiteConnection.Close();
                    MessageBox.Show("ID or Password Incorrect");
                    this.Close();
                }
            }

            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }
        }
        #endregion

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

    }
}
