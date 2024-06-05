import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import { useDelete, useGet, usePatch } from "../../api/service";
import { success } from "../../utils/constants";
import {
  Combobox,
  ComboboxButton,
  ComboboxInput,
  ComboboxOption,
  ComboboxOptions,
} from "@headlessui/react";
import { useNavigate, useParams } from "react-router-dom";

interface Category {
  id: string;
  name: string;
}

interface FormData {
  id: string;
  name: string;
  categoryId: string;
  description: string;
}

const SubmitPatch = async (formData: FormData) => {
  return await usePatch("/subcategory", formData);
};

const SubmitDelete = async (id: string) => {
  return await useDelete("/subcategory/" + id);
};

const UpdateSubcategory = () => {
  const navigate = useNavigate();
  const { id } = useParams();

  const [category, setCategory] = useState<Category | null>(null);
  const [query, setQuery] = useState("");
  const [filteredCategories, setFilteredCategories] = useState([]);

  const [formData, setFormData] = useState<FormData>({
    id: "",
    name: "",
    categoryId: "",
    description: "",
  });

  const page = defaultPage;
  const pageSize = defaultSelectPageSize;

  const handleChange = (e: { target: { name: string; value: string } }) => {
    const { name, value } = e.target;
    setFormData((prevState) => ({ ...prevState, [name]: value }));
  };

  const { data: categoryData, error: categoryError } = useGet(
    "/category" + "?page=" + page + "&pageSize=" + pageSize
  );

  if (categoryError) toast.error(categoryError.message);

  const { data: subcategoryData, error: subcategoryError } = useGet("/subcategory/" + id);

  useEffect(() => {
    if (subcategoryData?.subcategory)
      setFormData({
        id: subcategoryData?.subcategory?.id,
        name: subcategoryData?.subcategory?.name,
        categoryId: subcategoryData?.subcategory?.category?.id,
        description: subcategoryData?.subcategory?.description,
      });
  }, [subcategoryData]);

  if (subcategoryError) {
    const message = subcategoryError?.response?.data?.Message;
    toast.error(message);
  }

  useEffect(() => {
    if (subcategoryData?.subcategory && categoryData?.categories) {
      const currentCategory = categoryData.categories.find((e: { id: string; }) => e.id === subcategoryData?.subcategory.category.id);
      setCategory(currentCategory);
    }
  }, [subcategoryData, categoryData])

  useEffect(() => {
    const filteredData = categoryData?.categories.filter(
      (category: Category) => {
        return category.name.toLowerCase().includes(query.toLowerCase());
      }
    );
    setFilteredCategories(filteredData);
  }, [categoryData?.categories, query]);

  const onSubmit = async (event: { preventDefault: () => void }) => {
    try {
      event.preventDefault();
      const response = await SubmitPatch(formData);
      if (response?.data?.status === success) {
        toast.success("Success");
        navigate("/subcategories");
      }
    } catch (error) {
      const errorMessage = error?.response?.data?.Message || error?.message;
      toast.error(errorMessage);
    }
  };

  const handleDelete = async () => {
    const confirmation = window.confirm(
      "Are you sure you want to delete this subcategory?"
    );
    if (confirmation) {
      try {
        const response = await SubmitDelete(id);
        if (response?.data?.status === success) {
          toast.success("Success");
          navigate("/subcategories");
        }
      } catch (error) {
        const errorMessage = error?.response?.data?.Message || error?.message;
        toast.error(errorMessage);
      }
    }
  };

  return (
    <div className="w-2/3 mx-auto rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
      <div className="flex flex-row items-center border-b border-stroke py-4 px-6 dark:border-strokedark">
        <h3 className="grow font-medium text-black dark:text-white">
          Update subcategory
        </h3>
        <button
          onClick={() => handleDelete()}
          className="flex items-center justify-center rounded bg-danger p-3 font-medium text-gray hover:bg-opacity-90 cursor-pointer"
        >
          <i className="fa-solid fa-trash"></i>
          <span className="ml-2">Delete this subcategory</span>
        </button>
      </div>
      <form onSubmit={onSubmit}>
        <div className="p-6">
          <div className="mb-4">
            <div>
              <input
                type="text"
                className="hidden"
                value={formData.id}
                required
                disabled
              />
            </div>

            <label className="mb-2 block text-black dark:text-white">
              Name <span className="text-meta-1">*</span>
            </label>
            <input
              type="text"
              placeholder="Enter subcategory name"
              className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
              name="name"
              value={formData.name}
              onChange={handleChange}
              required
            />
          </div>

          <div className="mb-4">
            <label className="mb-2 block text-black dark:text-white">
              Category <span className="text-meta-1">*</span>
            </label>
            <Combobox
              value={category}
              onChange={(value: Category) => {
                setCategory(value);
                setFormData((prevState) => ({
                  ...prevState,
                  categoryId: value.id,
                }));
              }}
              onClose={() => setQuery("")}
            >
              <div className="relative">
                <ComboboxInput
                  className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
                  placeholder="Enter category name"
                  displayValue={(category: Category) => category?.name}
                  onInput={(event) =>
                    setQuery((event.target as HTMLInputElement).value)
                  }
                  required
                />
                <ComboboxButton className="absolute inset-y-0 right-0 px-4">
                  <i className="fa-solid fa-chevron-down"></i>
                </ComboboxButton>
              </div>
              <ComboboxOptions anchor="bottom">
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
              name="description"
              value={formData.description}
              onChange={handleChange}
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

export default UpdateSubcategory;
