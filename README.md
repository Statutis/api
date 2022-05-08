<div>
  <h1 align="center">Statutis API</h1>

  <p align="center">Développé par HEBAN Simon & Ludwig SILVAIN</p>
</div>

# Packages utilisés

Le projet utilise différent package pour pouvoir fournir les éléments de base, on retrouve donc :

- `Npgsql.EntityFrameworkCore.PostfreSQL` : Permet la connexion à la base de données
- `Swashbuckle.AspNetCore` : Permet d'afficher l'interface OpenAPI
- `DNSClient` : Permet la résolution des champs DNS
- `Isopoh.Cryptography.Argon2` : Permet de le hashage en Argon2id

# Les différentes couches

Le projet a été construit autour de l'architecture N-Tiers. On utilise dans ce projet 6 couches :

- `API` : Gère les end-points
- `Business` : Gère la logique du logiciel
- `Core` : Contient toutes les interfaces à implémenter
- `Cron`: Service permettant la vérification des services
- `DbRepository` : Gère les opérations liées à la base de données, contient aussi le `DbContext`
- `Entity` : Contient toutes les entitées utilisées dans le projet. 


# Mise en place de l'environnement de développement

Tout d'abord il va falloir démarrer la base de données pour cela un fichier `docker-compose.yml` se trouve à la racine du projet. Pour le démarrer il suffit d'exécuter la commande suivante : 
- `docker-compose up -d`

Maintenant que la base de données est démarrée, il faut restaurer le projet grâce à la commande suivante:
- `dotnet restore`

Une fois restauré il ne reste plus qu'à build le projet:
- `dotnet build`
