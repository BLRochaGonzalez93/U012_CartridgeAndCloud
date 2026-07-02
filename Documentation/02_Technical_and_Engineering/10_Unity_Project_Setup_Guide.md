
---
title: "Cartridge & Cloud — Unity Project Setup Guide"
author: "VRM Games / Blas Luis Rocha González"
date: "2026-07-01"
lang: "es-ES"
document_version: "1.0"
status: "Fuente vigente de configuración y restauración del proyecto Unity"
project: "Cartridge & Cloud"
platform: "PC / Steam"
engine: "Unity 6.3 LTS 6000.3.18f1"
render_pipeline: "URP 17.3.0"
application_version_reference: "0.0.17"
---

# Cartridge & Cloud — Unity Project Setup Guide

## 0. Propósito

Esta guía define cómo instalar, clonar, abrir, validar, reparar y actualizar el proyecto Unity de
*Cartridge & Cloud* de forma reproducible. Su objetivo no es describir únicamente cómo crear un
proyecto nuevo, sino preservar la configuración real que permite continuar el desarrollo sin
introducir diferencias silenciosas entre máquinas, escenas, paquetes, assets o builds.

La guía cubre:

- requisitos de hardware y software;
- instalación exacta del Editor y módulos;
- restauración desde Git;
- configuración de `ProjectSettings` y `Packages`;
- estructura de `Assets/_Project`;
- límites de assemblies;
- escenas, Build Profiles y composition roots;
- URP, calidad, iluminación, audio, input y navegación;
- importación de modelos, texturas, animaciones y audio;
- prefabs, ScriptableObjects y catálogos;
- pruebas EditMode y PlayMode;
- limpieza, reimportación y recuperación;
- incorporación de una máquina nueva;
- procedimiento de cambio de Unity o paquetes;
- decisiones que requieren ADR;
- criterios de aceptación del setup.

No sustituye:

- al TDD, que define la arquitectura;
- al estándar C#, que define el código;
- a la guía de build y versionado, que define artefactos distribuibles;
- al QA Testing Plan, que define la estrategia de verificación.

## 0.1. Jerarquía de autoridad

Se aplica el siguiente orden:

1. `00_Enfoque_y_Alcance.md`
2. `01_Game_Design_Document.md`
3. `02_Vertical_Slice_Specification.md`
4. `03_Technical_Design_Document.md`
5. `04_Modelo_de_Datos.md`
6. `05_UX_Flow.md`
7. `06_Production_Roadmap_y_Sprint_Plan.md`
8. `07_QA_Testing_Plan.md`
9. `08_QA_Testing_Matrix.xlsx`
10. `09_CSharp_Coding_Standards.md`
11. Unity Project Setup Guide v0.6
12. `ProjectSettings`, `Packages`, `Assets` y `.asmdef` reales
13. ADR-0001 y ADR-0004
14. registros de entorno y paquetes de Sprint 0
15. versiones v0.5, v0.4 y v0.3

La configuración serializada real es la verdad sobre lo que abrirá Unity. Sin embargo, cuando
esa configuración contradiga un requisito vigente, no se reinterpreta como norma: se registra
como **deuda o migración pendiente**.

## 0.2. Etiquetas de estado

| Etiqueta | Significado |
|---|---|
| **OBLIGATORIO** | Debe estar presente para abrir, compilar o validar la baseline. |
| **OBSERVADO** | Valor encontrado en los archivos suministrados. |
| **OBJETIVO** | Configuración que debe alcanzarse antes del gate indicado. |
| **DEUDA** | Diferencia conocida entre el estado observado y el objetivo. |
| **OPCIONAL** | Puede añadirse tras justificar coste y dependencia. |
| **PROHIBIDO** | No debe hacerse sin una decisión formal. |
| **REQUIERE ADR** | Cambio estructural que necesita decisión, impacto y QA. |

## 0.3. Fotografía de la baseline

| Campo | Valor vigente |
|---|---|
| Proyecto | `Cartridge & Cloud` |
| Company | `VRM Games` |
| Editor | `6000.3.18f1` |
| Revisión del Editor | `5ebeb53e4c07` |
| Pipeline | URP `17.3.0` |
| Plataforma inicial | Windows x64 |
| Versión de aplicación | `0.0.17` |
| Application Identifier | `com.vrmgames.cartridgeandcloud` |
| Input | Input System Package |
| Color Space | Linear |
| Profile de calidad para Standalone | `PC` |
| Build Profile | `Windows_Development` |
| Pruebas documentadas | `1215 EditMode + 70 PlayMode = 1285 PASS` |
| Estado | Sprints 0–15 cerrados; Sprint 16 en curso |

Los resultados de pruebas son una referencia de la working copy de la baseline v0.6. Una
máquina nueva debe volver a ejecutarlos; no debe copiar su estado como si fuera evidencia propia.

---

# 1. Requisitos del equipo de desarrollo

## 1.1. Sistema operativo

El entorno validado utiliza Windows. La plataforma de build inicial es Windows x64.

La máquina debe disponer de:

- una versión de Windows todavía soportada por el Editor seleccionado;
- permisos para instalar Unity Hub, el Editor y módulos;
- rutas locales con permisos de lectura y escritura;
- espacio suficiente para proyecto, `Library`, builds y cachés;
- controlador gráfico estable;
- reloj del sistema correcto, para no distorsionar archivos, logs o certificados.

La evidencia histórica se obtuvo en Windows 10 Pro 19045 con un i9-9900K, 31,9 GB de RAM y
RTX 3060. Ese hardware no es un requisito mínimo oficial; es el entorno de referencia que debe
registrarse al comparar rendimiento.

## 1.2. Capacidad recomendada

Para trabajo cómodo:

- 16 GB de RAM como mínimo práctico; 32 GB recomendado;
- SSD con espacio libre amplio;
- GPU compatible con las capacidades requeridas por Unity 6 y URP;
- CPU multinúcleo moderna;
- monitor que permita validar al menos 1920×1080;
- segundo espacio de almacenamiento para artefactos o backups cuando sea posible.

No se debe inferir un requisito comercial de hardware a partir de estas recomendaciones. Los
requisitos de Steam se fijarán después del profiling de Sprint 17.

## 1.3. Herramientas

**OBLIGATORIO:**

- Unity Hub;
- Unity `6000.3.18f1`;
- Windows Build Support para esa instalación;
- IDE C# compatible;
- Git;
- acceso al repositorio;
- herramienta para revisar cambios Git.

El entorno histórico usa Visual Studio Community y GitHub Desktop. El repositorio contiene
`.vsconfig` con el workload `Microsoft.VisualStudio.Workload.ManagedGame`.

## 1.4. Privacidad del registro de entorno

Los registros pueden incluir:

- versión de Windows;
- CPU, RAM, GPU y driver;
- versión de Unity;
- versión del IDE;
- volumen de disco y espacio aproximado;
- versión, SHA y resultados.

No deben incluir:

- usuario del sistema;
- nombre del equipo;
- tokens;
- rutas personales completas;
- seriales;
- credenciales;
- claves de firma;
- correo privado no necesario.

---

# 2. Instalación de Unity

## 2.1. Instalar Unity Hub

1. Instalar Unity Hub desde un instalador confiable.
2. Iniciar sesión solo si la licencia lo requiere.
3. Activar una licencia válida.
4. Confirmar que Hub puede escribir en la ubicación de Editors y Projects.
5. No crear todavía un proyecto nuevo para reemplazar el repositorio.

## 2.2. Instalar el Editor exacto

Instalar **Unity 6.3 LTS `6000.3.18f1`**.

No utilizar:

- otra revisión `6000.3.x` por considerarla equivalente;
- Unity 6.0 o 6.1;
- una Tech Stream;
- una beta;
- una versión más nueva “compatible”.

`ProjectSettings/ProjectVersion.txt` contiene:

```text
m_EditorVersion: 6000.3.18f1
m_EditorVersionWithRevision: 6000.3.18f1 (5ebeb53e4c07)
```

Una actualización del Editor requiere el procedimiento del capítulo 45.

## 2.3. Módulos

Instalar:

- Windows Build Support;
- Microsoft Visual Studio Community o integración con el IDE elegido;
- documentación local solo si aporta valor;
- símbolos o herramientas de profiling cuando la campaña los requiera.

No instalar módulos móviles o de consola como supuesto compromiso de plataforma.

## 2.4. Validación de instalación

Antes de abrir el proyecto:

- verificar la versión en Hub;
- verificar Windows Build Support;
- confirmar licencia;
- comprobar espacio libre;
- registrar versión del IDE;
- comprobar que Git funciona.

---

# 3. Obtención del repositorio

## 3.1. Clonado

Clonar el repositorio completo, no descargar una carpeta parcial de `Assets`.

Deben estar presentes en la raíz:

```text
Assets/
Packages/
ProjectSettings/
Documentation/
Tools/
.gitignore
.gitattributes
.vsconfig
```

`Library`, `Temp`, `Obj`, `Logs`, `UserSettings` y builds locales se generan o permanecen
locales.

## 3.2. Ruta local

La ruta debe:

- ser local;
- evitar sincronización en tiempo real de servicios cloud;
- evitar permisos restringidos;
- evitar rutas extremadamente largas;
- disponer de espacio;
- no estar dentro de otra working copy Unity.

No abrir el proyecto desde un `.zip` ni desde una carpeta temporal.

## 3.3. Estado Git inicial

Antes de abrir:

1. comprobar rama;
2. hacer fetch/pull;
3. confirmar que no hay cambios locales;
4. registrar SHA;
5. verificar que no faltan archivos LFS, si en el futuro se introduce LFS;
6. no generar `.meta` manualmente.

## 3.4. Archivos generados por IDE

Los `.csproj`, `.sln` y `.slnx` generados por Unity están ignorados. El paquete recibido puede
contener copias para auditoría, pero la working copy normal debe regenerarlos desde Unity.

No corregir una referencia de assembly editando un `.csproj` generado.

---

# 4. Control de versiones y serialización

## 4.1. Visible Meta Files

`VersionControlSettings.asset` está configurado en `Visible Meta Files`.

Reglas:

- todo asset versionado conserva su `.meta`;
- mover assets se realiza dentro de Unity cuando sea posible;
- si se mueve fuera de Unity, se mueve también el `.meta`;
- no borrar `.meta` para “forzar” una reparación;
- no copiar un asset con el `.meta` original si se necesita un GUID nuevo;
- no regenerar el `.meta` de un prefab referenciado.

## 4.2. Force Text

`EditorSettings.asset` contiene `m_SerializationMode: 2`, correspondiente a la política de
serialización textual de la baseline.

Esto permite revisar y fusionar:

- escenas;
- prefabs;
- assets;
- materiales;
- animaciones;
- configuraciones.

Los conflictos no deben resolverse aceptando automáticamente “ours” o “theirs” sin comprender
las referencias.

## 4.3. Finales de línea

