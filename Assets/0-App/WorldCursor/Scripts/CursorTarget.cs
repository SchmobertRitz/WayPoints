using UnityEngine;


namespace MaibornWolff.Waypoints.WorldCursor
{
    public class CursorTarget : MonoBehaviour
    {
        public virtual int Priority { get; set; }

        public virtual void OnCursorEnter(RaycastHit hit) { }
        public virtual float OnCursorStay(RaycastHit hit) { return float.PositiveInfinity; }
        public virtual void OnCursorExit() { }
    }
}
