import { Visibility, VisibilityOff } from '@mui/icons-material';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import PermIdentityIcon from '@mui/icons-material/PermIdentity';
import PhoneEnabledOutlinedIcon from '@mui/icons-material/PhoneEnabledOutlined';
import { TextField, Typography, Box, InputAdornment, IconButton, FormHelperText } from '@mui/material';

const isPersian = (text) => {
  const persianRegex = /[\u0600-\u06FF]/;
  return persianRegex.test(text);
};

const TextInput = (props) => (
    <Box sx={{ display: 'flex', flexDirection: 'column', ...(!props.signup && { gap: 1 }), }}>
        <Typography sx={{ fontSize: '16px' }}>{props.label}</Typography>
        
        <Box sx={{ position: 'relative', display: 'flex', alignItems: 'center' }}>
            {props.LockOutlinedIcon && (
                <LockOutlinedIcon
                    sx={{
                        position: 'absolute',
                        right: 10,
                        pointerEvents: 'none',
                        marginLeft: '8px'
                    }}
                />
            )}

            <TextField
                name={props.name}
                value={props.value}
                onChange={props.onChange}
                type={props.type}
                placeholder={props.placeholder}
                inputProps={{
                    style: {
                      fontSize: '14px',
                        height: '20px',
                        padding: '15px',
                        paddingRight: props.LockOutlinedIcon       ? '23px' : 
                                      props.icon                   ? '4px'  :
                                      props.showPasswordToggle     ? '0px'  : '13px',
                        textAlign: props.value && !isPersian(props.value) ? 'left' : 'right',
                    }
                }}
                error={Boolean(props.error)}
             
                InputProps={{
                    ...(props.PermIdentityIcon && {
                        startAdornment: (
                          <InputAdornment position="start" sx={{ pr: 0.5 }}>
                            <PermIdentityIcon />
                          </InputAdornment>
                        ),
                     }),
                    ...(props.PhoneEnabledOutlinedIcon && {
                      startAdornment: (
                        <InputAdornment position="start" sx={{ pr: 0.5 }}>
                            <PhoneEnabledOutlinedIcon />
                        </InputAdornment>
                      ),
                    }),
                    ...(props.showPasswordToggle && {
                      endAdornment: (
                        <InputAdornment position="end">
                          <IconButton onClick={props.showPasswordToggle} edge="end">
                            {props.type === "text" ? <VisibilityOff /> : <Visibility />}
                          </IconButton>
                        </InputAdornment>
                      ),
                    }),
                  }}
                  
                sx={{
                    width: '100%'
                }}
            />
        </Box>

        {props.error && (
            <FormHelperText error sx={{ textAlign: 'right', color: 'red' }}>
                {props.error}
            </FormHelperText>
        )}
    </Box>
);

export default TextInput;