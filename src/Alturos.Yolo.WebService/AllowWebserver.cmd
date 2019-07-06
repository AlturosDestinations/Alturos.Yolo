﻿netsh http delete urlacl http://*:8080/
netsh http add urlacl http://*:8080/ user=%username%
pause
