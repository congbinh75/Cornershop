import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import { useGet, usePut } from "../../api/service";
import { success } from "../../utils/constants";
import { useNavigate } from "react-router-dom";
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

interface Subcategory {
  id: string;
  name: string;
}

interface FormData {
  name: string;
  code: string;
  description: string;
  categoryId: string;
  subcategoryId: string;
  price: number;
  originalPrice: number;
  uploadedImages: string[];
  width: number;
  height: number;
  length: number;
  pages: number;
  format: number;
  stock: number;
  publishedYear: number;
  authorsIds: string[];
  publisherId: string;
  isVisible: boolean;
}

const SubmitForm = async (formData: FormData) => {
  return await usePut("/product", formData);
};

const NewProduct = () => {
  const navigate = useNavigate();

  const [category, setCategory] = useState<Category | null>(null);
  const [subcategory, setSubcategory] = useState<Subcategory | null>(null);
  const [images, setImages] = useState();
  const [mainImage, setMainImage] = useState();

  const [formData, setFormData] = useState<FormData>({
    name: "",
    code: "",
    description: "",
    categoryId: category?.id ?? "",
    subcategoryId: subcategory?.id ?? "",
    price: 0,
    originalPrice: 0,
    uploadedImages: [],
    width: 0,
    height: 0,
    length: 0,
    pages: 0,
    format: 0,
    stock: 0,
    publishedYear: 0,
    authorsIds: [],
    publisherId: "",
    isVisible: false,
  });

  const [categoryQuery, setCategoryQuery] = useState("");
  const [filteredCategories, setFilteredCategories] = useState([]);

  const [subcategoryQuery, setSubcategoryQuery] = useState("");
  const [filteredSubcategories, setFilteredSubcategories] = useState([]);

  const page = 1;
  const pageSize = 128;

  const handleChange = (e: { target: { name: string; value: string } }) => {
    const { name, value } = e.target;
    setFormData((prevState) => ({ ...prevState, [name]: value }));
  };

  const handleImagesChange = (e) => {
    const files = e.target.files;
    const promises = [];
    for (let i = 0; i < files.length; i++) {
      const file = files[i];
      promises.push(
        new Promise((resolve, reject) => {
          const reader = new FileReader();
          reader.readAsDataURL(file);
          reader.onload = () => resolve(reader.result);
          reader.onerror = (error) => reject(error);
        })
      );
    }
    Promise.all(promises)
      .then((base64Images) => {
        setImages(base64Images);
      })
      .catch((error) => {
        console.error("Error encoding files: ", error);
      });
  };

  const handleMainImageChange = (e) => {
    const file = e.target.file;
    const promise = new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => resolve(reader.result);
      reader.onerror = (error) => reject(error);
    });
    promise
      .then((base64Image) => {
        setMainImage(base64Image);
      })
      .catch((error) => {
        console.error("Error encoding file: ", error);
      });
  };

  const onSubmit = async (event: { preventDefault: () => void }) => {
    event.preventDefault();
    const response = await SubmitForm(formData);
    if (response?.data?.status === success) {
      toast.success("Success");
      navigate("/products");
    }
  };

  const { data: categoryData, error: categoryError } = useGet(
    "/category" + "?page=" + page + "&pageSize=" + pageSize
  );

  if (categoryError) toast.error(categoryError.message);

  const { data: subcategoryData, error: subcategoryError } = useGet(
    "/subcategory" + "?page=" + page + "&pageSize=" + pageSize
  );

  if (subcategoryError) toast.error(subcategoryError.message);

  useEffect(() => {
    const filteredData = categoryData?.categories.filter(
      (category: Category) => {
        return category.name
          .toLowerCase()
          .includes(categoryQuery.toLowerCase());
      }
    );
    setFilteredCategories(filteredData);
  }, [categoryData?.categories, categoryQuery]);

  useEffect(() => {
    const filteredData = subcategoryData?.subcategories.filter(
      (subcategory: Subcategory) => {
        return subcategory.name
          .toLowerCase()
          .includes(subcategoryQuery.toLowerCase());
      }
    );
    setFilteredSubcategories(filteredData);
  }, [subcategoryData?.subcategories, subcategoryQuery]);

  return (
    <div className="w-full lg:w-2/3 mx-auto rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
      <div className="border-b border-stroke py-4 px-6 dark:border-strokedark">
        <h3 className="font-medium text-black dark:text-white">
          Create new product
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
              placeholder="Enter product name"
              className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
              name="name"
              value={formData.name}
              onChange={handleChange}
              required
            />
          </div>

          <div className="mb-4">
            <label className="mb-2 block text-black dark:text-white">
              Code <span className="text-meta-1">*</span>
            </label>
            <input
              type="text"
              placeholder="Enter product code"
              className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
              name="code"
              value={formData.code}
              onChange={handleChange}
              required
            />
          </div>

          <div className="mb-6">
            <label className="mb-2 block text-black dark:text-white">
              Description
            </label>
            <textarea
              rows={6}
              placeholder="Enter product description"
              className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
              name="description"
              value={formData.description}
              onChange={handleChange}
            ></textarea>
          </div>

          <div className="mb-4">
            <label className="mb-2 block text-black dark:text-white">
              Category <span className="text-meta-1">*</span>
            </label>
            <Combobox
              value={category}
              onChange={(value) => setCategory(value)}
              onClose={() => setCategoryQuery("")}
            >
              <div className="relative">
                <ComboboxInput
                  className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
                  placeholder="Choose category"
                  displayValue={(category: Category) => category?.name}
                  onInput={(event) => setCategoryQuery(event.target.value)}
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
                    key={category?.id}
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

          <div className="mb-4">
            <label className="mb-2 block text-black dark:text-white">
              Subcategory <span className="text-meta-1">*</span>
            </label>
            <Combobox
              value={subcategory}
              onChange={(value: Category) => setSubcategory(value)}
              onClose={() => setSubcategoryQuery("")}
            >
              <div className="relative">
                <ComboboxInput
                  className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
                  placeholder="Choose subcategory"
                  displayValue={(subcategory: Subcategory) => subcategory?.name}
                  onInput={(event) => setCategoryQuery(event.target.value)}
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
                {filteredSubcategories?.map((subcategory: Subcategory) => (
                  <ComboboxOption
                    key={subcategory?.id}
                    value={subcategory}
                    className="w-[var(--input-width)] [--anchor-gap:var(--spacing-1)] cursor-pointer bg-slate-100 data-[focus]:bg-slate-200 dark:bg-slate-800 dark:data-[focus]:bg-slate-700"
                  >
                    <div className="text-sm p-4 text-black dark:text-white">
                      {subcategory.name}
                    </div>
                  </ComboboxOption>
                ))}
              </ComboboxOptions>
            </Combobox>
          </div>

          <div className="mb-6 flex flex-row gap-4">
            <div className="grow">
              <label className="mb-2 block text-black dark:text-white">
                Price <span className="text-meta-1">*</span>
              </label>
              <input
                type="number"
                placeholder="Enter price"
                className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
                name="price"
                value={formData.price}
                onChange={handleChange}
                required
              ></input>
            </div>
            <div className="grow">
              <label className="mb-2 block text-black dark:text-white">
                Original price <span className="text-meta-1">*</span>
              </label>
              <input
                type="number"
                placeholder="Enter original price"
                className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
                name="originalPrice"
                value={formData.originalPrice}
                onChange={handleChange}
                required
              ></input>
            </div>
          </div>

          <div className="mb-6 flex flex-row gap-4">
            <div className="grow">
              <label className="mb-2 block text-black dark:text-white">
                Width (mm)<span className="text-meta-1">*</span>
              </label>
              <input
                type="number"
                placeholder="Enter width (mm)"
                className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
                name="width"
                value={formData.width}
                onChange={handleChange}
                required
              ></input>
            </div>
            <div className="grow">
              <label className="mb-2 block text-black dark:text-white">
                Length (mm)<span className="text-meta-1">*</span>
              </label>
              <input
                type="number"
                placeholder="Enter length (mm)"
                className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
                name="length"
                value={formData.length}
                onChange={handleChange}
                required
              ></input>
            </div>
            <div className="grow">
              <label className="mb-2 block text-black dark:text-white">
                Height (mm)<span className="text-meta-1">*</span>
              </label>
              <input
                type="number"
                placeholder="Enter height (mm)"
                className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
                name="height"
                value={formData.height}
                onChange={handleChange}
                required
              ></input>
            </div>
          </div>

          <div className="mb-6 flex flex-row gap-4">
            <div className="grow">
              <label className="mb-2 block text-black dark:text-white">
                Pages <span className="text-meta-1">*</span>
              </label>
              <input
                type="number"
                placeholder="Enter pages count"
                className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
                name="pages"
                value={formData.pages}
                onChange={handleChange}
                required
              ></input>
            </div>
            <div className="grow">
              <label className="mb-2 block text-black dark:text-white">
                Stock <span className="text-meta-1">*</span>
              </label>
              <input
                type="number"
                placeholder="Enter stock count"
                className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
                name="stock"
                value={formData.stock}
                onChange={handleChange}
                required
              ></input>
            </div>
          </div>

          <div className="mb-6 flex flex-row gap-4">
            <div className="grow">
              <label className="mb-2 block text-black dark:text-white">
                Format <span className="text-meta-1">*</span>
              </label>
              <input
                type="number"
                placeholder="Enter format"
                className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
                name="format"
                value={formData.format}
                onChange={handleChange}
                required
              ></input>
            </div>
            <div className="grow">
              <label className="mb-2 block text-black dark:text-white">
                Published year <span className="text-meta-1">*</span>
              </label>
              <input
                type="text"
                placeholder="Enter published year"
                className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
                name="publishedYear"
                value={formData.publishedYear}
                onChange={handleChange}
                required
              />
            </div>
          </div>

          <div className="mb-6">
            <label className="mb-2 block text-black dark:text-white">
              Main image <span className="text-meta-1">*</span>
            </label>
            <input type="file" onChange={handleMainImageChange} />
          </div>

          <div className="mb-6">
            <label className="mb-2 block text-black dark:text-white">
              Images (multiple) <span className="text-meta-1">*</span>
            </label>
            <input type="file" multiple onChange={handleImagesChange} />
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

export default NewProduct;
