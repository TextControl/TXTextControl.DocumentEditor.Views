# Form Fields

Insert, inspect, list, and remove form fields in the TX Text Control Document Editor.

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

    function withFormFields(onReady) {
        TXTextControl.textParts.getMainText(function(mainText) {
            onReady(mainText.formFields);
        }, onTxError);
    }
</script>
```

---

## Form Field Events

Attach form-field listeners with `TXTextControl.addEventListener(eventName, callback)`.

Documented form-field events include:

- `formFieldCheckChanged`
- `formFieldDateChanged`
- `formFieldSelectionChanged`

Example: check form field changed.

```javascript
TXTextControl.addEventListener("formFieldCheckChanged", function(args) {
    console.log("Check field event args:", args);
    console.log({
        id: args.formField.id,
        name: args.formField.name,
        start: args.formField.start,
        text: args.formField.text,
        checked: args.formField.checked
    });
});
```

Return object for `formFieldCheckChanged`:

```javascript
{
    formField: {
        id: 10,
        name: "Approved",
        start: 42,
        text: "",
        checked: true
    }
}
```

Example: date form field changed.

```javascript
TXTextControl.addEventListener("formFieldDateChanged", function(args) {
    console.log("Date field event args:", args);
    console.log({
        id: args.formField.id,
        name: args.formField.name,
        start: args.formField.start,
        text: args.formField.text,
        date: args.formField.date,
        dateFormat: args.formField.dateFormat,
        emptyWidth: args.formField.emptyWidth
    });
});
```

Return object for `formFieldDateChanged`:

```javascript
{
    formField: {
        id: 11,
        name: "DueDate",
        start: 55,
        text: "01/01/2026",
        date: 1767225600,
        dateFormat: "MM/dd/yyyy",
        emptyWidth: 1440
    }
}
```

Example: selection form field changed.

```javascript
TXTextControl.addEventListener("formFieldSelectionChanged", function(args) {
    console.log("Selection field event args:", args);
    console.log({
        id: args.formField.id,
        name: args.formField.name,
        start: args.formField.start,
        text: args.formField.text,
        selectedIndex: args.formField.selectedIndex,
        items: args.formField.items,
        emptyWidth: args.formField.emptyWidth
    });
});
```

Return object for `formFieldSelectionChanged`:

```javascript
{
    formField: {
        id: 12,
        name: "Priority",
        start: 70,
        text: "High",
        selectedIndex: 2,
        items: ["Low", "Medium", "High"],
        emptyWidth: 1440
    }
}
```

---

## Insert Text Form Field

```javascript
withFormFields(function(fields) {
    fields.addTextFormField(1440, function(textField) {
        textField.setText("Sample text", function() {
            console.log("Text form field added.");
        }, onTxError);
    }, onTxError);
});
```

Placeholders:
- `1440` -> `{emptyWidthTwips}`
- `"Sample text"` -> `{initialTextValue}`

---

## Insert Check Form Field

```javascript
withFormFields(function(fields) {
    fields.addCheckFormField(true, function(checkField) {
        checkField.setChecked(true, function() {
            console.log("Check form field added.");
        }, onTxError);
    }, onTxError);
});
```

Placeholders:
- `true` -> `{initialCheckedValue}`

---

## Insert Selection Form Field

```javascript
withFormFields(function(fields) {
    fields.addSelectionFormField(1440, function(selectionField) {
        selectionField.setItems(["One", "Two", "Three"], function() {
            selectionField.setSelectedIndex(0, function() {
                console.log("Selection form field added.");
            }, onTxError);
        }, onTxError);
    }, onTxError);
});
```

Placeholders:
- `1440` -> `{emptyWidthTwips}`
- `["One", "Two", "Three"]` -> `{selectionItems}`
- `0` -> `{selectedIndex}`

---

## Insert Date Form Field

```javascript
withFormFields(function(fields) {
    fields.addDateFormField(1440, function(dateField) {
        dateField.setDate(1767225600, function() {
            console.log("Date form field added.");
        }, onTxError);
    }, onTxError);
});
```

Placeholders:
- `1440` -> `{emptyWidthTwips}`
- `1767225600` -> `{unixTime}`

---

## Get Form Field Names and Count

```javascript
withFormFields(function(fields) {
    fields.getCount(function(count) {
        console.log("Total form fields:", count);
    }, onTxError);

    fields.forEach(function(field) {
        field.getID(function(id) {
            field.getName(function(name) {
                console.log("Form field:", { id: id, name: name });
            }, onTxError);
        }, onTxError);
    }, onTxError);
});
```

---

## Get Current Form Field

```javascript
withFormFields(function(fields) {
    fields.getItem(function(field) {
        if (!field) {
            console.warn("No form field at current text input position.");
            return;
        }

        field.getID(function(id) {
            field.getName(function(name) {
                field.getText(function(text) {
                    console.log("Current form field:", { id: id, name: name, text: text });
                }, onTxError);
            }, onTxError);
        }, onTxError);
    }, onTxError);
});
```

---

## Get Form Field by ID

```javascript
withFormFields(function(fields) {
    fields.getItem(function(field) {
        if (!field) {
            console.warn("Form field not found for ID:", 10);
            return;
        }

        field.getName(function(name) {
            console.log("Found form field:", { id: 10, name: name });
        }, onTxError);
    }, onTxError, 10);
});
```

Placeholders:
- `10` -> `{formFieldId}`

---

## Remove Current Form Field

```javascript
withFormFields(function(fields) {
    fields.getItem(function(field) {
        if (!field) {
            console.warn("No form field at current text input position.");
            return;
        }

        fields.remove(field, function(removed) {
            console.log("Current form field removed:", removed);
        }, onTxError);
    }, onTxError);
});
```

---

## Remove Form Field by ID

```javascript
withFormFields(function(fields) {
    fields.getItem(function(field) {
        if (!field) {
            console.warn("Form field not found for ID:", 10);
            return;
        }

        fields.remove(field, function(removed) {
            console.log("Form field removed by ID:", removed);
        }, onTxError);
    }, onTxError, 10);
});
```

Placeholders:
- `10` -> `{formFieldId}`

---

## API Mapping
- `TXTextControl.textParts.getMainText(callback, errorCallback)`
- `FormattedText.formFields`
- `FormFieldCollection.addTextFormField(emptyWidth, callback, errorCallback)`
- `FormFieldCollection.addCheckFormField(isChecked, callback, errorCallback)`
- `FormFieldCollection.addSelectionFormField(emptyWidth, callback, errorCallback)`
- `FormFieldCollection.addDateFormField(emptyWidth, callback, errorCallback)`
- `FormFieldCollection.getItem(callback, errorCallback, id)`
- `FormFieldCollection.remove(formField, callback, errorCallback)`
- `Collection.getCount(callback, errorCallback)`
- `Collection.forEach(callback, errorCallback)`
- `TextField.getID(callback, errorCallback)`
- `TextField.getName(callback, errorCallback)`
- `TextField.getText(callback, errorCallback)`
- `TextField.setText(text, callback, errorCallback)`
- `CheckFormField.setChecked(isChecked, callback, errorCallback)`
- `SelectionFormField.setItems(items, callback, errorCallback)`
- `SelectionFormField.setSelectedIndex(index, callback, errorCallback)`
- `DateFormField.setDate(unixTime, callback, errorCallback)`

## Notes

- `getItem` without `id` reads the form field at the current text input position.
- `getItem` with `id` returns `null` when no field exists for that ID.
- Use only the APIs and object paths listed in this task.
- Form-field event callbacks expose `args.formField`, which is a `CheckFormFieldInfo`, `DateFormFieldInfo`, or `SelectionFormFieldInfo` object depending on the event.
