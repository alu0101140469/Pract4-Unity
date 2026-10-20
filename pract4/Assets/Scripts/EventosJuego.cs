using System;


// Centraliza los eventos utilizados para comunicar objetos de la escena.
// Los suscriptores no necesitan conocer el objeto que publica el evento.
public static class EventosJuego
{
    // Eventos para notificar colisiones y otras interacciones en la escena.
    public static event Action CuboTocaCilindro;
    public static event Action CuboTocaUnidadTipo1;
    public static event Action CuboTocaUnidadTipo2;
    public static event Action CuboEnZonaReferencia;

    // Métodos para notificar los eventos.
    public static void NotificarCuboTocaCilindro()
    {
        // Se invoca el evento cuando el cubo toca el cilindro.
        CuboTocaCilindro?.Invoke();
    }

    public static void NotificarCuboTocaUnidadTipo1()
    {
        // Se invoca el evento cuando el cubo toca una unidad del tipo 1.
        CuboTocaUnidadTipo1?.Invoke();
    }

    public static void NotificarCuboTocaUnidadTipo2()
    {
        // Se invoca el evento cuando el cubo toca una unidad del tipo 2.
        CuboTocaUnidadTipo2?.Invoke();
    }

    public static void NotificarCuboEnZonaReferencia()
    {
        // Se invoca el evento cuando el cubo entra en la zona de referencia.
        CuboEnZonaReferencia?.Invoke();
    }
}