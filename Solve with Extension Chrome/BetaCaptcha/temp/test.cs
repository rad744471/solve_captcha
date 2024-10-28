skip to content

ZennoLab
Automate everything

User Tools
Login
Site Tools
Recent changesMedia ManagerSitemap
Trace: • recognition • rc2
Translations of this page:
English
Русский
中文
English
 Introduction
Overview
ProjectMaker
ZennoPoster
ZennoProxyChecker
ZennoDroid
CapMonster
 CapMonster - new version
 System Requirements
 Program settings
 Recognizing captchas
 Integration with other programs
 Change log
Module Creation Studio
Special modules
 ReCaptcha 2
 KeyCaptcha
 AudioReCaptcha
 ReCaptchaAssociations
 AudioSolveMedia
 FunCaptcha
 Are You Human Captcha
Addons
 Changelog
Русский
中文
en:addons:capmonster:rc2
Table of Contents
ReCaptcha 2
How it works
Using in ZennoPoster
NOTE
ReCaptcha 2
Google has recently developed new kind of protection from bots called ReCaptcha2. On websites with such protection you get “I'm not robot” option, which you have to check and then select several pictures specified in task title.

ReCaptcha2 looks like this:



Here you can get ReCaptcha2 sample: Demo-page from Google

CapMonster2 allows to recognize new ReCaptcha2 starting from 2.4.0.0 version.

How it works
To send captcha to CapMonster 2, you should create a request that includes captcha image with several answer variants and task as additional parameter. Parameter name: «Task». Value - question in recaptcha, for example «Select all images with trains».

CapMonster2 answer to this captcha will be string with numbers - numbers of pictures that should be clicked. Numbers are sorted in decreasing probability that the picture contains specified object.

For example, you should select pictures with soup. CapMonster recognizes it as 29. That means that soup is most probably displayed on the picture #2, then picture 9. So, you should click the pictures 2,9 and then press “Verify”.



If CapMonster2 could not recognize soup on more than one picture, (2 pcitures minimum for correct response) it returns empty answer, so that you will be able to send this captcha to recognition service.

Using in ZennoPoster
For sending ReCaptcha2 from ZennoPoster for CapMonster 2.8.3.0 and higher you can use Action Recognize ReCaptcha2



Besides, using this action you can adjust the parameters of captchas recognition.



It is possible to solve ReCaptcha 2 using Sitekey.



In some sites users need to deal with autosubmit event in order to the solving of ReCaptcha2 is counted. Using ZennoPoster help to solve this issue.



To recognize ReCaptcha2 in ZennoPoster (ver 5.9+) using CapMonster2, use the following C# snippet: (This snippet should be used in Capmonster version 2.6.3+. The old snippet for previous program versions is available here)


// Main parameters
 
// waiting time
var waitTime = 1500;
// the number of recognition attempts
var tryRecognize = 10;
// the number of attempts to choose dynamic pictures
var dynamicImagesRecognizeAttempts = 20;
// the number of attempts to load the element
var tryLoadElement = 60;
// get the full answer
bool fullAnswer = false;
// show the messages about recognition progress
var needShowMessages = false;
// check correctness of the recognized answer
var needToCheck = true;
 
// Secondary variables
 
// tab
Tab tab = instance.ActiveTab;
// congratulations, you are not a robot
var success = false;
// time is over
var timeout = false;
// task for recaptcha 2
string task = string.Empty;
// image url
var src = string.Empty;
// picture in base64 format
var imageString = string.Empty;
// answer for the captcha
string answer = string.Empty;
// the captcha has been changed
var changed = false;
// empty answer
bool answerIsEmpty = false;
// dynamic captcha
bool dynamicCaptcha = false;
// input the captcha several times
bool notOneEnter = false;
var coincidenceReCaptcha2Index = -1;
 
