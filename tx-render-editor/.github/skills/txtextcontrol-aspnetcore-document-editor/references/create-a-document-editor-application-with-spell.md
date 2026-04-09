# Create a Document Editor Application with TX Spell .NET

## When to use this task

Use this task when the user wants to add TX Spell .NET spell checking to an existing
ASP.NET Core MVC application that already hosts the TX Text Control .NET Server for ASP.NET
Document Editor.

Use `create-a-document-editor-application.md` first when creating a brand new application.

## Prerequisites

- Existing baseline project from `create-a-document-editor-application.md`
- Access to the Text Control Private NuGet Feed

## Steps

### 1. Add the TX Spell NuGet package

Set the package source to the private NuGet feed (**TextControl**) and install:

- `TXTextControl.TXSpell.Core.SDK`

Install this package from the same Text Control private NuGet feed used by the baseline packages.

### 2. Update the editor view to enable spell checking

Find `Views/Home/Index.cshtml` in the baseline application and add this JavaScript to enable spell checking:

```cshtml
@using TXTextControl.Web.MVC

<script>
    TXTextControl.addEventListener("textControlLoaded", function() {
            TXTextControl.selectAll();
            TXTextControl.selection.setCulture("en-US");
            TXTextControl.selection.setLength(0);

            TXTextControl.isSpellCheckingEnabled = true;
    });
</script>
```

### 3. Event information

Use `TXTextControl.addEventListener("textControlLoaded", ...)` so spell checking is enabled only after the editor is ready.

Callback signature:

```javascript
function()
```

Return object for `textControlLoaded`:

```javascript
undefined
```

### 4. Start the application

Start the application by pressing **F5** or choosing **Debug -> Start Debugging**.

## Output

A baseline ASP.NET Core MVC document editor application with:

- TX Spell package installed from the same private feed
- spell checking enabled in the document editor rendered in `Views/Home/Index.cshtml`

## Notes

- Use the Text Control private NuGet feed - it might be added with another name in your Visual Studio, but it should point to `https://www.textcontrol.com/nuget`.
- The TX Spell package `TXTextControl.TXSpell.Core.SDK` must be installed from this same feed.
- Do not invent feed credentials, tokens, or license values
- Use only the package names and code shown in this task
- `textControlLoaded` does not pass an event args object.
