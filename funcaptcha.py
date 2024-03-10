import time
import base64
import requests

# # Nếu bạn muốn chuyển image thành base64
# def image_to_base64(image_path):
#     with open(image_path, "rb") as image_file:
#         return base64.b64encode(image_file.read()).decode('utf-8')

# imginstructions: Giá trị lấy ở tiêu đều mô tả hình ảnh

# Tạo Job
json_data = {
    "api_token": "YOUR_API_KEY",
    "data": {
        "type_job": "fun_capcha_click",
        "imginstructions": "Use the arrows to move the train to the coordinates indicated in the left image",
        "body": "image as base64 encoded"
    }
}
createJob = requests.post("http://127.0.0.1:5000/api/createJob", json=json_data)

# Lấy kết quả trả về
for _ in range(6):
    json_data = {
        "api_token": "YOUR_API_KEY",
        "taskid": createJob.json()["taskid"]
    }
    response = requests.post("http://127.0.0.1:5000/api/getJobResult", json=json_data)
    if response.json()["status"] != "running":
        solve = response.json()["result"]
        break
    else:
        time.sleep(1)

print(">> Kết quả:", solve)
