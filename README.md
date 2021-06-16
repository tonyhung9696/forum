# forum

1. setup a new mssql connection on appsetting.json

2.  dotnet ef migrations add init
    dotnet ef database update
    
    (using this two command to setup the database, therefore, that will create two account)
    
    username:admin,
    password:password
    
    and
    
    username:vistor,
    password:password
    
 3. dotnet run
