Host: betacaptcha.com
Content-Type: application/json


import time
import base64
import requests


````markdown
def image_to_base64(image_path):
    with open(image_path, "rb") as image_file:
        return base64.b64encode(image_file.read()).decode('utf-8')

json_data = {
    "api_token": "23aeb70e1205da8179569a5d250f58bf",
    "data": {
        "type_job": "fun_capcha_click",
        "imginstructions": "Use the arrows to move the train to the coordinates indicated in the left image (1 of 10)",
        "body": image_to_base64('image\\image (1).jpg')
    }
}

current_time = time.time()

createJob = requests.post("http://127.0.0.1/api/createJob", json=json_data)
print(createJob.json())

while True:
    json_data = {
        "api_token": "23aeb70e1205da8179569a5d250f58bf",
        "taskid": createJob.json()["taskid"]
    }
    response = requests.post("http://127.0.0.1/api/getJobResult", json=json_data)
    if response.json()["status"] != "running":
        print(time.time() - current_time, response.json())
        break
    else:
        time.sleep(1)
````markdown

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
