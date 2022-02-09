using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.script
{
    public interface IPlayerState
    {
        string GetStateType();
        void CheckState(PlayerController pc);
        void BeginState(PlayerController pc, params object[] parameters);
        void UpdateState(PlayerController pc);
        void FixedUpdateState(PlayerController pc);
        void FinishState(PlayerController pc);
    }
}
