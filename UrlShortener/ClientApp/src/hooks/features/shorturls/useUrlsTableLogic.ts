import { useState, useEffect, useCallback } from "react";
import type { ShortUrlDto } from "@api-types/shorturls";
import { useShortUrlsApi } from "@hooks/api/useShortUrls";
import { useAuthContext } from "@hooks/features/auth";

export const useUrlsTableLogic = () => {
  const [urls, setUrls] = useState<ShortUrlDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const [newUrl, setNewUrl] = useState("");
  const [creating, setCreating] = useState(false);
  const [createError, setCreateError] = useState("");

  const { isAuthenticated, isAdmin, user } = useAuthContext();
  const shortUrlsApi = useShortUrlsApi();

  const loadUrls = useCallback(async () => {
    try {
      const urlsData = await shortUrlsApi.getAllUrls();
      setUrls(urlsData);
      setError("");
    } catch {
      setError("Failed to load URLs");
    } finally {
      setLoading(false);
    }
  }, [shortUrlsApi]);

  useEffect(() => {
    loadUrls();
  }, [loadUrls]);

  const handleCreateUrl = async (e: React.FormEvent) => {
    e.preventDefault();
    setCreateError("");
    setCreating(true);

    try {
      await shortUrlsApi.createShortUrl({ originalUrl: newUrl });
      setNewUrl("");
      await loadUrls();
    } catch (err: unknown) {
      const error = err as { response?: { status: number } };
      if (error.response?.status === 400) {
        setCreateError("Invalid URL or URL already exists");
      } else {
        setCreateError("Failed to create short URL");
      }
    } finally {
      setCreating(false);
    }
  };

  const handleDelete = async (id: number) => {
    if (!window.confirm("Are you sure you want to delete this URL?")) {
      return;
    }

    try {
      await shortUrlsApi.deleteUrl(id);
      await loadUrls();
    } catch {
      alert("Failed to delete URL");
    }
  };

  const canDelete = (createdBy: string) => {
    return isAdmin || (isAuthenticated && user?.username === createdBy);
  };

  return {
    urls,
    loading,
    error,
    newUrl,
    creating,
    createError,
    isAuthenticated,
    setNewUrl,
    handleCreateUrl,
    handleDelete,
    canDelete,
  };
};
