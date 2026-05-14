// import type { RefObject } from "react";
// import { Toast } from "primereact/toast";

// let toastRef: RefObject<Toast | null> | null = null;

// export const setToastRef = (ref: RefObject<Toast | null>) => {
//     toastRef = ref;
// };


// export const showSuccess = (detail: string, summary: string = "FMDC", life = 3000) => {
//     toastRef?.current?.show({ severity: "success", summary, detail, life });
// };

// export const showError = (detail: string, summary: string = "FMDC", life = 3000) => {

//     toastRef?.current?.show({ severity: "error", summary, detail, life });
// };

// export const showInfo = (detail: string, summary: string = "FMDC", life = 3000) => {
//     toastRef?.current?.show({ severity: "info", summary, detail, life });
// };

// export const showWarning = (detail: string, summary: string = "FMDC", life = 3000) => {
//     toastRef?.current?.show({ severity: "warn", summary, detail, life });
// };