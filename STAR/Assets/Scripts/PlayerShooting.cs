using UnityEngine;
using System.Collections;
using static UnityEngine.GraphicsBuffer;

public class PlayerShooting : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] int gunDamage = 1;
    [SerializeField] float fireRate = 0.25f;
    float nextFire;
    [SerializeField] float range = 60;
    [SerializeField] float hitForce = 150;

    [Header("Camera and Position")]
    [SerializeField] Transform gunEnd;
    Camera playerCam;


    [Header("Weapon Shooting Visuals")]
    [SerializeField] float shotDuration = 0.5f;
    [SerializeField] ParticleSystem shootingPS;

    [Header("Bullet")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private float maxBulletDist = 100f;

    //Ref in Start
    AudioSource _as;
    LineRenderer _lr;


    private float lazerXAngle = 0;
    private float lazerYAngle = 0;
    private float lazerLength = 0;

    Vector3 bulletStartPoint;
    Vector3 bulletEndPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _lr = GetComponent<LineRenderer>();
        _as = GetComponent<AudioSource>();
        playerCam = Camera.main;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            

            Vector3 rayOrigin = playerCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));

            bulletStartPoint = rayOrigin;
            bulletEndPoint = rayOrigin + playerCam.transform.forward * maxBulletDist;

            RaycastHit hit;

            //_lr.SetPosition(0, gunEnd.transform.position);

            if (Physics.Raycast(rayOrigin, playerCam.transform.forward, out hit, range))
            {
                //_lr.SetPosition(1, hit.point);

                /////Added by Robert
                Renderer rend = hit.transform.GetComponent<Renderer>();
                if (rend != null)
                {
                    Texture2D tex = rend.material.mainTexture as Texture2D;
                    if (tex != null)
                    {
                        var xInTex = (int)(hit.textureCoord.x * tex.width);
                        var yInTex = (int)(hit.textureCoord.y * tex.height);
                        var pixel = tex.GetPixel(xInTex, yInTex);
                        if (pixel.a > 0)
                        {
                            if (hit.collider.CompareTag("Target"))
                            {
                                Target target = hit.collider.GetComponent<Target>();
                                target.TargetHit();
                            }
                        }
                    }
                }
                if (hit.collider.CompareTag("Enemy"))
                {
                    RobotEnemy robotEnemy = hit.transform.root.gameObject.GetComponent<RobotEnemy>();
                    if (robotEnemy != null)
                    {
                        robotEnemy.TakeDamage(gunDamage);
                    }
                }
                else if(hit.collider.CompareTag("EnemyHead"))
                {
                    RobotEnemy robotEnemy = hit.transform.root.gameObject.GetComponent<RobotEnemy>();
                    if (robotEnemy != null)
                    {
                        Debug.Log("6");
                        robotEnemy.HeadTakeDamage();
                    }
                }

                if (hit.transform.gameObject.layer == 9)
                {
                    TriggerObject trigger = hit.transform.GetComponent<TriggerObject>();
                    trigger.isTriggered = true;
                }

                lazerXAngle = Vector3.SignedAngle(hit.point - gunEnd.transform.position, gunEnd.transform.forward, gunEnd.transform.up);
                lazerYAngle = Vector3.SignedAngle(hit.point - gunEnd.transform.position, gunEnd.transform.forward, gunEnd.transform.right);
                lazerLength = (hit.point - gunEnd.transform.position).magnitude;



                bulletEndPoint = hit.point;
            }
            else
            {
                //_lr.SetPosition(1, playerCam.transform.forward * 1000000);

                lazerXAngle = Vector3.SignedAngle(playerCam.transform.forward * 10000 - gunEnd.transform.position, gunEnd.transform.forward, gunEnd.transform.up);
                lazerYAngle = Vector3.SignedAngle(playerCam.transform.forward * 10000 - gunEnd.transform.position, gunEnd.transform.forward, gunEnd.transform.right);
                lazerLength = (playerCam.transform.forward * 10000 - gunEnd.transform.position).magnitude;
            }

            StartCoroutine(ShootingEffect());
        }

        Vector3 thevector = Quaternion.AngleAxis(-lazerXAngle, gunEnd.transform.up) * gunEnd.transform.forward;
        thevector = Quaternion.AngleAxis(-lazerYAngle * 0.5f, gunEnd.transform.right) * thevector;
        _lr.SetPosition(1, gunEnd.transform.position + thevector * lazerLength);
        _lr.SetPosition(0, gunEnd.transform.position);

        IEnumerator ShootingEffect()
        {
            _as.Play();
            _lr.enabled = true;

            //yield return new WaitForSeconds(shotDuration);

            _lr.enabled = false;

            Debug.Log($"Instantiating bullet");
            // Added by Tom
            shootingPS.Play();
            GameObject temp = Instantiate(bullet);
            Bullet tempBullet = temp.GetComponent<Bullet>();
            tempBullet.startPos = bulletStartPoint;
            tempBullet.endPos = bulletEndPoint;

            yield return null;


        }


    }
}
