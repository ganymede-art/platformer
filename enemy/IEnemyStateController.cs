using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;

namespace Assets.script.enemy
{
    public interface IEnemyStateController
    {
        EnemyStateType GetStateType();
        void CheckState(IEnemyCoreController ec);
        void BeginState(IEnemyCoreController ec);
        void UpdateState(IEnemyCoreController ec);
        void FixedUpdateState(IEnemyCoreController ec);
        void FinishState(IEnemyCoreController ec);
        void UpdateStateAnimator(IEnemyCoreController ec);
    }
}
