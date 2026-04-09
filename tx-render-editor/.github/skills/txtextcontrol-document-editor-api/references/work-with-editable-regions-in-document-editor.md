# Editable Regions

Add, inspect, update, navigate, and remove editable regions in the TX Text Control Document Editor.

## Scope

This task is framework-agnostic and focuses on the TX Text Control JavaScript API.
It applies to any host framework (for example plain JavaScript, Angular, React, or ASP.NET).

## Prerequisites

- A running TX Text Control editor instance in your host application

## Setup (once)

Add this helper script to `{hostViewOrComponent}`:

```html
<script>
    function onTxError(error) {
        console.error("TX Text Control error:", error);
    }

    function withEditableRegions(onReady) {
        TXTextControl.textParts.getMainText(function(mainText) {
            onReady(mainText.editableRegions);
        }, onTxError);
    }
</script>
```

---

## Editable Region Events

Editable-region event callbacks use `EditableRegionEventArgs`.

Documented editable-region events include:

- `editableRegionEntered`
- `editableRegionLeft`

Example: inspect the payload of the documented `editableRegionEntered` event.

```javascript
TXTextControl.addEventListener("editableRegionEntered", function(args) {
    console.log("Editable region event args:", args);
    console.log({
        id: args.id,
        start: args.start,
        length: args.length,
        userName: args.userName
    });
});
```

Return object shape for editable-region event callbacks:

```javascript
{
    id: 25,
    start: 120,
    length: 18,
    userName: "jsmith"
}
```

---

## Add an Editable Region

```javascript
withEditableRegions(function(editableRegions) {
    editableRegions.add("jsmith", 25, 120, 18, function(response, addResult) {
        console.log("Add result:", addResult);

        response.editableRegion.getUserName(function(userName) {
            console.log("Added editable region:", {
                id: 25,
                userName: userName
            });
        }, onTxError);
    }, onTxError);
});
```

Placeholders:
- `"jsmith"` -> `{userName}`
- `25` -> `{editableRegionId}`
- `120` -> `{startPosition}`
- `18` -> `{length}`

Notes:
- `start` is one-based.
- If `length` is `0`, the current text selection defines the region position.
- `response.editableRegion` is the added `EditableRegion` object.

---

## Get Editable Regions at the Current Input Position

`EditableRegionCollection.getItems` returns all editable regions at the current text input position.

```javascript
withEditableRegions(function(editableRegions) {
    editableRegions.getItems(function(result) {
        if (!result || result.length === 0) {
            console.warn("No editable region at current text input position.");
            return;
        }

        result.forEach(function(editableRegion) {
            editableRegion.getID(function(id) {
                editableRegion.getUserName(function(userName) {
                    console.log("Editable region at input position:", {
                        id: id,
                        userName: userName
                    });
                }, onTxError);
            }, onTxError);
        });
    }, onTxError);
});
```

Notes:
- The callback receives `EditableRegion[]`.
- The callback result is `null` when no editable region exists at the current input position.

---

## Inspect Editable Region Metadata

```javascript
withEditableRegions(function(editableRegions) {
    editableRegions.getItems(function(result) {
        if (!result || result.length === 0) {
            console.warn("No editable region at current text input position.");
            return;
        }

        var editableRegion = result[0];

        editableRegion.getID(function(id) {
            editableRegion.getUserName(function(userName) {
                editableRegion.getStart(function(start) {
                    editableRegion.getLength(function(length) {
                        editableRegion.getText(function(text) {
                            editableRegion.getHighlightMode(function(highlightMode) {
                                editableRegion.getHighlightColor(function(highlightColor) {
                                    console.log("Editable region metadata:", {
                                        id: id,
                                        userName: userName,
                                        start: start,
                                        length: length,
                                        text: text,
                                        highlightMode: highlightMode,
                                        highlightColor: highlightColor
                                    });
                                }, onTxError);
                            }, onTxError);
                        }, onTxError);
                    }, onTxError);
                }, onTxError);
            }, onTxError);
        }, onTxError);
    }, onTxError);
});
```

