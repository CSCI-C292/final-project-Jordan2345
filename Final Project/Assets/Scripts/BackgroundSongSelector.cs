using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSongSelector : MonoBehaviour
{
    [SerializeField] AudioClip[] _mapSongs;
    [SerializeField] GameObject _cameras;
    [SerializeField] RuntimeData _runtimeData;
    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckActiveCamera();
    }
    private void CheckActiveCamera()
    {
        int totalCameras = _cameras.transform.childCount;
        for(int i = 0; i < totalCameras; i++)
        {
            string currCam = _cameras.transform.GetChild(i).gameObject.name;
            if(_cameras.transform.GetChild(i).gameObject.activeSelf)
            {
                if(!audioSource.clip.name.Contains(_runtimeData._currentLevelData.currentWorld+""))
                {
                    audioSource.clip = _mapSongs[GetClipIndex()];
                    audioSource.Play(); 
                }
            }
        }
    }
    private int GetClipIndex()
    {
        int counter = 0;
        foreach(AudioClip clip in _mapSongs)
        {
            if (clip.name.Contains(_runtimeData._currentLevelData.currentWorld + ""))
                return counter;
            counter++;

        }
        return -1;
    }
}
