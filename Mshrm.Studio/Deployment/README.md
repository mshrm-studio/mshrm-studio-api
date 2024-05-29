# Config Setup

- Download config from K8 Droplet in Digital Ocean
- Place config in C:/Users/User/.kube
- Download kubectl via Chocolatey
- Ensure Registry is created
- Ensure registry DigitalOcean Kubernetes integration is setup to allow access from cluster

# Get All Containers In Pod

```Powershell
kubectl get po
```

# Get Logs For Container

```Powershell
kubectl logs pod_name
```

# Create Registry Secrets

Download docker-config from Digital Ocean registry

```Powershell
kubectl create secret generic mshrm-studio-registry --from-file=.dockerconfigjson=docker-config.json --type=kubernetes.io/dockerconfigjson
```

# Add Registry Secrets To Pods (Defaults all images to use)

```Powershell
kubectl patch serviceaccount default -p '{\"imagePullSecrets\": [{\"name\": \"mshrm-studio-registry\"}]}'
```

# Debugging

Useful for when the status ImagePullError is shown

```Powershell
kubectl describe pod pod_name
```

# URL

```Powershell
kubectl get svc
```