Notes:
- `start` is one-based.
- Use `result[0]` when multiple editable regions overlap at the current input position and you want the first returned item.

---

## Update the Current Editable Region User Name or ID

Set a user name:

```javascript
withEditableRegions(function(editableRegions) {
    editableRegions.getItems(function(result) {
        if (!result || result.length === 0) {
            console.warn("No editable region at current text input position.");
            return;
        }

        result[0].setUserName("adoe", function() {
            console.log("Editable region user name updated.");
        }, onTxError);
    }, onTxError);
});
```

Set an ID:

```javascript
withEditableRegions(function(editableRegions) {
    editableRegions.getItems(function(result) {
        if (!result || result.length === 0) {
            console.warn("No editable region at current text input position.");
            return;
        }

        result[0].setID(42, function() {
            console.log("Editable region ID updated.");
        }, onTxError);
    }, onTxError);
});
```

Placeholders:
- `"adoe"` -> `{newUserName}`
- `42` -> `{newEditableRegionId}`

---

## Update Highlighting for the Current Editable Region

Set the region highlight mode:

```javascript
withEditableRegions(function(editableRegions) {
    editableRegions.getItems(function(result) {
        if (!result || result.length === 0) {
            console.warn("No editable region at current text input position.");
            return;
        }

        result[0].setHighlightMode(TXTextControl.HighlightMode.Frame, function() {
            console.log("Editable region highlight mode updated.");
        }, onTxError);
    }, onTxError);
});
```

Set the region highlight color:

```javascript
withEditableRegions(function(editableRegions) {
    editableRegions.getItems(function(result) {
        if (!result || result.length === 0) {
            console.warn("No editable region at current text input position.");
            return;
        }

        result[0].setHighlightColor("#ffd54f", function() {
            console.log("Editable region highlight color updated.");
        }, onTxError);
    }, onTxError);
});
```

Placeholders:
- `TXTextControl.HighlightMode.Frame` -> `{highlightMode}`
- `"#ffd54f"` -> `{highlightColor}`

---

## Get or Set the Global Editable Region Highlight Mode

Get the global highlight mode:

```javascript
TXTextControl.getEditableRegionHighlightMode(function(highlightMode) {
    console.log("Global editable region highlight mode:", highlightMode);
}, onTxError);
```

Set the global highlight mode:

```javascript
TXTextControl.setEditableRegionHighlightMode(TXTextControl.HighlightMode.Frame, function() {
    console.log("Global editable region highlight mode updated.");
}, onTxError);
```

Placeholders:
- `TXTextControl.HighlightMode.Frame` -> `{highlightMode}`

---

## Save or Navigate to the Current Editable Region

Save the current editable region:

```javascript
withEditableRegions(function(editableRegions) {
    editableRegions.getItems(function(result) {
        if (!result || result.length === 0) {
            console.warn("No editable region at current text input position.");
            return;
        }

        result[0].save(function(saved) {
            console.log("Editable region saved:", saved);
        }, onTxError);
    }, onTxError);
});
```

Scroll to the current editable region:

```javascript
withEditableRegions(function(editableRegions) {
    editableRegions.getItems(function(result) {
        if (!result || result.length === 0) {
            console.warn("No editable region at current text input position.");
            return;
        }

        result[0].scrollTo(function(scrolled) {
            console.log("Scrolled to editable region:", scrolled);
        }, onTxError);
    }, onTxError);
});
```

---

## List Editable Regions

Get the editable-region count and enumerate all items in the main text.

```javascript
withEditableRegions(function(editableRegions) {
    editableRegions.getCount(function(count) {
        console.log("Total editable regions:", count);
    }, onTxError);

    editableRegions.forEach(function(editableRegion) {
        editableRegion.getID(function(id) {
            editableRegion.getUserName(function(userName) {
                editableRegion.getStart(function(start) {
                    console.log("Editable region item:", {
                        id: id,
                        userName: userName,
                        start: start
                    });
                }, onTxError);
            }, onTxError);
        }, onTxError);
    }, onTxError);
});
```

