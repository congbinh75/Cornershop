import { KeyedMutator } from "swr";

interface Props {
  pagesCount: number;
  page: number;
  setPage: React.Dispatch<React.SetStateAction<any>>;
  pageSize: number;
  setPageSize: React.Dispatch<React.SetStateAction<any>>;
  mutate: KeyedMutator<any>;
}

const TablePageControl = (props: Props) => {
  return (
    <>
      <div className="flex flex-row grow gap-4">
        <select
          value={props.pageSize}
          onChange={(e) => {
            if (Number(e.target.value) !== props.pageSize) {
              props.setPageSize(Number(e.target.value));
            }
          }}
          className="inline-flex items-center justify-center rounded-md bg-transparent border border-stroke p-4 text-center font-medium text-black dark:border-form-strokedark dark:text-white"
        >
          <option value="15">15</option>
          <option value="30">30</option>
          <option value="45">45</option>
        </select>
        <button
          className="inline-flex items-center justify-center rounded-md border border-stroke p-4 text-center font-medium text-black dark:border-form-strokedark dark:text-white"
          onClick={() => {
            if (page > 1) {
              props.setPage(props.page - 1);
              props.mutate();
            }
          }}
        >
          <i className="fa-solid fa-arrow-left"></i>
        </button>
        <span className="inline-flex items-center justify-center">
          {props.page + "/" + props.pagesCount}
        </span>
        <button
          className="inline-flex items-center justify-center rounded-md border border-stroke p-4 text-center font-medium text-black dark:border-form-strokedark dark:text-white"
          onClick={() => {
            if (props.page < props.pagesCount) {
              props.setPage(props.page + 1);
              props.mutate();
            }
          }}
        >
          <i className="fa-solid fa-arrow-right"></i>
        </button>
      </div>
    </>
  );
};

export default TablePageControl;
