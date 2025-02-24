import DiamondIcon from "@mui/icons-material/Diamond";
import { Box, Grid, Typography, Paper } from "@mui/material";
import FlameIcon from "@mui/icons-material/LocalFireDepartment";
import WorkspacePremiumTwoToneIcon from '@mui/icons-material/WorkspacePremiumTwoTone';

const Profile_Status = (props) => {
    return (
        <Box
            sx={{
                p: 1,
                backgroundColor: "background.default",
                display: "flex",
                justifyContent: "center",
            }}
        >
            <Box
                sx={{
                    width: "100%"
                }}
            >
                <Typography
                    variant="h5"
                    color="text.primary"
                    fontWeight={500}
                    sx={{ mb: 2, textAlign: "right" }}
                >   
                    وضعیت
                </Typography>
                <Grid container spacing={2}>
                    <Grid item xs={12} sm={6}>
                        <Paper
                            elevation={3}
                            sx={{
                                p: 1.5,
                                backgroundColor: "background.paper",
                                borderRadius: 2,
                            }}
                        >
                            <Box
                                sx={{
                                    display: "flex",
                                    alignItems: "center",
                                    gap: 0.5
                                }}
                            >
                                <FlameIcon sx={{ color: "error.main", fontSize: 25 }} /> 
                                <Typography
                                    variant="h6"
                                    color="primary.main"
                                    fontWeight={700}
                                >
                                    3
                                </Typography>
                            </Box>
                            <Typography
                                variant="body2"
                                color="text.secondary"
                                sx={{ mt: 0.5 }}
                            >
                                روزهای پشت سر هم
                            </Typography>
                        </Paper>
                    </Grid>
                    <Grid item xs={12} sm={6}>
                        <Paper
                            elevation={3}
                            sx={{
                                p: 1.5,
                                backgroundColor: "background.paper",
                                borderRadius: 2,
                            }}
                        >
                            <Box
                                sx={{
                                    display: "flex",
                                    alignItems: "center",
                                    gap: 0.5
                                }}
                            >
                                <DiamondIcon sx={{ color: "primary.main", fontSize: 25 }} />
                                <Typography
                                    variant="h6" 
                                    color="primary.main"
                                    fontWeight={700}
                                >
                                    1234
                                </Typography>
                            </Box>
                            <Typography
                                variant="body2"
                                color="text.secondary"
                                sx={{ mt: 0.5 }}
                            >
                                مجموع امتیازات کسب شده
                            </Typography>
                        </Paper>
                    </Grid>

                    <Grid item xs={12} sm={6}>
                        <Paper
                            elevation={3}
                            sx={{
                                p: 1.5,
                                backgroundColor: "background.paper",
                                borderRadius: 2,
                            }}
                        >
                            <Box
                                sx={{
                                    display: "flex",
                                    alignItems: "center",
                                    gap: 0.5
                                }}
                            >
                                <WorkspacePremiumTwoToneIcon
                                sx={{ color: "warning.main", fontSize: 25 }}
                                /> 
                                <Typography
                                    variant="h6"
                                    color="primary.main"
                                    fontWeight={700}
                                >
                                    1
                                </Typography>
                            </Box>
                            <Typography
                                variant="body2"
                                color="text.secondary"
                                sx={{ mt: 0.5 }}
                            >
                                3 فیتالیست برتر
                            </Typography>
                        </Paper>
                    </Grid>
                </Grid>
            </Box>
        </Box>
    );
};

export default Profile_Status;