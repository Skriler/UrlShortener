import React from "react";
import { Link } from "react-router-dom";
import { useUrlDetailsLogic } from "@hooks/features/shorturls";
import { UrlDetails } from "@components/ShortUrls/Details";

const UrlDetailsPage: React.FC = () => {
  const { urlDetails, loading, error } = useUrlDetailsLogic();

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return (
      <div>
        <h2>Error</h2>
        <p style={{ color: "red" }}>{error}</p>
        <Link to="/">Back to URLs Table</Link>
      </div>
    );
  }

  if (!urlDetails) {
    return (
      <div>
        <h2>URL Not Found</h2>
        <Link to="/">Back to URLs Table</Link>
      </div>
    );
  }

  return (
    <div>
      <h2>URL Details</h2>
      <UrlDetails urlDetails={urlDetails} />
      <div>
        <Link to="/">Back to URLs Table</Link>
      </div>
    </div>
  );
};

export default UrlDetailsPage;
