using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<T, U>
    where T : class
    where U : System.Enum
{
    U StateId { get; }

    void BeginState(T controller, Dictionary<string, object> args = null);
    void UpdateState(T controller);
    void FixedUpdateState(T controller);
    void EndState(T controller);
}
