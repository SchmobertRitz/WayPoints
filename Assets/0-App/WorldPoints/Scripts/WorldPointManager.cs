using MaibornWolff.Waypoints.WorldCursor;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MaibornWolff.Waypoints.WorldPoints
{
    public class WorldPointManager : IInitializable
    {
        private List<WorldPoint> allWorldPoints = new List<WorldPoint>();

        [Inject]
        private WorldCursor.Cursor cursor;

        [Inject]
        private IInteractionSource interactionSource;

        [Inject(Id = WayPoints.Ids.WorldPointType)]
        private Type worldPointType;

        private ILogger Logger = Debug.logger;

        public void Initialize()
        {
            interactionSource.OnCreateNewWorldPoint += OnCreateNewWorldPoint;
        }

        public List<WorldPoint> GetWorldPointList()
        {
            return new List<WorldPoint>(allWorldPoints);
        }

        private void OnCreateNewWorldPoint(WorldPointState state, IInteractionSource source)
        {
            Vector3? pos = cursor.GetPosition();
            if (pos.HasValue)
            {
                GameObject newGameObject = new GameObject("[WorldPoint "+pos.ToString()+"]");
                newGameObject.transform.position = cursor.GetPosition().Value;
                WorldPoint worldPoint = newGameObject.AddComponent(worldPointType) as WorldPoint;
                if (worldPoint != null)
                {
                    worldPoint.SetWorldPointState(state);
                    worldPoint.Create(true, true);
                    
                    allWorldPoints.Add(worldPoint);
                    worldPoint.OnDelete += OnDeleteWorldPoint;
                } else
                {
                    Logger.LogError("WorldPointManager", "Unable to reate WorldPoint component of type " + worldPointType);
                }
            }
        }

        private void OnDeleteWorldPoint(Interactable obj)
        {
            allWorldPoints.Remove((WorldPoint)obj);
        }
    }
}
