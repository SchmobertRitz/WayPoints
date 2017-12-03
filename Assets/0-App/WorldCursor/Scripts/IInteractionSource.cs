using System;

namespace Robs.Waypoints.WorldCursor
{
    public interface IInteractionSource
    {
        Action<IInteractionSource> OnGrabAction
        { get; }

        Action<IInteractionSource> OnUngrabAction
        { get; }

        bool IsGrabbing
        { get; }
    }
}
