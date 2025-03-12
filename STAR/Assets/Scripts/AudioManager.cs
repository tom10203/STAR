using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Player2 player2;
    [SerializeField] private AudioSource runningAudioSource;
    [SerializeField] private AudioSource slidingAudioSource;

    CharacterState characterState;
    Vector2 playerMovementInput;


    private void Update()
    {
        if (player != null)
        {
            characterState = player.characterState;
            playerMovementInput = player.playerMovementInput;
        }
        else if (player2 != null)
        {
            characterState = player2.characterState;
            playerMovementInput = player2.playerMovementInput;
        }

        // Handle sliding audio
        if (characterState.stance is Stance.Slide)
        {
            if (!slidingAudioSource.isPlaying)
            {
                runningAudioSource.Stop(); // Stop running sound
                slidingAudioSource.Play(); // Play sliding sound
            }
        }
        else if (characterState.stance == Stance.Stand && playerMovementInput != Vector2.zero)
        {
            if (!runningAudioSource.isPlaying)
            {
                slidingAudioSource.Stop(); // Stop sliding sound
                runningAudioSource.Play(); // Play running sound
            }
        }
        else
        {
            // Stop audio when no movement
            runningAudioSource.Stop();
            slidingAudioSource.Stop();
        }
    }
}