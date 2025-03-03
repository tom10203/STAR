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

    //Ref in Start
    AudioSource _as;
    LineRenderer _lr;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _lr = GetComponent<LineRenderer>();
        _as = GetComponent<AudioSource>();
        playerCam = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            StartCoroutine(ShootingEffect());

            Vector3 rayOrigin = playerCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));

            RaycastHit hit;

            _lr.SetPosition(0, gunEnd.transform.position);

            if (Physics.Raycast(rayOrigin, playerCam.transform.forward, out hit, range))
            {
                _lr.SetPosition(1, hit.point);

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
                    RobotEnemy robotEnemy = hit.collider.GetComponent<RobotEnemy>();
                    if(robotEnemy != null)
                    {
                        robotEnemy.TakeDamage(gunDamage);
                    }
                }
                /////
            }
            else
            {
                _lr.SetPosition(1, playerCam.transform.forward * 1000000);
            }

        }

        IEnumerator ShootingEffect()
        {
            _as.Play();
            _lr.enabled = true;

            yield return new WaitForSeconds(shotDuration);

            _lr.enabled = false;

        }


    }
}
