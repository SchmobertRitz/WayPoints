using System;
using UnityEngine;

namespace MaibornWolff.Waypoints.WorldCursor
{
    public class DebugCursorTarget : CursorTarget
    {
        private ILogger Logger = Debug.logger;

        public override void OnCursorEnter(RaycastHit hit)
        {
            Logger.Log("OnCursorEnter at " + hit.point.ToString());
        }

        public override void OnCursorExit()
        {
            Logger.Log("OnCursorExit");
        }

        public override float OnCursorStay(RaycastHit hit)
        {
            Logger.Log("OnCursorStay at " + hit.point.ToString());
            return 1;
        }
    }
}
