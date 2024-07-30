Để áp dụng mã API KEY mặc định khi extension được khởi động, cấu hình khóa API KEY trong file `./config.json`.
## Giá trị :
## How to disable opening the extension page after installation?

To do this, in the file `./manifest.json`, delete the following lines:

```json
"options_ui": {
    "page": "options/options.html",
    "open_in_tab": true
},```



Chỉnh sửa file `./common/config.js` để cấu hình cài đặt.
## Description of the values of the data-state attribute:

| Attribute           | Description                                                                 |
|---------------------|-----------------------------------------------------------------------------|
| `data-state="ready"`  | The extension is ready to solve the captcha. To send a captcha, you need to click on the button. |
| `data-state="solving"` | Solving the captcha.                                                      |
| `data-state="solved"`  | Captcha has been successfully solved                                      |
| `data-state="error"`   | An error when receiving a response or a captcha was not successfully solved. |
