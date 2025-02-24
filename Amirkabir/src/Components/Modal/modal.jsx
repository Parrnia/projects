import React, { useState } from 'react';
import { Box, Modal, Typography } from '@mui/material';

const KeepMountedModal = (props) => {
  const [componentKey, setComponentKey] = useState(0);


  const handleClose = () => {
    props.onClose();
    setComponentKey((prevKey) => prevKey + 1);
  };


  return (
    <Modal
      keepMounted
      open={props.open}
      onClose={handleClose}
      aria-labelledby="keep-mounted-modal-title"
      aria-describedby="keep-mounted-modal-description"
    >
      <Box
        sx={{
          position: 'absolute',
          top: '50%',
          left: '50%',
          transform: 'translate(-50%, -50%)',

          width: {
            xs: '90%',
            sm: '50%',
            md: '45%', 
            lg: '35%',  
            xl: '23%', 
          },

          height: 'auto',
          maxWidth: '95%',
          maxHeight: '95%',
          bgcolor: 'background.paper',
          boxShadow: 24,

          p: {
            xs: 2,
            sm: 3,
            md: 4,
          },

          borderRadius: '10px',
          overflow: 'auto',
        }}
      >
        <Typography
          sx={{
            fontSize: {
              xs: '1rem',
              sm: '1.2rem',
              md: '1.5rem',
              lg: '1.8rem',
            },

            fontWeight: 'bold',
            textAlign: 'center',
            marginBottom: 2,
          }}

          id="keep-mounted-modal-title"
        >
          {props.title}
        </Typography>

        {React.cloneElement(props.component, { key: componentKey })}
      </Box>
    </Modal>
  );
};

export default KeepMountedModal;