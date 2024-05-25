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

interface SimpleEntity {
  id: string;
  name: string;
}

interface FormData {
  name: string;
  description: string;
  subcategoryId: string;
  price: number;
  originalPrice: number;
  uploadedMainImageFile: string;
  uploadedImagesFiles: string[];
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
  return await usePut("/product", formData);
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
    description: "",
    subcategoryId: subcategory?.id ?? "",
    price: 0,
    originalPrice: 0,
    uploadedMainImageFile: mainImage,
    uploadedImagesFiles: images,
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

  const handleImagesChange = (e: React.FormEvent<HTMLInputElement>) => {
    const files = (e.target as HTMLInputElement).files;
    console.log(files);
    const promises: Promise<string>[] = [];
    if (files) {
      for (let i = 0; i < files.length; i++) {
        const file = files[i];
        promises.push(
          new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = () => resolve(reader.result as string);
            reader.onerror = (error) => reject(error);
          })
        );
      }
      Promise.all(promises)
        .then((base64Images: string[]) => {
          setImages(base64Images);
        })
        .catch((error) => {
          console.error("Error encoding files: ", error);
        });
    }
  };

  const handleMainImageChange = (e: React.FormEvent<HTMLInputElement>) => {
    const file = (e.target as HTMLInputElement).files[0];
    const promise: Promise<string> = new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => resolve(reader.result as string);
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
    try {
      event.preventDefault();
      const response = await SubmitForm(formData);
      if (response?.data?.status === success) {
        toast.success("Success");
        navigate("/products");
      }
    } catch (error) {
      const errorMessage = error?.response?.data?.message || error?.message;
      toast.error(errorMessage);
    }
  };

  const { data: categoryData, error: categoryError } = useGet(
    "/category" + "?page=" + page + "&pageSize=" + pageSize
  );

  if (categoryError) {
    const errorMessage =
      categoryError?.response?.data?.message || categoryError?.message;
    toast.error(errorMessage);
  }

  const { data: subcategoryData, error: subcategoryError } = useGet(
    "/subcategory" +
      "?page=" +
      page +
      "&pageSize=" +
      pageSize +
      "&categoryId=" +
      category?.id
  );

  if (subcategoryError) {
    const errorMessage =
      subcategoryError?.response?.data?.message || subcategoryError?.message;
    toast.error(errorMessage);
  }

  const { data: authorData, error: authorError } = useGet(
    "/author" + "?page=" + page + "&pageSize=" + pageSize
  );

  if (authorError) {
    const errorMessage =
      authorError?.response?.data?.message || authorError?.message;
    toast.error(errorMessage);
  }

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
      UploadedImagesFiles: images,
    }));
  }, [images]);

  useEffect(() => {
    setFormData((prevState) => ({
      ...prevState,
      uploadedMainImageFile: mainImage,
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
                  onInput={(event) =>
                    setCategoryQuery((event.target as HTMLInputElement).value)
                  }
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
                  onInput={(event) =>
                    setAuthorQuery((event.target as HTMLInputElement).value)
                  }
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
                  onInput={(event) =>
                    setPublisherQuery((event.target as HTMLInputElement).value)
                  }
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
                className="w-full cursor-pointer rounded-lg border-[1.5px] border-stroke bg-transparent outline-none transition file:mr-5 file:border-collapse file:cursor-pointer file:border-0 file:border-r file:border-solid file:border-stroke file:bg-whiter file:py-3 file:px-5 file:hover:bg-primary file:hover:bg-opacity-10 focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:file:border-form-strokedark dark:file:bg-white/30 dark:file:text-white dark:focus:border-primary"
                type="file"
                accept="image/png, image/jpeg"
                onChange={handleMainImageChange}
                required
              />
            </div>
            <div className="mb-4">
              <label className="mb-2 block text-black dark:text-white">
                Images (multiple)
              </label>
              <input
                className="w-full cursor-pointer rounded-lg border-[1.5px] border-stroke bg-transparent outline-none transition file:mr-5 file:border-collapse file:cursor-pointer file:border-0 file:border-r file:border-solid file:border-stroke file:bg-whiter file:py-3 file:px-5 file:hover:bg-primary file:hover:bg-opacity-10 focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:file:border-form-strokedark dark:file:bg-white/30 dark:file:text-white dark:focus:border-primary"
                type="file"
                accept="image/png, image/gif, image/jpeg"
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
                  value={isVisible.toString()}
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
