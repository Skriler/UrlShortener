import type { AxiosResponse } from "axios";
import type {
  CreateUrlDto,
  ShortUrlDetailsDto,
  ShortUrlDto,
} from "@api-types/shorturls";
import { httpClient } from "api/client";

export const shorturlsApi = {
  BASE_URL: "/shorturls",

  async getAllUrls(): Promise<ShortUrlDto[]> {
    const response: AxiosResponse<ShortUrlDto[]> = await httpClient.get(
      `${this.BASE_URL}`
    );
    return response.data;
  },

  async getUrlDetails(id: number): Promise<ShortUrlDetailsDto> {
    const response: AxiosResponse<ShortUrlDetailsDto> = await httpClient.get(
      `${this.BASE_URL}/${id}`
    );
    return response.data;
  },

  async createShortUrl(
    createUrlDto: CreateUrlDto
  ): Promise<ShortUrlDetailsDto> {
    const response: AxiosResponse<ShortUrlDetailsDto> = await httpClient.post(
      `${this.BASE_URL}`,
      createUrlDto
    );
    return response.data;
  },

  async deleteUrl(id: number): Promise<void> {
    await httpClient.delete(`${this.BASE_URL}/${id}`);
  },
};
