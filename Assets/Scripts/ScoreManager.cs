using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int puntajeJugador = 0;
    public int puntajeComputadora = 0;

    public TextMeshProUGUI textoJugador;
    public TextMeshProUGUI textoComputadora;

    public static ScoreManager instancia;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SumarPuntoJugador()
    {
        puntajeJugador++;
        textoJugador.text = puntajeJugador.ToString();
    }

    public void SumarPuntoComputadora()
    {
        puntajeComputadora++;
        textoComputadora.text = puntajeComputadora.ToString();
    }
}
