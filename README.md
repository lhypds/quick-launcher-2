
Quick Launcher 2
================

Quick Launcher 2 is a desktop app that help user run command super fast  
User can use [WinHotKey](https://directedge.us/content/winhotkey/) or [AutoHotKey](https://www.autohotkey.com/) to quickly launch it by shortcut


Screenshot
----------

![image](https://github.com/lhypds/quick-launcher-2/assets/4526937/ab86a4ca-0a10-464b-9c06-4b30fa870b54)


Shortcut
--------

Ctrl + click a command, enter the command editing  


Command Examples
----------------

* Copy to clipboard  

```
echo | set /p = "TEXT_TO_COPY_INTO_CLIPBOARD" | clip
```

* Copy multiple line text to clipboard  

```
type C:\example.txt | clip
```

* Convert Github Image Size  

```
echo | set /p = "<img src="IMG_URL" width="400" />" | clip 
```

* Copy current datetime string to clipboard  

```
echo | set /p = "%date% %time%" | clip 
```
