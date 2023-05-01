#!/bin/bash
source colors.sh

echo -e "${GREEN}[DoDockerComposeUp.sh] - BEGIN${COLOR_OFF}"
docker rmi lbt-lbt-api
docker-compose -f '../docker-compose.yml' build --no-cache
docker-compose -f '../docker-compose.yml' up -d
echo -e "${GREEN}[DoDockerComposeUp.sh] - END${COLOR_OFF}"