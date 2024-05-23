import { toast } from "react-toastify";
import { defaultPageSize } from "../../utils/constants";
import { useState } from "react";
import { useGet } from "../../api/service";
import { Select } from "@headlessui/react";
import { Link } from "react-router-dom";

interface Publisher {
  name: string;
  description: string;
}

const Publishers = () => {
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(defaultPageSize);

  const { data, error, mutate } = useGet(
    "/publisher" + "?page=" + page + "&pageSize=" + pageSize
  );

  if (error) {
    toast.error(error.response?.data?.message);
  }

  return (
    <div>
      <div className="flex flex-row w-full mb-5">
        <div className="grow">
          <Link
            to="/publishers/create"
            className="inline-flex align-middle items-center justify-center rounded-md bg-primary p-4 text-center font-medium text-white hover:bg-opacity-90 gap-4"
          >
            <i className="fa-solid fa-plus"></i>
            <span>New publisher</span>
          </Link>
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
      </div>
      <div className="mb-5 rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
        <div className="grid grid-cols-8 border border-stroke py-4 px-4 dark:border-strokedark sm:grid-cols-8 md:px-6 2xl:px-8">
          <div className="col-span-3 flex items-center">
            <p className="font-medium">Name</p>
          </div>
          <div className="col-span-4 flex items-center">
            <p className="font-medium">Description</p>
          </div>
          <div className="col-span-1 flex items-center"></div>
        </div>

        {data?.publishers?.length <= 0 ? (
          <div className="py-4 px-4 border border-stroke dark:border-strokedark md:px-6 2xl:px-8">
            <p className="mx-auto w-fit">No data</p>
          </div>
        ) : (
          data?.publishers?.map((publisher: Publisher, key: string) => (
            <div
              className="grid grid-cols-8 py-4 px-4 border border-stroke dark:border-strokedark sm:grid-cols-8 md:px-6 2xl:px-8"
              key={key}
            >
              <div className="col-span-3 flex items-center">
                <div className="flex flex-col gap-4 sm:flex-row sm:items-center">
                  <p className="text-sm text-black dark:text-white line-clamp-1">
                    {publisher.name}
                  </p>
                </div>
              </div>
              <div className="col-span-4 flex items-center">
                <p className="text-sm text-black dark:text-white line-clamp-1">
                  {publisher.description}
                </p>
              </div>
              <div className="col-span-1 flex items-center justify-center">
                <button className="text-sm text-black dark:text-white line-clamp-1">
                  <i className="fa-solid fa-pen"></i>
                </button>
              </div>
            </div>
          ))
        )}
      </div>
      <div className="flex flex-row grow gap-4">
        <Select
          name="pageSize"
          aria-label="Page size"
          className="inline-flex items-center justify-center rounded-md bg-inherit border border-stroke p-4 text-center font-medium text-black dark:border-form-strokedark dark:text-white"
          onChange={(e) => {
            if (Number(e.target.value) !== pageSize) {
              setPageSize(Number(e.target.value));
              mutate();
            }
          }}
        >
          <option value="15">15</option>
          <option value="30">30</option>
          <option value="45">45</option>
        </Select>
        <button
          className="inline-flex items-center justify-center rounded-md border border-stroke p-4 text-center font-medium text-black dark:border-form-strokedark dark:text-white"
          onClick={() => {
            if (page > 1) {
              setPage(page - 1);
              mutate();
            }
          }}
        >
          <i className="fa-solid fa-arrow-left"></i>
        </button>
        <span className="inline-flex items-center justify-center">
          {page + "/" + data?.pagesCount}
        </span>
        <button
          className="inline-flex items-center justify-center rounded-md border border-stroke p-4 text-center font-medium text-black dark:border-form-strokedark dark:text-white"
          onClick={() => {
            if (page < data?.pagesCount) {
              setPage(page + 1);
              mutate();
            }
          }}
        >
          <i className="fa-solid fa-arrow-right"></i>
        </button>
      </div>
    </div>
  );
};

export default Publishers;