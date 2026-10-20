using TMPro;
using UnityEngine;


// Gestiona la puntuación, actualiza la UI y muestra una recompensa cada 100 puntos.
public class GestorPuntuacion : MonoBehaviour
{
    // Referencias a los elementos de la UI.
    public TMP_Text textoPuntuacion;
    public TMP_Text textoRecompensa;
    public GameObject panelRecompensa;

    // La puntuación actual.
    private int puntuacion;
    private int siguienteUmbral = 100;

    // Devuelve la puntuación actual.
    public int PuntuacionActual => puntuacion;

    private void Start()
    {
        // Actualiza el texto de la puntuación.
        ActualizarTextoPuntuacion();

        // Inicializa el texto de la recompensa.
        if (textoRecompensa != null)
            textoRecompensa.text = string.Empty;

        // Inicializa el panel de recompensa.
        if (panelRecompensa != null)
            panelRecompensa.SetActive(false);
    }

    // Suma puntos a la puntuación actual.
    public void SumarPuntos(int cantidad)
    {
        // Verifica que la cantidad sea positiva.
        if (cantidad <= 0)
            return;

        // Suma los puntos a la puntuación.
        puntuacion += cantidad;
        ActualizarTextoPuntuacion();

        // Muestra un mensaje en la consola con la puntuación actual.
        Debug.Log("Puntuación actual: " + puntuacion);

        // Comprueba si se ha alcanzado el siguiente umbral de puntos.
        while (puntuacion >= siguienteUmbral)
        {
            int numeroRecompensa = siguienteUmbral / 100;

            // Crea el mensaje de la recompensa.
            string mensaje = "¡Recompensa " + numeroRecompensa +
                             " desbloqueada! (" + siguienteUmbral +
                             " puntos)";

            if (textoRecompensa != null)
                textoRecompensa.text = mensaje;

            if (panelRecompensa != null)
                panelRecompensa.SetActive(true);

            Debug.Log(mensaje);

            // Actualiza el siguiente umbral.
            siguienteUmbral += 100;
        }
    }

    // Actualiza el texto de la puntuación.
    private void ActualizarTextoPuntuacion()
    {
        if (textoPuntuacion != null)
            textoPuntuacion.text = "Puntuación: " + puntuacion;
    }
}