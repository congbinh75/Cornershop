import { useEffect, useRef, useState } from "react";
import { Link } from "react-router-dom";
import DarkModeSwitcher from "./darkModeSwitcher";

const DropdownUser = () => {
  const [dropdownOpen, setDropdownOpen] = useState(false);

  const trigger = useRef<any>(null);
  const dropdown = useRef<any>(null);

  // close on click outside
  useEffect(() => {
    const clickHandler = ({ target }: MouseEvent) => {
      if (!dropdown.current) return;
      if (
        !dropdownOpen ||
        dropdown.current.contains(target) ||
        trigger.current.contains(target)
      )
        return;
      setDropdownOpen(false);
    };
    document.addEventListener("click", clickHandler);
    return () => document.removeEventListener("click", clickHandler);
  });

  // close if the esc key is pressed
  useEffect(() => {
    const keyHandler = ({ keyCode }: KeyboardEvent) => {
      if (!dropdownOpen || keyCode !== 27) return;
      setDropdownOpen(false);
    };
    document.addEventListener("keydown", keyHandler);
    return () => document.removeEventListener("keydown", keyHandler);
  });

  return (
    <div className="relative">
      <div
        ref={trigger}
        onClick={() => setDropdownOpen(!dropdownOpen)}
        className="flex items-center gap-4"
      >
        <span className="text-right">
          <span className="block text-sm font-medium text-black dark:text-white">
            Thomas Anree
          </span>
        </span>
        <i className="fa-solid fa-chevron-down"></i>
      </div>

      <div
        ref={dropdown}
        onFocus={() => setDropdownOpen(true)}
        onBlur={() => setDropdownOpen(false)}
        className={`absolute right-0 mt-4 flex w-62.5 flex-col rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark ${
          dropdownOpen === true ? "block" : "hidden"
        }`}
      >
        <ul className="border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
          <li className="flex items-center gap-3.5 px-6 py-3 text-sm font-medium duration-300 ease-in-out hover:text-primary lg:text-base">
            <Link
              to="/profile"
              className="flex items-center gap-3.5 text-sm font-medium duration-300 ease-in-out hover:text-primary lg:text-base"
            >
              <i className="fa-solid fa-user"></i>
              My Profile
            </Link>
          </li>
          <button className="flex items-center gap-3.5 px-6 py-3 text-sm font-medium duration-300 ease-in-out hover:text-primary lg:text-base">
            <i className="fa-solid fa-right-from-bracket"></i>
            Log Out
          </button>
        </ul>
        <div className="flex items-center gap-3.5 px-6 py-3 text-sm font-medium duration-300 ease-in-out lg:text-base">
          <div className="grow">
            Dark mode
          </div>
          <DarkModeSwitcher />
        </div>
      </div>
    </div>
  );
};

export default DropdownUser;
