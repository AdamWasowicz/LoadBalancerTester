#!/bin/bash
sh './delete/deleteApi.sh';
sh './delete/deleteSecret.sh';
sh './delete/deleteConfigMap.sh';
sh './delete/deletePsqlDb.sh';

read -p "Press enter to continue...";