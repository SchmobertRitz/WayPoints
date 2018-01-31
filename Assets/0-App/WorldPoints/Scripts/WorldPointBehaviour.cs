using MaibornWolff.Waypoints.Avatar;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaibornWolff.Waypoints.WorldPoints
{
    public abstract class WorldPointBehaviour : MonoBehaviour
    {
        public enum EAvatarBehaviour
        {
            ProceedsGuidance, WaitHere 
        }

        private ILogger Logger = Debug.logger;

        public abstract void OnVisitByAvatar(AvatarRemote avatar);
    }
}
