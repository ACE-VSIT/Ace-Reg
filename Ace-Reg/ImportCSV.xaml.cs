using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Ace_Reg
{
    /// <summary>
    /// Interaction logic for ImportCSV.xaml
    /// </summary>
    public partial class ImportCSV : Window
    {

        private readonly string dbConString = @"Data Source=Events.db;Version=3;Password=simonLikesApples;";

        SQLiteConnection sqLite; string Query, tableNames;
        private string selectedTable, approvalTable;
        DataRowView rowView;

        public ImportCSV()
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
                        eventBox.Items.Add(tableNames);
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

        private void import_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Import selected data?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                MessageBox.Show("Import Process Canceled");
            }

            else
            {

                SQLiteConnection sqLite = new SQLiteConnection(dbConString);

                approvalTable = selectedTable + "_approval";

                try
                {
                    sqLite.Open();

                    var selected = importCsvGrid.SelectedItems;

                    foreach(var selectedRows in selected)
                    {
                        rowView = (DataRowView)selectedRows;

                        string Query = "INSERT INTO '" + selectedTable + "'(EID, Name, RollNo, College, Course, Semester_Section, Prize) values('"
                        + rowView["EID"] + "', '" + rowView["Name"] + "', '" + rowView["RollNo"] + "',  '" + rowView["College"] + "',  '"
                        + rowView["Course"] + "',  '" + rowView["Semester_Section"] + "', '" + rowView["Prize"] + "')";
                        help(Query, sqLite);

                        Query = "INSERT INTO '" + approvalTable + "'(EID, Name, Prize) values('" + rowView["EID"] + "', '" + rowView["Name"] + "', '" + rowView["Prize"] + "' )";
                        help(Query, sqLite);
                    }

                    MessageBox.Show("Success!!");
                }

                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                finally
                {
                    sqLite.Close();
                }
            }
        }

        void help(string q, SQLiteConnection sq)
        {
            SQLiteCommand createCommand = new SQLiteCommand(q, sq);
            createCommand.ExecuteNonQuery();
        }

        private void eventBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTable = eventBox.SelectedValue as string;
        }

        #region CSV to DataGrid
        private DataTable ReadCsv(string fileName)
        {
            DataTable table = new DataTable("Data");

            using (OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=\"" +
                Path.GetDirectoryName(fileName) + "\"; Extended Properties='text;HDR=yes;Delimited(,)';"))
            {
                using (OleDbCommand cmd = new OleDbCommand(string.Format("select * from [{0}]", new FileInfo(fileName).Name), con))
                {
                    con.Open();
                    using (OleDbDataAdapter adapt = new OleDbDataAdapter(cmd))
                    {
                        adapt.Fill(table);
                    }
                }
            }

            return table;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog()
                {
                    Filter = "CSV | *csv",
                    ValidateNames = true,
                    Multiselect = false
                };

                ofd.ShowDialog();
                importCsvGrid.ItemsSource = ReadCsv(ofd.FileName).DefaultView;
            }

            catch(Exception ex)
            {
                ex.ToString();
                MessageBox.Show("No File Selected", "Message");
            }
        }        
        #endregion

        #region Nav & Exit   
        private void goBack_Click(object sender, RoutedEventArgs e)
        {
            InsertEvent ins = new InsertEvent();
            this.Hide();
            ins.Show();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Environment.Exit(0);
        }
        #endregion

        #region Drag Drop        
        private void importCsvGrid_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.                
                var test = e.Data.GetData(DataFormats.FileDrop);
                string[] files = (string[])test;
                var file = files[0];

                if (file.EndsWith("csv"))
                    importCsvGrid.ItemsSource = ReadCsv(file).DefaultView;

                else if (file.EndsWith("wav") || file.EndsWith("mp3"))
                {
                    secretMusic.Source = new Uri(file);
                    secretMusic.Play();
                }
            }

        }

        private void importCsvGrid_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }
        #endregion
    }
}
