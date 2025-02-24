import Footer from "../Footer/Footer";
import Sidebar from "../Sidebar/Sidebar";
import ThemeProvider from "../Theme/ThemeProvider";
import React from "react";
import { Box } from "@mui/material";

const MainLayout = ({ children }) => {
  return (
    <ThemeProvider>
      <Box
        sx={{
          display: "flex",
          flexDirection: "column",
          minHeight: "100vh",
        }}
      >
        <Box
          sx={{
            display: "flex",
            flex: 1,
            flexDirection: "row",
            width: "100%",
            paddingBottom: "64px"
          }}
        >
          <Box
            sx={{
              display: { xs: "none", sm: "block" },
              flexShrink: 0,
              width: { sm: "20%", md: "19%" },
              position: "sticky",
              top: 0,
              height: "100vh",
              overflowY: "auto",
              backgroundColor: "background.default",
              zIndex: 999,
            }}
          >
            <Sidebar />
          </Box>

          <Box
            sx={{
              flex: 1,
              padding: { xs: 2, sm: 3, md: 4 },
              marginLeft: { xs: 0, sm: "auto" },
              marginRight: "auto",
              overflowY: "auto"
            }}
          >
            {children}
          </Box>
        </Box>

        <Box
          sx={{
            position: "fixed",
            bottom: 0,
            left: 0,
            width: "100%",
            backgroundColor: "background.paper",
            boxShadow: "0 -2px 5px rgba(0,0,0,0.1)",
            zIndex: 1000,
          }}
        >
          <Footer />
        </Box>
      </Box>
    </ThemeProvider>
  );
};

export default MainLayout;