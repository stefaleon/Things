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


