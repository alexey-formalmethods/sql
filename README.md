This extension pack for Ms Sql Server is a powerfull tool, allowing to build ETL solutions in Sql Server without calling external scripts. 
Every projects is a simple set of .Net functions, adapted for use in Ms Sql Server environment.

All you need to start is 
1. Microsoft .Net Framework 4.6.1 Developer Pack (Get it here https://dotnet.microsoft.com/download/dotnet-framework/net461). You will need msbuild to build projects.
2. Nuget (Get it here https://www.nuget.org/downloads)
3. Add nuget.exe System Environment Variable "Path"

Instructions:
Choose a project you want to deploy
Go to project directory
restore packages nuget restore "..\..\solutions\bi_dev.sql.mssql.extensions"
Build project msbuild
Open deploy.sql and edit -- INPUT -- sction: set correct path to your project in @build_location and @build_file_name variables
run deploy.sql


