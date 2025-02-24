import Sidebar from "../../Components/Sidebar/Sidebar";
import ThemeProvider from "../../Components/Theme/ThemeProvider";
import { Box } from '@mui/material';
import { useTheme } from '@mui/material/styles';
import useMediaQuery from '@mui/material/useMediaQuery';

const LayoutSidebar = (props) => {
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('sm'));
    const isLargeScreen = useMediaQuery(theme.breakpoints.up('md'));

    return (
        <ThemeProvider>
            <Box
                sx={{
                    display: "flex",
                    flexDirection: "column",
                    width: { xs: "100%", sm: "0%", md: "19%" },
                }}
            >
                <Sidebar />
            </Box>
        
            <Box
                sx={{
                    flex: 1,
                    marginTop: isMobile ? '56px' : 0,
                    padding: isMobile ? '10px' : '20px',
                    transition: 'margin-left 0.3s ease',
                    marginRight: isLargeScreen ? '290px' : 0,
                }}
            >
                {props.component}
            </Box>
        </ThemeProvider>
    );
};

export default LayoutSidebar;