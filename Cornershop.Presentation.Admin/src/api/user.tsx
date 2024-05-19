import { getSingle, post } from "./utils/provider";

const url = 'user';

export const getUser = async (id: string) => {
    return await getSingle(url, id);
}

export const loginUser = async (username: string, password: string) => {
    return await post(url + "/login", { username: username, password: password});
}