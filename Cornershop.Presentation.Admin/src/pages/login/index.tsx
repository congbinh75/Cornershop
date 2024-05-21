import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { success } from "../../utils/constants";
import { usePost } from "../../api/service";

const Login = () => {
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const handlePost = async (event) => {
    event.preventDefault();
    const response = await usePost("/user/login", {
      email: email,
      password: password,
    });
    if (response?.data?.status === success) {
      navigate("/");
    } else {
      toast.error(response?.data?.message);
    }
  };

  return (
    <>
      <div className="bg-white dark:bg-slate-800">
        <div className="w-full mx-auto p-12 sm:w-1/2 xl:w-1/3">
          <h2 className="mb-9 mx-auto w-fit text-2xl font-bold text-black dark:text-white sm:text-title-xl2">
            Login
          </h2>

          <form onSubmit={handlePost}>
            <div className="mb-4">
              <label className="mb-2.5 block font-medium text-black dark:text-white">
                Email or Username
              </label>
              <div className="relative">
                <input
                  type="text"
                  placeholder="sample@email.com"
                  className="w-full rounded-lg border border-stroke bg-transparent py-4 px-6 text-black outline-none focus:border-primary focus-visible:shadow-none dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
                  onChange={e => setEmail(e.target.value)}
                  value={email}
                />
              </div>
            </div>

            <div className="mb-6">
              <label className="mb-2.5 block font-medium text-black dark:text-white">
                Password
              </label>
              <div className="relative">
                <input
                  type="password"
                  placeholder="********"
                  className="w-full rounded-lg border border-stroke bg-transparent py-4 px-6 text-black outline-none focus:border-primary focus-visible:shadow-none dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
                  onChange={e => setPassword(e.target.value)}
                  value={password}
                />
              </div>
            </div>

            <div className="mb-5">
              <input
                type="submit"
                value="Sign In"
                className="w-full cursor-pointer rounded-lg border border-primary bg-primary p-4 text-white transition hover:bg-opacity-90"
              />
            </div>
            <button className="flex w-full items-center justify-center gap-3.5 rounded-lg border border-stroke bg-gray p-4 hover:bg-opacity-50 dark:border-strokedark dark:bg-meta-4 dark:hover:bg-opacity-50">
              Sign in with Google
            </button>
          </form>
        </div>
        <ToastContainer />
      </div>
    </>
  );
};

export default Login;
