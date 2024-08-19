## Xem tài liệu hướng dẫn sử dụng API trên:
[Tài liệu API Beta Captcha](https://github.com/rad744471/solve_captcha/tree/main)

[Tải xuống file.zip Extension Chrome giải Funcaptcha & Recaptcha](https://drive.google.com/file/d/1wQZ-h_bHsqHg7l2hQH55xPlnyrDNqtDg/view?usp=sharing)

[Video giới thiệu và sử dụng dịch vụ](https://www.youtube.com/watch?v=tSjsGsD3DZA)

[Video hướng dẫn giải Recaptcha hình ảnh bằng Extension Chrome](https://www.youtube.com/watch?v=K5wV2kpPxlg)

## Làm cách nào để có thể sử dụng extension với key mặc định?
Để áp dụng mã API KEY mặc định khi extension được khởi động, cấu hình khóa API KEY trong file `./config.json`. Điều này sẽ giúp các coder xử lý trong nền tảng selenium tốt hơn.

Funcaptcha được hoạt động tốt nhất nếu trình duyệt được sử dụng ngôn ngữ là `Tiếng Anh`. Để cấu hình `Tiếng Anh` là ngôn ngữ mặc định khi mở selenium vui lòng thêm thuộc tính `--lang=en` vào ChromeOptions khi tiến hành mở chrome.

<p>Hãy điều chỉnh "YOUR API KEY" trong file <code>./config.json</code>, thay thế bằng khóa API bạn lấy trên website: https://betacaptcha.com</p>

<pre><code class="json">
{
    "API_KEY": "YOUR API_KEY",
    "Recaptcha_Image": true,
    "delay_solve": 2000
}
</code></pre>
| Giá trị           | Mô tả                                                                 |
|---------------------|-----------------------------------------------------------------------------|
| `API_KEY`  | Giá trị API lấy tại [Trang chủ](https://betacaptcha.com). |
| `Recaptcha_Image` | để `true` để giải captcha bằng hình ảnh, `false` để giải bằng token. |
| `delay_solve`  | thời gian delay giữa 2 lần kiểm tra |



## Dữ liệu được xử lý trong khi giải Recaptcha:

| Giá trị           | Mô tả                                                                 |
|---------------------|-----------------------------------------------------------------------------|
| `Solving...`  | Tiến trình captcha đang được xử lý. |
| `Ready!` | Recaptcha đã được xử lý thành công. |
| `ERROR!`  | Recaptcha xử lý lỗi |
