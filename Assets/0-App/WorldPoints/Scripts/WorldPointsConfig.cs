using System;
using System.Collections.Generic;
using UnityEngine;

namespace MaibornWolff.Waypoints.WorldPoints
{
    [CreateAssetMenu(fileName = "WorldPointsConfiguration", menuName = "WorldPoints/Config", order = 1)]
    public class WorldPointsConfig : ScriptableObject
    {
        public static WorldPointsConfig Load()
        {
            return Resources.Load<WorldPointsConfig>("WorldPoints/WorldPointsConfiguration");
        }

        [Serializable]
        public class WorldPointType
        {
            public string BehaviourTypeName;
            public string VisualComponentResourcePath;

            public WorldPointState CreateState()
            {
                return new WorldPointState
                {
                    Id = Guid.NewGuid().ToString(),
                    BehaviourType = string.IsNullOrEmpty(BehaviourTypeName) ? null : Type.GetType(BehaviourTypeName),
                    VisualComponentResourcePath = VisualComponentResourcePath
                };
            }
        }

        private ILogger Logger = Debug.logger;

        [SerializeField]
        public List<WorldPointType> WorldPointTypes;
    }
}
