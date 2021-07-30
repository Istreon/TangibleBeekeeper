# TangibleBeekeeper - VersionQuest

Cette version du projet a été développée pour être portable sur casque *Oculus Quest* (2).

## Générer un exécutable
### Paramètres
Les paramètres à régler afin d'obtenir un exécutable (.apk) fonctionnels sont les suivants :

**Build Settings :**  
* Texture compression : `ASTC`

**Player Settings :**  
* XR Plug-in Management/Oculus/Stereo Rendering Mode : `Mutli Pass`  
* Quality/Rendering : utiliser le fichier de settings `UniversalRenderPipelineAsset` (Assets/Shader/UniversalRenderPipelineAsset.asset)  
* (Optionnel) Graphics/Always Included Shaders : ajouter le shader `BeePointCloudShader` (Assets/ImportedAssets/3DGraph/GraphShader/Custom/BeePointCloudShader.shader)  

**Universal Render Pipeline Asset :**  
* Quality/Anti Aliasing : `x4`  

Après avoir branché le casque et vérifié qu'il est détecté par *Unity*, cliquer sur `Build And Run` ce qui permettra de générer l'exécutable tout en l'installant sur le casque.

### Lancer l'application dans le casque
Pour retrouver l'application dans le casque :
* aller dans `Application` ;
* changer `Toutes les applications` par `Sources Inconnues` ;
* lancer le fichier désiré.


## Structure de l'application
L'application est structurée en plusieurs scènes afin de fournir une narration à l'utilisateur. L'ordre des scènes est le suivant :
* `LobbyScene`
* `DivisionScene`
* `Scenario0Scene`
* `Scenario1Scene`

### Contenu des scènes
La scène `LobbyScene` correspond à l'écran d'accueil de l'application. La seule action demandée à l'utilisateur correspond à changer de scène.  
La scène `DivisionScene` offre la possibilité à l'utilisateur de faire une division de ruche en écoutant un tutoriel, puis en vérifiant sa division. C'est dans cette scène que se fait la manipulation d'objet (à deux mains).  
Les scènes `Scenario0Scene` et `Scenario1Scene` correspondent à l'étude d'un graph 3D et de sa comparaison avec le graph 2D associé. Le graph 3D est manipulable "à la main" (pour la rotation dans l'espace) ou grâce aux joysticks des contrôleurs.

### Transition entre les scènes
Lors du démarrage de l'application, un écran de chargement (`LoadingScreen`) est initialisé. Il sera ensuite transmis de scène en scène au fur et à mesure de la navigation dans l'application. Avant de passer à une scène suivante, il y a un écran d'"attente" qui signifie à l'utilisateur qu'il doit enlever son casque avant de passer à la suite (dans le cadre de l'expérimentation mise en place). Le chargement de la scène suivante se fait après cet écran d'"attente".  
Un menu est disponible (bouton de menu sur le contrôleur gauche) pour choisir une scène ou rejouer le tutoriel.

### Manipulation des cadres
Dans la scène de division, les objets se manipulent à la main (avec le bouton de "grip" des contrôleurs), et peuvent tous être saisis à deux mains. Les objets manipulables dans cette scènes sont les cadres ainsi que l'enfumoir.  
Il y a deux types de cadres : une version sans poids (Assets/Model/Interactable/UnweightedFrame.prefab) et une version avec retour pseudo-haptique du poids. Pour la représentation du poids, deux préfabs sont disponibles (toujours dans Assets/Model/Interatable/) :
* `HingeFrame` qui permet simplement d'avoir une rotation du cadre lorsqu'il n'est saisi qu'avec une main ;
* et `WeightedFrameV1` qui permet - en plus de la rotation - d'avoir une difference de position entre la main virtuelle et la vraie main (pour simuler le poids) /!\ CETTE VERSION N'EST PAS STABLE /!\

### Manipulation du graph 3D
La manipulation du graph 3D se fait principalement grâce aux contrôleurs. Le joystick du contrôleur gauche permet de faire avancer/reculer le graph dans le temps. Le joystick du contrôleur droit permet de faire tourner le graph avec des rotations pré-définies ; sinon, il est possible de saisir le graphique (bouton de "grip") pour le faire tourner comme on le souhaite.

### Avancement et narration dans les scènes
Certaines scènes disposent d'un tutoriel et leur avancement est géré par un manager (objet `SceneProgression`). Pour modifier le déroulé d'une scène, il faudra modifier le script d'avancement associé à cet objet.

## Scripts
### Managers d'objet/de comportement (Assets/Scripts/OculusMode/)
Ces scripts permettent notamment de gérer le processus de division (dans `DivisionScene`).
* **DivisionManager.cs :** gère l'affichage de l'avancement et du résultat de la division. Ce script est appliqué à l'objet `CompleteHive` (du prefab éponyme) ;
* **HiveManager.cs :** gère le placement des cadres dans la ruches. C'est avec ce script que les positions disponibles pour les cadres sont initialisées et les cadres associés créés. Les différents paramètres (ruche pleine/vide, utilisation du poids des cadres ou non) sont associés à ce script. Ce script est associé à l'objet `HivePrincipalBox` (du prefab CompleteHive.prefab) ;
* **WoodFrameManager.cs :** gère les cadres (leur type, leur mass, la présence de la reine). Ce script est en communication avec `HiveManager.cs` pour le positionnement et le rangement des cadres dans la ruche. Ce script est associé au cadre (`UnweightedFrame`, `HingeFrame` ou `WeightedFrameV1`)
* **JointManager.cs :** est utilisé pour les cadres avec poids et gère les ressorts des joints associés (le ressort des joints dépend de la masse du cadre). Ce script permet également de positionner l'ancre des joints en fonctions de point d'attache de la main. Ce script est associé au cadre (`HingeFrame` ou `WeightedFrameV1`).
* **HandleManager.cs :** gère les contraintes des joints entre les poignées des cadres et le cadre lui-même. Ce script gère également le changement de couleur du cadre lorsque ses poignées sont survolées par la main.

