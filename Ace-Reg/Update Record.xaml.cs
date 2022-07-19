using System;
using System.Windows;
using System.Data.SQLite;

namespace Ace_Reg
{
    /// <summary>
    /// Interaction logic for Update_Record.xaml
    /// </summary>
    public partial class Update_Record : Window
    {
        private readonly string dbConString = @"Data Source=members.db;Version=3;";

        SQLiteConnection sqLite; String Query;
        private string courseBox, semBox, depBox, secBox, testID;
        int x;

        public Update_Record()
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

        #region Search
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            sqLite = new SQLiteConnection(dbConString);

            try
            {

                sqLite.Open();
                Query = "SELECT * FROM MemberData WHERE EnrolmentNo='" + SearchingBox.Text + "' OR Name='" + SearchingBox.Text + "' ORDER BY Name";
                SQLiteCommand createCommand = new SQLiteCommand(Query, sqLite);
                SQLiteDataReader dataReader = createCommand.ExecuteReader();
                x = 0;
                while (dataReader.Read())
                {
                    testID = dataReader.GetString(0);
                    x++;
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

            finally
            {
                if (x >= 1 )
                {
                    MessageBox.Show("Record Selected: " + testID);
                    testID = null;
                    x = 0;
                }

                else
                    MessageBox.Show("No such record");

                sqLite.Close();
            }
        }
        #endregion

        #region Update
        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            sqLite = new SQLiteConnection(dbConString);

            if (MessageBox.Show("Update All Columns?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                MessageBox.Show("Updation Process Canceled");
            }

            else
            {
                try
                {
                    sqLite.Open();
                    Query = "UPDATE MemberData SET EnrolmentNo = '" + this.UIDbox.Text + "', Name = '" + this.NameBox.Text + "', Course = '" + this.courseBox + "', Semester = '" + this.semBox + "', Section = '" + this.secBox + "',  Department = '" + this.depBox + "', Email = '" + this.Emailbox.Text + "', ContactNo = '" + this.Contactbox.Text + "' WHERE EnrolmentNo='" + this.SearchingBox.Text + "' OR NAME = '" + this.SearchingBox.Text + "'";
                    SQLiteCommand createCommand = new SQLiteCommand(Query, sqLite);
                    createCommand.ExecuteNonQuery();
                    MessageBox.Show("Record Updated");
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
            }
        }
        #endregion

        #region Combo-Box
        private void CourseBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            courseBox = CourseBox.SelectedValue as string;
        }

        private void SemesterBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            semBox = SemesterBox.SelectedValue as string;
        }

        private void SectionBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            secBox = SectionBox.SelectedValue as string;
        }

        private void DeptBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            depBox = DeptBox.SelectedValue as string;
        }
        #endregion

        #region Nav-Exit
        private void goToOptions_Click(object sender, RoutedEventArgs e)
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

        #region Generic Update
        private void updateGeneric()
        {
            sqLite = new SQLiteConnection(dbConString);

            if (MessageBox.Show("Update Selected Column?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                MessageBox.Show("Updation Process Canceled");
            }

            else
            {
                try
                {
                    sqLite.Open();
                    SQLiteCommand createCommand = new SQLiteCommand(Query, sqLite);
                    createCommand.ExecuteNonQuery();
                    MessageBox.Show("Record Updated");
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
            }
        }
        #endregion

        #region Specific Column Update Buttons
        private void upContact_Click(object sender, RoutedEventArgs e)
        {
            Query = "UPDATE MemberData SET ContactNo = '" + this.Contactbox.Text + "' WHERE EnrolmentNo='" + this.SearchingBox.Text + "' OR NAME = '" + this.SearchingBox.Text + "'";

            updateGeneric();
        }

        private void upDep_Click(object sender, RoutedEventArgs e)
        {
            Query = "UPDATE MemberData SET Department = '" + this.depBox + "' WHERE EnrolmentNo='" + this.SearchingBox.Text + "' OR NAME = '" + this.SearchingBox.Text + "'";

            updateGeneric();
        }

        private void upName_Click(object sender, RoutedEventArgs e)
        {
            Query = "UPDATE MemberData SET Name = '" + this.NameBox.Text + "' WHERE EnrolmentNo='" + this.SearchingBox.Text + "' OR NAME = '" + this.SearchingBox.Text + "'";

            updateGeneric();
        }

        private void upSem_Click(object sender, RoutedEventArgs e)
        {
            Query = "UPDATE MemberData SET Semester = '" + this.semBox + "' WHERE EnrolmentNo='" + this.SearchingBox.Text + "' OR NAME = '" + this.SearchingBox.Text + "'";

            updateGeneric();
        }

        private void upCourse_Click(object sender, RoutedEventArgs e)
        {
            Query = "UPDATE MemberData SET Course = '" + this.courseBox + "' WHERE EnrolmentNo='" + this.SearchingBox.Text + "' OR NAME = '" + this.SearchingBox.Text + "'";

            updateGeneric();
        }

        private void upID_Click(object sender, RoutedEventArgs e)
        {
            Query = "UPDATE MemberData SET EnrolmentNo = '" + this.UIDbox.Text + "' WHERE EnrolmentNo='" + this.SearchingBox.Text + "' OR NAME = '" + this.SearchingBox.Text + "'";

            updateGeneric();
        }

        private void upSec_Click(object sender, RoutedEventArgs e)
        {
            Query = "UPDATE MemberData SET Section = '" + this.secBox + "' WHERE EnrolmentNo='" + this.SearchingBox.Text + "' OR NAME = '" + this.SearchingBox.Text + "'";

            updateGeneric();
        }

        private void upEmail_Click(object sender, RoutedEventArgs e)
        {
            Query = "UPDATE MemberData SET Email = '" + this.Emailbox.Text + "' WHERE EnrolmentNo='" + this.SearchingBox.Text + "' OR NAME = '" + this.SearchingBox.Text + "'";

            updateGeneric();
        }
        #endregion

    }
}
