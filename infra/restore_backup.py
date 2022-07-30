import json
import requests

lift_logs_url = "http://server_url/api/LiftLogs"
headers = {'Content-type': 'application/json', 'Accept': 'application/json'}
backup_file_name = "backup.json"

with open(backup_file_name, "r") as backup_file:
    logs_to_restore = json.load(backup_file)

    for log in logs_to_restore:
        response = requests.post(lift_logs_url, json=log, headers=headers)
        for entry in log["entries"]:
            add_entry_url = f'{lift_logs_url}/{log["name"]}/Lifts'
            requests.post(add_entry_url, json=entry, headers=headers)
