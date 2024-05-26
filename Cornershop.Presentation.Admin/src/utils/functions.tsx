export const getDateFromString = (str: string) => {
  const date = new Date(str);
  const localDateStr = date.toLocaleDateString();
  const localDate = new Date(localDateStr);

  const day = localDate.getDate();
  const month = localDate.getMonth() + 1;
  const year = localDate.getFullYear();
  return day + "/" + month + "/" + year;
};