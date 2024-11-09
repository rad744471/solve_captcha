import time
import requests

# Tạo Job
json_data = {
    "api_token": "YOUR_API_KEY",
    "data": {
        "type_job": "tiktok_slide",
        "body": "image as base64 encoded"
    }
}
createJob = requests.post("https://betacaptcha.com/api/createJob", json=json_data)

# Lấy kết quả trả về
for _ in range(3):
    json_data = {
        "api_token": "YOUR_API_KEY",
        "taskid": createJob.json()["taskid"]
    }
    getJobResult = requests.post("https://betacaptcha.com/api/getJobResult", json=json_data)
    if getJobResult.json()["status"] != "running":
        result = getJobResult.json()["result"]
        break
    else:
        time.sleep(1)

print(">> Kết quả:", result)