---

## Get an Editable Region by Index

```javascript
withEditableRegions(function(editableRegions) {
    editableRegions.elementAt(0, function(editableRegion) {
        if (!editableRegion) {
            console.warn("No editable region at index:", 0);
            return;
        }

        editableRegion.getID(function(id) {
            editableRegion.getUserName(function(userName) {
                console.log("Editable region at index:", {
                    index: 0,
                    id: id,
                    userName: userName
                });
            }, onTxError);
        }, onTxError);
    }, onTxError);
});
```

Placeholders:
- `0` -> `{zeroBasedIndex}`

---

## Remove the Current Editable Region and Keep Its Text

```javascript
withEditableRegions(function(editableRegions) {
    editableRegions.getItems(function(result) {
        if (!result || result.length === 0) {
            console.warn("No editable region at current text input position.");
            return;
        }

        editableRegions.remove(result[0], false, function(removed) {
            console.log("Editable region removed and text kept:", removed);
        }, onTxError);
    }, onTxError);
});
```

Notes:
- Removing an editable region does not remove its text.
- `selectedPart = false` removes the whole editable region.

---

## Remove Only the Selected Part of the Current Editable Region

```javascript
withEditableRegions(function(editableRegions) {
    editableRegions.getItems(function(result) {
        if (!result || result.length === 0) {
            console.warn("No editable region at current text input position.");
            return;
        }

        editableRegions.remove(result[0], true, function(removed) {
            console.log("Selected editable-region part removed:", removed);
        }, onTxError);
    }, onTxError);
});
```

Notes:
- `selectedPart = true` removes only the selected part of the editable region.
- If the current selection is in the middle of the region, the remaining content can be split into two editable regions with the same parameters.

---

## API Mapping

- `TXTextControl.addEventListener(eventName, callback)`
- `TXTextControl.textParts.getMainText(callback, errorCallback)`
- `FormattedText.editableRegions`
- `EditableRegionCollection.add(userName, id, start, length, callback, errorCallback)`
- `EditableRegionCollection.getItems(callback, errorCallback)`
- `EditableRegionCollection.remove(editableRegion, selectedPart, callback, errorCallback)`
- `Collection.getCount(callback, errorCallback)`
- `Collection.forEach(callback, errorCallback)`
- `Collection.elementAt(index, callback, errorCallback)`
- `EditableRegion.getID(callback, errorCallback)`
- `EditableRegion.getUserName(callback, errorCallback)`
- `EditableRegion.getStart(callback, errorCallback)`
- `EditableRegion.getLength(callback, errorCallback)`
- `EditableRegion.getText(callback, errorCallback)`
- `EditableRegion.getHighlightMode(callback, errorCallback)`
- `EditableRegion.getHighlightColor(callback, errorCallback)`
- `EditableRegion.setID(id, callback, errorCallback)`
- `EditableRegion.setUserName(userName, callback, errorCallback)`
- `EditableRegion.setHighlightMode(value, callback, errorCallback)`
- `EditableRegion.setHighlightColor(value, callback, errorCallback)`
- `EditableRegion.save(callback, errorCallback)`
- `EditableRegion.scrollTo(callback, errorCallback)`
- `TXTextControl.getEditableRegionHighlightMode(callback, errorCallback)`
- `TXTextControl.setEditableRegionHighlightMode(value, callback, errorCallback)`

## Notes

- `EditableRegionCollection.getItems` reads all editable regions at the current text input position and returns `EditableRegion[]` or `null`.
- `EditableRegionEventArgs` contains `id`, `start`, `length`, and `userName`.
- `EditableRegionCollection.add` uses an `AddEditableRegionCallback`, which receives `response.editableRegion` and an add result value.
- Use only the APIs and object paths listed in this task.