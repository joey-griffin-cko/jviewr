#!/bin/bash

echo "Starting API...";
dotnet api/out/api.dll > api.log &

echo "Starting UI...";
cd ui && npm start