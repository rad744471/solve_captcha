Host: betacaptcha.com

Code mẫu được tham khảo phía dưới:

```markdown
import time
import requests

# Tạo Job
json_data = {
    "api_token": "YOUR_API_KEY",
    "data": {
        "type_job": "id dịch vụ",
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

| Header 1 | Header 2 |
|----------|----------|
| Content 1| Content 2|
| Content 3| Content 4|

