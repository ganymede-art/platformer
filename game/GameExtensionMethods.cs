using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script
{
    public static class GameExtensionMethods
    {
        public static void ResetAllAnimatorTriggers(this Animator animator)
        {
            foreach (var trigger in animator.parameters)
            {
                if (trigger.type == AnimatorControllerParameterType.Trigger)
                {
                    animator.ResetTrigger(trigger.name);
                }
            }
        }
    }
}
