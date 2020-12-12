using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _gameTitle;
    [SerializeField] RuntimeData _runtimeData;
    [SerializeField] MapLevelData _startLevel;
    private void Start()
    {
        InvokeRepeating("ChangeTextColor",0f,.35f);
        //reset all values in runtime Data
        InitializeRuntimeData();
    }
    private void InitializeRuntimeData()
    {
        _runtimeData._totalCoins = 0;
        _runtimeData._totalLives = 5;
        _runtimeData._totalScore = 0;
        _runtimeData._hasWeapon = false;
        _runtimeData._hasSecondary = false;
        _runtimeData._gateScore = 0;
        _runtimeData._currentLevel = "Yoshi's Island Home";
        _runtimeData._currentLevelData = _startLevel;
    }
    private void ChangeTextColor()
    {
        //heavily using this implementation - https://forum.unity.com/threads/change-color-of-a-single-word.538706/

        //colors are cyan, yellow, red,green
        Color32[] availableColors = { new Color32(14, 211, 238, 255), new Color32(255, 255, 0, 255), new Color32(238, 26, 12, 255), new Color32(29, 178, 8, 255) };
        Color32 prevColor=new Color32();
        for(int i = 0; i < _gameTitle.textInfo.wordCount; i++)
        {
            TMP_WordInfo info = _gameTitle.textInfo.wordInfo[i];
            for (int j = 0; j < info.characterCount; j++)
            {
                int charIndex = info.firstCharacterIndex + j;
                int meshIndex = _gameTitle.textInfo.characterInfo[charIndex].materialReferenceIndex;
                int vertexIndex = _gameTitle.textInfo.characterInfo[charIndex].vertexIndex;

                Color32[] vertexColors = _gameTitle.textInfo.meshInfo[meshIndex].colors32;
                Color32 myColor = availableColors[Random.Range(0, availableColors.Length)];
                if(prevColor.Equals(myColor))
                {
                    while(myColor.Equals(prevColor))
                        myColor = availableColors[Random.Range(0, availableColors.Length)];
                }
                vertexColors[vertexIndex + 0] = myColor;
                vertexColors[vertexIndex + 1] = myColor;
                vertexColors[vertexIndex + 2] = myColor;
                vertexColors[vertexIndex + 3] = myColor;
                prevColor = myColor;
            }
        }
        
        _gameTitle.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
    }
}
