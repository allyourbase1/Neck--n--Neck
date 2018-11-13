using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    Transform headObj;
    Transform t;
    Rigidbody rb;
    Rigidbody headRb;
    bool playerOne;
    int p1;

    [Range(1,50), SerializeField] public float speed = 10;
    [SerializeField] float playerBounds = 10f;

    [Range(1, 20), SerializeField] public float neckSpeed = 10f;
    Vector3 neckBase;
    float neckLength;
    float neckSpeedFactor = 2f;

    Ray headDir;
    Quaternion headCenter;
    [Range(90, 170), SerializeField] float maxHeadAngle = 140;
    float curHeadAngle = 0;
    Vector3 headStartAngle;
    float headMoveX = 0;
    float headMoveY = 0;

    Vector3 lastHeadPos;
    Vector3 lastPos;
    public float headVelocity;
    [Range(0.5f, 2f), SerializeField] public float damageFactor;
    [Range(0, 1), SerializeField] float playerVelocityFactor = 0.3f;
    [Range(1, 5), SerializeField] float depthVelocityFactor = 1.5f;

    Transform otherPlayer;
    float bodyLength = 1.5f;

    bool starting = true;
    float bobX = 5;
    float bobY = 0;
    int ix = 1;
    int iy = 1;
    int bobLimit = 15;
    int i = 0;
    [SerializeField] public float startTimer = 1.5f;

    // Use this for initialization
    void Start ()
    {
        t = GetComponent<Transform>();
        headObj = t.Find("Head");
        rb = GetComponent<Rigidbody>();

        neckBase = t.Find("NeckBase").localPosition;

        headDir = new Ray(neckBase, headObj.localPosition - neckBase);
        neckLength = Vector3.Distance(neckBase, headObj.localPosition);
        neckSpeed = neckSpeed / neckSpeedFactor;

        headCenter = Quaternion.FromToRotation(t.up, headDir.direction);
        float angle = Vector3.Angle(headDir.direction, (-1)*t.forward);
          
        headStartAngle = Quaternion.AngleAxis(angle, t.up) * headDir.direction;

        headRb = headObj.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lastHeadPos = headObj.position;
        lastPos = t.position;

        //Move the character
        float inSpeed = 0;
        if (!starting)
        {
            if (playerOne)
            {
                inSpeed = speed * Input.GetAxis("LeftHorizontal1");
                p1 = 1;
            }
            else
            {
                inSpeed = speed * Input.GetAxis("LeftHorizontal2");
                p1 = -1;
            }
        }

        //Constrain movement by setting movement speed to 0 if trying to leave play boundaries or passing through the other player
        if ((inSpeed < 0 && t.position.z <= -playerBounds - inSpeed*Time.deltaTime) || (!playerOne && inSpeed < 0 && t.position.z <= otherPlayer.position.z - p1 * 2 * bodyLength))
        {
            inSpeed = 0;
        }
        if ((inSpeed > 0 && t.position.z >= playerBounds - inSpeed*Time.deltaTime) || (playerOne && inSpeed > 0 && t.position.z >= otherPlayer.position.z - p1 * 2 * bodyLength))
        {
            inSpeed = 0;
        }


        rb.position += new Vector3(0, 0, inSpeed) * Time.deltaTime;

        //Prevent getting knocked off 2D plane, restrict boundaries to play area
        t.position = new Vector3(0, 0, t.position.z);

        headDir.origin = t.position + neckBase;

        //Head Control
        //Implement Quaternion for angles
        Quaternion headQuat = new Quaternion();
        headQuat = Quaternion.FromToRotation(headDir.direction, headDir.direction);
        Quaternion tempQuat = headQuat;

        if (!starting) //Get controller input
        {
            if (playerOne)
            {
                headMoveX = Input.GetAxis("RightHorizontal1") * neckSpeed;
                headMoveY = Input.GetAxis("RightVertical1") * neckSpeed;
            }
            else
            {
                headMoveX = Input.GetAxis("RightHorizontal2") * neckSpeed;
                headMoveY = Input.GetAxis("RightVertical2") * neckSpeed;
            }
        }
        else //starting the game, do the head bob
        {
            if (bobX > bobLimit || bobX < -bobLimit)
                ix *= -1;
            if (bobY > bobLimit || bobY < 1.2f * -bobLimit)
                iy *= -1;

            bobX += ix;
            bobY += iy;

            headMoveX = bobX / 10;
            headMoveY = bobY / 10;

            i++;

            if(i > startTimer / Time.fixedDeltaTime)
            {
                starting = false;
            }
        }

        //apply the calculation
        tempQuat = tempQuat * Quaternion.AngleAxis(headMoveX, t.right);
        tempQuat = tempQuat * Quaternion.AngleAxis(headMoveY, t.forward);

        //This line constrains the head within a cone
        curHeadAngle = Quaternion.Angle(headCenter, tempQuat * Quaternion.FromToRotation(headStartAngle, headObj.localPosition - neckBase));

        //TODO constrain angle different amounts for different axes?
        if (curHeadAngle < maxHeadAngle)
        {
            headQuat = tempQuat;
        }

        //Calculate and place head position
        Vector3 targetDir = headQuat * headObj.localPosition;
        headDir.direction = Vector3.Normalize(targetDir - neckBase);
        headObj.localPosition = neckBase + headDir.direction * neckLength;
        headRb.position = headObj.position;
    }

    void LateUpdate()
    {
        //Use velocity to calculate damage, faster movement is a harder hit!
        //In order to emphasize head movement mechanic, character movement is factored less into this velocity
        headVelocity = ((headObj.position - lastHeadPos) + (t.position - lastPos)*playerVelocityFactor).magnitude / Time.deltaTime;

        //The depth plays a larger factor in damage to encourage more involved gameplay without punishing the more approachable "chop" tactic
        headVelocity += (depthVelocityFactor * 10 * Mathf.Abs(headObj.position.x - lastHeadPos.x));
        headVelocity *= damageFactor;
    }

    public void FindOpponent()
    {
        if (gameObject.tag == "Player1")
        {
            playerOne = true;
            p1 = 1;
            otherPlayer = GameObject.FindGameObjectWithTag("Player2").GetComponent<Transform>();
        }
        else
        {
            playerOne = false;
            p1 = -1;
            otherPlayer = GameObject.FindGameObjectWithTag("Player1").GetComponent<Transform>();
        }
    }
}
