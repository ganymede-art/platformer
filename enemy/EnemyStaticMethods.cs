using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.script.enemy
{
    public static class EnemyStaticMethods
    {
        public static EnemyStateType GetRandomNextStateType(EnemyStateType[] next_states)
        {
            int next_state_index = GameMasterController.staticRandom.Next
                (0, next_states.Length);

            return next_states[next_state_index];
        }
    }
}
