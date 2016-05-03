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
            Console.WriteLine("You placing squares is true! You!");
        }

        private void startBox_Checked(object sender, RoutedEventArgs e)
        {
            placingStart = true;
            Console.WriteLine("You placing start You");
        }

        private void endBox_Checked(object sender, RoutedEventArgs e)
        {
            placingEnd = true;
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
            //first we need to partition the canvas into cells
            //next we need to put those partitions into a graph as vertices
            //then we need to add additional vertices on the boundaries
            //then we need to sort the vertices
            //then connect them via straight lines
            //then run SSSP
            //also check to make sure we've placed all the shit we need
            //do other shit too
            //like actually do the planning :P
            /*
            for doing the partitions, keep a sorted list of all vertices, sorted on their x coordinates.
                for all verts with same x
                    find vertices with min y and max y
                    "draw line" from min to top and max to bottom, checking for intersections with other objects
                    if there is an intersection, draw the line up/down to the intersection and make that be the newest partition
                    Need to figure out still how to concretely represent these "partitions"
                    maybe keep them as rectangle objects?
                    At any rate...
            */

            // TLDR: My method lets us just run a search on the graph
            // Handle Unallocated Case
            if (StartPos.X == -1 || StartPos.Y == -1 || EndPos.X == -1 || EndPos.Y == -1)
            {
                Console.WriteLine("Update Waypoints first!");
                return;
            }
            List<Point> Route = new List<Point>(); // Generate List for Route
            Point currpos = new Point(StartPos.X, StartPos.Y);
            Point oldpos = currpos;
            Point FirstFail = new Point(-1,-1);
            int climbX = 0; // -1 for left, 0 for no direction, 1 for right
            int climbY = 0; // -1 for down, 0 for no direction, 1 for up

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
        bool GoUp(ref Point Currpos)
        {
            if ((int)Math.Floor(Currpos.Y) - 1 < 500 && (int)Math.Floor(Currpos.Y) - 1 >= 0) // Bounds check
                if (screenArray[(int)Math.Floor(Currpos.X), (int)Math.Floor(Currpos.Y) - 1] == 0) {  // Verify navigable
                    Currpos = new Point(Currpos.X, Currpos.Y - 1);
                    screenArray[(int)Math.Floor(Currpos.X), (int)Math.Floor(Currpos.Y) - 1] = 2;
                    return true;
                }
            return false;
        }

        List<Point> AStar(Point SP, Point EP)
        {
            List<Point> Output = new List<Point>();
            SimplePriorityQueue<Point> Frontier = new SimplePriorityQueue<Point>();
            Frontier.Enqueue(SP, ManhattanDist(SP, EP)); // Add the Start Point to the Priority Queue
            Point TempPoint = new Point();

            while (Frontier.Count != 0)
            {
                TempPoint = Frontier.Dequeue();

            }

            return Output;
        }

        bool GoDown(ref Point Currpos)
        {
            if ((int) Math.Floor(Currpos.Y) + 1 < 500 && (int)Math.Floor(Currpos.Y) + 1 >= 0) // Bounds check
                if (screenArray[(int)Math.Floor(Currpos.X), (int)Math.Floor(Currpos.Y) + 1] == 0) {  // Verify navigable
                    Currpos = new Point(Currpos.X, Currpos.Y + 1);
                    screenArray[(int)Math.Floor(Currpos.X), (int)Math.Floor(Currpos.Y) + 1] = 2; // Invalidate the point for further travel
                    return true;
                }
            return false;
        }

        bool GoLeft(ref Point Currpos)
        {
            if ((int) Math.Floor(Currpos.X) - 1 < 500 && (int)Math.Floor(Currpos.X) - 1 >= 0) // Bounds check
                if (screenArray[(int)Math.Floor(Currpos.X) - 1, (int)Math.Floor(Currpos.Y)] == 0) { // Verify navigable
                    Currpos = new Point(Currpos.X - 1, Currpos.Y);
                    screenArray[(int)Math.Floor(Currpos.X) - 1, (int)Math.Floor(Currpos.Y)] = 2;
                    return true;
                }
            return false;
        }

        bool GoRight(ref Point Currpos)
        {
            if ((int) Math.Floor(Currpos.X) + 1 < 500 && (int)Math.Floor(Currpos.X) + 1 >= 0) // Bounds check
                if (screenArray[(int)Math.Floor(Currpos.X) + 1, (int)Math.Floor(Currpos.Y)] == 0) { // Verify navigable
                    Currpos = new Point(Currpos.X + 1, Currpos.Y);
                    screenArray[(int)Math.Floor(Currpos.X) + 1, (int)Math.Floor(Currpos.Y)] = 2;
                    return true;
                }
            return false;
        }

        // Return The Manhattan Distance between two points
        int ManhattanDist(Point currpos, Point endpos){
            return (int)(Math.Abs(currpos.X - endpos.X) + Math.Abs(currpos.Y - endpos.Y));
        }
    }
}
