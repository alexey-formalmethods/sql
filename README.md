This extension pack for Ms Sql Server is a powerfull tool, allowing to build ETL solutions in Sql Server without calling external scripts. 
Every projects is a simple set of .Net functions, adapted for use in Ms Sql Server environment.

All you need to start is 
1. Microsoft .Net Framework 4.6.1 Developer Pack (Get it here https://dotnet.microsoft.com/download/dotnet-framework/net461). You will need msbuild to build projects.
2. Nuget (Get it here https://www.nuget.org/downloads)
3. Add nuget.exe System Environment Variable "Path"

Instructions:
1. Choose a project you want to deploy
2. Go to project directory
3. restore packages ```nuget restore "..\..\solutions\bi_dev.sql.mssql.extensions"```
4. Build project ```msbuild```
5. Open deploy.sql and edit ```-- INPUT --``` sction: set correct path to your project in ```@build_location``` and ```@build_file_name``` variables
6. Choose a database you want to deploy functions to and make it trustworthy ```alter database <<your database>> set trustworthy on;``` (I suggest that you should create a separate database for all clr functions)
8. run ```deploy.sql``` 


