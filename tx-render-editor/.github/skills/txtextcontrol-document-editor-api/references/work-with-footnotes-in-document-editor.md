# Footnotes

Add, inspect, navigate, format, and remove footnotes in the TX Text Control Document Editor.

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

    function withFootnotes(onReady) {
        onReady(TXTextControl.footnotes);
    }
</script>
```

---

## Footnote Events

Attach footnote-related listeners with `TXTextControl.addEventListener(eventName, callback)`.
Footnote event callbacks use `FootnoteCallback` and receive one `FootnoteEventArgs` object.

```javascript
TXTextControl.addEventListener("{footnoteEventName}", function(args) {
    console.log("Footnote event args:", args);
    console.log({
        name: args.name,
        number: args.number,
        referenceMarkLength: args.referenceMarkLength,
        referenceMarkStart: args.referenceMarkStart,
        highlightColor: args.highlightColor,
        highlightMode: args.highlightMode,
        id: args.id
    });
});
```

Placeholders:
- `"{footnoteEventName}"` -> `{documentedFootnoteEventName}`

Return object shape for footnote event callbacks:

```javascript
{
    name: "MyFootnote",
    number: 1,
    referenceMarkLength: 1,
    referenceMarkStart: 42,
    highlightColor: "#ffff00",
    highlightMode: "Inherit",
    id: 25
}
```

---

## Add a Footnote at the Current Input Position

```javascript
withFootnotes(function(footnotes) {
    footnotes.add("Footnote text", "MyFootnote", 25, function(response) {
        console.log("Add response:", response);
        console.log("Added:", response.added);

        if (!response.added || !response.footnote) {
            return;
        }

        response.footnote.getNumber(function(number) {
            response.footnote.getName(function(name) {
                console.log("Added footnote:", {
                    name: name,
                    number: number
                });
            }, onTxError);
        }, onTxError);
    }, onTxError);
});
```

Placeholders:
- `"Footnote text"` -> `{footnoteText}`
- `"MyFootnote"` -> `{footnoteNameOrEmpty}`
- `25` -> `{footnoteIdOrZero}`

Notes:
- Set `name` to `null` or an empty string to omit the name.
- Set `id` to `null` or `0` to omit the identifier.
- The add callback receives `FootnoteCallbackData` with `{ footnote, added }`.

---

## List Footnotes

Get the footnote count and enumerate all footnotes.

```javascript
withFootnotes(function(footnotes) {
    footnotes.getCount(function(count) {
        console.log("Total footnotes:", count);
    }, onTxError);

    footnotes.forEach(function(footnote) {
        footnote.getNumber(function(number) {
            footnote.getName(function(name) {
                footnote.getID(function(id) {
                    console.log("Footnote item:", {
                        number: number,
                        name: name,
                        id: id
                    });
                }, onTxError);
            }, onTxError);
        }, onTxError);
    }, onTxError);
});
```

Placeholders:
- `{scrollPositionValue}` -> `{TXTextControl.ScrollPosition value}`

---

## Get a Footnote by Index

```javascript
withFootnotes(function(footnotes) {
    footnotes.elementAt(0, function(footnote) {
        if (!footnote) {
            console.warn("No footnote at index:", 0);
            return;
        }

        footnote.getName(function(name) {
            footnote.getNumber(function(number) {
                console.log("Footnote at index:", {
                    index: 0,
                    name: name,
                    number: number
                });
            }, onTxError);
        }, onTxError);
    }, onTxError);
});
```

Placeholders:
- `0` -> `{zeroBasedIndex}`

---

## Navigate to a Footnote

Scroll to a footnote by using either the default behavior or an explicit scroll position.

```javascript
withFootnotes(function(footnotes) {
    footnotes.elementAt(0, function(footnote) {
        if (!footnote) {
            console.warn("No footnote at index:", 0);
            return;
        }

        footnote.scrollTo(function(scrolled) {
            console.log("Scrolled to footnote:", scrolled);
        }, onTxError);
    }, onTxError);
});
```

```javascript
withFootnotes(function(footnotes) {
    footnotes.elementAt(0, function(footnote) {
        if (!footnote) {
            console.warn("No footnote at index:", 0);
            return;
        }

        footnote.scrollToPosition({scrollPositionValue}, function(scrolled) {
            console.log("Scrolled to footnote at center:", scrolled);
        }, onTxError);
    }, onTxError);
});
```

---

## Edit the Current Footnote Text

Move the input position into a footnote so the user can edit its text.

```javascript
withFootnotes(function(footnotes) {
    footnotes.elementAt(0, function(footnote) {
        if (!footnote) {
            console.warn("No footnote at index:", 0);
            return;
        }

        footnote.edit(function() {
            console.log("Input moved to footnote text for editing.");
        }, onTxError);
    }, onTxError);
});
```

---

## Update Footnote Metadata

```javascript
withFootnotes(function(footnotes) {
    footnotes.elementAt(0, function(footnote) {
        if (!footnote) {
            console.warn("No footnote at index:", 0);
            return;
        }

        footnote.setName("UpdatedFootnote", function() {
            footnote.setID(42, function() {
                footnote.setHighlightColor("#ffff00", function() {
                    footnote.setHighlightMode({highlightModeValue}, function() {
                        console.log("Footnote metadata updated.");
                    }, onTxError);
                }, onTxError);
            }, onTxError);
        }, onTxError);
    }, onTxError);
});
```

Placeholders:
- `"UpdatedFootnote"` -> `{newFootnoteName}`
- `42` -> `{newFootnoteId}`
- `"#ffff00"` -> `{highlightColor}`
- `{highlightModeValue}` -> `{TXTextControl.HighlightMode value}`

---

## Configure Collection Formatting

```javascript
withFootnotes(function(footnotes) {
    footnotes.setStartNumber(1, function() {
        footnotes.setNumberFormat({numberFormatValue}, function() {
            footnotes.setHighlightMode({highlightModeValue}, function() {
                console.log("Footnote collection formatting updated.");
            }, onTxError);
        }, onTxError);
    }, onTxError);
});
```

Placeholders:
- `{numberFormatValue}` -> `{TXTextControl.NumberFormat value}`
- `{highlightModeValue}` -> `{TXTextControl.HighlightMode value}`

```javascript
withFootnotes(function(footnotes) {
    footnotes.getStartNumber(function(startNumber) {
        footnotes.getNumberFormat(function(numberFormat) {
            footnotes.getHighlightMode(function(highlightMode) {
                footnotes.getDefaultFootnoteHighlightColor(function(defaultColor) {
                    console.log("Footnote settings:", {
                        startNumber: startNumber,
                        numberFormat: numberFormat,
                        highlightMode: highlightMode,
                        defaultHighlightColor: defaultColor
                    });
                }, onTxError);
            }, onTxError);
        }, onTxError);
    }, onTxError);
});
```

---

## Remove a Footnote

```javascript
withFootnotes(function(footnotes) {
    footnotes.elementAt(0, function(footnote) {
        if (!footnote) {
            console.warn("No footnote at index:", 0);
            return;
        }

        footnotes.remove(footnote, function(removed) {
            console.log("Footnote removed:", removed);
        }, onTxError);
    }, onTxError);
});
```

---

## API Mapping
- `TXTextControl.footnotes`
- `TXTextControl.addEventListener(eventName, callback)`
- `FootnoteCollection.add(text, name, id, callback, errorCallback)`
- `FootnoteCollection.remove(footnote, callback, errorCallback)`
- `FootnoteCollection.getDefaultFootnoteHighlightColor(callback, errorCallback)`
- `FootnoteCollection.getHighlightMode(callback, errorCallback)`
- `FootnoteCollection.setHighlightMode(highlightMode, callback, errorCallback)`
- `FootnoteCollection.getNumberFormat(callback, errorCallback)`
- `FootnoteCollection.setNumberFormat(numberFormat, callback, errorCallback)`
- `FootnoteCollection.getStartNumber(callback, errorCallback)`
- `FootnoteCollection.setStartNumber(startNumber, callback, errorCallback)`
- `Collection.getCount(callback, errorCallback)`
- `Collection.forEach(callback, errorCallback)`
- `Collection.elementAt(index, callback, errorCallback)`
- `Footnote.edit(callback, errorCallback)`
- `Footnote.getHighlightColor(callback, errorCallback)`
- `Footnote.setHighlightColor(color, callback, errorCallback)`
- `Footnote.getHighlightMode(callback, errorCallback)`
- `Footnote.setHighlightMode(highlightMode, callback, errorCallback)`
- `Footnote.getID(callback, errorCallback)`
- `Footnote.setID(id, callback, errorCallback)`
- `Footnote.getName(callback, errorCallback)`
- `Footnote.setName(name, callback, errorCallback)`
- `Footnote.getNumber(callback, errorCallback)`
- `Footnote.getReferenceMarkLength(callback, errorCallback)`
- `Footnote.getReferenceMarkStart(callback, errorCallback)`
- `Footnote.scrollTo(callback, errorCallback)`
- `Footnote.scrollToPosition(position, callback, errorCallback)`

## Notes

- `TXTextControl.footnotes` returns all footnotes in the main text and is read-only as a property.
- Add callbacks use `AddFootnoteCallback` and return `FootnoteCallbackData`.
- Footnote event callbacks use `FootnoteCallback` and receive a `FootnoteEventArgs` object.
- Use only documented footnote event names for `eventName`.
