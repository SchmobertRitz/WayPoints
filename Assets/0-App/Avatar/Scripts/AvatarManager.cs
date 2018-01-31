using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaibornWolff.Waypoints.Avatar
{
    public class AvatarManager
    {
        private ILogger Logger = Debug.logger;

        private string avatarResourcePath = "Avatar/[DebugAvatar]";

        private AvatarBehaviour avatarBehaviour;
        private List<AvatarRemote> listOfAvatarAccesses = new List<AvatarRemote>();

        public AvatarRemote RequestAvatar(object owner)
        {
            if (avatarBehaviour == null)
            {
                avatarBehaviour = GameObject.Instantiate(Resources.Load<GameObject>(avatarResourcePath)).GetComponent<AvatarBehaviour>();
            }
            AvatarRemote avatarAccess = new AvatarRemote(avatarBehaviour, owner, OnAvatarReleased);
            listOfAvatarAccesses.Add(avatarAccess);
            return avatarAccess;
        }

        public object GetCurrentAvatarOwner()
        {
            return listOfAvatarAccesses.Count == 0 ? null : listOfAvatarAccesses[listOfAvatarAccesses.Count - 1].GetOwner();
        }

        private void OnAvatarReleased(AvatarRemote releasedAvatar)
        {
            listOfAvatarAccesses.Remove(releasedAvatar);
            CheckAvatarAccesses();
        }

        private void CheckAvatarAccesses()
        {
            if (listOfAvatarAccesses.Count == 0)
            {
                GameObject.Destroy(avatarBehaviour.gameObject);
                avatarBehaviour = null;
            }
        }
    }
}
