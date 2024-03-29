﻿using Microsoft.Win32;
using System;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Data.SQLite;
using System.Windows;

namespace Ace_Reg
{
    /// <summary>
    /// Interaction logic for ImportMember.xaml
    /// </summary>
    public partial class ImportMember : Window
    {

        private readonly string dbConString = @"Data Source=members.db;Version=3;";

        SQLiteConnection sqLite; string Query;        
        DataRowView rowView;
        OpenFileDialog ofd;

        public ImportMember()
        {
            InitializeComponent();
        }

        private void import_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Import selected data?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                MessageBox.Show("Import Process Canceled");
            }

            else
            {

                sqLite = new SQLiteConnection(dbConString);

                try
                {
                    sqLite.Open();

                    var selected = importCsvGrid.SelectedItems;

                    foreach (var selectedRows in selected)
                    {
                        rowView = (DataRowView)selectedRows;

                        Query = "INSERT INTO MemberData (EnrolmentNo, Name, Course, Semester, Section, Department, ContactNo, Email ) values('" 
                            + rowView["EnrolmentNo"] + "', '" + rowView["Name"] + "', '" + rowView["Course"] + "',  '" + rowView["Semester"] + "',  '" 
                            + rowView["Section"] + "',  '" + rowView["Department"] + "', '" + rowView["ContactNo"] + "', '" + rowView["Email"] + "' )";

                        help(Query, sqLite);
                    }

                    MessageBox.Show("Success!!");
                }

                catch (Exception ex)
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


        #region CSV to DataGrid
        private DataTable ReadCsv(string fileName)
        {
            DataTable table = new DataTable("Data");

            using (OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=\"" +
                System.IO.Path.GetDirectoryName(fileName) + "\"; Extended Properties='text;HDR=yes;Delimited(,)';"))
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
            openTheFile();
        }
        #endregion

        #region Nav & Exit   
        private void goBack_Click(object sender, RoutedEventArgs e)
        {
            Options ins = new Options();
            this.Hide();
            ins.Show();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
        #endregion

        void openTheFile()
        {
            try
            {
                ofd = new OpenFileDialog()
                {
                    Filter = "CSV | *csv",
                    ValidateNames = true,
                    Multiselect = false
                };

                ofd.ShowDialog();
                importCsvGrid.ItemsSource = ReadCsv(ofd.FileName).DefaultView;
            }

            catch (Exception ex)
            {
                ex.ToString();
                MessageBox.Show("No File Selected", "Message");
            }
        }

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
