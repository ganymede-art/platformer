using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IPersistenceLoadable
{
    public void LoadFromPersistence(PersistenceHighLogic.PersistenceInfo pi);
}