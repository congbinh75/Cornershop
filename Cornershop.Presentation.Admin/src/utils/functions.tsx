export const getDateFromString = (str: string) => {
  const date = new Date(str);
  const localDateStr = date.toLocaleDateString();
  const localDate = new Date(localDateStr);

  const day = localDate.getDate();
  const month = localDate.getMonth() + 1;
  const year = localDate.getFullYear();
  return day + "/" + month + "/" + year;
};

export const getDateAndTimeFromString = (str: string) => {
  const date = new Date(str)

  function pad(number: number) {
      return number < 10 ? '0' + number : number;
  }

  const day = pad(date.getDate());
  const month = pad(date.getMonth() + 1);
  const year = date.getFullYear();
  const hours = pad(date.getHours());
  const minutes = pad(date.getMinutes());
  const seconds = pad(date.getSeconds());

  return `${day}-${month}-${year} ${hours}:${minutes}:${seconds}`;
};