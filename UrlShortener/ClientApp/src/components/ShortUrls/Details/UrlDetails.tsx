import React from "react";
import type { ShortUrlDetailsDto } from "@api-types/shorturls";

interface UrlDetailsProps {
  urlDetails: ShortUrlDetailsDto;
}

export const UrlDetails: React.FC<UrlDetailsProps> = ({ urlDetails }) => {
  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleString();
  };

  return (
    <div>
      <p>
        <strong>ID:</strong> {urlDetails.id}
      </p>
      <p>
        <strong>Original URL:</strong>
        <a
          href={urlDetails.originalUrl}
          target="_blank"
          rel="noopener noreferrer"
        >
          {urlDetails.originalUrl}
        </a>
      </p>
      <p>
        <strong>Short URL:</strong>
        <a href={urlDetails.shortUrl} target="_blank" rel="noopener noreferrer">
          {urlDetails.shortUrl}
        </a>
      </p>
      <p>
        <strong>Created At:</strong> {formatDate(urlDetails.createdAt)}
      </p>
      <p>
        <strong>Created By:</strong> {urlDetails.createdBy} (ID:{" "}
        {urlDetails.createdById})
      </p>
    </div>
  );
};