### Gestions des objets *interactables* (Assets/Scripts/OculusMode/Interactable/)
Ces scripts gèrent les objets saisissables à la main. (`GrabPointManager.cs` n'est pas/plus utilisé)
* **OffsetGrab.cs :** hérite de `XRGrabInteractable.cs` (permettant de saisir un objet) et permet de ne pas centrer l'objet saisi sur la main.
* **InteractiveGrabber.cs :** est associé aux poignées des objets. Ce script à la même fonction que `OffsetGrab.cs` et permet de communiquer à l'objet père de la poignée que l'objet a été saisi. Ce script permet également d'attacher le modèle de la main à l'objet père.

### Gestions des interacteurs (Assets/Scripts/OculusMode/Interactor/)
Ces scripts gèrent les différents interacteurs nécessaires pour saisir les objets ou interagir avec le graph 3D.
* **HandPresence.cs :** gère l'initialisation du modèle des mains ainsi que leur animation (est associé au XRController/ModelPrefab de chaque main (Optionnel))
* **GhostHandPresence.cs :** gère la même chose que `HandPresence.cs` et permet d'attacher l'objet associé à ce script à l'objet saisi (utile pour une meilleure visualisation du poids)
* **GrabInteractor.cs :** est associé à un objet qui peut être saisi et contrôle le nombre de main ayant saisi l'objet, son changement de couleur lors du survol ou de la sélection ainsi que les vibrations associées à la collision de cet objet avec un autre
* **HandInteractor.cs :** permet de gérer l'attache des *GhostHands* à l'objet saisi. Ce script hérite de `XRDirectInteractor.cs` et le remplace pour chaque main  
(les autres scripts ne sont pas/plus utilisés)

### Avancement des scènes (Assets/Scripts/OculusMode/SceneManagement/)
Ces scripts gèren l'avancement des scènes ainsi que le passage (chargement) entres-elles. (`HiveSceneProgression.cs` n'est pas/plus utilisé)
* **SceneProgression.cs :** classe mère qui gère l'avancement de la scène en cours *via* les boutons 'A' et 'X' des contrôleurs. Permet de passer d'une scène à l'autre en gérant l'audio de la scène et l'écran de chargement. Les classes `LobbyProgression.cs`, `DivisionProgression.cs`, `Scenario0Progression.cs` et `Scenario1Progression.cs` héritent de cette classe et l'adapte en fonction des besoins de la scène (tutoriel, par exemple)
* **BlackBocMode.cs :** est associé à l'objet `VRRig` et permet de plonger l'utilisateur dans le noir lors du chargement de la scène suivante en rendant inactifs les interacteurs de l'utilisateur, ainsi que d'afficher le menu
* **LoadingScreen.cs :** permet de gérer l'écran de chargement ainsi que le chargement lui-même des scènes. Ce script permet également de transmettre ce même écran de chargement entre les différentes scènes

### UI (Assets/Scripts/OculusMode/UI/)
Ces scripts gèrent le comportement du *ring menu* utilisé pour la rotation du graph 3D (voir la vidéo dont sont tirés les scripts : https://www.youtube.com/watch?v=tPH_BYRsvxI)

### Graph 3D (Assets/ImportedAssets/3DGraph/GraphScripts/)
Ces scripts gèrent l'affichage et la manipulation du graph 3D.
* **BeeData.cs :** gère la récupération des données, les organisent et permet d'avancer/reculer dans le temps grâce au joystick gauche.
* **ContactGrapherRetriever.cs :** permet de mettre à jour les points du graph en en récupérant la liste depuis `BeeData.cs`. Ce script permet également de placer les points de façon relative en fonction du maximum
* **UpdateOrder.cs, PointCloudReferencer.cs, MyPointCloud.cs et IDManager.cs :** gèrent les points du graph (affichage, couleur, etc.)
* **GraphAnchorManager.cs :** gère la rotation libre (à la main) et prédéfinie (avec le joystick droit) du graph
* **RotatorTowardsCam.cs :** gère la rotation des textes du graph 3D de façon à ce qu'ils soient toujours lisibles par l'utilisateur
(les autres scripts ne sont pas/plus utilisés)


## Ressources et prefabs utlisés
### Assets/Ressources/
Ce dossier contient notamment les prefabs associés au *VR Rig* ainsi qu'aux écrans d'"attente" et de chargement.  
Il contient, de plus, un dossier `custom_dir` dans lequel se trouvent les fichiers .txt et .csv utilisés dans l'application (textes des tutoriels, données du graph 3D) afin de pouvoir être retrouvés dans le casque.

### Assets/Model/Interactable/
Ce dossier contient tous les prefabs utilisé pour `DivisionScene`. On y trouve notamment le prefab de la ruche ainsi que des différents types de cadres utilisés.

### Assets/Prefab
Ce dossier contient le prefab des graphs. Il inclut le graph 2D ainsi que le graph 3D composé lui-même de prefabs disponibles dans le dossier Assets/ImportedAssets/3DGraph/GraphPrefabs/ .

### Assets/UI/
Ce dossier contient les éléments permettant de faire le *ring menu* utilisé pour la manipulation du graph 3D.  
Dans le dossier "Texture/" se trouvent toutes les images et *sprites* utilisés dans l'UI de l'application.
