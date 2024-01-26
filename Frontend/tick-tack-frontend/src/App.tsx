import { QueryClientProvider } from "@tanstack/react-query";
import { AppRouter } from "./router";
import { queryClient } from "./lib/ReactQuery";
import { ToastContainer, Bounce } from "react-toastify";
import { AuthProvider } from "./lib/AuthProvider";


function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <AuthProvider>
        <AppRouter />
        <ToastContainer
          position="top-right"
          autoClose={5000}
          hideProgressBar
          newestOnTop={false}
          closeOnClick
          rtl={false}
          pauseOnFocusLoss
          draggable
          pauseOnHover
          theme="dark"
          transition={Bounce}
        />
      </AuthProvider>
    </QueryClientProvider>
  )
}

export default App
