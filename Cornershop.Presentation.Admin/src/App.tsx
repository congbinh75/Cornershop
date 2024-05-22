import { useEffect, useState } from "react";
import { Outlet, useLocation, useNavigate } from "react-router-dom";
import Sidebar from "./components/sidebar";
import Navbar from "./components/navbar";
import { toast, ToastContainer } from "react-toastify";
import { useGet } from "./api/service";
import Loader from "./components/loader";

function App() {
  const navigate = useNavigate();
  const { pathname } = useLocation();
  const [sidebarOpen, setSidebarOpen] = useState(false);

  useEffect(() => {
    window.scrollTo(0, 0);
  }, [pathname]);

  const { data, isLoading, isError } = useGet("/user/admin/current");

  if (isLoading) return <Loader />;
  if (isError) toast(isError.message);

  return (
    <>
      <div className="dark:bg-boxdark-2 dark:text-bodydark">
        <div className="flex h-screen overflow-hidden">
          <Sidebar sidebarOpen={sidebarOpen} setSidebarOpen={setSidebarOpen} />
          <div className="relative flex flex-1 flex-col overflow-y-auto overflow-x-hidden">
            <Navbar
              sidebarOpen={sidebarOpen}
              setSidebarOpen={setSidebarOpen}
              currentUser={data?.user}
            />
            <main>
              <div className="mx-auto max-w-screen-2xl p-4 md:p-6 2xl:p-10">
                <Outlet />
              </div>
            </main>
          </div>
          <ToastContainer />
        </div>
      </div>
    </>
  );
}

export default App;
