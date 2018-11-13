using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Range(0, 0.2f), SerializeField] float speedFactorDuration = 0.1f;
    [Range(0, 0.5f), SerializeField] float speedFactorWidth = 0.25f;
    [Range(1, 10), SerializeField] float speedFactorSpeed = 5;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator Shake(float speed)
    {
        float newPos = transform.localPosition.z;
        float startPos = transform.localPosition.z;
        for (float f = 1 + speed * speedFactorDuration; f >= 0; f -= 0.1f)
        {
            newPos += speed * speedFactorSpeed * Time.deltaTime;

            if(newPos > startPos + Mathf.Abs(speed) * speedFactorWidth || newPos < startPos - Mathf.Abs(speed) * speedFactorWidth)
            {
                speed *= -1f;
            }

            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, newPos), Time.deltaTime);

            yield return null;
        }
    }
}
