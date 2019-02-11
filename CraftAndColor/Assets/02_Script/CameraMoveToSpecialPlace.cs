using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveToSpecialPlace : MonoBehaviour {

    public Camera view;

    private Camera mainCamera;
    private CameraMove movingCamera;


    void Start () {
        mainCamera = Camera.main;
        movingCamera = mainCamera.GetComponent<CameraMove>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    void OnTriggerEnter2D(Collider2D other) {
        if(movingCamera != null)
        movingCamera.DesiredView = view;
        print("enter: " + gameObject.name);

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (movingCamera.DesiredView == view) {
            movingCamera.DesiredView = mainCamera;
            print("exit: " + gameObject.name);
        }
    }
}
