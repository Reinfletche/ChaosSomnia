using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Enemigo : MonoBehaviour
{
    [SerializeField] private int _vida;
    [SerializeField] private float velocidadCaminar;
    [SerializeField] private float velocidadCorrer;

    //Patrulla
    [SerializeField] private Transform[] puntosPatrulla; 

    //Componentes
    #region Componentes
    private NavMeshAgent nav;
    private CapsuleCollider collider;
    private Animator animator;
    #endregion Componentes
    
    
    #region CORE
    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        collider = GetComponent<CapsuleCollider>();
        animator = transform.GetChild(0).GetComponent<Animator>();

        vision = visionInicial;
    }

    private void Start()
    {
        crPatrulla = CrPatrulla();
        StartCoroutine(crPatrulla);
        StartCoroutine(CrVision());
    }

    private void FixedUpdate()
    {
        FixedUpdate_Perseguir();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Patrulla"))
            StartCoroutine(CrPatrulla());
    }
    
    private void OnDrawGizmos()
    {
        GizmozPatrulla();
        GizmosVision();
    }
    #endregion CORE

    #region Stats
    private void Morir()
    {
        Parar();
        animator.SetTrigger("morir");
        //StopAllCoroutines();
        enabled = false;
       Destroy(gameObject,10);
    }
    
    public int Vida
    {
        get => _vida;
        set
        {
            _vida = value;

            EnPersecucion = true;
            vision = Vector3.Distance(transform.position, Jugador.Posicion) + 6;
            
            if(_vida <= 0) 
                Morir();
        }
    }
    #endregion < < Stats

    #region Movimiento
    public void Moverse(Vector3 posicion)
    {
        nav.speed = velocidadCaminar;
        nav.SetDestination(posicion);
        animator.SetBool("walk",true);
    }

    public void Correr(Vector3 posicion)
    {
        nav.speed = velocidadCorrer;
        nav.stoppingDistance = 2f;
        nav.SetDestination(posicion);
        animator.SetBool("run",true);
    }

    public void Parar()
    {
        nav.SetDestination(transform.position);
        animator.SetBool("walk",false);
    }
    #endregion < < Movimiento

    
    
    #region Patrulla

    private IEnumerator crPatrulla;
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

    private void GizmozPatrulla()
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
    
    
    #region Ataque
    private float vision;
    [SerializeField] private float visionInicial;
    [SerializeField] private float rangoAtaque;
    [SerializeField] private int damage;
    [SerializeField] private float cadencia = 5f;
    private bool _enPersecucion = false;
    private bool enCadencia = false;
    
    private void GizmosVision()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,vision);
    }

    private IEnumerator CrVision()
    {
        InicioVision:

        yield return new WaitForSeconds(0.1f);

        if (Physics.OverlapSphere(transform.position, vision, LayerMask.GetMask("Jugador")).Length > 0)
            EnPersecucion = true;
        else
            EnPersecucion = false;

        goto InicioVision;
    }

    private void FixedUpdate_Perseguir()
    {
        if (EnPersecucion)
        {
            Correr(Jugador.Posicion);

            if (!enCadencia)
            {
                if (Vector3.Distance(transform.position, Jugador.Posicion) < 3)
                    Atacar();
            }
        }
    }

    private void Atacar()
    {
        animator.SetTrigger("atacar");
        enCadencia = true;
        StartCoroutine(TimerDamage());
        StartCoroutine(TimerCadencia());
    }

    private IEnumerator TimerDamage()
    {
        yield return new WaitForSeconds(0.5f);
        Jugador.Vida -= damage;
    }
    
    private IEnumerator TimerCadencia()
    {
        yield return new WaitForSeconds(cadencia);
        enCadencia = false;
    }

    public bool EnPersecucion
    {
        get => _enPersecucion;
        set
        {
            if(_enPersecucion == value)
                return;
            
            _enPersecucion = value;

            if (_enPersecucion)
            {
                print("PERSEGUIR");
                vision = visionInicial * 2;
                StopCoroutine(crPatrulla);
            }
            else
            {
                vision = visionInicial;
                Parar();
                StartCoroutine(crPatrulla);
            }
        }
    }

    #endregion
}
