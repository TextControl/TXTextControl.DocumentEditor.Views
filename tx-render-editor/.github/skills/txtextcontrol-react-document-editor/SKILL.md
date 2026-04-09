---
name: txtextcontrol-react-document-editor
description: |
  Use this skill when a user asks how to create and configure
  a TX Text Control Document Editor application in React.

  Trigger it for requests involving React project creation,
  installing the TX Text Control React package,
  rendering the DocumentEditor component, and configuring webSocketURL.
metadata:
  author: Text Control
  version: "34.3.0"
---

# TX Text Control React Document Editor Setup

## Overview

This skill provides React setup guidance for creating a TX Text Control
Document Editor application.

It covers project creation, package installation, React component wiring,
and rendering the editor with hosted or self-hosted backend URLs.

## Prerequisites

- Node.js and npm installed
- create-react-app tooling available
- A TX Text Control trial access token (for hosted backend) or your own backend endpoint

## Quick Start Example

### Example 1: Create a new React document editor app
**User:** "Create a React app with TX Text Control Document Editor"

**Result:** Use `references/create-a-document-editor-application-with-react.md`

## Generate React Setup Guidance

**Trigger keywords:** "react", "create-react-app", "document editor", "tx-react-document-editor", "DocumentEditor", "webSocketURL", "trial token".

**Workflow:**

1. Use `references/create-a-document-editor-application-with-react.md`.
2. Keep commands and code aligned with the documented React package setup.
3. Use only documented setup steps and avoid inventing APIs or package names.
4. If user asks for feature programming (comments, fields, tables, track changes), route to `txtextcontrol-document-editor-api`.

## Code References

| File | Task |
|------|------|
| `create-a-document-editor-application-with-react.md` | Create and run a TX Text Control Document Editor React application |

## Rules

- Use only documented React setup commands and package names
- Do not hardcode secrets; keep trial token placeholders
- Do not introduce unrelated framework setup
