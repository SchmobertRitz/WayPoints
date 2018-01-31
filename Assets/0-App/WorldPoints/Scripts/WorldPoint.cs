using MaibornWolff.Waypoints.WorldCursor;
using System;
using UnityEngine;

namespace MaibornWolff.Waypoints.WorldPoints
{
    public abstract class WorldPoint : Interactable
    {
        private ILogger Logger = Debug.logger;

        protected string Id
        { get; set; }

        protected Type BehaviourType
        { get; set; }

        protected string VisualComponentResourcePath
        { get; set; }

        public abstract void SetLocked(bool locked);
        public abstract bool IsLocked();
        public abstract WorldPointState GetWorldPointState();

        public WorldPointBehaviour GetBehaviour()
        {
            return BehaviourType == null ? null : GetComponent(BehaviourType) as WorldPointBehaviour;
        }

        public override void StartGrabbing(Transform pivot)
        {
            SetLocked(false);
            base.StartGrabbing(pivot);
        }

        public override void StopGrabbing()
        {
            base.StopGrabbing();
            SetLocked(true);
        }

        public virtual void SetWorldPointState(WorldPointState state)
        {
            Id = state.Id;
            BehaviourType = state.BehaviourType;
            VisualComponentResourcePath = state.VisualComponentResourcePath;
        }
        
        public void Create(bool createVisualComponent, bool createBehaviour)
        {
            while(transform.childCount != 0)
            { Destroy(transform.GetChild(0)); }
            
            gameObject.name = "[WorldPoint " + Id + "]";

            if (createVisualComponent)
            {
                GameObject resource = Resources.Load<GameObject>(VisualComponentResourcePath);
                GameObject visualComponent = resource != null ? Instantiate(resource) 
                        : new GameObject("[UNABLE TO CREATE '" + Id + "'. REASON: COULD NOT LOAD '" + VisualComponentResourcePath + "']");
                visualComponent.transform.SetParent(transform, false);
                visualComponent.transform.localPosition = Vector3.zero;
                visualComponent.transform.localRotation = Quaternion.identity;
                visualComponent.transform.localScale = Vector3.one;
            }

            if (createBehaviour && BehaviourType != null)
            {
                gameObject.AddComponent(BehaviourType);
            }
        }
    }
}
