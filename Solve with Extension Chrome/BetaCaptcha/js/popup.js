document.addEventListener('DOMContentLoaded', function () {
    // Lấy địa chỉ API_KEY
    const apiKeyInput = document.getElementById('apiKey');
    chrome.runtime.sendMessage({
        action: "get api key",
    }, (response) => {
        apiKeyInput.value = response.api_token;
        document.querySelector('.balance > span').textContent = "Balance: " + response.result.balance + "$";
    });
    
    // Lấy địa chỉ API_KEY
    document.querySelector('#btn_save').addEventListener('click', function() {
        const API_KEY = document.getElementById('apiKey').value;
        chrome.runtime.sendMessage({
            action: "change api key",
            API_KEY: API_KEY
        }, (response) => {
            apiKeyInput.value = response.api_token;
            document.querySelector('.balance > span').textContent = "Balance: " + response.result.balance + "$";
        });
    });

    // Điều hướng sang trang nạp tiền
    document.getElementById('addFundsButton').onclick = function() {
        window.open('https://betacaptcha.com/banking', '_blank');
    };
});
