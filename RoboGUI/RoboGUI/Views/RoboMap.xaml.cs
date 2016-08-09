using GeneralLibrary;
using Microsoft.Win32;
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
        private static int CellSize = 50;

        private double zoom;

        private List<Field> fields;

        private List<Position> createdRoute;

        private Point currentRobotPosition;

        private double currentRobotRotation;

        // drag action
        private bool mapDown;

        private bool moveMap;

        private bool robotDown;

        private bool moveRobot;

        private Point oldMouse;
        private double tx;
        private double ty;

        // Bindings

        private double mousePositionX;

        private double mousePositionY;

        // edit

        private Map currentMap;

        private MapMode currentMode;

        public RoboMap()
        {
            InitializeComponent();

            this.fields = new List<Field>();
            this.createdRoute = new List<Position>();

            this.Loaded += RoboMap_Loaded;

            this.mousePositionGrid.DataContext = this;

            this.fieldStatesComboBox.ItemsSource = new Fieldstate[] 
            {
                Fieldstate.freeScanned,
                Fieldstate.occupied
            };
        }

        public delegate void RouteRun(Map map, Route route);

        public delegate void RouteCancel();

        public event RouteRun OnRouteRun;

        public event RouteCancel OnRouteCancel;

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

        public void UpdateRobotPosition(Point newPosition)
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

        public void UpdateMap(Map map)
        {
            ResetMap();
            ResetRoute();
            this.currentMap = map;

            for (int i = 0; i < map.Fields.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < map.Fields.GetUpperBound(1) + 1; j++)
                {
                    this.AddField(map.Fields[i, j]);
                }
            }
        }

        public void UpdateRoute(Route route)
        {
            this.ResetRoute();

            foreach (Position pos in route.Points)
            {
                this.AddTravelPoint(pos);
            }
        }

        private void SwitchToMode(MapMode mode)
        {
            this.currentMode = mode;

            if (mode == MapMode.Show)
            {
                clearroute.Visibility = Visibility.Visible;
                editmapButton.Visibility = Visibility.Visible;
                editModePanel.Visibility = Visibility.Collapsed;
                exitEditButton.Visibility = Visibility.Collapsed;
            }
            else if (mode == MapMode.Edit)
            {
                clearroute.Visibility = Visibility.Collapsed;
                editmapButton.Visibility = Visibility.Collapsed;
                editModePanel.Visibility = Visibility.Visible;
                exitEditButton.Visibility = Visibility.Visible;

                this.ResetRoute();
            }
        }

        private void RoboMap_Loaded(object sender, RoutedEventArgs e)
        {
            this.Reset();

            //this.UpdateRobotPosition(new Point(200, 200));

            //this.UpdateRobotRotation(45);

            //this.AddField(new Field(0, 0) { State = Fieldstate.free });
            //this.AddField(new Field(5, 5) { State = Fieldstate.freeScanned });
            //this.AddField(new Field(-5, -5) { State = Fieldstate.occupied });
        }

        private void Reset()
        {
            this.UpdateZoom(1.0);

            this.ResetMap();

            this.ResetRoute();

            this.UpdateRobotPosition(new Point(0, 0));
            this.UpdateRobotRotation(0);

            this.moveMap = false;

            this.GenerateGridMap(10000, 10000, this.zoom);
        }

        private void ResetRoute()
        {
            this.createdRoute.Clear();
            this.routeMap.Children.Clear();

            this.createdRoute.Add(new Position(0, 0));
        }

        private void ResetMap()
        {
            this.currentMap = new Map();
            this.currentMap.Fields = new Field[1, 1];
            this.currentMap.Fields[0, 0] = new Field(0, 0);
            this.currentMap.Fields[0, 0].State = Fieldstate.free;

            this.scanMap.Children.Clear();
            this.fields.Clear();
        }

        private void GenerateGridMap(double width, double height, double zoom)
        {
            this.gridMap.Children.Clear();

            /*Rectangle start = new Rectangle();

            start.Width = CellSize;
            start.Height = CellSize;

            Canvas.SetLeft(start, CellSize / -2);
            Canvas.SetTop(start, CellSize / -2);

            start.Fill = Brushes.White;

            this.gridMap.Children.Add(start);*/

            Field start = new Field(0, 0);
            start.State = Fieldstate.freeScanned;

            this.AddField(start);

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

        private void AddField(Field field)
        {
            this.fields.Add(field);

            Rectangle rect = new Rectangle();

            rect.Width = CellSize;
            rect.Height = CellSize;

            Canvas.SetLeft(rect, field.Position.X * CellSize - CellSize / 2);
            Canvas.SetTop(rect, field.Position.Y * CellSize - CellSize / 2);

            rect.MouseLeftButtonUp += Rect_MouseLeftButtonUp;
            rect.MouseRightButtonUp += Rect_MouseRightButtonUp;
            rect.MouseMove += Rect_MouseMove;
            rect.DataContext = field;

            this.FillRectangleByField(rect, field);

            this.scanMap.Children.Add(rect);
        }

        private void Rect_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.ChangeStateOfRectangle((Rectangle)sender);
        }

        private void Rect_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                this.ChangeStateOfRectangle((Rectangle)sender);
            }
        }

        private void ChangeStateOfRectangle(Rectangle rect)
        {
            if (this.currentMode == MapMode.Edit)
            {
                Field field = (Field)rect.DataContext;

                // We assume that the 0 0 position is the robot position
                // so it cannot be marked as occupied.
                if (!(field.Position.X == 0 && field.Position.Y == 0))
                {
                    field.State = (Fieldstate)fieldStatesComboBox.SelectedItem;
                    rect.Fill = Brushes.DarkRed;

                    this.FillRectangleByField(rect, field);
                }
            }
        }

        private void FillRectangleByField(Rectangle rect, Field field)
        {
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
        }

        private void Rect_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Field field = (Field)((Rectangle)sender).DataContext;
            Point p = e.GetPosition(scanMap);

            if (moveMap)
            {
                return;
            }

            if (moveRobot)
            {
                if (field.State == Fieldstate.free || field.State == Fieldstate.freeScanned)
                {
                    // Translate the map to make sure the robot is still at the position 0 0
                    Map newMap = new Map();
                    newMap.Fields = this.currentMap.Fields;
                    int tempX = field.Position.X;
                    int tempY = field.Position.Y;

                    for (int i = 0; i < newMap.Fields.GetUpperBound(0) + 1; i++)
                    {
                        for (int j = 0; j < newMap.Fields.GetUpperBound(1) + 1; j++)
                        {
                            newMap.Fields[i, j].Position.X = newMap.Fields[i, j].Position.X - tempX;
                            newMap.Fields[i, j].Position.Y = newMap.Fields[i, j].Position.Y - tempY;
                        }
                    }

                    this.UpdateMap(newMap);
                }
            }
            else
            {
                if (this.currentMode == MapMode.Show)
                {
                    // Maybe only free scanned?
                    if (field.State == Fieldstate.free || field.State == Fieldstate.freeScanned)
                    {
                        AddTravelPoint(new Position(
                            field.Position.X * 1,
                            field.Position.Y * 1));
                    }
                    //MousePositionX = p.X;
                    //MousePositionY = p.Y;
                }
            }
        }

        private void AddTravelPoint(Position tp)
        {
            Ellipse el = new Ellipse();
            el.Width = 10;
            el.Height = 10;

            el.Fill = Brushes.Magenta;
            Canvas.SetLeft(el, tp.X * CellSize - el.Width / 2);
            Canvas.SetTop(el, tp.Y * CellSize - el.Height / 2);

            if (this.createdRoute.Count > 0)
            {
                Line l1 = new Line();
                l1.Stroke = Brushes.Magenta;
                l1.StrokeThickness = 1;

                l1.X1 = tp.X * CellSize;
                l1.Y1 = tp.Y * CellSize;

                l1.X2 = this.createdRoute[this.createdRoute.Count - 1].X * CellSize;
                l1.Y2 = this.createdRoute[this.createdRoute.Count - 1].Y * CellSize;

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
            Position tp = (Position)((Ellipse)sender).DataContext;

            // The first travel point cannot be deleted because the start is always the first travel point.
            if (this.createdRoute.IndexOf(tp) != 0)
            {
                this.createdRoute.Remove(tp);
                List<Position> copied = new List<Position>(this.createdRoute);

                this.createdRoute.Clear();

                this.routeMap.Children.Clear();

                foreach (Position p in copied)
                {
                    this.AddTravelPoint(p);
                }
            }
        }

        private void El_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //routePropertiesGrid.DataContext = ((Ellipse)sender).DataContext;
        }

        private void mainGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //this.oldMouse = e.GetPosition(this);
            if (!robotDown)
            {
                mapDown = true;

                this.oldMouse = e.GetPosition(this);
                tx = scanmapTranslateTransform.X;
                ty = scanmapTranslateTransform.Y;
            }
        }

        private void mainGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mapDown = false;
            moveMap = false;
            
            robotDown = false;
            moveRobot = false;

            this.UpdateRobotPosition(new Point(0, 0));
            this.robotPositionPolygon.IsHitTestVisible = true;
        }

        private void mainGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                moveMap = mapDown;
                moveRobot = robotDown;

                if (moveMap)
                {
                    scanmapTranslateTransform.X = tx + e.GetPosition(this).X - oldMouse.X;
                    scanmapTranslateTransform.Y = ty + e.GetPosition(this).Y - oldMouse.Y;
                }
                else if (moveRobot)
                {
                    robotTranslateTransform.X = tx + e.GetPosition(scanMap).X - oldMouse.X;
                    robotTranslateTransform.Y = ty + e.GetPosition(scanMap).Y - oldMouse.Y;
                }
            }

            Point p = e.GetPosition(scanMap);

            MousePositionX = Math.Round(p.X); // + (this.mainGrid.ActualWidth / zoom) / 2, 0);
            MousePositionY = Math.Round(p.Y); // + (this.mainGrid.ActualHeight / zoom) / 2, 0);
        }

        private void robotPositionPolygon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Robots position can only be changed in edit mode!
            if (this.currentMode == MapMode.Edit)
            {
                robotDown = true;
                // Make sure that the underlying field gets notified about the mouse up event.
                robotPositionPolygon.IsHitTestVisible = false;

                this.oldMouse = e.GetPosition(scanMap);
                tx = robotTranslateTransform.X;
                ty = robotTranslateTransform.Y;
            }
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
            this.ResetRoute();
        }

        private void createmapButton_Click(object sender, RoutedEventArgs e)
        {
            MapCreatorDialog dia = new MapCreatorDialog();
            
            if (dia.ShowDialog() == true)
            {
                this.Reset();
                Map newMap = new Map();
                newMap.Fields = new Field[dia.MapWidth, dia.MapHeight];

                for (int i = 0; i < dia.MapWidth; i++)
                {
                    for (int j = 0; j < dia.MapHeight; j++)
                    {
                        newMap.Fields[i, j] = new Field(i, j);
                    }
                }

                this.UpdateMap(newMap);
            }
        }

        private void editmapButton_Click(object sender, RoutedEventArgs e)
        {
            this.SwitchToMode(MapMode.Edit);
        }

        private void exitEditButton_Click(object sender, RoutedEventArgs e)
        {
            this.SwitchToMode(MapMode.Show);
        }

        //
        // Persistence loading
        //

        private void loadmapButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dia = new OpenFileDialog();
            dia.FileName = "";
            dia.DefaultExt = "rmap";

            string filename = string.Empty;

            if (dia.ShowDialog() == true)
            {
                filename = dia.FileName;

                Map map = Persistent.LoadMap(filename);

                if (map != null)
                {
                    this.UpdateMap(map);
                }
            }
        }

        private void savemapButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dia = new SaveFileDialog();
            dia.FileName = "";
            dia.DefaultExt = "rmap";
            dia.AddExtension = true;

            string filename = string.Empty;

            if (dia.ShowDialog() == true)
            {
                filename = dia.FileName;
                Persistent.SaveMap(this.currentMap, filename);
            }
        }

        private void runrouteButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.createdRoute.Count > 1)
            {
                // Return back
                this.AddTravelPoint(new Position(0, 0));

                if (this.OnRouteRun != null)
                {
                    this.OnRouteRun(this.currentMap, new Route(this.createdRoute));
                }
            }
            else
            {
                MessageBox.Show("You have to create a route first!");
            }
        }

        private void cancelrouteButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.OnRouteCancel != null)
            {
                this.OnRouteCancel();
            }
        }
    }
}
