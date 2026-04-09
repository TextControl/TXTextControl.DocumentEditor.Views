# Create a Document Editor Application with Angular CLI 19

## Scope

Create a new Angular application and integrate the TX Text Control Document Editor
component using the Angular package and a WebSocket backend URL.

## Prerequisites

- Node.js and npm installed
- Angular CLI installed globally
- A trial access token or your own backend endpoint

## Steps

### 1. Install Angular CLI globally

```bash
npm install -g @angular/cli
```

### 2. Create a new Angular project

```bash
ng new my-editor-app --no-standalone
```

When prompted:
- Choose `CSS` as the stylesheet format
- Choose `N` to disable server-side rendering and static page generation

### 3. Change into the new project directory

```bash
cd my-editor-app
```

### 4. Install the TX Text Control Angular package

```bash
npm i @txtextcontrol/tx-ng-document-editor
```

### 5. Register the editor module

Open `src/app/app.module.ts` and replace the content with:

```typescript
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { DocumentEditorModule } from '@txtextcontrol/tx-ng-document-editor';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    DocumentEditorModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
```

### 6. Render the editor component

Open `src/app/app.component.html` and replace the content with:

```html
<tx-document-editor
  width="1000px"
  height="500px"
  webSocketURL="wss://backend.textcontrol.com/TXWebSocket?access-token=yourtoken">
</tx-document-editor>
```

Placeholders:
- `yourtoken` -> `{trialAccessToken}`

Notes:
- For a self-hosted backend, replace `webSocketURL` with your endpoint, for example:
  - `ws://localhost:8080/TXWebSocket`

### 7. Start the application

```bash
ng serve --open
```

## Notes

- This setup uses the TX Text Control Angular component package for the editor host.
- After this baseline setup, use `txtextcontrol-document-editor-api` for feature operations and JavaScript API programming.
