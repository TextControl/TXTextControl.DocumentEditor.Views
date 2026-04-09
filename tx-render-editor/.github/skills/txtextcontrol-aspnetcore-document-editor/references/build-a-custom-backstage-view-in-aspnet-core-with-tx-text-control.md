# Build a Custom Backstage View in ASP.NET Core with TX Text Control

## When to use this task

Use this task when the user wants to add a custom File-style backstage menu to an ASP.NET Core MVC application
that already hosts the TX Text Control Document Editor.

Use `create-a-document-editor-application.md` first when creating a brand new application.

## Prerequisites

- Existing baseline project from `create-a-document-editor-application.md`

## Reference Sources

- Tutorial: https://www.textcontrol.com/blog/2026/02/17/build-a-custom-backstage-view-in-aspnet-core-with-tx-text-control/
- Sample repository: https://github.com/TextControl/TXTextControl.Web.Backstage

## Rule

Use the exact code shown below for the backstage implementation. Do not invent additional controller actions,
JavaScript behavior, HTML structure, or CSS.

## Event information

The backstage sample waits until the ribbon UI is ready before it injects the custom File tab and backstage view.

Attach that lifecycle event with `TXTextControl.addEventListener("ribbonTabsLoaded", callback)`.
The documented callback signature is `function()` for this event.

```javascript
TXTextControl.addEventListener("ribbonTabsLoaded", function() {
    initBackstageMenu();
    initFileTab();
});
```

Return object for `ribbonTabsLoaded` in this sample:

```javascript
undefined
```

## Required Files and Code

### 1. Create the folder structure

Create these folders if they do not already exist:

- `Models`
- `Views/Home/Stageviews`
- `wwwroot/js/backstage`
- `wwwroot/css`
- `App_Data/documents`

### 2. Add `Models/Document.cs`

Create `Models/Document.cs` with this exact content:

```csharp
namespace TXTextControl.BackstageView.Models
{
    /// <summary>
    /// Document metadata for backstage document list
    /// </summary>
    public class Document
    {
        public string FileName { get; set; }
        public string FullPath { get; set; }
        public DateTime LastModified { get; set; }
        public long FileSize { get; set; }

        public Document(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            FileName = fileInfo.Name;
            FullPath = filePath;
            LastModified = fileInfo.LastWriteTime;
            FileSize = fileInfo.Length;
        }

        /// <summary>
        /// Format file size into human-readable format
        /// </summary>
        public string GetFormattedSize()
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = FileSize;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }
    }
}
```

### 3. Replace `Controllers/HomeController.cs`

Replace `Controllers/HomeController.cs` with this exact content:

```csharp
using Microsoft.AspNetCore.Mvc;
using TXTextControl;
using TXTextControl.BackstageView.Models;

namespace TXTextControl.BackstageView.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public HomeController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Fetch all documents from App_Data/documents folder
        /// and return as a partial view for the backstage
        /// </summary>
        public ActionResult GetDocuments()
        {
            var documents = new List<Document>();
            string documentsPath = Path.Combine(_environment.ContentRootPath, "App_Data", "documents");

            if (Directory.Exists(documentsPath))
                {
                foreach (string file in Directory.GetFiles(documentsPath))
                    {
                         Document doc = new Document(file);
                         documents.Add(doc);
                    }
                }

            return PartialView("Stageviews/Open", documents);
        }

        /// <summary>
        /// Load a document and convert it to TX Text Control's native format
        /// </summary>
        [HttpPost]
        public ActionResult LoadTemplate([FromBody] DocumentRequest request)
        {
            try
            {
                string filePath = Path.Combine(_environment.ContentRootPath, "App_Data", "documents", request.FileName);

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound(new { error = "File not found" });
                }

                using (ServerTextControl tx = new ServerTextControl())
                {
                    tx.Create();

                    // Detect document format from file extension
                    string extension = Path.GetExtension(filePath).ToLower();
                    StreamType streamType = extension switch
                    {
                        ".docx" => StreamType.WordprocessingML,
                        ".doc" => StreamType.MSWord,
                        ".rtf" => StreamType.RichTextFormat,
                        ".txt" => StreamType.PlainText,
                        ".html" => StreamType.HTMLFormat,
                        ".pdf" => StreamType.AdobePDF,
                        ".tx" => StreamType.InternalUnicodeFormat,
                        _ => StreamType.WordprocessingML // Default fallback
                    };

                    // Load and convert to InternalUnicodeFormat
                    tx.Load(filePath, streamType);
                    byte[] data;
                    tx.Save(out data, BinaryStreamType.InternalUnicodeFormat);
                    return Ok(new { data = Convert.ToBase64String(data) });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Request model for document loading
        /// </summary>
        public class DocumentRequest
        {
            public string FileName { get; set; } = string.Empty;
        }
    }
}
```

