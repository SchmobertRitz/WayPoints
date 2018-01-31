using System.Collections;
using UnityEngine;


namespace MaibornWolff.Waypoints
{
    public interface ICoroutines
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
        void StopCoroutine(Coroutine coroutine);
    }
}
