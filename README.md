# Práctica 4 Unity: Delegados, Eventos

## Autor

Daniel Palenzuela Álvarez (alu0101140469)

## Introducción

En esta práctica he desarrollado una serie de escenas en Unity para trabajar con la comunicación entre objetos mediante eventos, el movimiento de personajes, la detección de colisiones, la recogida de objetos y la actualización de una interfaz de usuario. Los ejercicios se han realizado progresivamente, reutilizando los componentes y scripts de las primeras escenas en las siguientes.

## 2. Organización del código y patrón observador

Los scripts están separados por responsabilidad. `EventosJuego` declara los eventos que usan los demás componentes. `NotificadorCilindro` y `ZonaReferencia` detectan una interacción y publican el evento correspondiente. `UnidadReaccionEventos` escucha los eventos y ejecuta una respuesta según el tipo de unidad. `InteraccionesJugador` distingue los contactos del jugador con unidades y objetos recolectables. Por último, `GestorPuntuacion` mantiene la puntuación y actualiza la interfaz.

Este diseño aplica el patrón observador, donde el emisor no necesita tener una lista de todas las unidades que reaccionan, sino que cada unidad se suscribe a los eventos que le interesan. Las suscripciones se realizan cuando el componente se activa y se eliminan cuando se desactiva, para evitar que queden referencias a objetos que ya no participan en la escena.  

En los ejercicios 1–8 el cubo utiliza un Rigidbody cinemático. La entrada se lee en `Update`, y el desplazamiento del Rigidbody se calcula en `FixedUpdate` mediante `MovePosition`. Las unidades también usan rigidbodies cinemáticos y se desplazan hacia los destinos asignados. En el ejercicio 9, el cubo utiliza un Rigidbody dinámico y se mueve mediante `AddForce`, el motor de física calcula su movimiento y resuelve las colisiones.

### Scripts y su responsabilidad

| Archivo | Responsabilidad |
|---|---|
| `EventosJuego.cs` | Declara y publica los eventos. |
| `NotificadorCilindro.cs` | Publica un evento cuando el jugador entra en el trigger del cilindro. |
| `ZonaReferencia.cs` | Publica un evento al entrar el jugador en la zona de referencia. |
| `UnidadReaccionEventos.cs` | Suscribe cada unidad a los eventos y controla movimiento, orientación y teletransporte según el tipo. |
| `MovimientoCuboKinematico.cs` | Controla el cubo en los ejercicios 1–8 con un Rigidbody cinemático. |
| `ObjetoRecolectable.cs` | Identifica el tipo del objeto recolectable y su valor en puntos. |
| `GestorPuntuacion.cs` | Suma puntos, actualiza TextMesh y muestra recompensas por umbrales. |
| `InteraccionesJugador.cs` | Interpreta contactos del jugador con unidades y objetos recolectables. |
| `CambiarColorAlTocarRecolectable.cs` | Cambia el color de una unidad al contactar con un objetivo configurado. |
| `MovimientoCuboFisico.cs` | Controla el cubo del ejercicio 9 mediante fuerzas físicas. |

## Ejercicio 1. Esferas, cubo, cilindro y patrón observador

### Cómo lo he realizado

He creado una escena con un plano que sirve de suelo, un cubo que controla el jugador, un cilindro y cinco esferas. He distinguido los dos grupos mediante materiales: las esferas rojas son de tipo 1 y las esferas verdes son de tipo 2. Para cumplir la condición de tener cinco esferas, he creado tres esferas rojas y dos verdes.

Al cubo le he asignado el tag `Player`, un `Box Collider`, un `Rigidbody` cinemático y el script `MovimientoCuboKinematico`, que permite moverlo con las teclas de dirección o con W, A, S y D. El cilindro tiene un `Capsule Collider` con `Is Trigger` activado y el script `NotificadorCilindro`. Cada esfera tiene un color usando Material y un `Sphere Collider`, un `Rigidbody` cinemático y el script `UnidadReaccionEventos`. En este último he configurado el campo `Tipo` con el valor 1 o 2 y he asignado los destinos desde el Inspector.

He seleccionado una de las esferas verdes como destino fijo de las esferas de tipo 1. En las esferas de tipo 2 he asignado el cilindro como destino.

