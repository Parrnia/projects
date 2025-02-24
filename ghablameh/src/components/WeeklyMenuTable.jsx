import React from "react";
import styles from "./WeeklyMenuTable.module.css";
const WeeklyMenuTable = () => {
  return (
    <div className={styles.ltr}>
    <table class="border-collapse border w-full  border-blue-500 m-10 mx-auto">
  <thead>
    <tr class="bg-blue-500 text-white ">
      <th class="py-2 px-4 text-center	">روز</th>
      <th class="py-2 px-4 text-center	">صبحانه</th>
      <th class="py-2 px-4 text-center	">نهار</th>
      <th class="py-2 px-4 text-center	">شام</th>
    </tr>
  </thead>
  <tbody>
    <tr class="bg-white border-b border-blue-500">
      <td class="py-2 px-4 text-center h-14">شنبه</td>
      <td class="py-2 px-4 text-center	"></td>
      <td class="py-2 px-4 text-center	"></td>
    </tr>
    <tr class="bg-white border-b border-blue-500">
    <td class="py-2 px-4 text-center	h-14">یکشنبه</td>
      <td class="py-2 px-4 text-center	"></td>
      <td class="py-2 px-4 text-center	"></td>
    </tr>
    <tr class="bg-white border-b border-blue-500">
      <td class="py-2 px-4 text-center	h-14">دوشنبه</td>
      <td class="py-2 px-4 text-center	"></td>
      <td class="py-2 px-4 text-center	"></td>
    </tr>
    <tr class="bg-white border-b border-blue-500">
      <td class="py-2 px-4 text-center	h-14">سه شنبه</td>
      <td class="py-2 px-4 text-center	"></td>
      <td class="py-2 px-4 text-center	"></td>
    </tr>
    <tr class="bg-white border-b border-blue-500">
      <td class="py-2 px-4 text-center	h-14"></td>
      <td class="py-2 px-4 text-center	"></td>
      <td class="py-2 px-4 text-center	"></td>
    </tr>
    <tr class="bg-white border-b border-blue-500">
      <td class="py-2 px-4 text-center	h-14">پنج شنبه</td>
      <td class="py-2 px-4 text-center	"></td>
      <td class="py-2 px-4 text-center	"></td>
    </tr>
    <tr class="bg-white border-b border-blue-500">
      <td class="py-2 px-4 text-center	h-14">جمعه</td>
      <td class="py-2 px-4 text-center	"></td>
      <td class="py-2 px-4 text-center	"></td>
    </tr>
  </tbody>
</table>
</div>
  );
};
export default WeeklyMenuTable;
