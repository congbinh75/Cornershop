interface Props {
  sidebarOpen: boolean;
  setSidebarOpen: (arg: boolean) => void;
}

const Sidebar = (props: Props) => {
  return (
    <div
      className={`absolute left-0 top-0 z-9999 flex h-screen w-72 flex-col overflow-y-hidden duration-300 ease-linear bg-slate-100 dark:bg-slate-800 lg:static lg:translate-x-0 ${
        props.sidebarOpen ? "translate-x-0" : "-translate-x-full"
      }`}
    >
      {/* <!-- SIDEBAR HEADER --> */}
      <div className="flex items-center justify-between gap-2 px-6 py-5.5 lg:py-6.5">
        <a href="/">
          <img />
        </a>
      </div>

      <div className="no-scrollbar flex flex-col overflow-y-auto duration-300 ease-linear">
        <nav className="mt-5 py-4 px-4 lg:mt-9 lg:px-6">
          <div>
            <ul className="mb-6 flex flex-col gap-1.5">
              <div
                className=" relative flex items-center gap-2.5 rounded-sm px-4 py-2 font-medium text-black dark:text-white duration-300 ease-in-out hover:bg-graydark dark:hover:bg-meta-4
                  bg-graydark dark:bg-meta-4"
              >
                <i className="fa-solid fa-house"></i>
                Dashboard
              </div>
            </ul>
          </div>
        </nav>
      </div>
    </div>
  );
};

export default Sidebar;
