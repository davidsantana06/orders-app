#!/bin/bash
set -e

# Start SQL Server in background
/opt/mssql/bin/sqlservr &

echo "Waiting for SQL Server to be ready..."
sleep 30s

echo "Executing initialization script..."
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -C -i /docker-entrypoint-initdb.d/init.sql

echo "Initialization completed!"

# Keep SQL Server running
wait
