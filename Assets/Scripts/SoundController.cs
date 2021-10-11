using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static AudioClip blipSound, completedSound;
    static AudioSource audioSource;
    void Start()
    {

        blipSound = Resources.Load<AudioClip>("Sounds/Blip");
        completedSound = Resources.Load<AudioClip>("Sounds/Completed");
        audioSource = GetComponent<AudioSource>();
    }
    public static void playSound(AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
    }
}
