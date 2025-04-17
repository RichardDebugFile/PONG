using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    float velocidadInicial = 5f;
    [SerializeField]
    float incrementoVelocidad = 0.5f;
    [SerializeField]
    float velocidadMaxima = 20f;

    int direccionActualX = 0;

    private Rigidbody2D rb;
    private float velocidadActual;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        velocidadActual = velocidadInicial;
        Invoke("LanzarPelota", 1f);
    }

    void LanzarPelota()
    {
        direccionActualX = Random.value < 0.5f ? -1 : 1;
        Vector2 direccion = new Vector2(direccionActualX, Random.Range(-0.5f, 0.5f)).normalized;
        rb.velocity = direccion * velocidadActual;
    }

    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Raqueta"))
        {
            float y = (transform.position.y - collision.transform.position.y) / collision.collider.bounds.size.y;
            y = Mathf.Clamp(y, -0.75f, 0.75f);
            Vector2 nuevaDireccion = new Vector2(rb.velocity.x > 0 ? 1 : -1, y).normalized;
            velocidadActual = Mathf.Min(velocidadActual + incrementoVelocidad, velocidadMaxima);
            rb.velocity = nuevaDireccion * velocidadActual;
        }
        
        else if (collision.gameObject.CompareTag("ParedIzquierda"))
        {
            ScoreManager.instancia.SumarPuntoComputadora();
            StartCoroutine(ReiniciarPelota());
        }
        else if (collision.gameObject.CompareTag("ParedDerecha"))
        {
            ScoreManager.instancia.SumarPuntoJugador();
            StartCoroutine(ReiniciarPelota());
        }

    }

    IEnumerator ReiniciarPelota()
    {
        rb.velocity = Vector2.zero;
        transform.position = Vector3.zero; //Desde la posicion deseada

        yield return new WaitForSeconds(1f); // espera antes de relanzar

        velocidadActual = velocidadInicial;
        LanzarPelota();
    }


}