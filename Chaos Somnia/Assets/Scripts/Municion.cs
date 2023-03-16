using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Municion : MonoBehaviour, IUsable
{
    public void Usar(Jugador jugador)
    {
        print("Soy MUNICION :D");
    }
}
