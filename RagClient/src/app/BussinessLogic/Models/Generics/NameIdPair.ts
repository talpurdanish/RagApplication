export interface NameIdPair {

    id: number;
    name: string;
}

export function createNameIdPair(raw: any): NameIdPair {

    return {
        id: raw.id,
        name: raw.name
    };

}