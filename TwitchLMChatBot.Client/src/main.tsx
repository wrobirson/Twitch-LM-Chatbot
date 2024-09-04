import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import App from './App.tsx'
import './index.css'
import {ConfigProvider, App as AppContext} from "antd";
import es_ES from "antd/locale/es_ES";
import en_US from "antd/locale/en_US";
import LanguageDetector from 'i18next-browser-languagedetector';

import './i18n.ts'

console.log((new LanguageDetector()).detect(), 'languageDetector');
const lang = (new LanguageDetector()).detect()
createRoot(document.getElementById('root')!).render(
  <StrictMode>
      <ConfigProvider locale={lang == "es" ? es_ES: en_US} theme={{
      }}>
          <AppContext>
              <App />
          </AppContext>
      </ConfigProvider>
  </StrictMode>,
)
