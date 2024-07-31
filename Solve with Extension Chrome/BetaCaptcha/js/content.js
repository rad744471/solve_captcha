function convert_element_to_base64() {
    // Lấy đường dẫn hình ảnh
    var backgroundImage = window.getComputedStyle(document.querySelector('[aria-live="assertive"]'), null);
    var imageUrl = backgroundImage.getPropertyValue('background-image').match(/url\("(.+)"\)/)[1];
    // Tạo element chứa hình ảnh
    var img = new Image();
    img.src = imageUrl;
    // Vẽ lên canvas hình ảnh
    var canvas = document.createElement('canvas');
    canvas.width = img.width;
    canvas.height = img.height;
    var ctx = canvas.getContext('2d');
    ctx.drawImage(img, 0, 0);
    // Chuyển dữ liệu hình ảnh thành dạng base64
    var imageData = canvas.toDataURL('image/jpeg');
    return imageData;
}

window.onload = function() {
    function solve_captcha() {
        try {
            // Hàm hỗ trợ giải funcaptcha
            if (document.querySelector('[data-theme="home.verifyButton"], #home_children_button, .match-game, #game_children_challenge')) {
                // Click để mở funcaptcha
                const btn_succes = document.querySelector('[data-theme="victory.subtitle"]');
                const element_title_captcha = document.querySelector('[role="text"], [font-size="1.25"], #game_children_text > h2');
                const verifyButton = document.querySelector('[data-theme="home.verifyButton"], #home_children_button, .match-game-fail > button');
                if (btn_succes) {
                    // Hàm giải captcha thành công, dừng chương trình
                } else if (verifyButton) {
                    verifyButton.click()
                    setTimeout(solve_captcha, 3000);
                } else if (element_title_captcha) {
                    // Lấy data để tiến hành giải
                    var element_image_captcha = document.querySelector("#game_challengeItem_image")
                    if (element_image_captcha) {
                        var imageData = element_image_captcha.src
                    } else {
                        var imageData = convert_element_to_base64();
                    }
                    if (imageData.length > 100) {
                        imginstructions = element_title_captcha.textContent;
                        console.log(imginstructions)
                        // Gửi lệnh lên backgroud.js
                        chrome.runtime.sendMessage({
                            action: "funcaptcha",
                            imginstructions: imginstructions,
                            body: imageData.split(',')[1]
                        }, (response) => {
                            if (response.error) {
                                console.error(response.error);
                                setTimeout(solve_captcha, 3000);
                                return;
                            } else {
                                console.log(response.result)
                                if (response.result) {
                                    // Lấy giá trị taskid và lấy kết quả job
                                    function get_task_result() {
                                        chrome.runtime.sendMessage({
                                            action: "get_task_result",
                                            taskid: response.result
                                        }, (response) => {
                                            if (response.error) {
                                                console.error(response.error);
                                                return;
                                            }

                                            console.log(response.result)
                                            // Kiểm tra kết quả được trả về
                                            const result = response.result;
                                            if (result.result) {
                                                // Tiến hành xác minh captcha
                                                const numberOfClicks = parseInt(result.result);
                                                if (document.documentElement.innerHTML.includes("right-arrow")) {
                                                    for (let i = 0; i < numberOfClicks - 1; i++) {
                                                        document.querySelector('.right-arrow').click();
                                                    };
                                                    document.querySelector('.match-game > div > button').click();
                                                } else {
                                                    // Click chọn 1 trong 6 ảnh
                                                    try {
                                                        document.querySelector(`[aria-label^="Image ${numberOfClicks}"]`).click();
                                                    } catch {
                                                        document.querySelectorAll('a[class^="ChallengeSelect"]')[numberOfClicks-1].click()
                                                    }
                                                };
                                                setTimeout(solve_captcha, 3000);
                                                // setTimeout(location.reload(), 3000)
                                            } else if (result.status === "running") {
                                                setTimeout(get_task_result, 1000);
                                            } else if (result.status === "error" || result.length < 2) {
                                                console.log(result);
                                            }
                                        });
                                    }
                                    get_task_result();
                                } else {
                                    setTimeout(solve_captcha, 3000);
                                }
                            }
                        });
                    } else {
                        setTimeout(solve_captcha, 3000);
                    }
                    
                } else {
                    setTimeout(solve_captcha, 3000);
                }
            }

            // Hàm hỗ trợ giải Recaptcha
            else if (document.querySelector("#g-recaptcha-response")) {
                let loop_check_solve = 0;

                // Lấy dữ liệu site key và URL
                const siteurl = document.URL;
                const sitekey = document.querySelector('[title="reCAPTCHA"]').getAttribute('src').split('&k=')[1].split('&')[0];

                // Kiểm tra element đã được thêm popup thông báo chưa
                let recaptcha_solver_info = document.querySelector(".captcha-solver-info");
                if (!recaptcha_solver_info) {
                    // Thêm style cho element .captcha-solver-info
                    const style = document.createElement('style');
                    style.textContent = `
                        .captcha-solver-info {
                            background-color: #f0f8ff;
                            color: #333;
                            border: 2px solid #00bfff;
                            border-radius: 5px;
                            padding: 10px;
                            margin-top: 10px;
                            font-family: Arial, sans-serif;
                            font-size: 14px;
                            text-align: center;
                            box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
                            width: fit-content;
                            max-width: 80%;
                            margin-left: auto;
                            margin-right: auto;
                        }
                        .error {
                            border: 2px solid #EC2626;
                        }
                    `;
                    document.head.appendChild(style);
                    // Thêm element mới vào để thông báo trạng thái
                    const recaptchaResponse = document.querySelector("#g-recaptcha-response");
                    recaptchaResponse.insertAdjacentHTML('afterend', '<div class="captcha-solver-info">Solving...</div>');
                    recaptcha_solver_info = document.querySelector(".captcha-solver-info");
                }

                // Tiến hành giải Recaptcha
                recaptcha_solver_info.textContent = "Đang tiến hành giải...";
                chrome.runtime.sendMessage({
                    action: "recaptcha",
                    siteurl: siteurl,
                    sitekey: sitekey
                }, (response) => {
                    if (response.error) {
                        console.error(response.error);
                        setTimeout(solve_captcha, 3000);
                        return;
                    }

                    console.log(response.result);

                    if (response.result) {
                        // Lấy giá trị taskid và lấy kết quả job
                        function get_task_result() {
                            chrome.runtime.sendMessage({
                                action: "get_task_result",
                                taskid: response.result
                            }, (response) => {
                                if (response.error) {
                                    console.error(response.error);
                                    return;
                                }

                                // Kiểm tra kết quả được trả về
                                const result = response.result;
                                console.log(result)
                                if (result.status === "success") {
                                    document.querySelector("#g-recaptcha-response").value = result.result;
                                    recaptcha_solver_info.textContent = `Ready!`;
                                    recaptcha_solver_info.classList.remove("error");
                                } else if (result.status === "running") {
                                    recaptcha_solver_info.textContent = `Solving...`;
                                    setTimeout(get_task_result, 1000);
                                    loop_check_solve += 1;
                                } else if (result.status === "error") {
                                    recaptcha_solver_info.textContent = "ERROR!";
                                    recaptcha_solver_info.classList.add("error");
                                    console.log(result);
                                }
                            });
                        }
                        get_task_result();
                    }
                });
            } else {
                setTimeout(solve_captcha, 3000);
            }
        } catch (error) {
            // Gặp lỗi sẽ bắt đầu lại từ đầu
            console.error('Đã xảy ra lỗi:', error);
            setTimeout(solve_captcha, 3000);
        }
    }
    solve_captcha();
}
