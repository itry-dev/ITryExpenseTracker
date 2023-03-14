## Migration Api

Use this Api for 
- Run migrations if pending migrations. An optional json model can be sent to seed the database.

A model like this is required:  
```
{
"seedData": (true|false),
"adminName": (string),
"adminUserName": (string),
"adminPassword": (string),
"adminEmail": (string),
"categories: [
  "--(category name here)--": (string)
  ]
}
```
If seedData is set to **false** the json will be ignored.