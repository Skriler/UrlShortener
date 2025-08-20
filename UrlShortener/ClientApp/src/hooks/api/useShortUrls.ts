import { shorturlsApi } from "@api/endpoints/shorturls";
import type { CreateUrlDto } from "@api-types/shorturls";

export const useShortUrlsApi = () => {
  const getAllUrls = async () => {
    return await shorturlsApi.getAllUrls();
  };

  const getUrlDetails = async (id: number) => {
    return await shorturlsApi.getUrlDetails(id);
  };

  const createShortUrl = async (createUrlDto: CreateUrlDto) => {
    return await shorturlsApi.createShortUrl(createUrlDto);
  };

  const deleteUrl = async (id: number) => {
    await shorturlsApi.deleteUrl(id);
  };

  return {
    getAllUrls,
    getUrlDetails,
    createShortUrl,
    deleteUrl,
  };
};
