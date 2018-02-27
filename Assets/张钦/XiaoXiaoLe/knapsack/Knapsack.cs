using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knapsack : Inventory {

    private static Knapsack _instance;//继承自Inventory
    public static Knapsack Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("Canvas/knapsack").GetComponent<Knapsack>();
            }
            return _instance;
        }
    }
}
