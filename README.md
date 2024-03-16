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
for _ in range(3):
    json_data = {
        "api_token": "YOUR_API_KEY",
        "taskid": createJob.json()["taskid"]
    }
    getJobResult = requests.post("http://betacaptcha.com/api/getJobResult", json=json_data)
    if getJobResult.json()["status"] != "running":
        result = getJobResult.json()["result"]
        break
    else:
        time.sleep(1)

print(">> Kết quả:", result)
```

| Name | Type | Required | Description |
|----------|----------|----------|----------|
| api_token | text | yes | Khóa tài khoản khách hàng |
| data.type_job | text | yes | Dịch vụ sử dụng (textcaptcha, tiktok_slide, tiktok_click, tiktok_rotate, fun_capcha_click) |
| data.body | text | yes | Hình ảnh được mã hóa base64 (không phải ảnh chụp màn hình) ![data.body](https://github.com/rad744471/solve_captcha/blob/main/image/funcaptcha.jpg?raw=true)|
| data.imginstructions | text | yes* | Văn bản câu hỏi captcha (Nếu sử dụng fun_capcha_click) ![data.imginstructions](https://github.com/rad744471/solve_captcha/blob/main/image/imginstructions.jpg?raw=true)|

