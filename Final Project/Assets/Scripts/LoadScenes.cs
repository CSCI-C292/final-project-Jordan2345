using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScenes : MonoBehaviour
{
    [SerializeField] RuntimeData _runtimeData;
    public static LoadScenes SceneInstance;
    private bool flag = false;
    private void Awake()
    {
        SceneInstance = this;
        _runtimeData._timeLeft = 300;
        _runtimeData._totalDragonCoins = 0;
        flag = false;
    }
    private void Start()
    {
        Debug.Log("AT START");
        if(!getSceneName().Equals("World Map"))
        {
            Debug.Log("INVOKE");
            InvokeRepeating("StartTimer",1f,1f);            
        }
    }
    private void Update()
    {
        if(!getSceneName().Equals("World Map"))
        {
            if (LevelEnded() && !flag)
            {
                Debug.Log("Timer Stopped");
                CancelInvoke("StartTimer");
                flag = true;
                //initiate end of level sequence
                StartCoroutine(EndLevel());
            }
        }
        
    }
    private IEnumerator EndLevel()
    {
        yield return new WaitForSeconds(10f);
        StopCoroutine(EndLevel());
        LoadScene("World Map");
    }
    private void StartTimer()
    {
        _runtimeData._timeLeft--;
    }
    private bool LevelEnded()
    {
        return EndOfLevelTicker.EndOfLevelInstance.HasLevelEnded();
    }
    public string getSceneName()
    {
        Scene currScene = SceneManager.GetActiveScene();
        return currScene.name;
    }
    public void LoadScene(string level)
    {
        SceneManager.LoadScene(level);  
    }
}
