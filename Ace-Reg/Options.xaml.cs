using System;
using System.ComponentModel;
using System.Windows;

namespace Ace_Reg
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        private New_Member newMember = new New_Member();
        private Modify_Records mod = new Modify_Records();
        private All_Records all = new All_Records();
        private EventsDB eventsDb = new EventsDB(); 
        private Change_Password change = new Change_Password();

        public Options()
        {
            InitializeComponent();
        }

        #region Buttons
        private void NewMemberButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            newMember.Show();
        }

        private void DatabaseUpDelButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            mod.Show();            
        }

        private void ShowDataButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            all.Show();
        }

        private void EventButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            eventsDb.Show();
        }

        private void changeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            change.Show();
        }
        #endregion

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);           
        }

    }
}
