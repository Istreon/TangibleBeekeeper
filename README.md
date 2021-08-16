# TangibleBeekeeper

## Notes
Cette branche contient un client RA du projet TangibleBeekeeper. Il est utilisable uniquement avec un Hololens 2.
Pour pouvoir utiliser complètement ce projet, il est nécessaire de lancer au préalable la version "serveur", qui est la branche principale de ce projet (la version RV).

## Objectif
L'objectif de cette verion RA du projet TangibleBeekeeper et d'offrir à un utilisateur, différent de celui qui manipule en utilisant un casque de RV, un monitoring et ainsi suivre les manipulations de l'autre utilisateur en choisissant son point de vue et ainsi permettre de guider l'autre utilisateur.
Dans un contecte plus réaliste avefc des vrais apiculteur, cela permettrait à un formatteur de suivre et d'aider un apprentit utilisant le projet RV.


## Le contenu du projet
![Start menu screenshot](/docs/AR_hierarchie.PNG)  

Différents gameobjects composent la scène :
- **Lights** contient les différents éclairages de la scène
- **PhotonLauncher** permet de se connecter au réseau photon, et ainsi au projet RV si celui ci est lancé.
- **3DTexts** contient des textes en 3D qui sont affichés durant la simulation. Ils offrent ainsi des informations à l'utilisateur, comme l'état de connection au serveur.
- **MixedRealityToolkit**, **MixedRealityPlayspace**, **MixedRealitySceneContent** sont les gameObjects provenant de l'API MRTK pour l'hololens2. Ils offrent l'essentiel pour utiliser un Hololens2 en RA.
- **HologramCollection** contient les différents composant permettant le tracking QR code, mais aussi la ruche virtuelle et ses composants.  

Un fois connecté au serveur photon, la position de la ruche virtuelle et de ses différents composants sont récupéré et mis à jour en continu. Pour pouvoir superposer la ruche virtuelle sur la ruche réelle, un tracking QR code est utilisé. Ainsi, la détection du QR code va permettre de déplacer l'ensemble des éléments de la ruche à la bonne position pour coincider avec leur version réelle.
Le visuel actuel du cadre est aussi mis à jour.
