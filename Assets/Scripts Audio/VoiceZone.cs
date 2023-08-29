using UnityEngine;

public class VoiceZone : MonoBehaviour
{
    // play one time
    public bool hasPlayed;
    // trigger the sound when the player is near to a specific object.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasPlayed)
        {
            hasPlayed = true;
            Spirit spirit = GetComponent<Spirit>();
            spirit.PlayDialog();
        }
    }
}