### Cómo funcionan los scripts

`NotificadorCilindro` detecta la entrada del objeto con tag `Player` en el trigger del cilindro. Cuando ocurre, publica el evento `CuboTocaCilindro` a través de `EventosJuego`. Esta clase centraliza las notificaciones para que el cilindro no tenga que conocer a cada esfera.

Cada instancia de `UnidadReaccionEventos` se suscribe a los eventos cuando se activa y elimina las suscripciones cuando se desactiva. Al recibir el evento del cilindro, si la unidad es de tipo 1 utiliza el destino fijo asignado; si es de tipo 2 utiliza el cilindro. El movimiento se realiza en `FixedUpdate` mediante `Rigidbody.MovePosition`, y las unidades también giran gradualmente hacia la dirección en la que se desplazan. `MovimientoCuboKinematico` lee la entrada del jugador y mueve el cubo mediante su Rigidbody.

### Qué ocurre al ejecutar

Al pulsar Play, muevo el cubo hasta entrar en el trigger del cilindro. El cilindro publica el evento y las esferas reaccionan sin que sea necesario que el cilindro las controle directamente: las rojas se dirigen hacia la esfera verde seleccionada y las verdes se acercan al cilindro. En la consola aparece un mensaje que confirma la activación del evento.

**Evidencia:** `pract4/media/ej1.gif`

## Ejercicio 2. Sustitución de las esferas por humanoides

### Cómo lo he realizado

He guardado una copia de la escena anterior para conservar el ejercicio 1. Después he importado los dos modelos humanoides indicados en el enunciado:

- [Mini Legion Footman](https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/mini-legion-footman-pbr-hp-polyart-86576).
- [Mini Legion Lich](https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/mini-legion-lich-pbr-hp-polyart-91497).

He sustituido las esferas por las instancias de los modelos. Para cada personaje he utilizado un objeto raíz que contiene el `Capsule Collider`, el `Rigidbody` cinemático y `UnidadReaccionEventos`, el modelo importado queda como hijo de la raíz. Así, el script mueve la raíz y el modelo visual la acompaña. He ajustado la altura y el centro de las cápsulas para que se adapten a cada personaje y he configurado el campo `Tipo` según el grupo al que pertenece.

He guardado las raíces de los personajes como prefabs dentro de `Assets/Prefabs`, para reutilizarlas y mantener la misma configuración al crear más unidades. Me salía rosa el personaje 3D porque el personaje venía preparado para el motor gráfico viejo y mi proyecto usa el nuevo (URP), y para arreglarlo, fui al menú de arriba en Window > Rendering > Render Pipeline Converter, seleccioné la opción de convertir los materiales a URP y le di a convertir.

### Cómo funcionan los scripts

En este ejercicio se reutilizan `EventosJuego`, `NotificadorCilindro`, `MovimientoCuboKinematico` y `UnidadReaccionEventos`. No es necesario cambiar la lógica de los eventos porque las reacciones se basan en el tipo de unidad y en sus destinos, no en que el objeto tenga forma de esfera. El cilindro sigue notificando el contacto y los humanoides suscritos responden de la misma forma que las esferas del ejercicio anterior.

### Qué ocurre al ejecutar

Al pulsar Play y llevar el cubo hasta el cilindro, los humanoides responden al evento: las unidades de tipo 1 se dirigen al destino seleccionado y las de tipo 2 al cilindro. De esta manera he comprobado que la lógica programada se puede reutilizar con modelos 3D diferentes sin tener que escribir de nuevo el comportamiento.

**Evidencia:** `pract4/media/ej2.gif`

## Ejercicio 3. Grupos de humanoides y escudos de dos tipos

### Cómo lo he realizado

He creado una copia de la escena de humanoides y he mantenido los dos grupos. Después he añadido escudos de tipo 1 y de tipo 2. Para empezar, cada escudo puede representarse mediante una primitiva, posteriormente he sustituido su representación visual por el modelo de escudo elegido, manteniendo en el objeto raíz los componentes necesarios para las colisiones.

