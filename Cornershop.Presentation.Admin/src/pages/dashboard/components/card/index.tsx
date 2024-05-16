interface Props {
  title: string;
  total: string;
}

const Card = (props : Props) => {
  return (
    <div className="rounded py-6 px-6 text-black dark:text-white bg-slate-100 dark:bg-slate-800">
      <div className="flex h-12 w-12 items-center justify-center rounded-full bg-slate-200 dark:bg-slate-900">
        <i className="fa-solid fa-cart-shopping"></i>
      </div>
      <div className="mt-4 flex items-end justify-between">
        <div>
          <h4 className="text-title-md font-bold text-black dark:text-white">
            {props.total}
          </h4>
          <span className="text-sm font-medium">{props.title}</span>
        </div>
      </div>
    </div>
  );
};

export default Card;
