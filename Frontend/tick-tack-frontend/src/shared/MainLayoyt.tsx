import { NextUIProvider } from "@nextui-org/react";
import React from "react";
import { Outlet, useNavigate } from "react-router-dom";

export const MainLayoyt: React.FC = () => {
  const navigate = useNavigate();

  return (
    <NextUIProvider navigate={navigate}>
      <React.Suspense
        fallback={
          <div className="flex items-center justify-center w-screen h-screen">
            123
            {/*<Spinner size="large"/>*/}
          </div>
        }
      >
        <main className="h-full">
          <Outlet />
        </main>
      </React.Suspense>
    </NextUIProvider>)
}