### 4. Add `Views/Home/Stageviews/Open.cshtml`

Create `Views/Home/Stageviews/Open.cshtml` with this exact content:

```cshtml
@* Partial view for displaying document list in backstage *@
@model List<TXTextControl.BackstageView.Models.Document>

@if (Model != null && Model.Any())
{
    <ul class="document-list">
        @foreach (var doc in Model)
        {
            <li class="document-item">
                <div class="document-info">
                    <div class="document-name">@doc.FileName</div>
                    <div class="document-meta">
                        Modified: @doc.LastModified.ToString("MMM dd, yyyy hh:mm tt") | Size: @doc.GetFormattedSize()
                    </div>
                </div>
                <button class="load-button" id="load-button" data-filename="@doc.FileName">Open</button>
            </li>
        }
    </ul>
}
else
{
    <p>No documents found. Add files to App_Data/documents folder.</p>
}
```

### 5. Replace `Views/Home/Index.cshtml`

Replace `Views/Home/Index.cshtml` with this exact content:

```cshtml
@* Main view with TX Text Control editor and backstage templates *@
@using TXTextControl.Web.MVC
@{
    ViewData["Title"] = "TX Text Control - Backstage View";
}
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"]</title>
    <link rel="stylesheet" href="~/css/backstage.css" />
</head>
<body>
    @{
        var textControlHtml = Html.TXTextControl().TextControl(settings =>
        {
            settings.UserNames = new string[] { "User" };
        });
    }

    <div id="editorWrapper" style="position: relative; width: 100%; height: 100vh;">
        @textControlHtml.Render()

        <!-- File Tab Template -->
        <template id="file-tab-template">
            <li id="li_tabFile" style="opacity: 1; pointer-events: initial;">
                <a id="tabFile" data-text="File" tabindex="-1">
                    <div class="txTabContainer">File</div>
                </a>
            </li>
        </template>

        <!-- Backstage View Template -->
        <template id="backstage-template">
            <aside id="backstage" class="backstage">
                <div class="backstage__menu">
                    <a id="back-link" class="backstage__menu__back-link">← Back</a>
                    <ul id="menu" class="backstage__menu__items">
                        <li class="backstage__menu__item"><a id="new-document">📄 New</a></li>
                        <li class="backstage__menu__item"><a id="open-document">📂 Open</a></li>
                    </ul>
                </div>
                <div class="backstage-right">
                    <div id="title" class="backstage-right__title">
                        <h1>Open</h1>
                    </div>
                    <div id="stage" class="backstage-right__stage"></div>
                </div>
            </aside>
        </template>
    </div>

    <!-- Custom Backstage Script -->
    <script src="~/js/backstage/index.js" type="module"></script>
</body>
</html>
```

### 6. Add `wwwroot/js/backstage/index.js`

Create `wwwroot/js/backstage/index.js` with this exact content:

```javascript
// index.js - Application entry point for backstage initialization

import { initBackstageMenu } from "./backstageMenu.js";
import { initFileTab } from "./fileTab.js";


// Wait for TX Text Control to fully load before initializing backstage
document.addEventListener('DOMContentLoaded', function () {
    TXTextControl.addEventListener("ribbonTabsLoaded", function () {
        initBackstageMenu();
        initFileTab();
    });
});
```

### 7. Add `wwwroot/js/backstage/fileTab.js`

Create `wwwroot/js/backstage/fileTab.js` with this exact content:

