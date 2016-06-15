%1 :: For some reason this needs to be here or the first attempt to store this in a variable won't work.
SET ProjectDir=%1
SET BinDir=%ProjectDir:~0,-1%\Bin"

CD %BinDir%

FOR /R %%F IN (*) DO (
  COPY /Y "%%F" .
)

ECHO DONE