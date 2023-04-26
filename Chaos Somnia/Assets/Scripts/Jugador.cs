using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{

    #region CORE
    private static Jugador self;

    private void Awake()
    {
        self = this;
    }

    private void Start()
    {
        Vida = vidaMaxima;
        InicializarArmas();
    }

    private void Update()
    {
        Update_Armas();
        Update_Objetos();
    }

    public static Vector3 Posicion => self.transform.position;

    #endregion CORE

    

    
    
    #region Stats
    [Header("Stats")]

    public int vidaMaxima = 100;
    private int _vida;
    
    public static int Vida
    {
        get => self._vida;
        set
        {
            self._vida = value;
            HUD.BarraVida = (float)  self._vida /  self.vidaMaxima;
            HUD.TextoVida =  self._vida + "/" +  self.vidaMaxima;
        }
    }
    #endregion
    
    
    #region Armas

    [SerializeField] private Arma[] armas;
    private int indiceArma = -1;
    private Arma _arma = null;

    private void InicializarArmas()
    {
        Arma = null;
    }
    
    private void Update_Armas()
    {
        if(indiceArma == -1)
            return;
        
        if (Input.GetMouseButtonDown(0))
        {
            Arma.Disparar();
        }
        
        int scroll = (int) Input.mouseScrollDelta.y;
        
        if (scroll != 0)
        {
            int indice = indiceArma + scroll;

            CicloArmas:
            if (indice < 0) //-1
                indice = 2;
            else if (indice > 2) //3
                indice = 0;

            Arma armaTemp = armas[indice];

            if (armaTemp.bloqueada)
            {
                indice++;
                goto CicloArmas;
            }

            Arma = armaTemp;
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

            if (_arma)
            {
                indiceArma = IndiceArma(Arma.tipoArma);
                
                HUD.IconoBala = _arma.iconoBala;
                HUD.TextoBalas = _arma.Balas + "/" + _arma.BalasReserva;
            }
            else
            {
                HUD.IconoBala = Resources.Load<Sprite>("municionVacia");
                HUD.TextoBalas = string.Empty;
            }
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

    public int IndiceArma(TipoArma tipoArma)
    {
        switch (tipoArma)
        {
            case TipoArma.pistola: return 0;
            case TipoArma.escopeta: return 1;
            case TipoArma.rifle: return 2;
            default: return -1;
        }
    }

    public void DesbloquearArma(TipoArma tipoArma)
    {
        ObtenerArma(tipoArma).bloqueada = false;

        if (indiceArma == -1)
        {
            Arma = ObtenerArma(tipoArma);
        }
    }
    #endregion Armas

    
    
    #region Objetos
    private Usable usable;

    private ChangeScene changescene;

    private void Update_Objetos()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (usable && !changescene)
                usable.Usar(this);
            else if (changescene && usable)
            {
                changescene.Interact();
            }
        }


    }
    
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Usable":
                {
                    usable = other.GetComponent<Usable>();
                    usable.Activo = true;
                    break;
                }
            case "Interactable":
                {
                    usable = other.GetComponent<Usable>();
                    changescene = other.GetComponent<ChangeScene>();
                    usable.Activo = true;
                    break;
                }   
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "Usable":
                {
                    usable.Activo = false;
                    usable = null;
                    break;
                }

            case "Interactable":
                {
                    usable.Activo = false; //Se usa unicamente para activar el canvas
                    changescene = null;
                    usable = null;
                    break;
                }
                
        }
    }

    #endregion

}

