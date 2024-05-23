import { useEffect, useRef, useState } from "react";
import { Link, useLocation } from "react-router-dom";
import Logo from "/images/logo/logo_darkmode.png";

interface SidebarProps {
  sidebarOpen: boolean;
  setSidebarOpen: (arg: boolean) => void;
}

const Sidebar = ({ sidebarOpen, setSidebarOpen }: SidebarProps) => {
  const location = useLocation();
  const { pathname } = location;

  const trigger = useRef<any>(null);
  const sidebar = useRef<any>(null);

  const storedSidebarExpanded = localStorage.getItem("sidebar-expanded");
  const [sidebarExpanded, setSidebarExpanded] = useState(
    storedSidebarExpanded === null ? false : storedSidebarExpanded === "true"
  );

  // close on click outside
  useEffect(() => {
    const clickHandler = ({ target }: MouseEvent) => {
      if (!sidebar.current || !trigger.current) return;
      if (
        !sidebarOpen ||
        sidebar.current.contains(target) ||
        trigger.current.contains(target)
      )
        return;
      setSidebarOpen(false);
    };
    document.addEventListener("click", clickHandler);
    return () => document.removeEventListener("click", clickHandler);
  });

  // close if the esc key is pressed
  useEffect(() => {
    const keyHandler = ({ keyCode }: KeyboardEvent) => {
      if (!sidebarOpen || keyCode !== 27) return;
      setSidebarOpen(false);
    };
    document.addEventListener("keydown", keyHandler);
    return () => document.removeEventListener("keydown", keyHandler);
  });

  useEffect(() => {
    localStorage.setItem("sidebar-expanded", sidebarExpanded.toString());
    if (sidebarExpanded) {
      document.querySelector("body")?.classList.add("sidebar-expanded");
    } else {
      document.querySelector("body")?.classList.remove("sidebar-expanded");
    }
  }, [sidebarExpanded]);

  return (
    <aside
      ref={sidebar}
      className={`absolute left-0 top-0 z-50 flex h-screen w-72.5 flex-col overflow-y-hidden bg-black duration-300 ease-linear dark:bg-boxdark lg:static lg:translate-x-0 ${
        sidebarOpen ? "translate-x-0" : "-translate-x-full"
      }`}
    >
      <div className="flex items-center justify-between gap-2 px-6 py-5.5 lg:py-6.5">
        <Link to="/" className="my-4 px-2">
          <img src={Logo} alt="Logo" />
        </Link>

        <button
          ref={trigger}
          onClick={() => setSidebarOpen(!sidebarOpen)}
          aria-controls="sidebar"
          aria-expanded={sidebarOpen}
          className="block lg:hidden"
        >
          <i className="fa-solid fa-arrow-left text-white"></i>
        </button>
      </div>

      <div className="no-scrollbar flex flex-col grow overflow-y-auto duration-300 ease-linear">
        <nav className="mt-5 py-4 px-4 lg:mt-9 lg:px-6">
          <div>
            <ul className="mb-6 flex flex-col gap-1.5">
              <li>
                <Link
                  to="/"
                  className={`group relative flex items-center gap-2.5 rounded-sm px-4 py-2 font-medium text-bodydark1 duration-300 ease-in-out hover:bg-graydark dark:hover:bg-meta-4 ${
                    (pathname === "/" || pathname.includes("/dashboard")) &&
                    "bg-graydark dark:bg-meta-4"
                  }`}
                >
                  <span className="w-5">
                    <i className="fa-solid fa-house"></i>
                  </span>
                  <span>Dashboard</span>
                </Link>
              </li>

              <li>
                <Link
                  to="/products"
                  className={`group relative flex items-center gap-2.5 rounded-sm py-2 px-4 font-medium text-bodydark1 duration-300 ease-in-out hover:bg-graydark dark:hover:bg-meta-4 ${
                    pathname.includes("/products") &&
                    "bg-graydark dark:bg-meta-4"
                  }`}
                >
                  <span className="w-5">
                    <i className="fa-solid fa-book"></i>
                  </span>
                  <span>Products</span>
                </Link>
              </li>

              <li>
                <Link
                  to="/authors"
                  className={`group relative flex items-center gap-2.5 rounded-sm py-2 px-4 text-bodydark1 duration-300 ease-in-out hover:bg-graydark dark:hover:bg-meta-4 ${
                    pathname.includes("/authors") && "bg-graydark dark:bg-meta-4"
                  }`}
                >
                  <span className="w-5">
                  </span>
                  <span className="text-sm">Authors</span>
                </Link>
              </li>

              <li>
                <Link
                  to="/publishers"
                  className={`group relative flex items-center gap-2.5 rounded-sm py-2 px-4 text-bodydark1 duration-300 ease-in-out hover:bg-graydark dark:hover:bg-meta-4 ${
                    pathname.includes("/publishers") && "bg-graydark dark:bg-meta-4"
                  }`}
                >
                  <span className="w-5">
                  </span>
                  <span className="text-sm">Publishers</span>
                </Link>
              </li>

              <li>
                <Link
                  to="/categories"
                  className={`group relative flex items-center gap-2.5 rounded-sm py-2 px-4 font-medium text-bodydark1 duration-300 ease-in-out hover:bg-graydark dark:hover:bg-meta-4 ${
                    pathname.includes("/categories") && "bg-graydark dark:bg-meta-4"
                  }`}
                >
                  <span className="w-5">
                    <i className="fa-solid fa-layer-group"></i>
                  </span>
                  <span>Categories</span>
                </Link>
              </li>

              <li>
                <Link
                  to="/subcategories"
                  className={`group relative flex items-center gap-2.5 rounded-sm py-2 px-4 text-bodydark1 duration-300 ease-in-out hover:bg-graydark dark:hover:bg-meta-4 ${
                    pathname.includes("/subcategories") && "bg-graydark dark:bg-meta-4"
                  }`}
                >
                  <span className="w-5">
                  </span>
                  <span className="text-sm">Subcategories</span>
                </Link>
              </li>

              <li>
                <Link
                  to="/orders"
                  className={`group relative flex items-center gap-2.5 rounded-sm py-2 px-4 font-medium text-bodydark1 duration-300 ease-in-out hover:bg-graydark dark:hover:bg-meta-4 ${
                    pathname.includes("/orders") && "bg-graydark dark:bg-meta-4"
                  }`}
                >
                  <span className="w-5">
                    <i className="fa-solid fa-cart-shopping"></i>
                  </span>
                  <span>Orders</span>
                </Link>
              </li>
              <li>
                <Link
                  to="/users"
                  className={`group relative flex items-center gap-2.5 rounded-sm py-2 px-4 font-medium text-bodydark1 duration-300 ease-in-out hover:bg-graydark dark:hover:bg-meta-4 ${
                    pathname.includes("/users") &&
                    "bg-graydark dark:bg-meta-4"
                  }`}
                >
                  <span className="w-5">
                    <i className="fa-solid fa-users"></i>
                  </span>
                  <span>Users</span>
                </Link>
              </li>
            </ul>
          </div>
        </nav>
      </div>
    </aside>
  );
};

export default Sidebar;
