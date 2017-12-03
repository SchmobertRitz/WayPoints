using EMP.Wire;
using System;
using System.Collections;
using UnityEngine;


namespace Robs.Waypoints.WorldCursor
{
    public class DebugInteractionSource : IInteractionSource
    {
        private ILogger Logger = Debug.logger;

        private Action<IInteractionSource> onGrabAction = (_ => { });
        private Action<IInteractionSource> onUngrabAction = (_ => { });

        public Action<IInteractionSource> OnGrabAction
        { get { return onGrabAction; } }

        public Action<IInteractionSource> OnUngrabAction
        { get { return onUngrabAction; } }

        public bool IsGrabbing
        { get; private set; }
        
        [Inject]
        private ICoroutines coroutines;

        [Inject]
        public DebugInteractionSource(ICoroutines coroutines)
        {
            coroutines.StartCoroutine(GrabDetectionLoop());
        }

        private IEnumerator GrabDetectionLoop()
        {
            while (true)
            {
                yield return null;

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (!IsGrabbing)
                    {
                        IsGrabbing = true;
                        FireEvent();
                    }
                } else
                {
                    if (IsGrabbing)
                    {
                        IsGrabbing = false;
                        FireEvent();
                    }
                }
            }
        }

        private void FireEvent()
        {
            if (IsGrabbing)
            {
                try
                {
                    OnGrabAction(this);
                } catch(Exception e)
                {
                    Logger.LogError("DebugInteractionSource", e);
                }
            } else
            {
                try
                {
                    OnUngrabAction(this);
                }
                catch (Exception e)
                {
                    Logger.LogError("DebugInteractionSource", e);
                }
            }
        }
    }
}
