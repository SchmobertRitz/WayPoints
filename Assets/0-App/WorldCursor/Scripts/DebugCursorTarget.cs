using UnityEngine;

namespace Robs.Waypoints.WorldCursor
{
    public class DebugCursorTarget : MonoBehaviour, ICursorTarget
    {
        private ILogger Logger = Debug.logger;

        public void OnCursorEnter(RaycastHit hit)
        {
            Logger.Log("OnCursorEnter at " + hit.point.ToString());
        }

        public void OnCursorExit()
        {
            Logger.Log("OnCursorExit");
        }

        public void OnCursorStay(RaycastHit hit)
        {
            Logger.Log("OnCursorStay at " + hit.point.ToString());
        }
    }
}
