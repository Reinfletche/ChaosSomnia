using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemigo : MonoBehaviour
{
    private int _vida;
    [SerializeField] private int vidaMax;

    //Componentes
    #region Componentes
    private NavMeshAgent nav;
    private CapsuleCollider collider;
    private Rigidbody rb;
    public Rigidbody Rb => rb;
    #endregion Componentes
    
    
    #region CORE
    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        collider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
    }
    #endregion CORE

    
    
    #region Stats
    private void Morir()
    {
        
    }
    
    public int Vida
    {
        get => _vida;
        set
        {
            _vida = value;
            
            if(_vida <= 0)
                Morir();
        }
    }
    #endregion < < Stats
}