El repositorio normaliza a LF mediante `.gitattributes`:

- C#;
- JSON;
- Markdown;
- YAML;
- `.meta`;
- escenas;
- prefabs;
- assets;
- shaders y UI.

Los binarios se marcan como binary. La configuración serializada del Editor para nuevos scripts
no sustituye a `.gitattributes`.

## 4.4. `.gitignore`

Se ignoran, entre otros:

```text
Library/
Temp/
Obj/
Build/
Builds/
Logs/
UserSettings/
MemoryCaptures/
Recordings/
.vs/
.idea/
*.csproj
*.sln
*.slnx
```

Se versionan:

- `Assets` con `.meta`;
- `Packages`;
- `ProjectSettings`;
- documentación;
- herramientas;
- evidencia explícitamente aceptada.

## 4.5. Git LFS

**OBSERVADO:** no existe una política LFS activa en la `.gitattributes` suministrada. Los
binarios se clasifican como binarios normales.

**OBJETIVO:** introducir LFS solo cuando el crecimiento real de FBX, PSD, WAV o vídeo lo
justifique y exista un plan de migración. No activar patrones LFS parcialmente en mitad de un
sprint sin verificar clones limpios.

---

# 5. Primera apertura

## 5.1. Selección del proyecto

En Unity Hub:

1. elegir `Open`;
2. seleccionar la raíz que contiene `Assets`, `Packages` y `ProjectSettings`;
3. confirmar el Editor exacto;
4. aceptar la reimportación;
5. no seleccionar una subcarpeta.

## 5.2. Importación inicial

La primera apertura puede regenerar `Library` y tardar significativamente.

Durante la importación:

- no cerrar el Editor salvo bloqueo real;
- no mover assets;
- no ejecutar herramientas de autoría;
- no modificar Package Manager;
- no interpretar warnings transitorios como estado final.

## 5.3. Orden de diagnóstico

Después de la importación:

1. esperar que desaparezca la actividad;
2. revisar Console;
3. resolver errores rojos desde el primero;
4. comprobar Package Manager;
5. comprobar assemblies;
6. comprobar escenas;
7. ejecutar tests;
8. comprobar Git.

## 5.4. Safe Mode

Si Unity entra en Safe Mode:

- leer el primer error de compilación;
- corregir referencia, paquete o archivo;
- no borrar assets al azar;
- no reimportar todo como primer paso;
- no cambiar asmdefs para ocultar el problema;
- no salir de Safe Mode sin una compilación coherente.

## 5.5. Reimportación y Git

Tras una apertura limpia, Git no debe mostrar cambios inesperados. Cambios masivos en `.meta`,
escenas o materiales indican:

- Editor incorrecto;
- import settings diferentes;
- archivos faltantes;
- generación automática no idempotente;
- normalización no aplicada.

Detenerse antes de hacer commit.

---

# 6. Identidad y Player Settings

## 6.1. Valores obligatorios

| Campo | Valor |
|---|---|
| Company Name | `VRM Games` |
| Product Name | `Cartridge & Cloud` |
| Identifier Standalone | `com.vrmgames.cartridgeandcloud` |
| Bundle Version | `0.0.17` en la baseline |
| Color Space | Linear |
| Player Log | Activado |
| Input Handling | Input System Package |

## 6.2. Versión

No incrementar `bundleVersion` por cada apertura. Se cambia cuando el sprint o build lo exige y
se registra junto con SHA, schema y evidencia.

## 6.3. Valores observados que requieren revisión

La configuración suministrada contiene:

| Ajuste | Valor observado | Tratamiento |
|---|---:|---|
| resolución por defecto | `1024 × 768` | **DEUDA:** decidir antes de H6; QA usa 1920×1080 y otras resoluciones |
| ventana redimensionable | desactivada | validar decisión UX antes de release |
| run in background | desactivado | mantener salvo requisito explícito |
| fullscreen mode serializado | `1` | validar comportamiento en build, no inferir solo del YAML |
| VSync en calidad PC | `0` | el frame cap debe definirse y probarse |
| Player Log | activado | obligatorio en builds internas |
| Flip Model Swapchain | activado | validar en hardware objetivo |
| GPU Skinning | activado | medir con personajes representativos |
| deterministic compilation | activado | conservar |
| unsafe code | desactivado | conservar salvo ADR |

No cambiar varios de estos valores simultáneamente sin una build comparativa.

---

# 7. Paquetes

## 7.1. Paquetes directos aprobados

| Paquete | Versión | Uso |
|---|---:|---|
| `com.unity.ai.navigation` | `2.0.13` | navegación y NavMesh |
| `com.unity.ide.visualstudio` | `2.0.26` | integración IDE |
| `com.unity.inputsystem` | `1.19.0` | input |
| `com.unity.localization` | `1.5.12` | localización |
| `com.unity.render-pipelines.universal` | `17.3.0` | render pipeline |
| `com.unity.test-framework` | `1.6.0` | EditMode/PlayMode |
| `com.unity.ugui` | `2.0.0` | UI |

`manifest.json` y `packages-lock.json` se versionan juntos.

## 7.2. Paquetes retirados

El registro de Sprint 0 retiró:

- Rider Editor;
- Collab Proxy;
- Multiplayer Center;
- Visual Scripting;
- Timeline.

No reintroducirlos por plantilla o conveniencia sin uso real.

## 7.3. Política de versiones

- solo paquetes Released;
- no usar Preview;
- no editar `packages-lock.json` manualmente;
- no actualizar todos los paquetes a la vez;
- no aceptar una actualización automática sin diff;
- comprobar changelog y compatibilidad;
- ejecutar suites y build tras cada cambio.

## 7.4. Paquetes opcionales

| Paquete/capacidad | Condición |
|---|---|
| Cinemachine | spike que demuestre valor frente a la cámara actual |
| Addressables | volumen de contenido y estrategia de distribución justificados |
| Steamworks | build representativa y plan de integración aprobado |
| Git LFS | crecimiento del repositorio y migración aprobados |
| analyzers | plan gradual del estándar C# |

## 7.5. Paquetes built-in

El manifiesto incluye módulos built-in de Unity. No se interpretan como plataformas
comprometidas. No retirar módulos individuales a mano para “limpiar” el manifiesto sin comprobar
sus dependencias.

---

# 8. Estructura real de Assets

## 8.1. Raíz propia

Todo contenido propio vive bajo `Assets/_Project`.

```text
Assets/_Project/
├── Animations/
├── Art/
├── Audio/
├── Content/
├── Data/
├── Editor/
├── Prefabs/
├── Scenes/
├── Scripts/
├── Settings/
├── Tests/
├── UI/
└── InputSystem_Actions.inputactions
```

## 8.2. Organización por responsabilidad

- `Art`: modelos, texturas, materiales, sprites y VFX.
- `Audio`: ambience, music, SFX y UI.
- `Content`: localización y contenido de autoría no técnico.
- `Data`: catálogos y definiciones ScriptableObject.
- `Editor`: herramientas que solo compilan en Editor.
- `Prefabs`: arquitectura, muebles, productos, personajes y expansiones.
- `Scenes`: escenas versionadas.
- `Scripts`: capas y runtime.
- `Settings`: URP, Build Profiles y registros runtime.
- `Tests`: suites EditMode, PlayMode e Input System.
- `UI`: fuentes, iconos, estilos y vistas.

## 8.3. Reglas

- no crear carpetas genéricas `Misc`, `New Folder` o `Shared` para código;
- `Generated` solo para assets cuyo origen y regeneración estén documentados;
- separar assets técnicos de representativos;
- no guardar evidencia o builds dentro de `Assets`;
- no colocar scripts fuera de un assembly propio;
- no usar `Resources` como repositorio general.

## 8.4. Inventario observado

La baseline contiene aproximadamente:


| Área | Archivos sin `.meta` |
|---|---:|
| `Animations` | 11 |
| `Art` | 140 |
| `Audio` | 12 |
| `Content` | 3 |
| `Data` | 42 |
| `Editor` | 16 |
| `Prefabs` | 68 |
| `Scenes` | 5 |
| `Scripts` | 306 |
| `Settings` | 10 |
| `Tests` | 147 |
| `UI` | 4 |


Los conteos son una fotografía, no un objetivo. El gate valida integridad y función, no cantidad.

---

# 9. Assembly Definitions

## 9.1. Principio

Los assemblies reducen acoplamiento y tiempo de compilación, y protegen la simulación de
referencias indebidas a Unity.

Grafo principal:

```text
Domain
  ↑
Application
  ↑            ↑
Infrastructure Presentation
       \       /
        Runtime composition
```

## 9.2. Assemblies actuales

La baseline ya no contiene solo los seis assemblies de Sprint 0; existen doce:


| Assembly | Referencias directas | Plataforma | No Engine | Auto referenced |
|---|---|---|---:|---:|
| `VRMGames.CartridgeAndCloud.Editor.ProjectOrganization` | VRMGames.CartridgeAndCloud.Domain, VRMGames.CartridgeAndCloud.Infrastructure, VRMGames.CartridgeAndCloud.Presentation | Editor | No | No |
| `VRMGames.CartridgeAndCloud.Editor` | VRMGames.CartridgeAndCloud.Application, VRMGames.CartridgeAndCloud.Presentation, VRMGames.CartridgeAndCloud.Infrastructure.InputSystem, Unity.InputSystem | Editor | No | No |
| `VRMGames.CartridgeAndCloud.Application` | VRMGames.CartridgeAndCloud.Domain | Todas | Sí | No |
| `VRMGames.CartridgeAndCloud.Domain` | Ninguna | Todas | Sí | No |
| `VRMGames.CartridgeAndCloud.Infrastructure.InputSystem` | VRMGames.CartridgeAndCloud.Application, VRMGames.CartridgeAndCloud.Infrastructure, VRMGames.CartridgeAndCloud.Presentation, Unity.InputSystem | Todas | No | No |
| `VRMGames.CartridgeAndCloud.Infrastructure` | VRMGames.CartridgeAndCloud.Domain, VRMGames.CartridgeAndCloud.Application | Todas | No | No |
| `VRMGames.CartridgeAndCloud.Presentation` | VRMGames.CartridgeAndCloud.Domain, VRMGames.CartridgeAndCloud.Application | Todas | No | No |
| `VRMGames.CartridgeAndCloud.Runtime.VerticalSlicePhase1` | VRMGames.CartridgeAndCloud.Domain, VRMGames.CartridgeAndCloud.Application, VRMGames.CartridgeAndCloud.Infrastructure, VRMGames.CartridgeAndCloud.Presentation | Todas | No | Sí |
| `VRMGames.CartridgeAndCloud.Tests.EditMode` | VRMGames.CartridgeAndCloud.Domain, VRMGames.CartridgeAndCloud.Application, VRMGames.CartridgeAndCloud.Infrastructure, VRMGames.CartridgeAndCloud.Presentation, UnityEngine.TestRunner, UnityEditor.TestRunner | Editor | No | No |
| `VRMGames.CartridgeAndCloud.InputSystem.Tests.EditMode` | VRMGames.CartridgeAndCloud.Application, VRMGames.CartridgeAndCloud.Infrastructure.InputSystem, Unity.InputSystem, UnityEngine.TestRunner, UnityEditor.TestRunner | Editor | No | No |
| `VRMGames.CartridgeAndCloud.InputSystem.Tests.PlayMode` | VRMGames.CartridgeAndCloud.Application, VRMGames.CartridgeAndCloud.Infrastructure, VRMGames.CartridgeAndCloud.Infrastructure.InputSystem, VRMGames.CartridgeAndCloud.Presentation, UnityEngine.TestRunner | Todas | No | No |
| `VRMGames.CartridgeAndCloud.Tests.PlayMode` | VRMGames.CartridgeAndCloud.Domain, VRMGames.CartridgeAndCloud.Application, VRMGames.CartridgeAndCloud.Infrastructure, VRMGames.CartridgeAndCloud.Presentation, VRMGames.CartridgeAndCloud.Runtime.VerticalSlicePhase1, UnityEngine.TestRunner | Todas | No | No |


