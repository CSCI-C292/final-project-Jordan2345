using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WorldMapHUDManager : MonoBehaviour
{
    [SerializeField] RuntimeData _runtimeData;
    [SerializeField] TextMeshProUGUI _currentLives;
    [SerializeField] TextMeshProUGUI _currentLevel;

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateText();
    }
    private void UpdateText()
    {
        _currentLives.text = _runtimeData._totalLives + "";
        _currentLevel.text = _runtimeData._currentLevel;
    }
}
