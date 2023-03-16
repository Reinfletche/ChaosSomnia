using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usable : MonoBehaviour
{
    private IUsable iUsable;
    private Canvas canvas;
    
    private void Awake()
    {
        iUsable = transform.parent.GetComponent<IUsable>();
        canvas = transform.GetChild(0).GetComponent<Canvas>();
        canvas.enabled = false;
    }

    public void Usar(Jugador jugador)
    {
        iUsable.Usar(jugador);
    }
    
    public bool Activo
    {
        set => canvas.enabled = value;
    }
}
