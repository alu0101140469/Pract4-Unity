using UnityEngine;

// Comportamiento reutilizable para las unidades tipo 1 y tipo 2.

// Se requiere un Rigidbody para el movimiento físico.
[RequireComponent(typeof(Rigidbody))]
public class UnidadReaccionEventos : MonoBehaviour
{
    // Tipo de unidad: 1 o 2.
    [Range(1, 2)]
    public int tipo = 1;

    // Descripción del tipo de unidad.
    [Tooltip("1 para unidad tipo 1, 2 para unidad tipo 2")]
    [Header("Destinos de movimiento")]
    public Transform destinoFijoTipo1;
    public Transform destinoCilindro;
    public Transform destinoGrupo2;

    // Destinos para el ejercicio 4.
    [Header("Ejercicio 4")]
    public Transform puntoTeletransporteTipo1;
    public Transform objetivoOrientacionTipo2;

    // Velocidades de movimiento y giro.
    [Header("Velocidades")]
    public float velocidadMovimiento = 2f;
    public float velocidadGiro = 360f;

    // Componentes y variables de estado.
    private Rigidbody cuerpo;
    private Transform destinoActual;
    private bool orientarHaciaObjetivo;

    // Se ejecuta antes de que el objeto se active.
    private void Awake()
    {
        // Se obtiene el componente Rigidbody.
        cuerpo = GetComponent<Rigidbody>();
    }

    // Se ejecuta cuando el objeto se activa.
    private void OnEnable()
    {
        // Establece los registros de eventos.
        EventosJuego.CuboTocaCilindro += ResponderCilindro;
        EventosJuego.CuboTocaUnidadTipo1 += ResponderCuboTocaTipo1;
        EventosJuego.CuboTocaUnidadTipo2 += ResponderCuboTocaTipo2;
        EventosJuego.CuboEnZonaReferencia += ResponderZonaReferencia;
    }

    // Se ejecuta cuando el objeto se desactiva.
    private void OnDisable()
    {
        // Cancela las suscripciones al desactivar o destruir la unidad.
        EventosJuego.CuboTocaCilindro -= ResponderCilindro;
        EventosJuego.CuboTocaUnidadTipo1 -= ResponderCuboTocaTipo1;
        EventosJuego.CuboTocaUnidadTipo2 -= ResponderCuboTocaTipo2;
        EventosJuego.CuboEnZonaReferencia -= ResponderZonaReferencia;
    }

    // Respuesta al evento de tocar el cilindro.
    private void ResponderCilindro()
    {
        // Ejercicio 1: tipo 1 va al objetivo prefijado y tipo 2 al cilindro.
        if (tipo == 1)
            IrA(destinoFijoTipo1);
        else
            IrA(destinoCilindro);
    }

    // Respuesta al evento de tocar una unidad tipo 2.
    private void ResponderCuboTocaTipo2()
    {
        // Ejercicio 3: al tocar el jugador una unidad tipo 2, las tipo 1
        // se dirigen al objetivo tipo 1 seleccionado.
        if (tipo == 1)
            IrA(destinoFijoTipo1);
    }

    // Respuesta al evento de tocar una unidad tipo 1.
    private void ResponderCuboTocaTipo1()
    {
        // Ejercicio 3: al tocar el jugador una unidad tipo 1, las tipo 1
        // se dirigen a su destino del grupo 2.
        if (tipo == 1)
            IrA(destinoGrupo2);
    }

    // Respuesta al evento de entrar en la zona de referencia.
    private void ResponderZonaReferencia()
    {
        if (tipo == 1 && puntoTeletransporteTipo1 != null)
        {
            // Asigna la posición del Rigidbody para realizar el teletransporte.
            cuerpo.position = puntoTeletransporteTipo1.position;
            destinoActual = null;
            Debug.Log(name + " se ha teletransportado al punto fijado.");
        }
        else if (tipo == 2)
        {
            // Activa la orientación hacia el objetivo para unidades tipo 2.
            orientarHaciaObjetivo = true;
        }
    }

    // Hace que la unidad se mueva hacia un destino.
    private void IrA(Transform destino)
    {
        // Verifica si el destino es válido.
        if (destino == null)
        {
            Debug.LogWarning(name + ": falta asignar uno de los destinos en el Inspector.");
            return;
        }

        // Asigna el destino y desactiva la orientación hacia el objetivo.
        destinoActual = destino;
        orientarHaciaObjetivo = false;
    }

    // Actualiza la posición y orientación de la unidad en cada frame fijo.
    private void FixedUpdate()
    {
        // Calcula la posición actual de la unidad.
        Vector3 posicionActual = cuerpo.position;

        // Verifica si hay un destino actual.
        if (destinoActual != null)
        {
            // El movimiento se restringe al plano XZ.
            Vector3 destinoPlano = new Vector3(
                destinoActual.position.x,
                posicionActual.y,
                destinoActual.position.z
            );

            // Calcula la dirección hacia el destino.
            Vector3 direccion = destinoPlano - posicionActual;
            Vector3 nuevaPosicion = Vector3.MoveTowards(
                posicionActual,
                destinoPlano,
                velocidadMovimiento * Time.fixedDeltaTime
            );

            // Verifica si la dirección es significativa.
            if (direccion.sqrMagnitude > 0.001f)
            {
                // Calcula la rotación objetivo.
                Quaternion rotacionObjetivo =
                    Quaternion.LookRotation(direccion.normalized);
                // Aplica la rotación al cuerpo.
                cuerpo.MoveRotation(Quaternion.RotateTowards(
                    cuerpo.rotation,
                    rotacionObjetivo,
                    velocidadGiro * Time.fixedDeltaTime
                ));
            }

            // Mueve la posición del cuerpo.
            cuerpo.MovePosition(nuevaPosicion);
        }

        // Si la orientación hacia el objetivo está activa y la unidad es 
        // del tipo 2, orienta hacia el objetivo.
        if (orientarHaciaObjetivo &&
            tipo == 2 &&
            objetivoOrientacionTipo2 != null)
        {
            Vector3 direccion =
                objetivoOrientacionTipo2.position - cuerpo.position;

            direccion.y = 0f;

            // Si la dirección es significativa, orienta la unidad.
            if (direccion.sqrMagnitude > 0.001f)
            {
                // Calcula la rotación objetivo.
                Quaternion rotacionObjetivo =
                    Quaternion.LookRotation(direccion.normalized);

                // Aplica la rotación al cuerpo.
                cuerpo.MoveRotation(Quaternion.RotateTowards(
                    cuerpo.rotation,
                    rotacionObjetivo,
                    velocidadGiro * Time.fixedDeltaTime
                ));
            }
        }
    }
}