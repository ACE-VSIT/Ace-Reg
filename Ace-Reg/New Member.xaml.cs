using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SQLite;

namespace Ace_Reg
{
    /// <summary>
    /// Interaction logic for New_Member.xaml
    /// </summary>
    public partial class New_Member : Window
    {
        private readonly string dbConString = @"Data Source=Members.db;Version=3;";
        private string  courseBox, semBox, depBox, secBox;


        public New_Member()
        {
            InitializeComponent();
            fillCombo();
        }


        #region Combo Populate
        private void fillCombo()
        {
            CourseBox.Items.Add("BCA");
            CourseBox.Items.Add("MCA");
            CourseBox.Items.Add("BJMC");
            CourseBox.Items.Add("BA.LLB");
            CourseBox.Items.Add("MJMC");
            CourseBox.Items.Add("B.COM");
            CourseBox.Items.Add("BA (H)");

        //--------------------------------------------------------------------//

            SemesterBox.Items.Add("One");
            SemesterBox.Items.Add("Two");
            SemesterBox.Items.Add("Three");
            SemesterBox.Items.Add("Four");
            SemesterBox.Items.Add("Five");
            SemesterBox.Items.Add("Six");
            SemesterBox.Items.Add("Seven");
            SemesterBox.Items.Add("Eight");
            SemesterBox.Items.Add("Nine");
            SemesterBox.Items.Add("Ten");

        //--------------------------------------------------------------------//

            SectionBox.Items.Add("Morning A");
            SectionBox.Items.Add("Morning B");
            SectionBox.Items.Add("Morning C");
            SectionBox.Items.Add("Evening A");
            SectionBox.Items.Add("Evening B");

            //--------------------------------------------------------------------//

            DeptBox.Items.Add("President");
            DeptBox.Items.Add("Vice President");
            DeptBox.Items.Add("Executive");
            DeptBox.Items.Add("PR Lead");
            DeptBox.Items.Add("Technical Lead");
            DeptBox.Items.Add("AV - Core");
            DeptBox.Items.Add("WD - Core");
            DeptBox.Items.Add("SD - Core");
            DeptBox.Items.Add("GD - Core");
            DeptBox.Items.Add("AV - Trainee");
            DeptBox.Items.Add("WD - Trainee");
            DeptBox.Items.Add("SD - Trainee");
            DeptBox.Items.Add("GD - Trainee");
        }
        #endregion

        #region Data-Insertion
        private void SubmitButon_OnClick(object sender, RoutedEventArgs e)
        {
            SQLiteConnection sqLite = new SQLiteConnection(dbConString);

            //if ( UIDbox.Text.Equals(null) || NameBox.Text.Equals(null) || courseBox.Equals(null) || semBox.Equals(null) || secBox.Equals(null) || depBox.Equals(null) )
              //  MessageBox.Show("Fill all the details");

          //  else
          //  {
                try
                {
                    sqLite.Open();


                    string Query = "INSERT INTO MemberData (EnrolmentNo, Name, Course, Semester, Section, Department, ContactNo, Email ) values('" + this.UIDbox.Text + "', '" + this.NameBox.Text + "', '" + this.courseBox + "',  '" + this.semBox + "',  '" + this.secBox + "',  '" + this.depBox + "', '" + this.ContactBox.Text + "', '" + this.EmailBox.Text + "' )";
                    SQLiteCommand createCommand = new SQLiteCommand(Query, sqLite);
                    createCommand.ExecuteNonQuery();
                    MessageBox.Show("New Record Inserted");

                }
                catch (Exception exception)
                {

                    MessageBox.Show(exception.Message);
                }

                finally
                {
                    Options o = new Options();
                    sqLite.Close();
                    this.Hide();
                    o.Show();
                }
           // }
        }

        #endregion

        #region Combo-Box
        private void CourseBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            courseBox = CourseBox.SelectedValue as string;
        }

        private void SemesterBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            semBox = SemesterBox.SelectedValue as string;
        }

        private void SectionBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            secBox = SectionBox.SelectedValue as string;
        }

        private void DeptBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            depBox = DeptBox.SelectedValue as string;
        }

        #endregion

        #region Navigation
        private void goBack_Click(object sender, RoutedEventArgs e)
        {
            Options op = new Options();
            this.Hide();
            op.Show();
        }

        private void modBut_Click(object sender, RoutedEventArgs e)
        {
            Modify_Records mod = new Modify_Records();
            this.Hide();
            mod.Show();
        }

        private void importData_Click(object sender, RoutedEventArgs e)
        {
            ImportMember immem = new ImportMember();
            this.Hide();
            immem.Show();
        }

        #endregion

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
