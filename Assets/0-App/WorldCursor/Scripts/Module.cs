using EMP.Wire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Robs.Waypoints.WorldCursor
{
    public class Module
    {
        [Provides("WorldCursor")]
        [Singleton]
        private GameObject WorldCursor()
        {
            return Object.Instantiate(Resources.Load<GameObject>("[WorldCursor]"));
        }

        [Provides]
        
        private IInteractionSource ProvideInteractionSource(Wire wire)
        {
            return new DebugInteractionSource(wire.Get<ICoroutines>());
        }
    }
}
