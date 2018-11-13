using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeckBehavior : MonoBehaviour {

    CapsuleCollider[] necks;

	// Use this for initialization
	void Start () {
        necks = this.transform.GetComponentsInChildren<CapsuleCollider>();
        for(int i = 1; i<necks.Length-1; i++)
        {
            Physics.IgnoreCollision(necks[i], necks[i + 1]);
            Physics.IgnoreCollision(necks[i], necks[i - 1]);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