```javascript
// fileTab.js - Custom File tab creation for TX Text Control ribbon
import { openBackstage } from "./backstageMenu.js";

/**
 * Initialize and inject the custom File tab into TX Text Control's ribbon
 */
export function initFileTab() {
    const ribbonBar = document.getElementById('ribbonbar');
    const tabsContainer = ribbonBar.querySelector('.tabs, [class*="tabs"]');
    // Clone the File tab template
    const template = document.getElementById('file-tab-template');
    const fileTab = template.content.cloneNode(true);

    // Attach click handler to open backstage
    const fileTabLink = fileTab.querySelector('#tabFile');
    fileTabLink.addEventListener('click', function (e) {
        e.preventDefault();
        openBackstage();
    });

    // Insert File tab at the beginning of the ribbon
    tabsContainer.insertBefore(fileTab, tabsContainer.firstChild);
}
```

### 8. Add `wwwroot/js/backstage/backstageMenu.js`

Create `wwwroot/js/backstage/backstageMenu.js` with this exact content:

```javascript
// backstageMenu.js - Core backstage functionality and server communication

/**
 * Initialize the backstage menu by cloning the HTML template 
 * and setting up event handlers
 */
export function initBackstageMenu() {

    // Locate TX Text Control container and inject backstage template
    const ribbonBar = document.getElementById('ribbonbar');
    const txContainer = ribbonBar.parentElement;

    const template = document.getElementById('backstage-template');
    const clone = template.content.cloneNode(true);
    txContainer.appendChild(clone);

    // Handle "New Document" action
    const newDocTrigger = document.getElementById("new-document");
    if (newDocTrigger) {
        newDocTrigger.addEventListener("click", function (e) {
            TXTextControl.resetContents();
            closeBackstage();
        });
    }

    // Handle "Back" button
    const button = document.getElementById("back-link");
    if (button) {
        button.addEventListener("click", function (e) {
            e.preventDefault();
            closeBackstage();
        });
    }

    // Event delegation for dynamically loaded document buttons
    document.addEventListener("click", function (e) {
        const loadButton = e.target.closest('#load-button');

        if (loadButton) {
            const fileName = loadButton.dataset.filename;
            LoadFromController(fileName, loadButton);
        }
    });

    // Close backstage with Escape key
    document.addEventListener("keydown", function (e) {
        const backstage = document.getElementById("backstage");
        if (e.key === "Escape" && backstage && backstage.style.opacity === "1") {
            closeBackstage();
        }
    });
}

/* Backstage Controls */
export function openBackstage() {
    const backstage = document.getElementById("backstage");
    if (backstage) {
        backstage.classList.add('active');
        loadDocuments();
    } else {
        console.error("BACKSTAGE NOT FOUND!");
    }
}

function closeBackstage() {
    const backstage = document.getElementById("backstage");
    if (backstage) {
        backstage.classList.remove('active');
    }
}

/* Server Communication */

/**
 * Fetch and display the document list from the server
 */
function loadDocuments() {
    fetch("/Home/GetDocuments", { method: "GET" })
        .then(response => response.text())
        .then(html => {
            const stageElement = document.getElementById('stage');
            if (stageElement) {
                stageElement.innerHTML = html;
            }
        })
        .catch(error => {
            console.error("Error loading view:", error);
        });
}

/**
 * Load selected document into TX Text Control
 */
export function LoadFromController(fileName, loadButton) {
    // Show loading state
    const originalText = loadButton.textContent;
    loadButton.textContent = "Loading...";
    loadButton.disabled = true;

    fetch("/Home/LoadTemplate", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ FileName: fileName })
    })
        .then(res => res.json())
        .then(data => {
            if (data?.data) {
                // Load document in TX Text Control's native format
                TXTextControl.loadDocument(
                    TXTextControl.streamType.InternalUnicodeFormat,
                    data.data
                );
                closeBackstage();
            }
        })
        .catch(err => {
            alert(err.message);
            // Restore button state on error
            loadButton.textContent = originalText;
            loadButton.disabled = false;
        });
}
```

### 9. Add `wwwroot/css/backstage.css`

Create `wwwroot/css/backstage.css` with this exact content:

