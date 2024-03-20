using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationNode : MonoBehaviour
{
    public NavigationNodeTypeConstant navigationNodeType;
    public NavigationNode[] nextNavigationNodes;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(nextNavigationNodes != null && nextNavigationNodes.Length > 0)
            for (int i = 0; i < nextNavigationNodes.Length; i++)
                if(nextNavigationNodes[i] != null)
                    Gizmos.DrawLine(transform.position, nextNavigationNodes[i].transform.position);
    }
#endif
}
