## Create ASP .NET CORE with terminal:
1. cd to the folder
2. open folder
3. dotnet new sln -n <name>.sln
4. dotnet new webapi -o <name>.Api
5. dotnet add . -> / -> tab -> / -> tab until <name>.Api.csproj
6. git init
7. dotnet new gitignore
8. start <name>.sln

```
cd ./your-folder
start .
dotnet new sln -n your-project-name.sln
dotnet new webapi -o your-project-name.Api
dotnet add ./your-project-name.Api/your-project-name.Api.csproj
git init
dotnet new gitignore
start your-project-name.sln
```

## Create related projects: 
> --find class library (by searching lib + .net core), choose one with c# and targeting .net core in description--
1. <name>.Core
2. <name>.Contracts
3. <name>.Repository

## Refer projects: 
> --right-click project -> add -> project refernce--
1. <name>.Api : tick .Repository
2. <name>.Repository : tick .Contracts
3. <name>.Contracts : tick .Core
4. <name>.Core : tick nothing

## Install NuGet: 
> --right-click solution -> Manage NuGet Packages for solution...--
1. MS.Entity.FrameworkCore
2. MS.Entity.FrameworkCore.Design
3. MS.Entity.FrameworkCore.SqlSv
4. MS.Entity.FrameworkCore.Tools

## Create folders: 
> --right-click -> Add -> New Folder--
1. .Core : Database, Entities

## Create table:
* create new PUBLIC classes in .Core.Entities folder (remember to add the PUBLIC access modifier)
* each of created classes is an entity and is a table
* class's properties are table's attributes
* property : public <data-type> <attribute-name> { get; set; } - get = getter() or getValue(), set = setter() or setValue()
* faster way : enter "prop" -> tab

## Create DbContext (AppDbContext/<name>DbConext)
1. In .Core.Database folder -> Add -> New Item... -> C# class -> name it -> extends a specific DbContext class
2. add 'public <DbContext name>(DbContextOptions< <DbContext name> > options) : base(options) { }'
3. for each entity (table) : public DbSet< <entity name> > <entity name>s { get; set; }
4. for constraints : 
	- add : 
		```protected override void OnModelCreating(ModelBuilder modelBuilder)```
        	```{```
            		```base.OnModelCreating(modelBuilder);```

			```//put tables' contraints here```
		```}```
	- for each table, add :	
		```modelBuilder.Entity< <Entity's name> >(entity => { //put constraints here });```

	- for each constraint add :
		```entity.<method>```
* about DbContext classes: 2 common - DbContext and IndentityDbContext< <IdentityUser's subclass, a.k.a Identity entity/table> >
* about constraint methods:
	+ 

## Add migration
1. Open Package Manager console
2. Change default project : .Core
3. Enter : ```add-migration```
4. Enter : ```update-database```

## Post - JSON file's props: 
1. Redundant props: OK
2. Lack of props: not OK if attribute is not nullable coz the attribute is null by default
