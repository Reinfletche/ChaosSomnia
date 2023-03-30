using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemigo : MonoBehaviour
{
    private int _vida;
    [SerializeField] private int vidaMax;
    [SerializeField] private int velocidad;

    //Patrulla
    [SerializeField] private Transform[] puntosPatrulla; 

    //Componentes
    #region Componentes
    private NavMeshAgent nav;
    private CapsuleCollider collider;
    private Rigidbody rb;
    public Rigidbody Rb => rb;
    private Animator animator;
    #endregion Componentes
    
    
    #region CORE
    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        collider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        
        nav.speed = velocidad;
    }

    private void Start()
    {
        StartCoroutine(CrPatrulla());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Patrulla"))
            StartCoroutine(CrPatrulla());
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

    #region Movimiento
    public void Moverse(Vector3 posicion)
    {
        nav.SetDestination(posicion);
        animator.SetBool("walk",true);
    }

    public void Parar()
    {
        nav.SetDestination(transform.position);
        animator.SetBool("walk",false);
    }

    #endregion < < Movimiento

    
    
    #region Patrulla
    private int indicePatrulla = 0;
    [SerializeField] private float paradoEnPuntoMin = 2;
    [SerializeField] private float paradoEnPuntoMax = 4;
    private bool patrullaReversa = false;
    
    public IEnumerator CrPatrulla()
    {
        if (puntosPatrulla.Length < 2)
            yield break;
        
        Parar();
        float tiempoParado = Random.Range(paradoEnPuntoMin, paradoEnPuntoMax);
        
        yield return new WaitForSeconds(tiempoParado);

        indicePatrulla = patrullaReversa ? indicePatrulla - 1 : indicePatrulla + 1;

        if (indicePatrulla >= puntosPatrulla.Length)
        {
            indicePatrulla -= 2;
            patrullaReversa = true;
        }
        else if (indicePatrulla < 0)
        {
            indicePatrulla += 2;
            patrullaReversa = false;
        }

        Moverse(puntosPatrulla[indicePatrulla].position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        for (int i = 0; i < puntosPatrulla.Length; i++)
        {
            Gizmos.DrawWireSphere(puntosPatrulla[i].position+Vector3.up,0.5f);

            if (i >= 0 && i < puntosPatrulla.Length-1)
            {
                Vector3 p1 = puntosPatrulla[i].position + Vector3.up;
                Vector3 p2 = puntosPatrulla[i+1].position + Vector3.up;
                Gizmos.DrawLine(p1,p2);
            }
        }
        
    }

    #endregion
}
