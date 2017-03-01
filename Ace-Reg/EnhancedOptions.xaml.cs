using System;
using System.Windows;

namespace Ace_Reg
{
    /// <summary>
    /// Interaction logic for EnhancedOptions.xaml
    /// </summary>
    public partial class EnhancedOptions : Window
    {
        public EnhancedOptions()
        {
            InitializeComponent();
        }

        private void goBack_Click(object sender, RoutedEventArgs e)
        {
            EventsDB goinBack = new EventsDB();
            this.Hide();
            goinBack.Show();
        }

        private void userHistoryBut_Click(object sender, RoutedEventArgs e)
        {
            UserHistory history = new UserHistory();
            this.Hide();
            history.Show();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
