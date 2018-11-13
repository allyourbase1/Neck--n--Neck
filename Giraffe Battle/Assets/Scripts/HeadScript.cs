using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(AudioSource))]
public class HeadScript : MonoBehaviour
{
    [SerializeField] Sprite regularFace;
    [SerializeField] Sprite hitFace;
    [SerializeField] float hitDuration = 1.5f;
    [SerializeField] AudioClip[] hurtSFX;

    bool canHit = true;
    SpriteRenderer giraffeFace;

    BoxCollider box;
    Rigidbody rb;
    AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        box = GetComponent<BoxCollider>();
        audioSource = GetComponent<AudioSource>();

        giraffeFace = GetComponentInChildren<SpriteRenderer>();
    }

    public bool CanHit
    {
        get
        {
            return canHit;
        }
    }

    public void DisableHits()
    {
        canHit = false;
        box.size = new Vector3(2, 2, 2);
    }

    public void EnableHits()
    {
        canHit = true;
        box.size = Vector3.one;
    }

    public void EnableHeadRagdoll()
    {
        rb.isKinematic = false;
    }

    public void ShowHitFace()
    {
        StartCoroutine(HitTimer());
    }

    IEnumerator HitTimer()
    {
        audioSource.PlayOneShot(hurtSFX[Random.Range(0, hurtSFX.Length)]);
        giraffeFace.sprite = hitFace;
        yield return new WaitForSeconds(hitDuration);
        giraffeFace.sprite = regularFace;
    }
}
