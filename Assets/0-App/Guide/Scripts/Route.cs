using MaibornWolff.Waypoints.WorldPoints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaibornWolff.Waypoints.Guide
{
    public class Route
    {
        private ILogger Logger = Debug.logger;
        private List<WorldPoint> worldPoints = new List<WorldPoint>();

        private int index;

        public Route(List<WorldPoint> worldPoints)
        {
            this.worldPoints.AddRange(worldPoints);
        }

        public WorldPoint GetCurrentWorldPoint()
        {
            return worldPoints.Count > index ? worldPoints[index] : null;
        }

        public void Next()
        {
            index++;
        }
    }
}
