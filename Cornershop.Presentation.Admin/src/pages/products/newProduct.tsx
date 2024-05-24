import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import { useGet, useGetFrequent, usePut } from "../../api/service";
import { success } from "../../utils/constants";
import { useNavigate } from "react-router-dom";
import {
  Combobox,
  ComboboxButton,
  ComboboxInput,
  ComboboxOption,
  ComboboxOptions,
} from "@headlessui/react";

interface SimpleEntity {
  id: string;
  name: string;
}

interface FormData {
  name: string;
  code: string;
  description: string;
  subcategoryId: string;
  price: number;
  originalPrice: number;
  uploadedMainImage: string;
  uploadedImages: string[];
  width: number;
  height: number;
  length: number;
  pages: number;
  format: number;
  stock: number;
  publishedYear: number;
  authorId: string;
  publisherId: string;
  isVisible: boolean;
}

const SubmitForm = async (formData: FormData) => {
  try {
    return await usePut("/product", formData);
  } catch (error: any) {
    const message = error?.response?.data?.Message;
    toast.error(message);
  }
};

const NewProduct = () => {
  const navigate = useNavigate();

  const [category, setCategory] = useState<SimpleEntity | null>(null);
  const [subcategory, setSubcategory] = useState<SimpleEntity | null>(null);
  const [images, setImages] = useState<string[]>([]);
  const [mainImage, setMainImage] = useState<string>("");
  const [format, setFormat] = useState(0);
  const [isVisible, setIsVisible] = useState<boolean>(false);
  const [author, setAuthor] = useState<SimpleEntity | null>(null);
  const [publisher, setPublisher] = useState<SimpleEntity | null>(null);

  const [formData, setFormData] = useState<FormData>({
    name: "",
    code: "",
    description: "",
    subcategoryId: subcategory?.id ?? "",
    price: 0,
    originalPrice: 0,
    uploadedMainImage: mainImage,
    uploadedImages: images,
    width: 0,
    height: 0,
    length: 0,
    pages: 0,
    format: 0,
    stock: 0,
    publishedYear: 0,
    authorId: author?.id ?? "",
    publisherId: publisher?.id ?? "",
    isVisible: false,
  });

  const [categoryQuery, setCategoryQuery] = useState("");
  const [filteredCategories, setFilteredCategories] = useState([]);

  const [subcategoryQuery, setSubcategoryQuery] = useState("");
  const [filteredSubcategories, setFilteredSubcategories] = useState([]);

  const [publisherQuery, setPublisherQuery] = useState("");
  const [filteredPublishers, setFilteredPublishers] = useState([]);

  const [authorQuery, setAuthorQuery] = useState("");
  const [filteredAuthors, setFilteredAuthors] = useState([]);

  const page = 1;
  const pageSize = 128;

  const handleChange = (e: { target: { name: string; value: string } }) => {
    const { name, value } = e.target;
    setFormData((prevState) => ({ ...prevState, [name]: value }));
  };

  const handleImagesChange = (e: { target: { files: any } }) => {
    const files = e.target.files;
    console.log(files);
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
    const file = e.target.files[0];
    const promise = new Promise((resolve, reject) => {
      console.log(file);
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
    "/subcategory" +
      "?page=" +
      page +
      "&pageSize=" +
      pageSize +
      "&categoryId=" +
      category?.id
  );

  if (subcategoryError) toast.error(subcategoryError.message);

  const { data: authorData, error: authorError } = useGetFrequent(
    "/author" + "?page=" + page + "&pageSize=" + pageSize
  );

  if (authorError) toast.error(authorError.message);

  const { data: publisherData, error: publisherError } = useGet(
    "/publisher" + "?page=" + page + "&pageSize=" + pageSize
  );

  if (publisherError) toast.error(publisherError.message);

  useEffect(() => {
    const filteredData = categoryData?.categories.filter(
      (category: SimpleEntity) => {
        return category.name
          .toLowerCase()
          .includes(categoryQuery.toLowerCase());
      }
    );
    setFilteredCategories(filteredData);
  }, [categoryData?.categories, categoryQuery]);

  useEffect(() => {
    const filteredData = subcategoryData?.subcategories.filter(
      (subcategory: SimpleEntity) => {
        return subcategory.name
          .toLowerCase()
          .includes(subcategoryQuery.toLowerCase());
      }
    );
    setFilteredSubcategories(filteredData);
  }, [subcategoryData?.subcategories, subcategoryQuery]);

  useEffect(() => {
    const filteredData = publisherData?.publishers.filter(
      (publisher: SimpleEntity) => {
        return publisher.name
          .toLowerCase()
          .includes(publisherQuery.toLowerCase());
      }
    );
    setFilteredPublishers(filteredData);
  }, [publisherData?.publishers, publisherQuery]);

  useEffect(() => {
    const filteredData = authorData?.authors.filter((author: SimpleEntity) => {
      return author.name.toLowerCase().includes(authorQuery.toLowerCase());
    });
    setFilteredAuthors(filteredData);
  }, [authorData?.authors, authorQuery]);

  useEffect(() => {
    setFormData((prevState) => ({
      ...prevState,
      uploadedImages: images,
    }));
  }, [images]);

  useEffect(() => {
    setFormData((prevState) => ({
      ...prevState,
      uploadedMainImage: mainImage,
    }));
  }, [mainImage]);

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
              onChange={(value) => {
                setCategory(value);
                setSubcategory(null);
              }}
              onClose={() => setCategoryQuery("")}
            >
              <div className="relative">
                <ComboboxInput
                  className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
                  placeholder="Choose category"
                  displayValue={(category: SimpleEntity) => category?.name}
                  onInput={(event) => setCategoryQuery(event)}
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
                {filteredCategories?.map((category: SimpleEntity) => (
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
              onChange={(value) => {
                setSubcategory(value);
                setFormData((prevState) => ({
                  ...prevState,
                  subcategoryId: value?.id ?? "",
                }));
              }}
              onClose={() => setSubcategoryQuery("")}
              disabled={category ? false : true}
            >
              <div className="relative">
                <ComboboxInput
                  className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
                  placeholder="Choose subcategory"
                  displayValue={(subcategory: SimpleEntity) =>
                    subcategory?.name
                  }
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
                {filteredSubcategories?.map((subcategory: SimpleEntity) => (
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

          <div className="mb-4">
            <label className="mb-2 block text-black dark:text-white">
              Author <span className="text-meta-1">*</span>
            </label>
            <Combobox
              value={author}
              onChange={(value: SimpleEntity) => {
                setAuthor(value);
                setFormData((prevState) => ({
                  ...prevState,
                  authorId: value?.id ?? "",
                }));
              }}
              onClose={() => setAuthorQuery("")}
            >
              <div className="relative">
                <ComboboxInput
                  className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
                  placeholder="Choose author"
                  displayValue={(author: SimpleEntity) => author?.name}
                  onInput={(event) => setAuthorQuery(event.target.value)}
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
                {filteredAuthors?.map((author: SimpleEntity) => (
                  <ComboboxOption
                    key={author?.id}
                    value={author}
                    className="w-[var(--input-width)] [--anchor-gap:var(--spacing-1)] cursor-pointer bg-slate-100 data-[focus]:bg-slate-200 dark:bg-slate-800 dark:data-[focus]:bg-slate-700"
                  >
                    <div className="text-sm p-4 text-black dark:text-white">
                      {author?.name}
                    </div>
                  </ComboboxOption>
                ))}
              </ComboboxOptions>
            </Combobox>
          </div>

          <div className="mb-4">
            <label className="mb-2 block text-black dark:text-white">
              Publisher <span className="text-meta-1">*</span>
            </label>
            <Combobox
              value={publisher}
              onChange={(value: SimpleEntity) => {
                setPublisher(value);
                setFormData((prevState) => ({
                  ...prevState,
                  publisherId: value?.id ?? "",
                }));
              }}
              onClose={() => setPublisherQuery("")}
            >
              <div className="relative">
                <ComboboxInput
                  className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
                  placeholder="Choose publisher"
                  displayValue={(publisher: SimpleEntity) => publisher?.name}
                  onInput={(event) => setPublisherQuery(event.target.value)}
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
                {filteredPublishers?.map((publisher: SimpleEntity) => (
                  <ComboboxOption
                    key={publisher?.id}
                    value={publisher}
                    className="w-[var(--input-width)] [--anchor-gap:var(--spacing-1)] cursor-pointer bg-slate-100 data-[focus]:bg-slate-200 dark:bg-slate-800 dark:data-[focus]:bg-slate-700"
                  >
                    <div className="text-sm p-4 text-black dark:text-white">
                      {publisher?.name}
                    </div>
                  </ComboboxOption>
                ))}
              </ComboboxOptions>
            </Combobox>
          </div>

          <div className="mb-6 grid grid-cols-1 lg:grid-cols-2 lg:gap-4">
            <div className="mb-4">
              <label className="mb-2 block text-black dark:text-white">
                Price <span className="text-meta-1">*</span>
              </label>
              <input
                type="number"
                min="0"
                placeholder="Enter price"
                className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
                name="price"
                value={formData.price}
                onChange={handleChange}
                required
              ></input>
            </div>
            <div className="mb-4">
              <label className="mb-2 block text-black dark:text-white">
                Original price <span className="text-meta-1">*</span>
              </label>
              <input
                type="number"
                min="0"
                placeholder="Enter original price"
                className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
                name="originalPrice"
                value={formData.originalPrice}
                onChange={handleChange}
                required
              ></input>
            </div>
          </div>

          <div className="mb-6 grid grid-cols-1 lg:grid-cols-3 lg:gap-4">
            <div className="mb-4">
              <label className="mb-2 block text-black dark:text-white">
                Width (mm)<span className="text-meta-1">*</span>
              </label>
              <input
                type="number"
                min="0"
                placeholder="Enter width (mm)"
                className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
                name="width"
                value={formData.width}
                onChange={handleChange}
                required
              ></input>
            </div>
            <div className="mb-4">
              <label className="mb-2 block text-black dark:text-white">
                Length (mm)<span className="text-meta-1">*</span>
              </label>
              <input
                type="number"
                min="0"
                placeholder="Enter length (mm)"
                className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
                name="length"
                value={formData.length}
                onChange={handleChange}
                required
              ></input>
            </div>
            <div className="mb-4">
              <label className="mb-2 block text-black dark:text-white">
                Height (mm)<span className="text-meta-1">*</span>
              </label>
              <input
                type="number"
                min="0"
                placeholder="Enter height (mm)"
                className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
                name="height"
                value={formData.height}
                onChange={handleChange}
                required
              ></input>
            </div>
          </div>

          <div className="mb-6 grid grid-cols-1 lg:grid-cols-2 lg:gap-4">
            <div className="mb-4">
              <label className="mb-2 block text-black dark:text-white">
                Pages <span className="text-meta-1">*</span>
              </label>
              <input
                type="number"
                min="0"
                placeholder="Enter pages count"
                className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
                name="pages"
                value={formData.pages}
                onChange={handleChange}
                required
              ></input>
            </div>
            <div className="mb-4">
              <label className="mb-2 block text-black dark:text-white">
                Stock <span className="text-meta-1">*</span>
              </label>
              <input
                type="number"
                min="0"
                placeholder="Enter stock count"
                className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
                name="stock"
                value={formData.stock}
                onChange={handleChange}
                required
              ></input>
            </div>
          </div>

          <div className="mb-6 grid grid-cols-1 lg:grid-cols-2 lg:gap-4">
            <div className="mb-4">
              <label className="mb-2 block text-black dark:text-white">
                Format <span className="text-meta-1">*</span>
              </label>
              <div className="mb-4">
                <div className="relative z-20 bg-transparent dark:bg-form-input">
                  <select
                    value={format}
                    onChange={(e) => {
                      setFormat(e.target.value);
                    }}
                    className="relative z-20 w-full appearance-none rounded border border-stroke bg-transparent py-3 px-5 outline-none transition focus:border-primary active:border-primary dark:border-form-strokedark dark:bg-form-input dark:focus:border-primary text-black dark:text-white"
                  >
                    <option value="0" className="text-body dark:text-bodydark">
                      Paperback
                    </option>
                    <option value="1" className="text-body dark:text-bodydark">
                      Hardcover
                    </option>
                    <option value="2" className="text-body dark:text-bodydark">
                      Massmarket
                    </option>
                  </select>

                  <span className="absolute top-1/2 right-4 z-30 -translate-y-1/2">
                    <i className="fa-solid fa-chevron-down"></i>
                  </span>
                </div>
              </div>
            </div>
            <div className="mb-4">
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

          <div className="mb-6 grid grid-cols-1 lg:grid-cols-2 lg:gap-4">
            <div className="mb-4">
              <label className="mb-2 block text-black dark:text-white">
                Main image <span className="text-meta-1">*</span>
              </label>
              <input
                className="overflow-hidden"
                type="file"
                onChange={handleMainImageChange}
              />
            </div>
            <div className="mb-4">
              <label className="mb-2 block text-black dark:text-white">
                Images (multiple) <span className="text-meta-1">*</span>
              </label>
              <input
                className="overflow-hidden"
                type="file"
                multiple
                onChange={handleImagesChange}
              />
            </div>
          </div>

          <div className="mb-6">
            <label className="mb-2 block text-black dark:text-white">
              Visible to customers <span className="text-meta-1">*</span>
            </label>
            <div className="mb-4">
              <div className="relative z-20 bg-transparent dark:bg-form-input">
                <select
                  value={isVisible}
                  onChange={(e) => {
                    setIsVisible(e.target.value);
                  }}
                  className="relative z-20 w-full appearance-none rounded border border-stroke bg-transparent py-3 px-5 outline-none transition focus:border-primary active:border-primary dark:border-form-strokedark dark:bg-form-input dark:focus:border-primary text-black dark:text-white"
                >
                  <option
                    value="false"
                    className="text-body dark:text-bodydark"
                  >
                    Not visible
                  </option>
                  <option value="true" className="text-body dark:text-bodydark">
                    Visible
                  </option>
                </select>

                <span className="absolute top-1/2 right-4 z-30 -translate-y-1/2">
                  <i className="fa-solid fa-chevron-down"></i>
                </span>
              </div>
            </div>
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
