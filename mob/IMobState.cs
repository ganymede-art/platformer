using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IMobState
{
    string GetStateId();
    void BeginState(MobController mc, params object[] parameters);
    void UpdateState(MobController mc);
    void FixedUpdateState(MobController mc);
    void FinishState(MobController mc);
}
