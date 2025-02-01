#!/bin/bash

# Wait for PostgreSQL to be available
until pg_isready -h postgreservice -U postgres; do
  echo "Waiting for PostgreSQL to be ready..."
  sleep 2
done

# Once PostgreSQL is ready, start the web service
echo "PostgreSQL is ready, starting web service..."
exec "$@"
