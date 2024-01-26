import { Navigate, Outlet, RouteObject, RouterProvider, createBrowserRouter, useLocation } from "react-router-dom";
import { ModuleRoutes } from "./types";
import { ErrorFallback } from "./shared/ErrorFallback";
import { MainLayoyt } from "./shared/MainLayoyt";
import { GameRoutes } from "./features/routes";
import { useAuth } from "./lib/AuthProvider";

export const AuthorizedRouteWrapper: React.FC = () => {
  const auth = useAuth();
  const location = useLocation();
  return auth.user != null ? (<Outlet />) : (<Navigate to="/login" state={{ returnPath: location.pathname }} />);
}

export const AnonymousRouteWrapper: React.FC = () => {
  const auth = useAuth();
  const location = useLocation();

  return auth.user == null ? (<Outlet/>) : (<Navigate to={location.state?.returnPath ?? '/'} />);
}

export const authorizedRoutes: RouteObject = {
  path: "/",
  element: <AuthorizedRouteWrapper />,
  children: []
}

export const anonymousRoutes: RouteObject = {
  path: "/",
  element: <AnonymousRouteWrapper />,
  children: []
}

export const commonRoutes: RouteObject[] = [
];

function registerModuleRoutes(routes: ModuleRoutes) {
  anonymousRoutes.children!.push(...routes.anonymous);
  authorizedRoutes.children!.push(...routes.authenticated);
  commonRoutes.push(...routes.common);
}

registerModuleRoutes(GameRoutes)

const router = createBrowserRouter([
  {
    path: "/",
    errorElement: <ErrorFallback />,
    element: <MainLayoyt />,
    children: [
      ...commonRoutes,
      authorizedRoutes,
      anonymousRoutes,
    ]
  }]);

export const AppRouter: React.FC = () => {
  return (
    <RouterProvider router={router} />
  )
};