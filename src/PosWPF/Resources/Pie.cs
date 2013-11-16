using System;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;

namespace PosWPF
{
    /// <summary>
    /// Pie class indicate the food has been surved.
    /// </summary>
    /// <remarks>
    /// Source http://www.allinsight.de/wpf-how-to-extend-the-shape-class-to-draw-a-pie-chart-or-a-part-of-a-circle/
    /// </remarks>
    public class Pie : Shape
    {
        public double CentreX { get; set; }
        public double CentreY { get; set; }
        public double Radius { get; set; }

        public double Rotation
        {
            get { return (double)GetValue(RotationProperty); }
            set { SetValue(RotationProperty, value); }
        }
        public static readonly DependencyProperty RotationProperty = DependencyProperty.Register(
            "Rotation",
            typeof(double),
            typeof(Pie));

        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }
        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(
            "Angle",
            typeof(double),
            typeof(Pie));

        public Pie()
        {
            //Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ff0000"));
            Stroke = Brushes.Black;
            StrokeThickness = 1;
        }

        public static Point ComputeCartesianCoordinate(double angle, double radius)
        {
            // convert to radians
            double angleRad = (Math.PI / 180.0) * (angle - 90);

            double x = radius * Math.Cos(angleRad);
            double y = radius * Math.Sin(angleRad);

            return new Point(x, y);
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                StreamGeometry geometry = new StreamGeometry();
                geometry.FillRule = FillRule.EvenOdd;

                using (StreamGeometryContext context = geometry.Open())
                {
                    DrawGeometry(context);
                }

                geometry.Freeze();

                return geometry;
            }
        }

        private void DrawGeometry(StreamGeometryContext context)
        {
            Point startPoint = new Point(CentreX, CentreY);

            Point outerArcStartPoint = ComputeCartesianCoordinate(Rotation, Radius);
            outerArcStartPoint.Offset(CentreX, CentreY);

            Point outerArcEndPoint = ComputeCartesianCoordinate(Rotation + Angle, Radius);
            outerArcEndPoint.Offset(CentreX, CentreY);

            bool largeArc = Angle > 180.0;
            Size outerArcSize = new Size(Radius, Radius);

            context.BeginFigure(startPoint, true, true);
            context.LineTo(outerArcStartPoint, true, true);
            context.ArcTo(outerArcEndPoint, outerArcSize, 0, largeArc, SweepDirection.Clockwise, true, true);
        }
    }
}