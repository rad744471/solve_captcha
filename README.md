Host: betacaptcha.com
Content-Type: application/json

```markdown
import time
import requests

# Tạo Job
json_data = {
    "api_token": "YOUR_API_KEY",
    "data": {
        "type_job": "fun_capcha_click",
        "body": "image as base64 encoded",
        "imginstructions": "Use the arrows to move the train to the coordinates indicated in the left image"
    }
}
createJob = requests.post("http://betacaptcha.com/api/createJob", json=json_data)

# Lấy kết quả trả về
time.sleep(2)
json_data = {
    "api_token": "YOUR_API_KEY",
    "taskid": createJob.json()["taskid"]
}
response = requests.post("http://betacaptcha.com/api/getJobResult", json=json_data)

print(">> Kết quả:", response.json()["result"])
```

- api_token: Khóa API lấy tại trang chủ chúng tôi


| Header 1 | Header 2 |
|----------|----------|
| Content 1| Content 2|
| Content 3| Content 4|


````markdown
```markdown
| Tên | Tuổi |
|-----|------|
| John| 30   |
| Jane| 25   |