```css
/* Reset and Base */
* {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
}

:root {
    /* Base Colors */
    --color-primary: #2b579a;
    --color-primary-dark: #1e4276;
    --color-white: #ffffff;
    --color-gray-50: #f5f5f5;
    --color-gray-200: #e0e0e0;
    --color-gray-400: #999999;
    --color-gray-500: #888888;
    --color-gray-600: #666666;
    --color-gray-700: #555555;
    --color-gray-900: #333333;

    /* Overlays */
    --overlay-light: rgba(255, 255, 255, 0.1);
    --overlay-active: rgba(255, 255, 255, 0.2);

    /* Shadows */
    --shadow-sm: 0 2px 6px rgba(0, 0, 0, 0.08);
    --shadow-md: 0 4px 12px rgba(0, 0, 0, 0.12);
    --shadow-lg: 0 6px 16px rgba(0, 0, 0, 0.2);
    --shadow-button: 0 4px 8px rgba(0, 0, 0, 0.15);

    /* Spacing */
    --space-xs: 6px;
    --space-sm: 12px;
    --space-md: 16px;
    --space-lg: 20px;
    --space-xl: 24px;
    --space-2xl: 30px;
    --space-3xl: 40px;
    --space-4xl: 60px;

    /* Border Radius */
    --radius-sm: 4px;
    --radius-md: 6px;
    --radius-lg: 8px;

    /* Font Sizes */
    --font-xs: 13px;
    --font-sm: 14px;
    --font-base: 15px;
    --font-md: 16px;
    --font-lg: 24px;
    --font-xl: 28px;

    /* Font Weights */
    --font-normal: 500;
    --font-semibold: 600;
}

body {
    font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, sans-serif;
    overflow: hidden;
}

/* Editor Wrapper - Contains TX Text Control + Backstage */
#editorWrapper {
    position: relative;
    width: 100%;
    height: 100vh;
    overflow: hidden;
}

/* Backstage Container - Overlays ONLY the editor */
.backstage {
    position: absolute;
    top: 0;
    left: -100%;
    width: 100%;
    height: 100%;
    background: var(--color-white);
    z-index: 10000;
    display: flex;
    opacity: 0;
    transition: left 0.3s ease, opacity 0.3s ease;
}

/* Backstage Menu (Left Sidebar) */
.backstage__menu {
    width: 260px;
    background: linear-gradient(180deg, var(--color-primary) 0%, var(--color-primary-dark) 100%);
    color: var(--color-white);
    padding: var(--space-lg) 0;
    flex-shrink: 0;
    display: flex;
    flex-direction: column;
}

.backstage__menu__back-link {
    color: var(--color-white);
    text-decoration: none;
    display: block;
    padding: var(--space-md) var(--space-lg);
    margin-bottom: var(--space-lg);
    cursor: pointer;
    transition: background 0.2s;
    font-size: var(--font-md);
    font-weight: var(--font-normal);
}

    .backstage__menu__back-link:hover {
        background: var(--overlay-light);
    }

.backstage__menu__items {
    list-style: none;
    padding: 0;
    margin: 0;
}

.backstage__menu__item {
    border-bottom: 1px solid var(--overlay-light);
}

    .backstage__menu__item a {
        display: block;
        padding: var(--space-md) var(--space-lg);
        color: var(--color-white);
        text-decoration: none;
        transition: background 0.2s;
        cursor: pointer;
        font-size: var(--font-base);
    }

        .backstage__menu__item a:hover {
            background: var(--overlay-light);
        }

        .backstage__menu__item a.active {
            background: var(--overlay-active);
            font-weight: var(--font-semibold);
            border-left: 4px solid var(--color-white);
        }

/* Backstage Right Side */
.backstage-right {
    flex: 1;
    display: flex;
    flex-direction: column;
    overflow: hidden;
    background: var(--color-gray-50);
}

/* Title Section */
.backstage-right__title {
    padding: var(--space-2xl) var(--space-3xl);
    background: var(--color-white);
    border-bottom: 2px solid var(--color-gray-200);
    flex-shrink: 0;
}

    .backstage-right__title h1 {
        margin: 0;
        font-size: var(--font-xl);
        font-weight: var(--font-semibold);
        color: var(--color-gray-900);
    }

/* Content Stage */
.backstage-right__stage {
    flex: 1;
    padding: var(--space-3xl);
    background: var(--color-gray-50);
    overflow-y: auto;
}

/* Document List */
.document-list {
    list-style: none;
    padding: 0;
    margin: 0;
}

.document-item {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: var(--space-lg);
    margin-bottom: var(--space-sm);
    background: var(--color-white);
    border-radius: var(--radius-md);
    box-shadow: var(--shadow-sm);
    transition: all 0.2s;
}

    .document-item:hover {
        box-shadow: var(--shadow-md);
        transform: translateY(-2px);
    }

.document-info {
    flex: 1;
    min-width: 0;
}

.document-name {
    font-weight: var(--font-semibold);
    font-size: var(--font-md);
    color: var(--color-gray-900);
    margin-bottom: var(--space-xs);
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
}

.document-meta {
    font-size: var(--font-xs);
    color: var(--color-gray-600);
}

.load-button {
    padding: 10px var(--space-xl);
    background: var(--color-primary);
    color: var(--color-white);
    border: none;
    border-radius: var(--radius-sm);
    cursor: pointer;
    font-size: var(--font-sm);
    font-weight: var(--font-normal);
    transition: all 0.2s;
    flex-shrink: 0;
}

    .load-button:hover {
        background: var(--color-primary-dark);
        transform: translateY(-1px);
        box-shadow: var(--shadow-button);
    }

    .load-button:active {
        transform: translateY(0);
    }

    .load-button:disabled {
        background: var(--color-gray-400);
        cursor: not-allowed;
        transform: none;
    }

/* New Document View */
.backstage-view-content {
    text-align: center;
    padding: var(--space-4xl) var(--space-3xl);
    background: var(--color-white);
    border-radius: var(--radius-lg);
    box-shadow: var(--shadow-sm);
    max-width: 600px;
    margin: 0 auto;
}

    .backstage-view-content h2 {
        font-size: var(--font-xl);
        color: var(--color-gray-900);
        margin-bottom: var(--space-md);
    }

    .backstage-view-content p {
        font-size: var(--font-md);
        color: var(--color-gray-600);
        margin-bottom: var(--space-3xl);
    }

.new-document-options {
    display: flex;
    justify-content: center;
    gap: var(--space-lg);
}

.action-button {
    display: inline-flex;
    align-items: center;
    gap: var(--space-sm);
    padding: var(--space-lg) var(--space-3xl);
    border: none;
    border-radius: var(--radius-md);
    font-size: var(--font-md);
    font-weight: var(--font-semibold);
    cursor: pointer;
    transition: all 0.2s;
}

    .action-button.primary {
        background: var(--color-primary);
        color: var(--color-white);
    }

        .action-button.primary:hover {
            background: var(--color-primary-dark);
            transform: translateY(-2px);
            box-shadow: var(--shadow-lg);
        }

    .action-button .icon {
        font-size: var(--font-lg);
    }

    .action-button .label {
        font-size: var(--font-md);
    }

/* Empty State */
.backstage .stage p {
    text-align: center;
    padding: var(--space-4xl) var(--space-lg);
    color: var(--color-gray-400);
    font-size: var(--font-md);
}

/* Scrollbar Styling */
.backstage .stage::-webkit-scrollbar {
    width: 8px;
}

.backstage .stage::-webkit-scrollbar-track {
    background: var(--color-gray-200);
}

.backstage .stage::-webkit-scrollbar-thumb {
    background: var(--color-gray-500);
    border-radius: var(--radius-sm);
}

    .backstage .stage::-webkit-scrollbar-thumb:hover {
        background: var(--color-gray-700);
    }

#backstage.active {
    left: 0;
    opacity: 1;
}

/* Responsive Design */
@media (max-width: 768px) {
    .backstage__menu {
        width: 200px;
    }

    .backstage .title {
        padding: var(--space-lg);
    }

        .backstage .title h1 {
            font-size: var(--font-lg);
        }

    .backstage .stage {
        padding: var(--space-lg);
    }

    .document-item {
        flex-direction: column;
        align-items: flex-start;
        gap: var(--space-md);
    }

    .load-button {
        width: 100%;
    }
}
```

### 10. Add sample documents

Place the documents that should appear in the backstage Open view in:

- `App_Data/documents`

The controller code above scans this folder and renders all files in the Open stage view.

### 11. Run the application

Start the application and click the custom **File** tab.

The backstage panel will:

- render over the editor
- show the Open view
- list files from `App_Data/documents`
- load the selected document through `/Home/LoadTemplate`

## Output

A baseline ASP.NET Core MVC document editor application with:

- a custom File tab in the ribbon
- a backstage overlay panel
- server-side document discovery from `App_Data/documents`
- multi-format document loading converted to `InternalUnicodeFormat`

## Notes

- This task is incremental and assumes the baseline setup already exists.
- The code in this task is taken from the sample repository and should be used as shown.
- Do not invent additional controller actions, JavaScript modules, or CSS structure.
