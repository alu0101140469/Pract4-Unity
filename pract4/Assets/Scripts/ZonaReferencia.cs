using UnityEngine;


// Se coloca en el objeto que representa la zona de referencia del ejercicio 4.
public class ZonaReferencia : MonoBehaviour
{
    // Método que se llama cuando un objeto entra en el trigger.
    private void OnTriggerEnter(Collider other)
    {
        // Comprueba si el objeto que entra en el trigger es el jugador.
        if (!other.CompareTag("Player"))
            return;

        // Si es el jugador, se publica el evento.
        Debug.Log("El cubo ha entrado en la zona de referencia.");
        EventosJuego.NotificarCuboEnZonaReferencia();
    }
}