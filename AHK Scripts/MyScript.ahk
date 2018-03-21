MsgBox, CTRL+A will send "Hello World!" and CTRL+R will reload this script.

^r::
	MsgBox, Reloading now...
	Reload
	Sleep, 500
return

^a::Send, Hello World
return
