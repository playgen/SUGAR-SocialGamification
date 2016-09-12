%1 :: For some reason this needs to be here or the first attempt to store this in a variable won't work.
%2
 SET BuildDir=%1
 SET ProjectDir=%2
copy %BuildDir% "%ProjectDir%/../../../Assets/Plugins/SUGAR"

