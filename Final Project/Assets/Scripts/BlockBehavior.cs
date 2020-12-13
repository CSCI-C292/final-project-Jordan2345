﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehavior : MonoBehaviour
{
    [SerializeField] LayerMask _playerMask;
    [SerializeField] int _numHits;
    [SerializeField] RuntimeData _runtimeData;
    [SerializeField] GameObject _weaponType;
    private Animator animator;
    private bool flag = false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void CoinBlockCheck()
    {
        if(_numHits > 0)
        {
            if (_numHits == 1)
            {
                animator.SetBool("noCoins", true);
            }
            else
            {
                animator.SetBool("noCoins", false);
            }
            AudioManager.AudioInstance.PlaySound("Coin Block");
            _runtimeData._totalCoins++;
            _numHits--;
        }
        else
        {
            AudioManager.AudioInstance.PlaySound("Turn Block");
        }

    }
    private void TurnBlockCheck()
    {
        _numHits--;
        bool hasBeenHit = _numHits == 0;
        if(hasBeenHit && !flag)
        {
            //Debug.Log("Starting");
            AudioManager.AudioInstance.PlaySound("Turn Block");
            StartCoroutine("TurnBlock");
        }
    }
    private void PowerupBlockCheck()
    {
        if (_numHits > 0)
        {
            animator.SetBool("hasBeenHit", true);
            AudioManager.AudioInstance.PlaySound("Powerup Block");
            _numHits--;
            Instantiate(_weaponType, transform.position, Quaternion.identity);
        }
        else
        {
            AudioManager.AudioInstance.PlaySound("Turn Block");
        }
    }
    private IEnumerator TurnBlock()
    {
        animator.SetBool("hasBeenHit", true);
        GetComponent<BoxCollider2D>().enabled = false;
        //Debug.Log("TURNING");
        yield return new WaitForSeconds(5f);
        animator.SetBool("hasBeenHit", false);
        GetComponent<BoxCollider2D>().enabled = true;
        //Debug.Log("STOPPED");
        flag = true;
        _numHits = 1;

    }
    private void PSwitchCheck()
    {
        if(!flag && !animator.GetBool("hitSwitch"))
        {
            animator.SetBool("hitSwitch", true);
            StartCoroutine(PSwitchTurn());
        }
        
    }
    private IEnumerator PSwitchTurn()
    {
        flag = true;
        AudioManager.AudioInstance.StopSound();
        AudioManager.AudioInstance.PlaySound("Hit P-Switch");
        AudioManager.AudioInstance.PlaySound("P-Switch Theme");
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        List<GameObject> coins = new List<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.tag.Equals("Coin"))
            {
                //add this game object to a list so when the p-switch timer runs out, we revert it back to coins
                obj.GetComponent<Animator>().SetBool("coinSwitchHit", true);
                obj.layer = LayerMask.NameToLayer("Foreground");
                coins.Add(obj);
            }
        }
        yield return new WaitForSeconds(15f);
        //revert back to coins
        foreach(GameObject obj in coins)
        {
            obj.GetComponent<Animator>().SetBool("coinSwitchHit", false);
            obj.layer = LayerMask.NameToLayer("Collectables");
        }
        AudioManager.AudioInstance.PlaySound(LoadScenes.SceneInstance.getSceneName());
        Destroy(gameObject);

    }
    private bool isUnderBlock()
    {
        float padding = 2.5f;
        BoxCollider2D collider2D = GetComponent<BoxCollider2D>();
        RaycastHit2D hit = Physics2D.Raycast(collider2D.bounds.center, Vector2.down, collider2D.bounds.extents.y + padding, _playerMask);
        Color ray;
        if (hit.collider != null)
            ray = Color.green;
        else
            ray = Color.red;
        Vector2 down = Vector2.down * collider2D.bounds.extents.y;
        down.y -= padding;
        Debug.DrawRay(collider2D.bounds.center, down, ray);
        return hit.collider != null;
    }
    private bool isAboveBlock()
    {
        float padding = 2.5f;
        BoxCollider2D collider2D = GetComponent<BoxCollider2D>();
        RaycastHit2D hit = Physics2D.Raycast(collider2D.bounds.center, Vector2.down, collider2D.bounds.extents.y - padding, _playerMask);
        Color ray;
        if (hit.collider != null)
            ray = Color.green;
        else
            ray = Color.red;
        Vector2 down = Vector2.down * collider2D.bounds.extents.y;
        down.y += padding;
        Debug.DrawRay(collider2D.bounds.center, down, ray);
        return hit.collider != null;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool isPlayer = collision.gameObject.name.Equals("Player");
        if(isPlayer)
        {
            //check for different types of blocks
            Debug.Log(gameObject.tag);
            if(gameObject.tag.Equals("Coin Block") && isUnderBlock())
            {
                //Debug.Log("Underneath");
                CoinBlockCheck();
            }
            else if(gameObject.tag.Equals("Turn Block") && isUnderBlock())
            {
                flag = false;
                TurnBlockCheck();
            }
            else if(gameObject.tag.Equals("Powerup Block") && isUnderBlock())
            {
                PowerupBlockCheck();
            }
            else if(gameObject.tag.Equals("P-Switch") && isAboveBlock())
            {
                flag = false;
                PSwitchCheck();
            }
        }
    }
}
