using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace PFSoftware.ProductivityTimer
{
    /// <summary>Interaction logic for TimerPage.xaml</summary>
    public partial class TimerPage : Page, INotifyPropertyChanged
    {
        private TimeSpan _totalWorkTime, _totalBreakTime, _currentWorkTime, _currentBreakTime;
        private readonly TimeSpan _oneSecond = DateTime.MinValue.AddSeconds(1) - DateTime.MinValue;
        private readonly DispatcherTimer _timerBreak = new DispatcherTimer();
        private readonly DispatcherTimer _timerWork = new DispatcherTimer();

        #region INotifyPropertyChanged Members

        /// <summary>The event that is raised when a property that calls the NotifyPropertyChanged method is changed.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Raises the PropertyChanged event alerting the WPF Framework to update the UI.</summary>
        /// <param name="propertyNames">The names of the properties to update in the UI.</param>
        protected void NotifyPropertyChanged(params string[] propertyNames)
        {
            if (PropertyChanged != null)
            {
                foreach (string propertyName in propertyNames)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        /// <summary>Raises the PropertyChanged event alerting the WPF Framework to update the UI.</summary>
        /// <param name="propertyName">The optional name of the property to update in the UI. If this is left blank, the name will be taken from the calling member via the CallerMemberName attribute.</param>
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged Members

        #region Properties

        /// <summary>The amount of time worked during this current work session.</summary>
        public TimeSpan CurrentWorkTime
        {
            get { return _currentWorkTime; }
            set { _currentWorkTime = value; NotifyPropertyChanged(nameof(CurrentWorkTimeToString)); }
        }

        /// <summary>The amount of time worked during this current work session, formatted.</summary>
        public string CurrentWorkTimeToString => CurrentWorkTime.ToString(@"hh\:mm\:ss");

        /// <summary>The amount of time breaked during this current break session.</summary>
        public TimeSpan CurrentBreakTime
        {
            get { return _currentBreakTime; }
            set { _currentBreakTime = value; NotifyPropertyChanged(nameof(CurrentBreakTimeToString)); }
        }

        /// <summary>The amount of time breaked during this current break session, formatted.</summary>
        public string CurrentBreakTimeToString => CurrentBreakTime.ToString(@"hh\:mm\:ss");

        /// <summary>The total amount of time worked during this session.</summary>
        public TimeSpan TotalWorkTime
        {
            get { return _totalWorkTime; }
            set { _totalWorkTime = value; NotifyPropertyChanged(nameof(TotalWorkTimeToString)); }
        }

        /// <summary>The total amount of time worked during this session, formatted.</summary>
        public string TotalWorkTimeToString => TotalWorkTime.ToString(@"hh\:mm\:ss");

        /// <summary>The total amount of breaked worked during this session.</summary>
        public TimeSpan TotalBreakTime
        {
            get { return _totalBreakTime; }
            set { _totalBreakTime = value; NotifyPropertyChanged(nameof(TotalBreakTimeToString)); }
        }

        /// <summary>The total amount of breaked worked during this session, formatted.</summary>
        public string TotalBreakTimeToString => TotalBreakTime.ToString(@"hh\:mm\:ss");

        #endregion Properties

        #region Button-Click Methods

        private void BtnStartWork_Click(object sender, RoutedEventArgs e)
        {
            CurrentWorkTime = new TimeSpan();
            _timerBreak.Stop();
            _timerWork.Start();
            BtnStartBreak.IsEnabled = true;
            BtnStartWork.IsEnabled = false;
            BtnStopWork.IsEnabled = true;
        }

        private void BtnStartBreak_Click(object sender, RoutedEventArgs e)
        {
            CurrentBreakTime = new TimeSpan();
            _timerWork.Stop();
            _timerBreak.Start();
            BtnStopWork.IsEnabled = true;
            BtnStartBreak.IsEnabled = false;
            BtnStartWork.IsEnabled = true;
        }

        private void BtnStopWork_Click(object sender, RoutedEventArgs e)
        {
            _timerWork.Stop();
            _timerBreak.Stop();
            BtnStartBreak.IsEnabled = false;
            BtnStartWork.IsEnabled = true;
            BtnStopWork.IsEnabled = false;
        }

        #endregion Button-Click Methods

        #region Timer Ticks

        private void TimerBreak_Tick(object sender, EventArgs e)
        {
            CurrentBreakTime += _oneSecond;
            TotalBreakTime += _oneSecond;
        }

        private void TimerWork_Tick(object sender, EventArgs e)
        {
            CurrentWorkTime += _oneSecond;
            TotalWorkTime += _oneSecond;
        }

        #endregion Timer Ticks

        public TimerPage()
        {
            InitializeComponent();
            DataContext = this;
            _timerBreak.Tick += TimerBreak_Tick;
            _timerBreak.Interval = new TimeSpan(0, 0, 1);
            _timerWork.Tick += TimerWork_Tick;
            _timerWork.Interval = new TimeSpan(0, 0, 1);
        }
    }
}