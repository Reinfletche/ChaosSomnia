using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DesbloquearArma : MonoBehaviour, IUsable
{
    [SerializeField] private TipoArma tipoArma;
    
    public void Usar(Jugador jugador)
    {
        jugador.DesbloquearArma(tipoArma);
        Destroy(gameObject);
    }
}
