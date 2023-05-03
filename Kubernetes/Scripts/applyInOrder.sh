#!/bin/bash
sh './apply/applySecret.sh';
sh './apply/applyConfigMap.sh';
sh './apply/applyApi.sh';
sh './apply/applyPsqlDb.sh';

read -p "Press enter to continue...";