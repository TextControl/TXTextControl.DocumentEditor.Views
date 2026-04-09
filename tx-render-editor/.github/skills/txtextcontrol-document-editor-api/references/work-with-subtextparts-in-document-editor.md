# SubTextParts

Add, inspect, navigate, rename, and remove subtextparts in the TX Text Control Document Editor.

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

    function withSubTextParts(onReady) {
        TXTextControl.textParts.getMainText(function(mainText) {
            onReady(mainText.subTextParts);
        }, onTxError);
    }
</script>
```

---

## SubTextPart Events

SubTextPart-related event callbacks use `SubTextPartEventArgs`.

Example: inspect the payload of the documented `subTextPartEntered` event.

```javascript
TXTextControl.addEventListener("subTextPartEntered", function(args) {
    console.log("SubTextPart event args:", args);
    console.log({
        id: args.id,
        start: args.start,
        length: args.length,
        name: args.name,
        number: args.number,
        nestedLevel: args.nestedLevel
    });
});
```

Return object shape for SubTextPart event callbacks:

```javascript
{
    id: 25,
    start: 120,
    length: 18,
    name: "AddressBlock",
    number: 2,
    nestedLevel: 1
}
```

---

## Add a SubTextPart

```javascript
withSubTextParts(function(subTextParts) {
    subTextParts.add("AddressBlock", 25, 120, 18, function(response, addResult) {
        console.log("Add result:", addResult);

        response.subTextPart.getName(function(name) {
            console.log("Added subtextpart:", { name: name, id: 25 });
        }, onTxError);
    }, onTxError);
});
```

Placeholders:
- `"AddressBlock"` -> `{subTextPartName}`
- `25` -> `{subTextPartId}`
- `120` -> `{startPosition}`
- `18` -> `{length}`

Notes:
- `start` is one-based.
- Use `0` for `id` when the subtextpart should not have an identifier.
- `response.subTextPart` is the added `SubTextPart` object.

---

## Get Current SubTextPart

Read the subtextpart at the current text input position.

```javascript
withSubTextParts(function(subTextParts) {
    subTextParts.getItem(function(subTextPart) {
        if (!subTextPart) {
            console.warn("No subtextpart at current text input position.");
            return;
        }

        subTextPart.getID(function(id) {
            subTextPart.getName(function(name) {
                subTextPart.getText(function(text) {
                    console.log("Current subtextpart:", {
                        id: id,
                        name: name,
                        text: text
                    });
                }, onTxError);
            }, onTxError);
        }, onTxError);
    }, onTxError);
});
```

---

## Inspect SubTextPart Metadata

```javascript
withSubTextParts(function(subTextParts) {
    subTextParts.getItem(function(subTextPart) {
        if (!subTextPart) {
            console.warn("No subtextpart at current text input position.");
            return;
        }

        subTextPart.getID(function(id) {
            subTextPart.getName(function(name) {
                subTextPart.getStart(function(start) {
                    subTextPart.getLength(function(length) {
                        subTextPart.getNumber(function(number) {
                            subTextPart.getNestedLevel(function(nestedLevel) {
                                console.log("SubTextPart metadata:", {
                                    id: id,
                                    name: name,
                                    start: start,
                                    length: length,
                                    number: number,
                                    nestedLevel: nestedLevel
                                });
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

---

## Navigate to the Current SubTextPart

```javascript
withSubTextParts(function(subTextParts) {
    subTextParts.getItem(function(subTextPart) {
        if (!subTextPart) {
            console.warn("No subtextpart at current text input position.");
            return;
        }

        subTextPart.scrollTo(function(scrolled) {
            console.log("Scrolled to subtextpart:", scrolled);
        }, onTxError);
    }, onTxError);
});
```

---

## Rename the Current SubTextPart

```javascript
withSubTextParts(function(subTextParts) {
    subTextParts.getItem(function(subTextPart) {
        if (!subTextPart) {
            console.warn("No subtextpart at current text input position.");
            return;
        }

        subTextPart.setName("ShippingAddress", function() {
            console.log("SubTextPart name updated.");
        }, onTxError);
    }, onTxError);
});
```

Placeholders:
- `"ShippingAddress"` -> `{newSubTextPartName}`

---

## Change the Current SubTextPart ID

```javascript
withSubTextParts(function(subTextParts) {
    subTextParts.getItem(function(subTextPart) {
        if (!subTextPart) {
            console.warn("No subtextpart at current text input position.");
            return;
        }

        subTextPart.setID(42, function() {
            console.log("SubTextPart ID updated.");
        }, onTxError);
    }, onTxError);
});
```

Placeholders:
- `42` -> `{newSubTextPartId}`

---

## List SubTextParts

Get the subtextpart count and enumerate all items in the main text.

```javascript
withSubTextParts(function(subTextParts) {
    subTextParts.getCount(function(count) {
        console.log("Total subtextparts:", count);
    }, onTxError);

    subTextParts.forEach(function(subTextPart) {
        subTextPart.getID(function(id) {
            subTextPart.getName(function(name) {
                console.log("SubTextPart item:", {
                    id: id,
                    name: name
                });
            }, onTxError);
        }, onTxError);
    }, onTxError);
});
```

---

## Get a SubTextPart by Index

```javascript
withSubTextParts(function(subTextParts) {
    subTextParts.elementAt(0, function(subTextPart) {
        if (!subTextPart) {
            console.warn("No subtextpart at index:", 0);
            return;
        }

        subTextPart.getName(function(name) {
            console.log("SubTextPart at index:", { index: 0, name: name });
        }, onTxError);
    }, onTxError);
});
```

Placeholders:
- `0` -> `{zeroBasedIndex}`

---

## Remove the Current SubTextPart and Keep Visible Text

```javascript
withSubTextParts(function(subTextParts) {
    subTextParts.getItem(function(subTextPart) {
        if (!subTextPart) {
            console.warn("No subtextpart at current text input position.");
            return;
        }

        subTextParts.remove(subTextPart, true, true, function(removed) {
            console.log("SubTextPart removed and text preserved:", removed);
        }, onTxError);
    }, onTxError);
});
```

Notes:
- `keepText = true` removes the subtextpart without deleting the visible text.
- `keepNested = true` preserves nested subtextparts when visible text is kept.

---

## Remove the Current SubTextPart and Delete Its Text

```javascript
withSubTextParts(function(subTextParts) {
    subTextParts.getItem(function(subTextPart) {
        if (!subTextPart) {
            console.warn("No subtextpart at current text input position.");
            return;
        }

        subTextParts.remove(subTextPart, false, false, function(removed) {
            console.log("SubTextPart and text removed:", removed);
        }, onTxError);
    }, onTxError);
});
```

---

## Clear All SubTextParts

```javascript
withSubTextParts(function(subTextParts) {
    subTextParts.clear(function() {
        console.log("All subtextparts removed.");
    }, onTxError);
});
```

---

## API Mapping
- `TXTextControl.addEventListener(eventName, callback)`
- `TXTextControl.textParts.getMainText(callback, errorCallback)`
- `FormattedText.subTextParts`
- `SubTextPartCollection.add(name, id, start, length, callback, errorCallback)`
- `SubTextPartCollection.getItem(callback, errorCallback)`
- `SubTextPartCollection.remove(subTextPart, keepText, keepNested, callback, errorCallback)`
- `SubTextPartCollection.clear(callback, errorCallback)`
- `Collection.getCount(callback, errorCallback)`
- `Collection.forEach(callback, errorCallback)`
- `Collection.elementAt(index, callback, errorCallback)`
- `SubTextPart.getID(callback, errorCallback)`
- `SubTextPart.getName(callback, errorCallback)`
- `SubTextPart.getText(callback, errorCallback)`
- `SubTextPart.getStart(callback, errorCallback)`
- `SubTextPart.getLength(callback, errorCallback)`
- `SubTextPart.getNumber(callback, errorCallback)`
- `SubTextPart.getNestedLevel(callback, errorCallback)`
- `SubTextPart.scrollTo(callback, errorCallback)`
- `SubTextPart.setName(name, callback, errorCallback)`
- `SubTextPart.setID(id, callback, errorCallback)`

## Notes

- `SubTextPartCollection.getItem` reads the subtextpart at the current text input position.
- `SubTextPartEventArgs` contains `id`, `start`, `length`, `name`, `number`, and `nestedLevel`.
- `SubTextPartCollection.add` uses an `AddSubTextPartCallback`, which receives `response.subTextPart` and an add result value.
- Use only the APIs and object paths listed in this task.