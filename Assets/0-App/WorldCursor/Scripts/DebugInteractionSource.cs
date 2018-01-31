using System;
using System.Collections;
using MaibornWolff.Waypoints.WorldPoints;
using UnityEngine;
using Zenject;

namespace MaibornWolff.Waypoints.WorldCursor
{
    public class DebugInteractionSource : IInteractionSource
    {
        private ILogger Logger = Debug.logger;

        public Action<IInteractionSource> OnGrabAction
        { get; set; }

        public Action<IInteractionSource> OnUngrabAction
        { get; set; }

        private bool isGrabbing;

        public Action<WorldPointState, IInteractionSource> OnCreateNewWorldPoint
        { get; set; }

        public Action<IInteractionSource> OnDeleteAction
        { get; set; }

        public Action<IInteractionSource> OnYesAction
        { get; set; }

        public Action<IInteractionSource> OnNoAction
        { get; set; }

        [Inject]
        private ICoroutines coroutines;

        [Inject]
        public DebugInteractionSource(ICoroutines coroutines)
        {
            coroutines.StartCoroutine(InputDetectionLoop());
        }

        private IEnumerator InputDetectionLoop()
        {
            WorldPointsConfig worldPointsConig = WorldPointsConfig.Load();
            while (true)
            {
                yield return null;

                if (Input.GetKey(KeyCode.Space))
                {
                    if (!isGrabbing)
                    {
                        Logger.Log("Grab On");
                        isGrabbing = true;
                        FireGrabEvent();
                    }
                }
                else
                {
                    if (isGrabbing)
                    {
                        Logger.Log("Grab Off");
                        isGrabbing = false;
                        FireGrabEvent();
                    }
                }

                if (Input.GetKeyDown(KeyCode.Delete))
                {
                    FireEvent(OnDeleteAction);
                }
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    FireEvent(OnYesAction);
                }
                if (Input.GetKeyDown(KeyCode.N))
                {
                    FireEvent(OnNoAction);
                }

                if (worldPointsConig != null)
                {
                    int max = Math.Min(8, worldPointsConig.WorldPointTypes.Count);
                    for (int i = 0; i < max; ++i)
                    {
                        if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                        {
                            FireWorldPointEvent(worldPointsConig.WorldPointTypes[i].CreateState());
                        }
                    }
                }

            }
        }

        private void FireEvent(Action<IInteractionSource> action)
        {
            if (action != null)
            {
                try
                {
                    action(this);
                }
                catch (Exception e)
                {
                    Logger.LogError("DebugInteractionSource", e);
                }
            }
        }

        private void FireWorldPointEvent(WorldPointState worldPointState)
        {
            try
            {
                OnCreateNewWorldPoint(worldPointState, this);
            }
            catch (Exception e)
            {
                Logger.LogError("DebugInteractionSource", e);
            }
        }

        private void FireGrabEvent()
        {
            if (isGrabbing)
            {
                try
                {
                    OnGrabAction(this);
                }
                catch (Exception e)
                {
                    Logger.LogError("DebugInteractionSource", e);
                }
            }
            else
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
