using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateAction<T, U>
    where T : class
    where U : System.Enum
{
    U StateId { get; }

    void BeginStateAction(T controller, Dictionary<string, object> args = null);
    void UpdateStateAction(T controller);
    void FixedUpdateStateAction(T controller);
    void EndStateAction(T controller);
}
