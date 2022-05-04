using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script
{
    interface IReplacerController
    {
        string GetReplacement();
        object GetReplacementValue();
    }
}
