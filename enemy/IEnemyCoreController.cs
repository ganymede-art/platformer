using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.script;
using static Assets.script.AttributeDataClasses;

namespace Assets.script.enemy
{
    public interface IEnemyCoreController
    {
        Dictionary<EnemyStateType,IEnemyStateController> GetStates();
        IEnemyStateController GetCurrentState();
        EnemyStateType GetDefaultStateType();
        bool GetDoesStateExist(EnemyStateType enemy_state_type);
        EnemyCoreData GetData();
        void ChangeState(EnemyStateType new_state_type);
        void SetDamaged(AttributeDamageData damageData);
        void UnsetDamaged();
    }
}
