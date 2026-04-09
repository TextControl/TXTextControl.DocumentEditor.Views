# Application Fields

Insert, inspect, update, list, and remove application fields in the TX Text Control Document Editor.

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

    function withApplicationFields(onReady) {
        TXTextControl.textParts.getMainText(function(mainText) {
            onReady(mainText.applicationFields);
        }, onTxError);
    }
</script>
```

---

## Application Field Events

Application fields are exposed through the documented generic text-field events.

Attach listeners with `TXTextControl.addEventListener(eventName, callback)` and filter for
`args.textField.type === "APPLICATIONFIELD"`.

Documented text-field events include:

- `textFieldClicked`
- `textFieldEntered`
- `textFieldLeft`

Example: inspect clicked application fields.

```javascript
TXTextControl.addEventListener("textFieldClicked", function(args) {
    if (!args.textField || args.textField.type !== "APPLICATIONFIELD") {
        return;
    }

    console.log("Application field event args:", args);
    console.log({
        id: args.textField.id,
        name: args.textField.name,
        start: args.textField.start,
        length: args.textField.length,
        text: args.textField.text,
        type: args.textField.type
    });
});
```

Return object shape for text-field event callbacks:

```javascript
{
    textField: {
        id: 18,
        name: "CustomerName",
        start: 96,
        length: 12,
        text: "Contoso Ltd.",
        type: "APPLICATIONFIELD"
    }
}
```

Notes:
- `args.textField` is static field info, not the live `ApplicationField` object.
- Use `withApplicationFields(...); applicationFields.getItem(...)` to resolve the current live field object.

---

## Check Whether an Application Field Can Be Added

```javascript
withApplicationFields(function(applicationFields) {
    applicationFields.getCanAdd(function(canAdd) {
        console.log("Can add application field at current input position:", canAdd);
    }, onTxError);
});
```

---

## Insert an Application Field

This example inserts a Microsoft Word merge field.

```javascript
withApplicationFields(function(applicationFields) {
    applicationFields.add(
        TXTextControl.ApplicationFieldFormat.MSWord,
        "MERGEFIELD",
        "CustomerName",
        ["CustomerName", "\\* MERGEFORMAT"],
        function(applicationField) {
            applicationField.getID(function(id) {
                console.log("Application field added:", {
                    id: id,
                    typeName: "MERGEFIELD"
                });
            }, onTxError);
        },
        onTxError
    );
});
```

Placeholders:
- `TXTextControl.ApplicationFieldFormat.MSWord` -> `{applicationFieldFormat}`
- `"MERGEFIELD"` -> `{typeName}`
- `"CustomerName"` -> `{visibleText}`
- `["CustomerName", "\\* MERGEFORMAT"]` -> `{parameters}`

Notes:
- `parameters` is an array of strings.
- For a Word `MERGEFIELD`, the first parameter is typically the field name and the remaining items are switches.
- Use `null` for `parameters` when the field type has no parameters.

---

## Get the Current Application Field

Read the application field at the current text input position.

```javascript
withApplicationFields(function(applicationFields) {
    applicationFields.getItem(function(applicationField) {
        if (!applicationField) {
            console.warn("No application field at current text input position.");
            return;
        }

        applicationField.getID(function(id) {
            applicationField.getName(function(name) {
                applicationField.getText(function(text) {
                    console.log("Current application field:", {
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

## Inspect Application Field Metadata

```javascript
withApplicationFields(function(applicationFields) {
    applicationFields.getItem(function(applicationField) {
        if (!applicationField) {
            console.warn("No application field at current text input position.");
            return;
        }

        applicationField.getID(function(id) {
            applicationField.getName(function(name) {
                applicationField.getStart(function(start) {
                    applicationField.getText(function(text) {
                        applicationField.getFormat(function(format) {
                            applicationField.getTypeName(function(typeName) {
                                applicationField.getParameters(function(parameters) {
                                    console.log("Application field metadata:", {
                                        id: id,
                                        name: name,
                                        start: start,
                                        text: text,
                                        format: format,
                                        typeName: typeName,
                                        parameters: parameters
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

---

## Update the Visible Text

```javascript
withApplicationFields(function(applicationFields) {
    applicationFields.getItem(function(applicationField) {
        if (!applicationField) {
            console.warn("No application field at current text input position.");
            return;
        }

        applicationField.setText("Updated customer name", function() {
            console.log("Application field text updated.");
        }, onTxError);
    }, onTxError);
});
```

Placeholders:
- `"Updated customer name"` -> `{newVisibleText}`

---

## Rename or Re-ID the Current Application Field

Set a name:

```javascript
withApplicationFields(function(applicationFields) {
    applicationFields.getItem(function(applicationField) {
        if (!applicationField) {
            console.warn("No application field at current text input position.");
            return;
        }

        applicationField.setName("PrimaryCustomerField", function() {
            console.log("Application field name updated.");
        }, onTxError);
    }, onTxError);
});
```

Set an ID:

```javascript
withApplicationFields(function(applicationFields) {
    applicationFields.getItem(function(applicationField) {
        if (!applicationField) {
            console.warn("No application field at current text input position.");
            return;
        }

        applicationField.setID(25, function() {
            console.log("Application field ID updated.");
        }, onTxError);
    }, onTxError);
});
```

Placeholders:
- `"PrimaryCustomerField"` -> `{applicationFieldName}`
- `25` -> `{applicationFieldId}`

---

## Update Format, Type Name, and Parameters

If the type name changes, update the parameters to match the new field type.

```javascript
withApplicationFields(function(applicationFields) {
    applicationFields.getItem(function(applicationField) {
        if (!applicationField) {
            console.warn("No application field at current text input position.");
            return;
        }

        applicationField.setFormat(TXTextControl.ApplicationFieldFormat.MSWord, function() {
            applicationField.setTypeName("MERGEFIELD", function() {
                applicationField.setParameters(["InvoiceNumber", "\\* MERGEFORMAT"], function() {
                    console.log("Application field definition updated.");
                }, onTxError);
            }, onTxError);
        });
    }, onTxError);
});
```

Placeholders:
- `TXTextControl.ApplicationFieldFormat.MSWord` -> `{applicationFieldFormat}`
- `"MERGEFIELD"` -> `{typeName}`
- `["InvoiceNumber", "\\* MERGEFORMAT"]` -> `{parameters}`

---

## Navigate to the Current Application Field

```javascript
withApplicationFields(function(applicationFields) {
    applicationFields.getItem(function(applicationField) {
        if (!applicationField) {
            console.warn("No application field at current text input position.");
            return;
        }

        applicationField.scrollTo(function(scrolled) {
            console.log("Scrolled to application field:", scrolled);
        }, onTxError);
    }, onTxError);
});
```

---

## List Application Fields

Get the application field count and enumerate all items in the main text.

```javascript
withApplicationFields(function(applicationFields) {
    applicationFields.getCount(function(count) {
        console.log("Total application fields:", count);
    }, onTxError);

    applicationFields.forEach(function(applicationField) {
        applicationField.getID(function(id) {
            applicationField.getTypeName(function(typeName) {
                applicationField.getText(function(text) {
                    console.log("Application field item:", {
                        id: id,
                        typeName: typeName,
                        text: text
                    });
                }, onTxError);
            }, onTxError);
        }, onTxError);
    }, onTxError);
});
```

---

## Get an Application Field by Index

```javascript
withApplicationFields(function(applicationFields) {
    applicationFields.elementAt(0, function(applicationField) {
        if (!applicationField) {
            console.warn("No application field at index 0.");
            return;
        }

        applicationField.getTypeName(function(typeName) {
            console.log("Application field at index 0:", {
                typeName: typeName
            });
        }, onTxError);
    }, onTxError);
});
```

Placeholders:
- `0` -> `{index}`

---

## Remove the Current Application Field and Keep Its Text

```javascript
withApplicationFields(function(applicationFields) {
    applicationFields.getItem(function(applicationField) {
        if (!applicationField) {
            console.warn("No application field at current text input position.");
            return;
        }

        applicationFields.remove(applicationField, true, function(removed) {
            console.log("Application field removed and text kept:", removed);
        }, onTxError);
    }, onTxError);
});
```

---

## Remove the Current Application Field and Delete Its Text

```javascript
withApplicationFields(function(applicationFields) {
    applicationFields.getItem(function(applicationField) {
        if (!applicationField) {
            console.warn("No application field at current text input position.");
            return;
        }

        applicationFields.remove(applicationField, false, function(removed) {
            console.log("Application field removed and text deleted:", removed);
        }, onTxError);
    }, onTxError);
});
```

---

## API Mapping

- `mainText.applicationFields` -> access the `ApplicationFieldCollection` of the main text.
- `ApplicationFieldCollection.add` -> insert a new application field at the current input position.
- `ApplicationFieldCollection.getCanAdd` -> check whether insertion is currently allowed.
- `ApplicationFieldCollection.getItem` -> resolve the application field at the current input position.
- `ApplicationFieldCollection.getCount` / `forEach` / `elementAt` -> enumerate collection items.
- `ApplicationFieldCollection.remove` -> remove an application field and optionally keep its visible text.
- `ApplicationField.getFormat` / `setFormat` -> read or set the application-specific field format.
- `ApplicationField.getTypeName` / `setTypeName` -> read or set the underlying field type name.
- `ApplicationField.getParameters` / `setParameters` -> read or replace the field parameter array.
- `ApplicationField.getID`, `getName`, `getText`, `getStart`, `scrollTo`, `setID`, `setName`, `setText` -> inherited text-field style operations.

## Notes

- Application fields are different from form fields. Use `FormFieldCollection` only for Word form fields such as text, check, date, or selection fields.
- Application-field event filtering depends on `TextFieldInfo.type`, which is documented as either `"APPLICATIONFIELD"` or `"TEXTFIELD"`.
- If you change the type name, update the parameter array in the same flow so the field definition stays consistent.