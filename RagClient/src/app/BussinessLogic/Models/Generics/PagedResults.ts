export interface PagedResults<T> {
    data: T[];
    currentPage: number;
    totalRecords: number;
    totalPages: number;
    pageSize: number;
    nextPage?: number;
    prevPage?: number;
}


export function createPagedResults<T>(raw: any, modelFactory?: (raw: any) => T) {

    return {
        currentPage: raw.currentPage ?? 1,
        totalRecords: raw.totalRecords ?? 1,
        totalPages: raw.totalPages ?? 1,
        pageSize: raw.pageSize ?? 5,
        nextPage: raw.nextPage,
        prevPage: raw.prevPage,
        data: mapData<T>(raw.data, modelFactory)

    };
}

export function createDefaultPagedResults() {

    return {
        currentPage: 1,
        totalRecords: 1,
        totalPages: 1,
        pageSize: 5,
        nextPage: undefined,
        prevPage: undefined,
        data: []

    };
}

function mapData<T>(raw: any, modelFactory?: (raw: any) => T) {
    return modelFactory && Array.isArray(raw) ?
        raw.map((item: any) => modelFactory(item)) : [];
}