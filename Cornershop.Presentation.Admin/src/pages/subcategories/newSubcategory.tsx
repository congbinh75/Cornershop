import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import { useGet, usePut } from "../../api/service";
import { success } from "../../utils/constants";
import {
  Combobox,
  ComboboxButton,
  ComboboxInput,
  ComboboxOption,
  ComboboxOptions,
} from "@headlessui/react";

interface Category {
  id: string;
  name: string;
}

const SubmitForm = async (
  name: string,
  categoryId: string | undefined,
  description: string
) => {
  return await usePut("/subcategory", {
    name: name,
    categoryId: categoryId,
    description: description,
  });
};

const NewSubcategory = () => {
  const [name, setName] = useState("");
  const [category, setCategory] = useState<Category | null>(null);
  const [description, setDescription] = useState("");
  const [query, setQuery] = useState("");
  const [filteredCategories, setFilteredCategories] = useState([]);

  const page = 1;
  const pageSize = 128;

  const { data, error } = useGet(
    "/category" + "?page=" + page + "&pageSize=" + pageSize
  );

  if (error) toast.error(error.message);

  const onSubmit = async (event: { preventDefault: () => void }) => {
    event.preventDefault();
    const response = await SubmitForm(name, category?.id, description);
    if (response?.data?.status === success) toast.success("Success");

    setName("");
    setCategory(null);
    setDescription("");
  };

  useEffect(() => {
    const filteredData = data?.categories.filter((category: Category) => {
      return category.name.toLowerCase().includes(query.toLowerCase());
    });
    setFilteredCategories(filteredData);
  }, [data?.categories, query]);

  return (
    <div className="w-2/3 mx-auto rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
      <div className="border-b border-stroke py-4 px-6 dark:border-strokedark">
        <h3 className="font-medium text-black dark:text-white">
          Create new subcategory
        </h3>
      </div>
      <form onSubmit={onSubmit}>
        <div className="p-6">
          <div className="mb-4">
            <label className="mb-2 block text-black dark:text-white">
              Name <span className="text-meta-1">*</span>
            </label>
            <input
              type="text"
              placeholder="Enter subcategory name"
              className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
              value={name}
              onChange={(e) => setName(e.target.value)}
              required
            />
          </div>
          <div className="mb-4">
            <label className="mb-2 block text-black dark:text-white">
              Category <span className="text-meta-1">*</span>
            </label>
            <Combobox
              value={category}
              onChange={(value: Category) => setCategory(value)}
              onClose={() => setQuery("")}
            >
              <div className="relative">
                <ComboboxInput
                  className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
                  placeholder="Enter category name"
                  displayValue={(category: Category) => category?.name}
                  onInput={(event) => setQuery(event.target.value)}
                  required
                />
                <ComboboxButton className="absolute inset-y-0 right-0 px-4">
                  <i className="fa-solid fa-chevron-down"></i>
                </ComboboxButton>
              </div>
              <ComboboxOptions
                anchor="bottom"
                className="empty:hidden overflow-hidden"
              >
                {filteredCategories?.map((category: Category) => (
                  <ComboboxOption
                    key={category.id}
                    value={category}
                    className="w-[var(--input-width)] [--anchor-gap:var(--spacing-1)] cursor-pointer bg-slate-100 data-[focus]:bg-slate-200 dark:bg-slate-800 dark:data-[focus]:bg-slate-700"
                  >
                    <div className="text-sm p-4 text-black dark:text-white">
                      {category.name}
                    </div>
                  </ComboboxOption>
                ))}
              </ComboboxOptions>
            </Combobox>
          </div>
          <div className="mb-6">
            <label className="mb-2 block text-black dark:text-white">
              Description
            </label>
            <textarea
              rows={6}
              placeholder="Enter subcategory description"
              className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
              value={description}
              onChange={(e) => setDescription(e.target.value)}
            ></textarea>
          </div>
          <input
            type="submit"
            className="flex w-full justify-center rounded bg-primary p-3 font-medium text-gray hover:bg-opacity-90 cursor-pointer"
            value="Submit"
          ></input>
        </div>
      </form>
    </div>
  );
};

export default NewSubcategory;
