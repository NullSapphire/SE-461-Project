using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    public float fireDistance;
    public float chargeTimeInSeconds;
    public float rechargeTimeInSeconds;
    private bool foundTarget = false;
    private bool charging = false;
    private GameObject target;
    private Animator animator;
    private float chargeTimeRemaining;
    private float rechargeTimeRemaining;
    private Vector2 lineDest;
    private LineRenderer lr;

    [SerializeField] private Transform rayPosition;
    [SerializeField] private GameObject bullet;
    [SerializeField] private LayerMask viewableLayers;
    [SerializeField] private AudioSource chargingAudioSource;
    [SerializeField] private AudioSource firingAudioSource;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        lr = GetComponent<LineRenderer>();
        chargeTimeRemaining = chargeTimeInSeconds;
        rechargeTimeRemaining = 0.0f;
        lineDest = rayPosition.position;
        lr.SetPosition(0, rayPosition.position);
        lr.enabled = false;
    }
    private void FixedUpdate()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        RaycastHit2D hitTarget = Physics2D.Raycast(rayPosition.position, target.transform.position - rayPosition.transform.position, float.PositiveInfinity, viewableLayers);

        if (hitTarget.collider != null)
        {
            foundTarget = hitTarget.collider.CompareTag("Player");

            if (foundTarget && rechargeTimeRemaining <= 0.0f) //Ray hit player and enemy is not recharging
            {
                if (hitTarget.distance <= fireDistance) //Player is within range
                {
                    if (!charging)
                        chargingAudioSource.Play();
                    animator.SetTrigger("Charging");
                    charging = true;
                    lineDest = target.transform.position;
                }

                if (charging)
                {
                    lr.enabled = true;
                    chargeTimeRemaining -= Time.deltaTime;
                    lr.SetPosition(1, lineDest);
                }
                else
                {
                    lr.enabled = false;
                    chargingAudioSource.Stop();
                }

                if (chargeTimeRemaining <= 0.0f)
                {
                    Fire();
                }
            }
            else //Player left sight or enemy is recharging
            {
                charging = false;
                lr.enabled = false;
                animator.SetTrigger("Fired");
                chargeTimeRemaining = chargeTimeInSeconds;
                rechargeTimeRemaining -= Time.deltaTime;
            }
        }
    }

    private void Fire()
    {
        firingAudioSource.Play();
        lr.enabled = false;
        animator.SetTrigger("Fired");
        chargeTimeRemaining = chargeTimeInSeconds;
        rechargeTimeRemaining = rechargeTimeInSeconds;
        Vector2 LaunchDir = target.transform.position - rayPosition.position;
        GameObject currBullet = Instantiate<GameObject>(bullet, rayPosition.position, rayPosition.rotation);
        currBullet.GetComponent<Bullet>().Init(rayPosition.position, LaunchDir);
    }

}
