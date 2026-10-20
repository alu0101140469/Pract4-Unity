using UnityEngine;

// Controla al jugador del ejercicio 9 mediante fuerzas sobre un Rigidbody dinámico.
[RequireComponent(typeof(Rigidbody))]
public class MovimientoCuboFisico : MonoBehaviour
{
    // aceleración del cubo y velocidad máxima
    public float aceleracion = 18f;
    public float velocidadMaxima = 6f;

    // Referencias a los componentes del objeto.
    private Rigidbody cuerpo;
    private Vector3 direccionEntrada;

    private void Awake()
    {
        // Inicializa las referencias a los componentes.
        cuerpo = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Actualiza la dirección de entrada.
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Normaliza la dirección de entrada.
        direccionEntrada = new Vector3(horizontal, 0f, vertical);

        // Si la magnitud cuadrada de la dirección de entrada 
        // es mayor que 1, normaliza la dirección.
        if (direccionEntrada.sqrMagnitude > 1f)
            direccionEntrada.Normalize();
    }

    private void FixedUpdate()
    {
        // Aplica una fuerza al cuerpo para moverlo.
        cuerpo.AddForce(
            direccionEntrada * aceleracion,
            ForceMode.Acceleration
        );

        // Limita la velocidad horizontal y conserva la componente vertical para
        // que sigan funcionando la gravedad y los contactos con el suelo.
        Vector3 velocidadActual = cuerpo.linearVelocity;

        // Obtiene la velocidad horizontal.
        Vector3 velocidadHorizontal = new Vector3(
            velocidadActual.x,
            0f,
            velocidadActual.z
        );

        // Si la magnitud de la velocidad horizontal es mayor que la velocidad máxima,
        // limita la velocidad horizontal.
        if (velocidadHorizontal.magnitude > velocidadMaxima)
        {
            velocidadHorizontal =
                velocidadHorizontal.normalized * velocidadMaxima;

            cuerpo.linearVelocity = new Vector3(
                velocidadHorizontal.x,
                velocidadActual.y,
                velocidadHorizontal.z
            );
        }
    }
}