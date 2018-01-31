using System.Collections;
using UnityEngine;
using System;
using Zenject;

namespace MaibornWolff.Waypoints.WorldCursor
{
    public class Cursor : IInitializable
    {
        private ILogger Logger = Debug.logger;

        public bool IsActive
        { get { return cursoredCollider != null; } }

        public CursorTarget CursoredObject
        { get; private set; }

        [Inject(Id = "MainCamera")]
        private Transform mainCam;

        [Inject(Id = "WorldCursor")]
        private Transform worldCursor;

        [Inject]
        private ICoroutines coroutines;

        [Inject]
        private IInteractionSource interactionSource;

        private CursorTarget cursoredCollider;
        private Interactable currentGrabbedObject;
        private Coroutine coroutine;
        private float nextCursorStayNotification = 0;

        public Cursor()
        {
        }

        public void Initialize()
        {
            worldCursor.gameObject.SetActive(false);
            interactionSource.OnGrabAction += OnGrab;
            interactionSource.OnUngrabAction += OnUngrab;
            interactionSource.OnDeleteAction += OnDelete;
            Start();
        }

        private void OnDelete(IInteractionSource obj)
        {
            if (cursoredCollider != null)
            {
                currentGrabbedObject = cursoredCollider.GetComponent<Interactable>();
                if (currentGrabbedObject != null)
                {
                    currentGrabbedObject.Delete();
                }
            }
        }

        private void OnGrab(IInteractionSource obj)
        {
            if (currentGrabbedObject != null)
            {
                currentGrabbedObject.StopGrabbing();
                currentGrabbedObject = null;
            }
            if (cursoredCollider != null)
            {
                currentGrabbedObject = cursoredCollider.GetComponent<Interactable>();
                if (currentGrabbedObject != null)
                {
                    currentGrabbedObject.StartGrabbing(mainCam);
                }
            }
        }

        private void OnUngrab(IInteractionSource obj)
        {
            if (currentGrabbedObject != null)
            {
                currentGrabbedObject.StopGrabbing();
            }
            currentGrabbedObject = null;
        }

        public void Start()
        {
            if (coroutine == null)
            {
                coroutine = coroutines.StartCoroutine(WorldCursorPositionLoop());
            }
        }

        public void Stop()
        {
            if (coroutine != null)
            {
                coroutines.StopCoroutine(coroutine);
                coroutine = null;
            }
        }

        public Vector3? GetPosition()
        {
            if (worldCursor.gameObject.activeSelf)
            {
                return worldCursor.transform.position;
            }
            else
            {
                return null;
            }
        }

        private IEnumerator WorldCursorPositionLoop()
        {
            yield return null;
            RaycastHit[] hitsBuffer = new RaycastHit[256];
            while (true)
            {
                int hitsCount = Physics.RaycastNonAlloc(mainCam.position, mainCam.forward, hitsBuffer);
                if (hitsCount > 0)
                {
                    RaycastHit hit = new RaycastHit();
                    CursorTarget target = null;
                    int highestPrio = int.MinValue;
                    for (int i = hitsCount - 1; i >= 0; --i)
                    {
                        RaycastHit hitCandidate = hitsBuffer[i];
                        CursorTarget targetCandidate = hitCandidate.collider.GetComponentInParent<CursorTarget>();
                        if (targetCandidate == null)
                        {
                            continue;
                        }
                        if (target == null)
                        {
                            target = targetCandidate;
                            highestPrio = target.Priority;
                            hit = hitCandidate;
                        } else
                        {
                            int currentPrio = targetCandidate == null ? 0 : targetCandidate.Priority;
                            if (currentPrio >= highestPrio)
                            {
                                if (currentPrio > highestPrio || hitCandidate.distance < hit.distance)
                                {
                                    hit = hitCandidate;
                                    target = targetCandidate;
                                }
                                highestPrio = currentPrio;
                            }
                        }
                    }
                    if (target == null)
                    {
                        worldCursor.gameObject.SetActive(false);
                        NotifyCursorExit();
                        cursoredCollider = null;
                    } else
                    {

                        worldCursor.gameObject.SetActive(true);
                        worldCursor.transform.position = hit.point;

                        if (target == cursoredCollider)
                        {
                            NotifyCursorStay(hit);
                        }
                        else
                        {
                            NotifyCursorExit();
                            cursoredCollider = target;
                            NotifyCursorEnter(hit);
                        }
                    }
                }
                else
                {
                    worldCursor.gameObject.SetActive(false);
                    NotifyCursorExit();
                    cursoredCollider = null;
                }
                yield return new WaitForSeconds(0.001f);
            }
        }

        private void NotifyCursorEnter(RaycastHit hit)
        {
            if (cursoredCollider != null)
            {
                nextCursorStayNotification = 0;
                foreach (CursorTarget target in cursoredCollider.GetComponents<CursorTarget>())
                {
                    try
                    {
                        target.OnCursorEnter(hit);
                    }
                    catch (Exception e)
                    {
                        Logger.LogError("Cursor", e);
                    }
                }
            }
            else
            {
                cursoredCollider = null;
            }
        }

        private void NotifyCursorStay(RaycastHit hit)
        {
            if (cursoredCollider != null)
            {
                if (Time.time >= nextCursorStayNotification)
                {
                    float interval = float.PositiveInfinity;
                    foreach (CursorTarget target in cursoredCollider.GetComponents<CursorTarget>())
                    {
                        try
                        {
                            interval = Mathf.Min(interval, target.OnCursorStay(hit));
                        }
                        catch (Exception e)
                        {
                            Logger.LogError("Cursor", e);
                        }
                    }
                    nextCursorStayNotification = Time.time + interval;
                }
            }
            else
            {
                cursoredCollider = null;
            }
        }

        private void NotifyCursorExit()
        {
            if (cursoredCollider != null)
            {
                foreach (CursorTarget target in cursoredCollider.GetComponents<CursorTarget>())
                {
                    try
                    {
                        target.OnCursorExit();
                    }
                    catch (Exception e)
                    {
                        Logger.LogError("Cursor", e);
                    }
                }
            }
        }
    }

    /*
    /// <summary>
    /// Analyzes the current world cursor position and determines if this position is a valid anchor position.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns>If there is a valid anchor position.</returns>
    private bool GetPositionForNewWaypoint(out Vector3 pos)
    {
        if (WorldCursor.CursorHitSth && !WorldCursor.CursorHitSpatialCollider)
        {
            // Dont add new waypoints when the user if focusing sth
            pos = Vector3.zero;
            return false;
        }
        if (WorldCursor.CursorHitSpatialCollider) {
            pos = WorldCursor.CursorPosition;
            pos.y = CameraHelper.Stats.groundPos.y;
            return true;
        } else
        {
            var stats = CameraHelper.Stats;
            float beta = Mathf.Deg2Rad * (90 - (Vector3.Angle(Vector3.down, stats.camLookDir)));
            float lookDist = stats.eyeHeight * (Mathf.Cos(beta) / Mathf.Sin(beta));
            pos = stats.groundPos + stats.camLookDirInPlane * lookDist;
            return lookDist < 100 && lookDist > 0;
        }
}
    */
}
