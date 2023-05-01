#!/bin/bash
source colors.sh

echo -e "${BLUE}[DoEntityFrameworkMigration.sh] - BEGIN${COLOR_OFF}"

# Delete current migration
echo -e "${PURPLE}[DELETE PREVIOUS MIGRATION CATALOG] - BEGIN${COLOR_OFF}"
if [ -d "../LBT_Api/Migrations" ]; then
  echo '[LOG] Migrations directory detected, begining deletion'
  rm -rf ../LBT_Api/Migrations
  echo '[LOG] Migrations directory deleted'
fi
echo -e "${PURPLE}[DELETE PREVIOUS MIGRATION CATALOG] - END${COLOR_OFF}"

# Create new migration
echo -e "${GREEN}[CREATE MIGRATION] - BEGIN${COLOR_OFF}"
now=$(date)
stamp=${now//[ ,.:]/_}
dotnet ef migrations add $stamp -p '../LBT_Api'
echo -e "${GREEN}[CREATE MIGRATION] - END${COLOR_OFF}"

# Apply migration
echo -e "${YELLOW}[APPLY MIGRATION] - BEGIN${COLOR_OFF}"
dotnet ef database update -c LBT_DbContext -p '../LBT_Api'
echo -e "${YELLOW}[APPLY MIGRATION] - END${COLOR_OFF}"

echo -e "${BLUE}[DoEntityFrameworkMigration.sh] - END${COLOR_OFF}"
read -p "Press enter to continue..."