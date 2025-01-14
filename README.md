# CyberSecurity_Bosh


Bonjour, ici votre capitaine de bord Eliot Reniaud accompagné de son fidèle accolyte Chemsdine Ahmidach.
Nous vous présentons dans ce document comment utiliser nos magnifiques codes python et C# qui permettent de diagnostiquer les fautes et bientôt les cyber-attaques dans les lignes de production et en particulier le modèle Schneider.

Il faut commencer par lancer le .exe que l'on trouve au bout du chemin ..\Code d'origine\2012-05-15 FDIToolModBus_modif\2012-05-15 FDIToolModBus\FDITool\bin\Release. S'il n'est pas présent il faut alors le compiler à l'aide par exemple de Visual Studio (bien penser à faire ouvrir le projet et sélectionner le .sln dans ..\Code d'origine\2012-05-15 FDIToolModBus_modif\2012-05-15 FDIToolModBus).
Une fois le programme lancer, on peut lancer le programme "Python lecture continue robuste avec sendUDP". Faire les opérations dans ce sens permet de bien avoir l'initialisation des états dans le programme.
Ce code Python peut se lancer depuis idlepython, il faut bien vérifier que l'automate est branché sur le port indiqué dans le programme avec la bonne adresse IP sans quoi la connexion échouera et le programme s'arrêtera. 
Une fois cela effectué, chaque action sur l'automate sera enregistrée dans un fichier texte en même temps qu'elle est envoyée au programme C# qui la traite dans le but de fournir un diagnostique qui est accessible à l'adresse http://localhost/mss/ .
