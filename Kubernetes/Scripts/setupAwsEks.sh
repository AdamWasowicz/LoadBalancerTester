#!/bin/bash
# Connect to K8 cluster
#aws eks --region example_region update-kubeconfig --name cluster_name

# Install helm
sudo yum install openssl
curl https://raw.githubusercontent.com/helm/helm/master/scripts/get-helm-3 > get_helm.sh
chmod 700 get_helm.sh
./get_helm.sh

# Setup nginx controller
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
kubectl create namespace lbt

helm install my-nginx ingress-nginx/ingress-nginx \
--namespace lbt \
--set controller.metrics.enabled=true \
--set-string controller.metrics.service.annotations."prometheus\.io/port"="10254" \
--set-string controller.metrics.service.annotations."prometheus\.io/scrape"="true"

# Apply depoyment
kubectl apply -f aws-deployment.yml