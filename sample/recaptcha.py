import time
import requests

# Tạo Job
json_data = {
    "api_token": "YOUR_API_KEY",
    "data": {
        "type_job": "recaptcha",
        "sitekey": "được lấy từ site-key",
        "siteurl": "địa chỉ url của website"
    }
}
createJob = requests.post("https://betacaptcha.com/api/createJob", json=json_data, verify=False)

# Lấy kết quả trả về
for _ in range(3):
    json_data = {
        "api_token": "YOUR_API_KEY",
        "taskid": createJob.json()["taskid"]
    }
    getJobResult = requests.post("https://betacaptcha.com/api/getJobResult", json=json_data, verify=False)
    if getJobResult.json()["status"] != "running":
        result = getJobResult.json()["result"]
        break
    else:
        time.sleep(1)

print(">> Kết quả:", result)
