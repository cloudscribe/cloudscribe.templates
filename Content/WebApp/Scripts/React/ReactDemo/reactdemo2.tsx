import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
//import './index.css'
import App from "./app2";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <App />
  </StrictMode>,
);
