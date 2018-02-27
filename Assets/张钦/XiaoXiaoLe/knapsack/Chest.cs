using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Inventory {
    private static Chest _instance;//继承自Inventory
    public static Chest Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("Canvas/Chestpanel").GetComponent<Chest>();
            }
            return _instance;
        }
    }

}
