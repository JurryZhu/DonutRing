namespace DonutRing
{
    using System;
    using Android.App;
    using Android.Content;
    using Android.Runtime;
    using Android.Views;
    using Android.Widget;
    using Android.OS;
    using DonutRing.Androids;
    using Android.Content.PM;
    using DonutRing.Shared;
    using DonutRing.Shared.Manager;

    [Activity(Label = "Donut Ring", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        #region Protected Methods

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            var container = FindViewById<RelativeLayout>(Resource.Id.Container);
            container.Touch += (object sender, View.TouchEventArgs args) =>
            {
                try
                {
                    var e = args.Event;
                    if (e.Action == MotionEventActions.Down)
                    {
                        SpawnCircle((int)e.GetX(), (int)e.GetY());
                    }
                }
                catch(Exception ex)
                {
                    LogManager.Log(ex);
                }
            };
            this.SpawnCircle(Settings.CIRCLE_DEFAULT_LOCATION, Settings.CIRCLE_DEFAULT_LOCATION);
        }

        #endregion

        #region Private Methods

        private Circle SpawnCircle(int left, int top)
        {
            var circle = default(Circle);
            try
            {
                var container = FindViewById<RelativeLayout>(Resource.Id.Container);
                circle = new Circle(this) { Background = container.Background };
                container.AddView(circle, (int)circle.Radius, (int)circle.Radius);
                circle.Locate(left - (int)(circle.Radius / 2), top - (int)(circle.Radius / 2));
                circle.RandomizeColor();
            }
            catch (Exception ex)
            {
                LogManager.Log(ex);
            }

            return circle;
        }

        #endregion
    }
}