## 9.3. Reglas de dependencia

- Domain no referencia Unity ni otros assemblies propios.
- Application referencia Domain y no Unity.
- Infrastructure implementa puertos y referencia Domain/Application.
- Presentation referencia contratos, no implementaciones de Infrastructure.
- el assembly InputSystem encapsula el paquete de input;
- runtime compone dependencias explícitas;
- tests no entran en Player salvo constraints aprobadas;
- código Editor usa `includePlatforms: Editor`;
- nuevas referencias requieren revisión.

## 9.4. Root namespaces

Los asmdefs establecen root namespaces explícitos. `EditorSettings` no define un root namespace
global. No rellenarlo sin comprobar el impacto sobre plantillas y namespaces generados.

## 9.5. Assemblies de tests

Los PlayMode usan `UNITY_INCLUDE_TESTS` donde se necesita impedir compilación accidental en el
Player. Un fallo histórico de build demostró que esta restricción es obligatoria.

---

# 10. Escenas

## 10.1. Escenas versionadas

| Escena | Función | Estado |
|---|---|---|
| `Bootstrap.unity` | entrada y `ApplicationRoot` | producción |
| `MainMenu.unity` | menú y slots | producción |
| `Store.unity` | escena funcional anterior/fallback | migración |
| `StoreInitial.unity` | tienda autorada objetivo | Sprint 16 |
| `TestLab.unity` | pruebas aisladas | desarrollo |

## 10.2. GUIDs

Los GUIDs observados confirman que `StoreInitial` es un asset distinto:


| Escena | GUID |
|---|---|
| `Bootstrap` | `ada1e52ac6e09b9468e2109ccc7cbfc0` |
| `MainMenu` | `b00786c6952a55d418cf16503030c766` |
| `Store` | `45adcce906944f74bad3a0fb7d62f8e5` |
| `StoreInitial` | `32a95e5e16aaf15449a6f81392cf5dcf` |
| `TestLab` | `002b2ad62af3c614b91e32722c9452be` |


## 10.3. Flujo de producción

```text
Bootstrap → MainMenu → Slot Flow → StoreInitial
```

`Bootstrap` permanece primera. No entrar directamente en `MainMenu` o `StoreInitial` para
validar producción, salvo un test aislado que lo declare.

## 10.4. Estado de migración observado

`EditorBuildSettings.asset` y `Windows_Development.asset` todavía contienen:

```text
Bootstrap
MainMenu
Store
TestLab
```

`StoreInitial` existe, pero aún no sustituye a `Store` en el profile suministrado. Esto es
coherente con Sprint 16 en curso.

**PROHIBIDO:** cambiar la lista únicamente para aparentar el cierre. Primero deben conectarse:

- `StoreInitialSceneContext`;
- runtime;
- persistencia;
- catálogos;
- input;
- Golden Path;
- build externa.

## 10.5. TestLab

`TestLab` puede permanecer en un profile interno. La futura build candidata de hito debe decidir
si lo excluye. Nunca debe aparecer como opción de usuario.

---

# 11. Build Profiles

## 11.1. Profile observado

`Assets/_Project/Settings/BuildProfiles/Windows_Development.asset`:

- target Windows Standalone;
- desarrollo activado;
- compresión configurada;
- lista de escenas propia;
- profiler no conectado por defecto;
- deep profiling desactivado;
- debugging gestionado desactivado.

## 11.2. Reglas

- versionar el Build Profile;
- no depender de la lista global si el profile la sobrescribe;
- validar que Bootstrap es índice 0;
- no editar YAML a mano salvo recuperación excepcional;
- generar el profile desde Unity;
- registrar cambios de escenas;
- separar Development, QA, Candidate y Release cuando proceda.

## 11.3. Migración a StoreInitial

Cuando Sprint 16 cumpla sus pasos:

1. validar escena en Editor;
2. validar context;
3. desactivar blockout procedural en la ruta nueva;
4. ejecutar suites;
5. sustituir `Store` por `StoreInitial` en el profile;
6. generar build;
7. ejecutar EXE;
8. revisar `Player.log`;
9. conservar fallback hasta aceptar la migración;
10. registrar decisión de retirada.

---

# 12. ApplicationRoot y composición

## 12.1. Ownership

`ApplicationRoot` pertenece a Bootstrap y persiste durante transiciones. No crear copias en
MainMenu, Store o StoreInitial.

## 12.2. Composition roots

La composición debe:

- crear servicios una vez;
- inyectar contratos;
- registrar escenas;
- separar runtime de objetos visuales;
- evitar service locator global;
- fallar con diagnóstico si falta una referencia.

## 12.3. Preloaded assets

`ProjectSettings.asset` referencia como preloaded assets:

- `InputSystem_Actions.inputactions`;
- `RuntimeAssetRegistry.asset`.

Eliminar o sustituir estas entradas puede romper bootstrap y catálogos. Requiere tests de
arranque, input y registry.

---

# 13. StoreInitial

## 13.1. Principio de autoría

`StoreInitial.unity` se autora manualmente. Runtime registra y opera el entorno; no reconstruye
la arquitectura desde nombres, bounds o jerarquías de FBX.

## 13.2. Jerarquía objetivo

```text
StoreInitialEnvironment
├── Architecture
├── InitialFurniture
├── Lighting
├── Anchors
└── TechnicalColliders
```

## 13.3. StoreInitialSceneContext

Debe serializar referencias explícitas a:

- environment root;
- entrada y salida;
- recepción;
- checkout;
- puntos de clientes;
- roots de muebles y displays;
- colliders técnicos;
- iluminación o volumen necesarios;
- elementos de puerta.

Debe validar referencias antes de devolver control.

## 13.4. Estado runtime observado

`StoreRuntimeSettings.asset` todavía contiene:

```text
_storeSceneName: Store
_buildBlockoutOnLoad: 1
_hideOccludingWalls: 1
_showProcedurePanel: 1
_vfxPoolSize: 24
_maximumBlockoutCustomers: 4
```

Esto es una **DEUDA DE MIGRACIÓN**. No cambiar `_storeSceneName` y blockout hasta que la escena
nueva esté conectada y validada.

## 13.5. Arquitectura y grid

- tamaño aproximado: `10 × 15 m`;
- grid lógico: `20 × 30`;
- celda: `0,5 m`;
- colliders y grid son autoridad técnica;
- visual y colisión no deben contradecirse;
- entrada, checkout, almacén y recepción deben ser alcanzables.

## 13.6. Puerta

La puerta debe usar referencias explícitas. No deducir hojas por nombre. Validar:

- posición cerrada;
- dirección de apertura;
- colliders;
- trigger;
- navegación;
- estado de jornada;
- carga/restauración si aplica.

---

# 14. Runtime Asset Registry y catálogos

## 14.1. Registry

`RuntimeAssetRegistry.asset` centraliza referencias a:

- settings;
- content catalog;
- store shell;
- material palette;
- presentation catalog;
- audio catalog;
- representative prefab catalog.

## 14.2. Reglas

- no buscar catálogos con `Resources.FindObjectsOfTypeAll`;
- no duplicar registries por escena;
- mantener IDs estables;
- validar referencias nulas en Editor y runtime;
- no crear assets de catálogo en Play Mode;
- no reordenar datos si el orden tiene semántica sin migración.

## 14.3. Datos técnicos y representativos

Las carpetas `Data/*/Technical` contienen assets válidos para integración y pruebas. No deben
confundirse con contenido comercial final. Un catálogo representativo sustituye referencias, no
invariantes de dominio.

---

# 15. URP y Graphics Settings

## 15.1. Pipeline global

`GraphicsSettings.asset` apunta a `PC_RPAsset.asset`.

Valores observados:

- luces con intensidad lineal;
- temperatura de color activada;
- pipeline PC global;
- SRP Batcher activado en assets URP.

## 15.2. PC_RPAsset

| Ajuste | Valor observado |
|---|---:|
| HDR | activado |
| Render Scale | `1.0` |
| sombras de luz principal | activadas |
| sombras de luces adicionales | activadas |
| distancia de sombras | `50` |
| cascadas | `4` |
| SRP Batcher | activado |

## 15.3. Mobile_RPAsset

Existe como configuración secundaria:

- Render Scale `0.8`;
- una cascada;
- sombras adicionales desactivadas;
- SRP Batcher activado.

Su existencia no compromete una versión móvil.

## 15.4. Reglas URP

- no cambiar pipeline asset desde una escena;
- no duplicar materiales por error de shader;
- no convertir todos los materiales automáticamente sin revisión;
- no activar features caras sin profiling;
- probar profundidad, sombras y transparencias en build;
- mantener materiales compatibles con URP;
- registrar renderer features.

---

# 16. Quality Settings

## 16.1. Profiles observados

Existen dos perfiles:

- `Mobile`;
- `PC`.

Standalone usa `PC` y el índice actual es el profile PC.

## 16.2. Valores PC relevantes

- pixel lights: 2;
- sombras activadas;
- shadow distance: 40 en Quality Settings;
- dos cascadas en Quality Settings;
- MSAA/antiAliasing: 0 en Quality Settings;
- VSync: 0;
- LOD bias: 2;
- anisotropic textures: 2;
- mip streaming desactivado.

URP contiene ajustes adicionales; el comportamiento efectivo se valida visualmente y con
Profiler.

## 16.3. Deuda respecto a perfiles históricos

La guía v0.3 proponía Low/Medium/High, pero la baseline real usa Mobile/PC. No crear tres perfiles
por cumplir un documento antiguo. Primero definir necesidades de usuario y coste de mantenerlos.

## 16.4. Política

- cambiar un ajuste por hipótesis medible;
- registrar comparación antes/después;
- no optimizar sin profiler;
- validar en build;
- no usar calidad para ocultar assets mal importados;
- revisar sombras en StoreInitial.

