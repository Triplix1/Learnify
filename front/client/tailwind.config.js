/** @type {import('tailwindcss').Config} */
module.exports = {
  darkMode: ['selector', '[data-theme="dark"]'],
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      colors: {
        navigation: 'rgb(var(--navigation) / <alpha-value>)',
        logo: 'rgb(var(--logo) / <alpha-value>)',
        dropdown: 'rgb(var(--dropdown) / <alpha-value>)',
        primar: 'rgb(var(--primar) / <alpha-value>)',
        main: 'rgb(var(--main) / <alpha-value>)',
        error: 'rgb(var(--error) / <alpha-value>)',
        link: 'rgb(var(--link) / <alpha-value>)',
        input: 'rgb(var(--input) / <alpha-value>)',
      },
    },
  },
  // variants: {
  //   extend: {
  //     backgroundColor: ['dark'],
  //     textColor: ['dark'],
  //     borderColor: ['dark'],
  //   },
  // },
  plugins: [],
}