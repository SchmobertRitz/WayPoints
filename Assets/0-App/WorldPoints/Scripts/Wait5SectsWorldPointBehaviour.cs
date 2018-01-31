using System;
using System.Collections;
using System.Collections.Generic;
using MaibornWolff.Waypoints.Avatar;
using UnityEngine;

namespace MaibornWolff.Waypoints.WorldPoints
{
    public class Wait5SectsWorldPointBehaviour : WorldPointBehaviour
    {
        private ILogger Logger = Debug.logger;

        public override void OnVisitByAvatar(AvatarRemote avatar)
        {
            StartCoroutine(DoWaiting(avatar));
        }

        private IEnumerator DoWaiting(AvatarRemote avatar)
        {
            avatar.MoveTo(transform.position + Vector3.up);
            yield return new WaitForSeconds(4);
            avatar.MoveTo(transform.position);
            yield return new WaitForSeconds(1);
            avatar.Release();
        }
    }
}
