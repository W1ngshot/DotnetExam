import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.tsx'

import 'dayjs/locale/ru'
import './index.css'
import 'react-toastify/dist/ReactToastify.css';
import relativeTime from 'dayjs/plugin/relativeTime'
import dayjs from 'dayjs'
dayjs.locale('ru')

dayjs.extend(relativeTime)

ReactDOM.createRoot(document.getElementById('root')!).render(
  // <React.StrictMode>
      <App />
  // </React.StrictMode>
)
