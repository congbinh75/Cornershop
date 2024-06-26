import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { success } from "../../utils/constants";
import { usePost } from "../../api/service";

const SubmitForm = async (email: string, password: string) => {
  return await usePost("/user/login", {
    email: email,
    password: password,
  });
};

const Login = () => {
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const handlePost = async (event: { preventDefault: () => void }) => {
    event.preventDefault();
    const data = await SubmitForm(email, password);
    if (data?.data?.status === success) {
      navigate("/");
    }
  };

  return (
    <>
      <div className="bg-white dark:bg-slate-800 flex items-center h-screen">
        <div className="mx-auto p-8 w-[32rem] rounded-sm sm:border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
          <h2 className="mb-9 mx-auto w-fit text-2xl font-bold text-black dark:text-white sm:text-title-xl2">
            Log in
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
                  onChange={(e) => setEmail(e.target.value)}
                  value={email}
                  required
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
                  onChange={(e) => setPassword(e.target.value)}
                  value={password}
                  required
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
            {/* <button className="flex w-full items-center justify-center gap-3.5 rounded-lg border border-stroke bg-gray p-4 hover:bg-opacity-50 dark:border-strokedark dark:bg-meta-4 dark:hover:bg-opacity-50">
              Sign in with Google
            </button> */}
          </form>
        </div>
        <ToastContainer />
      </div>
    </>
  );
};

export default Login;
