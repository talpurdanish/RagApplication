import { isDevMode } from '@angular/core';
import {
  format,
  parseISO,
  differenceInYears,
  differenceInMonths,
  differenceInDays,
} from 'date-fns';

const BASE_URI_U = 'https://localhost:6300';
const BASE_URI_OP = 'https://localhost:6400';

export const Constants = {
  BASE_URI_U,
  BASE_URI_OP,
  API_URL_U: `${BASE_URI_U}/api/fmdc`,
  API_URL_OP: `${BASE_URI_OP}/api/fmdc`,
  API_URL_RAG: isDevMode() ? `https://localhost:4100/api` : `http://localhost:6100/api`,

  USER_STORAGE_KEY: 'b500b594-a6e0-4ffd-ac00-c39b5744f08c',
  THEME_STORAGE_KEY: 'theme',
  MENU_COLLAPSE_SETTINGS_KEY: 'menu',
  RECEIPT_STORAGE_KEY: 'receipt',
  PRESCRIPTION_STORAGE_KEY: 'prescription',
  MESSAGES_STORAGE_KEY: 'messages',
  CHAT_STORAGE_KEY: 'chats',
  WEIGHTS_STORAGE_KEY: 'weights',

  RECEIPT_TRIGGER: 'reciept',
  NOTIFICATION_TRIGGER: 'notification',
  APPOINTMENT_TRIGGER: 'appointment',
  INCOME_TRIGGER: 'income',
};

export default function debounce<T extends (...args: any[]) => void>(fn: T, delay: number) {
  let timer: ReturnType<typeof setTimeout>;
  return (...args: Parameters<T>) => {
    clearTimeout(timer);
    timer = setTimeout(() => fn(...args), delay);
  };
}

// ✅ Format optional date
export function formatOptionalDate(formatStr: string, date?: Date): string {
  if (!date) return '';
  return format(date, formatStr);
}

// ✅ Format date
export function formatDate(date: Date, formatStr: string): string {
  return format(date, formatStr);
}

// ✅ Parse ISO date string safely
export function parseIsoDateString(raw: Date | string | null | undefined): Date {
  if (!raw) return new Date();
  try {
    if (raw instanceof Date) return raw;
    const normalized = (raw as string).replace(/\.(\d{3})\d*Z$/, '.$1Z');
    const parsed = parseISO(normalized);
    return isNaN(parsed.getTime()) ? new Date() : parsed;
  } catch {
    return new Date();
  }
}

// ✅ Calculate age (years, months, days)
export function calculateAge(dob: Date): { years: number; months: number; days: number } {
  const today = new Date();
  const years = differenceInYears(today, dob);
  const months = differenceInMonths(today, dob) % 12;
  const days = differenceInDays(
    today,
    new Date(today.getFullYear(), today.getMonth(), dob.getDate()),
  );

  return { years, months, days };
}

// ✅ Age string
export function calculateAgeString(dob: Date): string {
  const age = calculateAge(dob);
  return `${age.years} years, ${age.months} months, ${age.days} days`;
}

export function formatRole(role: number): string {
  let roleString: string = '';
  switch (role) {
    case 1:
      roleString = 'Admin';
      break;
    case 2:
      roleString = 'Doctor';
      break;
    case 3:
      roleString = 'Staff';
      break;
  }
  return roleString;
}

export function formatGender(gender: number): string {
  let returnString: string = '';
  switch (gender) {
    case 1:
      returnString = 'Male';
      break;
    case 2:
      returnString = 'Female';
      break;
    case 3:
      returnString = 'Other';
      break;
  }
  return returnString;
}

export class Role {
  id: number = -1;
  name: string = '';

  constructor(id: number, name: string) {
    this.id = id;
    this.name = name;
  }
}

export const Roles = {
  admin: new Role(1, 'admin'),
  doctor: new Role(2, 'doctor'),
  staff: new Role(3, 'patient'),
};

export const NotificationType = {
  paymentSucceeded: 1,
  paymentFailed: 2,
  appointmentCreated: 3,
  appointmentEnded: 4,
  generalAlert: 5,
};

export const NotificationSeverity = {
  Success: 1,
  Info: 2,
  Warning: 3,
  Error: 4,
};
