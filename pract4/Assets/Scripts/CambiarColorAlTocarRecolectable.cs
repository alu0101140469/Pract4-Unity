using UnityEngine;

// Cambia el color de una unidad al tocar un objeto objetivo del tipo configurado.
// Se coloca en la raíz del prefab del humanoide.
public class CambiarColorAlTocarRecolectable : MonoBehaviour
{
    // Si es true, solo se cambia el color al tocar un objeto tipo 2.
    public bool soloTipo2 = true;
    public Color colorContacto = Color.yellow;

    private Renderer[] renderizadores;
    private bool yaCambioColor;

    private void Awake()
    {
        // Inicializa los renderizadores.
        renderizadores = GetComponentsInChildren<Renderer>(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Se llama cuando un trigger entra en contacto con el jugador.
        ComprobarContacto(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Se llama cuando un objeto colisiona con el jugador.
        ComprobarContacto(collision.collider);
    }

    private void ComprobarContacto(Collider otroCollider)
    {
        // Comprueba si el contacto es válido.
        if (yaCambioColor)
            return;

        // Obtiene el objeto recolectable del collider.
        ObjetoRecolectable objeto =
            otroCollider.GetComponentInParent<ObjetoRecolectable>();

        // Si el objeto no es válido, se ignora el contacto.
        if (objeto == null)
            return;

        // Si se está en modo solo tipo 2 y el objeto no es de tipo 2, se ignora el contacto.
        if (soloTipo2 && objeto.TipoNumerico != 2)
            return;

        // Cambia el color de los renderizadores.
        foreach (Renderer renderizador in renderizadores)
        {
            if (renderizador != null)
                renderizador.material.color = colorContacto;
        }

        // Marca que ya se ha cambiado el color.
        yaCambioColor = true;

        // Muestra un mensaje en la consola.
        Debug.Log(
            name + " ha cambiado de color al tocar un objetivo tipo " +
            objeto.TipoNumerico + "."
        );
    }
}