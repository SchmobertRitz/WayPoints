using MaibornWolff.Waypoints.Avatar;
using MaibornWolff.Waypoints.WorldCursor;
using MaibornWolff.Waypoints.WorldPoints;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MaibornWolff.Waypoints.Guide
{
    public class GuideManager : IInitializable, ITickable
    {
        private class State
        {
            public virtual State OnEnter(GuideManager manager)
            { return this; }
            public virtual State OnUpdate(GuideManager manager)
            { return this; }
            public virtual State OnUserSaysYes(GuideManager manager)
            { return this; }
            public virtual State OnUserSaysNo(GuideManager manager)
            { return this; }
        }

        private class InitState : State
        {
            public override State OnEnter(GuideManager manager)
            {
                Debug.Log("Hallo");
                manager.avatar = manager.avatarManager.RequestAvatar(manager);
                return this;
            }

            public override State OnUserSaysYes(GuideManager manager)
            { return new GuidanceState(); }
        }

        private class GuidanceState : State
        {
            public override State OnEnter(GuideManager manager)
            {
                if (manager.route == null)
                {
                    manager.route = new Route(manager.worldPointManager.GetWorldPointList());
                }
                return this;
            }
            public override State OnUpdate(GuideManager manager)
            {
                if (manager.route.GetCurrentWorldPoint() == null) {
                    // End of route
                    manager.avatar.Release();
                    return new InitState();
                }
                Transform avatar = manager.avatar.GetAvatarTransform();
                Vector3 worldPointPos = manager.route.GetCurrentWorldPoint().transform.position;
                avatar.rotation = Quaternion.Lerp(avatar.rotation, (Quaternion.LookRotation((worldPointPos - avatar.position).normalized, Vector3.up)), Time.deltaTime * 2);
                avatar.position += (worldPointPos - avatar.position).normalized * Time.deltaTime;
                if (Vector3.Distance(avatar.position, worldPointPos) < 0.25f)
                {
                    WorldPointBehaviour behaviour = manager.route.GetCurrentWorldPoint().GetBehaviour();
                    if (behaviour != null)
                    {
                        behaviour.OnVisitByAvatar(manager.avatarManager.RequestAvatar(manager.route.GetCurrentWorldPoint()));
                        if (manager.avatarManager.GetCurrentAvatarOwner() != manager)
                        {
                            return new InterruptGuidanceState();
                        }
                    }
                    manager.route.Next();
                }
                return this;
            }
        }

        private class InterruptGuidanceState : State
        {
            public override State OnUpdate(GuideManager manager)
            {
                if (manager.avatarManager.GetCurrentAvatarOwner() == manager)
                {
                    manager.route.Next();
                    return new GuidanceState();
                } else
                {
                    return this;
                }
            }
        }

        private ILogger Logger = Debug.logger;

        [Inject]
        private IInteractionSource interactionSource;

        [Inject]
        private WorldPointManager worldPointManager;

        [Inject]
        private AvatarManager avatarManager;

        private State currentState = new State();
        private AvatarRemote avatar;
        private Route route;

        public void Initialize()
        {
            interactionSource.OnYesAction += (_ => HandleResultingState(currentState.OnUserSaysYes(this)));
            interactionSource.OnNoAction += (_ => HandleResultingState(currentState.OnUserSaysNo(this)));
            HandleResultingState(new InitState());
        }

        public void Tick()
        {
            HandleResultingState(currentState.OnUpdate(this));
        }

        private void HandleResultingState(State newState)
        {
            if (newState != currentState)
            {
                currentState = newState;
                HandleResultingState(currentState.OnEnter(this));
            }
        }
    }
}
