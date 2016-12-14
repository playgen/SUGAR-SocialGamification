# Account Setup

The next component on the SUGAR prefab is the Account Unity Client script component. This is used to handle the player's login to their SUGAR account or the registration of new accounts.
There are two ways of logging in a user. This can be done by a typical login panel gameobject or login can occur automatically via code if credentials come in from an external source.

* Login User Interface - Login panel gameobject, which appears to prompt the user to login. The gameobject must have the LoginUserInterface script attached. Included in the prefabs folder ("Assets/SUGAR/Prefabs") is a fully functional login panel template. 

* Allow Auto Login - This enables the use of auto-login (bypassing the need for the login panel). If this is checked, and the command line arguments for auto login have been passed, then the Account Unity Client will attempt to login with the credentials. The command line options format is ``game.exe -u [username] -s [source] -autologin``.

* Allow Register - Currently, this displays the makes the register on the login panel visible for custom registration handlers. 

* Default Source Token - Source tokens are set to ensure the player is logging in from an allowed source. If the source is not set in the commandline options, then this value becomes the default.