---

# 17. Iluminación, volúmenes y VFX

## 17.1. Settings

La carpeta contiene:

- `DefaultVolumeProfile.asset`;
- `SampleSceneProfile.asset`;
- renderer assets;
- pipeline global.

## 17.2. Reglas

- un volumen global debe tener ownership claro;
- evitar múltiples volúmenes globales no intencionados;
- no usar postprocesado para compensar iluminación defectuosa;
- conservar legibilidad de UI;
- revisar exposición en entrada, displays, almacén y checkout;
- validar parpadeos y flashes;
- VFX críticos tienen alternativa visual o textual.

## 17.3. Baked, mixed y realtime

La elección debe basarse en:

- movilidad de luces;
- autoría de StoreInitial;
- coste de bake;
- iteración;
- memoria;
- rendimiento.

No iniciar un bake grande justo antes de un commit sin asegurar que los datos generados están
correctamente versionados o excluidos según política.

---

# 18. Input System

## 18.1. Configuración

El paquete `1.19.0` está instalado y `activeInputHandler` utiliza la ruta del Input System.
`InputSystem_Actions.inputactions` está preloaded.

## 18.2. Estado del asset

El asset contiene mapas y bindings generados por la plantilla, incluidos controles que no todos
corresponden al diseño final. La arquitectura real usa contextos y adaptadores propios.

**DEUDA:** revisar acciones genéricas como `Attack`, `Crouch` o `Jump` antes de considerarlas
contrato del juego.

## 18.3. Reglas

- no leer `Keyboard.current` desde Domain/Application;
- encapsular Input System en Infrastructure;
- Presentation consume contratos;
- UI tiene prioridad;
- bindings visibles se obtienen del sistema;
- no codificar iconos o nombres de teclas como texto fijo;
- tests de Input System permanecen en assemblies propios.

## 18.4. UI y gameplay

Antes de raycast de mundo, comprobar UI. Los contextos deben ser exclusivos:

```text
None
MainMenuUI
Exploration
Interaction
BuildMode
ModalUI
Paused
Results
```

Abrir o cerrar UI no debe mover, colocar, retirar ni cobrar.

---

# 19. UI y EventSystem

## 19.1. EventSystem

Cada escena operativa debe tener exactamente el EventSystem previsto por composición. No crear
uno en prefabs de entorno.

## 19.2. Reglas

- una UI modal captura input;
- el clic que cierra un panel no se propaga;
- Escape resuelve el nivel más profundo;
- el foco vuelve al control origen;
- los botones destructivos exigen confirmación;
- Canvas y escalado se validan en 1280×720, 1920×1080 y 2560×1440;
- no depender solo del color.

## 19.3. Carpetas

`UI/Fonts`, `UI/Icons`, `UI/Styles` y `UI/Views` existen como estructura. Mantener assets visuales
separados de lógica.

---

# 20. AI Navigation y NavMesh

## 20.1. Paquete

AI Navigation `2.0.13` es dependencia directa.

## 20.2. Areas observadas

Solo existen las áreas estándar:

- Walkable;
- Not Walkable;
- Jump.

No hay áreas propias registradas.

## 20.3. Reglas

- no crear un área por cada tipo de mueble;
- no cambiar costes sin caso de uso;
- placement debe invalidar o actualizar acceso según arquitectura;
- entrada, checkout y salida son puntos obligatorios;
- validar ruta antes de abrir tienda;
- un cliente bloqueado no impide cierre;
- no asumir que un collider visual es automáticamente navegación.

## 20.4. Bake y runtime

Documentar:

- surface;
- agent type;
- layers incluidas;
- volumen;
- overrides;
- método de actualización.

Un cambio de escala o puerta exige rebake o actualización coherente.

---

# 21. Physics y Time

## 21.1. TimeManager

Valores observados:

- Fixed Timestep: `0.02`;
- Maximum Allowed Timestep: `0.33333334`;
- Time Scale: `1`.

## 21.2. Reglas

- no usar `FixedUpdate` para lógica que no dependa de física;
- no cambiar timestep para corregir rendimiento;
- el tiempo de jornada usa un reloj lógico propio;
- pause no deja transacciones parciales;
- pruebas deterministas controlan el tiempo;
- no usar tiempo real local para simular días.

## 21.3. Colliders

- ajustar al volumen jugable;
- evitar colliders invisibles engañosos;
- usar colliders simples cuando sea posible;
- no usar MeshCollider convexo indiscriminadamente;
- validar puertas y pasillos;
- distinguir trigger y bloqueo físico.

---

# 22. Tags, Layers y Sorting Layers

## 22.1. Estado observado

- no existen tags personalizados;
- layers personalizadas no están definidas;
- Sorting Layer: `Default`;
- Rendering Layers: Default y Light Layer 1–7.

## 22.2. Consecuencia

El proyecto no debe depender de strings de tags inexistentes. La selección y composición se
resuelven mediante referencias, componentes y máscaras serializadas.

## 22.3. Añadir una layer

Antes de añadir:

1. definir uso;
2. comprobar si un componente basta;
3. asignar índice y nombre;
4. actualizar collision matrix;
5. actualizar cámaras y raycasts;
6. actualizar pruebas;
7. documentar.

Una layer no se renombra después de usarse sin auditar assets serializados.

---

# 23. Audio

## 23.1. AudioManager observado

| Ajuste | Valor |
|---|---:|
| volumen global | `1` |
| speaker mode | serializado `2` |
| sample rate | sistema (`0`) |
| DSP buffer | `1024` |
| virtual voices | `512` |
| real voices | `32` |

## 23.2. Organización

```text
Audio/
├── Ambience
├── Music
├── SFX
└── UI
```

## 23.3. Importación

- WAV fuente cuando proceda;
- compresión según duración y uso;
- Decompress On Load para clips breves críticos cuando esté justificado;
- Streaming para música larga tras medir;
- normalizar sin destruir dinámica;
- evitar clipping;
- loop points revisados;
- no duplicar el mismo clip por escena.

## 23.4. AudioCatalog

Los clips runtime se registran en catálogo. No usar rutas mágicas. Las alertas importantes
disponen de alternativa visual.

---

# 24. Localización

## 24.1. Paquete

Localization `1.5.12` está instalado.

## 24.2. Estado observado

`Content/Localization` contiene únicamente un placeholder `.gitkeep` en el paquete suministrado.
Esto significa que la infraestructura está preparada, pero no demuestra que tablas ES/EN estén
cerradas.

## 24.3. Objetivo

Antes de H6:

- definir locales ES/EN;
- crear String Tables;
- usar claves estables;
- eliminar textos visibles hardcoded;
- probar plurales y parámetros;
- probar formatos de moneda/fecha;
- validar expansión y truncamiento;
- persistir idioma.

## 24.4. Reglas

- no usar texto español como clave;
- no concatenar frases localizadas;
- no almacenar referencias a tablas por nombre variable sin validación;
- no borrar claves usadas sin migración;
- exportaciones de traducción deben conservar IDs.

---

# 25. Importación de assets: reglas generales

## 25.1. Antes de importar

Comprobar:

- licencia y procedencia;
- escala;
- ejes;
- unidades;
- pivote;
- nombres;
- materiales;
- texturas;
- animaciones;
- LODs;
- colliders;
- destino en carpetas.

## 25.2. Importar dentro de Unity

- copiar al destino final o a staging documentado;
- esperar importación;
- revisar Inspector;
- crear prefab;
- probar en escena aislada;
- registrar en catálogo si procede;
- no referenciar directamente el FBX desde todos los sistemas.

## 25.3. Herramientas representativas

La herramienta de importación representativa se ejecuta solo cuando cambian FBX o reglas de
importación. Debe ser idempotente.

No ejecutar herramientas masivas sobre una working copy sucia.

## 25.4. Nombres `_LOD0`

Los archivos principales pueden terminar en `_LOD0` y contener LOD0/1/2 internos. No interpretar
el sufijo como ausencia de otros niveles sin inspeccionar el asset.

---

# 26. Modelos FBX y LOD

## 26.1. Escala

- 1 unidad Unity equivale a 1 metro;
- validar tamaño con grid de 0,5 m;
- no corregir escala solo en el Transform de cada instancia;
- preferir import scale coherente;
- documentar excepciones.

## 26.2. Ejes y pivotes

- forward/up coherentes;
- pivote adecuado para placement;
- muebles apoyan en suelo;
- puertas pivotan según diseño;
- productos se alinean con slots.

## 26.3. LODGroup

- transferir LODs al prefab, no deducirlos en runtime;
- revisar thresholds en cámara real;
- evitar popping visible;
- LOD final puede usar simplificación o culling;
- colliders no cambian de forma impredecible con LOD;
- medir coste total.

## 26.4. Colliders

- arquitectura usa colliders técnicos explícitos;
- muebles colocables definen footprint y colisión;
- evitar MeshCollider de alta densidad;
- el collider no se deriva por bounds para arquitectura autorada.

---

# 27. Texturas, sprites y materiales

## 27.1. Texturas

Configurar:

- texture type;
- sRGB según naturaleza;
- normal map correctamente;
- max size;
- compresión;
- mipmaps;
- alpha;
- wrap/filter.

No usar la misma importación para UI, albedo, normal y máscaras.

## 27.2. Sprites/UI

- Sprite type apropiado;
- pivote consistente;
- pixels per unit documentado;
- atlas solo cuando exista estrategia;
- iconos legibles a escala real;
- evitar texto incrustado.

## 27.3. Materiales

- URP shaders;
- paleta registrada;
- no duplicar material por instancia;
- no modificar `sharedMaterial` en runtime sin intención;
- usar MaterialPropertyBlock para variaciones apropiadas;
- validar batching y memoria.

## 27.4. MaterialPalette

El catálogo de materiales actúa como fuente de referencias representativas. No usar búsquedas
por nombre de material.

---

# 28. Animaciones y personajes

## 28.1. Importación

- rig correcto;
- avatar validado;
- clips nombrados;
- loop revisado;
- root motion decidido;
- eventos de animación mínimos y documentados;
- compresión comparada visualmente.

## 28.2. Runtime

La animación no es autoridad del dominio. Un evento visual no debe confirmar una venta o
transferencia.

## 28.3. Identidad visual

- dependientes mantienen cohesión de VRM Games;
- clientes y proveedores pueden variar;
- perfiles lógicos no se identifican por nombre del prefab;
- los prefabs se registran mediante catálogo.

---

# 29. Prefabs

## 29.1. Principios

Un prefab debe tener:

- responsabilidad clara;
- nombre estable;
- componentes requeridos;
- referencias serializadas;
- collider/renderer coherentes;
- ausencia de managers globales;
- prueba o validación de autoría cuando sea crítico.

## 29.2. Prefab variants

