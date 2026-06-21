# Estructura documental de Cartridge & Cloud

## Principio

La documentación oficial del proyecto vive fuera de `Assets`, pero dentro del
repositorio Git. De este modo Unity no importa PDFs, Excel o Markdown ni genera
archivos `.meta` para ellos.

## Estructura

```text
Documentation/
├── 00_Official_Baseline/
│   └── v0.3/
│       └── [contenido íntegro del paquete documental v0.3]
├── 10_Development_Records/
│   ├── ADR/
│   ├── Change_Control/
│   ├── Environment/
│   └── Sprints/
│       └── Sprint_00/
│           └── Setup/
├── 20_Working_Drafts/
└── 90_Archive/
```

### 00_Official_Baseline

Contiene la fuente oficial vigente recibida al iniciar el proyecto. El paquete se
copia descomprimido, conservando su estructura y nombres originales.

No debe contener el archivo RAR original, porque duplicaría todos los documentos.

### 10_Development_Records

Contiene documentación generada durante el desarrollo:

- ADR: decisiones arquitectónicas.
- Change_Control: registros y propuestas de cambio.
- Environment: información del entorno de desarrollo.
- Sprints: definición, ejecución, validación y cierre de cada sprint.

### 20_Working_Drafts

Documentos en revisión que todavía no forman parte de la línea base oficial.

### 90_Archive

Versiones sustituidas o documentos declarados obsoletos. No se usa como papelera.

## Reglas

1. No guardar documentación del proyecto dentro de `Assets`.
2. No duplicar un documento en varias categorías.
3. Conservar el número de versión en el nombre del archivo.
4. Editar preferentemente las fuentes Markdown o Excel.
5. Tratar los PDF como exportaciones publicadas.
6. No borrar una versión sustituida sin registrar el cambio.
7. No incluir archivos temporales de Office (`~$...`).
8. No incluir el RAR original en Git.
