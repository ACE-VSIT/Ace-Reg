using System;
using System.Windows;
using System.Data.SQLite;

namespace Ace_Reg
{
    /// <summary>
    /// Interaction logic for eveMakeIt.xaml
    /// </summary>
    public partial class eveMakeIt : Window
    {
        private readonly string dbConString = @"Data Source=events.db;Version=3;";
        SQLiteConnection sqLite; string eveTable, approvalTable;

        private readonly string chickwa = "apricot_udon";

        public eveMakeIt()
        {
            InitializeComponent();
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            createEvent();
        }

        #region Create Event
        private void createEvent()
        {
            sqLite = new SQLiteConnection(dbConString);

            if (MessageBox.Show("Create this event?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                MessageBox.Show("Process Terminated");
            }

            else
            {

                eveTable = createEventBox.Text;
                approvalTable = eveTable + "_approval";

                if (!(string.IsNullOrWhiteSpace(eveTable)))
                {
                    try
                    {
                        sqLite.Open();

                        string Q = "CREATE TABLE '" + eveTable + "'(EID TEXT PRIMARY KEY, Name TEXT, RollNo TEXT, College TEXT, Course TEXT, Semester_Section TEXT, Prize TEXT)";
                        SQLiteCommand createCommand = new SQLiteCommand(Q, sqLite);
                        createCommand.ExecuteNonQuery();

                        Q = "CREATE TABLE '" + approvalTable + "'(EID TEXT PRIMARY KEY, Name TEXT, Prize TEXT, Status TEXT, SeatNo TEXT UNIQUE)";
                        createCommand = new SQLiteCommand(Q, sqLite);
                        createCommand.ExecuteNonQuery();

                        MessageBox.Show("Event Creation Success!");
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

                else
                {
                    MessageBox.Show("White space not allowed");
                }
            }
        }
        #endregion

        private void login_Click(object sender, RoutedEventArgs e)
        {
            bool test = false;

            test = passingMarks(chickwa, checkPass.Password);
            if (test == true)
            {
                createButton.Visibility = Visibility.Visible;
                label.Visibility = Visibility.Visible;

                login.Visibility = Visibility.Collapsed;
                checkPass.Visibility = Visibility.Collapsed;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            createButton.Visibility = Visibility.Collapsed;
            label.Visibility = Visibility.Collapsed;
        }

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

        private bool passingMarks(string A, string B)
        {
            if (A.Equals(B))
                return true;
            else
                return false;
        }
    }
}
