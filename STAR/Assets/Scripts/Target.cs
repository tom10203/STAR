using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] AudioSource audio;
    [SerializeField] AudioClip hitSound;
    [SerializeField] Animator targetAnim;
    private bool gotHit = false;
    [SerializeField] float timeToAdd = 2f;
    private InGameUI inGameUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inGameUI = FindAnyObjectByType<InGameUI>();
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
            inGameUI.AddToTimer(timeToAdd);
        }
    }
}