:aaa
set /a n+=1
if %n% leq 4 (
start cmd \k ipconfig
goto :aaa)