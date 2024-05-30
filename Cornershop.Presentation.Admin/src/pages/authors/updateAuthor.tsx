import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import { useDelete, useGet, usePatch } from "../../api/service";
import { success } from "../../utils/constants";
import { useNavigate, useParams } from "react-router-dom";
import { useForm } from "react-hook-form";
import Loader from "../../components/loader";

interface Author {
  id: string;
  name: string;
  description: string;
}

const SubmitPatch = async (formData: Author) => {
  return await usePatch("/author", formData);
};

const SubmitDelete = async (id: string) => {
  return await useDelete("/author/" + id);
};

const UpdateAuthor = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  const [loading, setLoading] = useState(true);

  const { data, error } = useGet("/author/" + id);

  if (error) {
    setLoading(false);
    const message = error?.response?.data?.Message;
    toast.error(message);
  }

  const { register, handleSubmit, reset } = useForm<Author>();

  useEffect(() => {
    setLoading(false);
    if (data) reset(data?.author);
  }, [reset, data]);

  const submit = async (data: Author) => {
    try {
      const response = await SubmitPatch(data);
      if (response?.data?.status === success) {
        toast.success("Success");
        navigate("/authors");
      }
    } catch (error) {
      if (error?.response?.status == 401) {
        navigate("/login");
      }
      const errorMessage = error?.response?.data?.Message || error?.message;
      toast.error(errorMessage);
    }
  };

  const handleDelete = async () => {
    const confirmation = window.confirm(
      "Are you sure you want to delete this author?"
    );
    if (confirmation) {
      try {
        const response = await SubmitDelete(id);
        if (response?.data?.status === success) {
          toast.success("Success");
          navigate("/authors");
        }
      } catch (error) {
        const errorMessage = error?.response?.data?.Message || error?.message;
        toast.error(errorMessage);
      }
    }
  };

  return loading ? (
    <Loader />
  ) : (
    <div className="w-2/3 mx-auto rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
      <div className="flex flex-row items-center border-b border-stroke py-4 px-6 dark:border-strokedark">
        <h3 className="grow font-medium text-black dark:text-white">
          Update category
        </h3>
        <button
          onClick={() => handleDelete()}
          className="flex items-center justify-center rounded bg-danger p-3 font-medium text-gray hover:bg-opacity-90 cursor-pointer"
        >
          <i className="fa-solid fa-trash"></i>
          <span className="ml-2">Delete this author</span>
        </button>
      </div>
      <form onSubmit={handleSubmit(submit)}>
        <div className="p-6">
          <div className="mb-4">
            <div>
              <input
                type="text"
                className="hidden"
                {...register("id", { required: true })}
              />
            </div>

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

export default UpdateAuthor;
