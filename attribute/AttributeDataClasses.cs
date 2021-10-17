using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.script
{
    public class AttributeDataClasses
    {
        [System.Serializable]
        public class AttributeDamageData
        {
            public int damageAmount;
            public bool isDamageInstant;
            public float horizontalForceMultiplier;
            public float verticalForceMultiplier;
            public AudioClip damageSound;

            public static AttributeDamageData GetDefault()
            {
                var def = new AttributeDamageData();
                def.damageAmount = 1;
                def.isDamageInstant = false;
                def.horizontalForceMultiplier = 3f;
                def.verticalForceMultiplier = 3f;
                def.damageSound = null;
                return def;
            }
        }
    }
}
