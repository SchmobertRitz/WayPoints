using System;

namespace MaibornWolff.Waypoints.WorldPoints
{
    [Serializable]
    public class DebugWorldPointState : WorldPointState
    {
        public float rotX, rotY, rotZ, rotW;
        public float posX, posY, posZ;
    }
}
