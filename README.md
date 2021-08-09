# TangibleBeekeeper

## Notes importantes
***Ce projet est financé par l'IMT Atlantique et réalisé dans le cadre des travaux de recherche de l'équipe INUIT du Lab-STICC.  
C'est une simulation de ruche d'abeille en réalité virtuelle (RV), compatible avec un casque de type Oculus Quest 2. Il utilise un système de tracking Optitrack ainsi qu'une ruche réelle équipée de marqueurs Optitrack. Il est néanmoins possible d'utiliser la simulation sans, en désactivant l'option sur le menu de démarrage de la simulation.  
Deux versions utilisant Photon existent offrant un simulation RV et un monitoring AR pour un Hololens 2 (ces deux versions se trouvent respectivement dans les branches : version_Photon_VR et version_Photon_AR).***

## Mots clés
Interaction tangible, Réalité virtuelle, Réalité augmentée, Immersion, Tracking, Interface Homme Machine


## Objectif du projet
L'objectif de ce projet est d'améliorer les interactions en RV avec une ruche virtuelle. Pour cela, une ruche réelle (sans abeille) et différents outils de l'apiculteur sont utilisés. Ces différents éléments sont traqués avec un système de tracking Optitrack (composé dans mon cas de 24 caméras, même si ce n'est pas nécessaire d'en avoir autant) et superposés avec leurs jumeaux virtuels, afin de ne manipuler qu'un seul objet pour chaque élément.  
Il n'y a pour le moment pas de colonie d'abeille ayant un comportement réaliste dans la simulation, mais l'environnement est travaillé pour offrir un simulation réaliste, offrant des interactions naturelles avec la ruche.  
À long terme, une simulation multi-agent d'une colonie d'abeille sera ajouté, et ce projet servira d'interface tangible entre la colonie et l'utilisateur.



![Contribution guidelines for this project](/docs/startMenu.PNG)
