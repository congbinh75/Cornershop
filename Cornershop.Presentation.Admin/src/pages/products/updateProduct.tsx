import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import { useDelete, useGet, usePatch } from "../../api/service";
import { success } from "../../utils/constants";
import { useNavigate, useParams } from "react-router-dom";
import {
  Combobox,
  ComboboxButton,
  ComboboxInput,
  ComboboxOption,
  ComboboxOptions,
} from "@headlessui/react";
import { UploadImage } from "../../utils/firebase";
import Loader from "../../components/loader";
import { getDateAndTimeFromString } from "../../utils/functions";

interface SimpleEntity {
  id: string;
  name: string;
}

interface Image {
  id: string;
  imageUrl: string;
  isMainImage: boolean;
}

interface FormData {
  id: string;
  name: string;
  description: string;
  subcategoryId: string;
  price: number;
  originalPrice: number;
  newMainImageUrl: string;
  newOtherImagesUrls: (string | undefined)[];
  productImagesIds: string[];
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
  return await usePatch("/product", formData);
};

const DeleteThisProduct = async (id: string) => {
  return await useDelete("/product/" + id);
};

const UpdateProduct = () => {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const { id } = useParams();

  const [category, setCategory] = useState<SimpleEntity | null>(null);
  const [subcategory, setSubcategory] = useState<SimpleEntity | null>(null);
  const [newImages, setnewImages] = useState<File[]>([]);
  const [newMainImage, setNewMainImage] = useState<File>();
  const [currentProductImages, setCurrentProductImages] = useState<Image[]>([]);
  const [format, setFormat] = useState(0);
  const [author, setAuthor] = useState<SimpleEntity | null>(null);
  const [publisher, setPublisher] = useState<SimpleEntity | null>(null);
  const [uploaded, setUploaded] = useState<boolean>(false);

  const [formData, setFormData] = useState<FormData>({
    id: "",
    name: "",
    description: "",
    subcategoryId: subcategory?.id ?? "",
    price: 0,
    originalPrice: 0,
    newMainImageUrl: "",
    newOtherImagesUrls: [],
    productImagesIds: [],
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

  const { data: productData, error: productError } = useGet(
    "/product/admin/" + id + "?isHiddenIncluded=true"
  );

  useEffect(() => {
    if (productData?.product)
      setFormData({
        id: productData?.product?.id,
        name: productData?.product?.name,
        description: productData?.product?.description,
        subcategoryId: productData?.product?.subcategory?.id,
        price: productData?.product?.price,
        originalPrice: productData?.product?.originalPrice,
        newMainImageUrl: "",
        newOtherImagesUrls: [],
        productImagesIds: productData?.product?.productImages.map(
          (img: Image) => img.id
        ),
        width: productData?.product?.width,
        height: productData?.product?.height,
        length: productData?.product?.length,
        pages: productData?.product?.pages,
        format: productData?.product?.format,
        stock: productData?.product?.stock,
        publishedYear: productData?.product?.publishedYear,
        authorId: productData?.product?.author?.id,
        publisherId: productData?.product?.publisher?.id,
        isVisible: productData?.product?.isVisible,
      });
    setCurrentProductImages(productData?.product?.productImages);
  }, [productData]);

  if (productError) {
    const message = productError?.response?.data?.Message;
    toast.error(message);
  }

  const handleChange = (e: { target: { name: string; value: string } }) => {
    const { name, value } = e.target;
    setFormData((prevState) => ({ ...prevState, [name]: value }));
  };

  const onUpload = async (event: { preventDefault: () => void }) => {
    try {
      event.preventDefault();

      let uploadedMainImageData = null;
      if (!newMainImage && newImages.length <= 0) {
        setUploaded(true);
        return;
      }

      if (newMainImage) {
        uploadedMainImageData = await UploadImage(newMainImage);
      }

      const uploadPromises = Array.from(newImages).map((image) =>
        UploadImage(image)
      );
      let uploadedFilesData = await Promise.all(uploadPromises);
      uploadedFilesData = uploadedFilesData.filter(
        (url): url is string => url !== undefined
      );

      if (
        ((newMainImage && uploadedMainImageData) || !newMainImage) &&
        ((newImages.length > 0 && uploadedFilesData) || newImages.length <= 0)
      ) {
        if (uploadedMainImageData) {
          setFormData((prevState) => ({
            ...prevState,
            newMainImageUrl: uploadedMainImageData,
          }));
        }

        if (uploadedFilesData) {
          setFormData((prevState) => ({
            ...prevState,
            newOtherImagesUrls: uploadedFilesData,
          }));
        }

        setUploaded(true);
      }
    } catch (error) {
      const errorMessage = error?.response?.data?.message || error?.message;
      toast.error(errorMessage);
    }
  };

  const onSubmit = async () => {
    try {
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

  useEffect(() => {
    if (uploaded) {
      if (newMainImage && newImages.length > 0) {
        if (
          formData.newMainImageUrl &&
          formData.newMainImageUrl !== "" &&
          formData.newOtherImagesUrls.length > 0
        ) {
          onSubmit();
        }
      }
      if (newMainImage && newImages.length <= 0) {
        if (formData.newMainImageUrl && formData.newMainImageUrl !== "") {
          onSubmit();
        }
      }
      if (!newMainImage && newImages.length > 0) {
        if (formData.newOtherImagesUrls.length > 0) {
          onSubmit();
        }
      }
      if (!newMainImage && newImages.length <= 0) {
        onSubmit();
      }
    }
  }, [formData.newMainImageUrl, formData.newOtherImagesUrls, uploaded]);

  const handleDelete = async () => {
    const confirmation = window.confirm(
      "Are you sure you want to delete this product?"
    );
    if (confirmation) {
      try {
        const response = await DeleteThisProduct(id);
        if (response?.data?.status === success) {
          toast.success("Success");
          navigate("/products");
        }
      } catch (error) {
        const errorMessage = error?.response?.data?.Message || error?.message;
        toast.error(errorMessage);
      }
    }
  };

  //Category ----------------------------------------------------------------------------------------------------------
  const { data: categoryData, error: categoryError } = useGet(
    "/category" + "?page=" + page + "&pageSize=" + pageSize
  );

  if (categoryError) {
    const errorMessage =
      categoryError?.response?.data?.message || categoryError?.message;
    toast.error(errorMessage);
  }

  useEffect(() => {
    if (
      productData?.product?.subcategory?.category &&
      categoryData?.categories
    ) {
      const currentCategory = categoryData.categories.find(
        (e: { id: string }) =>
          e.id === productData?.product.subcategory.category.id
      );
      setCategory(currentCategory);
    }
  }, [productData, categoryData]);

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

  //Subcategory -----------------------------------------------------------------------------------------------------
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

  useEffect(() => {
    if (productData?.product?.subcategory && subcategoryData?.subcategories) {
      const currentSubcategory = subcategoryData?.subcategories.find(
        (e: { id: string }) => e.id === productData?.product.subcategory.id
      );
      setSubcategory(currentSubcategory);
    }
  }, [productData, subcategoryData]);

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

  //Author ----------------------------------------------------------------------------------------------------------
  const { data: authorData, error: authorError } = useGet(
    "/author" + "?page=" + page + "&pageSize=" + pageSize
  );

  if (authorError) {
    const errorMessage =
      authorError?.response?.data?.message || authorError?.message;
    toast.error(errorMessage);
  }

  useEffect(() => {
    if (productData?.product?.author && authorData?.authors) {
      const currentAuthor = authorData.authors.find(
        (e: { id: string }) => e.id === productData?.product.author.id
      );
      setAuthor(currentAuthor);
    }
  }, [productData, authorData]);

  useEffect(() => {
    const filteredData = authorData?.authors.filter((author: SimpleEntity) => {
      return author.name.toLowerCase().includes(authorQuery.toLowerCase());
    });
    setFilteredAuthors(filteredData);
  }, [authorData?.authors, authorQuery]);

  //Publisher ---------------------------------------------------------------------------------------------------------
  const { data: publisherData, error: publisherError } = useGet(
    "/publisher" + "?page=" + page + "&pageSize=" + pageSize
  );

  if (publisherError) toast.error(publisherError.message);

  useEffect(() => {
    if (productData?.product?.publisher && publisherData?.publishers) {
      const currentPublisher = publisherData.publishers.find(
        (e: { id: string }) => e.id === productData?.product.publisher.id
      );
      setPublisher(currentPublisher);
    }
  }, [productData, publisherData]);

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

  const replaceCurrentMainImage = (file: File) => {
    const mainImg = currentProductImages?.find(
      (img) => img?.isMainImage === true
    );
    setFormData((prevState) => ({
      ...prevState,
      productImagesIds: formData.productImagesIds.filter(
        (id) => id !== mainImg?.id
      ),
    }));
    setCurrentProductImages(
      currentProductImages?.filter((img) => img?.isMainImage !== true)
    );
    setNewMainImage(file);
  };

  const deleteCurrentOtherImage = (e: any, id: string) => {
    e.preventDefault();
    const confirmation = window.confirm(
      "Are you sure you want to delete this image? Changes are pending before submitting"
    );
    if (confirmation) {
      const deleteImg = currentProductImages?.find((img) => img.id === id);
      setFormData((prevState) => ({
        ...prevState,
        productImagesIds: formData.productImagesIds.filter(
          (id) => id !== deleteImg.id
        ),
      }));
      setCurrentProductImages(
        currentProductImages?.filter((img) => img.id !== id)
      );
    }
  };

  return loading ? (
    <Loader />
  ) : (
    <div className="w-full lg:w-2/3 mx-auto rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
      <div className="flex flex-row items-center border-b border-stroke py-4 px-6 dark:border-strokedark">
        <h3 className="grow font-medium text-black dark:text-white">
          Update product
        </h3>
        <button
          onClick={() => handleDelete()}
          className="flex items-center justify-center rounded bg-danger p-3 font-medium text-gray hover:bg-opacity-90 cursor-pointer"
        >
          <i className="fa-solid fa-trash"></i>
          <span className="ml-2">Delete this product</span>
        </button>
      </div>
      <form onSubmit={onUpload}>
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
                  onInput={(event) => setCategoryQuery((event.target as HTMLInputElement).value)}
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
                    setSubcategoryQuery((event.target as HTMLInputElement).value)
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
            <div>
              <label className="mb-2 block text-black dark:text-white">
                Current main image
              </label>
              <img
                className="w-32 h-32 lg:w-56 lg:h-56 mx-auto object-contain rounded border border-stroke dark:border-form-strokedark"
                src={
                  currentProductImages?.find(
                    (productImage: Image) => productImage.isMainImage === true
                  )?.imageUrl
                }
              />
            </div>

            <div>
              <label className="mb-2 block text-black dark:text-white">
                Current other images
              </label>
              <div className="w-full overflow-x-auto">
                <div className="w-full flex flex-row gap-2 min-w-max">
                  {currentProductImages?.filter(
                    (productImage: Image) => productImage.isMainImage === false
                  ).length <= 0 ? (
                    <div className="flex items-center justify-center w-full h-32 lg:h-56">
                      <p className="text-center">No data</p>
                    </div>
                  ) : (
                    currentProductImages
                      ?.filter(
                        (productImage: Image) =>
                          productImage.isMainImage === false
                      )
                      .map((productImage: Image) => (
                        <div key={productImage.id}>
                          <img
                            className="w-32 h-32 lg:w-56 lg:h-56 mb-2 object-contain rounded border border-stroke dark:border-form-strokedark"
                            src={productImage.imageUrl}
                          ></img>
                          <div className="w-fit mx-auto">
                            <button
                              className="px-2 py-1"
                              onClick={(e) =>
                                deleteCurrentOtherImage(e, productImage.id)
                              }
                            >
                              <i className="fa-solid fa-trash mt-2"></i>
                            </button>
                          </div>
                        </div>
                      ))
                  )}
                </div>
              </div>
            </div>
          </div>

          <div className="mb-6 grid grid-cols-1 lg:grid-cols-2 lg:gap-4">
            <div className="mb-4">
              <label className="mb-2 block text-black dark:text-white">
                New main image (replace current)
              </label>
              <input
                className="w-full cursor-pointer rounded-lg border border-stroke bg-transparent outline-none transition file:mr-5 file:border-collapse file:cursor-pointer file:border-0 file:border-r file:border-solid file:border-stroke file:bg-whiter file:py-3 file:px-5 file:hover:bg-primary file:hover:bg-opacity-10 focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:file:border-form-strokedark dark:file:bg-white/30 dark:file:text-white dark:focus:border-primary"
                type="file"
                accept="image/png, image/jpeg"
                onChange={(e) => replaceCurrentMainImage(e.target.files[0])}
              />
            </div>

            <div className="mb-4">
              <label className="mb-2 block text-black dark:text-white">
                New images (multiple, append to current)
              </label>
              <input
                className="w-full cursor-pointer rounded-lg border border-stroke bg-transparent outline-none transition file:mr-5 file:border-collapse file:cursor-pointer file:border-0 file:border-r file:border-solid file:border-stroke file:bg-whiter file:py-3 file:px-5 file:hover:bg-primary file:hover:bg-opacity-10 focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:file:border-form-strokedark dark:file:bg-white/30 dark:file:text-white dark:focus:border-primary"
                type="file"
                accept="image/png, image/gif, image/jpeg"
                multiple
                onChange={(e) => setnewImages(e.target.files)}
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
                  name="isVisible"
                  value={formData.isVisible?.toString()}
                  onChange={(e) => {
                    setFormData((prevState) => ({
                      ...prevState,
                      isVisible: (e.target.value === 'true'),
                    }));
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

          <div className="mb-6 grid grid-cols-1 lg:grid-cols-2 lg:gap-4">
            <div className="mb-4">
              <label className="mb-2 block text-black dark:text-white">
                Created on
              </label>
              <input
                type="text"
                className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
                value={getDateAndTimeFromString(productData?.product?.createdOn)}
                disabled
              ></input>
            </div>

            <div className="mb-4">
              <label className="mb-2 block text-black dark:text-white">
                Updated on
              </label>
              <input
                type="text"
                className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
                value={getDateAndTimeFromString(productData?.product?.updatedOn)}
                disabled
              ></input>
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

export default UpdateProduct;
