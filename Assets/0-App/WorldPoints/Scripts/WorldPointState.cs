using System;

namespace MaibornWolff.Waypoints.WorldPoints
{
    [Serializable]
    public class WorldPointState
    {
        public string Id;
        public Type BehaviourType;
        public string VisualComponentResourcePath;
    }
}
