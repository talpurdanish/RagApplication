export interface ImageModel {
  id: number;
  image: string;
  width: number;
  height: number;
  format: string;
  thumbnailImage: string;
  thumbWidth: number;
  thumbHeight: number;
  thumbFormat: string;
  fileSizeBytes: number;
  uploadedAt: string; // DateTime is typically handled as an ISO string in TS
  aiInsights: string | null;
  similarity: number;
  inProgress: boolean;
}

export function createImageModel(raw: any): ImageModel {
  return {
    id: raw.id,
    image: raw.image ?? '',
    width: raw.width,
    height: raw.height,
    format: raw.format ?? '',
    thumbnailImage: raw.thumbnailImage ?? '',
    thumbWidth: raw.thumbWidth,
    thumbHeight: raw.thumbHeight,
    thumbFormat: raw.thumbFormat ?? '',
    fileSizeBytes: raw.fileSizeBytes,
    uploadedAt: raw.uploadedAt,
    aiInsights: raw.aiInsights ?? null,
    similarity: raw.similarity ?? 0,
    inProgress: raw.inProgress,
  };
}

export interface GeneratedImageModel {
  url: string;
  width: number;
  height: number;
  contentType: string;
}

export function createGeneratedImageModel(raw: any): GeneratedImageModel {
  return {
    url: raw.url ?? '',
    width: raw.width,
    height: raw.height,
    contentType: raw.contentType ?? '',
  };
}
