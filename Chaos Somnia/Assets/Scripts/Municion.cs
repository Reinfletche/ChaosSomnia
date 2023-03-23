using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Municion : MonoBehaviour, IUsable
{
    [SerializeField] private TipoArma tipoArma;
    [SerializeField] private int balas;

    public void Usar(Jugador jugador)
    {
        jugador.ObtenerArma(tipoArma).BalasReserva += balas;
        Destroy(gameObject);
    }
}