He añadido `ObjetoRecolectable` a los escudos y he seleccionado `Tipo1` o `Tipo2` en el Inspector. A los escudos que deben comportarse como objetos físicos les he puesto un collider adecuado y un Rigidbody dinámico (con `Is Kinematic` desactivado), `Use Gravity` desactivado y las restricciones de posición y rotación necesarias para que permanezcan en su sitio sin dejar de participar en las colisiones. En las unidades tipo 1 he asignado un escudo de tipo 1 como destino fijo y un escudo de tipo 2 como destino del grupo 2.

También he añadido `CambiarColorAlTocarRecolectable` a las raíces de los humanoides. He configurado el tipo de objetivo que activa el cambio de color y he ajustado el detector de contacto para que las unidades puedan detectar los escudos.

### Cómo funcionan los scripts

`InteraccionesJugador` detecta el contacto del cubo con una unidad mediante `OnTriggerEnter` o `OnCollisionEnter`. Si la unidad es de tipo 2, publica el evento correspondiente; las unidades de tipo 1 lo reciben y se dirigen al escudo de tipo 1 asignado. Si el cubo contacta con una unidad de tipo 1, se publica el otro evento y las unidades de tipo 1 se dirigen al escudo de tipo 2 que se les ha asignado.

El movimiento y la reacción de los humanoides continúan gestionándose mediante `UnidadReaccionEventos`. Por otro lado, `CambiarColorAlTocarRecolectable` detecta si una unidad entra en contacto con un objeto que contiene `ObjetoRecolectable`. Si el tipo de objetivo coincide con la configuración, cambia el color de los renderizadores del personaje y registra el cambio en la consola. El cambio se realiza una sola vez por unidad.

### Qué ocurre al ejecutar

Al ejecutar la escena, si el cubo toca un humanoide de tipo 2, las unidades tipo 1 se desplazan hacia el escudo de tipo 1 seleccionado. Si el cubo toca un humanoide de tipo 1, las unidades tipo 1 se dirigen hacia el escudo de tipo 2 asignado. Cuando un humanoide toca un escudo del tipo configurado, cambia de color. Esto permite observar tanto la comunicación mediante eventos como la detección de contactos entre personajes y objetos.

**Evidencia:** `pract4/media/ej3.gif`

## Ejercicio 4. Zona de referencia, teletransporte y orientación

### Cómo lo he realizado

He guardado otra copia de la escena y he añadido un objeto vacío llamado `PuntoReferencia`. Le he agregado un `Sphere Collider` con `Is Trigger` activado para definir una zona de proximidad. A ese objeto le he añadido el script `ZonaReferencia`.

También he creado dos objetos vacíos que sirven como referencias: `PuntoTeletransporteTipo1`, que señala la posición de llegada de las unidades de tipo 1, y `ObjetivoMiradaTipo2`, que indica hacia dónde deben mirar las unidades de tipo 2. He asignado estos objetos en los campos correspondientes de `UnidadReaccionEventos` en el Inspector.

### Cómo funcionan los scripts

Cuando el jugador entra en el trigger de `PuntoReferencia`, `ZonaReferencia` comprueba que el objeto que ha entrado tiene el tag `Player` y publica el evento `CuboEnZonaReferencia` mediante `EventosJuego`.

Las instancias de `UnidadReaccionEventos` están suscritas a ese evento. Las unidades de tipo 1 asignan al Rigidbody la posición de `PuntoTeletransporteTipo1` y dejan de desplazarse hacia su destino anterior. Las unidades de tipo 2 activan la orientación hacia `ObjetivoMiradaTipo2`; en cada paso de física calculan la dirección horizontal y giran gradualmente hacia el objetivo.

### Qué ocurre al ejecutar

Al pulsar Play y acercar el cubo a la zona de referencia, el trigger publica el evento. Las unidades de tipo 1 se teletransportan al punto que he seleccionado y las unidades de tipo 2 giran para mirar hacia el objeto de orientación. El comportamiento se activa por proximidad y no requiere que el cubo empuje físicamente el punto de referencia.

**Evidencia:** `pract4/media/ej4.gif`

## Ejercicio 5. Recogida de escudos y puntuación en consola

### Cómo lo he realizado

He guardado una copia de la escena y he distribuido escudos recolectables de los dos tipos. A cada escudo le he añadido `ObjetoRecolectable`; los objetos de tipo 1 tienen un valor de 5 puntos y los de tipo 2 tienen un valor de 10 puntos.

