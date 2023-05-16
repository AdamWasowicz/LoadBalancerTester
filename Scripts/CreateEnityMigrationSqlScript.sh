#!/bin/bash
now=$(date)
stamp=${now//[ ,.:]/_}
dotnet ef migrations add $stamp -p '../LBT_Api'
dotnet ef migrations script -p '../LBT_Api' --output '../Db/EntityMigration.sql'