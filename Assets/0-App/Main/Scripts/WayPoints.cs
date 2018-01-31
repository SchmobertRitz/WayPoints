using MaibornWolff.Waypoints.Avatar;
using MaibornWolff.Waypoints.Guide;
using MaibornWolff.Waypoints.SpatialMesh;
using MaibornWolff.Waypoints.WorldCursor;
using MaibornWolff.Waypoints.WorldPoints;
using System;
using UnityEngine;
using Zenject;

namespace MaibornWolff.Waypoints
{
    public class WayPoints : MonoInstaller, ICoroutines
    {
        public class Ids
        {
            public const string WorldCursor = "WorldCursor";
            public const string MainCamera = "MainCamera";
            public const string WorldPointType = "WorldPointType";

        }
        private ILogger Logger = Debug.logger;

        public override void InstallBindings()
        {
            Container.BindInstance(Camera.main).AsSingle();
            Container.Bind<Transform>().WithId(Ids.WorldCursor).FromInstance(Instantiate(Resources.Load<GameObject>("[WorldCursor]")).transform);
            Container.Bind<Transform>().WithId(Ids.MainCamera).FromInstance(Camera.main.transform);
            Container.Bind<ICoroutines>().FromInstance(this).AsSingle();
            Container.Bind<IInteractionSource>().To<DebugInteractionSource>().AsSingle();

            Container.Bind<IInitializable>().To<WorldCursor.Cursor>().AsSingle();
            Container.Bind<WorldCursor.Cursor>().AsSingle().NonLazy();

            Container.Bind<Type>().WithId(Ids.WorldPointType).FromInstance(typeof(DebugWorldPoint));
            Container.Bind<IInitializable>().To<WorldPointManager>().AsSingle();
            Container.Bind<WorldPointManager>().AsSingle().NonLazy();

            Container.Bind<AvatarManager>().AsSingle();

            Container.InstantiateComponentOnNewGameObject<DebugSpatialMesh>();

            Container.Bind<GuideManager>().AsSingle().NonLazy();
            Container.Bind<IInitializable>().To<GuideManager>().AsSingle();
            Container.Bind<ITickable>().To<GuideManager>().AsSingle();
        }


    }
}
