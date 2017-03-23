using System;
using System.IO;
using System.Data;
using System.Windows;
using System.Data.SQLite;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Controls;


namespace Ace_Reg
{
    /// <summary>
    /// Interaction logic for All_Records.xaml
    /// </summary>    
    public partial class All_Records : Window
    {

        private readonly string dbConString = @Constants.MEMDB;
        SQLiteConnection sqLite; 

        private String Query;
        public All_Records()
        {
            InitializeComponent();
            allRecord();
        }                                      
        
        #region Helper Methods
        private void allRecord()
        {
            sqLite = new SQLiteConnection(dbConString);
            try
            {
                sqLite.Open();
              //  sqLite.ChangePassword("simonLikesApples");               
                Query = "SELECT * FROM MemberData ORDER BY Name";
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

        private void buttonHelp()
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

        #region Buttons
        private void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            sqLite = new SQLiteConnection(dbConString);

            try
            {
                sqLite.Open();
                //sqLite.ChangePassword("simonLikesApples");
                Query = "SELECT * FROM MemberData WHERE EnrolmentNo='" + SearchingBox.Text + "' OR Name='" + SearchingBox.Text + "' ORDER BY Name";
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

        private void AllRecButtonon_OnClick(object sender, RoutedEventArgs e)
        {
            allRecord();
        }
        #endregion

        #region Nav-Exit
        private void goBack_Click(object sender, RoutedEventArgs e)
        {
            Options op = new Options();
            this.Hide();
            op.Show();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
        #endregion

        #region Export to Excel
        private void expButton_Click(object sender, RoutedEventArgs e)
        {
            ExportDataGrid();                    
        }

        private void ExportDataGrid()
        {
           
            try
            {                
                recordsTable.SelectAllCells();
                recordsTable.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
                ApplicationCommands.Copy.Execute(null, recordsTable);
                recordsTable.UnselectAllCells();
                String Clipboardresult = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
                StreamWriter swObj = new StreamWriter("Members Data.csv");
                swObj.WriteLine(Clipboardresult);
                swObj.Close();
                Process.Start("Members Data.csv");
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        #endregion                 

    }
}