Usar variants cuando comparten estructura y el vínculo aporta valor. Evitar cadenas profundas
de variantes que oculten overrides.

## 29.3. Overrides

Antes de aplicar overrides:

- revisar cada cambio;
- no aplicar transform de una instancia de escena accidentalmente;
- no aplicar referencias a objetos de escena;
- comprobar impacto en otras escenas.

## 29.4. Entorno

`StoreInitialEnvironment.prefab` contiene entorno fijo. No incluye EventSystem, HUD, cámara,
servicios, save ni `ApplicationRoot`.

---

# 30. ScriptableObjects y datos

## 30.1. Uso

ScriptableObjects son apropiados para:

- definiciones;
- catálogos;
- configuración;
- paletas;
- registry;
- settings de autoría.

No son el estado mutable autoritativo de una partida.

## 30.2. IDs

- IDs estables;
- no usar nombre de asset como identidad persistente;
- no regenerar IDs al reimportar;
- validar duplicados;
- conservar referencias durante rename/move.

## 30.3. Datos técnicos

Los assets técnicos permiten pruebas e integración. Deben estar claramente marcados y no
mezclarse con contenido representativo sin decisión.

## 30.4. Edición en Play Mode

Los cambios accidentales sobre ScriptableObjects pueden persistir. Las herramientas y tests
deben clonar o restaurar cuando proceda.

---

# 31. Resources y carga de assets

## 31.1. Estado

Existe `Assets/_Project/Resources`, pero no debe convertirse en ruta general de carga.

## 31.2. Política

Usar referencias serializadas, registries o catálogos. `Resources.Load` solo con justificación,
ruta estable y test.

No introducir Addressables hasta que se apruebe su coste y migración.

---

# 32. Código Editor

## 32.1. Ubicación

Herramientas Editor viven en:

- `Assets/_Project/Editor`;
- assemblies Editor-only.

## 32.2. Reglas

- usar Undo;
- marcar dirty correctamente;
- guardar assets explícitamente;
- preservar `.meta`;
- ser idempotente;
- mostrar resumen antes de cambios masivos;
- soportar dry-run cuando el riesgo lo justifique;
- no ejecutar automáticamente al abrir proyecto sin motivo crítico.

## 32.3. ProjectOrganization

Las herramientas de organización pueden crear, validar o reparar estructura. No deben mover
assets representativos silenciosamente ni alterar GUIDs.

---

# 33. Configuración de tests

## 33.1. Assemblies

Existen suites:

- EditMode general;
- PlayMode general;
- EditMode de Input System;
- PlayMode de Input System.

## 33.2. Baseline esperada

La working copy documentada reporta:

- 1215 EditMode PASS;
- 70 PlayMode PASS;
- total 1285 PASS.

La cantidad puede crecer. El requisito es que la suite vigente sea descubierta y pase, no
forzar siempre la cifra histórica.

## 33.3. Primera validación

1. abrir Test Runner;
2. ejecutar EditMode completo;
3. guardar reporte o captura;
4. ejecutar PlayMode completo;
5. esperar restauración de escenas;
6. revisar Console;
7. comprobar Git.

## 33.4. Fallos

- no pulsar Run repetidamente sin analizar;
- comprobar orden y contaminación de estado;
- comprobar assets generados;
- comprobar constraints de tests;
- comprobar escenas abiertas;
- registrar defecto si es producto;
- reparar test si la regla vigente cambió formalmente.

---

# 34. Validación de escenas y catálogos

## 34.1. Smoke tests

Después de una restauración:

- Bootstrap abre MainMenu;
- MainMenu muestra slots;
- Store funcional sigue disponible durante migración;
- StoreInitial abre sin referencias rotas cuando se valide;
- TestLab ejecuta fixtures aislados;
- registry carga catálogos;
- no hay managers duplicados.

## 34.2. Missing scripts

Buscar:

- `Missing (Mono Script)`;
- prefabs con referencias vacías;
- escenas con GUID no resuelto;
- materiales rosas;
- renderers sin mesh;
- catálogos con entradas nulas.

No guardar la escena hasta comprender la causa, porque Unity puede serializar pérdida de
referencias.

---

# 35. Validación de build mínima

La guía de build detallada será `11_Build_y_Versioning_Guide.md`. Para validar setup:

1. activar `Windows_Development`;
2. comprobar escenas;
3. generar en carpeta limpia fuera de Assets;
4. ejecutar EXE;
5. alcanzar MainMenu;
6. entrar en la escena configurada;
7. cerrar correctamente;
8. revisar `Player.log`;
9. comprobar Git limpio;
10. registrar versión y SHA.

Una build exitosa no valida por sí sola el Golden Path.

---

# 36. Player.log y diagnóstico

## 36.1. Player Log

Está activado y es obligatorio para builds internas.

Revisar:

- excepciones;
- errores repetidos;
- missing references;
- shader fallbacks;
- fallos de save;
- escenas ausentes;
- paquetes o plugins;
- warnings de rendimiento relevantes.

## 36.2. Evidencia

Archivar el log con:

- Build ID;
- versión;
- SHA;
- fecha;
- recorrido;
- resultado.

No publicar rutas personales sin sanitizar.

---

# 37. Limpieza segura

## 37.1. Qué puede regenerarse

Con Unity cerrado y Git limpio, pueden regenerarse:

- `Library`;
- `Temp`;
- `Obj`;
- archivos IDE;
- algunos caches locales.

## 37.2. Qué no debe borrarse

- `Assets`;
- `.meta`;
- `Packages/manifest.json`;
- `Packages/packages-lock.json`;
- `ProjectSettings`;
- Build Profiles;
- catálogos;
- escenas;
- documentación de evidencia.

## 37.3. Orden de recuperación

1. registrar error;
2. cerrar Unity;
3. guardar estado Git;
4. comprobar espacio/permisos;
5. retirar solo cache local necesaria;
6. abrir con Editor correcto;
7. esperar importación;
8. ejecutar tests;
9. comparar Git.

“Borrar Library” es una herramienta de diagnóstico de caché, no una solución universal.

---

# 38. Problemas habituales

## 38.1. Paquetes no resueltos

- confirmar conexión;
- revisar manifest/lock;
- comprobar Editor correcto;
- no borrar lockfile primero;
- revisar Package Manager log;
- comparar con baseline.

## 38.2. Materiales rosas

- comprobar pipeline;
- shader;
- renderer asset;
- importación;
- platform override;
- no convertir masivamente sin backup.

## 38.3. Scripts no compilan

- primer error;
- asmdef;
- namespace;
- paquete;
- define constraint;
- Editor-only API;
- archivo duplicado.

## 38.4. Tests no aparecen

- asmdef de test;
- reference a Test Runner;
- include platform;
- `UNITY_INCLUDE_TESTS`;
- errores de compilación;
- ubicación del script.

## 38.5. Build incluye tests

Verificar define constraints y referencias. No resolver eliminando tests.

## 38.6. StoreInitial muestra doble tienda

- revisar flag procedural;
- `StoreRuntimeSettings`;
- composition root;
- environment prefab;
- instancias duplicadas;
- escena y fallback.

## 38.7. UI mueve al jugador

- EventSystem;
- `IsPointerOverGameObject`;
- input context;
- orden de procesamiento;
- clic residual al cerrar panel.

## 38.8. Git muestra cientos de assets

- Editor incorrecto;
- herramienta masiva;
- reserialización;
- `.gitattributes`;
- import settings;
- cambios de pipeline.

No commit hasta clasificar.

---

# 39. Incorporación de una máquina nueva

## 39.1. Checklist previo

- [ ] Windows actualizado y estable.
- [ ] Drivers registrados.
- [ ] Unity Hub instalado.
- [ ] Editor `6000.3.18f1` instalado.
- [ ] Windows Build Support instalado.
- [ ] IDE y workload instalados.
- [ ] Git configurado.
- [ ] Acceso al repositorio confirmado.
- [ ] Espacio suficiente.

## 39.2. Checklist de clonación

- [ ] Clonado completo.
- [ ] Rama y SHA correctos.
- [ ] Working copy limpia.
- [ ] `Assets`, `Packages`, `ProjectSettings` presentes.
- [ ] `.gitignore` y `.gitattributes` presentes.
- [ ] No hay archivos faltantes.

## 39.3. Checklist de Unity

- [ ] Abre con Editor exacto.
- [ ] Paquetes resueltos.
- [ ] Cero errores rojos.
- [ ] Assemblies descubiertos.
- [ ] URP activo.
- [ ] Input actions disponibles.
- [ ] Runtime registry válido.
- [ ] Escenas presentes.
- [ ] EditMode pasa.
- [ ] PlayMode pasa.
- [ ] Build externa arranca.
- [ ] Player.log revisado.
- [ ] Git permanece limpio.

## 39.4. Registro

Crear o actualizar un Environment Record sin datos sensibles.

---

# 40. Checklist antes de un sprint

- [ ] Pull/fetch completado.
- [ ] SHA registrado.
- [ ] Working copy limpia.
- [ ] Editor correcto.
- [ ] Paquetes sin cambios no aprobados.
- [ ] Console limpia.
- [ ] Tests base pasan.
- [ ] Build Profile correcto.
- [ ] escena objetivo abre.
- [ ] catálogos validados.
- [ ] espacio libre.
- [ ] backup/evidencia preparada.
- [ ] alcance y exclusiones del sprint leídos.

---

# 41. Añadir un paquete

## 41.1. Definition of Ready

Antes de añadir:

- problema demostrado;
- alternativa sin paquete evaluada;
- versión Released;
- compatibilidad con Unity;
- licencia;
- mantenimiento;
- impacto de build;
- impacto de assemblies;
- plan de prueba;
- plan de retirada.

## 41.2. Procedimiento

1. working copy limpia;
2. rama/cambio aislado;
3. añadir desde Package Manager;
4. revisar manifest y lock;
5. esperar importación;
6. revisar dependencias transitivas;
7. compilar;
8. ejecutar suites;
9. generar build;
10. registrar decisión.

## 41.3. Rechazo

No añadir por:

- una única función trivial;
- tutorial desactualizado;
- “por si acaso”;
- plantilla;
- reducir unas pocas líneas a costa de dependencia permanente.

---

# 42. Actualizar un paquete

## 42.1. Un paquete cada vez

Evita mezclar causas de regresión.

## 42.2. Evidencia

- versión anterior/nueva;
- changelog;
- APIs afectadas;
- diff del manifest/lock;
- tests;
- build;
- Player.log;
- rendimiento si aplica;
- rollback.

## 42.3. URP/Input/Navigation

Estos paquetes afectan assets serializados y comportamiento. Sus upgrades requieren especial
atención a escenas, materiales, renderer assets, acciones, NavMesh y tests.

---

# 43. Actualizar Unity

## 43.1. REQUIERE ADR

Cambiar Unity requiere:

