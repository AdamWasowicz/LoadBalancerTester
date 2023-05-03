#!/bin/bash
kubectl apply -f ./../Deployment/psql-db-volume-deployment.yml
kubectl apply -f ./../Deployment/psql-db-deployment.yml