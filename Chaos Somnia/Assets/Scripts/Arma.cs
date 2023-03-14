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
    public int cadencia;
    
    private int balasMax;
    public int balasCargador;
    private int _balas;
    
    public float tiempoRecarga;
    public float fuerzaEmpuje;

    public Transform disparador;
    
    public void Disparar()
    {
        StartCoroutine(TimerCadencia());

        switch (tipoArma)
        {
            case TipoArma.pistola or TipoArma.rifle:
                Ray ray = new Ray(disparador.position, disparador.forward);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Collider collider = hit.collider;
                    
                    if (collider.CompareTag("Enemigo"))
                    {
                        Enemigo enemigo = collider.GetComponent<Enemigo>();
                        enemigo.Vida -= damage;

                        Vector3 empuje = -enemigo.transform.forward * fuerzaEmpuje;
                        enemigo.Rb.AddForce(empuje,ForceMode.Impulse);
                    }
                }
                break;
        }
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

            if (_balas <= 0)
                return false;

            if (recargando)
                return false;
            
            return true;
        }
    }
    
}

public enum TipoArma
{
    pistola,
    escopeta,
    rifle
}
