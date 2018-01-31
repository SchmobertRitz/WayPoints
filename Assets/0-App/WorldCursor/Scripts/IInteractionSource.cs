using MaibornWolff.Waypoints.WorldPoints;
using System;

namespace MaibornWolff.Waypoints.WorldCursor
{
    public interface IInteractionSource
    {
        Action<IInteractionSource> OnGrabAction
        { get; set; }

        Action<IInteractionSource> OnUngrabAction
        { get; set; }

        Action<IInteractionSource> OnDeleteAction
        { get; set; }

        Action<IInteractionSource> OnYesAction
        { get; set; }

        Action<IInteractionSource> OnNoAction
        { get; set; }

        Action<WorldPointState, IInteractionSource> OnCreateNewWorldPoint
        { get; set; }

    }
}
