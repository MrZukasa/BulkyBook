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
Il metodo index è anche l'action dell'URL quindi sarà poi dentro il Models, mentre `View()` è appunto una View.

Ogni volta che si definisce un Controller si deve usare la reference `using Microsoft.AspNetCore.Mvc;`

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

### Data Annotations
  - Quando si usa `[Key]` prima della dichiarazione di una proprietà nel costruttore si sta usando la Data Annotations
  > Il vantaggio dell'uso dei validator di annotazione dati consiste nel fatto che consentono di eseguire la convalida semplicemente aggiungendo uno o più attributi, ad esempio l'attributo Required o StringLength, a una proprietà di classe.
  
### SQL Server
Per poter avere una base di dati in locale è necessario installarsi 
- [SQL Server](https://www.microsoft.com/it-it/sql-server/sql-server-downloads)
- [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/it-it/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16) se serve, ma pare che con l'estensione di VSCode si possa fare tutto.
- Nel file di progetto denominato `appsettings.json` possiamo aggiungere la stringa di connessione al database
- Per aggiungere il Framework Entity Core è necessario usare `dotnet add package Microsoft.EntityFrameworkCore.SqlServer` ed includerne la reference `using Microsoft.EntityFrameworkCore` dentro il file.cs che si occuperà del database.
- Eredito la classe [DbContext](https://docs.microsoft.com/it-it/dotnet/api/system.data.entity.dbcontext?view=entity-framework-6.2.0) dal Framework EntityFrameworkCore e da li ne creo il costruttore per la tabella `category` dichiarata come models .cs
- Aggiungo al programma principale la reference realtiva al framework.sqlserver ed alla cartella `data` cosi da importare il mio models Category.
- Aggiungo al builder il servizio per creare la connessione ed all'interno configuro la stringa di connessioneal DB: 
  ```c#
  builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
  ))
  ```
- Prima di creare una Migration per aggiungere i dati nel DB è necessario installare un ulteriore pacchetto `dotnet add package Microsoft.EntityFrameworkCore.Tools.DotNet` ed il `dotnet add package Microsoft.EntityFrameworkCore.Design`
- Per aggiungere la Migration vera e propria si farà `dotnet ef migrations add nomemigration`
- Per confermare la Migration e pusharla sul DB si farà `dotnet ef database update`
> N.B. ricordarsi di aver installato globalmente [dotnet ef](https://docs.microsoft.com/it-it/ef/core/cli/dotnet) con `dotnet tool install --global dotnet-ef` altrimenti non va nulla!!!