# TX Text Control Document Editor - Layout Integration Patterns

This project demonstrates **5 professional layout patterns** for integrating the TX Text Control Document Editor into ASP.NET Core MVC applications. Each pattern is designed for specific use cases and provides a complete, copy-paste-ready implementation.

## ?? Table of Contents

- [Overview](#overview)
- [Project Setup](#project-setup)
- [Layout Patterns](#layout-patterns)
  - [1. Full Screen Editor](#1-full-screen-editor)
  - [2. Modal Popup Editor](#2-modal-popup-editor)
  - [3. Sidebar Panel Editor](#3-sidebar-panel-editor)
  - [4. Split Panel Editor](#4-split-panel-editor)
  - [5. Tabbed Document Editor](#5-tabbed-document-editor)
- [Common TX Text Control Integration](#common-tx-text-control-integration)
- [Best Practices](#best-practices)

---

## Overview

All examples are **self-contained** - each view includes:
- ? Complete HTML structure (`<!DOCTYPE>`, `<html>`, `<head>`, `<body>`)
- ? Embedded CSS styles (no external stylesheets needed)
- ? Embedded JavaScript (no external JS files needed)
- ? Local resource references (Bootstrap Icons)
- ? TX Text Control integration code

**Technology Stack:**
- ASP.NET Core MVC (.NET 10)
- TX Text Control Document Editor
- Bootstrap Icons 1.11.3
- Vanilla JavaScript (no jQuery required for TX Text Control)

---

## Project Setup

### Prerequisites

1. **TX Text Control License** - Valid license or trial token
2. **.NET 10 SDK** installed
3. **Client Libraries** managed via `libman.json`

### Restore Dependencies

```bash
# Restore NuGet packages
dotnet restore

# Restore client-side libraries (or right-click libman.json in Visual Studio)
# Libraries restored to wwwroot/lib/:
# - bootstrap@5.3.3
# - bootstrap-icons@1.11.3
# - jquery@3.7.1 (for other parts of the app, not required for TX Text Control)
```

### Run the Application

```bash
dotnet run
# Navigate to https://localhost:7187
```

---

## Layout Patterns

---

## 1. Full Screen Editor

### Description
A full-viewport document editor with a centered, elegant container. Perfect for focused document editing with maximum screen real estate.

### Use Cases
- Primary document editing interface
- Dedicated document creation/editing pages
- When the editor is the main focus of the page

### File
`Views/Home/FullScreen.cshtml`

### TX Text Control Specific Implementation

#### CSS Requirements

```css
#text-control-container {
    height: calc(100vh - 6rem);  /* Full viewport height minus margins */
    width: 100%;
}
```

**Key Points:**
- Container must have explicit height (TX Text Control needs defined dimensions)
- Using `calc()` to account for surrounding elements
- Width set to 100% to fill parent container

#### JavaScript Requirements

**No JavaScript required** for basic Full Screen implementation.

**Key Points:**
- TX Text Control initializes automatically when rendered
- No special JavaScript needed for this layout
- Container CSS handles all sizing requirements

#### Razor/Server-Side Integration

```razor
@Html.TXTextControl().TextControl(settings => {
    settings.Dock = TXTextControl.Web.DockStyle.Fill;
}).Render()
```

**Key Points:**
- `Dock = DockStyle.Fill` makes editor fill its container
- No additional configuration needed for basic setup

---

## 2. Modal Popup Editor

### Description
The editor opens in a centered modal dialog with backdrop overlay. Perfect for quick edits without leaving the current page context.

### Use Cases
- Quick document edits
- Inline editing workflows
- When context preservation is important

### File
`Views/Home/Modal.cshtml`

### TX Text Control Specific Implementation

#### CSS Requirements

```css
.modal-overlay {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5);
    z-index: 9999;
    display: none;  /* Hidden by default */
    opacity: 0;
    transition: opacity 0.3s ease;
}

.modal-overlay.show {
    display: flex !important;
    opacity: 1;
}

#text-control-container {
    height: calc(100% - 80px);  /* Account for modal header */
    width: calc(100% - 2rem);
    margin: 0 auto;
}
```

**Key Points:**
- Modal must be hidden initially (`display: none`)
- High `z-index` to appear above other content
- TX Text Control container height calculated after modal header space

#### JavaScript Requirements

```javascript
openModalBtn.addEventListener('click', function() {
    modalOverlay.style.display = 'flex';
    setTimeout(() => {
        modalOverlay.classList.add('show');
    }, 10);
});
```

**Key Points:**
- Display modal first, then trigger animation
- TX Text Control initializes automatically when rendered
- No special refresh or initialization code needed

---

## 3. Sidebar Panel Editor

### Description
A slide-out sidebar panel from the left covering 75% of screen width. Ideal for side-by-side workflows.

### Use Cases
- Document preview while browsing
- Context-preserving editing
- Multi-pane applications

### File
`Views/Home/Sidebar.cshtml`

### TX Text Control Specific Implementation

#### CSS Requirements

```css
.sidebar-container {
    position: fixed;
    top: 0;
    left: -75%;  /* Hidden off-screen */
    width: 75%;
    height: 100vh;
    transition: left 0.3s ease;
}

.sidebar-overlay.show .sidebar-container {
    left: 0;  /* Slide in */
}

#sidebar-text-control-container {
    flex: 1;
    width: calc(100% - 3rem);
    height: calc(100vh - 100px);  /* Account for header */
    margin: 1.5rem;
}
```

**Key Points:**
- Sidebar slides in from off-screen position
- Container dimensions calculated after header/margins
- Fixed positioning for smooth animation

#### JavaScript Requirements

```javascript
openSidebarBtn.addEventListener('click', function() {
    sidebarOverlay.style.display = 'block';
    setTimeout(() => {
        sidebarOverlay.classList.add('show');
    }, 10);
});
```

**Key Points:**
- Display sidebar, then trigger slide-in animation
- No special TX Text Control initialization needed
- Editor renders correctly without manual refresh

---

## 4. Split Panel Editor

### Description
Resizable split-pane layout with editor in the right panel. Perfect for displaying related content alongside the editor.

### Use Cases
- Document properties panel
- Template selection
- Reference materials display
- Navigation tree with editor

### File
`Views/Home/SplitPanel.cshtml`

### TX Text Control Specific Implementation

#### CSS Requirements

```css
.split-container {
    display: flex;
    height: calc(100vh - 60px);  /* Account for header */
    overflow: hidden;
}

.left-panel {
    flex: 0 0 35%;  /* Fixed at 35% initially */
    min-width: 200px;
}

.resizer {
    flex: 0 0 6px;
    cursor: col-resize;
}

.right-panel {
    flex: 1;  /* Takes remaining space */
    min-width: 300px;
}

#text-control-container {
    height: 100%;
    width: 100%;
}
```

**Key Points:**
- Flexbox layout for responsive panels
- Min-width constraints prevent unusable sizes
- Editor container fills right panel completely

#### JavaScript Requirements

**Resizable Panel Logic:**
```javascript
let isResizing = false;

resizer.addEventListener('mousedown', (e) => {
    isResizing = true;
});

document.addEventListener('mousemove', (e) => {
    if (!isResizing) return;
    
    const containerRect = splitContainer.getBoundingClientRect();
    const newLeftWidth = e.clientX - containerRect.left;
    const containerWidth = containerRect.width;
    
    // Constraints: 15% to 70%
    const minLeftWidth = containerWidth * 0.15;
    const maxLeftWidth = containerWidth * 0.70;
    
    if (newLeftWidth >= minLeftWidth && newLeftWidth <= maxLeftWidth) {
        const leftPercentage = (newLeftWidth / containerWidth) * 100;
        leftPanel.style.flex = `0 0 ${leftPercentage}%`;
    }
});

document.addEventListener('mouseup', () => {
    isResizing = false;
});
```

**TX Text Control Integration:**

No special JavaScript required for TX Text Control in split panel layout.

**Key Points:**
- No `refreshLayout()` needed during resize (flexbox handles it automatically)
- Min/max constraints ensure editor remains usable
- Editor automatically adjusts to panel size changes

---

## 5. Tabbed Document Editor

### Description
**Most advanced pattern**: Multiple document tabs with a single editor instance that moves in the DOM. Includes document persistence, cursor position memory, and smooth transitions.

### Use Cases
- Multi-document editing (IDE-like)
- Document comparison workflows
- Session-based document management

### File
`Views/Home/TabbedView.cshtml`

### TX Text Control Specific Implementation

#### CSS Requirements

```css
#text-control-container {
    height: 100%;
    width: 100%;
    padding: 1em;
    opacity: 1;
    transition: opacity 0.2s ease;  /* Smooth fade */
}

#text-control-container.fading {
    opacity: 0;  /* Fade out during tab switch */
}

.tab-pane {
    display: none;
    height: 100%;
    width: 100%;
    padding: 20px;
    position: absolute;
}

.tab-pane.active {
    display: block;
}
```

**Key Points:**
- Fade transition eliminates flickering during DOM moves
- Tab panes positioned absolutely for smooth switching
- Padding inside editor container for visual spacing

#### JavaScript Requirements - Core Pattern

**Storage:**
```javascript
const tabDocuments = {};   // Store document content
const tabPositions = {};   // Store cursor positions
```

**1. Save Document and Position:**
```javascript
function saveCurrentDocument(tabId, callback) {
    // STEP 1: Save cursor position
    TXTextControl.inputPosition.getTextPosition(function(position) {
        tabPositions[tabId] = position;
        
        // STEP 2: Save document content
        TXTextControl.saveDocument(
            TXTextControl.StreamType.InternalUnicodeFormat, 
            function(e) {
                tabDocuments[tabId] = e.data;
                if (callback) callback();
            }
        );
    });
}
```

**Key TX Text Control APIs:**
- `TXTextControl.inputPosition.getTextPosition(callback)` - Gets cursor position
- `TXTextControl.saveDocument(streamType, callback)` - Saves document
- `TXTextControl.StreamType.InternalUnicodeFormat` - Preserves all formatting

**2. Load Document and Restore Position:**
```javascript
function loadDocumentForTab(tabId, targetPane) {
    const savedDocument = tabDocuments[tabId];
    const savedPosition = tabPositions[tabId];
    
    if (savedDocument) {
        // Load saved document
        TXTextControl.loadDocument(
            TXTextControl.StreamType.InternalUnicodeFormat, 
            savedDocument, 
            function() {
                // Move editor in DOM
                editorWrapper.appendChild(editorContainer);
                
                // Fade in
                setTimeout(() => {
                    editorContainer.classList.remove('fading');
                    
                    // Restore position if exists
                    if (savedPosition) {
                        restorePosition(tabId, savedPosition);
                    } else {
                        TXTextControl.focus();
                    }
                }, 50);
            }
        );
    } else {
        // New tab: reset contents
        TXTextControl.resetContents(function() {
            editorWrapper.appendChild(editorContainer);
            setTimeout(() => {
                editorContainer.classList.remove('fading');
                TXTextControl.focus();
            }, 50);
        });
    }
}
```

**Key TX Text Control APIs:**
- `TXTextControl.loadDocument(streamType, data, callback)` - Loads document
- `TXTextControl.resetContents(callback)` - Clears editor
- `TXTextControl.focus()` - Sets focus to editor

**3. Restore Cursor Position:**
```javascript
function restorePosition(tabId, position) {
    // STEP 1: Set cursor position
    TXTextControl.setInputPositionByTextPosition(
        position,
        TXTextControl.TextFieldPosition.OutsideTextField,
        function() {
            // STEP 2: Scroll to position
            TXTextControl.inputPosition.scrollTo(
                TXTextControl.InputPosition.ScrollPosition.Top,
                function() {
                    // STEP 3: Set focus
                    TXTextControl.focus();
                }
            );
        }
    );
}
```

**Key TX Text Control APIs:**
- `TXTextControl.setInputPositionByTextPosition(position, textFieldPosition, callback)` - Sets cursor
- `TXTextControl.inputPosition.scrollTo(scrollPosition, callback)` - Scrolls viewport
- `TXTextControl.TextFieldPosition.OutsideTextField` - Position outside text fields
- `TXTextControl.InputPosition.ScrollPosition.Top` - Scroll to top of viewport

**4. Smooth Tab Switching:**
```javascript
function switchTab(tabId) {
    if (activeTabId === tabId) return;
    
    // STEP 1: Fade out
    editorContainer.classList.add('fading');
    
    // STEP 2: Wait for fade, then save and switch
    setTimeout(() => {
        saveCurrentDocument(activeTabId, function() {
            // Update UI
            document.querySelectorAll('.tab').forEach(t => t.classList.remove('active'));
            document.querySelectorAll('.tab-pane').forEach(p => p.classList.remove('active'));
            
            newTab.classList.add('active');
            newPane.classList.add('active');
            
            // STEP 3: Load new document (includes fade in)
            loadDocumentForTab(tabId, newPane);
            
            activeTabId = tabId;
        });
    }, 200);  // Match CSS transition duration
}
```

**Flow:**
1. Fade out (200ms)
2. Save current document + position
3. Switch UI
4. Load new document
5. Move editor in DOM (invisible during fade)
6. Restore position + scroll
7. Fade in (200ms)
8. Set focus

**Key Points:**
- Single editor instance moves in DOM (singleton pattern)
- Fade transitions prevent flickering
- Async callbacks ensure proper sequence
- Focus management for seamless UX

---

## Common TX Text Control Integration

### Basic Rendering

All views use the same Razor syntax:

```razor
@using TXTextControl.Web.MVC

@Html.TXTextControl().TextControl(settings => {
    settings.Dock = TXTextControl.Web.DockStyle.Fill;
}).Render()
```

### Required Container Properties

**Every TX Text Control container MUST have:**

```css
#container {
    height: [explicit-height];  /* Required - no auto or percentage without explicit parent */
    width: [width];              /* Usually 100% or explicit */
}
```

**Examples:**
```css
/* ? GOOD */
height: 500px;
height: calc(100vh - 200px);
height: 100%;  /* Only if parent has explicit height */

/* ? BAD */
height: auto;   /* Will not work */
height: 100%;   /* Without explicit parent height */
```

### Common JavaScript Patterns

#### 1. Document Operations
```javascript
// Save
TXTextControl.saveDocument(TXTextControl.StreamType.InternalUnicodeFormat, callback);

// Load
TXTextControl.loadDocument(TXTextControl.StreamType.InternalUnicodeFormat, data, callback);

// Reset
TXTextControl.resetContents(callback);
```

#### 2. Position Management
```javascript
// Get position
TXTextControl.inputPosition.getTextPosition(callback);

// Set position
TXTextControl.setInputPositionByTextPosition(position, textFieldPosition, callback);

// Scroll
TXTextControl.inputPosition.scrollTo(scrollPosition, callback);
```

#### 3. Focus Management
```javascript
// Set focus to editor
TXTextControl.focus();
```

---

## Best Practices

### 1. Container Sizing
? **Always use explicit heights**
```css
#editor { height: calc(100vh - 100px); }
```

? **Avoid auto or undefined heights**
```css
#editor { height: auto; }  /* Won't work */
```

### 2. Dynamic Layouts

? **TX Text Control handles layout automatically**
```javascript
// No manual refresh needed - editor adapts automatically
sidebarOverlay.classList.add('show');
```

? **Don't use unnecessary refreshLayout() calls**
```javascript
// Not needed in modern implementations
TXTextControl.refreshLayout();  // Unnecessary
```

### 3. Singleton Pattern

? **Move single instance in DOM**
```javascript
// Tabbed view approach
editorWrapper.appendChild(editorContainer);
```

? **Don't create multiple instances**
```html
<!-- Licensing and performance issues -->
<div id="tab1">@Html.TXTextControl()...</div>
<div id="tab2">@Html.TXTextControl()...</div>
```

### 4. Async Callbacks

? **Chain operations properly**
```javascript
TXTextControl.saveDocument(streamType, function(e) {
    tabDocuments[id] = e.data;
    // Now safe to switch tabs
});
```

? **Don't assume synchronous**
```javascript
TXTextControl.saveDocument(streamType, callback);
switchTabs();  // May execute before save completes
```

### 5. Self-Contained Views

? **Each view includes everything**
```razor
<!DOCTYPE html>
<html>
<head>
    <link rel="stylesheet" href="~/lib/bootstrap-icons/..." />
    <style>/* Embedded CSS */</style>
</head>
<body>
    <!-- Content -->
    <script>/* Embedded JS */</script>
</body>
</html>
```

Benefits:
- Easy to copy/paste
- No external dependencies
- Clear understanding of requirements
- Perfect for blog articles/tutorials

---

## Architecture Decisions

### Why Self-Contained Views?

Each layout (except Index) uses `Layout = null` and includes complete HTML:

**Advantages:**
1. **Copy-Paste Ready** - Developers can grab entire file
2. **No Hidden Dependencies** - All CSS/JS visible in one place
3. **Educational** - Perfect for tutorials and blog articles
4. **Portable** - Easy to move between projects
5. **Clear Requirements** - Explicit about what's needed

### Why Single Editor Instance? (Tabbed View)

Moving one editor instance in DOM instead of multiple instances:

**Advantages:**
1. **Licensing** - Only one license consumed
2. **Performance** - Lower memory footprint
3. **Consistency** - Same editor state/behavior
4. **Efficiency** - Faster than re-initialization

**Implementation:**
```javascript
// DOM manipulation (singleton)
targetWrapper.appendChild(editorContainer);
```

vs.

```javascript
// Multiple instances (avoid)
// Each tab has its own editor instance
```

---

## TX Text Control Stream Types

### InternalUnicodeFormat
```javascript
TXTextControl.StreamType.InternalUnicodeFormat
```

**Characteristics:**
- ? Preserves all formatting
- ? Preserves images and embedded objects
- ? Preserves track changes
- ? Best for save/restore operations
- ? Used in tabbed view for document persistence

**Use Cases:**
- Temporary storage (like tabbed view)
- Session state preservation
- Undo/redo implementations

### Other Common Formats
```javascript
TXTextControl.StreamType.MSWord           // .docx
TXTextControl.StreamType.AdobePDF         // .pdf
TXTextControl.StreamType.PlainText        // .txt
TXTextControl.StreamType.RichTextFormat   // .rtf
```

---

## Performance Considerations

### 1. Tabbed View Document Storage

Current implementation uses in-memory storage:
```javascript
const tabDocuments = {};  // In-memory
```

**For Production:**
- Consider localStorage for persistence across page reloads
- Implement auto-save to server
- Add document size limits
- Clear storage on tab close

### 2. Fade Transitions

Timing is crucial:
```javascript
editorContainer.classList.add('fading');    // 0ms
setTimeout(saveAndSwitch, 200);             // Wait for fade
setTimeout(fadeIn, 50);                     // Brief delay before fade in
```

**Optimization:**
- CSS transition: 200ms (fast enough, not jarring)
- JavaScript delays match CSS timing
- Fade prevents visible DOM manipulation

### 3. Refresh Layout Timing

```javascript
setTimeout(() => {
    TXTextControl.refreshLayout();
}, 350);  // Must exceed animation duration (300ms)
```

**Why Timing Matters:**
- Too early: Editor refreshes mid-animation (flickering)
- Too late: User sees incorrect layout briefly
- Just right: Smooth transition, correct sizing

---

## Troubleshooting Common Issues

### Editor Appears Blank

**Cause:** Container has no explicit height

**Solution:**
```css
#container {
    height: 600px;  /* Or calc() expression */
}
```

### Editor Doesn't Size Correctly in Sidebar

**Cause:** Container has incorrect CSS or dimensions

**Solution:**
```css
#sidebar-text-control-container {
    flex: 1;
    width: calc(100% - 3rem);
    height: calc(100vh - 100px);
}
```

### Tab Switch Shows Flickering

**Cause:** No fade transition during DOM move

**Solution:**
```css
#text-control-container {
    opacity: 1;
    transition: opacity 0.2s ease;
}
#text-control-container.fading {
    opacity: 0;
}
```

### Cursor Position Not Restored

**Cause:** Missing `scrollTo()` call

**Solution:**
```javascript
TXTextControl.setInputPositionByTextPosition(position, ..., function() {
    TXTextControl.inputPosition.scrollTo(..., function() {
        TXTextControl.focus();  // Don't forget this!
    });
});
```

---

## Resources

- **TX Text Control Documentation:** https://www.textcontrol.com/support/documentation/
- **Bootstrap Icons:** https://icons.getbootstrap.com/
- **ASP.NET Core MVC:** https://docs.microsoft.com/aspnet/core/mvc/

---

## License

This project serves as example code for integrating TX Text Control Document Editor. Refer to Text Control's licensing terms for production use.

---

**Created with ?? using TX Text Control Document Editor**

*Last Updated: 2026*
