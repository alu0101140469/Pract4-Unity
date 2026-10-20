using UnityEngine;

// Se coloca en el cilindro. Publica el evento cuando el jugador entra
// en el trigger del cilindro.
public class NotificadorCilindro : MonoBehaviour
{
    // Método que se llama cuando un objeto entra en el trigger.
    private void OnTriggerEnter(Collider other)
    {
        // Comprueba si el objeto que entra en el trigger es el jugador.
        if (!other.CompareTag("Player"))
            return;

        // Si es el jugador, se publica el evento.
        Debug.Log("El cubo ha tocado el cilindro. Se publica el evento.");
        EventosJuego.NotificarCuboTocaCilindro();
    }
}