// Checking of protection passing
Action CheckOK= () => 
{
	tab.WaitDownloading();
	for (int k = 0; k < tryLoadElement; k++)
	{
		System.Threading.Thread.Sleep(waitTime); // waiting for element load
		var check = tab.FindElementByAttribute("div", "class", "recaptcha-checkbox-checkmark", "regexp", 0);
 
		// checking the form's disappearing
		var loadedForm = tab.FindElementByAttribute("div", "class", "primary-controls", "regexp", 0);
		if (loadedForm.IsVoid)
		{
			success = true;
			break;
		}
		else
		{
			int xPrimaryControlsDisplaysment = loadedForm.DisplacementInTabWindow.X;
			int yPrimaryControlsDisplaysment = loadedForm.DisplacementInTabWindow.Y;
 
			if (xPrimaryControlsDisplaysment < 0 || yPrimaryControlsDisplaysment < 0) // there are no visible recaptcha
			{
				success = true;
				break;	
			}
			if (check.IsVoid)
				break;
		}
 
		var more = tab.FindElementByAttribute("div", "class", "rc-imageselect-error-select-more", "regexp", 0);
		var wrong = tab.FindElementByAttribute("div", "class", "rc-imageselect-incorrect-response", "regexp", 0);
		if (!more.IsVoid && !wrong.IsVoid)
		{
			var isNotVisibleMore = more.GetAttribute("outerhtml").Replace(" ","").Contains("display:none");
			var isNotVisibleWrong = wrong.GetAttribute("outerhtml").Replace(" ","").Contains("display:none");
			if (isNotVisibleMore && isNotVisibleWrong)
			{
				if (!check.IsVoid)
				{
					if (check.OuterHtml.Contains("style=\"\"")) 
					{
						success = true;
						break;
					}
					else break;
				}
			}
			else break;
		}
		if (k == (tryLoadElement - 1)) timeout = true;
 
	}
};
 
// Confirmation of the answer
Action VerifyAnswer= () =>
{
	project.SendInfoToLog("Checking correctness after input of dynamic captcha", needShowMessages);
 	tab.WaitDownloading();
	// searching for the button "Submit"
	HtmlElement apply = tab.FindElementById("recaptcha-verify-button");
	if (!apply.IsVoid) apply.Click();
	// checking correctness of the answer
	CheckOK();
};
 
Action InputNotBotText= () =>
{
	tab.WaitDownloading();
	var inputField = tab.FindElementByAttribute("input:text", "id", "default-response", "text", 0);
	if (!inputField.IsVoid)
	{
		inputField.SetValue("I am not robot", "Full");
		VerifyAnswer();
	}
};
 
Action UpdateImage= () => 
{
	project.SendInfoToLog("Captcha's update", needShowMessages);
 
	// Update of the captcha if it is necessary
	if (!changed) 
	{
		HtmlElement reload = tab.FindElementById("recaptcha-reload-button");
 
		if (!reload.IsVoid)
		{
			reload.Click();
			InputNotBotText();
		}
		else timeout = true;
	}
	changed = false;
 
	for (int k = 0; k < tryLoadElement; k++)
	{
		System.Threading.Thread.Sleep(waitTime); // waiting for element load
		// searching for the picture
		var testImage = tab.FindElementByAttribute("img", "class", "rc-image-tile", "regexp", 0);
		if (testImage.IsVoid) continue;
		// get image url
		var newSrc = testImage.GetAttribute("src");
		// if the image has been changed, go out
		if (newSrc != src) break;
		if (k == (tryLoadElement - 1)) timeout = true;
	}
};
 
Action VisibleIndexReCaptchaDefinition= () => {
	tab.WaitDownloading();
	var recaptchaElementsGroup = tab.FindElementsByAttribute("div", "class", "recaptcha-checkbox-checkmark", "regexp");
	int length = recaptchaElementsGroup.Elements.Length;
	if (length == 1)
	{
		coincidenceReCaptcha2Index = 0;
		return;
	}
 
	for(int i = 0; i < length; i++)
	{
		var element = recaptchaElementsGroup.Elements[i];
		if (!element.IsVoid)
		{
			int x = element.DisplacementInTabWindow.X;
			int y = element.DisplacementInTabWindow.Y;
 
			var suspectVisibleElement = tab.GetElementFromPoint(x, y).DisplacementInTabWindow;
			if (x == suspectVisibleElement.X && y == suspectVisibleElement.Y && element.Width != 0 && element.Height != 0 && x != 0 && y != 0)
			{
				coincidenceReCaptcha2Index = i;
				break;
			}
		}
	}
};
 
