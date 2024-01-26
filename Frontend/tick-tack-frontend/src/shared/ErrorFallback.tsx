import { Button } from "@nextui-org/react";

export const ErrorFallback: React.FC = () => (
    <div
        className="text-red-500 w-screen h-screen flex flex-col justify-center items-center"
        role="alert"
    >
        <h2 className="text-lg font-semibold">Неудалось загрузиться :( </h2>
        <Button className="mt-4" onClick={() => window.location.assign(window.location.toString())}>
            Повторить попытку
        </Button>
    </div>
);