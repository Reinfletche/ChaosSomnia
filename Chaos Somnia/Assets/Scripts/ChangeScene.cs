using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour, Iiteractable
{
    [SerializeField] int Escena;
    [SerializeField] float[] PlayerSpawn;

    GameManager gm;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public void Interact()
    {
        gm.PlayerSpawnPosition = new Vector3(PlayerSpawn[0], PlayerSpawn[1], PlayerSpawn[2]);

        foreach(GameObject objeto in gm.DontDestroy)
        {
            DontDestroyOnLoad(objeto);

        }
        DontDestroyOnLoad(gm.Player);

        SceneManager.LoadScene(Escena);
    }


}
