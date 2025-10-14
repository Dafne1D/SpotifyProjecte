> En aquest projecte seguirem la [Guideline Oficial de Microsoft](https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/)

# BASE DE DADES
## Disseny de la base de dades
Començarem fent el disseny de la base de dades creant un model Entitat/Relació *(No fan falta els camps)*

**Nom de la BD:** SpotifyDB  
**Taules:**
- *Users* 
- *Songs*
- *Files* (arxius mp3 de les cançons en diferents formats)
- *Playlists*
- *PlaylistSongs* (relacions entre *Playlists* i *Songs*)

## Creació de la base de dades
La base de dades serà un **Microsoft SQL Server**.  
Crearem la base de dades utilitzant **DBeaver**.  
Tot el sql utilitzat per la creació de la base de dades estarà a *db/db.sql*  

<br>

# API
## Detalls de l'api
- Al **Gitignore**:
  - bin
  - obj
- Per totes les classes, utilitzarem **Namespaces**
- Per les contrasenyes dels usuaris, utilitzarem **encriptació** utilitzant *HASH* i *SALT*
- Quan s'hagin de fer operacions sobre la base de dades, es podrán fer processos en paral·lel    


## Disseny de l'API
Crearem les carpetes:
- **Services:** De moment només hi haurà la classe per fer la connexió amb la *base de dades*.
- **Model:** Les classes per fer els objectes *(Users, Songs, Files...)*
- **Repository:** Les classes ADO per fer operacions *CRUD* sobre la base de dades
- **EndPoints:** Les definicions dels EndPoints de l'API  
  
<br>
  
# INTERFAÇ ADMINISTRADOR
## Disseny de l'interficie
Primer crearem el *wireframe* de l'interfaç.  
L'interficie de l'administrador consistirá en múltiples pàgines de manteniments CRUD per cada taula de la base de dades (Users, Songs i Playlists)

## Creació de l'interficie
Crearem un projecte de *WPF* amb Visual Studio Community 2022.  
Crearem una sola pantalla utilitzant WPF amb Visual Studio