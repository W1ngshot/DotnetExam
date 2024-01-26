import { ModuleRoutes } from "@/types";
import { Main } from "./game/Main";
import { RatingPage } from "./rating/Main";
import { LoginPage } from "./auth/Login";

export const GameRoutes: ModuleRoutes = {
    anonymous: [{
        path: '/login',
        element: <LoginPage />
    }],
    authenticated: [{
        path: '/',
        element: <RatingPage />
    },{
        path: '/game/:id',
        element: <Main guest={false}/>
    },{
        path: '/watch/:id',
        element: <Main guest/>
    }],
    common: []
} 