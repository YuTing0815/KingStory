using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //print(123);
            Knapsack.Instance.StoreItem(5);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
           // print(123);
            Knapsack.Instance.StoreItem(2);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            //print(123);
            Knapsack.Instance.StoreItem(4);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            //print(123);
            Knapsack.Instance.StoreItem(6);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            //print(123);
            Knapsack.Instance.StoreItem(7);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            //print(123);
            if (Knapsack.Instance.targetAlpha == 1)//控制背包的显示和隐藏
                Knapsack.Instance.Hide();
            else { Knapsack.Instance.Show(); }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            //print(123);
            if (Chest.Instance.targetAlpha == 1)//控制背包的显示和隐藏
                Chest.Instance.Hide();
            else { Chest.Instance.Show(); }
        }
    }
}
