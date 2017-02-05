using System;
using System.Windows;

namespace Ace_Reg
{
    /// <summary>
    /// Interaction logic for EventsDB.xaml
    /// </summary>
    public partial class EventsDB : Window
    {
        public EventsDB()
        {
            InitializeComponent();
        }


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

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            eveMakeIt ev = new eveMakeIt();
            this.Hide();
            ev.Show();
        }

        private void showEvent_Click(object sender, RoutedEventArgs e)
        {
            ShowEvents sh = new ShowEvents();
            this.Hide();
            sh.Show();
        }

        private void modEvent_Click(object sender, RoutedEventArgs e)
        {
            InsertEvent ins = new InsertEvent();
            this.Hide();
            ins.Show();
        }

        private void goEnh_Click(object sender, RoutedEventArgs e)
        {
            EnhancedOptions ins = new EnhancedOptions();
            this.Hide();
            ins.Show();
        }

        private void approvalStatus_Click(object sender, RoutedEventArgs e)
        {
            EventApprovalStatus approval = new EventApprovalStatus();
            this.Hide();
            approval.Show();
        }
    }
}
