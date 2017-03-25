using System;
using System.Collections.Generic;
using System.Linq;

namespace zad3
{
    class Program
    {
        public KDTree Build(List<Point> points, int d)
        {
            KDTree kdTree;
            if (points.Count == 1)
            {
                kdTree = new KDTree(points.ElementAt(0), null, null);
            }
            else
            {
                int median = points.Count/2;
                Point medianPoint = points.ElementAt(median);
                List<Point> toSlice = new List<Point>(); //this list will be prepared to slice
                List<Point> left = new List<Point>(); //left and down
                List<Point> right = new List<Point>(); //right and up
                if (d%2 == 0)
                {
                    toSlice = points.OrderBy(x => x.x).ThenBy(x => x.y).ToList();
                    left.AddRange(toSlice.Take(median).ToList());
                    right.AddRange(toSlice.Skip(median).ToList());
                }
                else
                {
                    toSlice =  points.OrderBy(x => x.y).ThenBy(x => x.x).ToList();
                    left.AddRange(toSlice.Take(median).ToList());
                    right.AddRange(toSlice.Skip(median).ToList());
                }
                KDTree leftSon = Build(left, d+1);
                KDTree rightSon = Build(right, d+1);
                kdTree = new KDTree(medianPoint, leftSon, rightSon);
            }
            return kdTree;
        }

        public List<Point> KDTreeQuery(KDTree kdTree, Point p1, Point p2, Point p3, Point p4, int d)
        {
            List<Point> pointsInArea = new List<Point>();
            if (kdTree.left == null && kdTree.right == null) //if kdtree is a leaf
            {
                if (IsPointInArea(kdTree.point, p1, p2, p3, p4))
                {
                    pointsInArea.Add(kdTree.point);
                }
            }
            else
            {
                if (IsPointInArea(kdTree.left.point, p1, p2, p3, p4)) //lewy
                {
                    if (d==4)
                    {
                        pointsInArea.AddRange(ReportSubtree(kdTree.left));
                    }
                    else
                    {
                        pointsInArea.AddRange(KDTreeQuery(kdTree.left, p1, p2, p3, p4, d+1));
                    }
                }

                if (IsPointInArea(kdTree.right.point, p1, p2, p3, p4)) //prawy
                {
                    if (d==4)
                    {
                        pointsInArea.AddRange(ReportSubtree(kdTree.right));
                    }
                    else
                    {
                        pointsInArea.AddRange(KDTreeQuery(kdTree.right, p1, p2, p3, p4, d+1));
                    }
                }
            }
            return pointsInArea;
        }

        public List<Point> ReportSubtree(KDTree kdtree)
        {
            List<Point> pointsInSubtree = new List<Point>();
            if (kdtree.left == null && kdtree.right == null)
            {
                pointsInSubtree.Add(kdtree.point);
            }
            else
            {
                pointsInSubtree.AddRange(ReportSubtree(kdtree.left));
                pointsInSubtree.AddRange(ReportSubtree(kdtree.right));
            }
            return pointsInSubtree;
        }

        public static List<Point> Generate(int count)
        {
            Random random = new Random();
            List<Point> list = new List<Point>();
            for (int i = 0; i < count; i++)
            {
                Point point = new Point(random.Next(-15,30), random.Next(-15,30));
                list.Add(point);
                
            }
            return list;
        }
        public bool IsPointInArea(Point point, Point p1, Point p2, Point p3, Point p4)
        {
            if (point.x > p1.x && point.x > p4.x && point.x < p2.x && point.x < p3.x
                    && point.y < p1.y && point.y > p4.y && point.y < p2.y && point.y > p3.y)
            {
                 return true;
            }
            else
            {
                 return false;
            }
        }

        static void Main(string[] args)
        {
            Program p = new Program();
            List<Point> points = new List<Point>();
            points = Generate(20);
        }
    }

    public class KDTree
    {
        public Point point;
        public KDTree left;
        public KDTree right;
        public KDTree(Point point, KDTree left, KDTree right)
        {
            this.point = point;
            this.right = right;
            this.left = left;
        }
    }

    public class Point
    {
        public double x;
        public double y;

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
