import { useState } from "react";
import { toast } from "react-toastify";
import { usePut } from "../../api/service";
import { success } from "../../utils/constants";
import { useNavigate } from "react-router-dom";

interface Publisher {
  name: string;
  description: string;
}

const SubmitForm = async (
    formData: Publisher
) => {
  return await usePut("/publisher", {
    name: formData.name,
    description: formData.description,
  });
};

const NewPublisher = () => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState<Publisher>({
    name: "",
    description: ""
  });

  const onSubmit = async (event: { preventDefault: () => void }) => {
    event.preventDefault();
    const response = await SubmitForm(formData);
    if (response?.data?.status === success) {
       toast.success("Success");
       navigate("/publishers");
    }
  };

  const handleChange = (e: { target: { name: string; value: string; }; }) => {
    const { name, value } = e.target;
    setFormData((prevState) => ({ ...prevState, [name]: value }));
  };

  return (
    <div className="w-2/3 mx-auto rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
      <div className="border-b border-stroke py-4 px-6 dark:border-strokedark">
        <h3 className="font-medium text-black dark:text-white">
          Create new publisher
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
              placeholder="Enter publisher name"
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
              placeholder="Enter publisher description"
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

export default NewPublisher;
