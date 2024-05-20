const Users = () => {
  return (
    <div>
      <div className="flex flex-row w-full mb-5">
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
      </div>
      <div className="mb-5 rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
        <div className="grid grid-cols-6 border-t border-stroke py-4.5 px-4 dark:border-strokedark sm:grid-cols-8 md:px-6 2xl:px-7.5">
          <div className="col-span-2 flex items-center">
            <p className="font-medium">Name</p>
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
          <div className="col-span-1 flex items-center">
            <p className="font-medium">CreatedDate</p>
          </div>
          <div className="col-span-1 flex items-center"></div>
        </div>

        {/* {categoryData.map((product, key) => ( */}
          <div
            className="grid grid-cols-6 border-t border-stroke py-4.5 px-4 dark:border-strokedark sm:grid-cols-8 md:px-6 2xl:px-7.5"
            // key={key}
          >
            <div className="col-span-4 flex items-center">
              <div className="flex flex-col gap-4 sm:flex-row sm:items-center">
                <p className="text-sm text-black dark:text-white">
                  {/* {product.name} */}
                </p>
              </div>
            </div>
            <div className="col-span-1 flex items-center">
              <p className="text-sm text-black dark:text-white">
                {/* {product.subcategories} */}
              </p>
            </div>
            <div className="col-span-1 flex items-center">
              <p className="text-sm text-black dark:text-white">
                {/* {product.products} */}
              </p>
            </div>
            <div className="col-span-1 flex items-center">
              <p className="text-sm text-black dark:text-white">
                {/* {product.stock} */}
              </p>
            </div>
            <div className="col-span-1 flex items-center">
              <button className="text-sm text-black dark:text-white">
                <i className="fa-solid fa-pen"></i>
              </button>
            </div>
          </div>
        {/* ))} */}
      </div>
      <div className="flex flex-row grow justify-center gap-4">
        <button className="inline-flex items-center justify-center rounded-md border border-stroke p-4 text-center font-medium text-black hover:bg-opacity-90 dark:border-form-strokedark dark:text-white">
          <i className="fa-solid fa-arrow-left"></i>
        </button>
        <span></span>
        <button className="inline-flex items-center justify-center rounded-md border border-stroke p-4 text-center font-medium text-black hover:bg-opacity-90 dark:border-form-strokedark dark:text-white">
          <i className="fa-solid fa-arrow-right"></i>
        </button>
      </div>
    </div>
  );
};

export default Users;
