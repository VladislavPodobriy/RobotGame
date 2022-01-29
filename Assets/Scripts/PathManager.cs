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
        if (waypoints.Count >= 2)
            waypoints[0] = FindNearestPointOnLine(waypoints[0], waypoints[1], origin);
        if (waypoints.Count >= 3)
            waypoints[waypoints.Count - 1] = FindNearestPointOnLine(waypoints[waypoints.Count - 1], waypoints[waypoints.Count - 2], target);
        return waypoints;
    }
    
    private static List<Vector2> GetWaypoints(Vector2 origin, Vector2 target)
    {
        var targetWaypoint = Instance.Waypoints
            .OrderBy(x => Vector2.Distance(x.transform.position, target))
            .FirstOrDefault();

        var originWaypoint = Instance.Waypoints
            .OrderBy(x => Vector2.Distance(x.transform.position, origin))
            .FirstOrDefault();
        
        if (targetWaypoint == null || originWaypoint == null)
            return new List<Vector2>();
        
        var waypointsToProcess = new List<Waypoint> {targetWaypoint};
        var visitedWaypoints = new List<Waypoint>();
        var path = new Dictionary<Waypoint, Waypoint>();
        while (waypointsToProcess.Count > 0)
        {
            var waypoint = waypointsToProcess[0];
            waypointsToProcess.Remove(waypoint);
            
            if (waypoint == originWaypoint)
            {
                var result = new List<Vector2>();
                while (true)
                {
                    result.Add(waypoint.transform.position);
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

        return new List<Vector2>();
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
