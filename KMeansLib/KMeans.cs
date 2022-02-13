using System.Collections.Concurrent;
using System.Drawing;
using System.Security.Cryptography;

namespace K_MeansLib;

public class KMeans
{
    private int _clustersCount;
    private ConcurrentBag<Point> _points = new ();
    public readonly ConcurrentBag<KMeansCluster> Clusters = new();
    private int _iterationsCount;

    public KMeans()
    {
        Init(0, new List<Point>());
    }
    
    public void Init(int clustersCount, List<Point> points)
    {
        _clustersCount = clustersCount;
        _points = new ConcurrentBag<Point>(points);
        _iterationsCount = 0;
        InitClusters();
    }

    public bool Iterate()
    {
        if (_iterationsCount > 0)
        {
            var isEnd = true;
            
            foreach (var cluster in Clusters)
            {
                if (!cluster.DefineNewCenter())
                {
                    isEnd = false;
                }
            }

            if (isEnd)
            {
                return true;
            }
            else
            {
                foreach (var cluster in Clusters)
                {
                    cluster.Points.Clear();
                }
            }
        }

        Parallel.ForEach(_points, DefinePointCluster);

        _iterationsCount++;
        
        return false;
    }

    private void DefinePointCluster(Point point)
    {
        var minDistance = double.MaxValue;
        KMeansCluster? minCluster = null;
        
        foreach (var cluster in Clusters)
        {
            var currentDistance = cluster.CountCenterDifference(point);

            if (currentDistance < minDistance)
            {
                (minCluster, minDistance) = (cluster, currentDistance);
            }
        }

        minCluster?.Points.Add(point);
    }

    private void InitClusters()
    {
        Clusters.Clear();

        var occupiedPoints = new List<Point>();
        
        for (var i = 0; i < _clustersCount; i++)
        {
            occupiedPoints.Add(IdentifyBaseClusterCenter(occupiedPoints));
            Clusters.Add(new KMeansCluster(occupiedPoints.Last()));
        }
    }

    private Point IdentifyBaseClusterCenter(List<Point> occupiedPoints)
    {
        Point center;
        do
        {
            center = _points.ToList()[RandomNumberGenerator.GetInt32(_points.Count)];
        } while (occupiedPoints.Contains(center));

        return center;
    }
}