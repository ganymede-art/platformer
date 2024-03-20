using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBehaviourMachine<T, U>
    where T : class
    where U : System.Enum
{
    public Dictionary<U, IBehaviour<T, U>> Behaviours { get; }
}
