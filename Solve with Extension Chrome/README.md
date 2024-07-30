
## Làm cách nào để có thể sử dụng extension với key mặc định?
Để áp dụng mã API KEY mặc định khi extension được khởi động, cấu hình khóa API KEY trong file `./config.json`. Điều này sẽ giúp các coder xử lý trong nền tảng selenium tốt hơn.

<p>Hãy điều chỉnh "YOUR API KEY" trong file <code>./config.json</code>, thay thế bằng khóa API bạn lấy trên website: https://betacaptcha.com/</p>

<pre><code class="json">
{
    "API_KEY": "YOUR API KEY"
}
</code></pre>


Chỉnh sửa file `./common/config.js` để cấu hình cài đặt.
## Description of the values of the data-state attribute:

| Attribute           | Description                                                                 |
|---------------------|-----------------------------------------------------------------------------|
| `data-state="ready"`  | The extension is ready to solve the captcha. To send a captcha, you need to click on the button. |
| `data-state="solving"` | Solving the captcha.                                                      |
| `data-state="solved"`  | Captcha has been successfully solved                                      |
| `data-state="error"`   | An error when receiving a response or a captcha was not successfully solved. |
