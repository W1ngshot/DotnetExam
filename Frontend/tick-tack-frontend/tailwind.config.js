const {nextui} = require("@nextui-org/react");

module.exports = {
    content: ["./src/**/*.{html,js,ts,jsx,tsx}","./node_modules/@nextui-org/theme/dist/**/*.{js,ts,jsx,tsx}"],
    plugins: [nextui()],
    theme: {
      extend: {
      }
    }
  }