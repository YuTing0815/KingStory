using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class Lead : MonoBehaviour{


    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag== "Dian(0)")
        {
            Debug.Log("切换场景0");
        }
        if (other.gameObject.tag == "Dian(1)")
        {
            Debug.Log("切换场景1");
        }
        if (other.gameObject.tag == "Dian(2)")
        {
            Debug.Log("切换场景2");
        }
        if (other.gameObject.tag == "Dian(3)")
        {
            Debug.Log("切换场景3");
        }
        if (other.gameObject.tag == "Dian(4)")
        {
            Debug.Log("切换场景4");
        }
        if (other.gameObject.tag == "Dian(5)")
        {
            Debug.Log("切换场景5");
        }
        if (other.gameObject.tag == "Dian(6)")
        {
            Debug.Log("切换场景6");
        }
        if (other.gameObject.tag == "Dian(7)")
        {
            Debug.Log("切换场景7");
        }
        if (other.gameObject.tag == "Dian(8)")
        {
            Debug.Log("切换场景8");
        }
        if (other.gameObject.tag == "Dian(9)")
        {
            Debug.Log("切换场景9");
        }
        if (other.gameObject.tag == "Dian(10)")
        {
            Debug.Log("切换场景10");
        }
    }
}
