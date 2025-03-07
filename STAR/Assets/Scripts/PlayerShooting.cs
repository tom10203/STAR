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
    [SerializeField] private float bulletSpeed = 250f;
    [SerializeField] private float bulletHitBuffer = 0.1f;
    [SerializeField] private ParticleSystem bulletHitPE;

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
        //_lr = GetComponent<LineRenderer>();
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

            bulletStartPoint = gunEnd.transform.position;
            bulletEndPoint = rayOrigin + playerCam.transform.forward * maxBulletDist;

            int layerHit = 0;

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
                            layerHit = 1;
                        }
                    }
                }
               
                if (hit.collider.CompareTag("Enemy"))
                {
                    layerHit = 2;
                }
                if (hit.collider.CompareTag("EnemyHead"))
                {
                    layerHit = 3;
                }

                if (hit.transform.gameObject.layer == 9) // interactable layer
                {
                        layerHit = 9;
                }

                //laser code
                /*
                /lazerXAngle = Vector3.SignedAngle(hit.point - gunEnd.transform.position, gunEnd.transform.forward, gunEnd.transform.up);
                lazerYAngle = Vector3.SignedAngle(hit.point - gunEnd.transform.position, gunEnd.transform.forward, gunEnd.transform.right);
                lazerLength = (hit.point - gunEnd.transform.position).magnitude;*/



                bulletEndPoint = hit.point;
                StartCoroutine(DelayBulletHit(hit, bulletStartPoint, bulletEndPoint, layerHit));


            }
            else
            {
                //laser code
                //_lr.SetPosition(1, playerCam.transform.forward * 1000000);

                /*lazerXAngle = Vector3.SignedAngle(playerCam.transform.forward * 10000 - gunEnd.transform.position, gunEnd.transform.forward, gunEnd.transform.up);
                lazerYAngle = Vector3.SignedAngle(playerCam.transform.forward * 10000 - gunEnd.transform.position, gunEnd.transform.forward, gunEnd.transform.right);
                lazerLength = (playerCam.transform.forward * 10000 - gunEnd.transform.position).magnitude;*/
            }

            StartCoroutine(ShootingEffect());
        }

        //laser code
        /*Vector3 thevector = Quaternion.AngleAxis(-lazerXAngle, gunEnd.transform.up) * gunEnd.transform.forward;
        thevector = Quaternion.AngleAxis(-lazerYAngle * 0.5f, gunEnd.transform.right) * thevector;
        _lr.SetPosition(1, gunEnd.transform.position + thevector * lazerLength);
        _lr.SetPosition(0, gunEnd.transform.position);*/

        
        float CalculateTimeToBulletHit(Vector3 startPos, Vector3 endPos)
        {
            return (endPos - startPos).magnitude / bulletSpeed;
        }

        IEnumerator DelayBulletHit(RaycastHit hit, Vector3 startPos, Vector3 endPos, int layer)
        {
            if (layer == 0)
            {
                yield break;
            }

            yield return new WaitForSeconds(CalculateTimeToBulletHit(startPos, endPos) - bulletHitBuffer);


            if (layer == 1)
            {
                Target target = hit.collider.GetComponent<Target>();
                if (target != null)
                {
                    target.TargetHit();
                }
            }
            else if (layer == 2)
            {
                RobotEnemy robotEnemy = hit.transform.root.gameObject.GetComponent<RobotEnemy>();
                if (robotEnemy != null)
                {
                    robotEnemy.TakeDamage(gunDamage);
                }
            }else if(layer == 3)
            {
                RobotEnemy robotEnemy = hit.transform.root.gameObject.GetComponent<RobotEnemy>();
                if (robotEnemy != null)
                {
                    robotEnemy.HeadTakeDamage();
                }
            }
            else if (layer == 9)
            {
                TriggerObject trigger = hit.transform.GetComponent<TriggerObject>();
                if (trigger != null)
                {
                    trigger.isTriggered = true;
                }
            }


            Instantiate(bulletHitPE, hit.point, Quaternion.identity);
        }


        IEnumerator ShootingEffect()
        {
            _as.Play();

            //laser code
            //_lr.enabled = true;
            //_lr.enabled = false;

            // Added by Tom
            shootingPS.Play();

            yield return new WaitForSeconds(shotDuration);


            GameObject temp = Instantiate(bullet);
            Bullet tempBullet = temp.GetComponent<Bullet>();
            tempBullet.startPos = bulletStartPoint;
            tempBullet.endPos = bulletEndPoint;
            tempBullet.bulletSpeed = bulletSpeed;

            yield return null;


        }


    }
}
