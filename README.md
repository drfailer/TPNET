# TPNET: Contact Manager

## Structure de données

La structure de données se trouve dans la solution `HierarchicalStructure`.
Les dossier `Folder` et contacts `Contact` dérivent d'une classe abstraite
`Node` (patern composite).

- Une `Node` possède les dates de créations et modifications, ainsi qu'un parent.
- Un `Contact` contient toutes les informations demmandées et chaque contact est
  identifié par **(nom, prenom)**. Pour gérer les mail, une exception à été créée
  et les mail sont testés lors de la création et de la modification.
- Un `Folder` possède une liste de contacts et une liste de sous dossier. Ce choix
  a été fait du fait que l'on a souvent besoin de manipuler soit que des contact,
  soit que des dossier. Cependant, du fait de l'utilisation d'un pattern composite,
  il est tout à fait possible de créer et d'utiliser une liste de `Node`. Les dossiers
  sont identifiés par leur **nom**, et ils ont des methodes permettant la modification,
  suppression et ajout de sous-dossier et de contacts.
  
## Organisation du code

La solution possède 4 projets:
- `HierarchicalStructure`: contient la structure de donnée.
- `Serializer`: contient le nécessaire à la sérialization.
- `ContactManager`: contient le manager et le menu: le manager gère le curseur (dossier
  courant) et le dossier racine. Le menu fait appel au manager pour l'exécution de toutes
  les commandes et le manager gère la structure arborescente en faisant appel au méthodes
  des dossiers et contacts.
- `TPNET`: contient la fonctions main.

# La sérialisation

L'application offre la possibilité de sauvegarder l'arborescence dans 2 types de fichiers.
Le type de fichier à utiliser est connu grace à l'extention du fichier spécifié à la
commande `save`:

```
cm> save fic.xml
cm> save fic.dat
```

Les fichiers de sauvegarde sont chiffrer à l'aide **d'AES 128**. Dans le code, on utilise le
hash de la clé spécifiée par l'utilisateur pour avoir une clé en byte de la bonne taille (donc
on peut mettre n'importe quelle string quand la clé est demmandée). Le vecteur d'initialisation
d'AES est générer aléatoirement lors du chiffrement et il est stocké en clair en début de fichier.
Une autre possiblité est de l'écrir en dur dans le code, cependant cela est moins sécurisé.

A noter que toutes les classes de la structure de données implémentent l'interface `ISerialisable`,
pour rendre possible la sérialisation avec `BinaryFormatter`.

## L'application

L'application se gère via la console, et on accèdes aux différentes features via
l'utilisation de commandes. La commandes `help` peut être utilisée pour obtenir
de l'aide.
