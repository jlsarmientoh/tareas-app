# Despliegue

## Preparación

1. Crear los [configs maps](config)
1. Crear los secrets
1. Crear los despliegues
1. Crear los [servicios](services)
1. Crear el [Ingress](ingress.yaml)

---

## Mapear los host virtuales

En linux o Mac /etc/hosts; en Windows C:\Windows\System32\drivers\etc\hosts

```
127.0.0.1 internal.ops.com
127.0.0.1 internal.web.com
127.0.0.1 internal.trace.com
```

---

## Hosts virtuales

| Aplicación | Host | Puerto |
| ---------- | ---- | ------ |
| TareasWeb | internal.web.com | 80 |
| Wathcdog | internal.ops.com | 80 |
| Seq | internal.trace.com | 80 |