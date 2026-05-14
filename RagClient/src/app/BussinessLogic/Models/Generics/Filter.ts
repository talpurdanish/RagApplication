export interface Filter {
  sortfield: string;
  order: number;
  page: number;
  pageSize: number;
}

export function convertFilter(filter: Filter): Record<string, string> {
  if (!filter) return {};
  return {
    sortfield: String(filter.sortfield),
    order: String(filter.order),
    page: String(filter.page),
    pageSize: String(filter.pageSize),
  };
}

// function generateQueryString(filter: any): string {

//   return `sortfield=${filter.sortfield}&order=${filter.order}&page=${filter.page}&pagesize=${filter.pageSize}`
// }

export default function createDefaultFilter(): Filter {
  return {
    sortfield: '1',
    order: 1,
    page: 1,
    pageSize: 10,
  };
}
