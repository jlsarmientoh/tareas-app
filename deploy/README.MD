# Despliegue

## Preparación

1. Crear los [configs maps](config)
1. Crear los secrets
1. Crear los despliegues
1. Crear los [servicios](services)
1. Crear el [Ingress](ingress.yaml)
    1. Para configurar el ingress usando nginx deben:
    1. Ejecutar  kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.2.0/deploy/static/provider/cloud/deploy.yaml .
    1. Esto va a crear un servicio con el ingress controller basado en nginx
    1. Ejecutar kubectl create deploy/ingress.yaml  para crear el recurso tipo ingress
	


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