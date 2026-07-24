import { useEffect, useState } from "react";
import { getPing } from "./api/pingApi";

function App() {
  const [message, setMessage] = useState("Chargement...");

  useEffect(() => {
    async function load() {
      try {
        const data = await getPing();
        setMessage(data.message);
        
      } catch (error) {
        console.error(error);
        setMessage("Erreur de communication avec l'API");
      }
    }

    load();
  }, []);

  return (
    <div>
      <h1>{message}</h1>
    </div>
  );
}

export default App;