## Create ASP .NET CORE with terminal:
1.  cd to the folder
2.  open folder -> open cmd/terminal
3.  dotnet new sln -n <project-name>.sln
4.  dotnet new webapi -o <project-name>.Api
5.  dotnet add . -> / -> tab -> / -> tab until <project-name>.Api.csproj
6.  git init
7.  dotnet new gitignore
8.  GitHub -> new repo -> copy url
9.  git remote add origin https://github.com/user/project-name.git
10. echo <#project-name> >> README.md
11. git add .
12. git commit -m "initilize"
13. git branch -M master
14. git push -u origin master
15. start <name>.sln


```
cd root/branch-folder/project-folder
start .
dotnet new sln -n project-name.sln
dotnet new webapi -o project-name.Api
dotnet add ./your-project-name.Api/project-name.Api.csproj
git init
dotnet new gitignore
git remote add origin https://github.com/someone/project-name.git
echo #project-name >> README.md
git add .
git commit -m "initialize"
git branch -M master
git push -u origin master
start your-project-name.sln
```


## Create related projects: 
> find class library (by searching lib + .net core), choose one with c# and targeting .net core in description
1. <project-name>.Core
2. <project-name>.Contracts
3. <project-name>.Repository

## Refer projects: 
> right-click project -> add -> project refernce
1. <project-name>.Api : tick .Repository
2. <project-name>.Repository : tick .Contracts
3. <project-name>.Contracts : tick .Core
4. <project-name>.Core : tick nothing

## Install NuGet: 
> right-click project -> Manage NuGet Packages...
1. .Api:
- AutoMapper
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
2. .Core:
- Microsoft.AspNetCore.Identity.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.Extensions.Identity.Stores


## Create folders: 
> right-click project -> Add -> New Folder
1. .Core : Database, Entities
2. .Api: Models, DataObjects, Settings, Controller (if not available)
3. .Repository: Extensions

## Create table:
* Create new PUBLIC classes in .Core.Entities folder (remember to add the PUBLIC access modifier)
* Each of created classes is an entity and is a (main) table
* Class's properties are table's attributes
* Property : public <data-type> <attribute-name> { get; set; } - get = getter() or getValue(), set = setter() or setValue()
* Daster way : enter "prop" -> tab
* If use IdentityUser -> extends IdentityUser -> no need to add attributes as it uses IdentityUser's attributes
* Use 'System.ComponentModel.DataAnnotations;' to add database notations ([Key], [Required]...)

## Relationship:
### 1. 1-1
- Add foreign key to one entity, make reference navigation property at both
- No need to add constraint in DbContext
```c#
public class FirstEntity{
	[Key]
	public int Id {get; set;}

	//some props...

	public SecondEntity? SecondEntity {get; set;} = null;	//Reference navigation property

	public int? SecondEnity_Id {get; set;}			//Foreign key
}

//and the 2nd one

public class SecondEntity{
	[Key]
	public int Id {get; set;}

	//some props...

	public FirstEntity? FirstEntity {get; set;} = null;	//Reference navigation property

								//No foreign key
}

```


### 2. 1-m
- Defining the relationship fully at both ends with the foreign key property in the dependent entity creates a one-to-many relationship
- Add constraint to the m-ralation entity
```c#
//1 student belongs to 1 department only ==> student is m-relation
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }

    public int? DepartmentId { get; set; }
    public Department? Department { get; set; }
}

//1 department has many students ==> department is 1-relation
public class Department
{
    public int GradeId { get; set; }
    public string GradeName { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new HashSet<Student>();
}

```

> constraints

```c#
builder.Entity<Student>(entity =>
            {
                entity.HasOne(s => s.Department)
                    .WithMany(d => d!.Students)
                    .HasForeignKey(s => s!.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

//or in general

builder.Entity<m-relation-entity>(entity =>
            {
                entity.HasOne(s => s.1-relation-entity)
                    .WithMany(d => d!.m-relation-entity)
                    .HasForeignKey(s => s!.1-relation-entity_Id)
                    .OnDelete(DeleteBehavior.Restrict);
            });

```


