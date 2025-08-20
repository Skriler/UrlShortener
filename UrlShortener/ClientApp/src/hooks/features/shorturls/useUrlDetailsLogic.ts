import { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import type { ShortUrlDetailsDto } from "@api-types/shorturls";
import { useShortUrlsApi } from "@hooks/api/useShortUrls";

export const useUrlDetailsLogic = () => {
  const { id } = useParams<{ id: string }>();
  const [urlDetails, setUrlDetails] = useState<ShortUrlDetailsDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const shortUrlsApi = useShortUrlsApi();

  useEffect(() => {
    const loadUrlDetails = async () => {
      if (!id) {
        setError("No URL ID provided");
        setLoading(false);
        return;
      }

      try {
        const details = await shortUrlsApi.getUrlDetails(parseInt(id));
        setUrlDetails(details);
      } catch (err: unknown) {
        const error = err as { response?: { status: number } };
        if (error.response?.status === 404) {
          setError("URL not found");
        } else {
          setError("Failed to load URL details");
        }
      } finally {
        setLoading(false);
      }
    };

    loadUrlDetails();
  }, [id, shortUrlsApi]);

  return {
    urlDetails,
    loading,
    error,
  };
};
