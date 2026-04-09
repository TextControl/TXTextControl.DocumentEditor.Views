# Create a Document Editor Application

## When to use this task

Use this task when the user wants to create a new ASP.NET Core MVC application that hosts the
TX Text Control .NET Server for ASP.NET Document Editor using the Text Control Private NuGet Feed.

## Prerequisites

- Visual Studio 2026
- .NET 10 SDK
- Access to the Text Control Private NuGet Feed
- A valid TX Text Control .NET Server for ASP.NET license assigned to the developer email

## Steps

### 1. Create the application

In Visual Studio 2026, create a new project:

1. Choose **Create a new project**
2. Select **ASP.NET Core Web App (Model-View-Controller)**
3. Enter a project name and location
4. Choose **.NET 10.0 (Long Term Support)**

### 2. Add the NuGet packages

Set the package source to the private NuGet feed (**TextControl**) and install the following packages:

- `TXTextControl.Web`
- `TXTextControl.TextControl.Core.SDK`
- `TXTextControl.Web.DocumentEditor.Backend`

### 3. Configure Program.cs

Open `Program.cs` and add these namespaces at the top:

```csharp
using TXTextControl.Web;
using TXTextControl.Web.DocumentEditor.Backend;
```

After this line:

```csharp
builder.Services.AddControllersWithViews();
```

add this line:

```csharp
builder.Services.AddHostedService<DocumentEditorWorkerManager>();
```

Add this code before `app.UseRouting();`:

```csharp
// enable Web Sockets
app.UseWebSockets();

// attach the Text Control WebSocketHandler middleware
app.UseTXWebSocketMiddleware();
```

The overall `Program.cs` file should look like this:

```csharp
using TXTextControl.Web;
using TXTextControl.Web.DocumentEditor.Backend;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHostedService<DocumentEditorWorkerManager>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseWebSockets();
app.UseTXWebSocketMiddleware();

app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
```

### 4. Add the editor to the MVC view

Find `Views/Home/Index.cshtml` and replace the complete content with this code:

```cshtml
@using TXTextControl.Web.MVC

@{
    var sDocument = "<html><body><p>Welcome to <strong>Text Control</strong></p></body></html>";
}
@Html.TXTextControl().TextControl(settings => {
    settings.UserNames = new string[] { "Tim Typer" };
}).LoadText(sDocument, TXTextControl.Web.StringStreamType.HTMLFormat).Render()

<input type="button" onclick="insertTable()" value="Insert Table" />
<script>
    function insertTable() {
        TXTextControl.tables.add(5, 5, 10, function(e) {
          if (e === true) { // if added
            TXTextControl.tables.getItem(function(table) {
              table.cells.forEach(function(cell) {

                cell.setText("Cell text");

              });
            }, null, 10);
          }
        })
    }
</script>
```

### 5. Attach editor events

Attach editor lifecycle events with `TXTextControl.addEventListener(eventName, callback)`.

For the baseline setup, the most important lifecycle event is `textControlLoaded`.
Its callback signature is `function()` and it does not return an event args object.

```html
<script>
  TXTextControl.addEventListener("textControlLoaded", function() {
    console.log("TX Text Control is ready.");
  });
</script>
```

Return object for `textControlLoaded`:

```javascript
undefined
```

### 6. Start the application

Start the application by pressing **F5** or choosing **Debug -> Start Debugging**.

## Output

A new ASP.NET Core MVC application with:

- the TX Text Control packages installed
- `DocumentEditorWorkerManager` registered
- WebSockets enabled
- `UseTXWebSocketMiddleware()` configured
- the document editor rendered in `Views/Home/Index.cshtml`

## Notes

- Use the Text Control private NuGet feed named `TextControl` - it might be added with another name in your Visual Studio, but it should point to `https://www.textcontrol.com/nuget`.
- Do not invent feed credentials, tokens, or license values
- Use only the package names and code shown in this task
- Event-specific return objects are documented in the feature task files for comments, form fields, and tracked changes.
