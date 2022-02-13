using System.Collections.Concurrent;
using System.Drawing;

namespace K_MeansLib;

public class KMeansCluster
{
    public readonly ConcurrentBag<Point> Points = new ();
    public Point Center { get; private set; }
    
    public KMeansCluster(Point center)
    {
        Center = center;
    }

    public double CountCenterDifference(Point point)
    {
        return Math.Sqrt(Math.Pow(Center.X - point.X, 2) + Math.Pow(Center.Y - point.Y, 2));
    }

    public bool DefineNewCenter()
    {
        var oldCenter = Center;

        if (Points.Count > 0)
        {
            Center = new Point()
            {
                X = (int)Math.Round(Points.Average(p => p.X)),
                Y = (int)Math.Round(Points.Average(p => p.Y))
            };
        }

        return oldCenter == Center;
    }
}