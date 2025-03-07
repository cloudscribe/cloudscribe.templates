// import './App.css';

import { useState } from "react";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { UserList } from "../Components/userlist";

function Button({ onClick, children }) {
  return <button onClick={onClick}>{children}</button>;
}

function App() {
  const [showText, setShowText] = useState(false);

  const myClient = new QueryClient({
    defaultOptions: {
      queries: {
        refetchOnWindowFocus: false,
        refetchInterval: 30000, //refetchTime,
        refetchIntervalInBackground: false,
      },
    },
  });

    return (
        <div className="App">
            <QueryClientProvider client={myClient}>
                <UserList></UserList>
            </QueryClientProvider>
        </div>
    );
}

export default App;