- motivación;
- versión objetivo LTS;
- compatibilidad de paquetes;
- copia/branch de upgrade;
- clone limpio;
- backup;
- reimportación;
- diff de ProjectSettings;
- suites completas;
- Golden Path;
- build;
- profiling;
- actualización documental.

## 43.2. Prohibiciones

- no abrir la única working copy con una versión superior;
- no aceptar resave masivo sin revisión;
- no mezclar upgrade con features;
- no borrar la baseline anterior antes de aprobar.

## 43.3. Rollback

Debe existir una ruta para volver a la versión y commit anteriores sin assets parcialmente
migrados.

---

# 44. Cambios que requieren ADR

- versión de Unity;
- render pipeline;
- plataforma objetivo;
- backend de scripting de producción;
- sistema de input;
- estrategia de Addressables;
- Steamworks;
- estructura principal de assemblies;
- ownership de escenas;
- sustitución de `ApplicationRoot`;
- estrategia de save;
- cambio de serialization mode;
- desactivar Visible Meta Files;
- introducir un DI container global;
- eliminar Store fallback antes del gate;
- cambiar estrategia de asset identity;
- introducir servicios online.

Un cambio menor de calidad o importación puede no necesitar ADR, pero sí evidencia si afecta al
gate.

---

# 45. Configuración que no debe cambiarse silenciosamente

| Área | Configuración protegida |
|---|---|
| Editor | `6000.3.18f1` |
| Serialization | Force Text |
| Version Control | Visible Meta Files |
| Color | Linear |
| Packages | versiones directas aprobadas |
| Scenes | Bootstrap primero |
| Assemblies | límites Domain/Application |
| Input | Input System y contextos exclusivos |
| Registry | referencias preloaded |
| Save | rutas y schema versionado |
| Build | Windows x64 y profile identificado |
| Identity | Company/Product/Identifier |

---

# 46. Deuda y discrepancias actuales

## 46.1. Tabla de deuda

| ID | Observación | Riesgo | Gate/acción |
|---|---|---|---|
| SETUP-DEBT-001 | Build Profile todavía usa `Store` | StoreInitial no está en build | cerrar Sprint 16 antes de sustituir |
| SETUP-DEBT-002 | `StoreRuntimeSettings` apunta a `Store` y blockout activo | doble shell o escena equivocada | migración controlada |
| SETUP-DEBT-003 | resolución por defecto `1024×768` | no representa objetivo de validación | decidir antes de H6 |
| SETUP-DEBT-004 | ventana no redimensionable | posible fricción PC | validar UX |
| SETUP-DEBT-005 | Localization sin tablas visibles | ES/EN no queda demostrado | completar en Sprint 17/H6 |
| SETUP-DEBT-006 | no hay tags/layers propios | no es defecto; limita máscaras nominales | añadir solo con caso real |
| SETUP-DEBT-007 | no hay `.editorconfig` ni analyzers | estilo no automatizado | plan gradual de Coding Standards |
| SETUP-DEBT-008 | no hay Git LFS | crecimiento de binarios | medir y decidir |
| SETUP-DEBT-009 | asset de Input incluye acciones genéricas | contrato confuso | limpiar sin romper IDs usados |
| SETUP-DEBT-010 | TestLab incluido en Development profile | no apto para release | profile de candidato separado |

## 46.2. Regla

Una deuda no se “arregla” modificando archivos sin criterio de aceptación. Cada acción debe
indicar responsable, prueba, build y rollback.

---

# 47. Definition of Done del setup

Una restauración o configuración de máquina está cerrada cuando:

1. usa Unity exacto;
2. tiene módulos requeridos;
3. clona la baseline completa;
4. Package Manager resuelve manifest y lock;
5. Console no tiene errores rojos;
6. Force Text y Visible Meta Files se conservan;
7. URP PC está activo;
8. assemblies compilan sin ciclos;
9. Domain/Application no tienen Engine References;
10. Bootstrap es primera escena del profile activo;
11. registry e input preloaded se resuelven;
12. escenas abren sin missing scripts;
13. EditMode pasa;
14. PlayMode pasa;
15. build Windows x64 arranca;
16. Player.log está revisado;
17. Git queda limpio;
18. Environment Record está actualizado;
19. diferencias aceptadas están registradas;
20. no se han introducido secretos.

---

# 48. Criterios de aceptación

| ID | Criterio | Evidencia |
|---|---|---|
| SETUP-001 | `ProjectVersion.txt` declara `6000.3.18f1`. | archivo y About Unity |
| SETUP-002 | Windows Build Support está disponible. | build externa |
| SETUP-003 | `manifest.json` contiene las siete dependencias directas aprobadas. | diff/revisión |
| SETUP-004 | `packages-lock.json` resuelve sin Preview. | Package Manager |
| SETUP-005 | Company, Product, Identifier y Version son correctos. | Player Settings |
| SETUP-006 | Color Space es Linear. | Project Settings |
| SETUP-007 | Visible Meta Files está activo. | Version Control Settings |
| SETUP-008 | Force Text está activo. | Editor Settings |
| SETUP-009 | Git ignora carpetas generadas. | status limpio |
| SETUP-010 | `.gitattributes` normaliza texto y clasifica binarios. | revisión |
| SETUP-011 | los doce asmdefs se descubren. | Inspector/compilación |
| SETUP-012 | Domain y Application usan No Engine References. | asmdefs |
| SETUP-013 | Bootstrap es primera escena del profile. | Build Profile |
| SETUP-014 | StoreInitial conserva GUID propio. | `.meta` |
| SETUP-015 | RuntimeAssetRegistry y Input Actions se resuelven. | arranque/tests |
| SETUP-016 | PC_RPAsset está asignado a Standalone. | Graphics/Quality |
| SETUP-017 | Input System funciona sin propagación UI→mundo. | PlayMode/manual |
| SETUP-018 | AI Navigation alcanza puntos críticos. | recorrido |
| SETUP-019 | no hay missing scripts ni materiales rosas. | revisión escena |
| SETUP-020 | EditMode vigente pasa. | reporte |
| SETUP-021 | PlayMode vigente pasa. | reporte |
| SETUP-022 | EXE Windows x64 arranca desde Bootstrap. | build record |
| SETUP-023 | Player.log no tiene errores bloqueantes. | log |
| SETUP-024 | Git permanece limpio tras abrir, probar y cerrar. | status |
| SETUP-025 | el registro de entorno no contiene secretos. | revisión |
| SETUP-026 | cambios de Unity/paquetes disponen de ADR o registro. | documentación |
| SETUP-027 | Store no se sustituye antes de validar StoreInitial. | gate Sprint 16 |
| SETUP-028 | Localization ES/EN se valida antes de H6. | QA matrix |
| SETUP-029 | resolución objetivo y ventana se deciden antes de candidato. | Player Settings/build |
| SETUP-030 | la guía y baseline reflejan la configuración real. | trazabilidad |

---

# 49. Mantenimiento

Actualizar esta guía cuando cambie:

- Editor;
- módulos;
- paquetes;
- ProjectSettings protegido;
- estructura principal;
- assemblies;
- escenas o Build Profiles;
- pipeline;
- input;
- navegación;
- herramientas de importación;
- proceso de apertura;
- criterios de QA.

No reescribir el histórico. Registrar qué decisión sustituye a la anterior.

El archivo debe guardarse como:

```text
Documentacion/
└── 10_Unity_Project_Setup_Guide.md
```

---


# 50. Mapa de configuración serializada

## 50.1. ProjectVersion

`ProjectVersion.txt` es el primer control de compatibilidad. Unity Hub puede mostrar varias
instalaciones con nombres similares; el archivo del proyecto es la referencia. La revisión
completa evita que dos builds con el mismo número visible pero distinta revisión se traten como
idénticas.

Comprobar siempre:

- `m_EditorVersion`;
- `m_EditorVersionWithRevision`;
- instalación seleccionada en Hub;
- versión registrada en build y evidencia.

## 50.2. ProjectSettings.asset

Este archivo contiene identidad, plataforma, resolución, logging, input y opciones de Player.
Debe revisarse por bloques, no como un diff opaco.

| Bloque | Campos de revisión | Riesgo de cambio |
|---|---|---|
| Identity | Company, Product, Identifier, Version | saves, distribución y trazabilidad |
| Display | resolución, fullscreen, resizable, background | UX y compatibilidad PC |
| Logging | Player Log | diagnóstico de QA |
| Rendering | color space, graphics APIs, batching | visual y rendimiento |
| Scripting | backend, unsafe, deterministic compilation | compatibilidad y build |
| Input | active input handler | pérdida total de controles |
| Preloaded | Input Actions y Runtime Registry | bootstrap y datos |

Un cambio de Player Settings debe registrar una captura o diff legible. No debe mezclarse con
una importación masiva de assets, porque dificultaría detectar la causa de una regresión.

## 50.3. EditorSettings.asset

Elementos protegidos:

- serialización textual;
- generación de proyectos;
- opciones de Play Mode;
- texture streaming en Editor y Play Mode;
- asset pipeline;
- cache server;
- prefab auto-save.

La baseline tiene Enter Play Mode Options activado con máscara serializada `0`. No se debe
interpretar el número sin comprobar el comportamiento de domain reload y scene reload en el
Editor usado. Cualquier optimización de entrada a Play Mode debe probar:

- estáticos;
- singletons;
- event subscriptions;
- `ApplicationRoot`;
- tests ejecutados en orden distinto;
- salida y reentrada en Play Mode.

## 50.4. GraphicsSettings y URP Global Settings

Revisar:

- pipeline global;
- shader stripping;
- renderer features;
- render graph o compatibilidad si cambia;
- luces lineales;
- volumen global;
- recursos de URP.

Una referencia de pipeline perdida puede convertir materiales en rosa y alterar todas las
escenas. No debe repararse asignando un asset arbitrario con nombre parecido; se verifica GUID y
profile de calidad.

## 50.5. QualitySettings

Cada profile tiene una referencia de pipeline. Al añadir un profile:

1. copiar solo como punto de partida;
2. asignar nombre y plataformas;
3. revisar pipeline asset;
4. revisar sombras, LOD, texturas y VSync;
5. probar cambio en runtime si se expone al usuario;
6. guardar métricas por profile;
7. actualizar QA Matrix.

## 50.6. TagManager

El archivo serializa índices. Cambiar el nombre de una layer no actualiza necesariamente todas
las expectativas humanas o herramientas externas. Reservar una tabla de ownership si se crean
layers propias.

## 50.7. TimeManager y Physics

Los valores se consideran parte de la simulación. Una modificación puede cambiar:

- velocidad aparente;
- resolución de colisiones;
- navegación;
- animación;
- reproducibilidad de tests;
- coste de CPU.

No cambiar timestep para “hacer que vaya más fluido”. Primero perfilar la causa.

## 50.8. AudioManager

Cambios de voces, buffer o sample rate pueden resolver un problema en una máquina y crear
latencia o cortes en otra. Toda modificación requiere:

