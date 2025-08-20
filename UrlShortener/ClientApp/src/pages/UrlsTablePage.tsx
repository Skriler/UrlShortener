import React from "react";
import { useUrlsTableLogic } from "@hooks/features/shorturls";
import { UrlsTable, CreateUrlForm } from "@components/ShortUrls/Table";

const UrlsTablePage: React.FC = () => {
  const logic = useUrlsTableLogic();

  if (logic.loading) {
    return <div>Loading...</div>;
  }

  return (
    <div>
      <h2>Short URLs</h2>

      {logic.error && <div style={{ color: "red" }}>{logic.error}</div>}

      {logic.isAuthenticated && <CreateUrlForm {...logic} />}

      <UrlsTable {...logic} />
    </div>
  );
};

export default UrlsTablePage;