// Searching for recaptcha 2
Action SearchReCaptcha2= () => 
{
	project.SendInfoToLog("Searching for recaptcha 2", needShowMessages);
 	tab.WaitDownloading();
	for (int k = 0; k < tryLoadElement; k++)
	{
		VisibleIndexReCaptchaDefinition();
		if (coincidenceReCaptcha2Index < 0) coincidenceReCaptcha2Index = 0;
 
		// searching for the button "I am not a robot"
		HtmlElement notRobot = tab.FindElementByAttribute("div", "class", "recaptcha-checkbox-checkmark", "regexp", coincidenceReCaptcha2Index);
 
		// the button exists
		if (!notRobot.IsVoid)
		{
			// click to the button
			notRobot.Click();
			System.Threading.Thread.Sleep(waitTime); // pause
 
			// if input of the captcha is not necessary
			var check = tab.FindElementByAttribute("div", "class", "recaptcha-checkbox-checkmark", "regexp", coincidenceReCaptcha2Index);
			if (!check.IsVoid)
			{
				if (check.OuterHtml.Contains("style=\"\""))
				{
					success = true;
					timeout = false;
					break;
				}
			}
		}
 
		// the form exists
		var loadedForm = tab.FindElementByAttribute("div", "class", "primary-controls", "regexp", 0);
		if (!loadedForm.IsVoid)
			break;
 
		// waiting for element load
		System.Threading.Thread.Sleep(waitTime);
		if (k == (tryLoadElement - 1)) timeout = true;
	}
};
 
// searching for recaptcha 2 task
Action SearchTask= () => 
{
	tab.WaitDownloading();
	project.SendInfoToLog("Searching for the task", needShowMessages);
	dynamicCaptcha = false;
	notOneEnter = false;
	answer = String.Empty;
 
	for (int k = 0; k < tryLoadElement; k++)
	{
		HtmlElement taskHe = tab.FindElementByAttribute("div", "class", "rc-imageselect-desc-wrapper", "regexp", 0);
 
		if (!taskHe.IsVoid)
		{
			task = taskHe.GetAttribute("innertext"); // getting the task
			string suspecttask = task.ToLower();
 
			if (suspecttask.Contains("click verify once there are none left") || suspecttask.Contains("when images will be end") ||
	suspecttask.Contains("когда изображения закончатся") ||
	suspecttask.Contains("коли зображень уже не залишиться, натисніть \"підтвердити\"") ||
	suspecttask.Contains("cliquez sur le bouton de validation") ||
	suspecttask.Contains("klicken sie") ||
	suspecttask.Contains("fai clic su verifica dopo averle selezionate tutte") ||
	suspecttask.Contains("gdy wybierzesz wszystkie, kliknij weryfikuj"))
		dynamicCaptcha = true;
 
if (suspecttask.Contains("if there are none, click skip") ||
	suspecttask.Contains("if they do not exist, click \"skip\"") ||
	suspecttask.Contains("wenn du keine siehst") ||
	suspecttask.Contains("s'il n'y en a aucune, cliquez sur \"ignorer\"") ||
	suspecttask.Contains("если их нет, нажмите \"пропустить\"") || suspecttask.Contains("якщо нічого немає") ||
	suspecttask.Contains("ich nie ma, kliknij") ||
	suspecttask.Contains("se non ne vedi, fai clic su salta"))
		notOneEnter = true;
 
			timeout = false;
			break;	
		}
 
		System.Threading.Thread.Sleep(waitTime); // waiting for load element
		if (k == (tryLoadElement - 1)) timeout = true;
	}
};
 
