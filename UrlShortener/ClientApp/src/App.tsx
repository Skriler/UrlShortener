import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { AuthProvider } from "@components/AuthProvider";
import { ProtectedRoute } from "@components/ProtectedRoute";
import { Navbar } from "@components/Navbar";
import LoginPage from "@pages/LoginPage";
import UrlsTablePage from "@pages/UrlsTablePage";
import UrlDetailsPage from "@pages/UrlDetailsPage";

const App: React.FC = () => {
  return (
    <AuthProvider>
      <Router>
        <div>
          <Navbar />
          <main>
            <Routes>
              <Route path="/login" element={<LoginPage />} />
              <Route path="/" element={<UrlsTablePage />} />
              <Route
                path="/url/:id"
                element={
                  <ProtectedRoute>
                    <UrlDetailsPage />
                  </ProtectedRoute>
                }
              />
            </Routes>
          </main>
        </div>
      </Router>
    </AuthProvider>
  );
};

export default App;