### 3. m-m
#### 2 solutions
- 1st: If intermediate table doesn't have any additional attributes (it just indicates the relationship between 2 entities), add reference navigation property at both ends
- No need to add constraints in this case
```c#
public class App : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string LogoURL { get; set; } = string.Empty;

        public ICollection<Dev>? Devs { get; set; } = new HashSet<Dev>();
    }

public class Dev : User
    {
        public ICollection<App>? Apps { get; set; } = new HashSet<App>();
    }

```

- 2nd: If intermediate table has some additional attributes, create intermediate entity which has reference navigation property of both 2 m-m entities and 2 foreign keys
- The intermediate entity has 1-m relationshop with 2 m-m entities ==> add constraints as normal 1-m relation ship, then add composite key constraint
```c#
public class AppDev
    {
        public bool IsLeader { get; set; } = false;	//additional attribute

        public Dev Dev { get; set; }			//first entity
        public string DevId { get; set; }		//first  entity's primary key ==> 1st foreign key

        public App App { get; set; }			//second entity
        public int AppId { get; set; }			//second entity's primary key ==> 2nd foreign key
    }

```

> constraints

```c#
modelBuilder.Entity<AppDev>(entity => {
                entity.HasOne<Dev>(ad => ad.Dev).WithMany(dv => dv.AppDevs).HasForeignKey(ad => ad.DevId);	//1-m with first entity
                entity.HasOne<App>(ad => ad.App).WithMany(ap => ap.AppDevs).HasForeignKey(ad => ad.AppId);	//1-m with second entity
                entity.HasKey(ad => new { ad.AppId, ad.DevId });						//composite key
            });
```

## Create DbContext (AppDbContext/<name>DbConext/<name>Context)
> DbContext is an important class in Entity Framework API.
> It is a bridge between your domain or entity classes and the database.
> DbContext is the primary class that is responsible for interacting with the database.
> -> DbContext: Where to register tables and to specify how tables are created, managed in database

1. In .Core.Database folder -> Add -> New Item... -> C# class -> name it -> extends a specific super DbContext class (see DbContext super classes below)
2. add constructor
```c#
public <DbContext name>(DbContextOptions<<AppDbContext>> options) : base(options) { /*nothing here*/ }
```

> now, we are out of the constructor and inside of this DbContext class

3. for each entity (table) : 
```c#
public DbSet<entity-name> <entity name>s { get; set; } //put an 's' after the entity's name
	//For example
	public DbSet<User> Users { get; set; }
```
4. for constraints : 
	- add : 

		```c#
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
	
			//put tables' contraints here
		}
		```

	- for each table, add :	
		```c#
		modelBuilder.Entity<entity-name>(entity => { //put constraints here });
		```

	- for each constraint add : 
		```c#
		entity.<method_1>().<method_2>();
		entity.<method_3>();
		```
* about DbContext super classes: 2 common 
- DbContext
- IndentityDbContext<User, Role, string, IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
* about constraint methods:
	- HasKey: 
		```c#
		x.HasKey(key => new { key.table-key-attribute-name });
		```

## Add database connection string
> Configure connection to database (find connection string on database server -> for C#)
1. Open .Api/appsettings.json
2. Add
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=tcp:database-server-url,port;Initial Catalog=database-name;Persist Security Info=False;User ID=your-username;Password=your-password;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
},

```

## Add JWT config
> Add max 256-bit secret key to program
1. Open .Api/appsettings.json
2. Add
```json
"JwtConfig": {
    "JWT_Secret": "your-secret-key"
},

```
3. In .Api/Settings -> create JwtConfig class
4. Add 1 prop
```c#
public string JWT_Secret { get; set; } = string.Empty;

```

> Make sure that the name of the field is identical with JWT class (class's name = parent field's name, prop's name = child field's name)

## Add migration
1. Open Package Manager console
2. Change default project : .Core
3. Enter : ```add-migration "name-of-migration"```
4. Enter : ```update-database```

## Post - JSON file's props: 
1. Redundant props: OK
2. Lack of props: if attribute is not nullable coz the attribute is null by default ==> not OK
