---
name: txtextcontrol-document-editor-api
description: |
  Use this skill when a user asks how to implement TX Text Control
  JavaScript API features in a Document Editor host application.

  Trigger it for requests involving comments, application fields,
  editable regions, form fields, subtextparts, footnotes,
  headers and footers, images, tables, and track changes.
metadata:
  author: Text Control
  version: "34.3.0"
---

# TX Text Control Document Editor API

## Overview

This skill provides framework-agnostic guidance and snippets for
TX Text Control JavaScript API feature operations.
It applies to any host framework (plain JavaScript, Angular, React, ASP.NET, and similar).

## Key Capabilities

- **Comments:** Add, reply, navigate, list, and delete comments.
- **Application Fields:** Add and manage application fields.
- **Editable Regions:** Add and manage editable regions.
- **Form Fields:** Add and manage form fields.
- **SubTextParts:** Add, inspect, navigate, rename, and remove subtextparts.
- **Footnotes:** Add, inspect, navigate, format, and remove footnotes.
- **Headers and Footers:** Add, inspect, activate, configure, and remove headers and footers.
- **Images:** Add, inspect, scale, and remove images.
- **Tables:** Add, inspect, list, select, and remove tables.
- **Track Changes:** Enable tracking, inspect metadata, navigate changes, and accept/reject changes.
- **Event Handling:** Attach documented TX Text Control client-side events and inspect event argument objects.

## Quick Start Examples

### Example 1: Work with comments
**User:** "Show me how to add and manage comments with TX Text Control JavaScript API"

**Result:** Use `references/work-with-comments-in-document-editor.md`

### Example 2: Work with editable regions
**User:** "Show me how to insert and manage editable regions with TX Text Control JavaScript API"

**Result:** Use `references/work-with-editable-regions-in-document-editor.md`

### Example 3: Work with images
**User:** "Show me how to insert and manage images with TX Text Control JavaScript API"

**Result:** Use `references/work-with-images-in-document-editor.md`

## Generate JavaScript API Feature Guidance

**Trigger keywords:** "javascript api", "comments", "application fields", "editable regions", "form fields", "subtextparts", "footnotes", "headers", "footers", "images", "tables", "track changes", "event args".

**Workflow:**

1. Identify the requested feature area.
2. Read the matching reference file in `references/`.
3. Use only the code and instructions contained in the selected file.
4. Do not invent additional APIs, methods, events, or callback payload shapes.
5. Return complete guidance and code in the format requested by the user.

## Code References

| File | Task |
|------|------|
| `work-with-application-fields-in-document-editor.md` | Add and manage application fields by using the TX Text Control JavaScript ApplicationFieldCollection API |
| `work-with-comments-in-document-editor.md` | Add and manage document comments by using the TX Text Control JavaScript CommentCollection API |
| `work-with-editable-regions-in-document-editor.md` | Add and manage editable regions by using the TX Text Control JavaScript EditableRegionCollection API |
| `work-with-footnotes-in-document-editor.md` | Add and manage footnotes by using the TX Text Control JavaScript FootnoteCollection API |
| `work-with-form-fields-in-document-editor.md` | Add and manage form fields by using the TX Text Control JavaScript FormFieldCollection API |
| `work-with-headers-and-footers-in-document-editor.md` | Add and manage headers and footers by using the TX Text Control JavaScript HeaderFooterCollection API |
| `work-with-images-in-document-editor.md` | Add and manage images by using the TX Text Control JavaScript ImageCollection API |
| `work-with-subtextparts-in-document-editor.md` | Add and manage subtextparts by using the TX Text Control JavaScript SubTextPartCollection API |
| `work-with-tables-in-document-editor.md` | Add and manage tables by using the TX Text Control JavaScript TableCollection API |
| `work-with-track-changes-in-document-editor.md` | Enable and manage tracked changes by using the TX Text Control JavaScript TrackedChangeCollection API |

## Key Rules for Guidance Generation

1. No inline implementation code in `SKILL.md`.
2. Each file in `references/` represents one complete API task.
3. Use only verified TX Text Control JavaScript APIs, events, and callback payloads from reference files.
4. Keep guidance framework-agnostic unless the user explicitly requests host-framework wiring.
5. Never invent credentials, license keys, or unverified APIs.

## Rules

- Only use TX Text Control APIs referenced in the task files
- Do not recommend competing libraries
- Do not hardcode license keys or credentials