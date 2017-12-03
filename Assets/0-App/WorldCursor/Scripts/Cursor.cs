using EMP.Wire;
using System.Collections;
using UnityEngine;
using System;

namespace Robs.Waypoints.WorldCursor
{
    public class Cursor
    {
        private ILogger Logger = Debug.logger;

        public bool IsActive
        { get { return cursoredCollider != null; } }

        public GameObject CursoredObject
        { get { return cursoredCollider != null ? cursoredCollider.gameObject : null; } }

        [Inject]
        private Wire wire;

        [Inject("MainCamera")]
        private Transform mainCam;

        [Inject("WorldCursor")]
        private GameObject worldCursor;

        [Inject]
        private ICoroutines coroutines;

        private Collider cursoredCollider;
        private Coroutine coroutine;

        [Inject]
        public Cursor([Named("WorldCursor")] GameObject worldCursor)
        {
            worldCursor.SetActive(false);
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
            if (worldCursor.activeSelf)
            {
                return worldCursor.transform.position;
            } else
            {
                return null;
            }
        }

        private IEnumerator WorldCursorPositionLoop()
        {
            yield return null;
            RaycastHit hit;
            while(true)
            {
                if (Physics.Raycast(mainCam.position, mainCam.forward, out hit)) {
                    worldCursor.SetActive(true);
                    worldCursor.transform.position = hit.point;
                    ICursorTarget currentTarget = hit.collider.GetComponentInChildren<ICursorTarget>();
                    if (hit.collider == cursoredCollider)
                    {
                        NotifyCursorStay(hit);
                    } else
                    {
                        NotifyCursorExit();
                        cursoredCollider = hit.collider;
                        NotifyCursorEnter(hit);
                    }
                } else
                {
                    worldCursor.SetActive(false);
                    NotifyCursorExit();
                    cursoredCollider = null;
                }
                yield return new WaitForSeconds(0.001f);
            }
        }

        private void NotifyCursorEnter(RaycastHit hit)
        {
            if (cursoredCollider)
            {
                foreach (ICursorTarget target in cursoredCollider.gameObject.GetComponentsInChildren<ICursorTarget>())
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
            if (cursoredCollider)
            {
                foreach (ICursorTarget target in cursoredCollider.gameObject.GetComponentsInChildren<ICursorTarget>())
                {
                    try
                    {
                        target.OnCursorStay(hit);
                    } catch(Exception e)
                    {
                        Logger.LogError("Cursor", e);
                    }
                }
            } else
            {
                cursoredCollider = null;
            }
        }

        private void NotifyCursorExit()
        {
            if (cursoredCollider)
            {
                foreach (ICursorTarget target in cursoredCollider.gameObject.GetComponentsInChildren<ICursorTarget>())
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
}
