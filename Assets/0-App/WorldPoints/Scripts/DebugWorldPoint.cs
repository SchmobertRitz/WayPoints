using System;
using UnityEngine;

namespace MaibornWolff.Waypoints.WorldPoints
{
    public class DebugWorldPoint : WorldPoint
    {
        private ILogger Logger = Debug.logger;
        private bool locked;

        public override WorldPointState GetWorldPointState()
        {
            return new DebugWorldPointState
            {
                Id = Id,
                BehaviourType = BehaviourType,
                VisualComponentResourcePath = VisualComponentResourcePath,
                rotX = transform.rotation.x,
                rotY = transform.rotation.y,
                rotZ = transform.rotation.z,
                rotW = transform.rotation.w,
                posX = transform.position.x,
                posY = transform.position.y,
                posZ = transform.position.z
            };
        }
        
        public override void SetWorldPointState(WorldPointState state)
        {
            base.SetWorldPointState(state);
            DebugWorldPointState debugState = state as DebugWorldPointState;
            if (debugState != null)
            {
                transform.rotation = new Quaternion(debugState.rotX, debugState.rotY, debugState.rotZ, debugState.rotW);
                transform.position = new Vector3(debugState.posX, debugState.posY, debugState.posZ);
            }
        }

        public override bool IsLocked()
        {
            return locked;
        }

        public override void SetLocked(bool locked)
        {
            this.locked = locked;
        }
    }
}
