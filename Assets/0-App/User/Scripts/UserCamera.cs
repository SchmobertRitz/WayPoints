using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MaibornWolff.Waypoints.User
{
    public class UserCamera : IInitializable
    {
        private ILogger Logger = Debug.logger;

        [Inject(Id = WayPoints.Ids.MainCamera)]
        private Camera mainCamera;

        public void Initialize()
        {


        }

        public Vector3 GetUserCameraPosition()
        {
            return mainCamera.transform.position;
        }
        
    }
}
