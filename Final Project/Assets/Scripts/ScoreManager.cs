using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    [SerializeField] RuntimeData _runtimeData;
    [SerializeField] TextMeshProUGUI _timeText;
    [SerializeField] TextMeshProUGUI _coinsText;
    [SerializeField] TextMeshProUGUI _livesText;
    // Update is called once per frame
    void FixedUpdate()
    {
        MakeChecks();
    }
    private void MakeChecks()
    {
        CheckTime();
        CheckTotalCoins();
        UpdateText();
    }
    private void CheckTime()
    {
        if (_runtimeData._timeLeft == 0)
            Debug.Log("GAME OVER");
    }
    private void CheckTotalCoins()
    {
        if (_runtimeData._totalCoins % 100 == 0 && _runtimeData._totalCoins !=0)
        {
            _runtimeData._totalCoins = 0;
            _runtimeData._totalLives++;
            AudioManager.AudioInstance.PlaySound("1-Up");
        }
    }
    private void UpdateText()
    {
        _timeText.text = _runtimeData._timeLeft + "";
        _coinsText.text = _runtimeData._totalCoins + "";
        _livesText.text = _runtimeData._totalLives + "";
    }
}
