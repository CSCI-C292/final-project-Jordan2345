using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] string[] namesOfClips;
    [SerializeField] AudioClip[] clips;
    public static AudioManager AudioInstance;
    private AudioSource audioSource;
    private Dictionary<string, AudioClip> map;
    // Start is called before the first frame update
    void Awake()
    {
        AudioInstance = this;
        map = new Dictionary<string, AudioClip>();
        audioSource = GetComponent<AudioSource>();
        for (int i = 0; i < clips.Length; i++)
        {
            map.Add(namesOfClips[i], clips[i]);
        }
    }
    public void PlaySound(string key)
    {
        if (map.ContainsKey(key))
        {
            audioSource.PlayOneShot(map[key]);
        }
    }
    public void StopSound()
    {
        audioSource.Stop();
    }
}
