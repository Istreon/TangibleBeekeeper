# TangibleBeekeeper

## Notes importantes
***Ce projet est financé par l'IMT Atlantique et réalisé dans le cadre des travaux de recherche de l'équipe INUIT du Lab-STICC.  
C'est une simulation de ruche d'abeille en réalité virtuelle (RV), compatible avec un casque de type Oculus Quest 2. Il utilise un système de tracking Optitrack ainsi qu'une ruche réelle équipée de marqueurs Optitrack. Il est néanmoins possible d'utiliser la simulation sans, en désactivant l'option sur le menu de démarrage de la simulation.  
Deux versions utilisant Photon existent offrant une simulation RV et un monitoring AR pour un Hololens 2 (ces deux versions se trouvent respectivement dans les branches : Main et version_Photon_AR).***

## Mots clés
Interaction tangible, Réalité virtuelle, Réalité augmentée, Immersion, Tracking, Interface Homme Machine, Unity


## Objectif du projet
L'objectif de ce projet est d'améliorer les interactions en RV avec une ruche virtuelle. Pour cela, une ruche réelle (sans abeille) et différents outils de l'apiculteur sont utilisés. Ces différents éléments sont traqués avec un système de tracking Optitrack (composé dans mon cas de 24 caméras, même si ce n'est pas nécessaire d'en avoir autant) et superposés avec leurs jumeaux virtuels, afin de ne manipuler qu'un seul objet pour chaque élément.  
Il n'y a pour le moment pas de colonie d'abeille ayant un comportement réaliste dans la simulation, mais l'environnement est travaillé pour offrir un simulation réaliste, offrant des interactions naturelles avec la ruche.  
À long terme, une simulation multi-agent d'une colonie d'abeille sera ajouté, et ce projet servira d'interface tangible entre la colonie et l'utilisateur.

## Le contenu du projet

Ce projet est composé de deux scènes Unity :
- Le menu de démarrage
- La simulation

### Le menu de démarrage

![Start menu screenshot](/docs/startMenu.PNG)

Ce menu est composé d'un bouton "Play" permettant de passer à la scène suivante (la simulation) et d'une case à cocher (checkbox) permettant d'activer ou non l'optitrack pour la simulation. Si aucun système Optitrack allumé n'est relié à la simulation, alors il faut décocher la case.

### La simulation de la ruche

![Simulation editor screen](/docs/simulationEditor.PNG)

La capture d'écran montre différents éléments dans la hiérarchie de cette scène :
- **XR** : contient la caméra et les éléments permettant d'utiliser la réalité virtuelle (on peut y trouver les manettes, le handtracking oculus etc..)
- **OptiTrack_mode** : gère la partie Optitrack, et peux activer ou désactiver les composants Optitrack de la scène
- **TrackedObjects** : contient tous éléments qui sont traqués par le système Optitrack (la ruche, le toit, les cadres etc..). Pour faire simple, ce sont tous les éléments que l'utilisateur peut manipuler en RV, avec ou sans Optitrack
- **Terrain** : contient tous les éléments du décors, ainsi que la lumière de la scène
- **VisualEnhancement** : contient les éléments d'amélioration du visuel (actuellement juste un "Reflection Probe")
- **Bees** : contient les abeilles se déplaçant dans la scène
- **HoneycombVisualManager** : permet de modifier le visuel des cadres, en récupérant un input clavier (touche 'N' actuellement) pour faire passer tous les cadres liés au visuel suivant
- **CatchDataManager** : permet d'enregistrer dans un fichier la positionnement des cadres dans la ruche en appuyant sur une touche du clavier (touche 'K' 'L' et 'M').
- **PhotonLauncher** : permet de lancer la connexion au serveur photon, et de créer un salle ou en rejoindre une.

#### XR

![XR gameObject](/docs/go_XR.PNG)

Le gameObject XR contient différents éléments permettant d'utiliser un casque de RV. La structure globale provient du XR rig proposé par Unity.

![XR Rig unity](/docs/go_xr_rig.png)

Dans le but d'utiliser le handTracking du casque Oculus Quest 2, un composant "OVR Manager" est ajouté au gameObject "XR Rig".

![OVR_manager](/docs/OVR_manager_script.PNG)

Ainsi les deux gameObjects "OVRHandPrefabRight" et "OVRHandPrefabLeft" fonctionneront. Ce sont les mains respectivement droite et gauche permettant le handtracking.  
Le gameObject "OculusQuest2SimpleControllers" représente les manettes Oculus quest2. 

Enfin, si le projet devait être amené à être utilisé avec un autre casque, le gameObject ControllerHandVisual doit être activer pour remplacer "OculusQuest2SimpleControllers" qui est spécifique à l'Oculus Quest 2. Il faut aussi désactiver le script "OVR Manager", et bien évidemment changer les paramètre XR (décoché Oculus et cocher OpenXR)

![XR settings](/docs/XR_settings.PNG)

#### Optitrack_mode

![Optitrack mode](/docs/go_optitrack_mode.PNG)

Ce gameObject contient deux gameObjects.
- "Client - Optitrack" correspond au client recevant les données de tracking Optitrack.
- "FrameTrackingSecurity" désactive le tracking optitrack des cadres quand ceux-ci sont dans la ruche et que le toit couvre la ruche. Ainsi, cela évite les pertes de tracking qui provoquent les bugs visuels.

#### TrackedObjects

Ce gameObject contient plusieurs gameObjects.

![TrackedObjects](/docs/go_trackedObjects.PNG)

- **Hive_frames** contient les différents cadres de la ruche.
- **HivePrincipaleBox** correspond à la partie haute de la ruche, pouvant contenir les cadres
- **rehausse_de_ruche** correspond à la réhausse de la ruche + le socle de la ruche.
- **HiveRoof** correspond au toit de la ruche.
- **Smoker** correspond à l'enfumoir.  

Chaque cadre de la ruche :
- Peut être manipulé avec les manettes de réalité virtuelle grâce au script "OffsetGrab.cs"
- Est manipulable avec un objet tangible réel traqué grâce au système Optitrack (le script "OptitrackRigidBody.cs" est équipé au cadre)
- Peut détecter son positionnement dans la ruche grâce au script "FramePositionningDetection.cs" et un gameObject fils nommé "FramePositionReference".
- Sa position et son orientation son communiquées via le service Photon.
- Le visuel du contenu du cadre peut changer avec son gameObject fils nommé "miel_visuel" équipé du script "HoneycombVIsualSelector.cs".

![Cadre de ruche](/docs/go_cadre_ruche.PNG)

De la même façon, le toit de la ruche, le corps principal, la réhausse et l'enfumoir peuvent être manipulés avec les manettes de réalité virtuels, avec les objets tangibles réels correspondant et voir leur position et orientation communiquées via photon.

L'enfumoir peut en plus être activé et libérer de la fumée, avec le script "Smoker.cs", en activant la gachette de la manette de RV ou en utilisant l'enfumoir tangible équipé d'une carte nodeMCU ESP8266 et le script "ConnectedSmoker.cs". Le code arduino se trouve dans le code source du projet, dans le dossier "ArduinoSourceCode/". Pour connected l'arduino au WIFI, il faut directement modifier les paramètre dans le code source. Il faut aussi récupérer l'adresse IP de la carte (en passant par l'editeur Arduino et en lisant le port série avec la carte branchée au pc) pour la renseigner dans l'editeur Unity dans le paramètre "Host" du script "ConnectedSmoker.cs".
