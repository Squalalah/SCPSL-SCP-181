# SCP-181

**Description**

SCP-181 est une personne qui manipule les probabilités sans le vouloir, lui permettant d'avoir une chance inouie.
Dans ce plugin, un Classe-D aléatoire le devient, et dispose de pouvoirs comme :

- Esquiver une attaque mortelle de SCP
- Ouvrir des portes sans la carte necessaire

Au fur et à mesure des essais, la chance de SCP-181 diminue.


# Modification des probabilités de SCP-181

- Lancez le serveur une première fois avec le plugin
- Une fois fait, l'arrêter, puis modifier le fichier "181.txt" à la source du serveur
- Modifier uniquement les chiffres, et aucun nombre à virgule (nombre d'utilisations possibles avant que le pouvoir disparaisse)


# Fonctionnement des probabilités

En partant du principe de cette configuration de 181.txt :
- #max_181_dodge_tries:5
- #max_181_door_tries:10

La première fois qu'un SCP touche le SCP-181, celui-ci a 5 chances sur 5 d'esquiver le coup (le premier sera donc annulé dans tout les cas)
Puis, sa chance tombera à 4 chances sur 5.

La seconde fois, il se fait toucher par un SCP, mais il ne réussit pas à esquiver (malgré 80% de reussite).
Puis, sa chance tombera à 3 chances sur 5.

- Le fonctionnement est identique pour les portes à cartes. Qu'on reussise ou non d'ouvrir ou d'esquiver, une chance se retire.
- Si on a de base 5 tentatives pour esquiver un SCP, chaque essai fait perdre 20% de reussite à la suivante.
- Si on a de base 10 tentatives pour ouvrir une porte, chaque essai fait perdre 10% de reussite à la suivante.

