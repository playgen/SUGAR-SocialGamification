RMDIR ..\unity-client\api /S /Q 2>nul
RMDIR ..\unity-client\development /S /Q 2>nul
RMDIR ..\unity-client\tutorials /S /Q 2>nul
RMDIR ..\unity-client\features /S /Q 2>nul

XCOPY ..\..\..\sugar-unity\docs\api ..\unity-client\api\ /E /Y
XCOPY ..\..\..\sugar-unity\docs\development ..\unity-client\development\ /E /Y
XCOPY ..\..\..\sugar-unity\docs\tutorials ..\unity-client\tutorials\ /E /Y
XCOPY ..\..\..\sugar-unity\docs\features ..\unity-client\features\ /E /Y