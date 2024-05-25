import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import { useGet, usePatch } from "../../api/service";
import { success } from "../../utils/constants";
import { useNavigate, useParams } from "react-router-dom";

interface FormData {
  id: string;
  name: string;
  description: string;
}

const SubmitForm = async (formData: FormData) => {
  return await usePatch("/category", {
    id: formData.id,
    name: formData.name,
    description: formData.description,
  });
};

const UpdateCategory = () => {
  const navigate = useNavigate();
  const { id } = useParams();

  const [formData, setFormData] = useState<FormData>({
    id: "",
    name: "",
    description: "",
  });

  const handleChange = (e: { target: { name: string; value: string } }) => {
    const { name, value } = e.target;
    setFormData((prevState) => ({ ...prevState, [name]: value }));
  };

  const onSubmit = async (event: { preventDefault: () => void }) => {
    try {
      event.preventDefault();
      const response = await SubmitForm(formData);
      if (response?.data?.status === success) {
        toast.success("Success");
        navigate("/categories");
      }
    } catch (error) {
      const errorMessage = error?.response?.data?.Message || error?.message;
      toast.error(errorMessage);
    }
  };

  const { data, error } = useGet("/category/" + id);

  useEffect(() => {
    if (data?.category)
      setFormData({
        id: data?.category?.id,
        name: data?.category?.name,
        description: data?.category?.description,
      });
  }, [data]);

  if (error) {
    const message = error?.response?.data?.Message;
    toast.error(message);
  }

  return (
    <div className="w-2/3 mx-auto rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
      <div className="border-b border-stroke py-4 px-6 dark:border-strokedark">
        <h3 className="font-medium text-black dark:text-white">
          Update category
        </h3>
      </div>
      <form onSubmit={onSubmit}>
        <div className="p-6">
          <div>
            <input
              type="text"
              className="hidden"
              value={formData.id}
              required
              disabled
            />
          </div>

          <div className="mb-4">
            <label className="mb-2 block text-black dark:text-white">
              Name <span className="text-meta-1">*</span>
            </label>
            <input
              type="text"
              placeholder="Enter category name"
              className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
              value={formData.name}
              name="name"
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
              placeholder="Enter category description"
              className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
              value={formData.description}
              name="description"
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

export default UpdateCategory;
