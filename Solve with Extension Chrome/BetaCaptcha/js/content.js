window.onload=function(){!function e(){try{if(document.querySelector('[data-theme="home.verifyButton"], .match-game')){const r=document.querySelector('[data-theme="victory.subtitle"]'),o=document.querySelector('[role="text"], [font-size="1.25"]'),n=document.querySelector('[data-theme="home.verifyButton"], .match-game-fail > button');if(r);else if(n)n.click(),setTimeout(e,3e3);else if(o){var t=function(){var e=window.getComputedStyle(document.querySelector('[aria-live="assertive"]'),null).getPropertyValue("background-image").match(/url\("(.+)"\)/)[1],t=new Image;t.src=e;var r=document.createElement("canvas");return r.width=t.width,r.height=t.height,r.getContext("2d").drawImage(t,0,0),r.toDataURL("image/jpeg")}();imginstructions=o.textContent,console.log(imginstructions),chrome.runtime.sendMessage({action:"funcaptcha",imginstructions,body:t.split(",")[1]},(t=>{if(t.error)return console.error(t.error),void setTimeout(e,3e3);console.log(t.result),function r(){chrome.runtime.sendMessage({action:"get_task_result",taskid:t.result},(t=>{if(t.error)return void console.error(t.error);console.log(t.result);const o=t.result;if(o.result){const t=parseInt(o.result);if(document.documentElement.innerHTML.includes("right-arrow")){for(let e=0;e<t-1;e++)document.querySelector(".right-arrow").click();document.querySelector(".match-game > div > button").click()}else document.querySelector(`[aria-label^="Image ${t}"]`).click();setTimeout(e,3e3)}else o.includes("CAPCHA_NOT_READY")?setTimeout(r,1e3):(o.includes("ERROR")||o.length<2)&&console.log(o)}))}()}))}else setTimeout(e,3e3)}else if(document.querySelector("#g-recaptcha-response")){let t=0;const r=document.URL,o=document.querySelector('[title="reCAPTCHA"]').getAttribute("src").split("&k=")[1].split("&")[0];let n=document.querySelector(".captcha-solver-info");if(!n){const e=document.createElement("style");e.textContent="\n                        .captcha-solver-info {\n                            background-color: #f0f8ff;\n                            color: #333;\n                            border: 2px solid #00bfff;\n                            border-radius: 5px;\n                            padding: 10px;\n                            margin-top: 10px;\n                            font-family: Arial, sans-serif;\n                            font-size: 14px;\n                            text-align: center;\n                            box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);\n                            width: fit-content;\n                            max-width: 80%;\n                            margin-left: auto;\n                            margin-right: auto;\n                        }\n                        .error {\n                            border: 2px solid #EC2626;\n                        }\n                    ",document.head.appendChild(e),document.querySelector("#g-recaptcha-response").insertAdjacentHTML("afterend",'<div class="captcha-solver-info">Solving...</div>'),n=document.querySelector(".captcha-solver-info")}n.textContent="Đang tiến hành giải...",chrome.runtime.sendMessage({action:"recaptcha",siteurl:r,sitekey:o},(r=>{if(r.error)return console.error(r.error),void setTimeout(e,3e3);console.log(r.result),r.result&&function e(){chrome.runtime.sendMessage({action:"get_task_result",taskid:r.result},(r=>{if(r.error)return void console.error(r.error);const o=r.result;console.log(o),"success"===o.status?(document.querySelector("#g-recaptcha-response").value=o.result,n.textContent="Ready!",n.classList.remove("error")):"running"===o.status?(n.textContent="Solving...",setTimeout(e,1e3),t+=1):"error"===o.status&&(n.textContent="ERROR!",n.classList.add("error"),console.log(o))}))}()}))}else setTimeout(e,3e3)}catch{setTimeout(e,3e3)}}()};