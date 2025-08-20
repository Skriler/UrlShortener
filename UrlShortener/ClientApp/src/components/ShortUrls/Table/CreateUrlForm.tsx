import React from "react";

interface CreateUrlFormProps {
  newUrl: string;
  creating: boolean;
  createError: string;
  setNewUrl: (value: string) => void;
  handleCreateUrl: (e: React.FormEvent) => void;
}

export const CreateUrlForm: React.FC<CreateUrlFormProps> = ({
  newUrl,
  creating,
  createError,
  setNewUrl,
  handleCreateUrl,
}) => {
  return (
    <div>
      <h3>Add New URL</h3>
      {createError && <div style={{ color: "red" }}>{createError}</div>}

      <form onSubmit={handleCreateUrl}>
        <div>
          <label>
            URL to shorten:
            <input
              type="url"
              value={newUrl}
              onChange={(e) => setNewUrl(e.target.value)}
              required
              maxLength={2048}
              placeholder="https://example.com"
            />
          </label>
        </div>
        <button type="submit" disabled={creating}>
          {creating ? "Creating..." : "Create Short URL"}
        </button>
      </form>
    </div>
  );
};
