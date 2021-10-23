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
        void CheckState(PlayerController mc);
        void BeginState(PlayerController mc);
        void UpdateState(PlayerController mc);
        void UpdateStateDragAndFriction(PlayerController mc);
        void UpdateStateSpeed(PlayerController mc);
        void UpdateStateSlide(PlayerController mc);
        void UpdateStateAnimator(PlayerController mc);
        void FinishState(PlayerController mc);
    }
}
