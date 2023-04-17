using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arma : MonoBehaviour
{
    [Header("Stats")] 
    public TipoArma tipoArma;

    //Estados
    public bool bloqueada = true;
    private bool enTimerCadencia = false;
    private bool recargando = false;
    
    public Sprite iconoBala;
    public int damage;
    public float cadencia;
    
    private int _balasReserva;
    [SerializeField] private int balasReservaMax;
    public int balasCargador = 0;
    private int _balas = 10;
    
    public float tiempoRecarga;
    public float fuerzaEmpuje;

    private Transform disparador;
    private Animator animator;
    private GameObject disparoEnemigoPf;
    
    private void Awake()
    {
        disparador = Camera.main.transform;
        animator = GetComponent<Animator>();
        disparoEnemigoPf = Resources.Load<GameObject>("disparoEnemigo");
    }

    public void Disparar()
    {
        print(PuedeDisparar);
        
        if (!PuedeDisparar)
            return;
        
        StartCoroutine(TimerCadencia());
        
        Balas--;

        switch (tipoArma)
        {
            case TipoArma.pistola or TipoArma.rifle:
                Ray ray = new Ray(disparador.position, disparador.forward);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Collider collider = hit.collider;
                    disparoV3 = hit.point;
                    print(hit.collider.name);
                    
                    if (collider.CompareTag("Enemigo"))
                    {
                        Enemigo enemigo = collider.GetComponent<Enemigo>();
                        enemigo.Vida -= damage;

                        Vector3 empuje = -enemigo.transform.forward * fuerzaEmpuje;
                        enemigo.Rb.AddForce(empuje,ForceMode.Impulse);

                        GameObject disparoEnemigo = Instantiate(disparoEnemigoPf, hit.point, Quaternion.identity);
                        Destroy(disparoEnemigo,2);
                    }
                }
                break;
            
            case TipoArma.escopeta:
                break;
        }
        
        print("Disparar");
        animator.SetTrigger("shoot");
    }

    private Vector3 disparoV3;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(disparador.position,disparoV3);

        Gizmos.color = Color.green;
        Vector3 apuntado = new Ray(disparador.position, disparador.forward).GetPoint(50);
        Gizmos.DrawLine(disparador.position,apuntado);
    }

    public IEnumerator TimerCadencia()
    {
        enTimerCadencia = true;
        yield return new WaitForSeconds(cadencia);
        enTimerCadencia = false;
    }

    public bool PuedeDisparar
    {
        get
        {
            if (enTimerCadencia)
                return false;

            if (balasCargador <= 0)
                return false;

            if (recargando)
                return false;
            
            return true;
        }
    }

    public int Balas
    {
        get => _balas;
        set
        {
            _balas = value;
            HUD.TextoBalas = _balas + "/" + _balasReserva;
        }
    }

    public int BalasReserva
    {
        get => _balasReserva;
        set
        {
            if (value > balasReservaMax)
                _balasReserva = balasReservaMax;
            else
                _balasReserva = value;

            if (gameObject.activeSelf)
                HUD.TextoBalas = Balas + "/" + BalasReserva;
        }
    }
}

public enum TipoArma
{
    pistola,
    escopeta,
    rifle
}
