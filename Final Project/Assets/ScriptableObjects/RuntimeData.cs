﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "New RunTimeData")]
public class RuntimeData : ScriptableObject
{
    public int _totalCoins;
    public int _totalDragonCoins;
    public int _totalLives;
    public int _timeLeft;
    public int _totalScore;
    public int _gateScore;
    public bool _hasWeapon;
    public bool _isGunFlipped;
    public bool _hasSecondary;
    public string _currentLevel;
    public Sprite _currentWeapon;
    public MapLevelData _currentLevelData;

}
