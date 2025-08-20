import React from "react";
import { Link } from "react-router-dom";
import type { ShortUrlDto } from "@api-types/shorturls";

interface UrlsTableProps {
  urls: ShortUrlDto[];
  isAuthenticated: boolean;
  handleDelete: (id: number) => void;
  canDelete: (createdBy: string) => boolean;
}

export const UrlsTable: React.FC<UrlsTableProps> = ({
  urls,
  isAuthenticated,
  handleDelete,
  canDelete,
}) => {
  return (
    <div>
      <h3>All URLs</h3>
      {urls.length === 0 ? (
        <p>No URLs found</p>
      ) : (
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Original URL</th>
              <th>Short Code</th>
              <th>Created By</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {urls.map((url) => (
              <tr key={url.id}>
                <td>{url.id}</td>
                <td>
                  <a
                    href={url.originalUrl}
                    target="_blank"
                    rel="noopener noreferrer"
                  >
                    {url.originalUrl}
                  </a>
                </td>
                <td>{url.shortCode}</td>
                <td>{url.createdBy}</td>
                <td>
                  {isAuthenticated && (
                    <Link to={`/url/${url.id}`}>View Details</Link>
                  )}
                  {canDelete(url.createdBy) && (
                    <button
                      onClick={() => handleDelete(url.id)}
                      style={{ marginLeft: "10px" }}
                    >
                      Delete
                    </button>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};