- hardware de prueba;
- escena con carga representativa;
- registro de latencia/cortes;
- comparación antes/después;
- build externa.

## 50.9. EditorBuildSettings y Build Profiles

La lista global y la lista del profile pueden diferir. En Unity 6, el profile puede sobrescribir
la lista global. QA debe registrar qué profile estaba activo. Nunca asumir que modificar
`EditorBuildSettings.asset` basta si el profile usa `m_OverrideGlobalSceneList`.

---

# 51. Perfiles detallados de importación

## 51.1. Arquitectura

Para muros, suelo, marcos y elementos fijos:

- escala métrica verificada;
- pivote coherente con autoría;
- materiales URP;
- lightmap UV cuando se use baking;
- static flags según función;
- collider técnico separado cuando la malla visual sea compleja;
- sin scripts de gameplay incrustados en el FBX;
- prefab bajo `Prefabs/Architecture`;
- instancia bajo `StoreInitialEnvironment/Architecture`.

Criterio de aceptación: visual, collider, grid y navegación describen el mismo espacio.

## 51.2. Mobiliario colocable

- pivote en base y punto de colocación;
- footprint definido por datos, no por bounds cambiantes;
- orientación inicial conocida;
- LODGroup si aporta valor;
- colliders simples;
- slots o anchors explícitos;
- prefab separado del FBX;
- definición ScriptableObject con ID estable;
- miniatura/icono si aparece en UI;
- material compartido cuando corresponde.

Probar rotaciones 0°, 90°, 180° y 270°. Un mueble visualmente simétrico puede tener footprint o
puntos de interacción asimétricos.

## 51.3. Displays

Además de mobiliario:

- capacidad lógica;
- slots visuales;
- productos compatibles;
- origen de reposición;
- punto de interacción;
- representación de vacío/parcial/lleno;
- persistencia de asignación.

No usar el número de hijos visuales como capacidad autoritativa.

## 51.4. Productos y packaging

- escala consistente entre familias;
- orientación frontal;
- punto de apoyo;
- materiales legibles;
- LOD si el volumen lo requiere;
- prefab registrado en catálogo;
- ID de producto independiente del nombre del prefab;
- representación de caja separada cuando el flujo la necesite.

Una carátula o etiqueta no debe incluir marcas reales sin autorización.

## 51.5. Personajes

- rig y avatar;
- escala;
- clips requeridos;
- materiales;
- LOD;
- collider/cápsula;
- punto de navegación;
- perfil lógico separado;
- variantes visuales sin cambiar identidad de dominio;
- animator controller compatible.

Validar con varios agentes simultáneos, no solo un personaje aislado.

## 51.6. Texturas de entorno

- resolución proporcional a tamaño en pantalla;
- tiling;
- normal map;
- mask maps si el shader las utiliza;
- compresión de Standalone;
- mipmaps;
- anisotropic filtering para superficies oblicuas;
- ausencia de seams críticos.

## 51.7. UI

- Sprite (2D and UI);
- alpha correcto;
- compresión que no introduzca halos;
- nine-slice cuando proceda;
- tamaño y PPU coherentes;
- atlas evaluado por lote;
- variantes de escala;
- contraste y legibilidad.

## 51.8. Audio por categoría

| Categoría | Importación inicial | Validación |
|---|---|---|
| UI | clip corto, latencia baja | repetición y volumen |
| SFX | según duración y concurrencia | clipping y voice count |
| Ambience | loop estable | transición y memoria |
| Music | streaming tras medir | seek, loop y carga |
| Voz | compresión orientada a claridad | subtítulos y localización |

## 51.9. Animaciones

- separar clips correctamente;
- revisar loop pose;
- decidir root motion;
- retirar curvas no usadas si aporta ahorro;
- no depender de nombres de clips generados por DCC;
- mantener una tabla de eventos permitidos;
- validar transición en Animator.

## 51.10. Presets

Los Presets de importación pueden reducir errores, pero no deben aplicarse a todos los assets por
extensión. Crear presets por categoría y documentar su filtro. Cambiar un preset puede
reimportar cientos de assets; se trata como cambio de alto riesgo.

---

# 52. Matriz de validación por tipo de cambio

| Cambio | Compilación | EditMode | PlayMode | Escena manual | Build | Player.log | Profiling |
|---|---:|---:|---:|---:|---:|---:|---:|
| C# Domain | sí | completa afectada | regresión | según sistema | gate | sí | si ruta crítica |
| C# Presentation | sí | afectada | completa UI/escena | sí | sí | sí | cuando aplique |
| asmdef | sí | completa | completa | smoke | sí | sí | no habitual |
| paquete | sí | completa | completa | Golden Path | sí | sí | según paquete |
| ProjectSettings | sí | afectada | afectada | sí | sí | sí | según ajuste |
| Build Profile | sí | smoke | smoke | flujo de escenas | obligatorio | obligatorio | no |
| escena | sí | autoría | completa relevante | obligatorio | obligatorio | obligatorio | si rendimiento |
| prefab crítico | sí | autoría | integración | obligatorio | gate | sí | si masivo |
| catálogo | sí | validación | integración | flujo afectado | gate | sí | no habitual |
| textura/material | no siempre | authoring | visual | obligatorio | recomendado | shader warnings | GPU/memoria |
| FBX/LOD | no siempre | authoring | visual/nav | obligatorio | recomendado | warnings | CPU/GPU |
| audio | no siempre | catálogo | audio | obligatorio | recomendado | warnings | voces/memoria |
| localización | sí si código | claves | UI | ES/EN | sí | missing keys | no |
| Unity upgrade | completa | completa | completa | Golden Path | obligatoria | obligatorio | obligatorio |

## 52.1. Cambio pequeño no significa riesgo pequeño

Un cambio de una línea en el Build Profile puede impedir arrancar. Un rename de asset puede
romper referencias si pierde su `.meta`. La profundidad de QA se decide por impacto, no por
número de archivos.

## 52.2. Validación de autoría

Las herramientas deben poder detectar:

- referencias nulas;
- IDs duplicados;
- prefabs sin componentes;
- catálogos inconsistentes;
- LODGroup incompleto;
- materiales incompatibles;
- colliders ausentes;
- escenas fuera de profile;
- assets técnicos en rutas representativas.

## 52.3. Validación posterior a merge

Aunque una rama pasara tests, después de merge se vuelve a comprobar:

- resolución de conflictos;
- paquete lock;
- escenas/prefabs;
- suites afectadas;
- build de gate;
- Git limpio.

---

# 53. Recuperación ante daños o pérdida de referencias

## 53.1. Principio

La recuperación conserva evidencia antes de intentar reparar. No sobrescribir el único estado
que muestra el problema.

## 53.2. Escena dañada

1. cerrar Unity sin guardar si la pérdida apareció al abrir;
2. copiar la escena y `.meta` fuera de la working copy para análisis;
3. revisar Git diff;
4. identificar GUID o script ausente;
5. restaurar desde commit si procede;
6. reabrir con Editor correcto;
7. validar referencias;
8. ejecutar tests y recorrido;
9. documentar causa.

No copiar únicamente la escena de otra rama sin sus prefabs y datos relacionados.

## 53.3. Prefab con Missing Script

- localizar GUID del script en YAML;
- comprobar `.meta` del script;
- comprobar assembly y namespace;
- comprobar si el tipo fue renombrado;
- restaurar o migrar explícitamente;
- no pulsar Remove Component en masa.

## 53.4. Material perdido

- localizar GUID;
- comprobar material y `.meta`;
- comprobar shader;
- comprobar pipeline;
- revisar cambios de importación;
- restaurar referencia, no crear clones con nombres similares.

## 53.5. Catálogo roto

- no iniciar una partida que pueda persistir estado parcial;
- ejecutar validador;
- comparar IDs;
- restaurar asset;
- comprobar registry;
- ejecutar tests de datos y save/load.

## 53.6. Manifest dañado

- restaurar desde Git;
- no reconstruir versiones “de memoria”;
- conservar lockfile correspondiente;
- abrir y esperar resolución;
- comprobar que no aparecen paquetes Preview o de plantilla.

## 53.7. Library corrupta

Síntomas posibles:

- imports incoherentes;
- cache de shader;
- referencias que vuelven tras reinicio;
- errores internos de importer.

Procedimiento:

1. confirmar que archivos versionados están sanos;
2. cerrar Unity/Hub;
3. renombrar `Library` en lugar de borrar si se necesita análisis;
4. abrir y reimportar;
5. comparar resultado;
6. eliminar backup local solo después de validar.

## 53.8. Conflicto de escena o prefab

Si el conflicto no es trivial:

- no editar a ciegas marcadores de merge;
- identificar objetos y fileIDs;
- considerar rehacer una parte en Unity;
- comparar ambos cambios;
- validar escena completa;
- documentar trabajo descartado.

---

# 54. Seguridad y secretos

## 54.1. Archivos prohibidos en Git

- tokens de Steam o servicios;
- contraseñas;
- certificados privados;
- keystores;
- `.env` reales;
- logs con datos personales;
- dumps de memoria sin revisar;
- configuraciones de máquina con usuario/rutas privadas.

## 54.2. Variables y configuración local

Cuando se introduzcan servicios externos:

- plantilla sin secretos;
- almacenamiento local seguro;
- inyección en build/CI;
- rotación;
- permisos mínimos;
- documentación de recuperación.

## 54.3. Assets de terceros

Registrar:

- origen;
- licencia;
- versión;
- modificaciones;
- restricciones de redistribución;
- créditos.

No incluir packages o samples de terceros sin verificar qué archivos pueden versionarse.

## 54.4. Logs y builds

Antes de compartir:

- revisar rutas;
- revisar IDs personales;
- eliminar tokens;
- comprobar save files;
- etiquetar build y propósito;
- no publicar una Development Build abierta sin necesidad.

---

# 55. Campaña de aceptación de una instalación nueva

## 55.1. Fase A — Integridad estática

- estructura raíz;
- ProjectVersion;
- manifest/lock;
- hashes o Git;
- ProjectSettings;
- asmdefs;
- escenas y metas;
- registry;
- input actions.

## 55.2. Fase B — Importación

- paquetes resueltos;
- Console;
- shaders;
- materiales;
- prefabs;
- catálogos;
- no reserialización inesperada.

## 55.3. Fase C — Compilación y tests

- compilación limpia;
- EditMode;
- PlayMode;
- Input System tests;
- authoring validators;
- repetición para detectar contaminación.

## 55.4. Fase D — Escenas

- Bootstrap;
- MainMenu;
- Store durante migración;
- StoreInitial cuando proceda;
- TestLab;
- transiciones;
- ApplicationRoot único.

## 55.5. Fase E — Build

- profile correcto;
- carpeta limpia;
- EXE externa;
- MainMenu;
- escena de tienda;
- salida;
- Player.log.

## 55.6. Fase F — Repositorio

