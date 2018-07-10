## Field Services Manager
Manage field services, schedule and complete jobs and more


## 00 Scaffold

* Create an ASP.NET Core Web Application, use the MVC scaffold.


## 01 Layout

* Edit the main layout.

`Views/Shared/_Layout.cshtml`

```
<a asp-area="" asp-controller="Home" asp-action="Index" class="navbar-brand">MVC Core App</a>
```

```
<li><a asp-area="" asp-controller="Home" asp-action="Things">Things</a></li>
```

* Add the Things  action stub in Controllers/HomeController.


* Add the Things cshtml stub in Views/Home.



## 02 Models



* Add the main model classes.

`Models/Thing.cs`

```
namespace MVCCoreApp.Models
{
    public class Thing
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
```



## 03 DbContext

* Add a class for DbContext.

`Models/ApplicationDbContext.cs`

```
using Microsoft.EntityFrameworkCore;

namespace MVCCoreApp.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Thing> Things { get; set; }        
    }
}

```

* Add a class for the repository props and methods.

`Models/Repository.cs`

```
using System.Linq;

namespace MVCCoreApp.Models
{
    public class Repository
    {
        private ApplicationDbContext context;

        // injecting the context dependency to the repository constructor
        public Repository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        // mapping the context queryables
        public IQueryable<Thing> Things => context.Things;


        // save functionality for things (add or edit)
        public void SaveThing(Thing thing)
        {
            if (thing.Id == 0)
            {
                context.Things.Add(thing);
            }
            else
            {
                Thing dbEntry = context.Things.FirstOrDefault(t => t.Id == thing.Id);
                if (dbEntry != null)
                {
                    dbEntry.Name = thing.Name;
                }
            }
            context.SaveChanges();
        }

        // delete things
        public Thing DeleteThing(int thingId)
        {
            Thing dbEntry = context.Things.FirstOrDefault(t => t.Id == thingId);
            if (dbEntry != null)
            {
                context.Things.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

    }
}
```

## 04 Connect to MSSQLLocalDB


* Configure a connection string.

`appsettings.json`

```
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=Things;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

* Configure the services for the database context and the repository.

`Startup.cs`

```
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
services.AddTransient<Repository>();
```


## 05 Migrate on startup

* Add an initial migration.

* Add code to the Configure method so that the migration is run on application start.

`Startup.cs`

```
// auto create the db on app start           
using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}
```




## 06 Things

* Overload the HomeController constructor.

`Controllers/HomeController.cs`

```
private Repository repo;

public HomeController(Repository rRepo)
{
    repo = rRepo;
}
```

* Edit the Things action.

`Controllers/HomeController.cs`

```
public ViewResult Things() => View(repo.Things.OrderBy(c => c.Name));
```

* Edit the Things view.

`Views/Home/Things.cshtml`

```
@model IEnumerable<Thing>


<div class="text-center" style="padding: 10px">
    <a asp-action="AddThing" class="btn btn-primary">Add a thing</a>
</div>


<table class="table table-striped table-bordered table-sm">
    <tr>
        <th>Name</th>        
    </tr>
    @foreach (var thing in Model)
    {
    <tr>
        <td>@thing.Name</td>        
        <td class="text-center">
            <form asp-action="DeleteThing" method="post">
                <input type="hidden" name="Id" value="@thing.Id" />
                <button type="submit" class="btn btn-danger btn-sm">
                    Delete
                </button>
            </form>
        </td>
    </tr>
    }
</table>
```


* Create the stubs for the AddThing and DeleteThing methods.

`Controllers/HomeController.cs`

```
public IActionResult AddThing()
{
    return View();
}
```

```
 public IActionResult DeleteThing()
{
    return View();
}
```


* Create the stubs for the AddThing and DeleteThing views.



## 07 Add Things

* Create a view that holds the post form.

`Views/Home/AddThing.cshtml`

```
@model Thing

<h4>Thing's Data</h4>

<form asp-action="AddThing" method="post">

    <div class="form-group">
        <label asp-for="Name">Name</label>
        <input asp-for="Name" class="form-control" required />
    </div>

    <div class="text-center">
        <button class="btn btn-primary" type="submit">Submit</button>
        <a asp-action="Things" class="btn btn-default">Cancel</a>
    </div>
</form>
```

* Add the HTTP POST action.

`Controllers/HomeController.cs`

```
public IActionResult AddThing()
{
    return View();
}

[HttpPost]
public IActionResult AddThing(Thing thing)
{

    if (ModelState.IsValid)
    {
        var thingExists = repo.Things.Any(t => t.Name == thing.Name);
        if (!thingExists)
        {
            repo.SaveThing(thing);
            return RedirectToAction("Things");
        }

        ViewData["Message"] = "A thing with this name already exists.";

        return View("CustomError");
    }
    return View();
}
```

`Views/Shared/CustomError.cshtml`

```
<h4 class="text-danger">@ViewData["Message"]</h4>
```
