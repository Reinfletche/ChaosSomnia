using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Vector3 PlayerSpawnPosition;

    public GameObject Player;

    public GameObject[] DontDestroy;


    public void OnSceneLoad()
    {
        Debug.Log("Transportar player");
        Transform PlayerPos = GameObject.FindWithTag("Player").GetComponent<Transform>();
        PlayerPos.transform.position = PlayerSpawnPosition;
    }
}
