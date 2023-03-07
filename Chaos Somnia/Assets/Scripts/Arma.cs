using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arma : MonoBehaviour
{
    [Header("Stats")] 
    public bool bloqueada = true;
    public Sprite iconoBala;
    public int damage;
    public int cadencia;
    
    private int balasMax;
    public int balasCargador;
    private int _balas;
    
    public float tiempoRecarga;
    public float fuerzaEmpuje;
    
    
    

}
