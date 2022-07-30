#!/bin/bash

SERVER_URL=<https://server_url>
BACKUP_FILE=./backup.json

python3 restore_backup.py $SERVER_URL $BACKUP_FILE
