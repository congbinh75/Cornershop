import { useState } from "react";
import "./App.css";
import Sidebar from "./components/sidebar";
import Navbar from "./components/navbar";
import { Outlet } from "react-router-dom";

const App = () => {
  const context = {};

  const [sidebarOpen, setSideBarOpen] = useState(true);

  return (
    <div className="flex flex-row w-full bg-slate-50 dark:bg-slate-900">
      <Sidebar sidebarOpen={sidebarOpen} setSidebarOpen={setSideBarOpen} />
      <div className="w-full p-5 flex flex-col">
        <Navbar />
        <Outlet context={context} />
      </div>
    </div>
  );
};

export default App;
