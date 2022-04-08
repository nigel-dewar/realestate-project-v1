# Migrations

Run from the Manage Directory in Terminal

create migration
```
dotnet ef migrations add "init" -p Manage.Repository/ -s Manage.API/
```

run migration
```
dotnet ef database update -p Manage.Repository/ -s Manage.API/
```

