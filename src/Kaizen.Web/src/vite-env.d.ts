/// <reference types="vite/client" />

interface ViteTypeOptions {
  strictImportMetaEnv: unknown; // Disallow unknown keys
}

interface ImportMetaEnv {
  readonly VITE_API_URL: string;
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}
