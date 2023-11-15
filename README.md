This extension pack for Ms Sql Server is a powerfull tool, allowing to build ETL solutions in Sql Server without calling external scripts. 
Every projects is a simple set of .Net functions, adapted for use in Ms Sql Server environment.

All you need to start is 
1. Microsoft .Net Framework 4.6.1 Developer Pack (Get it here https://dotnet.microsoft.com/download/dotnet-framework/net461). You will need msbuild to build projects.
2. Nuget (Get it here https://www.nuget.org/downloads)
3. Add nuget.exe location to System Environment Variable "Path"

Instructions:
1. Choose a project you want to deploy
2. Go to project directory
3. restore packages ```nuget restore "..\..\solutions\bi_dev.sql.mssql.extensions"```
4. Build project ```msbuild```
5. Open deploy.sql and edit ```-- INPUT --``` sction: set correct path to your project in ```@build_location``` and ```@build_file_name``` variables
6. Choose a database you want to deploy functions to and make it trustworthy ```alter database <<your database>> set trustworthy on;``` (I suggest that you should create a separate database for all clr functions)
8. run ```deploy.sql``` 

/***************************************************************************
Добавлены python-скрипты. В корне папка "py_scripts".
Основной запускаемый скрипт: run.py, имеет следующие параметры на входе:
   * --task : метод, который нужно дернуть. Мэтчинг этого параметра и метода находится в функции main
   * --url : адрес rest-метода. Формат строка.
   * --headers : заголовки rest-метода. Формат json-строка, но с одинарными кавычками (!!), чтобы нормально проходила передача через командную строку.
Пример: [{'name':'Authorization','value':'Bearer y0_AgAAAAAJ46TKAAqQ2wAAAADtwq-JDqIuC257Sz6KRiDjLFhGTOsDsVY'}]
   * --file : список файлов, которые будут выгружены в вызов rest-метода. Формат json-строка, но с одинарными кавычками (!!), чтобы нормально проходила передача через командную строку.
Пример: [{'name':'file','value':'C:\\storage\\hdd01\\files\\uploads\\yandex_audience_v2\\October 23 - Magnet iOS RU - install - ifadstech_int - 11982.tsv'}]

Пример вызова: python C:\storage\hdd01\proj\sql\py_scripts\run.py --task "web_post" --url "https://api-audience.yandex.ru/v1/management/segments/upload_file?" --headers "[{'name':'Authorization','value':'Bearer y0_AgAAAAAJ46TKAAqQ2wAAAADtwq-JDqIuC257Sz6KRiDjLFhGTOsDsVY'}]" --file "[{'name':'file','value':'C:\\storage\\hdd01\\files\\uploads\\yandex_audience_v2\\October 23 - Magnet iOS RU - install - ifadstech_int - 11982.tsv'}]"

Пример вызова из хранилища: хранимая процедура ToptrafficBI_INT..p_call_python_script

Можно вызвать через нее и другие методы. Если будут добавляться входные параметры, то просто добавить строки в formatmessage.
***************************************************************************/