// searching for the image
Action SearchImage= () => 
{
	tab.WaitDownloading();
	project.SendInfoToLog("Searching for the image", needShowMessages);
 
	for (int k = 0; k < tryLoadElement; k++)
	{
		HtmlElement image = null;
		if (dynamicCaptcha) image = tab.FindElementByAttribute("table", "class", "rc-imageselect-table", "regexp", 0); 
		else image = tab.FindElementByAttribute("img", "class", "rc-image-tile", "regexp", 0);
 
		// if there is the image
		if (!image.IsVoid)
		{
			// getting the image url
			if (!dynamicCaptcha) src = image.GetAttribute("src");
			imageString = image.DrawToBitmap(!dynamicCaptcha);
			timeout = false;
			break;
		}
 
		System.Threading.Thread.Sleep(waitTime); // waiting for element load
		if (k == (tryLoadElement - 1)) timeout = true;
	}
};
 
// Recognition
Action Recognize= () => {
	project.SendInfoToLog("Recognition", needShowMessages);
	var answerString = ZennoPoster.CaptchaRecognition("CapMonster2.dll", imageString, String.Format("Task={0}&FullAnswer={1}&CapMonsterModule=ZennoLab.ReCaptcha2", task, fullAnswer));
	var split = answerString.Split(new [] { "-|-" }, StringSplitOptions.RemoveEmptyEntries);
	answer = split[0];
};
 
//Input the answer
Action InputAnswer= () => 
{
	if (!String.IsNullOrEmpty(answer) && answer != "sorry")
	{
        project.SendInfoToLog("Input the answer and checking of correctness", needShowMessages);
		int count = 0;
 
        string[] answers;
		if (answer.Contains(",")) 
			answers = answer.Split(new [] { "," }, StringSplitOptions.RemoveEmptyEntries);
		else 
		{
			answers = new string[answer.Length];
			for (int i = 0; i < answer.Length; i++)
				answers[i] = answer[i].ToString();
		}
 
		foreach (string c in answers)
		{
			if (fullAnswer)
				if (count == 2) break;
 
			int index = Convert.ToInt32(c) - 1;
			HtmlElement he = tab.FindElementByAttribute("img", "class", "rc-image-tile", "regexp", index);
 
			if (!he.IsVoid) 
			{
				he.Click(); //click to the image
				System.Threading.Thread.Sleep(500);// pause
			}
			if (fullAnswer) count++;
		}
 
		// searching for the button "Submit"
		HtmlElement apply = tab.FindElementById("recaptcha-verify-button");
		if (!apply.IsVoid) apply.Click();
 
		// checking correctness of the answer
		CheckOK();
		if (success) return;
 
		// input the other part of the answer
		if (fullAnswer)
		{
			for (int i = count; i < answer.Length; i++)
			{
				// searching for the picture again
				var testImage = tab.FindElementByAttribute("img", "class", "rc-image-tile", "regexp", 0);
				if (testImage.IsVoid) break;
				// getting the image url
				var newSrc = testImage.GetAttribute("src");
				// if the image has been changed, go out
				if (newSrc != src) break;
				else changed = true;
				// otherwise to continue the insertion
				int index = Convert.ToInt32(answer[i].ToString()) - 1;
				var he = tab.FindElementByAttribute("img", "class", "rc-image-tile", "regexp", index);
				if (!he.IsVoid) 
				{
					he.Click();
					System.Threading.Thread.Sleep(500); // pause
					if (!apply.IsVoid) apply.Click();
					CheckOK();
					if (success) return;
				}
			}
		}
	}
	else answerIsEmpty = true;
};
 
//Input the answer
Action InputDynamicAnswer= () => 
{
	project.SendInfoToLog("Input the answer of dynamic captcha", needShowMessages);
 
	string[] answers;
	if (answer.Contains(",")) 
		answers = answer.Split(new [] { "," }, StringSplitOptions.RemoveEmptyEntries);
	else 
	{
		answers = new string[answer.Length];
		for (int i = 0; i < answer.Length; i++)
			answers[i] = answer[i].ToString();
	}
 
	foreach (string number in answers)
	{
		int index = Convert.ToInt32(number) - 1;
		HtmlElement he = tab.FindElementByAttribute("img", "class", "rc-image-tile", "regexp", index);
		if (he.IsVoid) he = tab.FindElementByAttribute("div", "class", "rc-image-tile-wrapper", "regexp", index);
		if (!he.IsVoid) 
		{
			//click to the image
			he.Click();
			// pause
			System.Threading.Thread.Sleep(500);
		}
	}
 
	// pause
	System.Threading.Thread.Sleep(waitTime*2);
};
 
