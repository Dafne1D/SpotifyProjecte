# Rols
- **Usuaris:**
  - Tindrá *N* Rols
- **Rols:**
  - Tindrá *N* Permisos
  - Podrá estar en *N* Usuaris
- **Permisos:**
  - Podrá estar en *N* Rols

**Coses a tenir en compte:**  
- Es podrá editar rols, pero no afegir ni treure.
- Als rols s'els hi podrá assignar permisos
- No es podrán crear ni borrar permisos.
- Es fará servir el sistema de: Tot prohibit, excepte el que es permeti.

| Rols | Veure Cançons | Gestionar Cançons | Veure Playlists | Gestionar Playlists | Veure Usuaris | Gestionar Usuaris |
| -------- | :------: | :------: | :------: | :------: | :------: | :------: |
| *Listener* | SI | NO | SI | SI | NO | NO |
| *Artist* | SI | SI | SI | SI | NO | NO |
| *Admin* | SI | SI | SI | SI | SI | SI |