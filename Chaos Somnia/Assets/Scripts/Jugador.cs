using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    public int vidaMaxima = 100;
    private int _vida;

    private void Start()
    {
        Vida = vidaMaxima;
    }

    public int Vida
    {
        get => _vida;
        set
        {
            _vida = value;
            HUD.BarraVida = (float) _vida / vidaMaxima;
            HUD.TextoVida = _vida + "/" + vidaMaxima;
        }
    }
}
