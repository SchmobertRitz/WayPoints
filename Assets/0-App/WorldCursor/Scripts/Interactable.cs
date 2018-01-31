using EMP.Animations;
using System;
using UnityEngine;


namespace MaibornWolff.Waypoints.WorldCursor
{
    public class Interactable : CursorTarget
    {
        public enum EDeleteMode
        {
            Animated, NotAnimated, Immediate
        }

        public Action<Interactable> OnDelete
        { get; set; }

        private ILogger Logger = Debug.logger;

        private Transform pivot;
        private bool isGrabbed;
        private Vector3 offsetVector;
        private Quaternion offsetRotation;
           
        public virtual void StartGrabbing(Transform pivot)
        {
            isGrabbed = true;
            this.pivot = pivot;
            offsetVector = Quaternion.Inverse(pivot.rotation) * (transform.position - pivot.position);
            offsetRotation = Quaternion.Inverse(pivot.rotation) * transform.rotation;
        }
 
        public virtual void StopGrabbing()
        {
            isGrabbed = false;
            pivot = null;
        }

        public void Delete(EDeleteMode deleteMode = EDeleteMode.Animated)
        {
            if (OnDelete != null)
            {
                try
                {
                    OnDelete(this);
                } catch(Exception e)
                {
                    Logger.LogWarning("Interactable", e);
                }
            }
            if (deleteMode == EDeleteMode.Immediate)
            {
                DestroyImmediate(gameObject);
            }
            else if (deleteMode == EDeleteMode.Animated) {
                DestroyAnimated(gameObject);
            } else
            {
                Destroy(gameObject);
            }
        }

        // FIXME: Delegate to destroyer
        private void DestroyAnimated(GameObject gameObject)
        {
            AnimateThis.With(gameObject.transform)
                .CancelAll()
                .Transform()
                .ToScale(Mathf.Epsilon * Vector3.one)
                .Duration(0.25f)
                .Ease(AnimateThis.EaseOutQuintic)
                .OnEnd(() => GameObject.Destroy(gameObject))
                .Start();
        }

        private void Update()
        {
            if (isGrabbed && pivot)
            {
                MoveGrabbedMe();
            } else if (isGrabbed)
            {
                pivot = null;
                isGrabbed = false;
            }
        }

        private void MoveGrabbedMe()
        {
            transform.position = pivot.position + pivot.rotation * offsetVector;
            transform.rotation = pivot.rotation * offsetRotation;
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }
    }
}
