using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBehaviour<T, U>
    where T : class
    where U : System.Enum
{
    U BehaviourId { get; }

    void BeginBehaviour(T controller, Dictionary<string, object> args = null);
    void UpdateBehaviour(T controller);
    void FixedUpdateBehaviour(T controller);
    void EndBehaviours(T controller);
}
