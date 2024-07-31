let apiUrl = 'https://betacaptcha.com';
let API_KEY = '';

// Hàm đọc file config
fetch(chrome.runtime.getURL('config.json'))
    .then(response => response.json())
    .then(config => {
        API_KEY = config.API_KEY;
});

// Đây là hàm để chuyển fetch sang chạy riêng lẻ
chrome.runtime.onMessage.addListener((request, sender, sendResponse) => {
    if (request.action === "get api key") {
        chrome.storage.local.get(['api_key'], function(result) {
            let api_key = result.api_key || API_KEY;
            fetch(apiUrl + '/api/balance', {
                method: 'POST',
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    api_token: api_key,
                })
            })
            .then(response => response.json())
            .then(text => sendResponse({result: text, api_token: api_key}))
            .catch(error => sendResponse({error: error.toString()}));
        });
        return true;
    } else if (request.action === "change api key") {
        let newApiKey = request.API_KEY;
        chrome.storage.local.set({api_key: newApiKey}, function() {
            fetch(apiUrl + '/api/balance', {
                method: 'POST',
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    api_token: newApiKey,
                })
            })
            .then(response => response.json())
            .then(text => sendResponse({result: text, api_token: newApiKey}))
            .catch(error => sendResponse({error: error.toString()}));
        });
        return true;
    } else if (request.action === "recaptcha") {
        fetch(apiUrl + '/api/createJob', {
            method: 'POST',
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                api_token: API_KEY,
                data: {
                    type_job: "recaptcha",
                    sitekey: request.sitekey,
                    siteurl: request.siteurl
                }
            })
        })
        .then(response => response.json())
        .then(text => sendResponse({result: text.taskid}))
        .catch(error => sendResponse({error: error.toString()}));
        return true;
    } else if (request.action === "funcaptcha") {
        fetch(apiUrl + '/api/createJob', {
            method: 'POST',
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                api_token: API_KEY,
                data: {
                    type_job: "fun_capcha_click",
                    imginstructions: request.imginstructions,
                    body: request.body
                }
            })
        })
        .then(response => response.json())
        .then(text => sendResponse({result: text.taskid}))
        .catch(error => sendResponse({error: error.toString()}));
        return true;
    } else if (request.action === "get_task_result") {
        fetch(apiUrl + '/api/getJobResult', {
            method: 'POST',
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                api_token: API_KEY,
                taskid: request.taskid
            })
        })
        .then(response => response.json())
        .then(text => sendResponse({result: text}))
        .catch(error => sendResponse({error: error.toString()}));
        return true;
    }
});
