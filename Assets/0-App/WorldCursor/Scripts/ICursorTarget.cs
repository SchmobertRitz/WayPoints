using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Robs.Waypoints.WorldCursor
{
    public interface ICursorTarget
    {
        void OnCursorEnter(RaycastHit hit);
        void OnCursorStay(RaycastHit hit);
        void OnCursorExit();
    }
}
