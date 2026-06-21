# Cartridge & Cloud — Migration Summary

## Resultado

El paquete anterior de **Indie Studio Simulator** queda sustituido como documentación operativa por `Cartridge_And_Cloud_Documentation_v0.3`. Las versiones antiguas deben conservarse únicamente como archivo histórico y no como guía de implementación.

## Sustituciones principales

| Área anterior | Nueva referencia |
|---|---|
| Guía de estado tras sprints | `00_Cartridge_And_Cloud_Guide_v0.3.pdf` |
| GDD del estudio indie | `Cartridge_And_Cloud_GDD_v0.4_PCSteam.pdf` |
| Roadmap desde Sprint 6 | `Cartridge_And_Cloud_Production_Roadmap_Sprint_Plan_v0.3.pdf`, desde Sprint 0 |
| Modelo basado en creación abstracta de juegos | Modelo físico de tienda, inventario, clientes y evolución empresarial |
| UX basada en paneles de creación de juegos | Mundo 3D interactivo más paneles de gestión |
| Oficina como escenario principal | Tienda física y complejo empresarial modular |
| Reviews/ventas como loop inicial | Compra, recepción, reposición, apertura, atención, cobro y cierre |
| Código/sprints existentes | Ningún código o sprint se considera implementado |

## Regla de archivo

Mover las versiones anteriores a una carpeta de archivo como:

```text
Documentation/Archive/Pre_Pivot_Indie_Studio_Simulator/
```

No mezclar documentos antiguos y vigentes en la misma carpeta operativa.

## Inicio recomendado

1. Leer Project Binder y Guía Maestra.
2. Revisar GDD y Vertical Slice Specification.
3. Confirmar Roadmap Sprint 0.
4. Preparar Unity mediante Setup Guide.
5. Registrar decisiones técnicas mediante ADRs.
6. Crear el proyecto vacío.
7. Ejecutar el primer gate QA.
