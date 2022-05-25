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
In questo caso l'URL è
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

### Metodo di lavoro
Fondamentalmente ogni progetto è costituito da diversi MVC e la sinergia tra questi componenti a rendere tutto quanto più semplice possibile.

Si parte creando un `Controller` con una funzione di `public IActionResult Index()` che restituisce il valore all'interno della View().

La `View` va anch'essa creata in relazione al controller che la chiama, nello specifico creerò la view realtiva al controller index, quindi la chiamerò Index.cshtml.
Quando richiamo la vista devo tenere conto del routing inerente alle informazioni da inserire nei TagHelpers, nello specifico so che il controller appartiene alla categoria, e l'action al nome del metodo.
```c#
<a class="nav-link" asp-area="" asp-controller="Category" asp-action="Index">Category</a>
```
Per leggere i dati da un DB devo usare un oggetto che deriverà dalla mia classe dbContext istanziata in precedenza, ma non ho bisogno di dichiarare nulla di nuovo, mi è bastato dichiarare il servizio:
```c#
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));
```
e grazie alle Dipendency Injection avviene tutti in automatico.

La classe `ApplicationDbContext` che ho dichiarato, ha un costruttore che gira direttamente nel servizio sopra citato.
```c#
public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
    {        
    }
```
Adesso è necessario richiamare l'applicazione ApplicationDbContext direttamente nel controller facendone assegnare il valore ad una variabile che chiamo `_db`.

Adesso devo far assegnare questi dati ad un costruttore all'interno del controller passandogli come parametro il servizio che si occupa di leggerli con il DB come opzione:
```c#
public CategoryController(ApplicationDbContext db)
    {
        _db = db;
    }
```
Dentro _db adesso ce quindi il contenuto del Database.
Adesso i dati vanno portati nella View e per farlo è necessario passarli all'interno della Index Action Method che restituisce i dati dentro una view che ho creato prima.
In un passaggio posso recuperare i dati come lista e assegnarli ad un interfaccia, passo tutto nel metodo e restituisco alla view vera a propria.
```c#
  IEnumerable<Category> objCategoryList = _db.Categories;
  return View(objCategoryList);
```
Adesso nella View catturo il risultato del model con `@model IEnumerable<Category>` ed essendo un interfaccia Enumerable la posso scorrere con un foreach direttamente in mezzo ad i tag html all'interno della vista facendo:
```c#
  @foreach(var obj in Model)
  {
    @obj.Name
    @obj.DisplayOrder
  }
```
`Model` va in maiuscolo perché è l'oggetto model che viene passato alla view, non è un nome specifico.

A questo punto creo un bottone per aggiungere dati al mio DB, lo linko con i TagHelpers al controller corretto ad all'action nuova che creo dentro il controller. Il controller sarà `CategoryController.cs` e l'action `public IActionResult Create()`.
Chiaramente va creata anche la View per questo model, all'interno della view lascio solo `@model Category` cosi che quando dal mio form nella view premo submit, venga passato al model direttamente il contenuto del form. Non specificando nulla questo è possibile.

Adesso nel controller devo creare un POST method per poter scrivere effettivamente i dati dentro il DB:
```c#
[HttpPost]
public IActionResult Create(Category obj) 
{
  _db.Categories.Add(obj);
  _db.SaveChanges();
  return RedirectToAction("Index");
}
```
>N.B.: obj è un Model
Quello che succede qui è: creo il metodo e raccolgo quello che il form restiuisce e che la view mi passa come Category e lo assegno ad obj, aggiungo l'obj alla tabella Categories del DB e poi applico le modifiche al DB vero e proprio.

### Validazione dei dati
Ci sono due tipi di validazione, server side quindi nel controller e client side quindi nella view.
- *Server side*: controllo il model nel controller quindi per esempio `if (ModelState.IsValid)`.
  Quando si usa questo tipo di validazione ci si può appoggiare a bootstrap per ripotare a video il messaggio di errore default contenuto nel metodo IsValid, ad esempio usando il TagHelper `asp-validation-for` in uno `<span>` legato ad un campo che abbiamo richiesto come ***required***.

  Esiste la possibilità di avere un altro TagHelper `asp-validation-summary-all` che riporta tutti gli errori presenti in un recap.
  E' possibile avere dei messaggi di errore custom aggiungendoli al ModelState facendo `ModelState.AddModelError(key,value)` dove value è il messaggio che voglio e key è una stringa custom o il nome dell'oggetto al quale voglio assegnare l'errore.

- *Client side*: Fare validazione su una View è possibile con l'utilizzo di una partial view (view che cominciano con _) per importarne una dentro la mia view devo fare:
```c#
@section Scripts{
  @{
    <partial name="_ValidationScriptPartial"/>
  }
}
```
Questa partial view contiene degli script jquery che si occupano della validazione direttamente nella view.
> N.B. per cambiare la nomenclatura di un campo si può assegnare un nuovo valore di Display dentro al Model sotto la definizione della proprietà
> ad esempio [DisplayName("Display Order")].

Un'altro modo per avere messaggi d'errore custom è usare dei Data Annotations (sempre dentro il Model) ad esempio `[Range(1,100, ErrorMessage = "Messaggio d'errore!")]`, cosi facendo si controlla il range di un espressione e si riporta il messaggio di errore eventuale.