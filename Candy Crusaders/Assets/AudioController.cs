using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public GameObject[] audioClips;

    private List<AudioSource> audioSources;
    private int index;
    private int count;

    public void Start()
    {
        this.index = 0;
        this.count = this.audioClips.Length;
        this.audioSources = new List<AudioSource>();

        // initialize the audio clips
        foreach(GameObject audioClip in this.audioClips)
        {
            this.audioSources.Add(audioClip.GetComponent<AudioSource>());
        }

        // start playing
        StartCoroutine(this.Play());
    }

    private IEnumerator Play()
    {
        // make sure index in range
        this.index = this.index % this.count;

        // play audio clip
        this.audioSources[this.index].Play();

        // wait until clip is over
        yield return new WaitForSeconds(1 + this.audioSources[this.index].clip.length);

        // increment song index
        this.index++;

        // play next song
        StartCoroutine(this.Play());
    }
}
