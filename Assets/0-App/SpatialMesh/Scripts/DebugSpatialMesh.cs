using MaibornWolff.Waypoints.WorldCursor;
using UnityEngine;

namespace MaibornWolff.Waypoints.SpatialMesh
{
    public class DebugSpatialMesh : CursorTarget
    {
        private ILogger Logger = Debug.logger;

        public override int Priority
        {
            get { return int.MinValue; }
            set { }
        }

        private void Start()
        {
            GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.GetComponent<Renderer>().enabled = false;
            plane.transform.SetParent(transform, false);
        }
    }
}
