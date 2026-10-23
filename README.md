# Metal Gear Solid: Ghost Infiltration (TP2)

- **Estudiante:** Lucas Joaquín Aldapi, LU: TUV000559
- **Carrera:** Tecnicatura en Programación de Videojuegos (Facultad de Ingeniería - Universidad Nacional de Jujuy)

## 📖 Descripción del Juego
Prototipo de videojuego de infiltración y acción táctica desarrollado en **Unity** utilizando el pipeline gráfico **URP**. El jugador debe sortear un entorno forestal a gran escala (1000x1000), evitar la línea de visión de los enemigos centinelas guiados por Inteligencia Artificial avanzada con `NavMesh`, neutralizar amenazas y llegar al objetivo de la misión de manera sigilosa.

## 🎮 Guía de Controles
- **Movimiento:** Teclas `W`, `A`, `S`, `D`
- **Correr (Sprint):** Mantener `Shift` (Izquierdo o Derecho)
- **Salto:** `Barra Espaciadora` (Validado mediante esfera de contacto de suelo)
- **Disparar:** `Clic Izquierdo del Mouse`
- **Cambiar Vista (FPS / TPS):** Tecla `C`
- **Interactuar:** Tecla `E` (Detección por Raycast frontal y LayerMask)
- **Pausar / Menú:** Tecla `P` o `ESC` (Libera el cursor del mouse)

## ⚙️ Detalle de Opcionales Elegidos y Características Técnicas (5)
1. **Sistema de Cambio de Cámaras Dinámico (FPS/TPS):** Permite alternar la perspectiva en tiempo real para mejorar el control táctico de la infiltración. Programado en `CameraSwitcher.cs`.
2. **Chequeo de Suelo y Raycast de Interacción Avanzado:** Utiliza física integrada y filtrado por `LayerMask` para validar saltos y detectar terminales u objetos interactuables en el escenario. Programado en `PlayerController.cs`.
3. **IA de Patrulla, Búsqueda y Ataque con NavMesh:** Los enemigos centinela utilizan `NavMeshAgent` y una máquina de estados por `switch` para patrullar puntos de ruta, inspeccionar áreas aleatorias y perseguir/atacar al jugador en tiempo real. Programado en `EnemyAI.cs`.
4. **Sistema de Combate y Salud Dinámico (`HealthSystem`):** Gestión de daño mediante proyectiles físicos con detección de colisión continua (*Continuous Dynamic*), barras de vida flotantes en *World Space* orientadas hacia la cámara, y panel de *Game Over* interactivo.
5. **Gestor Centralizado de Estado y Puntuación (`GameManager`):** Control global de estados de juego (*Playing, Paused, GameOver, Victory*), sincronización de salud y un sistema de conteo automático de bajas enemigas reflejado en la interfaz.

## 📸 Capturas del Nivel en Ejecución
*(Acá podés insertar tus capturas de pantalla de Unity arrastrándolas al repositorio o vinculándolas)*
* **Vista en Tercera Persona (TPS):** <img width="856" height="369" alt="image" src="https://github.com/user-attachments/assets/8344d4cf-cfd9-4204-a72d-4cde0b843b49" />

* **Vista en Primera Persona (FPS) / NavMesh:** <img width="856" height="364" alt="image" src="https://github.com/user-attachments/assets/71acc65f-8697-4bf8-9402-17f33acaf658" />

