using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.script
{
    public interface IPlayerStateController
    {
        PlayerStateType GetStateType();
        void CheckState(PlayerController pc);
        void BeginState(PlayerController pc);
        void UpdateState(PlayerController pc);
        void UpdateStateDragAndFriction(PlayerController pc);
        void UpdateStateSpeed(PlayerController pc);
        void UpdateStateSlide(PlayerController pc);
        void UpdateStateAnimator(PlayerController pc);
        void FinishState(PlayerController pc);
    }
}
