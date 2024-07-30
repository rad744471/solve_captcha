check
Chỉnh sửa file `./common/config.js` để cấu hình cài đặt.
## Description of the values of the data-state attribute:

| Attribute           | Description                                                                 |
|---------------------|-----------------------------------------------------------------------------|
| `data-state="ready"`  | The extension is ready to solve the captcha. To send a captcha, you need to click on the button. |
| `data-state="solving"` | Solving the captcha.                                                      |
| `data-state="solved"`  | Captcha has been successfully solved                                      |
| `data-state="error"`   | An error when receiving a response or a captcha was not successfully solved. |
