import { Snackbar, Alert } from '@mui/material';

const alert = (props) => (
    <Snackbar
        open={props.open}
        autoHideDuration={6000}
        onClose={() => props.setOpen(false)} 
        anchorOrigin={{ vertical: 'top', horizontal: 'center' }}
    >
        <div style={{  padding: '0 16px',mr: { md: 7, lg: 18, xl:0 } }}>
            <Alert 
                severity={props.severity}

                sx={{
                    width: '100%',
                    p: { xs: 0.5, md:3, lg: 1.3, xl: 1.3 },
                    marginLeft:'15px',
                    fontSize: '1.1rem',
                    textAlign: 'center',
                }}
            >
                {props.text}
            </Alert>
        </div>
    </Snackbar>
);

export default alert;