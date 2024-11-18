:: Copyright (c) Microsoft Corporation. All rights reserved.
:: Licensed under the MIT License.

:: Note: This script is used to skip a strong-naming check for Puppet apps.

:: Usage in Admin mode: 
::   .\strong-named-sign-delay.cmd [action] [assembliesPath]
:: Where: 
::   [action] - 'Vr' for register assemblies or 'Vu' for unregister assemblies.
::   [assembliesPath] - path to assemblies (ex. bin\Release\).

setlocal
set action=%1
set pathToFile=%2

call "%VSAPPIDDIR%..\Tools\VsDevCmd.bat"

call :registerOrUnregisterFiles %pathToFile%BD.AppCenter.dll
call :registerOrUnregisterFiles %pathToFile%BD.AppCenter.Analytics.dll
call :registerOrUnregisterFiles %pathToFile%BD.AppCenter.Crashes.dll
call :registerOrUnregisterFiles %pathToFile%BD.AppCenter.Distribute.dll
EXIT /B

:registerOrUnregisterFiles
echo "Start processing the file: %~1"
if exist %~1 (
    sn -%action% "%~1"
) else (
    echo "File %~1 doesn't exist".
)
