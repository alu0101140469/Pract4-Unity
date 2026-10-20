using UnityEngine;

// Controla al jugador en los ejercicios 1 a 8 mediante un Rigidbody cinemático.
[RequireComponent(typeof(Rigidbody))]
public class MovimientoCuboKinematico : MonoBehaviour
{
    // Velocidad de movimiento del jugador.
    public float velocidad = 5f;

    private Rigidbody cuerpo;
    private Vector3 direccionEntrada;

    private void Awake()
    {
        // Inicializa la referencia al Rigidbody.
        cuerpo = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Obtiene la entrada del jugador.
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Normaliza la dirección de entrada.
        direccionEntrada = new Vector3(horizontal, 0f, vertical);

        // Si la magnitud cuadrada de la dirección es mayor que 1, 
        // normaliza la dirección.
        if (direccionEntrada.sqrMagnitude > 1f)
            direccionEntrada.Normalize();
    }

    // Actualiza la posición del cuerpo en cada frame fijo.
    private void FixedUpdate()
    {
        // Mueve la posición del cuerpo.
        cuerpo.MovePosition(
            cuerpo.position +
            direccionEntrada * velocidad * Time.fixedDeltaTime
        );
    }
}