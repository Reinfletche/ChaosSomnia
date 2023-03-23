using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    //OBJETO
    [Header("Vida")] 
    [SerializeField] private Image barraVida;
    [SerializeField] private TMP_Text textoVida;

    [Header("Balas")] 
    [SerializeField] private Image iconoBala;
    [SerializeField] private TMP_Text textoBalas;
    
    private void Awake()
    {
        self = this;
    }

    //STATIC
    private static HUD self;
    
    public static float BarraVida
    {
        set => self.barraVida.fillAmount = value;
    }

    public static string TextoVida
    {
        set => self.textoVida.text = value;
    }

    public static Sprite IconoBala
    {
        set => self.iconoBala.sprite = value;
    }
    
    public static string TextoBalas
    {
        set => self.textoBalas.text = value;
    }
    
}
