using GeneralLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RoboGUI.Views
{
    /// <summary>
    /// Interaction logic for RoboMap.xaml
    /// </summary>
    public partial class RoboMap : UserControl, INotifyPropertyChanged
    {
        private static double CellSize = 50;

        private double zoom;

        private List<Field> fields;

        private List<TravelPoint> createdRoute;

        private Position currentRobotPosition;

        private double currentRobotRotation;

        // drag action
        private bool dragAction;

        private Point oldMouse;
        private double tx;
        private double ty;

        // Bindings

        private double mousePositionX;

        private double mousePositionY;

        public RoboMap()
        {
            InitializeComponent();

            this.fields = new List<Field>();
            this.createdRoute = new List<TravelPoint>();

            this.Loaded += RoboMap_Loaded;

            this.mousePositionGrid.DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public double MousePositionX
        {
            get
            {
                return this.mousePositionX;
            }

            set
            {
                this.mousePositionX = value;

                this.Notify();
            }
        }

        public double MousePositionY
        {
            get
            {
                return this.mousePositionY;
            }

            set
            {
                this.mousePositionY = value;

                this.Notify();
            }
        }

        public void UpdateRobotPosition(Position newPosition)
        {
            this.currentRobotPosition = newPosition;

            robotTranslateTransform.X = newPosition.X;
            robotTranslateTransform.Y = newPosition.Y;
        }

        public void UpdateRobotRotation(float newRotation)
        {
            this.currentRobotRotation = newRotation;

            robotRotationTransform.Angle = this.currentRobotRotation * -1;
        }

        public void UpdateZoom(double newZoom)
        {
            this.zoom = newZoom;

            // zoom grid
            this.gridmapScaleTransform.ScaleX = this.zoom;
            this.gridmapScaleTransform.ScaleY = this.zoom;

            // zoom robot
            //this.robotScaleTransform.ScaleX = this.zoom;
            //this.robotScaleTransform.ScaleY = this.zoom;

            // zoom scanned area
            this.scanmapScaleTransform.ScaleX = this.zoom;
            this.scanmapScaleTransform.ScaleY = this.zoom * -1;
        }

        private void AddField(Field field)
        {
            this.fields.Add(field);

            Rectangle rect = new Rectangle();

            rect.Width = CellSize;
            rect.Height = CellSize;

            Canvas.SetLeft(rect, field.Position.X * CellSize - CellSize / 2);
            Canvas.SetTop(rect, field.Position.Y * CellSize - CellSize / 2);

            rect.MouseLeftButtonUp += Rect_MouseLeftButtonUp;
            rect.DataContext = field;

            if (field.State == Fieldstate.free)
            {
                rect.Fill = Brushes.DarkGreen;
            }
            else if (field.State == Fieldstate.freeScanned)
            {
                rect.Fill = Brushes.LightGreen;
            }
            else if (field.State == Fieldstate.occupied)
            {
                rect.Fill = Brushes.DarkRed;
            }
            else if (field.State == Fieldstate.unscanned)
            {
                rect.Fill = Brushes.Black;
            }

            this.scanMap.Children.Add(rect);
        }

        private void Rect_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (dragAction)
            {
                return;
            }

            Field field = (Field)((Rectangle)sender).DataContext;
            Point p = e.GetPosition(scanMap);

            // Maybe only free scanned?
            if (field.State == Fieldstate.free || field.State == Fieldstate.freeScanned)
            {
                AddTravelPoint(new TravelPoint(new Point(p.X, p.Y)));
            }
            //MousePositionX = p.X;
            //MousePositionY = p.Y;
        }

        private void AddTravelPoint(TravelPoint tp)
        {
            Ellipse el = new Ellipse();
            el.Width = 10;
            el.Height = 10;

            el.Fill = Brushes.Magenta;
            Canvas.SetLeft(el, tp.Position.X - el.Width / 2);
            Canvas.SetTop(el, tp.Position.Y - el.Height / 2);

            if (this.createdRoute.Count > 0)
            {
                Line l1 = new Line();
                l1.Stroke = Brushes.Magenta;
                l1.StrokeThickness = 1;

                l1.X1 = tp.Position.X;
                l1.Y1 = tp.Position.Y;

                l1.X2 = this.createdRoute[this.createdRoute.Count - 1].Position.X;
                l1.Y2 = this.createdRoute[this.createdRoute.Count - 1].Position.Y;

                routeMap.Children.Add(l1);
            }

            routeMap.Children.Add(el);

            //TravelPoint t = new TravelPoint(new Point(tp.Position.X, tp.Position.Y));

            this.createdRoute.Add(tp);

            el.DataContext = tp;

            el.MouseLeftButtonUp += El_MouseLeftButtonUp;
            el.MouseRightButtonUp += El_MouseRightButtonUp;
        }

        private void El_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            TravelPoint tp = (TravelPoint)((Ellipse)sender).DataContext;

            this.createdRoute.Remove(tp);
            List<TravelPoint> copied = new List<TravelPoint>(this.createdRoute);

            this.createdRoute.Clear();

            this.routeMap.Children.Clear();

            foreach (TravelPoint p in copied)
            {
                this.AddTravelPoint(p);
            }
        }

        private void El_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //routePropertiesGrid.DataContext = ((Ellipse)sender).DataContext;
        }

        private void RoboMap_Loaded(object sender, RoutedEventArgs e)
        {
            this.Reset();

            this.UpdateRobotPosition(new Position(200, 200));

            this.UpdateRobotRotation(45);

            //this.AddField(new Field(0, 0) { State = Fieldstate.free });
            this.AddField(new Field(5, 5) { State = Fieldstate.freeScanned });
            this.AddField(new Field(-5, -5) { State = Fieldstate.occupied });
        }

        private void Reset()
        {
            this.zoom = 1.0;
            this.fields.Clear();

            this.dragAction = false;

            this.GenerateGridMap(10000, 10000, this.zoom);
        }

        private void GenerateGridMap(double width, double height, double zoom)
        {
            this.gridMap.Children.Clear();

            Rectangle start = new Rectangle();

            start.Width = CellSize;
            start.Height = CellSize;

            Canvas.SetLeft(start, CellSize / -2);
            Canvas.SetTop(start, CellSize / -2);

            start.Fill = Brushes.White;

            this.gridMap.Children.Add(start);

            for (double i = CellSize / -2; i <= width; i += (zoom * CellSize))
            {
                Line l1 = new Line();
                l1.X1 = i;
                l1.X2 = i;

                l1.Y1 = CellSize / -2 + height * -1;
                l1.Y2 = CellSize / 2 + height;

                l1.Stroke = Brushes.DarkGray;
                l1.StrokeThickness = 0.5;

                gridMap.Children.Add(l1);
            }

            for (double i = CellSize / -2; i > width * -1; i -= (zoom * CellSize))
            {
                Line l1 = new Line();
                l1.X1 = i;
                l1.X2 = i;

                l1.Y1 = CellSize / -2 + height * -1;
                l1.Y2 = CellSize / 2 + height;

                l1.Stroke = Brushes.DarkGray;
                l1.StrokeThickness = 0.5;

                gridMap.Children.Add(l1);
            }

            for (double i = CellSize / 2; i <= height; i += (zoom * CellSize))
            {
                Line l1 = new Line();
                l1.Y1 = i;
                l1.Y2 = i;

                l1.X1 = CellSize / -2 + width * -1;
                l1.X2 = CellSize / 2 + width;

                l1.Stroke = Brushes.DarkGray;
                l1.StrokeThickness = 0.5;

                gridMap.Children.Add(l1);
            }

            for (double i = CellSize / -2; i > height * -1; i -= (zoom * CellSize))
            {
                Line l1 = new Line();
                l1.Y1 = i;
                l1.Y2 = i;

                l1.X1 = CellSize / -2 + width * -1;
                l1.X2 = CellSize / 2 + width;

                l1.Stroke = Brushes.DarkGray;
                l1.StrokeThickness = 0.5;

                gridMap.Children.Add(l1);
            }
        }

        private void mainGrid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                this.UpdateZoom(this.zoom * 1.25);
            }
            else
            {
                this.UpdateZoom(this.zoom / 1.25);
            }
        }

        private void robotPositionPolygon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //this.oldMouse = e.GetPosition(this);
            this.oldMouse = e.GetPosition(this);
            tx = scanmapTranslateTransform.X;
            ty = scanmapTranslateTransform.Y;
        }

        private void robotPositionPolygon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            dragAction = false;
        }

        private void robotPositionPolygon_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                dragAction = true;

                scanmapTranslateTransform.X = tx + e.GetPosition(this).X - oldMouse.X;
                scanmapTranslateTransform.Y = ty + e.GetPosition(this).Y - oldMouse.Y;
            }

            Point p = e.GetPosition(scanMap);

            MousePositionX = Math.Round(p.X); // + (this.mainGrid.ActualWidth / zoom) / 2, 0);
            MousePositionY = Math.Round(p.Y); // + (this.mainGrid.ActualHeight / zoom) / 2, 0);
        }

        private void Notify([CallerMemberName]string propertyName = null)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void jumpcenter_Click(object sender, RoutedEventArgs e)
        {
            scanmapTranslateTransform.X = 0;
            scanmapTranslateTransform.Y = 0;
        }

        private void clearroute_Click(object sender, RoutedEventArgs e)
        {
            this.createdRoute.Clear();
            this.routeMap.Children.Clear();
        }
    }
}
