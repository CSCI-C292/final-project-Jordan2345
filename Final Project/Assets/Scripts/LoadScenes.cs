using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScenes : MonoBehaviour
{
    public static LoadScenes SceneInstance;
    private void Awake()
    {
        SceneInstance = this;
    }
    public string getSceneName()
    {
        Scene currScene = SceneManager.GetActiveScene();
        return currScene.name;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("End Of Level");
        AudioManager.AudioInstance.StopSound();
        AudioManager.AudioInstance.PlaySound("Level Clear");
    }
}
