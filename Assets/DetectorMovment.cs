using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorMovment : MonoBehaviour
{
    [Header("Booleans de dirección")]
    public bool subiendo; 
    public bool bajando;
    private Animator an;
    private Rigidbody2D rb;
    private Vector3 ultimaPosicion;
    public bool estaParado;
    public float timer;
    private float ultimaPosX;
    public bool mirandoDerecha = false;
    [Header("Sensibilidad")]
    public float umbral = 0.001f; 

    private float ultimaPosY;

    void Start()
    {
        ultimaPosY = transform.position.y;
        an = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ultimaPosicion = transform.position;
        ultimaPosX = transform.position.x;
    }

    void Update()
    {
        float posXActual = transform.position.x;
        float diferenciaX = posXActual - ultimaPosX;

        
        if (diferenciaX > 0.01f && !mirandoDerecha)
        {
            Girar();
        }
        
        else if (diferenciaX < -0.01f && mirandoDerecha)
        {
            Girar();
        }

        ultimaPosX = posXActual;
        estaParado = Vector3.Distance(transform.position, ultimaPosicion) < 0.001f;
        ultimaPosicion = transform.position;
        float posicionActualY = transform.position.y;
        float diferencia = posicionActualY - ultimaPosY;

        if (Mathf.Abs(diferencia) > umbral)
        {
            if (diferencia > 0)
            {
                subiendo = true;
                bajando = false;
            }
            else
            {
                subiendo = false;
                bajando = true;
            }
        }
        else
        {
            subiendo = false;
            bajando = false;
        }
        
        if(estaParado)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0f;
        }
            ultimaPosY = posicionActualY;
        an.SetFloat("Timer", timer);
        an.SetBool("Arriba", subiendo);
        an.SetBool("Abajo", bajando);


    }
    void Girar()
    {
        mirandoDerecha = !mirandoDerecha;

        Vector3 escala = transform.localScale;
        escala.x *= -1;        
        transform.localScale = escala;
    }

}
