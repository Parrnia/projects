/**
 * @typedef {import('@types/tailwindcss/tailwind-config').TailwindConfig & import('tailwindcss').Config} TailwindMergedConfig
 */

/** @type {TailwindMergedConfig} */
module.exports = {
  content: [
    "./node_modules/flowbite-react/lib/**/*.js",
    "./pages/**/*.{ts,tsx}",
    "./public/**/*.html",
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  plugins: [
    require("flowbite/plugin"),
    require('tailwindcss'),
    require('autoprefixer')
  ],
  theme: {
    extend: {
      colors: {
        template: {
          "custom-blue": '#26577C',
          "custom-white": "#EBE4D1",
          "custom-gray": '#B4B4B3',
          "custom-orange": '#E55604',
        }
      }
    }
  }
};
