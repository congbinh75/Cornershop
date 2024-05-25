import React from "react";
import ReactDOM from "react-dom/client";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import App from "./App";
import "./index.css";
import Profile from "./pages/users/profile";
import Login from "./pages/login";
import Products from "./pages/products";
import Categories from "./pages/categories";
import Orders from "./pages/orders";
import Users from "./pages/users";
import Dashboard from "./pages/dashboard";
import Subcategories from "./pages/subcategories";
import NewCategory from "./pages/categories/newCategory";
import NewSubcategory from "./pages/subcategories/newSubcategory";
import NewProduct from "./pages/products/newProduct";
import Authors from "./pages/authors";
import NewAuthor from "./pages/authors/newAuthor";
import Publishers from "./pages/publishers";
import NewPublisher from "./pages/publishers/newPublisher";
import UpdateCategory from "./pages/categories/updateCategory";

const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    //errorElement: <NotFound />,
    children: [
      {
        path: "/",
        element: <Dashboard />,
      },
      {
        path: "/products",
        element: <Products />,
      },
      {
        path: "/products/create",
        element: <NewProduct />,
      },
      {
        path: "/authors",
        element: <Authors />,
      },
      {
        path: "/authors/create",
        element: <NewAuthor />,
      },
      {
        path: "/publishers",
        element: <Publishers />,
      },
      {
        path: "/publishers/create",
        element: <NewPublisher />,
      },
      {
        path: "/categories",
        element: <Categories />,
      },
      {
        path: "/categories/create",
        element: <NewCategory />,
      },
      {
        path: "/categories/update/:id",
        element: <UpdateCategory />,
      },
      {
        path: "/subcategories",
        element: <Subcategories />,
      },
      {
        path: "/subcategories/create",
        element: <NewSubcategory />,
      },
      {
        path: "/orders",
        element: <Orders />,
      },
      {
        path: "/users",
        element: <Users />,
      },
      {
        path: "/profile",
        element: <Profile />,
      }
    ],
  },
  {
    path: "/login",
    element: <Login />,
  },
]);

ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>
);
