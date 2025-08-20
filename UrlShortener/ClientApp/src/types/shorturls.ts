export interface CreateUrlDto {
  originalUrl: string;
}

export interface ShortUrlDto {
  id: number;
  originalUrl: string;
  shortCode: string;
  createdBy: string;
}

export interface ShortUrlDetailsDto {
  id: number;
  originalUrl: string;
  shortUrl: string;
  createdAt: string;
  createdBy: string;
  createdById: string;
}
