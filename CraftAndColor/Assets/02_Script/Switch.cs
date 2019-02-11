using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {
    public GameObject GoalWall;        //　Inspectorから消したいオブジェクトを入れます

    void OnCollisionEnter2D(Collision2D other)
    {         
            GoalWall.gameObject.SetActive(false);    
    }

    void OnCollisionExit2D(Collision2D other)
    {
        GoalWall.gameObject.SetActive(true);   
    }

}
