using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    #region CORE
    private void Start()
    {
        Vida = vidaMaxima;
    }

    private void Update()
    {
        Update_Armas();
        Update_Objetos();
    }

    #endregion CORE

    

    
    
    #region Stats
    [Header("Stats")]
    public int vidaMaxima = 100;
    private int _vida;
    
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
    #endregion
    
    
    #region Armas

    [SerializeField] private Arma[] armas;
    private int indiceArma = 0;
    private Arma _arma = null;
    
    private void Update_Armas()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Arma.Disparar();
        }
        
        int scroll = (int) Input.mouseScrollDelta.y;
        
        if (scroll != 0)
        {
            indiceArma += scroll;

            if (indiceArma < 0) //-1
                indiceArma = 2;
            else if (indiceArma > armas.Length-1) //3
                indiceArma = 0;

            Arma = armas[indiceArma];
        }
    }

    public Arma Arma
    {
        get => _arma;
        set
        {
            _arma = value;

            foreach (Arma arma in armas)
            {
                if (arma == _arma)
                    arma.gameObject.SetActive(true);
                else
                    arma.gameObject.SetActive(false);
            }

            HUD.IconoBala = _arma.iconoBala;
            HUD.TextoBalas = _arma.Balas + "/" + _arma.BalasReserva;
        }
    }

    public Arma ObtenerArma(TipoArma tipoArma)
    {
        switch (tipoArma)
        {
            case TipoArma.pistola: return armas[0];
            case TipoArma.escopeta: return armas[1];
            case TipoArma.rifle: return armas[2];
            default: return null;
        }
    }
    #endregion Armas

    #region Objetos
    private Usable usable;

    private void Update_Objetos()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (usable)
                usable.Usar(this);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Usable":
                usable = other.GetComponent<Usable>();
                usable.Activo = true;
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "Usable":
                usable.Activo = false;
                usable = null;
                break;
        }
    }

    #endregion

}

