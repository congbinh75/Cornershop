import { useState } from "react";
import { defaultPageSize } from "../../utils/constants";
import { getDateFromString } from "../../utils/functions";
import { Roles } from "../../utils/enums";
import { toast } from "react-toastify";
import { useGet } from "../../api/service";
import TablePageControl from "../../components/table/tablePageControl";

interface User {
  firstName: string;
  lastName: string;
  username: string;
  email: string;
  role: number;
  createdOn: string;
}

const Users = () => {
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(defaultPageSize);

  const { data, error, mutate } = useGet(
    "/user" + "?page=" + page + "&pageSize=" + pageSize
  );

  if (error) toast.error(error.message);

  return (
    <div>
      {/* <div className="flex flex-row w-full mb-5">
        <div className="grow">
          <button className="inline-flex align-middle items-center justify-center rounded-md bg-primary p-4 text-center font-medium text-white hover:bg-opacity-90 gap-4">
            <i className="fa-solid fa-plus"></i>
            <span>New staff</span>
          </button>
        </div>
        <div>
          <div className="flex flex-row gap-4">
            <input
              type="text"
              placeholder="Search..."
              className="w-72 rounded-lg border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
            />
            <button className="inline-flex items-center justify-center rounded-md bg-primary p-4 text-center font-medium text-white hover:bg-opacity-90">
              <i className="fa-solid fa-magnifying-glass"></i>
            </button>
          </div>
        </div>
      </div> */}
      <div className="mb-5 rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
        <div className="grid grid-cols-8 border-t border-stroke py-4.5 px-4 dark:border-strokedark sm:grid-cols-8 md:px-6 2xl:px-7.5">
          <div className="col-span-1 flex items-center">
            <p className="font-medium">First name</p>
          </div>
          <div className="col-span-1 flex items-center">
            <p className="font-medium">Last name</p>
          </div>
          <div className="col-span-1 hidden items-center sm:flex">
            <p className="font-medium">Username</p>
          </div>
          <div className="col-span-2 hidden items-center sm:flex">
            <p className="font-medium">Email</p>
          </div>
          <div className="col-span-1 flex items-center">
            <p className="font-medium">Role</p>
          </div>
          <div className="col-span-2 flex items-center">
            <p className="font-medium">Created date</p>
          </div>
          <div className="col-span-1 flex items-center"></div>
        </div>

        {data?.users.length <= 0 ? (
          <div className="py-4 px-4 border border-stroke dark:border-strokedark md:px-6 2xl:px-8">
            <p className="mx-auto w-fit">No data</p>
          </div>
        ) : (
          data?.users.map((user: User, key: string) => (
            <div
              className="grid grid-cols-8 py-4 px-4 border border-stroke dark:border-strokedark sm:grid-cols-8 md:px-6 2xl:px-8"
              key={key}
            >
              <div className="col-span-1 flex items-center">
                <div className="flex flex-col gap-4 sm:flex-row sm:items-center">
                  <p className="text-sm text-black dark:text-white line-clamp-1">
                    {user.firstName}
                  </p>
                </div>
              </div>
              <div className="col-span-1 flex items-center">
                <p className="text-sm text-black dark:text-white line-clamp-1">
                  {user.lastName}
                </p>
              </div>
              <div className="col-span-1 flex items-center">
                <p className="text-sm text-black dark:text-white line-clamp-1">
                  {user.username}
                </p>
              </div>
              <div className="col-span-2 flex items-center">
                <p className="text-sm text-black dark:text-white line-clamp-1">
                  {user.email}
                </p>
              </div>
              <div className="col-span-1 flex items-center">
                <p className="text-sm text-black dark:text-white line-clamp-1">
                  {Roles[user?.role]}
                </p>
              </div>
              <div className="col-span-1 flex items-center">
                <p className="text-sm text-black dark:text-white line-clamp-1">
                  {getDateFromString(user.createdOn)}
                </p>
              </div>
              <div className="col-span-1 flex items-center">
                <button className="text-sm text-black dark:text-white">
                  <i className="fa-solid fa-pen"></i>
                </button>
              </div>
            </div>
          ))
        )}
      </div>
      <TablePageControl pagesCount={data?.pagesCount} page={page} setPage={setPage} pageSize={pageSize} setPageSize={setPageSize} mutate={mutate} />
    </div>
  );
};

export default Users;
