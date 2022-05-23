# ASP.NET MVC 
- progetto creato con Visual Studio 2022
- editato con VSCode via `dotnet watch run`
- [Link](https://www.youtube.com/watch?v=hZ1DASYd9rk)

### Routing

Copio qui le informazioni che mi sembrano più utili
```c#
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
```
In questo caso l'ULR è
- https://localhost:5555/{controller}/{action}/{id}
- ID è sempre opzionale

### Models 💃🏻
I modelli sono da pensare un po' come le tabelle di un DB

### Controllers 🛂
Nei controller è possibile definire le dependencies e i metodi come ad esempio:
```c#
public IActionResult Index(){
    return View();
}
```
Il metodo index è anche l'action dell'URL quindi sarà poi dentro il Models, mentre `View()` è appunto una View

### Views 🌇
la cartella shared contiente tutte le Partial Views che vengono usate dal progetto.
In ***_ViewImports*** è richiamata una libreria che si chiama `@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers` e seve ad includere nel progetto dei metodi stile react/angular come ad esempio `asp-controller="Home" asp-action="Privacy"` che servono a passare nell'URL controller e action.

### TagHelpers 🥰
- Abilitano codice lato server per creare e renderizare elementi HTML in Razor files.
- Razor:
  > Razor è una sintassi di markup per l'incorporamento di codice basato su .NET nelle pagine Web. La Razor sintassi è costituita Razor da markup, C# e HTML. I file contenenti Razor in genere hanno un'estensione .cshtml di file.
- Esempio di TagHelpers: 
  - HTML Helper:
    ```c#
    @using (Html.BeginForm("Index","Users", FormMethods.Post, new { @class = "form-horizontal" }))
    {}
    ```
  - TAG Helper:
    ```c#
    <form class="form-horizontal" method="post" asp-controller="Users" asp-action="Index"></form>
    <form class="form-horizontal" method="post" asp-page="Users/Index"></form>
    ```