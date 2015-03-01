namespace DonutRing.Androids
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Android.App;
    using Android.Content;
    using Android.OS;
    using Android.Runtime;
    using Android.Views;
    using Android.Widget;
    using Android.Graphics.Drawables;
    using Android.Graphics;
    using Android.Graphics.Drawables.Shapes;
    using DonutRing.Shared.Manager;
    using System.Threading.Tasks;
    using Android.Animation;
    using DonutRing.Shared;

    public class Circle : Button, View.IOnTouchListener
    {
        #region Public Properties

        public bool ShowText { get; set; }

        public Color Color { get; set; }

        public float Radius { get; private set; }

        #endregion

        #region Private Fields

        private float originalRadius { get; set; }

        private float originalX;

        private float originalY;

        private GestureDetector gestureDetector;

        #endregion

        #region Constructors

        public Circle(Context context)
            : base(context)
        {
            ShowText = true;
            Color = Color.Green;
            this.Text = Settings.COLOR_NAME;
            this.SetRadius(Settings.RADIUS);
            gestureDetector = new GestureDetector(new CircleGestureListener(() => ToggleText(), () => RandomizeColor()));
            this.SetOnTouchListener(this);
        }

        #endregion

        #region Public Methods

        public void SetRadius(float radius)
        {
            originalRadius = radius;
            this.Radius = radius;
        }

        public void ToggleText()
        {
            ShowText = !ShowText;
            this.Invalidate();
        }

        public void RandomizeColor()
        {
            try
            {
                var animator = this.BeginAnimation(this.originalRadius, 60, (object sender, ValueAnimator.AnimatorUpdateEventArgs e) => { this.Radius = (int)e.Animation.AnimatedValue; this.Invalidate(); });
                ColorManager.GetColorTupleAsync().ContinueWith(t =>
                {
                    animator.Cancel();
                    this.BeginAnimation(this.Radius, this.originalRadius, (object sender, ValueAnimator.AnimatorUpdateEventArgs e) => { this.Radius = (int)e.Animation.AnimatedValue; this.Invalidate(); });
                    var colorTuple = t.Result;
                    this.Text = colorTuple.Item1;
                    this.Color = Color.ParseColor(colorTuple.Item2);
                    this.Invalidate();
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (Exception ex)
            {
                LogManager.Log(ex);
            }
        }

        public void Locate(int left, int top)
        {
            var layoutParameters = this.LayoutParameters as RelativeLayout.LayoutParams;
            if (layoutParameters != null)
            {
                layoutParameters.LeftMargin = left;
                layoutParameters.TopMargin = top;
                this.LayoutParameters = layoutParameters;
            }
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (gestureDetector != null)
            {
                gestureDetector.OnTouchEvent(e);
            }

            return base.OnTouchEvent(e);
        }

        public bool OnTouch(View view, MotionEvent e)
        {
            var result = false;
            try
            {
                switch (e.Action)
                {
                    case MotionEventActions.Down:
                        originalX = e.GetX();
                        originalY = e.GetY();
                        break;
                    case MotionEventActions.Move:
                        ((Circle)view).Locate((int)(view.Left + e.GetX() - originalX), (int)(view.Top + e.GetY() - originalY));
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManager.Log(ex);
            }

            return result;
        }

        #endregion

        #region Protected Methods

        protected override void OnDraw(Canvas canvas)
        {
            try
            {
                var paint = new Paint();
                paint.Color = this.Color;
                paint.SetStyle(Android.Graphics.Paint.Style.Stroke);
                paint.AntiAlias = true;
                var strokeWidth = 6;
                paint.StrokeWidth = strokeWidth;
                canvas.DrawCircle(this.Width / 2, this.Height / 2, this.Radius / 2 - strokeWidth / 2, paint);
                if (ShowText)
                {
                    paint = new Paint() { Color = this.Color, TextSize = 20 };
                    paint.SetStyle(Android.Graphics.Paint.Style.Fill);
                    paint.SetTypeface(Typeface.Serif);
                    var textBounds = new Rect();
                    paint.GetTextBounds(this.Text, 0, this.Text.Length, textBounds);
                    canvas.DrawText(this.Text, -textBounds.Left + this.Width / 2 - textBounds.Width() / 2, -textBounds.Top + this.Height / 2 - textBounds.Height() / 2, paint);
                }
            }
            catch (Exception ex)
            {
                LogManager.Log(ex);
            }
        }

        #endregion

        #region Private Methods

        private ValueAnimator BeginAnimation(float start, float end, EventHandler<ValueAnimator.AnimatorUpdateEventArgs> updater)
        {
            var animator = ValueAnimator.OfFloat(start, end);
            animator.Update += (object sender, ValueAnimator.AnimatorUpdateEventArgs e) => { updater(sender, e); };
            animator.SetDuration(1000);
            animator.Start();
            return animator;
        }

        #endregion

        #region Inner Classes

        private class CircleGestureListener : GestureDetector.SimpleOnGestureListener
        {
            private Action DoubleTapAction;
            private Action SingleTapAction;

            public CircleGestureListener(Action doubleTapAction, Action singleTapAction)
            {
                this.DoubleTapAction = doubleTapAction;
                this.SingleTapAction = singleTapAction;
            }

            public override bool OnDoubleTap(MotionEvent e)
            {
                if (DoubleTapAction != null)
                {
                    DoubleTapAction.Invoke();
                }

                return base.OnDoubleTap(e);
            }

            public override bool OnSingleTapConfirmed(MotionEvent e)
            {
                if (SingleTapAction != null)
                {
                    SingleTapAction.Invoke();
                }

                return base.OnSingleTapConfirmed(e);
            }
        }

        #endregion
    }
}