using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Priority_Queue;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MotionPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool placingSquares = false;
        private bool placingStart = false;
        private bool placingEnd = false;
        private bool startPlaced = false;
        private bool endPlaced = false;
        private int MAX_NUM_SQUARES = 3;
        private int numSquaresPlaced = 0;
        private int indexSquare1 = -1;
        private int indexSquare2 = -1;
        private int indexSquare3 = -1;
        private int [,] screenArray = new int[500, 500]; // Either, -1 for edge, 0, or 1
        private Point [,] LastPointArray = new Point[500,500]; // Point at index [x,y] has the x and y of the point that went to it
                                                               // (used for backtracing)
        private Point StartPos = new Point(-1,-1), EndPos = new Point(-1, -1);

        public MainWindow()
        {
            InitializeComponent();
            var graph = new UndirectedGraph<int, Edge<int>>();
            graph.AddVertex(2);
            graph.AddVertex(3);
            graph.AddEdge(new TaggedEdge<int, int>(2, 3, 4));
            var RayGraph = graph.ToArrayUndirectedGraph();
            //Console.WriteLine(graph.ToString());
            foreach (var vertex in RayGraph.Vertices)
                Console.WriteLine(vertex);
            var vertList = RayGraph.Vertices.ToList();
            var x = vertList[0];
            
        }

         private void squaresBox_Checked(object sender, RoutedEventArgs e)
        {
            placingSquares = true;
            startBox.IsChecked = false;
            endBox.IsChecked = false;
            Console.WriteLine("You placing squares is true! You!");
        }

        private void startBox_Checked(object sender, RoutedEventArgs e)
        {
            placingStart = true;
            squaresBox.IsChecked = false;
            endBox.IsChecked = false;
            Console.WriteLine("You placing start You");
        }

        private void endBox_Checked(object sender, RoutedEventArgs e)
        {
            placingEnd = true;
            startBox.IsChecked = false;
            squaresBox.IsChecked = false;
            Console.WriteLine("You placing end You");
        }

        private void endBox_Unchecked(object sender, RoutedEventArgs e)
        {
            placingEnd = false;
        }

        private void squaresBox_Unchecked(object sender, RoutedEventArgs e)
        {
            placingSquares = false;
        }

        private void startBox_Unchecked(object sender, RoutedEventArgs e)
        {
            placingStart = false;
        }

        private void LayoutRoot_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(placingSquares && numSquaresPlaced < MAX_NUM_SQUARES)
            {
                Point p = e.MouseDevice.GetPosition(this);
                numSquaresPlaced++;
                Console.WriteLine("Please click where you want the top left corner of square {0} to be.", numSquaresPlaced);
                Console.WriteLine("Placing the square at ({0}, {1})", p.X, p.Y-21);
                Rectangle rect = new Rectangle();
                rect.Fill = Brushes.Honeydew;
                Canvas.SetTop(rect, p.Y - 21);
                Canvas.SetLeft(rect, p.X);
                double row;
                double col;
                int dim;
                switch(numSquaresPlaced)
                {
                    case 1:
                        dim = 200; // Dont Like Magic Numbers
                        rect.Width = dim;
                        rect.Height = dim;
                        indexSquare1 = LayoutRoot.Children.Count;
                        LayoutRoot.Children.Add(rect);
                        

                        //assign 1's to every cell in our screenArray to represent an obstacle in the array. 0's represent free space
                        for ( row = p.X; row <= dim + p.X; row++)
                        {
                            for ( col = p.Y - 0; col < dim + (p.Y - 0); col++)
                            {
                                if (col >= 500 || col < 0)
                                    continue;
                                else if (row >= 500 || row < 0)
                                    continue;
                                screenArray[(int)Math.Floor(row),(int)Math.Floor(col)] = 1;

                                // Update Left and Right sides with boundary markers (-1's)
                                if (row == p.X || row == p.X + dim)
                                {
                                    if (row > 0 && row == p.X) // Left Boundary Condition Check
                                    {
                                        if (screenArray[(int)Math.Floor(row - 1), (int)Math.Floor(col)] == 0)
                                            screenArray[(int)Math.Floor(row - 1), (int)Math.Floor(col)] = -1;
                                    }
                                    else if (row < 500 && row == p.X + dim) // Right Boundary Check 
                                    {
                                        if (screenArray[(int)Math.Floor(row + 1), (int)Math.Floor(col)] == 0)
                                            screenArray[(int)Math.Floor(row + 1), (int)Math.Floor(col)] = -1;
                                    }
                                }
                                // Update Top and Bottom sides with boundary markers (-1's)
                                if (col == p.Y || col == p.Y + dim)
                                {
                                    if (col > 0 && col == p.Y) // Top Boundary Condition Check
                                    {
                                        if (screenArray[(int)Math.Floor(row), (int)Math.Floor(col - 1)] == 0)
                                            screenArray[(int)Math.Floor(row), (int)Math.Floor(col - 1)] = -1;
                                    }
                                    else if (col < 500 && col == p.Y + dim) // Bottom Boundary Check 
                                    {
                                        if (screenArray[(int)Math.Floor(row), (int)Math.Floor(col + 1)] == 0)
                                            screenArray[(int)Math.Floor(row), (int)Math.Floor(col + 1)] = -1;
                                    }
                                }
                            }
                            Console.WriteLine("cell ({0},{1}) has a 1 in it", row, col);

                        }
                        break;
                    case 2:
                        dim = 150;
                        rect.Width = dim;
                        rect.Height = dim;
                        indexSquare2 = LayoutRoot.Children.Count;
                        LayoutRoot.Children.Add(rect);
                       

                        //assign 1's to every cell in our screenArray to represent an obstacle in the array. 0's represent free space
                        for ( row = p.X; row <= dim + p.X; row++)
                        {
                            for ( col = p.Y - 0; col < dim + (p.Y - 0); col++)
                            {
                                if (col >= 500 || col < 0)
                                    continue;
                                else if (row >= 500 || row < 0)
                                    continue;
                                screenArray[(int)Math.Floor(row), (int)Math.Floor(col)] = 1;

                                // Update Left and Right sides with boundary markers (-1's)
                                if (row == p.X || row == p.X + dim)
                                {
                                    if (row > 0 && row == p.X) // Left Boundary Condition Check
                                    {
                                        if (screenArray[(int)Math.Floor(row - 1), (int)Math.Floor(col)] == 0)
                                            screenArray[(int)Math.Floor(row - 1), (int)Math.Floor(col)] = -1;
                                    }
                                    else if (row < 500 && row == p.X + dim) // Right Boundary Check 
                                    {
                                        if (screenArray[(int)Math.Floor(row + 1), (int)Math.Floor(col)] == 0)
                                            screenArray[(int)Math.Floor(row + 1), (int)Math.Floor(col)] = -1;
                                    }
                                }
                                // Update Top and Bottom sides with boundary markers (-1's)
                                if (col == p.Y || col == p.Y + dim)
                                {
                                    if (col > 0 && col == p.Y) // Top Boundary Condition Check
                                    {
                                        if (screenArray[(int)Math.Floor(row), (int)Math.Floor(col - 1)] == 0)
                                            screenArray[(int)Math.Floor(row), (int)Math.Floor(col - 1)] = -1;
                                    }
                                    else if (col < 500 && col == p.Y + dim) // Bottom Boundary Check 
                                    {
                                        if (screenArray[(int)Math.Floor(row), (int)Math.Floor(col + 1)] == 0)
                                            screenArray[(int)Math.Floor(row), (int)Math.Floor(col + 1)] = -1;
                                    }
                                }
                            }
                            Console.WriteLine("cell ({0},{1}) has a 1 in it", row, col);

                        }
                        break;
                    case 3:
                        dim = 100;
                        rect.Width = dim;
                        rect.Height = dim;
                        indexSquare3 = LayoutRoot.Children.Count;
                        LayoutRoot.Children.Add(rect);
                       
                        //assign 1's to every cell in our screenArray to represent an obstacle in the array. 0's represent free space
                        for ( row = p.X; row <= dim + p.X; row++)
                        {
                            for ( col = p.Y - 0; col < dim + (p.Y - 0); col++)
                            {
                                if (col >= 500 || col < 0)
                                    continue;
                                else if (row >= 500 || row < 0)
                                    continue;
                                screenArray[(int)Math.Floor(row), (int)Math.Floor(col)] = 1;

                                // Update Left and Right sides with boundary markers (-1's)
                                if (row == p.X || row == p.X + dim)
                                {
                                    if (row > 0 && row == p.X) // Left Boundary Condition Check
                                    {
                                        if (screenArray[(int)Math.Floor(row - 1), (int)Math.Floor(col)] == 0)
                                            screenArray[(int)Math.Floor(row - 1), (int)Math.Floor(col)] = -1;
                                    }
                                    else if (row < 500 && row == p.X + dim) // Right Boundary Check 
                                    {
                                        if (screenArray[(int)Math.Floor(row + 1), (int)Math.Floor(col)] == 0)
                                            screenArray[(int)Math.Floor(row + 1), (int)Math.Floor(col)] = -1;
                                    }
                                }
                                // Update Top and Bottom sides with boundary markers (-1's)
                                if (col == p.Y || col == p.Y + dim)
                                {
                                    if (col > 0 && col == p.Y) // Top Boundary Condition Check
                                    {
                                        if (screenArray[(int)Math.Floor(row), (int)Math.Floor(col - 1)] == 0)
                                            screenArray[(int)Math.Floor(row), (int)Math.Floor(col - 1)] = -1;
                                    }
                                    else if (col < 500 && col == p.Y + dim) // Bottom Boundary Check 
                                    {
                                        if (screenArray[(int)Math.Floor(row), (int)Math.Floor(col + 1)] == 0)
                                            screenArray[(int)Math.Floor(row), (int)Math.Floor(col + 1)] = -1;
                                    }
                                }
                            }
                            Console.WriteLine("cell ({0},{1}) has a 1 in it", row, col);
                            
                        }
                        break;
                    default: break;
                }
            }
            else if(placingStart && !startPlaced)
            {
                Point p = e.MouseDevice.GetPosition(this);
                startPlaced = true;
                Ellipse e1 = new Ellipse();
                Canvas.SetTop(e1, p.Y - 21);
                Canvas.SetLeft(e1, p.X);
                Console.WriteLine("Start location at: ({0}, {1})", p.X, p.Y - 21);
                StartPos = new Point(p.X, p.Y);
                e1.Width = 2;
                e1.Height = 2;
                e1.Fill = Brushes.Tomato;
                LayoutRoot.Children.Add(e1);
            }
            else if (placingEnd && !endPlaced)
            {
                Point p = e.MouseDevice.GetPosition(this);
                endPlaced = true;
                Ellipse e1 = new Ellipse();
                Canvas.SetTop(e1, p.Y - 21);
                Canvas.SetLeft(e1, p.X);
                Console.WriteLine("End location at: ({0}, {1})", p.X, p.Y - 21);
                EndPos = new Point(p.X, p.Y);
                e1.Width = 2;
                e1.Height = 2;
                e1.Fill = Brushes.SeaShell;
                LayoutRoot.Children.Add(e1);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Planning...!");
            placingStart = false;
            startBox.IsChecked = false;
            placingSquares = false;
            squaresBox.IsChecked = false;
            endBox.IsChecked = false;
            placingEnd = false;

            if (StartPos.X == -1 || StartPos.Y == -1 || EndPos.X == -1 || EndPos.Y == -1)
            {
                Console.WriteLine("Update Waypoints first!");
                return;
            }
            List<Point> Route = new List<Point>(); // Generate List for Route
            Point currpos = new Point(StartPos.X, StartPos.Y);
            Point oldpos = currpos;
            Point FirstFail = new Point(-1,-1);

            Route = AStar(StartPos, EndPos);

            // Print Final Output
            Console.WriteLine("[");
            foreach (Point P in Route){
                Ellipse e1 = new Ellipse();
                Canvas.SetTop(e1, P.Y - 21);
                Canvas.SetLeft(e1, P.X);
                e1.Width = 2;
                e1.Height = 2;
                e1.Fill = Brushes.GreenYellow;
                LayoutRoot.Children.Add(e1);
                // Print the Point
                Console.WriteLine(P);
            }
            Console.WriteLine("]");
            //return Route;
        }

        ////////////////////////////
        // Navigational Functions //
        ////////////////////////////
        List<Point> AStar(Point SP, Point EP)
        {
            List<Point> Output = new List<Point>();
            List<Point> VistedNodes = new List<Point>();
            SimplePriorityQueue<Point> Frontier = new SimplePriorityQueue<Point>();
            Frontier.Enqueue(SP, 1 + ManhattanDist(SP, EP)); // Add the Start Point to the Priority Queue
            Point TempPoint = new Point();
            Point NextPoint = new Point();
            Point FailPoint = new Point(-1, -1); // For Failure Comparision Purposes

            while (Frontier.Count != 0)
            {
                TempPoint = Frontier.Dequeue();
                VistedNodes.Add(TempPoint);
                // Loop through every direction.
                for (int i = 0; i < 4; ++i)
                {
                    NextPoint = DirectionSeletor(i, TempPoint);
                    if (NextPoint == EP) // Exit Case
                    {
                        // Build Output, then break
                        break;
                    }
                    else if (!Frontier.Contains(NextPoint) && (NextPoint != FailPoint) && (!VistedNodes.Contains(NextPoint)))
                    {
                        // Add member to PQ if not invalid and not in PQ already.
                        Frontier.Enqueue(NextPoint, 1 + ManhattanDist(NextPoint, EP));
                    }
                }
            }

            return Output;
        }

        Point DirectionSeletor(int I, Point CurrPos)
        { // Based on I, go in the appropriate Direction.
            switch (I)
            {
                case 0:
                    return GoUp(CurrPos);
                case 1:
                    return GoLeft(CurrPos);
                case 2:
                    return GoRight(CurrPos);
                case 3:
                    return GoDown(CurrPos);
                default:
                    return new Point(-1, -1);
            }
        }

        Point GoUp(Point Currpos)
        {
            if ((int)Math.Floor(Currpos.Y) - 1 < 500 && (int)Math.Floor(Currpos.Y) - 1 >= 0) // Bounds check
                if (screenArray[(int)Math.Floor(Currpos.X), (int)Math.Floor(Currpos.Y) - 1] == 0) {  // Verify navigable
                    LastPointArray[(int)Math.Floor(Currpos.X), (int)Math.Floor(Currpos.Y) - 1] = Currpos;
                    Point pos = new Point(Currpos.X, Currpos.Y - 1);
                    return pos;
                }
            return new Point(-1,-1);
        }

        Point GoDown(Point Currpos)
        {
            if ((int) Math.Floor(Currpos.Y) + 1 < 500 && (int)Math.Floor(Currpos.Y) + 1 >= 0) // Bounds check
                if (screenArray[(int)Math.Floor(Currpos.X), (int)Math.Floor(Currpos.Y) + 1] == 0) {  // Verify navigable
                    Point pos = new Point(Currpos.X, Currpos.Y + 1);
                    LastPointArray[(int)Math.Floor(Currpos.X), (int)Math.Floor(Currpos.Y) + 1] = Currpos;
                    return pos;
                }
            return new Point(-1, -1);
        }

        Point GoLeft(Point Currpos)
        {
            if ((int) Math.Floor(Currpos.X) - 1 < 500 && (int)Math.Floor(Currpos.X) - 1 >= 0) // Bounds check
                if (screenArray[(int)Math.Floor(Currpos.X) - 1, (int)Math.Floor(Currpos.Y)] == 0) { // Verify navigable
                    Point pos = new Point(Currpos.X - 1, Currpos.Y);
                    LastPointArray[(int)Math.Floor(Currpos.X) - 1, (int)Math.Floor(Currpos.Y)] = Currpos;
                    return pos;
                }
            return new Point(-1, -1);
        }

        Point GoRight(Point Currpos)
        {
            if ((int) Math.Floor(Currpos.X) + 1 < 500 && (int)Math.Floor(Currpos.X) + 1 >= 0) // Bounds check
                if (screenArray[(int)Math.Floor(Currpos.X) + 1, (int)Math.Floor(Currpos.Y)] == 0) { // Verify navigable
                    Point pos = new Point(Currpos.X + 1, Currpos.Y);
                    LastPointArray[(int)Math.Floor(Currpos.X) + 1, (int)Math.Floor(Currpos.Y)] = Currpos;
                    return pos;
                }
            return new Point(-1, -1);
        }

        // Return The Manhattan Distance between two points
        int ManhattanDist(Point currpos, Point endpos){
            return (int)(Math.Abs(currpos.X - endpos.X) + Math.Abs(currpos.Y - endpos.Y));
        }
    }
}
