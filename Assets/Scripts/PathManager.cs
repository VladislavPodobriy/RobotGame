using System.Collections.Generic;
using System.Linq;
using Pixelplacement;
using UnityEngine;

public class PathManager : Singleton<PathManager>
{
    public List<Waypoint> Waypoints;

    public static void UpdateWaypoints()
    {
        Instance.Waypoints = FindObjectsOfType<Waypoint>().ToList();
    }
    
    public static List<Vector2> GetPath(Vector2 origin, Vector2 target)
    {
        var waypoints = GetWaypoints(origin, target);

        var path = waypoints.Select(x => (Vector2)x.transform.position).ToList();
        
        if (waypoints.Any())
        {
            var startWaypoint = waypoints.First();
            var startConnectedWaypoint = startWaypoint.ConnectedWaypoints.Select(x => new 
                {
                    Position = FindNearestPointOnLine(startWaypoint.transform.position, x.transform.position, origin), 
                    ConnectedPoint = x
                })
                .OrderBy(x => Vector2.Distance(x.Position, origin))
                .First();

            if (waypoints.Count > 1 && startConnectedWaypoint.ConnectedPoint == waypoints[1])
                path[0] = startConnectedWaypoint.Position;
            else
                path.Insert(0, startConnectedWaypoint.Position);
            
            var endWaypoint = waypoints.Last();
            var endConnectedWaypoint = endWaypoint.ConnectedWaypoints.Select(x => new 
                {
                    Position = FindNearestPointOnLine(endWaypoint.transform.position, x.transform.position, target), 
                    ConnectedPoint = x
                })
                .OrderBy(x => Vector2.Distance(x.Position, target))
                .First();

            if (waypoints.Count > 1 && endConnectedWaypoint.ConnectedPoint == waypoints[waypoints.Count - 2])
                path[path.Count - 1] = endConnectedWaypoint.Position;
            else
                path.Insert(path.Count, endConnectedWaypoint.Position);
        }
        return path;
    }
    
    private static List<Waypoint> GetWaypoints(Vector2 origin, Vector2 target)
    {
        var targetWaypoint = Instance.Waypoints
            .OrderBy(x => Vector2.Distance(x.transform.position, target))
            .FirstOrDefault();

        var originWaypoint = Instance.Waypoints
            .OrderBy(x => Vector2.Distance(x.transform.position, origin))
            .FirstOrDefault();
        
        if (targetWaypoint == null || originWaypoint == null)
            return new List<Waypoint>();
        
        var waypointsToProcess = new List<Waypoint> {targetWaypoint};
        var visitedWaypoints = new List<Waypoint>();
        var path = new Dictionary<Waypoint, Waypoint>();
        while (waypointsToProcess.Count > 0)
        {
            var waypoint = waypointsToProcess[0];
            waypointsToProcess.Remove(waypoint);
            
            if (waypoint == originWaypoint)
            {
                var result = new List<Waypoint>();
                while (true)
                {
                    result.Add(waypoint);
                    if (waypoint == targetWaypoint)
                        return result;
                    
                    waypoint = path[waypoint];
                }
            }
            
            visitedWaypoints.Add(waypoint);
            foreach (var connectedWaypoint in waypoint.ConnectedWaypoints)
            {
                if (!visitedWaypoints.Contains(connectedWaypoint))
                {
                    waypointsToProcess.Add(connectedWaypoint);
                    path.Add(connectedWaypoint, waypoint);
                }
            }
        }

        return new List<Waypoint>();
    }
    
    private static Vector2 FindNearestPointOnLine(Vector2 origin, Vector2 end, Vector2 point)
    {
        Vector2 heading = (end - origin);
        float magnitudeMax = heading.magnitude;
        heading.Normalize();
        
        Vector2 lhs = point - origin;
        float dotP = Vector2.Dot(lhs, heading);
        dotP = Mathf.Clamp(dotP, 0f, magnitudeMax);
        return origin + heading * dotP;
    }
}
