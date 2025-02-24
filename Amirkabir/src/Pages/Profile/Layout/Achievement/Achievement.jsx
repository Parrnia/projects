import React from "react";
import { Box, Typography, LinearProgress } from "@mui/material";
import StairsIcon from "../../../../Images/Profile/Stairs.jfif";

const Achievement = (props) => (
    <Box
        sx={{
            mb: props.mb,
            display: "flex",
            alignItems: "center",
            justifyContent: "flex-start",
            gap: 2
        }}
    >
        <Box
            sx={{
                flexShrink: 0,
                width: { xs: "15%", sm: "10%", md: "8%" },
                maxWidth: "80px",
                minWidth: "40px",
            }}
        >
            <img
                src={StairsIcon}
                alt="Achievement Icon"
                style={{ width: "100%", height: "auto" }}
            />
        </Box>

        <Box sx={{ flex: 1 }}>
            <Box
                sx={{
                    display: "flex",
                    justifyContent: "space-between",
                    alignItems: "center",
                    mb: 1,
                }}
            >
                <Typography
                    variant="body2"
                    color="text.secondary"
                    sx={{ textAlign: "right", fontWeight: 500 }}
                >
                    {props.title}
                </Typography>

                <Typography
                    variant="caption"
                    color="text.secondary"
                    sx={{ textAlign: "left" }}
                >
                    {props.progress}/{props.goal}
                </Typography>
            </Box>

            <LinearProgress
                variant="determinate"
                value={(props.progress / props.goal) * 100}
             
                sx={{
                    height: 8,
                    borderRadius: 1,
                    backgroundColor: "divider",
                    "& .MuiLinearProgress-bar": {
                        backgroundColor: "#FFC107",
                    },
                }}
            />

            <Typography
                variant="body2"
                color="text.secondary"
                sx={{ mt: 1, textAlign: "right", fontWeight: 500 }}
            >
                برای {props.goal} روز پشت سر هم به یادگیری ادامه بده
            </Typography>
        </Box>
    </Box>
);

export default Achievement;