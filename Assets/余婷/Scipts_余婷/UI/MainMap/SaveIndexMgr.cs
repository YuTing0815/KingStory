using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class SaveIndexMgr : Singleton<SaveIndexMgr>
{
    public int Curindex=-1;
    public void Save(int index)
    {
        Curindex = -1;
        Curindex = index;
    }
}

