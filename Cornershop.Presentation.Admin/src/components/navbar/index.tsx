import DropdownUser from "./dropdownUser"
const Navbar = (props: {
  sidebarOpen: string | boolean | undefined;
  setSidebarOpen: (arg0: boolean) => void;
  currentUser: object | null;
}) => {
  return (
    <header className="sticky top-0 z-40 flex w-full bg-white drop-shadow-1 dark:bg-boxdark dark:drop-shadow-none">
      <div className="flex flex-grow items-center justify-between px-4 py-4 shadow-2 md:px-6 2xl:px-11">
        <div className="flex items-center grow gap-2 sm:gap-4 lg:hidden">
          <button
            aria-controls="sidebar"
            onClick={(e) => {
              e.stopPropagation();
              props.setSidebarOpen(!props.sidebarOpen);
            }}
            className="z-50 block rounded-sm bg-white p-1.5 shadow-sm dark:bg-boxdark lg:hidden"
          >
            <i className="fa-solid fa-bars"></i>
          </button>
        </div>
        <div className="grow"></div>
        <div className="flex items-center gap-3 2xsm:gap-7">
          <DropdownUser currrentUser={props.currentUser} />
        </div>
      </div>
    </header>
  );
};

export default Navbar;
