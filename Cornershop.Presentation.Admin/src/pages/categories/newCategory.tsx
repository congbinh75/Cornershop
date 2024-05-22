import { useState } from "react";
import { toast } from "react-toastify";
import { usePut } from "../../api/service";
import { success } from "../../utils/constants";
import { useNavigate } from "react-router-dom";

const SubmitForm = async (name: string, description: string) => {
  return await usePut("/category", {
    name: name,
    description: description,
  });
};

const NewCategory = () => {
  const navigate = useNavigate();
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");

  const onSubmit = async (event: { preventDefault: () => void }) => {
    try {
      event.preventDefault();
      const response = await SubmitForm(name, description);
      if (response?.data?.status === success) {
        toast.success("Success");
        navigate("/categories");
      }
    } catch (error) {
      const errorMessage = error?.response?.data?.message || error?.message;
      toast.error(errorMessage);
    }

    setName("");
    setDescription("");
  };

  return (
    <div className="w-2/3 mx-auto rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
      <div className="border-b border-stroke py-4 px-6 dark:border-strokedark">
        <h3 className="font-medium text-black dark:text-white">
          Create new category
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
              placeholder="Enter category name"
              className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
              value={name}
              onChange={(e) => setName(e.target.value)}
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

export default NewCategory;
