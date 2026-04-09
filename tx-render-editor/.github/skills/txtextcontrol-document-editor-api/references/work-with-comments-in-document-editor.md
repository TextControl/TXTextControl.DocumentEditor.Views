# Comments

Add, reply to, navigate, list, and delete comments in the TX Text Control Document Editor.

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

    function withCommentCollection(onReady) {
        TXTextControl.textParts.getMainText(function(mainText) {
            onReady(mainText.comments);
        }, onTxError);
    }
</script>
```

---

## Comment Events

Attach comment-related listeners with `TXTextControl.addEventListener(eventName, callback)`.

Supported comment events include:

- `commentChanged`
- `commentCreated`
- `commentDeleted`
- `commentedTextEntered`
- `commentedTextLeft`
- `commentStateChanged`

Example: react to a changed comment and inspect the returned comment payload.

```javascript
TXTextControl.addEventListener("commentChanged", function(args) {
    console.log("Comment event args:", args);
    console.log("Commented text info:", args.commentedText);

    console.log({
        id: args.commentedText.id,
        number: args.commentedText.number,
        start: args.commentedText.start,
        length: args.commentedText.length,
        active: args.commentedText.active,
        userName: args.commentedText.userName,
        creationTime: args.commentedText.creationTime,
        comment: args.commentedText.comment
    });
});
```

Return object for `commentChanged` and the other comment events:

```javascript
{
    commentedText: {
        id: 12,
        number: 1,
        start: 25,
        length: 5,
        active: true,
        userName: "Tim Typer",
        creationTime: 1767225600,
        comment: "Updated comment text"
    }
}
```

---

## Add Comment

Insert a comment for the current selection.

```javascript
withCommentCollection(function(comments) {
    comments.add("Selection comment", function(response) {
        console.log("Comment added:", response);
    }, onTxError);
});
```

Placeholders:
- `"Selection comment"` -> `{commentText}`

---

## Add Comment at Position

Insert a comment for a specific text range.

```javascript
withCommentCollection(function(comments) {
    comments.addAtPosition("Position comment", 1, 5, function(response) {
        console.log("Comment added at position:", response);
    }, onTxError);
});
```

Placeholders:
- `"Position comment"` -> `{commentText}`
- `1` -> `{startPosition}`
- `5` -> `{length}`

Notes:
- `start` is one-based.

---

## Reply to Comment

Reply to the comment at the current text input position.

```javascript
withCommentCollection(function(comments) {
    comments.getItem(function(currentComment) {
        if (!currentComment) {
            console.warn("No active comment at current text input position.");
            return;
        }

        comments.addReply("Reply comment", currentComment, function(response) {
            console.log("Reply added:", response);
        }, onTxError);
    }, onTxError);
});
```

Placeholders:
- `"Reply comment"` -> `{replyText}`

---

## Get Current Comment

Read the comment at the current text input position.

```javascript
withCommentCollection(function(comments) {
    comments.getItem(function(comment) {
        if (!comment) {
            console.warn("No active comment at current text input position.");
            return;
        }

        comment.getID(function(id) {
            comment.getComment(function(text) {
                console.log("Current comment:", { id: id, text: text });
            }, onTxError);
        }, onTxError);
    }, onTxError);
});
```

---

## Navigate Comments

Move to the next comment:

```javascript
withCommentCollection(function(comments) {
    comments.getNextItem(function(comment) {
        if (!comment) {
            console.warn("No next comment found.");
            return;
        }

        comment.scrollTo(function() {
            console.log("Moved to next comment.");
        }, onTxError);
    }, onTxError);
});
```

Move to the previous comment:

```javascript
withCommentCollection(function(comments) {
    comments.getPreviousItem(function(comment) {
        if (!comment) {
            console.warn("No previous comment found.");
            return;
        }

        comment.scrollTo(function() {
            console.log("Moved to previous comment.");
        }, onTxError);
    }, onTxError);
});
```

---

## Delete Comment

Delete the comment at the current text input position.

```javascript
withCommentCollection(function(comments) {
    comments.getItem(function(comment) {
        if (!comment) {
            console.warn("No active comment to remove.");
            return;
        }

        comments.remove(comment, function(removed) {
            console.log("Comment removed:", removed);
        }, onTxError);
    }, onTxError);
});
```

---

## List Comments

Get comment count and enumerate all comments.

```javascript
withCommentCollection(function(comments) {
    comments.getCount(function(count) {
        console.log("Total comments:", count);
    }, onTxError);

    comments.forEach(function(comment) {
        comment.getID(function(id) {
            comment.getComment(function(text) {
                console.log("Comment item:", { id: id, text: text });
            }, onTxError);
        }, onTxError);
    }, onTxError);
});
```

---

## API Mapping
- `TXTextControl.textParts.getMainText(callback, errorCallback)`
- `FormattedText.comments`
- `CommentCollection.add(comment, callback, errorCallback)`
- `CommentCollection.addAtPosition(comment, start, length, callback, errorCallback)`
- `CommentCollection.addReply(comment, repliedComment, callback, errorCallback)`
- `CommentCollection.getItem(callback, errorCallback)`
- `CommentCollection.getNextItem(callback, errorCallback)`
- `CommentCollection.getPreviousItem(callback, errorCallback)`
- `CommentCollection.remove(comment, callback, errorCallback)`
- `Collection.getCount(callback, errorCallback)`
- `Collection.forEach(callback, errorCallback)`
- `CommentedText.getID(callback, errorCallback)`
- `CommentedText.getComment(callback, errorCallback)`
- `CommentedText.scrollTo(callback, errorCallback)`

## Notes

- Use only the APIs and object paths shown in this task.
- Comment event callbacks receive `CommentEventArgs`, which expose the `commentedText` object shown above.