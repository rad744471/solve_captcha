## Hướng dẫn sử dụng api IPv4 xoay

`GET /api/change_proxy?api_key={API_KEY}`

### Lấy IP mới (GET)

```
https://www.betacaptcha.com/api/change_proxy?api_key={API_KEY}
```
- **{API_KEY}**: Thay thế bằng API key của bạn.

## Response Format

API trả về một trong hai phản hồi dưới dạng JSON.

### Response Success

```json
{
    "next_change": 60,
    "proxy": "IP:PORT",
    "success": true,
    "timeout": 90
}
```
- **next_change**: Thời gian (tính bằng giây) trước khi proxy hiện tại thay đổi.
- **proxy**: Địa chỉ của proxy có sẵn để sử dụng (bao gồm hostname và port).
- **success**: Trạng thái của yêu cầu (true nếu yêu cầu thành công).
- **timeout**: Thời gian (tính bằng giây) trước khi proxy hết hạn sử dụng.

### Response Fail

```json
{
    "current_IP": "IP:PORT",
    "next_change": 10,
    "proxy": null,
    "success": true,
    "timeout": 90
}
```
```json
{
    "success": false,
    "description": "Please slow down your requests or your account will be locked."
}
```
- **current_IP**: Địa chỉ IP hiện tại mà bạn đang sử dụng.
- **next_change**: Thời gian (tính bằng giây) trước khi proxy thay đổi.
- **proxy**: Hiện không có proxy có sẵn (giá trị là `null`).
- **success**: Trạng thái của yêu cầu (true nếu yêu cầu thành công).
- **timeout**: Thời gian trước khi proxy hết hạn sử dụng.

## Usage Guide

1. Gửi yêu cầu `GET` đến URL đã chỉ định với API key hợp lệ.
2. Kiểm tra phản hồi JSON để lấy thông tin proxy.
   - Nếu trường **proxy** có giá trị, bạn có thể sử dụng proxy đó.
   - Nếu trường **proxy** là `null`, hãy chờ đến thời gian **next_change** để nhận proxy mới.

## Notes

- Thời gian **next_change** cho biết khoảng thời gian cần chờ trước khi thay đổi proxy hoặc nhận proxy mới.
- API key cần hợp lệ để truy cập vào dịch vụ.
- Khi proxy không có sẵn (**proxy** = `null`), hệ thống sẽ thông báo **current_IP** để người dùng biết thông tin về IP hiện tại.

