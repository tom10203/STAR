using UnityEngine;

public class EasterEgg : MonoBehaviour
{
    [SerializeField] private PlayerShooting playerShooting;
    [SerializeField] private int targetsInGame;

    MeshRenderer mRenderer;

    private void Start()
    {
        mRenderer = GetComponent<MeshRenderer>();
    }
    private void Update()
    {
        if (playerShooting.targetsHit >= targetsInGame)
        {
            mRenderer.enabled = true;
        }
    }
}
