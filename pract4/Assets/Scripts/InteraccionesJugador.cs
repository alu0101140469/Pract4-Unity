using UnityEngine;


// Gestiona los contactos del jugador con las unidades y los objetos recolectables.
// Funciona con triggers y con colisiones físicas normales.
public class InteraccionesJugador : MonoBehaviour
{
    // Referencia al gestor de puntuación.
    public GestorPuntuacion gestorPuntuacion;

    private void OnTriggerEnter(Collider other)
    {
        // Se llama cuando un trigger entra en contacto con el jugador.
        ProcesarContacto(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Se llama cuando un objeto colisiona con el jugador.
        ProcesarContacto(collision.collider);
    }

    // Procesa el contacto con otro objeto.
    private void ProcesarContacto(Collider otroCollider)
    {
        // unidad que entra en contacto
        UnidadReaccionEventos unidad =
            otroCollider.GetComponentInParent<UnidadReaccionEventos>();

        // Comprueba si la unidad es válida.
        if (unidad != null)
        {
            // Si la unidad es válida, notifica el contacto.
            // Notifica el contacto con una unidad tipo 1.
            if (unidad.tipo == 1)
            {
                EventosJuego.NotificarCuboTocaUnidadTipo1();

                Debug.Log(
                    "El cubo ha contactado con una unidad tipo 1."
                );
            }
            // Notifica el contacto con una unidad tipo 2.
            else if (unidad.tipo == 2)
            {
                EventosJuego.NotificarCuboTocaUnidadTipo2();

                Debug.Log(
                    "El cubo ha contactado con una unidad tipo 2."
                );
            }

            return;
        }

        // Comprueba si el objeto es válido.
        ObjetoRecolectable objeto =
            otroCollider.GetComponentInParent<ObjetoRecolectable>();

        // Hasta que se asigna el gestor, los objetivos sirven como referencias
        // para movimiento y contacto y no se destruyen al tocarlos.
        if (objeto == null || gestorPuntuacion == null)
            return;
        // Si el objeto es válido, intenta recogerlo.
        if (objeto.IntentarRecoger(out int puntos))
            gestorPuntuacion.SumarPuntos(puntos);
    }
}