using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class HUDManager : MonoBehaviour
{
    [SerializeField] RuntimeData _runtimeData;
    [SerializeField] TextMeshProUGUI _timeText;
    [SerializeField] TextMeshProUGUI _coinsText;
    [SerializeField] TextMeshProUGUI _livesText;
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] TextMeshProUGUI _gateScoreText;
    [SerializeField] List<GameObject> _dragonCoins;
    [SerializeField] Image _powerupImage;
    // Update is called once per frame
    void FixedUpdate()
    {
        MakeChecks();
    }
    private void MakeChecks()
    {
        CheckTime();
        CheckTotalCoins();
        CheckGateScore();
        CheckDragonCoins();
        CheckSecondaryPowerup();
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
    private void CheckGateScore()
    {
        if(_runtimeData._gateScore >= 100)
        {
            int newScore = _runtimeData._gateScore % 100;
            _runtimeData._gateScore = newScore;
            _runtimeData._totalLives++;
            AudioManager.AudioInstance.PlaySound("1-Up");
        }
    }
    private void CheckDragonCoins()
    {
        if(_runtimeData._totalDragonCoins == 5)
        {
            _runtimeData._totalLives++;
            AudioManager.AudioInstance.PlaySound("1-Up");
            foreach (GameObject obj in _dragonCoins)
                obj.GetComponent<Image>().enabled = false;
            _runtimeData._totalDragonCoins = 0;
        }
        else
        {
            for(int i = 0; i < 4; i++)
            {
                if (i < _runtimeData._totalDragonCoins)
                    _dragonCoins[i].GetComponent<Image>().enabled = true;
                else
                    _dragonCoins[i].GetComponent<Image>().enabled = false;
            }
        }
    }
    private void CheckSecondaryPowerup()
    {
        if (_runtimeData._hasSecondary)
            _powerupImage.enabled = true;
        else
            _powerupImage.enabled = false;
    }
    private void UpdateText()
    {
        _timeText.text = _runtimeData._timeLeft + "";
        _coinsText.text = _runtimeData._totalCoins + "";
        _livesText.text = _runtimeData._totalLives + "";
        _scoreText.text = _runtimeData._totalScore + "";
        _gateScoreText.text = _runtimeData._gateScore + "";
    }
}
