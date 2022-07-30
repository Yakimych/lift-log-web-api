""" Restore logs from a backup json file """
from sys import argv
import json
import requests

server_url = argv[1]
backup_file = argv[2]

LIFT_LOGS_URL = f"{server_url}/api/LiftLogs"
headers = {'Content-type': 'application/json', 'Accept': 'application/json'}

with open(backup_file, "r", encoding="utf-8") as backup_file:
    logs_to_restore = json.load(backup_file)
    for log in logs_to_restore:
        response = requests.post(LIFT_LOGS_URL, json=log, headers=headers)
        for entry in log["entries"]:
            add_entry_url = f'{LIFT_LOGS_URL}/{log["name"]}/Lifts'
            requests.post(add_entry_url, json=entry, headers=headers)
