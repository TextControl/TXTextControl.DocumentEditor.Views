# Images

Add, inspect, scale, and remove images in the TX Text Control Document Editor.

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

    function withImages(onReady) {
        onReady(TXTextControl.images);
    }
</script>
```

---

## Image Events

Attach image-related listeners with `TXTextControl.addEventListener(eventName, callback)`.
Image event callbacks use `ImageCallback` and receive one `ImageEventArgs` object.

```javascript
TXTextControl.addEventListener("{imageEventName}", function(args) {
    console.log("Image event args:", args);
    console.log("Image frame info:", args.image);
});
```

Placeholders:
- `"{imageEventName}"` -> `{documentedImageEventName}`

Return object shape for image event callbacks:

```javascript
{
    image: {
        id: 25,
        name: "LogoImage",
        location: { x: 1440, y: 720 },
        textPosition: 42
    }
}
```

Notes:
- Use only documented image event names for `eventName`.

---

## Add an Inline Image

```javascript
withImages(function(images) {
    images.addInline("{base64ImageData}", -1, function(image) {
        console.log("Inline image added:", image);
    }, onTxError);
});
```

Placeholders:
- `"{base64ImageData}"` -> `{base64EncodedImageData}`
- `-1` -> `{textPosition}`

Notes:
- `-1` inserts the image at the current input position.

---

## Add an Anchored Image

```javascript
withImages(function(images) {
    images.addAnchored(
        "{base64ImageData}",
        {horizontalAlignmentValue},
        -1,
        {imageInsertionModeValue},
        function(image) {
            console.log("Anchored image added:", image);
        },
        onTxError
    );
});
```

Placeholders:
- `"{base64ImageData}"` -> `{base64EncodedImageData}`
- `{horizontalAlignmentValue}` -> `{TXTextControl.HorizontalAlignment value}`
- `-1` -> `{textPosition}`
- `{imageInsertionModeValue}` -> `{TXTextControl.ImageInsertionMode value}`

---

## Get the Selected Image or Get by ID

```javascript
withImages(function(images) {
    images.getItem(function(image) {
        if (!image) {
            console.warn("No image selected at current input position.");
            return;
        }

        image.getID(function(id) {
            image.getName(function(name) {
                console.log("Selected image:", {
                    id: id,
                    name: name
                });
            }, onTxError);
        }, onTxError);
    }, onTxError);
});
```

```javascript
withImages(function(images) {
    images.getItem(function(image) {
        if (!image) {
            console.warn("No image found for requested id.");
            return;
        }

        image.getLocation(function(location) {
            image.getSize(function(size) {
                console.log("Image by id:", {
                    id: {imageId},
                    location: location,
                    size: size
                });
            }, onTxError);
        }, onTxError);
    }, onTxError, {imageId});
});
```

Placeholders:
- `{imageId}` -> `{existingImageId}`

---

## Update Image Properties

```javascript
withImages(function(images) {
    images.getItem(function(image) {
        if (!image) {
            console.warn("No image selected at current input position.");
            return;
        }

        image.setName("UpdatedImageName", function() {
            image.setHorizontalScaling(80, function() {
                image.setVerticalScaling(80, function() {
                    console.log("Image updated.");
                }, onTxError);
            }, onTxError);
        }, onTxError);
    }, onTxError);
});
```

Placeholders:
- `"UpdatedImageName"` -> `{newImageName}`
- `80` -> `{horizontalScalingPercent}`
- `80` -> `{verticalScalingPercent}`

Notes:
- Scaling values are percentage values.

---

## List Images

```javascript
withImages(function(images) {
    images.getCount(function(count) {
        console.log("Total images:", count);
    }, onTxError);

    images.forEach(function(image) {
        image.getID(function(id) {
            image.getName(function(name) {
                image.getTextPosition(function(textPosition) {
                    console.log("Image item:", {
                        id: id,
                        name: name,
                        textPosition: textPosition
                    });
                }, onTxError);
            }, onTxError);
        }, onTxError);
    }, onTxError);
});
```

---

## Get Image by Index

```javascript
withImages(function(images) {
    images.elementAt(0, function(image) {
        if (!image) {
            console.warn("No image at index:", 0);
            return;
        }

        image.getName(function(name) {
            console.log("Image at index:", {
                index: 0,
                name: name
            });
        }, onTxError);
    }, onTxError);
});
```

Placeholders:
- `0` -> `{zeroBasedIndex}`

---

## Remove an Image

```javascript
withImages(function(images) {
    images.getItem(function(image) {
        if (!image) {
            console.warn("No image selected at current input position.");
            return;
        }

        images.remove(image, function() {
            console.log("Image removed.");
        }, onTxError);
    }, onTxError);
});
```

---

## API Mapping
- `TXTextControl.images`
- `TXTextControl.addEventListener(eventName, callback)`
- `ImageCollection.addInline(imageData, textPosition, callback, errorCallback)`
- `ImageCollection.addAnchored(imageData, horizontalAlignment, textPosition, insertionMode, callback, errorCallback)`
- `ImageCollection.getItem(callback, errorCallback, id)`
- `ImageCollection.remove(image, callback, errorCallback)`
- `Collection.getCount(callback, errorCallback)`
- `Collection.forEach(callback, errorCallback)`
- `Collection.elementAt(index, callback, errorCallback)`
- `Image.setName(name, callback, errorCallback)`
- `Image.setHorizontalScaling(value, callback, errorCallback)`
- `Image.setVerticalScaling(value, callback, errorCallback)`

## Notes

- `ImageCollection` inherits standard collection methods from `Collection`.
- `Image` inherits common frame methods from `FrameBase` (for example `getName`, `setName`, `getLocation`, `setLocation`, `getSize`, and `setSize`).
