/*
@Authors - Craig
*/

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource springSource;
    public AudioSource jumpSource;
    public AudioSource deathSource;
    public AudioSource hopSource;
    public AudioSource checkpointSource;
    void Awake()
    {
        instance = this;
    }

    public void PlaySpring()
    {
        springSource.Play();
    }

    public void PlayJump()
    {
        jumpSource.Play();
    }

    public void PlayDeath()
    {
        deathSource.Play();
    }

    public void PlayHop()
    {
        hopSource.Play();
    }
    public void PlayCheckpoint()
    {
        checkpointSource.Play();
    }
}
