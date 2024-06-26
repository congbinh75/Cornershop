import { toast } from "react-toastify";
import { usePut } from "../../api/service";
import { success } from "../../utils/constants";
import { useNavigate } from "react-router-dom";
import { SubmitHandler, useForm } from "react-hook-form";

interface Author {
  name: string;
  description: string;
}

const SubmitPut = async (formData: Author) => {
  return await usePut("/author", {
    name: formData.name,
    description: formData.description,
  });
};

const NewAuthor = () => {
  const navigate = useNavigate();

  const { register, handleSubmit } = useForm<Author>();
  const onSubmit: SubmitHandler<Author> = (data) => submit(data);

  const submit = async (data: Author) => {
    try {
      const response = await SubmitPut(data);
      if (response?.data?.status === success) {
        toast.success("Success");
        navigate("/authors");
      }
    } catch (error) {
      if (error?.response?.status == 401) {
        navigate("/login");
      }
      const errorMessage = error?.response?.data?.message || error?.message;
      toast.error(errorMessage);
    }
  };

  return (
    <div className="w-2/3 mx-auto rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
      <div className="border-b border-stroke py-4 px-6 dark:border-strokedark">
        <h3 className="font-medium text-black dark:text-white">
          Create new author
        </h3>
      </div>
      <form onSubmit={handleSubmit(onSubmit)}>
        <div className="p-6">
          <div className="mb-4">
            <label className="mb-2 block text-black dark:text-white">
              Name <span className="text-meta-1">*</span>
            </label>
            <input
              type="text"
              placeholder="Enter author name"
              className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
              {...register("name", { required: true, maxLength: 100 })}
            />
          </div>
          <div className="mb-6">
            <label className="mb-2 block text-black dark:text-white">
              Description
            </label>
            <textarea
              rows={6}
              placeholder="Enter author description"
              className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
              {...register("description", { required: true, maxLength: 1000 })}
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

export default NewAuthor;
