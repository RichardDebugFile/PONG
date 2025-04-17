using UnityEngine;
using System.Collections;

public class Computer : MonoBehaviour
{
    [SerializeField] float velocidadBase = 4f;
    [SerializeField] Transform pelota;
    [SerializeField] float errorPosicional = 0.4f;
    [SerializeField] float suavizadoMovimiento = 2f;
    [SerializeField] float probabilidadError = 0.02f;
    [SerializeField] float tiempoReaccion = 0.1f;

    private Rigidbody2D rb;
    private float velocidadActual;
    private float objetivoY;
    private float tiempoUltimaDecision;
    private bool haciendoManiobraErronea;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        velocidadActual = velocidadBase;
        CalcularNuevoObjetivo();
    }

    void FixedUpdate()
    {
        if (pelota == null) return;

        // Actualizar objetivo con intervalo variable
        if (Time.time - tiempoUltimaDecision > tiempoReaccion)
        {
            if (Random.value < probabilidadError)
            {
                StartCoroutine(ManiobraErronea());
            }
            else
            {
                CalcularNuevoObjetivo();
            }
            tiempoUltimaDecision = Time.time;
        }

        if (!haciendoManiobraErronea)
        {
            MovimientoSuavizado();
        }
    }

    void CalcularNuevoObjetivo()
    {
        // Calcular objetivo con posible error
        float error = Random.Range(-errorPosicional, errorPosicional);
        objetivoY = pelota.position.y + error;

        // Ajustar velocidad según distancia al objetivo
        velocidadActual = velocidadBase * Mathf.Clamp(Mathf.Abs(objetivoY - transform.position.y), 0.5f, 2f);
    }

    void MovimientoSuavizado()
    {
        float distancia = objetivoY - transform.position.y;
        float direccion = Mathf.Clamp(distancia * suavizadoMovimiento, -1f, 1f);

        // Suavizar el movimiento
        float movimiento = direccion * velocidadActual * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + new Vector2(0, movimiento));
    }

    IEnumerator ManiobraErronea()
    {
        haciendoManiobraErronea = true;

        float direccionErronea = Random.Range(-1f, 1f);
        float duracionError = Random.Range(0.1f, 0.3f);

        float tiempoInicio = Time.time;
        while (Time.time - tiempoInicio < duracionError)
        {
            rb.MovePosition(rb.position + new Vector2(0, direccionErronea) * velocidadBase * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }

        haciendoManiobraErronea = false;
        CalcularNuevoObjetivo();
    }
}