using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usable : MonoBehaviour
{
    private IUsable iUsable;
    private Canvas canvas;
    private bool activo = false;
    
    private void Awake()
    {
        iUsable = transform.parent.GetComponent<IUsable>();
        canvas = transform.GetChild(0).GetComponent<Canvas>();
        canvas.enabled = false;
    }

    private void LateUpdate()
    {
        canvas.transform.LookAt(Camera.main.transform);
    }

    public void Usar(Jugador jugador)
    {
        iUsable.Usar(jugador);
    }
    
    public bool Activo
    {
        set
        {
            activo = value;
            canvas.enabled = value;
        }
    }
}
