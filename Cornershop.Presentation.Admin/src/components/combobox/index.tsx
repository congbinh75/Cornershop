import { useState } from "react";
import {
  Combobox,
  ComboboxButton,
  ComboboxInput,
  ComboboxOption,
  ComboboxOptions,
} from "@headlessui/react";

const ComboBox = ({
  value = "",
  onChange,
  onBlur,
  options,
}: {
  value: string | number;
  onChange: any;
  onBlur: any;
  options: {
    value: string | number;
    label: string;
    id: number | string;
  }[];
}) => {
  const [query, setQuery] = useState("");

  const filteredOptions =
    query === ""
      ? options
      : options.filter((option) => {
          return option.label.toLowerCase().includes(query.toLowerCase());
        });

  return (
    <>
      <Combobox value={value} onChange={onChange} onClose={() => setQuery("")}>
        <div className="relative">
          <ComboboxInput
            className="w-full rounded border border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
            displayValue={(option: { name: string }) => option?.name}
            onInput={(event) => setQuery(event)}
            onBlur={onBlur}
          />
          <ComboboxButton className="absolute inset-y-0 right-0 px-4">
            <i className="fa-solid fa-chevron-down"></i>
          </ComboboxButton>
        </div>
        <ComboboxOptions
          anchor="bottom"
          className="empty:hidden overflow-hidden"
        >
          {filteredOptions?.map((option: any) => (
            <ComboboxOption
              key={option?.id}
              value={option}
              className="w-[var(--input-width)] [--anchor-gap:var(--spacing-1)] cursor-pointer bg-slate-100 data-[focus]:bg-slate-200 dark:bg-slate-800 dark:data-[focus]:bg-slate-700"
            >
              <div className="text-sm p-4 text-black dark:text-white">
                {option.name}
              </div>
            </ComboboxOption>
          ))}
        </ComboboxOptions>
      </Combobox>
    </>
  );
}

export default ComboBox;