He creado un objeto vacío llamado `GestorPuntuacion` y le he añadido el script del mismo nombre. Después he asignado este objeto al campo `Gestor Puntuacion` de `InteraccionesJugador`, que está en el cubo. He configurado los colliders de los escudos para detectar la recogida y he comprobado que cada escudo tiene el tipo correcto.

### Cómo funcionan los scripts

`InteraccionesJugador` procesa el contacto del jugador. Si el collider pertenece a una unidad, publica el evento correspondiente; si pertenece a un objeto con `ObjetoRecolectable` y hay un gestor de puntuación asignado, intenta recogerlo. `ObjetoRecolectable` comprueba que el objeto no se haya recogido anteriormente, devuelve el valor correspondiente y elimina el escudo de la escena.

`GestorPuntuacion` recibe la cantidad de puntos, incrementa el total y lo muestra en la consola. La comprobación interna de recogida evita que el mismo objeto se contabilice más de una vez.

### Qué ocurre al ejecutar

Al recoger un escudo de tipo 1, la puntuación aumenta en 5; al recoger uno de tipo 2, aumenta en 10. En la ventana Console aparece el total actualizado después de cada recogida y el escudo recogido desaparece de la escena.

**Evidencia:** `pract4/media/ej5.gif`

## Ejercicio 6. Interfaz de usuario para mostrar la puntuación

### Cómo lo he realizado

He partido de la escena del ejercicio 5 y he creado un `Canvas` desde el menú de UI de Unity. Dentro del Canvas he añadido un objeto de texto `Text - TextMeshPro` y lo he nombrado `TextoPuntuacion`. He utilizado el `RectTransform` para colocarlo en una esquina de la pantalla y he ajustado el tamaño, color y alineación para que se lea durante la partida.

Después he seleccionado el objeto `GestorPuntuacion` y he arrastrado el componente TextMeshPro de `TextoPuntuacion` al campo `Texto Puntuacion`. Así, el gestor tiene una referencia al texto que debe actualizar. Si Unity solicita importar los recursos esenciales de TextMeshPro, se importan para que el texto pueda mostrarse correctamente.

### Cómo funcionan los scripts

`GestorPuntuacion` mantiene la puntuación en una variable y actualiza el texto cuando comienza la escena y cada vez que recibe una nueva cantidad de puntos. `InteraccionesJugador` y `ObjetoRecolectable` siguen siendo responsables de detectar y procesar la recogida; la UI solo representa el total actual.

### Qué ocurre al ejecutar

Al empezar la partida, la interfaz muestra una puntuación inicial de cero. Cada vez que recojo un escudo, el número se actualiza automáticamente. Por ejemplo, después de recoger un escudo de tipo 1 y otro de tipo 2, el texto muestra 15 puntos. La puntuación aparece tanto en la interfaz como en la consola.

**Evidencia:** `pract4/media/ej6.gif`

## Ejercicio 7. Recompensa por cada 100 puntos

### Cómo lo he realizado

He partido de la escena del ejercicio 6 y he añadido un segundo texto TextMeshPro al Canvas llamado `TextoRecompensa`. Lo he colocado en una zona visible de la pantalla. Opcionalmente, he creado un panel que contiene el texto y lo he llamado `PanelRecompensa`.

En el componente `GestorPuntuacion` he asignado `TextoRecompensa` al campo `Texto Recompensa` y, si he utilizado el panel, he asignado también `PanelRecompensa` al campo `Panel Recompensa`. He dejado el panel desactivado al inicio para que solo aparezca cuando se alcance una recompensa.

### Cómo funcionan los scripts

`GestorPuntuacion` suma los puntos y mantiene un umbral inicial de 100. Cada vez que la puntuación alcanza o supera el umbral, prepara un mensaje de recompensa, lo muestra en el texto si está asignado, activa el panel si existe y registra el mensaje en la consola. Después incrementa el umbral en 100 puntos. De esta forma, puede mostrar las recompensas de 100, 200, 300 puntos y sucesivas, sin repetir la misma recompensa en cada actualización.

### Qué ocurre al ejecutar

Al recoger escudos hasta alcanzar 100 puntos, aparece el mensaje de la primera recompensa en la interfaz y en la consola. Si continúo recogiendo objetos hasta llegar a 200 puntos, el mensaje se actualiza para indicar la siguiente recompensa. Para comprobarlo he colocado suficientes objetos en la escena como para superar el primer umbral.

