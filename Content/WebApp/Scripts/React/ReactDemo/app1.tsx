import { useState } from "react";
import { RenderCounter } from "./renderCounter";

function Button({ onClick, children }) {
  return <button onClick={onClick}>{children}</button>;
}

function App() {
  const [showText, setShowText] = useState(false);

  return (
    <div className="App">
      <Button onClick={() => setShowText(!showText)}>Toggle Text</Button>
      <RenderCounter message="parent renders" />

      {showText && <h1>Text rendered by React</h1>}
    </div>
  );
}

export default App;
