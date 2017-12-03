using EMP.Wire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Robs.Waypoints
{
    public class WayPoints : MonoBehaviour, ICoroutines
    {
        private ILogger Logger = Debug.logger;
        
        private void Start()
        {
            Wire wire = new Wire();
            wire.RegisterModule(new WorldCursor.Module());
            wire.BindInstance(Camera.main);
            wire.BindInstance("MainCamera", Camera.main.transform);
            wire.BindInstance<ICoroutines>(this);
            wire.Get<WorldCursor.Cursor>().Start();
        }


    }
}
