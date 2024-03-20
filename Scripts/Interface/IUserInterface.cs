using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserInterface
{
    public UserInterfaceStatus Status { get; }
    public IUserInterfaceWidget[] Widgets { get; }
    public void BeginUserInterface(Dictionary<string, object> args = null);
    public void EndUserInterface();
}
