using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class RobotEnemy : MonoBehaviour
{
    [SerializeField] NavMeshAgent navAgent;
    private PlayerCharacter2 player;
    private float searchTime = 1f;
    private float searchTimer = 1f;
    private int health = 3;
    private int damageAmount = 1;
    private float shootTimer = 1.8f;
    private float shootTime = 1.8f;
    private float shootDelayTimer = 0.2f;
    private float shootDelayTime = 0.2f;
    private float shootingTimer = 1f;
    private float shootingTime = 1f;
    private Transform camTransform;
    [SerializeField] private Transform eyeL;
    [SerializeField] private Transform eyeR;
    [SerializeField] private Transform head;
    [SerializeField] private LineRenderer laserL;
    [SerializeField] private LineRenderer laserR;
    private bool shooting = false;
    private bool delayingshoot = false;
    [SerializeField] private bool activated = false;
    private bool isRobertsLevel = false;
    private Vector3 shootpos;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float navStopDistance = 4f;
    [SerializeField] private Animator animator;
    private bool dead = false;
    [SerializeField] private Collider coll;
    [SerializeField] private AudioSource audio;
    private ActivatePortal activatePortalScript;

    private Vector3 lazerPointL = Vector3.zero;
    private float lazerXAngleL = 0;
    private float lazerYAngleL = 0;

    private Vector3 lazerPointR = Vector3.zero;
    private float lazerXAngleR = 0;
    private float lazerYAngleR = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindFirstObjectByType<PlayerCharacter2>();
        camTransform = Camera.main.transform;
        if (SceneManager.GetActiveScene().name == "Robert")
        {
            isRobertsLevel = true;
        }

        navAgent.stoppingDistance = navStopDistance;

        if (activated)
        {
            shootTimer = 1.8f;
            searchTimer = 1f;
            shootDelayTimer = 0.2f;
        }
        else
        {
            shootTimer = 0f;
            searchTimer = 0f;
            shootDelayTimer = 0f;
        }


        activatePortalScript = FindAnyObjectByType<ActivatePortal>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health > 0)
        {
            if (isRobertsLevel && activated)
            {
                FindPath();
                ShootLasers();
                Animations();
            }
        }
        else
        {
            if (!dead)
            {
                dead = true;
                Die();
            }
            else if (dead && laserL.enabled)
            {
                DeadLaserEyes();
            }
        }
    }

    /* private void OnCollisionEnter(Collision collision)
     {
         if (collision.collider.CompareTag("Player"))
         {
             player.TakeDamage(damageAmount);
         }
     }*/

    public void TakeDamage(int damage)
    {
        Debug.Log("Enemy got hit for " + damage + " damage");
        health -= damage;
    }
    public void HeadTakeDamage()
    {
        if (!dead)
        {
            Debug.Log("Headshot!");
            dead = true;
            health = 0;
            DieHeadshot();
        }
    }

    private void FindPath()
    {
        navAgent.SetDestination(player.transform.position);
        /*if (searchTimer < searchTime)
        {
            searchTimer += Time.deltaTime;
        }
        else
        {
            searchTimer = 0f;
            navAgent.SetDestination(player.transform.position);
        }*/
    }

    private void ShootLasers()
    {
        if (shootTimer < shootTime)
        {
            shootTimer += Time.deltaTime;
            if (!shooting)
            {
                shootpos = camTransform.position - Vector3.up * 0.5f;
            }
        }
        else
        {
            if (!delayingshoot)
            {
                RaycastHit hit;
                if (Physics.Raycast(head.transform.position, ((camTransform.position - Vector3.up * 0.5f) - head.transform.position).normalized, out hit, 300f, layerMask, QueryTriggerInteraction.Ignore))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        navAgent.stoppingDistance = navStopDistance;
                        delayingshoot = true;
                        shootpos = camTransform.position - Vector3.up * 0.5f;
                    }
                }
                else
                {
                    //navAgent.stoppingDistance = 0f;
                }
            }
            else
            {
                if (shootDelayTimer < shootDelayTime)
                {
                    shootDelayTimer += Time.deltaTime;
                }
                else
                {
                    audio.pitch = Random.Range(0.3f, 0.4f);
                    audio.Play();
                    shootTimer = 0f;
                    shootingTimer = 0f;
                    shooting = true;
                    delayingshoot = false;
                    shootDelayTimer = 0f;
                    RaycastHit hit2;
                    if (Physics.Raycast(head.transform.position, (shootpos - head.transform.position).normalized, out hit2, 300f, layerMask, QueryTriggerInteraction.Ignore))
                    {
                        if (hit2.collider.CompareTag("Player"))
                        {
                            player.TakeDamage(5);
                        }
                    }
                }
            }
        }

        if (shooting)
        {
            laserL.SetPosition(0, eyeL.position);
            laserR.SetPosition(0, eyeR.position);
            laserL.SetPosition(1, eyeL.position + (shootpos - eyeL.position).normalized * 300);
            laserR.SetPosition(1, eyeR.position + (shootpos - eyeR.position).normalized * 300);
            laserL.enabled = true;
            laserR.enabled = true;

            lazerXAngleL = Vector3.SignedAngle(laserL.GetPosition(1) - laserL.GetPosition(0), laserL.transform.forward, laserL.transform.up);
            lazerYAngleL = Vector3.SignedAngle(laserL.GetPosition(1) - laserL.GetPosition(0), laserL.transform.forward, laserL.transform.right);
            
            lazerXAngleR = Vector3.SignedAngle(laserR.GetPosition(1) - laserR.GetPosition(0), laserR.transform.forward, laserR.transform.up);
            lazerYAngleR = Vector3.SignedAngle(laserR.GetPosition(1) - laserR.GetPosition(0), laserR.transform.forward, laserR.transform.right);

            if (shootingTimer < shootingTime)
            {
                shootingTimer += Time.deltaTime;
            }
            else
            {
                shooting = false;
                laserL.enabled = false;
                laserR.enabled = false;

                audio.Stop();
            }
        }
    }

    public void ActivateRobots()
    {
        activated = true;
    }

    private void LateUpdate()
    {
        if (!dead)
        {
            head.LookAt(head.position + (head.position - shootpos));
        }
    }

    void Die()
    {
        animator.SetTrigger("Dead");
        StartCoroutine(KillLasers());
        coll.enabled = false;
        navAgent.enabled = false;

        if (activatePortalScript != null)
        {
            activatePortalScript.AddPortalPoint();
        }
    }
    void DieHeadshot()
    {

        Debug.Log("7");
        animator.SetTrigger("DeadHeadshot");
        StartCoroutine(KillLasers());
        coll.enabled = false;
        navAgent.enabled = false;

        if (activatePortalScript != null)
        {
            activatePortalScript.AddPortalPoint();
        }
    }

    void Animations()
    {
        animator.SetFloat("Speed", navAgent.velocity.magnitude);
    }

    IEnumerator KillLasers()
    {
        yield return new WaitForSeconds(1);
        laserL.enabled = false;
        laserR.enabled = false;
        audio.Stop();
    }

    void DeadLaserEyes()
    {
        Vector3 thevectorL = Quaternion.AngleAxis(-lazerXAngleL, laserL.transform.up) * laserL.transform.forward;
        thevectorL = Quaternion.AngleAxis(-lazerYAngleL * 0.5f, laserL.transform.right) * thevectorL;
        laserL.SetPosition(1, laserL.transform.position + thevectorL * 300);
        laserL.SetPosition(0, eyeL.position);

        Vector3 thevectorR = Quaternion.AngleAxis(-lazerXAngleR, laserR.transform.up) * laserR.transform.forward;
        thevectorR = Quaternion.AngleAxis(-lazerYAngleR * 0.5f, laserR.transform.right) * thevectorR;
        laserR.SetPosition(1, laserR.transform.position + thevectorR * 300);
        laserR.SetPosition(0, eyeR.position);
    }
}
