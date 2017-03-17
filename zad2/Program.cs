using System;
using System.Collections.Generic;
using System.Linq;

namespace zad2
{
    class Program
    {
        public Tuple<List<Point>, double> ClosestPoints(List<Point> pointsX, List<Point> pointsY)
        {
            if (pointsX.Count == 2)
            {
                double delta = Distance(pointsX.ElementAt(1), pointsX.ElementAt(0));
                return Tuple.Create(pointsX, delta);
            }
            else if (pointsX.Count == 3)
            {
                double d1, d2, d3;
                List<Point> ret = new List<Point>();
                d1 = Distance(pointsX.ElementAt(1), pointsX.ElementAt(0));
                d2 = Distance(pointsX.ElementAt(2), pointsX.ElementAt(0));
                d3 = Distance(pointsX.ElementAt(2), pointsX.ElementAt(1));
                if (d1 < d2 && d1 < d3)
                {
                    ret.Add(pointsX.ElementAt(1));
                    ret.Add(pointsX.ElementAt(0));
                    return Tuple.Create(ret, d1);
                }
                else if (d2 < d1 && d2 < d3)
                {
                    ret.Add(pointsX.ElementAt(2));
                    ret.Add(pointsX.ElementAt(0));
                    return Tuple.Create(ret, d2);    
                }
                else
                {
                    ret.Add(pointsX.ElementAt(2));
                    ret.Add(pointsX.ElementAt(1));
                    return Tuple.Create(ret, d3);
                
                }
            }
            else
            {
                int half = pointsX.Count/2;
                List<Point> leftX = new List<Point>();
                List<Point> rightX = new List<Point>();
                leftX.AddRange(pointsX.Take(half).ToList());
                rightX.AddRange(pointsX.Skip(half).ToList());
                var last = leftX.Last();
                var leftResult = ClosestPoints(leftX, pointsY);
                var rightResult = ClosestPoints(rightX, pointsY);
                double delta; List<Point> pair = new List<Point>();
                if (leftResult.Item2 < rightResult.Item2)
                {
                    delta = leftResult.Item2;
                    pair = leftResult.Item1;
                }
                else 
                {
                    delta = rightResult.Item2;
                    pair = rightResult.Item1;
                }
                List<Point> leftAndRight = new List<Point>();
                foreach (var p in pointsY)
                {
                    if (Math.Sqrt(Math.Pow((p.x - last.x),2)) < delta)
                    {
                        leftAndRight.Add(p);
                    }
                }
                for (int i = 0; i < leftAndRight.Count - 1; i++)
                {
                    for (int j = i + 1; j < leftAndRight.Count; j++)
                    {
                        var distance = Distance(leftAndRight.ElementAt(i), leftAndRight.ElementAt(j));
                        if (distance < delta)
                        {
                            delta = distance;
                            pair.RemoveRange(0,2);
                            pair.Add(leftAndRight.ElementAt(i));
                            pair.Add(leftAndRight.ElementAt(j));
                        }
                    }
                    
                }
                return Tuple.Create(pair, delta);
            }
        }

        public double Distance (Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow((point2.x - point1.x), 2) + Math.Pow((point2.y - point1.y), 2));
        }
        
        public static List<Point> generate(int count)
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
        static void Main(string[] args)
        {
            Program p = new Program();
            List<Point> listaX = new List<Point>();
            List<Point> listaY = new List<Point>();
            listaX = generate(20);
            listaX = listaX.OrderBy(x => x.x).ThenBy(x => x.y).ToList();
            listaY = listaX.OrderBy(y => y.y).ToList();
            Console.WriteLine("Posortowane wedlug x");
            foreach (var x in listaX)
            {
                Console.WriteLine(x.x + " " + x.y);
            }
            Console.WriteLine("Posortowane wedlug y");
            foreach (var y in listaY)
            {
                Console.WriteLine(y.x + " " + y.y);
            }
            var res = p.ClosestPoints(listaX, listaY);
            Console.WriteLine("WYNIK:");
            Console.WriteLine("Para: (" + res.Item1.ElementAt(0).x + ", " + res.Item1.ElementAt(0).y + ")-(" + res.Item1.ElementAt(1).x + ", " + res.Item1.ElementAt(1).y + 
                ") i odleglosc: " + res.Item2);
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
