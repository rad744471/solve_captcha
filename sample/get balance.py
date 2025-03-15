import requests

json_data = {
    "api_token": "YOUR_API_KEY"
}
balance = requests.post("https://betacaptcha.com/api/balance", json=json_data)

print(balance.json())