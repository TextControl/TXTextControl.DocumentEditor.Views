# Track Changes

Enable, inspect, navigate, and accept/reject tracked changes in the TX Text Control Document Editor.

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

    function withTrackedChangeCollection(onReady) {
        TXTextControl.textParts.getMainText(function(mainText) {
            onReady(mainText.trackedChanges);
        }, onTxError);
    }
</script>
```

---

## Tracked Change Events

Attach tracked-change listeners with `TXTextControl.addEventListener(eventName, callback)`.

Documented tracked-change events include:

- `trackedChangeChanged`
- `trackedChangeStateChanged`

Example: inspect the changed tracked-change payload.

```javascript
TXTextControl.addEventListener("trackedChangeChanged", function(args) {
    console.log("Tracked change event args:", args);
    console.log({
        number: args.trackedChange.number,
        start: args.trackedChange.start,
        length: args.trackedChange.length,
        changeKind: args.trackedChange.changeKind,
        changeTime: args.trackedChange.changeTime,
        userName: args.trackedChange.userName
    });
});
```

Return object for `trackedChangeChanged` and `trackedChangeStateChanged`:

```javascript
{
    trackedChange: {
        number: 3,
        start: 120,
        length: 8,
        changeKind: 1,
        changeTime: 1767225600,
        userName: "Tim Typer"
    }
}
```

---

## Enable Track Changes

```javascript
TXTextControl.setIsTrackChangesEnabled(true, function() {
    console.log("Track changes enabled.");
}, onTxError);
```

---

## Disable Track Changes

```javascript
TXTextControl.setIsTrackChangesEnabled(false, function() {
    console.log("Track changes disabled.");
}, onTxError);
```

---

## Get Track Changes Status

```javascript
TXTextControl.getIsTrackChangesEnabled(function(isEnabled) {
    console.log("Track changes enabled:", isEnabled);
}, onTxError);
```

---

## Get Current Tracked Change

Get the tracked change at the current text input position.

```javascript
withTrackedChangeCollection(function(trackedChanges) {
    trackedChanges.getItem(function(change) {
        if (!change) {
            console.warn("No tracked change at current input position.");
            return;
        }

        change.getText(function(text) {
            console.log("Current tracked change text:", text);
        }, onTxError);
    }, onTxError);
});
```

---

## Navigate Tracked Changes

Move to the next tracked change:

```javascript
withTrackedChangeCollection(function(trackedChanges) {
    trackedChanges.getItem(function(change) {
        if (!change) {
            console.warn("No next tracked change found.");
            return;
        }

        change.getStart(function(start) {
            console.log("Next tracked change starts at:", start);
        }, onTxError);
    }, true, onTxError);
});
```

Move to the previous tracked change:

```javascript
withTrackedChangeCollection(function(trackedChanges) {
    trackedChanges.getItem(function(change) {
        if (!change) {
            console.warn("No previous tracked change found.");
            return;
        }

        change.getStart(function(start) {
            console.log("Previous tracked change starts at:", start);
        }, onTxError);
    }, false, onTxError);
});
```

---

## Inspect Tracked Change Metadata

Read common metadata from a tracked change.

```javascript
withTrackedChangeCollection(function(trackedChanges) {
    trackedChanges.getItem(function(change) {
        if (!change) {
            console.warn("No tracked change available.");
            return;
        }

        change.getChangeKind(function(changeKind) {
            change.getUserName(function(userName) {
                change.getText(function(text) {
                    change.getStart(function(start) {
                        change.getLength(function(length) {
                            change.getChangeTime(function(unixTimestamp) {
                                console.log("Tracked change:", {
                                    changeKind: changeKind,
                                    userName: userName,
                                    text: text,
                                    start: start,
                                    length: length,
                                    changeTime: unixTimestamp
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
- `changeTime` is a Unix timestamp.

---

## Accept Current Tracked Change

```javascript
withTrackedChangeCollection(function(trackedChanges) {
    trackedChanges.getItem(function(change) {
        if (!change) {
            console.warn("No tracked change to accept.");
            return;
        }

        trackedChanges.remove(change, true, function(removed) {
            console.log("Tracked change accepted:", removed);
        }, onTxError);
    }, onTxError);
});
```

---

## Reject Current Tracked Change

```javascript
withTrackedChangeCollection(function(trackedChanges) {
    trackedChanges.getItem(function(change) {
        if (!change) {
            console.warn("No tracked change to reject.");
            return;
        }

        trackedChanges.remove(change, false, function(removed) {
            console.log("Tracked change rejected:", removed);
        }, onTxError);
    }, onTxError);
});
```

---

## List Tracked Changes

Get tracked-change count and enumerate items.

```javascript
withTrackedChangeCollection(function(trackedChanges) {
    trackedChanges.getCount(function(count) {
        console.log("Total tracked changes:", count);
    }, onTxError);

    trackedChanges.forEach(function(change) {
        change.getChangeKind(function(changeKind) {
            change.getUserName(function(userName) {
                change.getText(function(text) {
                    console.log("Tracked change item:", {
                        changeKind: changeKind,
                        userName: userName,
                        text: text
                    });
                }, onTxError);
            }, onTxError);
        }, onTxError);
    }, onTxError);
});
```

---

## API Mapping
- `TXTextControl.setIsTrackChangesEnabled(value, callback, errorCallback)`
- `TXTextControl.getIsTrackChangesEnabled(callback, errorCallback)`
- `TXTextControl.textParts.getMainText(callback, errorCallback)`
- `FormattedText.trackedChanges`
- `TrackedChangeCollection.getItem(callback, next, errorCallback)`
- `TrackedChangeCollection.remove(trackedChange, accept, callback, errorCallback)`
- `Collection.getCount(callback, errorCallback)`
- `Collection.forEach(callback, errorCallback)`
- `TrackedChange.getChangeKind(callback, errorCallback)`
- `TrackedChange.getUserName(callback, errorCallback)`
- `TrackedChange.getText(callback, errorCallback)`
- `TrackedChange.getStart(callback, errorCallback)`
- `TrackedChange.getLength(callback, errorCallback)`
- `TrackedChange.getChangeTime(callback, errorCallback)`

## Notes

- Use only the APIs and object paths shown in this task.
- For `TrackedChangeCollection.remove`, `accept: true` accepts the change, `accept: false` rejects it.
- Tracked-change event callbacks expose `TrackedChangeEventArgs`, which provide the `trackedChange` object shown above.
