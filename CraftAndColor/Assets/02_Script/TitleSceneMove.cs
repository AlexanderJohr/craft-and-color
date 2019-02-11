using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("X"))
        {
            SceneManager.LoadScene("Stage1");
        }
    }

}
