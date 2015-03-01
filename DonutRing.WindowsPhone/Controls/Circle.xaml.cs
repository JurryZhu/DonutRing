namespace DonutRing.WindowsPhone.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using Microsoft.Phone.Controls;
    using Microsoft.Phone.Shell;
    using System.Windows.Input;
    using DonutRing.Shared.Manager;
    using System.Windows.Media;
    using System.Threading.Tasks;
    using System.Globalization;
    using System.Windows.Media.Animation;
    using DonutRing.Shared;

    public partial class Circle : UserControl
    {
        #region Public Properties

        public static readonly DependencyProperty TitleVisibilityProperty = DependencyProperty.Register("TitleVisibility", typeof(Visibility), typeof(Circle), null);
        public Visibility TitleVisibility
        {
            get { return (Visibility)GetValue(TitleVisibilityProperty); }
            set { SetValue(TitleVisibilityProperty, value); }
        }

        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register("Radius", typeof(int), typeof(Circle), null);
        public int Radius
        {
            get { return (int)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(Circle), null);
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register("Stroke", typeof(SolidColorBrush), typeof(Circle), null);
        public SolidColorBrush Stroke
        {
            get { return (SolidColorBrush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        #endregion

        #region Constructors

        public Circle()
        {
            InitializeComponent();
            this.DataContext = this;
            this.TitleVisibility = Visibility.Visible;
            this.Radius = Settings.RADIUS;
            this.Stroke = new SolidColorBrush(Colors.Green);
            this.Text = Settings.COLOR_NAME;
            this.ManipulationDelta += (object _sender, ManipulationDeltaEventArgs _e) =>
            {
                translateTransform.X += _e.DeltaManipulation.Translation.X;
                translateTransform.Y += _e.DeltaManipulation.Translation.Y;
            };
            var singleTapConfirmed = default(bool);
            this.Tap += async (object sender, GestureEventArgs e) =>
            {
                singleTapConfirmed = true;
                await Task.Delay(Settings.SINGLE_TAP_DELAY);
                if (singleTapConfirmed)
                {
                    RandomizeColor();
                    e.Handled = true;
                }
            };

            this.DoubleTap += (object sender, GestureEventArgs e) =>
            {
                singleTapConfirmed = false;
                this.TitleVisibility = TitleVisibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
                e.Handled = true;
            };
        }

        #endregion

        #region Public Methods

        public void Locate(double left, double top)
        {
            this.SetValue(Canvas.LeftProperty, left);
            this.SetValue(Canvas.TopProperty, top);
        }

        public void RandomizeColor()
        {
            var animator = BeginAnimation(this.Opacity, 0);
            ColorManager.GetColorTupleAsync().ContinueWith(t =>
            {
                var colorTuple = t.Result;
                this.Text = colorTuple.Item1;
                this.Stroke = new SolidColorBrush(colorTuple.Item2.ColorFromString());
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion

        #region Private Methods

        private Storyboard BeginAnimation(double from, double to)
        {
            var doubleAnimation = new DoubleAnimation();
            doubleAnimation.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 1200));
            var storyBoard = new Storyboard();
            Storyboard.SetTarget(doubleAnimation, this);
            storyBoard.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("Opacity"));
            doubleAnimation.AutoReverse = true;
            doubleAnimation.From = from;
            doubleAnimation.To = to;
            storyBoard.Children.Add(doubleAnimation);
            storyBoard.Begin();
            return storyBoard;
        }

        #endregion
    }
}
