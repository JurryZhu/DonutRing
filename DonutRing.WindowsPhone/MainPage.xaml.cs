namespace DonutRing.WindowsPhone
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
    using DonutRing.WindowsPhone.Resources;
    using System.Windows.Input;
    using System.Windows.Media;
    using DonutRing.WindowsPhone.Controls;
    using DonutRing.Shared;

    public partial class MainPage : PhoneApplicationPage
    {
        #region Constructors

        public MainPage()
        {
            InitializeComponent();
            canvas.Tap += (object sender, GestureEventArgs e) =>
            {
                if (e.OriginalSource == canvas)
                {
                    var p = e.GetPosition(canvas);
                    SpawnCircle(p.X, p.Y);
                    e.Handled = true;
                }
            };

            SpawnCircle(Settings.CIRCLE_DEFAULT_LOCATION, Settings.CIRCLE_DEFAULT_LOCATION);
        }

        #endregion

        #region Private Methods

        private Circle SpawnCircle(double left, double top)
        {
            var circle = new Circle();
            this.canvas.Children.Add(circle);          
            circle.Locate((left - circle.Radius / 2), (top - circle.Radius / 2));
            circle.RandomizeColor();
            return circle;
        }

        #endregion
    }
}