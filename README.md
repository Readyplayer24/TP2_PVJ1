# Metal Gear Solid: Ghost Infiltration (TP2)

* **Estudiante:** Lucas Joaquín Aldapi, LU: TUV000559
* **Carrera:** Tecnicatura en Programación de Videojuegos (Facultad de Ingeniería)

## 📖 Descripción del Juego
Prototipo de videojuego de infiltración desarrollado en **Unity 6** utilizando el pipeline gráfico **URP**. El jugador debe sortear un terreno irregular, evitar la línea de visión de los drones centinelas guiados por Inteligencia Artificial (*NavMesh*) y llegar al objetivo de la misión de manera sigilosa.

## 🎮 Guía de Controles
* **Movimiento:** Teclas `W`, `A`, `S`, `D`
* **Salto:** `Barra Espaciadora` (Validado mediante Raycast de suelo)
* **Cambiar Vista (FPS / TPS):** Tecla `C`
* **Interactuar:** Tecla `E` (Detección por Raycast frontal y LayerMask)
* **Liberar Cursor:** Tecla `ESC`

## ⚙️ Detalle de Opcionales Elegidos (3)
1. **Sistema de Cambio de Cámaras Dinámico (FPS/TPS):** Permite alternar la perspectiva en tiempo real para mejorar el control táctico de la infiltración. Programado en `CameraSwitcher.cs`.
2. **Chequeo de Suelo y Raycast de Interacción Avanzado:** Utiliza `Physics.Raycast` filtrado por `LayerMask` para validar saltos y detectar terminales u objetos interactuables en el escenario. Programado en `PlayerController.cs`.
3. **IA de Patrulla y Persecución con NavMesh:** Los enemigos centinela utilizan `NavMeshAgent` para patrullar puntos aleatorios del terreno y cambiar al modo persecución al detectar al jugador. Programado en `EnemyAI.cs`.

## 📸 Capturas del Nivel en Ejecución
*(Acá podés insertar tus capturas de pantalla de Unity arrastrándolas al repositorio o vinculándolas)*
* **Vista en Tercera Persona (TPS):** ![TPS](ruta_o_link_imagen_1.png)
* **Vista en Primera Persona (FPS) / NavMesh:** ![FPS](ruta_o_link_imagen_2.png)
