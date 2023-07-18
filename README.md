
Quick Launcher 2
================

Quick Launcher 2 is a desktop app that helps users run commands with shortcut  
User can use [WinHotKey](https://directedge.us/content/winhotkey/) or [AutoHotKey](https://www.autohotkey.com/) to quickly launch it by shortcut


Screenshot
----------

<img src="https://github.com/lhypds/quick-launcher-2/assets/4526937/ab86a4ca-0a10-464b-9c06-4b30fa870b54" width="500" />


config.txt
----------

`enable_note,TRUE_OF_FALSE`  
Example, `enable_note,true`  
Enable note support.  

`default_text_editor,TEXT_EDITOR_PATH`  
Example, `default_text_editor,C:\Program Files\Sublime Text\sublime_text.exe`  
Set default text editor.  


Shortcut
--------

Ctrl + click a command, enter the command editing  


Command Examples
----------------

* Copy to clipboard  

```
echo | set /p = "TEXT_TO_COPY_INTO_CLIPBOARD" | clip
```

* Copy multiple line text to the clipboard  

```
type C:\example.txt | clip
```

* Convert Github Image Size  

```
echo | set /p = "<img src="IMG_URL" width="400" />" | clip 
```

* Copy the current date and time string to the clipboard  

```
echo | set /p = "%date% %time%" | clip 
```
