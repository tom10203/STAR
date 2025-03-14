using TMPro;
using UnityEngine;

public class TargetsDeactivateDoor : MonoBehaviour
{
    [SerializeField] private GameObject doorToDeactivate;

    [SerializeField] private int pointsToActivation = 0;
    private int currentPoints = 0;

    [SerializeField] private TMP_Text targetText;

    private void Start()
    {
        targetText.text = "TARGETS TO KILL: " + pointsToActivation;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPoints >= pointsToActivation)
        {
            doorToDeactivate.SetActive(false);
        }
    }

    public void AddDoorPoint()
    {
        currentPoints++;

        int num = pointsToActivation - currentPoints;
        if (num < 0)
        {
            num = 0;
        }
        targetText.text = "TARGETS TO KILL: " + num;
    }
}
