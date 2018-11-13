using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class GiraffeHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    float currenHealth;

    // Minimum percent of top speed needed to inflict damage
    [SerializeField] [Range(0, .49f)] float minSpeedPercent = .15f;
    // Percentages needed to determine audio clips used
    [SerializeField] [Range(.5f, .74f)] float medSpeedPercent = .5f;
    [SerializeField] [Range(.75f, 1)] float heavySpeedPercent = .8f;

    [SerializeField] float ragdollBaseForce = 100f;

    AudioSource audioSource;
    [SerializeField] AudioClip[] heavyHitsSFX;
    [SerializeField] AudioClip[] mediumHitsSFX;
    [SerializeField] AudioClip[] lightHitsSFX;
    [SerializeField] AudioClip defeatedSFX;

    [SerializeField] private Slider healthBar;

    [SerializeField] HeadScript giraffeHead;
    [SerializeField] float maxDamage = 20f;
    // Just guess on this I suppose
    float maxSpeed = 28f;
    GameManager gameManager;

    [Range(1, 10), SerializeField] float healthBarSpeed;

    PlayerControl pc;
    Rigidbody headRb;

    CameraShake shakeCam;
     
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currenHealth = maxHealth;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        healthBar = gameObject.tag == "Player1" ? GameObject.Find("PlayerOneHealthSlider").GetComponent<Slider>() : GameObject.Find("PlayerTwoHealthSlider").GetComponent<Slider>();
        healthBar.value = 1;

        pc = this.transform.parent.GetComponent<PlayerControl>();
        headRb = this.transform.parent.Find("Head").GetComponent<Rigidbody>();

        shakeCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
    }

    private void OnGUI()
    {
        healthBar.value = Mathf.SmoothStep(healthBar.value, currenHealth / maxHealth, Time.deltaTime*healthBarSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Determine if body has made contact with head or neck
        if (other.gameObject.GetComponent<HeadScript>() != null)
        {
            HeadScript head = other.GetComponent<HeadScript>();

            if (head.CanHit == true && other.tag != gameObject.tag)
            {
                giraffeHead.ShowHitFace();

                head.DisableHits();
                // Find the head and obtain its speed.
                float speed = other.transform.parent.GetComponent<PlayerControl>().headVelocity;

                // If speed of head is greater than mininmum percent of top speed, calculate damage
                if (speed > maxSpeed * minSpeedPercent)
                {
                    float damagePercentage = speed / maxSpeed;
                    currenHealth -= maxDamage * damagePercentage;
                    CheckHealth(speed, damagePercentage);
                }
                StartCoroutine(shakeCam.Shake(Mathf.Abs(speed)));
            }
        }
    }
    private void CheckHealth(float hitSpeed, float damagePercentage)
    {

        if (currenHealth > 0)
        {
            // Play hit animation if we have it. Possibly have knockback?
            PlayHitAudio(damagePercentage);
        }
        else
        {
            PlayDefeat(hitSpeed);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<HeadScript>() != null)
        {
            other.gameObject.GetComponent<HeadScript>().EnableHits();
        }
    }

    private void PlayDefeat(float speed)
    {
        audioSource.clip = defeatedSFX;
        audioSource.Play();

        pc.enabled = false;
        giraffeHead.EnableHeadRagdoll();
        headRb.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
        this.transform.parent.GetComponent<Rigidbody>().isKinematic = false;
        this.transform.parent.GetComponent<Rigidbody>().useGravity = true;
        this.transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX;
        GetComponent<Rigidbody>().useGravity = true;
        headRb.useGravity = true;
        EnableBodyRagdoll();
        this.transform.parent.GetComponent<Rigidbody>().AddForce(new Vector3(0, Mathf.Pow(speed, 3) * ragdollBaseForce, Mathf.Pow(speed, 3) * ragdollBaseForce), ForceMode.Impulse);
        headRb.AddForce(new Vector3(0, Mathf.Pow(speed, 3) * ragdollBaseForce, Mathf.Pow(speed, 3) * ragdollBaseForce), ForceMode.Impulse);
        GetComponent<Rigidbody>().AddForce(new Vector3(0, Mathf.Pow(speed, 3) * ragdollBaseForce, Mathf.Pow(speed, 3) * ragdollBaseForce), ForceMode.Impulse);
        gameManager.AddWin(gameObject.tag);
        //this.enabled = false;
    }

    private void EnableBodyRagdoll()
    {
        Rigidbody rb = gameObject.GetComponentInParent<Rigidbody>();
        rb.isKinematic = false;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }

    private void PlayHitAudio(float damagePercentage)
    {
        if (damagePercentage > heavySpeedPercent)
        {
            audioSource.clip = heavyHitsSFX[Random.Range(0, heavyHitsSFX.Length)];
        }
        else if (damagePercentage > medSpeedPercent)
        {
            audioSource.clip = mediumHitsSFX[Random.Range(0, mediumHitsSFX.Length)];
        }
        else
        {
            audioSource.clip = lightHitsSFX[Random.Range(0, lightHitsSFX.Length)];
        }

        audioSource.Play();
    }
}
