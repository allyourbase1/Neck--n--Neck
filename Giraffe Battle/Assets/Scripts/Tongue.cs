using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tongue : MonoBehaviour {

    Transform player;
    Vector3 tonguePos;
    [SerializeField] int p1;
    Transform t;
    [SerializeField] Transform rootT;
    [SerializeField] Rigidbody rootRb;
    public Vector3 pos;
    bool isSet = false;

	// Use this for initialization
	void Start () {
        t = this.transform;

        tonguePos = new Vector3(0.3f, -0.55f, p1 * 0.3f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isSet)
        {
            pos = 3 * player.localScale.x * tonguePos;
            Vector3 targetPos = player.position + pos;

            rootT.position = targetPos;
            rootRb.position = rootT.position;

            t.localScale = 3 * player.localScale;
        }
        else
        {
            if (p1 > 0)
            {
                player = GameObject.FindWithTag("Player1").transform.Find("Head").Find("headNeutral_0");
            }
            else
            {
                player = GameObject.FindWithTag("Player2").transform.Find("Head").Find("headNeutral_0");
                Debug.Log("p2 name is " + player.name);
            }
            GetComponentInChildren<FixedJoint>().connectedBody = player.transform.parent.GetComponent<Rigidbody>();
            GetComponentInChildren<FixedJoint>().anchor = player.transform.position;
            if (player != null)
            {
                isSet = true;
            }
        }
    }
}
