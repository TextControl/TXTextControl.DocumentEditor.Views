---
name: txtextcontrol-angular-document-editor
description: |
  Use this skill when a user asks how to create and configure
  a TX Text Control Document Editor application with Angular CLI 19.

  Trigger it for requests involving Angular project creation,
  installing the TX Text Control Angular package,
  wiring DocumentEditorModule, and rendering the tx-document-editor component.
metadata:
  author: Text Control
  version: "34.3.0"
---

# TX Text Control Angular Document Editor Setup

## Overview

This skill provides Angular setup guidance for creating a TX Text Control
Document Editor application with Angular CLI 19.

It covers project creation, package installation, Angular module setup,
and rendering the editor component with hosted or self-hosted backend URLs.

## Prerequisites

- Node.js and npm installed
- Angular CLI installed globally
- A TX Text Control trial access token (for hosted backend) or your own backend endpoint

## Quick Start Example

### Example 1: Create a new Angular document editor app
**User:** "Create an Angular CLI 19 app with TX Text Control Document Editor"

**Result:** Use `references/create-a-document-editor-application-with-angular-cli-19.md`

## Generate Angular Setup Guidance

**Trigger keywords:** "angular", "angular cli 19", "document editor", "tx-document-editor", "documenteditormodule", "websocketurl", "trial token".

**Workflow:**

1. Use `references/create-a-document-editor-application-with-angular-cli-19.md`.
2. Keep commands and code aligned with Angular CLI 19 and the documented package.
3. Use only documented setup steps and avoid inventing APIs or package names.
4. If user asks for feature programming (comments, fields, tables, track changes), route to `txtextcontrol-document-editor-api`.

## Code References

| File | Task |
|------|------|
| `create-a-document-editor-application-with-angular-cli-19.md` | Create and run a TX Text Control Document Editor Angular application with Angular CLI 19 |

## Rules

- Use only documented Angular setup commands and package names
- Do not hardcode secrets; keep trial token placeholders
- Do not introduce unrelated framework setup
