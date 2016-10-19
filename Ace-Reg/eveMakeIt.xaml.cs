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
        private readonly string dbConString = @"Data Source=Events.db;Version=3;Password=simonLikesApples;";        
        SQLiteConnection sqLite; String eveTable;

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

                if (!(string.IsNullOrWhiteSpace(eveTable)))
                {
                    try
                    {
                        sqLite.Open();

                        string Q = "CREATE TABLE '" + eveTable + "'(EID TEXT PRIMARY KEY, Name TEXT, RollNo TEXT, College TEXT, Course TEXT, Semester_Section TEXT, Prize TEXT)";                        
                        SQLiteCommand createCommand = new SQLiteCommand(Q, sqLite);
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