**Evidencia:** `pract4/media/ej7.gif`

## Ejercicio 8. Escena de prototipo que combina las mecánicas

### Cómo lo he realizado

He creado una escena llamada `PrototipoFinal` a partir de las escenas anteriores. He preparado un suelo amplio, una zona inicial, obstáculos con colliders, los dos grupos de humanoides, los escudos recolectables, el cilindro y la zona de referencia. También he colocado los puntos de teletransporte y de orientación usados en el ejercicio 4.

He mantenido el cubo controlable y la interfaz con puntuación y recompensa. He ajustado la cámara y la iluminación para que los elementos importantes se puedan ver durante la ejecución. He reutilizado los prefabs de los personajes y los objetos para mantener una configuración común entre las unidades.

### Cómo funcionan los scripts

En esta escena funcionan conjuntamente los scripts ya utilizados. `NotificadorCilindro` y `ZonaReferencia` publican los eventos de contacto o proximidad. `EventosJuego` permite comunicar esas notificaciones a las unidades suscritas. `UnidadReaccionEventos` decide si cada humanoide debe desplazarse, teletransportarse u orientarse según su tipo. `InteraccionesJugador` interpreta los contactos del cubo con los personajes y los objetos recolectables. `ObjetoRecolectable` determina el valor de cada escudo y `GestorPuntuacion` actualiza la UI y las recompensas.

### Qué ocurre al ejecutar

Al iniciar la escena se pueden comprobar varias mecánicas en un mismo entorno: el jugador activa el cilindro y la zona de referencia, contacta con humanoides de ambos tipos y recoge escudos. Los grupos reaccionan según sus eventos, el jugador acumula puntos y la interfaz muestra el total y las recompensas alcanzadas. Esta escena sirve para comprobar que los componentes funcionan conjuntamente y que las referencias asignadas en el Inspector son correctas.

**Evidencia:** `pract4/media/ej8.gif`

## Ejercicio 9. Repetir el ejercicio 3 con el cubo controlado por física

### Cómo lo he realizado

He duplicado la escena del ejercicio 3 y la he guardado como `Ejercicio09_CuboFisico`. He mantenido los humanoides, los escudos y sus scripts, pero he cambiado la configuración del cubo. He desactivado o quitado `MovimientoCuboKinematico` y he añadido `MovimientoCuboFisico`.

En el Rigidbody del cubo he desactivado `Is Kinematic`, activado `Use Gravity` y dejado el `Box Collider` como collider normal, sin `Is Trigger`. También he congelado las rotaciones X y Z para evitar que el cubo vuelque con facilidad. En los humanoides he utilizado colliders normales para los contactos físicos con el jugador; los triggers que sean necesarios para detectar proximidad se mantienen como detectores independientes.

### Cómo funcionan los scripts

`MovimientoCuboFisico` lee la entrada del jugador y aplica una fuerza al Rigidbody con `AddForce` durante `FixedUpdate`. Además, limita la velocidad horizontal máxima sin modificar la componente vertical de la velocidad, para conservar el efecto de la gravedad.

`InteraccionesJugador` conserva los métodos `OnTriggerEnter` y `OnCollisionEnter`, que envían los contactos al mismo método de procesamiento. De esta manera, cuando el jugador toca una unidad, se publica el evento correspondiente y `UnidadReaccionEventos` hace que las unidades de tipo 1 se dirijan al escudo que tengan asignado. El resto de la lógica del ejercicio 3 se reutiliza sin necesidad de cambiar los eventos.

### Qué ocurre al ejecutar

Al iniciar la escena, el cubo se mueve con aceleración y responde a la gravedad y a las colisiones del entorno. Al entrar en contacto con un humanoide tipo 2, las unidades tipo 1 se dirigen hacia el escudo de tipo 1 seleccionado. Al contactar con una unidad tipo 1, se activa el otro evento y las unidades tipo 1 se desplazan hacia el escudo de tipo 2 asignado. Así he comprobado que la misma mecánica se puede utilizar con el cubo controlado mediante un Rigidbody dinámico.

**Evidencia:** `pract4/media/ej9.gif`
