# Configure Manager

The first step is to drag-and-drop the SUGAR prefab into your scene. This prefab is the core controller of all the SUGAR interactions. 
The SUGAR Unity Manager script component on the prefab holds universal information required by other SUGAR components. Configuring this component is necessary to connect to SUGAR.

* Base Address - web address of SUGAR server (e.g. ``http://localhost:62312/`` or ``http://www.mysugarserver.com``). This is overwritten by the value set inside the config.json file (found in "Assets/StreamingAssets").

* Game Token - name of the <xref:game> used for database lookup.

* Game Id - database row Id of the <xref:game>, returned from token lookup.

* Use Achievements - check this to enable SUGAR <xref:achievement> 

* Use Leaderboards - check this to enable SUGAR <xref:leaderboard> 

