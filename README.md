# SCP-181

-----------------

**TRADUCTION FRANCAISE**

**Description**

SCP-181 est une personne qui manipule les probabilités sans le vouloir, lui permettant d'avoir une chance inouie.
Dans ce plugin, un Classe-D aléatoire le devient, et dispose de pouvoirs comme :

- Esquiver une attaque mortelle de SCP
- Ouvrir des portes sans la carte necessaire

Au fur et à mesure des essais, la chance de SCP-181 diminue.

# Installation

- Déplacez "SCP181Event.dll" dans le dossier "sm_plugins" de votre serveur
- Lancez le serveur une première fois avec le plugin installé afin de générer le fichier de config du plugin


# Modification des probabilités de SCP-181

- Lancez le serveur une première fois avec le plugin
- Une fois fait, l'arrêter, puis modifier le fichier "config.txt" de votre serveur pour rajouter les 3 lignes de configuration tout en bas du README.
- Modifier uniquement les chiffres, et aucun nombre à virgule (nombre d'utilisations possibles avant que le pouvoir disparaisse)


# Fonctionnement des probabilités

En partant du principe de cette configuration du config.txt :
- max_181_dodge_tries: 5
- max_181_door_tries: 10
- minimum_classe_d: 1

La première fois qu'un SCP touche le SCP-181, celui-ci a 5 chances sur 5 d'esquiver le coup (le premier sera donc annulé dans tout les cas)
Puis, sa chance tombera à 4 chances sur 5.

La seconde fois, il se fait toucher par un SCP, mais il ne réussit pas à esquiver (malgré 80% de reussite).
Puis, sa chance tombera à 3 chances sur 5.

- Le fonctionnement est identique pour les portes à cartes. Qu'on reussise ou non d'ouvrir ou d'esquiver, une chance se retire.
- Si on a de base 5 tentatives pour esquiver un SCP, chaque essai fait perdre 20% de reussite à la suivante.
- Si on a de base 10 tentatives pour ouvrir une porte, chaque essai fait perdre 10% de reussite à la suivante.
- SCP-181 spawn s'il y a un minimum d'1 classe-D.

-----------------

**ENGLISH TRANSLATION**

**Description**

SCP-181 is a man who can manipulate probabilities without knowing it, allowing him to have a huge luck.
In this plugin, a random D-class become him, and have powers like :

- Dodge an incoming SCP attack
- Opens restricted doors without the required card

The more you try, the less chance you will have.

# Installation

- Move "SCP181Event.dll" in "sm_plugins" server's folder.
- Launch the server and enjoy ! Don't forget to add the config lines in the config.txt of your server.


# SCP-181's probabilities modification

- Stop the server, and modify "squal_plugins_conf/181.txt" in server's root folder
- Please modify numbers only (no float), those are the maximum attempt of SCP-181


# How probabilities works

Example of "config.txt"'s configuration :
- max_181_dodge_tries: 5
- max_181_door_tries: 10
- minimum_classe_d: 1

The first time a SCP attack the player, this one have 5 chances out of 5 to dodge it (the first attack always will be cancelled)
Then, the player will have 4 chances out of 5 to dodge a SCP's attack.


- If you have 5 attempts to dodge a SCP, each attempt will reduce the chance of dodging the next SCP attack by 20%.
- If you have 10 attempts to open a door, each attempt will reduce the chance of opening a restricted door by 10%.
- SCP-181 spawns if there's a minimum of 1 Class-D.


-----------------

# Config.txt

Config Option | Value Type | Default Value | Description
--- | :---: | :---: | ---
enable_squal_scp_181 | Boolean | true | Enable or not the plugin
max_181_dodge_tries | Integer | 5 | How many attempts SCP-181 has to dodge SCP's attacks
max_181_door_tries | Integer | 10 | How many attempts SCP-181 has to open restricted doors
minimum_classe_d | Integer | 1 | How many Class-D must spawn to make spawn SCP-181
