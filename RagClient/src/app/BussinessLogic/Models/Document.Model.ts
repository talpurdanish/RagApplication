export interface DocumentModel {
  text: string;
  author: string;
  numberOfPages: number;
  creationDate: Date;
  title: string;
  similarity: number;
}

export function createDocumentModel(raw: any): DocumentModel {
  return {
    text: raw.text ?? '',
    author: raw.author ?? '',
    numberOfPages: Number(raw.numberOfPages) || 0,
    creationDate: raw.creationDate ? new Date(raw.creationDate) : new Date(),
    title: raw.title ?? '',
    similarity: Number(raw.similarity) || 0,
  };
}