//Input the answer
Action InputDynamicAnswer2= () => 
{
	project.SendInfoToLog("Input the answer of dynamic captcha", needShowMessages);
 
	string[] answers = answer.Split(new [] { "," }, StringSplitOptions.RemoveEmptyEntries);
	foreach (string number in answers)
	{
		int index = Convert.ToInt32(number) - 1;
		HtmlElement he = tab.FindElementByAttribute("img", "class", "rc-image-tile", "regexp", index);
		if (!he.IsVoid) 
		{
			//click to the image
			he.Click();
			// pause
			System.Threading.Thread.Sleep(500);
		}
	}
 
	// pause
	System.Threading.Thread.Sleep(waitTime*2);
};
 
SearchReCaptcha2();
if (success)
	return "ok";
 
if (timeout) throw new Exception("Waiting time of element load is over");
 
for (int i = 0; i < tryRecognize; i++)
{
	project.SendInfoToLog(String.Format("Attempt №:{0}", i+1), needShowMessages);
 
	InputNotBotText();
	SearchTask();
	if (timeout) break;
 
	// Additional checking
	CheckOK();
	if (success) return "ok";
 
	int count = 0;
 
	// if captcha is dynamic
	if (dynamicCaptcha)
	{
		while (count < dynamicImagesRecognizeAttempts)
		{
			if (count > 0)
				System.Threading.Thread.Sleep(waitTime * 3); // waiting for load of disappearing images
 
			SearchImage();
			if (timeout) break;
			Recognize();
			if (!String.IsNullOrEmpty(answer) && answer != "sorry") InputDynamicAnswer();
			else 
			{
				VerifyAnswer();
				CheckOK();
				if (!success) answerIsEmpty = true;
				break;
			}
			count++;
		}
	}
	else
	{
		if (notOneEnter)
		{
			while (notOneEnter && !dynamicCaptcha && count < dynamicImagesRecognizeAttempts)
			{
				SearchImage();
				if (timeout) break;
				Recognize();
				if (!String.IsNullOrEmpty(answer) && answer != "sorry") InputDynamicAnswer2();
				VerifyAnswer();
				timeout = false;
				if (success) break;
				SearchTask();
				if (timeout) break;
				count++;
			}
		}
		else
		{
			SearchImage();
			if (timeout) break;
			Recognize();
			InputAnswer();
		}
	}
	if (timeout) break;
 
	if (!needToCheck) return "ok";
 
	if (answerIsEmpty)
	{
		answerIsEmpty = false;
		UpdateImage();
		continue;
	}
 
	if (success) return "ok";
 
	if (i != (tryRecognize - 1)) UpdateImage();
	if (timeout) break;
}
 
if (timeout) throw new Exception("Waiting time of element load is over");
else throw new Exception("It has not been recognized. The recognition attempts has ended, before the answer has been counted");
NOTE
Currently CapMonster 2 supports only english, russian and ukrainian language ReCaptcha.

The snippet allows to perform any number of attempts to recognize captcha. Also, if you use slow proxies, you can increase loading elements waiting timeout and number of attempts to load elements. For it you should change the following parameters:

// waiting timeout
var waitTime = 1000;
// recognition attempts
var tryRecognize = 3;
// attempt to load element
var tryLoadElement = 60;
// need to check correctness of answer
var needToCheck = true;
Depending on your ip-address, ReCaptcha2 may accept answer without or with extra picture. However, if you make mistakes regularly, it won't accept even correct answers. So, we recommend to use reliable proxies.

en/addons/capmonster/rc2.txt · Last modified: 2017/11/13 12:03 by afameless
Page Tools
Show pagesource
Old revisions
Backlinks
Back to top
Except where otherwise noted, content on this wiki is licensed under the following license: CC Attribution-Share Alike 3.0 Unported
CC Attribution-Share Alike 3.0 Unported Donate Powered by PHP Valid HTML5 Valid CSS Driven by DokuWiki
