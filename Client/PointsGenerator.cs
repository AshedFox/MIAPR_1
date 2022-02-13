using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;

namespace Client;

public static class PointsGenerator
{
    public static List<Point> GeneratePoints(int count)
    {
        var points = new List<Point>();

        for (var i = 0; i < count; i++)
        {
            points.Add(new Point(RandomNumberGenerator.GetInt32(0, 100), RandomNumberGenerator.GetInt32(0, 150)));
        }

        return points;
    }
}