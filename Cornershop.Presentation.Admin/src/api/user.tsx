import { getSingle, post } from "./utils/provider";

const url = 'api/user';

export const getUser = async (id: string) => {
    return await getSingle(url, id);
}

export const loginUser = async (email: string, password: string) => {
    return await post(url + "/login", { email: email, password: password});
}