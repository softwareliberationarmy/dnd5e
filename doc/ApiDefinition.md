# API Definition

---
## Rolling Dice
`GET` `/api/roll` - rolls a 1d20 and returns the result

`GET` `/api/roll/2d20+5` - rolls the requested dice and returns the result. Supports multiples of a die (e.g. 2d6) and modifiers. Rolling with advantage or disadvantage is supported (append "/advantage" or "/disadvantage" to your request.

---
## Character Maintenance
`GET` `api/characters` - returns the set of characters you are authorized to see

`GET` `api/characters/{id}` - returns the character with this ID, or 404 NOT FOUND 

`POST` `api/characters` - creates a new character

`PUT` `api/characters/{id}` - updates the character with that ID

`PATCH` `api/characters/xp/{id}` - adds experience points to a character

