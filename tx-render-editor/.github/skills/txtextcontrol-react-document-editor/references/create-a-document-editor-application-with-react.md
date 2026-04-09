# Create a Document Editor Application with React

## Scope

Create a new React application and integrate the TX Text Control Document Editor
component using the React package and a WebSocket backend URL.

## Prerequisites

- Node.js and npm installed
- A trial access token or your own backend endpoint

## Steps

### 1. Install create-react-app globally

```bash
npm install -g create-react-app
```

### 2. Create a new React application

```bash
npx create-react-app my-tx-document-editor-app
```

### 3. Change into the new project directory

```bash
cd my-tx-document-editor-app
```

### 4. Install the TX Text Control React package

```bash
npm install --save @txtextcontrol/tx-react-document-editor
```

### 5. Render the editor component

Open `src/App.js` and replace the content with:

```javascript
import DocumentEditor from '@txtextcontrol/tx-react-document-editor';
import './App.css';

function App() {
    return (
        <DocumentEditor
            webSocketURL="wss://backend.textcontrol.com/api/TXWebSocket?access-token=your_trial_token"
            editMode="Edit">
        </DocumentEditor>
    );
}

export default App;
```

Placeholders:
- `your_trial_token` -> `{trialAccessToken}`

Notes:
- For a self-hosted backend, replace `webSocketURL` with your endpoint.

### 6. Start the application

```bash
npm start
```

## Notes

- This setup uses the TX Text Control React component package for the editor host.
- After this baseline setup, use `txtextcontrol-document-editor-api` for feature operations and JavaScript API programming.
