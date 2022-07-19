using System;
using System.IO;
using System.Data;
using System.Windows;
using System.Diagnostics;
using System.Data.SQLite;
using System.Windows.Input;
using System.Windows.Controls;

namespace Ace_Reg
{
    /// <summary>
    /// Interaction logic for ShowEvents.xaml
    /// </summary>
    public partial class ShowEvents : Window
    {
        private readonly string dbConString = @"Data Source=events.db;Version=3;";

        private readonly string back = "apricot_udon";

        SQLiteConnection sqLite; string Query, tableNames;
        string selectedTable, apTable;
        DataRowView rowView;

        bool test;

        public ShowEvents()
        {
            InitializeComponent();
            fillCombo();
            test = false;
        }

        #region Export To CSV
        private void exportBut_Click(object sender, RoutedEventArgs e)
        {
            ExportDataGrid();
        }

        private void ExportDataGrid()
        {

            try
            {
                eventRecords.SelectAllCells();
                eventRecords.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
                ApplicationCommands.Copy.Execute(null, eventRecords);
                eventRecords.UnselectAllCells();
                String Clipboardresult = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
                StreamWriter swObj = new StreamWriter("Events Data.csv");
                swObj.WriteLine(Clipboardresult);
                swObj.Close();
                Process.Start("Events Data.csv");
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message);
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
                while(reader.Read())
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
        }
        #endregion

        #region Search & Display
        private void selectEvent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTable = selectEvent.SelectedValue as string;
            sqLite = new SQLiteConnection(dbConString);

            try
            {
                sqLite.Open();
                Query = "SELECT * FROM '" + selectedTable + "' Order by Name";
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

        private void Sbut_Click(object sender, RoutedEventArgs e)
        {
            sqLite = new SQLiteConnection(dbConString);

            try
            {
                sqLite.Open();
                Query = "SELECT * FROM '" + selectedTable + "' WHERE EID='" + SBox.Text + "' OR Name='" + SBox.Text + "' Order by Name";
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
        #endregion

        #region Destroy Event
        private void desEvbut_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Destroy selected event?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                MessageBox.Show("Process Terminated");
            }

            else
            {
                if (test == true)
                {
                    sqLite = new SQLiteConnection(dbConString);
                    apTable = selectedTable + "_approval";
                    try
                    {
                        sqLite.Open();
                        Query = "DROP TABLE '" + selectedTable + "'";
                        SQLiteCommand createCommand = new SQLiteCommand(Query, sqLite);
                        createCommand.ExecuteNonQuery();

                        Query = "DROP TABLE '" + apTable + "'";
                        createCommand = new SQLiteCommand(Query, sqLite);
                        createCommand.ExecuteNonQuery();

                        MessageBox.Show("Event has been eliminated");

                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }

                    finally
                    {
                        EventsDB db = new EventsDB();
                        sqLite.Close();
                        this.Hide();
                        db.Show();
                    }
                }

                else
                    MessageBox.Show("You shall NOT pass!");
            }
        }
        #endregion

        #region Modify Records
        private void delRec_Click(object sender, RoutedEventArgs e)
        {
            sqLite = new SQLiteConnection(dbConString);
            apTable = selectedTable + "_approval";

            if (MessageBox.Show("Delete selected entry?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                MessageBox.Show("Deletion Process Canceled");
            }

            else
            {
                try
                {
                    sqLite.Open();

                    if (SBox.Text.Equals(null) || SBox.Text.Equals("") || SBox.Text.Equals(" "))
                    {
                        var selected = eventRecords.SelectedItems;

                        foreach(var selectedRows in selected)
                        {
                            rowView = (DataRowView)selectedRows;

                            Query = "DELETE FROM '" + selectedTable + "'  WHERE EID='" + rowView["EID"] + "'";
                            SQLiteCommand createCommand = new SQLiteCommand(Query, sqLite);
                            createCommand.ExecuteNonQuery();

                            Query = "DELETE FROM '" + apTable + "'  WHERE EID='" + rowView["EID"] + "'";
                            createCommand = new SQLiteCommand(Query, sqLite);
                            createCommand.ExecuteNonQuery();
                        }
                    }

                    else
                    {
                        Query = "DELETE FROM '" + selectedTable + "'  WHERE EID='" + SBox.Text + "' OR Name='" + SBox.Text + "'";
                        buttonHelp();

                        Query = "DELETE FROM '" + apTable + "'  WHERE EID='" + SBox.Text + "' OR Name='" + SBox.Text + "'";
                        buttonHelp();
                    }

                    MessageBox.Show("Selected Entry Deleted");
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }

                finally
                {
                    buttonHelp();
                    sqLite.Close();
                }
            }
        }

        private void upRec_Click(object sender, RoutedEventArgs e)
        {
            Update_Events up = new Update_Events();
            this.Hide();
            up.Show();
        }
        #endregion

        #region Helper Method
        private void buttonHelp()
        {
            SQLiteCommand createCommand = new SQLiteCommand(Query, sqLite);
            createCommand.ExecuteNonQuery();

            SQLiteDataAdapter Adapt = new SQLiteDataAdapter(createCommand);
            DataTable data = new DataTable(selectedTable);
            Adapt.Fill(data);
            eventRecords.ItemsSource = data.DefaultView;
            Adapt.Update(data);
        }

        private void image_MouseEnter(object sender, MouseEventArgs e)
        {
            image.Visibility = Visibility.Hidden;
            passwordCheck.Visibility = Visibility.Visible;
        }

        private void passwordCheck_MouseLeave(object sender, MouseEventArgs e)
        {
            if (passwordCheck.Password.Equals(back))
                test = true;
            else
                test = false;

            image.Visibility = Visibility.Visible;
        }

        private void image_MouseLeave(object sender, MouseEventArgs e)
        {
            image.Visibility = Visibility.Visible;
            passwordCheck.Visibility = Visibility.Hidden;
        }

        private void passwordCheck_MouseEnter(object sender, MouseEventArgs e)
        {
            image.Visibility = Visibility.Hidden;
            passwordCheck.Visibility = Visibility.Visible;
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
