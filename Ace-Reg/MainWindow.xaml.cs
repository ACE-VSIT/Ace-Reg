using System;
using System.ComponentModel;
using System.Windows;

namespace Ace_Reg
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private AceLogin ace = new AceLogin();
        private BackgroundWorker worker = new BackgroundWorker();

        public MainWindow()
        {

            InitializeComponent();

            /* Duration duration = new Duration(TimeSpan.FromSeconds(1));
            DoubleAnimation doubleanimation = new DoubleAnimation(100, duration);
            PBar.BeginAnimation(ProgressBar.ValueProperty, doubleanimation);         */

        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {


            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;

            worker.RunWorkerAsync();
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i <= 100; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                System.Threading.Thread.Sleep(20);

            }
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            PBar.Value = e.ProgressPercentage;

            if (PBar.Value == PBar.Maximum)
            {
                this.Close();
                ace.Show();
            }
        }
    }
}
