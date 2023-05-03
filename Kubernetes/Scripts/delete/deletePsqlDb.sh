#!/bin/bash
kubectl delete -f ./../Deployment/psql-db-deployment.yml
kubectl delete -f ./../Deployment/psql-db-volume-deployment.yml