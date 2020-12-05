using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectiles : MonoBehaviour
{
    [SerializeField] GameObject _bulletBill;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag.Equals("Player"))
        {
            AudioManager.AudioInstance.PlaySound("Launch Bill");
            Instantiate(_bulletBill, gameObject.transform.Find("SpawnPos").position, Quaternion.identity);
            //turn off the collider so that 1 spawn position can only spawn 1 bullet bill
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
