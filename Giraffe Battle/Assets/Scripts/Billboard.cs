using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {

    Transform t;
    Transform cam;

	// Use this for initialization
	void Start () {
        t = this.transform;
        cam = GameObject.FindWithTag("MainCamera").transform;
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        Vector3 target = new Vector3(cam.forward.x, t.forward.y, t.TransformDirection(t.up).z);
        if (t.forward != target)
        {
            t.forward = target;
        }
        //t.eulerAngles = new Vector3(0, t.eulerAngles.y, t.eulerAngles.z);
        //t.localEulerAngles = new Vector3(0, t.localEulerAngles.y, t.localEulerAngles.z);
    }
}
