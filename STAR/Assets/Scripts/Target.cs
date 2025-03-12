using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] AudioSource audio;
    [SerializeField] AudioClip hitSound;
    [SerializeField] Animator targetAnim;
    private bool gotHit = false;
    [SerializeField] float timeToSave = 2f;
    private InGameUI inGameUI;
    private ActivatePortal activatePortalScript;

    [SerializeField] private ParticleSystem minus2PE;
    [SerializeField] private ParticleSystem plus2PE;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inGameUI = FindAnyObjectByType<InGameUI>();
        activatePortalScript = FindAnyObjectByType<ActivatePortal>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TargetHit()
    {
        if (!gotHit)
        {
            gotHit = true;
            //ToDo - add remove time/points

            audio.PlayOneShot(hitSound, 1f);
            targetAnim.SetTrigger("TargetHit");
            inGameUI.AddToTimer(timeToSave);
            if (activatePortalScript != null)
            {
                activatePortalScript.AddPortalPoint();
            }

            if (timeToSave < 0f)
            {
                ParticleSystem plus2 = Instantiate(plus2PE, transform.position, Quaternion.identity);
                plus2.Play();
                Destroy(plus2, 2);
            }
            else
            {
                ParticleSystem minus2 = Instantiate(minus2PE, transform.position, Quaternion.identity);
                minus2.Play();
                Destroy(minus2, 2);
            }
        }
    }
}