using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMechanics : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] RuntimeData _runtimeData;
    [SerializeField] GameObject player;
    [SerializeField] GameObject _weaponType;
    public static DeathMechanics DeathMechanicsInstance;
    private void Awake()
    {
        DeathMechanicsInstance = this;
    }
    public void CheckDeath()
    {
        if (_runtimeData._hasWeapon)
        {
            _runtimeData._hasWeapon = false;
            AudioManager.AudioInstance.PlaySound("Lose Weapon");
            if(_runtimeData._hasSecondary)
            {
                Vector3 pos = new Vector3(player.transform.position.x, player.transform.position.y + 15,player.transform.position.z);
                Instantiate(_weaponType, pos, Quaternion.identity);
                _runtimeData._hasSecondary = false;
            }
        }
        else
        {
            _animator.SetBool("hasDied", true);
            player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 700);
            _runtimeData._totalLives--;
            player.GetComponent<CapsuleCollider2D>().enabled = false;
            StartCoroutine(DeathSequence());
        }
        
    }
    public IEnumerator DeathSequence()
    {
        AudioManager.AudioInstance.StopSound();
        AudioManager.AudioInstance.PlaySound("Death Sound");
        yield return new WaitForSeconds(3);
        _animator.SetBool("hasDied", false);
        player.GetComponent<CapsuleCollider2D>().enabled = true;
        LoadScenes.SceneInstance.LoadScene("World Map");
    }

}
