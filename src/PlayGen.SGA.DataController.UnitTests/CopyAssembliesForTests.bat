ECHO STARTED: CopyAssembliesForTests

%1 :: For some reason this needs to be here or the first attempt to store this in a variable won't work.
SET ProjectDir=%1
SET BinDir="%ProjectDir%\Bin"

::CD %BinDir%

%BinDir%

FOR /R %BinDir% %%F IN (*) DO (
  COPY /Y "%%F" %BinDir%
)

ECHO COMPLETED: CopyAssembliesForTests