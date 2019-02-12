using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateColorWheel : MonoBehaviour {
    RectTransform rectTransform;
    // Use this for initialization
    void Start () {
        rectTransform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        float horizontal = Input.GetAxis("Axis 3");
        float vertical = Input.GetAxis("Axis 6");

        bool analogStickIsNotDead = Mathf.Abs(horizontal) + Mathf.Abs(vertical) > 1;

        if (analogStickIsNotDead) {
            Vector3 eulerAngles = rectTransform.rotation.eulerAngles;

            float absAtan2 = Mathf.Atan2(horizontal, vertical);
            if (horizontal < 0)
            {
                absAtan2 = 2 * Mathf.PI + absAtan2;
            }

            float absDegree = (absAtan2 * 180 / Mathf.PI) + 180;

            rectTransform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, absDegree);

        }


    }
}
