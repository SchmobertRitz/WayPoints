using EMP.Animations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaibornWolff.Waypoints.Avatar
{
    public class AvatarRemote
    {
        private ILogger Logger = Debug.logger;

        private AvatarBehaviour avatarBehaviour;
        private Action<AvatarRemote> onAvatarReleasedHandler;
        private object owner;

        public AvatarRemote(AvatarBehaviour avatarBehaviour, object owner, Action<AvatarRemote> onAvatarReleasedHandler)
        {
            this.avatarBehaviour = avatarBehaviour;
            this.onAvatarReleasedHandler = onAvatarReleasedHandler;
            this.owner = owner;
        }

        public void MoveTo(Vector3 worldPosition)
        {
            if (avatarBehaviour != null)
            {
                AnimateThis
                    .With(avatarBehaviour)
                    .CancelByTag(this)
                    .Transform()
                    .Tag(this)
                    .ToPosition(worldPosition)
                    .Start();
            }
        }

        public void LookAt(Vector3 worldPosition)
        {
            if (avatarBehaviour != null)
            {

            }
        }

        public object GetOwner()
        {
            return owner;
        }

        public Transform GetAvatarTransform()
        {
            if (avatarBehaviour == null)
            {
                return null;
            } else
            {
                return avatarBehaviour.transform;
            }
        }

        public void Say(string ttsString)
        {
            if (avatarBehaviour != null)
            {

            }
        }

        public void Say(AudioClip voiceoverClip)
        {
            if (avatarBehaviour != null)
            {

            }
        }

        public void Release()
        {
            if (avatarBehaviour != null)
            {
                avatarBehaviour = null;
                onAvatarReleasedHandler(this);
            }
        }
    }
}
