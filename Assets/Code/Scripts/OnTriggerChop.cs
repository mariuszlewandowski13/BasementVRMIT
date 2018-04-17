using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerChop : MonoBehaviour
{
    public List<AudioClip> audioClips;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        other.gameObject.SendMessage("OnChop", null, SendMessageOptions.DontRequireReceiver);
    }

}
