using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialExitLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        LoadScenes.SceneInstance.LoadScene("World Map");
    }
}
