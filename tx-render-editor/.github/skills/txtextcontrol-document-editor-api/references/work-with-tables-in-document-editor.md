# Tables

Insert, inspect, list, select, and remove tables in the TX Text Control Document Editor.

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

    function withTables(onReady) {
        TXTextControl.textParts.getMainText(function(mainText) {
            onReady(mainText.tables);
        }, onTxError);
    }
</script>
```

---

## Table Events

Attach table listeners with `TXTextControl.addEventListener(eventName, callback)`.

Documented table events include:

- `tableCreated`
- `tableDeleted`

Example: inspect the created or deleted table payload.

```javascript
TXTextControl.addEventListener("tableCreated", function(args) {
    console.log("Table created event args:", args);
    console.log({
        id: args.table.id,
        nestedLevel: args.table.nestedLevel
    });
});

TXTextControl.addEventListener("tableDeleted", function(args) {
    console.log("Table deleted event args:", args);
    console.log({
        id: args.table.id,
        nestedLevel: args.table.nestedLevel
    });
});
```

Return object for `tableCreated` and `tableDeleted`:

```javascript
{
    table: {
        id: 25,
        nestedLevel: 1
    }
}
```

---

## Check Whether a Table Can Be Added

```javascript
withTables(function(tables) {
    tables.getCanAdd(function(canAdd) {
        console.log("Can add table at current input position:", canAdd);
    }, onTxError);
});
```

---

## Insert a Table

```javascript
withTables(function(tables) {
    tables.add(3, 4, 25, function(added) {
        console.log("Table added:", added);
    }, onTxError);
});
```

Placeholders:
- `3` -> `{rowCount}`
- `4` -> `{columnCount}`
- `25` -> `{tableId}`

Notes:
- Omit the `id` argument when TX Text Control should assign the table ID.

---

## Get Current Table

Read the table at the current text input position.

```javascript
withTables(function(tables) {
    tables.getItem(function(table) {
        if (!table) {
            console.warn("No table at current text input position.");
            return;
        }

        table.getID(function(id) {
            table.getNestedLevel(function(nestedLevel) {
                table.getDescriptiveText(function(descriptiveText) {
                    console.log("Current table:", {
                        id: id,
                        nestedLevel: nestedLevel,
                        descriptiveText: descriptiveText
                    });
                }, onTxError);
            }, onTxError);
        }, onTxError);
    }, onTxError);
});
```

---

## Get Table by ID

```javascript
withTables(function(tables) {
    tables.getItem(function(table) {
        if (!table) {
            console.warn("Table not found for ID:", 25);
            return;
        }

        table.getNestedLevel(function(nestedLevel) {
            console.log("Found table:", {
                id: 25,
                nestedLevel: nestedLevel
            });
        }, onTxError);
    }, onTxError, 25);
});
```

Placeholders:
- `25` -> `{tableId}`

---

## List Tables

Get the table count and enumerate all tables in the main text.

```javascript
withTables(function(tables) {
    tables.getCount(function(count) {
        console.log("Total tables:", count);
    }, onTxError);

    tables.forEach(function(table) {
        table.getID(function(id) {
            table.getNestedLevel(function(nestedLevel) {
                console.log("Table item:", {
                    id: id,
                    nestedLevel: nestedLevel
                });
            }, onTxError);
        }, onTxError);
    }, onTxError);
});
```

---

## Select the Current Table

```javascript
withTables(function(tables) {
    tables.getItem(function(table) {
        if (!table) {
            console.warn("No table at current text input position.");
            return;
        }

        table.select(function() {
            console.log("Table selected.");
        }, onTxError);
    }, onTxError);
});
```

---

## Set or Read Table Metadata

Set a descriptive text value:

```javascript
withTables(function(tables) {
    tables.getItem(function(table) {
        if (!table) {
            console.warn("No table at current text input position.");
            return;
        }

        table.setDescriptiveText("Invoice line items", function() {
            console.log("Table descriptive text updated.");
        }, onTxError);
    }, onTxError);
});
```

Set a table ID:

```javascript
withTables(function(tables) {
    tables.getItem(function(table) {
        if (!table) {
            console.warn("No table at current text input position.");
            return;
        }

        table.setID(25, function() {
            console.log("Table ID updated.");
        }, onTxError);
    }, onTxError);
});
```

Placeholders:
- `"Invoice line items"` -> `{descriptiveText}`
- `25` -> `{tableId}`

---

## Toggle Table Grid Lines

Read the current grid-line setting:

```javascript
withTables(function(tables) {
    tables.getGridLines(function(showGridLines) {
        console.log("Show table grid lines:", showGridLines);
    }, onTxError);
});
```

Enable grid lines:

```javascript
withTables(function(tables) {
    tables.setGridLines(true, function() {
        console.log("Table grid lines enabled.");
    }, onTxError);
});
```

Disable grid lines:

```javascript
withTables(function(tables) {
    tables.setGridLines(false, function() {
        console.log("Table grid lines disabled.");
    }, onTxError);
});
```

---

## Remove Current Table

```javascript
withTables(function(tables) {
    tables.removeAtInputPosition(function(removed) {
        console.log("Current table removed:", removed);
    }, onTxError);
});
```

---

## Remove Table by ID

```javascript
withTables(function(tables) {
    tables.removeById(25, function(removed) {
        console.log("Table removed by ID:", removed);
    }, onTxError);
});
```

Placeholders:
- `25` -> `{tableId}`

---

## Get a Table by Index

```javascript
withTables(function(tables) {
    tables.elementAt(0, function(table) {
        if (!table) {
            console.warn("No table at index:", 0);
            return;
        }

        table.getID(function(id) {
            console.log("Table at index:", { index: 0, id: id });
        }, onTxError);
    }, onTxError);
});
```

Placeholders:
- `0` -> `{zeroBasedIndex}`

---

## API Mapping
- `TXTextControl.addEventListener(eventName, callback)`
- `TXTextControl.textParts.getMainText(callback, errorCallback)`
- `FormattedText.tables`
- `TableCollection.add(rows, columns, id, callback, errorCallback)`
- `TableCollection.getCanAdd(callback, errorCallback)`
- `TableCollection.getGridLines(callback, errorCallback)`
- `TableCollection.setGridLines(value, callback, errorCallback)`
- `TableBaseCollection.getItem(callback, errorCallback, id)`
- `TableBaseCollection.removeAtInputPosition(callback, errorCallback)`
- `TableBaseCollection.removeById(id, callback, errorCallback)`
- `Collection.getCount(callback, errorCallback)`
- `Collection.forEach(callback, errorCallback)`
- `Collection.elementAt(index, callback, errorCallback)`
- `Table.getID(callback, errorCallback)`
- `Table.getNestedLevel(callback, errorCallback)`
- `Table.getDescriptiveText(callback, errorCallback)`
- `Table.select(callback, errorCallback)`
- `Table.setDescriptiveText(text, callback, errorCallback)`
- `Table.setID(id, callback, errorCallback)`

## Notes

- `getItem` without `id` reads the table at the current text input position.
- `getItem` with `id` returns `null` when no table exists for that ID.
- `TableEventArgs` exposes `args.table`, which contains a `TableInfo` object with `id` and `nestedLevel`.
- `nestedLevel` can be used to distinguish top-level tables from nested tables.
- Use only the APIs and object paths listed in this task.