/** @type {import('tailwindcss').Config} */
module.exports = {
  darkMode: ['selector', '[data-theme="dark"]'],
  content: [
    "./src/**/*.{html,ts}",
    "./node_modules/flowbite/**/*.js"
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
        navlink: 'rgb(var(--navlink) / <alpha-value>)',
        buttonAccept: 'rgb(var(--buttonAccept) / <alpha-value>)',
        buttonCancel: 'rgb(var(--buttonCancel) / <alpha-value>)',
        buttonAcceptHover: 'rgb(var(--buttonAcceptHover) / <alpha-value>)',
        buttonCancelHover: 'rgb(var(--buttonCancelHover) / <alpha-value>)',
        textInput: 'rgb(var(--textInput) / <alpha-value>)',
        text: 'rgb(var(--text) / <alpha-value>)',
        messageUsername: 'rgb(var(--messageUsername) / <alpha-value>)',
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
  plugins: [
    require('flowbite/plugin')
  ],
}