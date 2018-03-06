using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class SaveIndexMgr : Singleton<SaveIndexMgr>
{
    public int CurindexMapOne=-1;
    public void SaveMapOne(int index)
    {
        CurindexMapOne = -1;
        CurindexMapOne = index;
    }
    public int CurindexMapTwo= -1;
    public void SaveMapTwo(int index)
    {
        CurindexMapTwo = -1;
        CurindexMapTwo = index;
    }
    public int CurindexMapThree = -1;
    public void SaveMapThree(int index)
    {
        CurindexMapThree = -1;
        CurindexMapThree = index;
    }
    public int CurindexMapFour = -1;
    public void SaveMapFour(int index)
    {
        CurindexMapFour = -1;
        CurindexMapFour = index;
    }
    public int SaveSceneId { set; get; }
}

