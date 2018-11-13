using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadScaling : MonoBehaviour {

    Transform t;
    float startX;
    [Range(5, 50), SerializeField] float scaleFactor = 17;
    float startScale;

	// Use this for initialization
	void Start () {
        t = this.transform;
        startX = t.position.x;
        startScale = t.localScale.x;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        float toScale = startScale + (t.position.x + startX) / scaleFactor;
        t.localScale = new Vector3(toScale, toScale, toScale);
	}
}
