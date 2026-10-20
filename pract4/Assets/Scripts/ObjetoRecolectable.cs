using UnityEngine;

// Define los tipos de objetos recolectables.
public enum TipoRecolectable
{
    Tipo1 = 1,
    Tipo2 = 2
}


// Identifica objetos recolectables. El tipo 1 vale 5 puntos y el tipo 2 vale 10.
public class ObjetoRecolectable : MonoBehaviour
{
    // El tipo de objeto recolectable.
    public TipoRecolectable tipo = TipoRecolectable.Tipo1;

    private bool recogido;

    // Indica si el objeto ha sido recolectado.
    public int TipoNumerico => (int)tipo;

    // Devuelve el valor del objeto recolectable.
    public int Valor =>
        tipo == TipoRecolectable.Tipo1 ? 5 : 10;

    // Intenta recoger el objeto. Devuelve true si se recoge correctamente.
    public bool IntentarRecoger(out int puntos)
    {
        puntos = 0;

        if (recogido)
            return false;

        recogido = true;
        puntos = Valor;

        Destroy(gameObject);
        return true;
    }
}