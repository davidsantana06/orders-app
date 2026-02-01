#!/bin/bash
set -e

/opt/mssql/bin/sqlservr & # Start SQL Server in background

echo "Waiting for SQL Server to be ready..."
sleep 30s

echo "Executing initialization scripts..."
for script in /docker-entrypoint-initdb.d/*.sql; do
    if [ -f "$script" ]; then
        echo "Running $(basename "$script")..."
        /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -C -i "$script"
    fi
done

echo "Initialization completed!"

wait # Keep SQL Server running
