#!/bin/bash
# Delete current migration
if [ -d "../../LBT_Api/Migrations" ]; then
  echo '[LOG] Migrations directory detected, begining deletion'
  rm -rf ../../LBT_Api/Migrations
  echo '[LOG] Migrations directory deleted'
fi

# Create new migration
now=$(date)
stamp=${now//[ ,.:]/_}
dotnet ef migrations add $stamp -p '../../LBT_Api'

# Apply migration
dotnet ef database update -c LBT_DbContext -p '../../LBT_Api'

read -p "Press enter to continue..."