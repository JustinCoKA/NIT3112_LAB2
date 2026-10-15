#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")/.."
export DOTNET_USE_POLLING_FILE_WATCHER=1
export DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false
exec dotnet run --project JustinLee-Lab2/JustinLee-Lab2.csproj --no-launch-profile --urls http://127.0.0.1:5083
