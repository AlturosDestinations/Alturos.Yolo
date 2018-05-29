PATH=%PROGRAMFILES%\7-Zip;%PATH%

:: Install openCV
powershell -Command "Invoke-WebRequest https://netcologne.dl.sourceforge.net/project/opencvlibrary/opencv-win/3.4.0/opencv-3.4.0-vc14_vc15.exe?use_mirror=autoselect -OutFile opencv-3.4.0-vc14_vc15.exe"
opencv-3.4.0-vc14_vc15.exe -o"C:\opencv\340" -y

:: Cleanup
del opencv-3.4.0-vc14_vc15.exe

pause