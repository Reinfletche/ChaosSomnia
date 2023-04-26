using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    GameManager gm;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        gm.OnSceneLoad();
    }
}
