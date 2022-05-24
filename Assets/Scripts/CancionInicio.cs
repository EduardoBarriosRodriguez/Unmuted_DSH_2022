using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancionInicio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    public bool bLoop;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.volume = 0.3f;
        audioSource.Play();
        audioSource.loop = bLoop;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