- cerrar Unity;
- revisar Git;
- clasificar cambios;
- restaurar cualquier archivo generado no deseado;
- registrar resultado.

## 55.7. Resultado

La campaña termina como:

- PASS;
- PASS WITH OBSERVATION;
- FAIL;
- BLOCKED.

Un PASS WITH OBSERVATION debe incluir una observación no bloqueante y su seguimiento. No usarlo
para ocultar errores rojos, tests fallidos o una build que no arranca.

---

# 56. Handoff del entorno

Un handoff de setup debe indicar:

- Editor y revisión;
- plataforma y módulos;
- SHA;
- rama;
- versión de aplicación;
- package diff;
- ProjectSettings modificados;
- profile activo;
- suites y resultados;
- build y log;
- deuda o bloqueos;
- siguiente acción;
- acciones prohibidas.

Debe poder comprenderse sin depender de mensajes de chat o memoria personal.

---

# Anexo A. Comandos de diagnóstico

## A.1. Git

```bash
git status
git rev-parse HEAD
git diff -- ProjectSettings Packages Assets/_Project/Settings
```

## A.2. Comprobación de archivos

```text
Assets/
Packages/manifest.json
Packages/packages-lock.json
ProjectSettings/ProjectVersion.txt
ProjectSettings/ProjectSettings.asset
```

Los comandos son ejemplos. No deben ejecutarse sobre una working copy con cambios no
comprendidos.

# Anexo B. Trazabilidad de fuentes


| Fuente | Líneas | Palabras aprox. | SHA-256 | Uso |
|---|---:|---:|---|---|
| `adr0001.md` | 34 | 121 | `196d02854bf9a34c0e4c2acd7056cde24e0a232c74d345f900997fff9689f853` | Fuente histórica/complementaria |
| `adr0004.md` | 37 | 78 | `8eea405d40efbffd5f51ad2c945994ffd82165bb5fef57900518c94fc23ac800` | Fuente histórica/complementaria |
| `asmdefs.md` | 71 | 261 | `8d82f56f02a46b23f1033f7f7f4a7ab3c66e5e37c196cc4dbcc1d6fcf34a92c4` | Fuente histórica/complementaria |
| `env.md` | 60 | 325 | `34906df25b05006e5bf449ee1ee1a00a2ee89ba58b8b1b8f3c7a3b3ea9cc5906` | Fuente histórica/complementaria |
| `manifest_v03.md` | 94 | 653 | `7fb1bdbd0b11a372ccf4ad87efea6f88326188a21a8f4760680b0e49cd928783` | Fuente histórica/complementaria |
| `manifest_v04.md` | 71 | 343 | `ccf1ff8da3ecfb69e1ee93a902e1d13d706d5e7ada9ac4a185515c8f93cd2b7a` | Fuente histórica/complementaria |
| `manifest_v05.md` | 33 | 116 | `8bb540cf6f6eb6e6f079cf55786ab3b53f4ec042bb1dd01147edb03222f306c5` | Fuente histórica/complementaria |
| `manifest_v06.md` | 27 | 104 | `c25b2f638ce7814f8f2d9a153481efe6891a01b7e873f1a3251775ee51b922dd` | Fuente histórica/complementaria |
| `package_review.md` | 42 | 236 | `6caa7be0ef49e9fa2fd1719400d3190cfdcf47ef7f3ff9211ef6560b4233a71a` | Fuente histórica/complementaria |
| `v03.md` | 147 | 575 | `0a6f42c71d9d22cee8aa12796842fd5391c3570f675f7361f1932418b74e1e55` | Fuente histórica/complementaria |
| `v04.md` | 174 | 743 | `0feb637a512b120a1515127467a5a9af0e97758c5a2f93627b6a4f3165b63cf8` | Fuente histórica/complementaria |
| `v05.md` | 109 | 278 | `cb4a9ea65fc1c8b95df59019c3bf6887cf220606980d221904494dfb057cf6ca` | Fuente histórica/complementaria |
| `v06.md` | 81 | 310 | `e0ba621f7cbd548da5ffa6484a1c2a0c91b820f845e5d410e318f01c34547497` | Fuente histórica/complementaria |
| `00_Enfoque_y_Alcance.md` | 4495 | 18925 | `63dd6f0f1b9c2c80be2aec55718d18d9d031ce4acb14f677769d6e69ceaad21f` | Autoridad consolidada |
| `01_Game_Design_Document.md` | 4490 | 19760 | `a9cd9e3203abc5269c25c59dc7f626b0771df4ec2d65e781109ff146bceb2177` | Autoridad consolidada |
| `02_Vertical_Slice_Specification.md` | 1505 | 9834 | `75893f130116e88a08b8da5590dd81876a234581081eafaa8d08374c1a60513f` | Autoridad consolidada |
| `03_Technical_Design_Document.md` | 3305 | 13774 | `66264703c5ff4959d03093690e9845a98ec0e5025e685b9a639ac8cbb616cd12` | Autoridad consolidada |
| `04_Modelo_de_Datos.md` | 2691 | 12944 | `d851a72b55742c2c704cc7c002929bc5894e7142511c6af843440bb193fb19d4` | Autoridad consolidada |
| `05_UX_Flow.md` | 2458 | 8720 | `e08da5ff67fb073a8b950babe325ae58ddb2e121513dc2f553acd4f2c14b867e` | Autoridad consolidada |
| `06_Production_Roadmap_y_Sprint_Plan.md` | 2408 | 9418 | `d00b156c0ad1c66069278119c55f76c738a577a3e2835f770208d1a5f2faadff` | Autoridad consolidada |
| `07_QA_Testing_Plan.md` | 3324 | 12027 | `bc9c05a3737e345546ef16e5390e034295ef35ad66c31647852aec81b4e7affe` | Autoridad consolidada |
| `09_CSharp_Coding_Standards.md` | 3664 | 12639 | `3b734d6cd49f31c09c10bb4941625e143f6c634e3fb50c157f0720e73b1c2a68` | Autoridad consolidada |
| `projectsettings/ProjectSettings/ProjectVersion.txt` | 2 | 0 | `d03fca18d2e511a37f7a81f8709404ebb2a571d3e0e5c854038c91a943ae15da` | Configuración implementada |
| `projectsettings/ProjectSettings/ProjectSettings.asset` | 948 | 0 | `19e2a914806164939a5cae16291c9b6f29f0a8b1b8a9f50d8fd74ea65c03178c` | Configuración implementada |
| `projectsettings/ProjectSettings/EditorSettings.asset` | 50 | 0 | `32d6a9be4c9aa9795a96bd951820a2d9be1bfefab82ed05a5db111af0863938f` | Configuración implementada |
| `projectsettings/ProjectSettings/VersionControlSettings.asset` | 8 | 0 | `e7bda45bf7a394745fee1316e6c71fa676004d591ad1f3aa247fd3e4ceb86071` | Configuración implementada |
| `projectsettings/ProjectSettings/GraphicsSettings.asset` | 69 | 0 | `6736f33dc62c35b0e7a88e275242080bc7a1d94c91154d18fceb332b661a6bce` | Configuración implementada |
| `projectsettings/ProjectSettings/QualitySettings.asset` | 134 | 0 | `6814d6cc3eb850dd7d2a069a8b17ba9f7591deba8b1758a855978bd9c8627643` | Configuración implementada |
| `projectsettings/ProjectSettings/TagManager.asset` | 76 | 0 | `23fc56b484aa3d7edf5914cd3591f7a823ccef5e3f3496f8a51f30388ead63dc` | Configuración implementada |
| `projectsettings/ProjectSettings/TimeManager.asset` | 9 | 0 | `1a83e54adbbda7c9f4851103a0c6ab7f6448a3343d4ea5b7620452fa08416ecd` | Configuración implementada |
| `projectsettings/ProjectSettings/AudioManager.asset` | 19 | 0 | `aeedeb3243f07b61dbbe854aa08b5ef6f0b5274dd9852ad9f6129dde3e105a31` | Configuración implementada |
| `projectsettings/ProjectSettings/EditorBuildSettings.asset` | 22 | 0 | `220a1f0e6a56e964e25b64481b10049e3e4fad883000f3e67b7000596fe7bd32` | Configuración implementada |
| `packages/Packages/manifest.json` | 45 | 0 | `50137b7babf4c53742103d5f14c86fde2090a984c030292e2e4aaef4ce03dae6` | Configuración implementada |
| `packages/Packages/packages-lock.json` | 524 | 0 | `6af42e872600380cea023aed62c849bed1e4b204a9dfc9e88da8184b06b6bbe2` | Configuración implementada |
| `assets/Assets/_Project/Settings/BuildProfiles/Windows_Development.asset` | 60 | 0 | `63e658288030c33844a84c37fc636fa908e9d8fbe44d2180aefcf03a0e5c93fa` | Configuración implementada |
| `assets/Assets/_Project/Settings/Runtime/RuntimeAssetRegistry.asset` | 21 | 0 | `731fca5df6d2e3a4562e3e40cfeb017e75e56bf421e27b8b0121993aacefdb09` | Configuración implementada |
| `assets/Assets/_Project/Settings/Runtime/StoreRuntimeSettings.asset` | 20 | 0 | `24a083371710e60d82373e12009abf1f5633d92efd51ae358d383ccfb68686ad` | Configuración implementada |
| `assets/Assets/_Project/InputSystem_Actions.inputactions` | 1057 | 0 | `cd5a6308f9bce6ddac2023b5ed185ff6bc36ac09fbfd9680054d367822203812` | Configuración implementada |


# Anexo C. Decisiones sustituidas

| Materia | Estado antiguo | Regla vigente |
|---|---|---|
| Proyecto | preproducción sin archivos | proyecto funcional con Sprints 0–15 cerrados |
| Escena tienda | Store genérica | StoreInitial autorada, aún en migración |
| Tamaño | 5×5 m conceptual | aproximadamente 10×15 m |
| Assemblies | seis | doce observados, manteniendo límites |
| Tests | 9 iniciales | suite vigente de 1285 en baseline |
| Calidad | Low/Medium/High propuestos | Mobile/PC implementados |
| Paquetes | lista prevista | siete directos versionados |
| Localización | futura | paquete instalado; tablas aún por cerrar |
| Build scene list | Store | sustituir solo tras gate de StoreInitial |
| Runtime shell | procedural | entorno autorado con fallback temporal |

# Anexo D. Resumen de no negociables

1. Editor exacto.
2. Manifest y lock versionados.
3. Force Text.
4. Visible Meta Files.
5. Bootstrap primero.
6. No Engine References en Domain/Application.
7. GUIDs estables.
8. StoreInitial no sustituye Store antes del gate.
9. Tests y build después de cambios estructurales.
10. Player.log y Git limpio como evidencia.

---

**Estado del documento:** fuente vigente de configuración, restauración y validación inicial del
proyecto Unity para la nueva carpeta `Documentacion/`.

