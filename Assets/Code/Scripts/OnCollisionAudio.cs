using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class OnCollisionAudio : MonoBehaviour {

    private AudioSource audioSource;
    public string triggerTag;
    public AudioClip clip;

    public List<AudioTag> audioTags = new List<AudioTag>();

	void Start () {
        audioSource = GetComponent<AudioSource>();
	    audioSource.spatialBlend = 1.0f;
	}

    void PlayAudioClip(AudioClip localClip)
    {
        //Chad! I'm sorry, but I saw no alternative given the time constraint!
        if(audioSource.isPlaying && (localClip.name == "medchop" || localClip.name == "hardchop"))
            return;
        if (localClip.name == "fishsquish")
        {
            audioSource.PlayOneShot(localClip, .1f);
            return;
        }
        audioSource.PlayOneShot(localClip);
    }

    void HandleCollision(GameObject collidingObject)
    {
        if (collidingObject.tag == triggerTag)
            PlayAudioClip(clip);

        foreach (var audioTag in audioTags)
        {
            if (collidingObject.tag == audioTag.tag)
                PlayAudioClip(audioTag.clip);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        HandleCollision(collision.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        HandleCollision(other.gameObject);
    }
}

[System.Serializable]
public class AudioTag
{
    public string tag;
    public AudioClip clip;
}
