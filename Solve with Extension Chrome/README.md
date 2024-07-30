
## Làm cách nào để có thể sử dụng extension với key mặc định?
Để áp dụng mã API KEY mặc định khi extension được khởi động, cấu hình khóa API KEY trong file `./config.json`. Điều này sẽ giúp các coder xử lý trong nền tảng selenium tốt hơn.

<p>Hãy điều chỉnh "YOUR API KEY" trong file <code>./config.json</code>, thay thế bằng khóa API bạn lấy trên website: https://betacaptcha.com/</p>

<pre><code class="json">
{
    "API_KEY": "YOUR API KEY"
}
</code></pre>


## Dữ liệu được xử lý trong khi giải Recaptcha:

| Giá trị           | Mô tả                                                                 |
|---------------------|-----------------------------------------------------------------------------|
| `Solving...`  | Tiến trình captcha đang được xử lý. |
| `Ready!` | Recaptcha đã được xử lý thành công. |
| `ERROR!`  | Recaptcha xử lý lỗi |
