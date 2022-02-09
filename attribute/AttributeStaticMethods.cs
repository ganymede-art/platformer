using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.script.attribute
{
    public static class AttributeStaticMethods
    {
        public static Vector3 GetAttributeDamageVector(AttributeDamageData data, GameObject source, GameObject target)
        {
            Vector3 verticalVector = Vector3.zero;
            verticalVector.y = data.verticalForceMultiplier;

            Vector3 horizontalVector = (target.transform.position - source.transform.position).normalized;
            horizontalVector *= data.horizontalForceMultiplier;

            return verticalVector + horizontalVector;
        }
    }
}
