document.addEventListener("DOMContentLoaded",(function(){const e=document.getElementById("apiKey");chrome.runtime.sendMessage({action:"get api key"},(n=>{e.value=n.api_token,document.querySelector(".balance > span").textContent="Balance: "+n.result.balance+"$"})),document.querySelector("#btn_save").addEventListener("click",(function(){const n=document.getElementById("apiKey").value;chrome.runtime.sendMessage({action:"change api key",API_KEY:n},(n=>{e.value=n.api_token,document.querySelector(".balance > span").textContent="Balance: "+n.result.balance+"$"}))})),document.getElementById("addFundsButton").onclick=function(){window.open("https://betacaptcha.com/banking","_blank")}}));