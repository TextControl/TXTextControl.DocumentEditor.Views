# Headers and Footers

Add, inspect, activate, configure, and remove headers and footers in the TX Text Control Document Editor.

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

    function withHeadersAndFooters(onReady) {
        onReady(TXTextControl.headersAndFooters);
    }
</script>
```

---

## Header and Footer Events

Attach header/footer-related listeners with `TXTextControl.addEventListener(eventName, callback)`.
Header/footer event callbacks use `HeaderFooterCallback` and receive one `HeaderFooterEventArgs` object.

```javascript
TXTextControl.addEventListener("{headerFooterEventName}", function(args) {
    console.log("HeaderFooter event args:", args);
    console.log({
        connectedToPrevious: args.connectedToPrevious,
        distance: args.distance,
        type: args.type
    });
});
```

Placeholders:
- `"{headerFooterEventName}"` -> `{documentedHeaderFooterEventName}`

Return object shape for header/footer event callbacks:

```javascript
{
    connectedToPrevious: true,
    distance: 720,
    type: "Header"
}
```

Notes:
- Use only documented header/footer event names for `eventName`.

---

## Add Headers or Footers

```javascript
withHeadersAndFooters(function(headersAndFooters) {
    headersAndFooters.add({headerFooterTypeValue}, function(added) {
        console.log("Header/footer added:", added);
    }, onTxError);
});
```

Placeholders:
- `{headerFooterTypeValue}` -> `{TXTextControl.HeaderFooterType value or combination}`

Notes:
- `add` receives a `HeaderFooterType` value and optional callback/error callback.
- To add more than one kind, use a documented combination of `HeaderFooterType` values.

---

## Get a Specific Header or Footer

```javascript
withHeadersAndFooters(function(headersAndFooters) {
    headersAndFooters.getItem({headerFooterTypeValue}, function(headerFooter) {
        if (!headerFooter) {
            console.warn("Requested header/footer was not found.");
            return;
        }

        headerFooter.getType(function(type) {
            headerFooter.getDistance(function(distance) {
                headerFooter.getConnectedToPrevious(function(connectedToPrevious) {
                    console.log("Header/footer:", {
                        type: type,
                        distance: distance,
                        connectedToPrevious: connectedToPrevious
                    });
                }, onTxError);
            }, onTxError);
        }, onTxError);
    }, onTxError);
});
```

Placeholders:
- `{headerFooterTypeValue}` -> `{TXTextControl.HeaderFooterType value}`

---

## Activate or Deactivate a Header/Footer

```javascript
withHeadersAndFooters(function(headersAndFooters) {
    headersAndFooters.getItem({headerFooterTypeValue}, function(headerFooter) {
        if (!headerFooter) {
            console.warn("Requested header/footer was not found.");
            return;
        }

        headerFooter.activate(function(activated) {
            console.log("Header/footer activated:", activated);
        }, onTxError);
    }, onTxError);
});
```

```javascript
withHeadersAndFooters(function(headersAndFooters) {
    headersAndFooters.getItem({headerFooterTypeValue}, function(headerFooter) {
        if (!headerFooter) {
            console.warn("Requested header/footer was not found.");
            return;
        }

        headerFooter.deactivate(function(deactivated) {
            console.log("Header/footer deactivated:", deactivated);
        }, onTxError);
    }, onTxError);
});
```

---

## Update Header/Footer Settings

```javascript
withHeadersAndFooters(function(headersAndFooters) {
    headersAndFooters.getItem({headerFooterTypeValue}, function(headerFooter) {
        if (!headerFooter) {
            console.warn("Requested header/footer was not found.");
            return;
        }

        headerFooter.setDistance(720, function() {
            headerFooter.setConnectedToPrevious(true, function() {
                console.log("Header/footer settings updated.");
            }, onTxError);
        }, onTxError);
    }, onTxError);
});
```

Placeholders:
- `{headerFooterTypeValue}` -> `{TXTextControl.HeaderFooterType value}`
- `720` -> `{distanceInTwips}`
- `true` -> `{connectedToPrevious}`

Notes:
- `distance` is specified in twips.

---

## List Headers and Footers

```javascript
withHeadersAndFooters(function(headersAndFooters) {
    headersAndFooters.getCount(function(count) {
        console.log("Total headers/footers:", count);
    }, onTxError);

    headersAndFooters.forEach(function(headerFooter) {
        headerFooter.getType(function(type) {
            headerFooter.getDistance(function(distance) {
                console.log("Header/footer item:", {
                    type: type,
                    distance: distance
                });
            }, onTxError);
        }, onTxError);
    }, onTxError);
});
```

---

## Get by Index

```javascript
withHeadersAndFooters(function(headersAndFooters) {
    headersAndFooters.elementAt(0, function(headerFooter) {
        if (!headerFooter) {
            console.warn("No header/footer at index:", 0);
            return;
        }

        headerFooter.getType(function(type) {
            console.log("Header/footer at index:", {
                index: 0,
                type: type
            });
        }, onTxError);
    }, onTxError);
});
```

Placeholders:
- `0` -> `{zeroBasedIndex}`

---

## Remove Headers or Footers

```javascript
withHeadersAndFooters(function(headersAndFooters) {
    headersAndFooters.remove({headerFooterTypeValue}, function(removed) {
        console.log("Header/footer removed:", removed);
    }, onTxError);
});
```

Placeholders:
- `{headerFooterTypeValue}` -> `{TXTextControl.HeaderFooterType value or combination}`

---

## API Mapping
- `TXTextControl.headersAndFooters`
- `TXTextControl.addEventListener(eventName, callback)`
- `HeaderFooterCollection.add(headerFooterType, callback, errorCallback)`
- `HeaderFooterCollection.getItem(headerFooterType, callback, errorCallback)`
- `HeaderFooterCollection.remove(headerFooterType, callback, errorCallback)`
- `Collection.getCount(callback, errorCallback)`
- `Collection.forEach(callback, errorCallback)`
- `Collection.elementAt(index, callback, errorCallback)`
- `HeaderFooter.activate(callback, errorCallback)`
- `HeaderFooter.deactivate(callback, errorCallback)`
- `HeaderFooter.getConnectedToPrevious(callback, errorCallback)`
- `HeaderFooter.setConnectedToPrevious(connectedToPrevious, callback, errorCallback)`
- `HeaderFooter.getDistance(callback, errorCallback)`
- `HeaderFooter.setDistance(distance, callback, errorCallback)`
- `HeaderFooter.getType(callback, errorCallback)`

## Notes

- `TXTextControl.headersAndFooters` returns a `HeaderFooterCollection` for the document.
- `HeaderFooterEventArgs` exposes `connectedToPrevious`, `distance`, and `type`.
- `HeaderFooterCollection.getItem` uses `RequestHeaderFooterCallback`, which receives one `HeaderFooter` object.
- Use only the APIs and object paths listed in this task.
