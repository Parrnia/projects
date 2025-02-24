import { createTheme, alpha } from '@mui/material/styles';

const defaultTheme = createTheme();

const green = {
  50: '#E0F7F1',
  100: '#A0E5E2',
  200: '#5DD7D1',
  300: '#30C9C6',
  400: '#00B3B0',
  main: '#00a2a7',
  500: '#009C9A',
  600: '#008686',
  700: '#007B7B',
  800: '#007171',
  900: '#006565',
};

const greenDark = {
  50: '#B3E6E0',
  100: '#90D7D0',
  200: '#6CC2C1',
  300: '#4DA6A5',
  main: '#00a2a7',
  400: '#007A78',
  500: '#006C6A',
  600: '#005B5A',
  700: '#004E4D',
  800: '#004141',
  900: '#003636',
};

const grey = {
  50: '#F3F6F9',
  100: '#E7EBF0',
  200: '#E0E3E7',
  300: '#CDD2D7',
  400: '#B2BAC2',
  500: '#A0AAB4',
  600: '#6F7E8C',
  700: '#3E5060',
  800: '#2D3843',
  900: '#1A2027',
};


const systemFont = [
  'Vazir',
  '-apple-system',
  'BlinkMacSystemFont',
  '"Segoe UI"',
  'Roboto',
  '"Helvetica Neue"',
  'Arial',
  // 'sans-serif',
  '"Apple Color Emoji"',
  '"Segoe UI Emoji"',
  '"Segoe UI Symbol"',
];

export const getDesignTokens = (mode) => ({
  palette: {
    primary: {
      ...green,
      ...(mode === 'dark' && {
        main: green[400],
      }),
    },
    divider: mode === 'dark' ? alpha(green[100], 0.08) : grey[100],
    primaryDark: greenDark,
    mode,
    ...(mode === 'dark' && {
      background: {
        default: greenDark[800],
        paper: greenDark[900],
      },
    }),
    common: {
      black: '#1D1D1D',
    },
    text: {
      ...(mode === 'light' && {
        primary: grey[900],
        secondary: grey[700],
      }),
      ...(mode === 'dark' && {
        primary: '#fff',
        secondary: grey[400],
      }),
    },
    grey: {
      ...(mode === 'light' && {
        main: grey[100],
        contrastText: grey[600],
      }),
      ...(mode === 'dark' && {
        main: greenDark[700],
        contrastText: grey[600],
      }),
    },
    error: {
      50: '#FFF0F1',
      100: '#FFDBDE',
      200: '#FFBDC2',
      300: '#FF99A2',
      400: '#FF7A86',
      500: '#FF505F',
      main: '#EB0014',
      600: '#EB0014',
      700: '#C70011',
      800: '#94000D',
      900: '#570007',
    },
    success: {
      50: '#E9FBF0',
      100: '#C6F6D9',
      200: '#9AEFBC',
      300: '#6AE79C',
      400: '#3EE07F',
      500: '#21CC66',
      600: '#1DB45A',
      ...(mode === 'light' && {
        main: '#1AA251',
      }),
      ...(mode === 'dark' && {
        main: '#1DB45A',
      }),
      700: '#1AA251',
      800: '#178D46',
      900: '#0F5C2E',
    },
    warning: {
      50: '#FFF9EB',
      100: '#FFF3C1',
      200: '#FFECA1',
      300: '#FFDC48',
      400: '#F4C000', 
      500: '#DEA500', 
      main: '#DEA500',
      600: '#D18E00',
      700: '#AB6800', 
      800: '#8C5800',
      900: '#5A3600',
    },
    action: {
      ...(mode === 'dark' && {
        hoverOpacity: 0.08,
        active: grey[700],
        disabled: grey[200],
      }),
      ...(mode === 'light' && {
        hoverOpacity: 0.04,
        active: grey[700],
        disabled: alpha(grey[700], 0.3),
      }),
    },
  },
  shape: {
    borderRadius: 10,
  },
  spacing: 10,
  typography: {
    fontFamily: ['"Vazir"', ...systemFont].join(','),
    fontFamilyCode: [
      'Consolas',
      'Menlo',
      'Monaco',
      'Andale Mono',
      'Ubuntu Mono',
      'monospace',
    ].join(','),
    fontFamilyTagline: ['"Vazir"', ...systemFont].join(','),
    fontFamilySystem: systemFont.join(','),
    fontWeightSemiBold: 600,
    fontWeightExtraBold: 800,
    h1: {
      fontFamily: ['"Vazir"', ...systemFont].join(','),
      fontSize: 'clamp(2.625rem, 1.2857rem + 3.5714vw, 4rem)',
      fontWeight: 800,
      lineHeight: 78 / 70,
      ...(mode === 'light' && {
        color: greenDark[900],
      }),
    },
    h2: {
      fontFamily: ['"Vazir"', ...systemFont].join(','),
      fontSize: 'clamp(1.5rem, 0.9643rem + 1.4286vw, 2.25rem)',
      fontWeight: 800,
      lineHeight: 44 / 36,
      color: mode === 'dark' ? grey[100] : greenDark[700],
    },
    h3: {
      fontFamily: ['"Vazir"', ...systemFont].join(','),
      fontSize: defaultTheme.typography.pxToRem(36),
      lineHeight: 44 / 36,
      letterSpacing: 0.2,
    },
    h4: {
      fontFamily: ['"Vazir"', ...systemFont].join(','),
      fontSize: defaultTheme.typography.pxToRem(28),
      lineHeight: 42 / 28,
      letterSpacing: 0.2,
    },
    h5: {
      fontFamily: ['"Vazir"', ...systemFont].join(','),
      fontSize: defaultTheme.typography.pxToRem(24),
      lineHeight: 36 / 24,
      letterSpacing: 0.1,
      color: mode === 'dark' ? green[300] : green.main,
    },
    h6: {
      fontSize: defaultTheme.typography.pxToRem(20),
      lineHeight: 30 / 20,
    },
    button: {
      textTransform: 'initial',
      fontWeight: 700,
      letterSpacing: 0,
    },
    MuiButton: {
      defaultProps: {
        size: 'large',
      }
    },
    subtitle1: {
      fontSize: defaultTheme.typography.pxToRem(18),
      lineHeight: 24 / 18,
      letterSpacing: 0,
      fontWeight: 500,
    },
    body1: {
      fontSize: defaultTheme.typography.pxToRem(16), //16px
      lineHeight: 24 / 16,
      letterSpacing: 0,
    },
    body2: {
      fontSize: defaultTheme.typography.pxToRem(14), //14px
      lineHeight: 21 / 14,
      letterSpacing: 0,
    },
    caption: {
      fontSize: defaultTheme.typography.pxToRem(12), //12px
      lineHeight: 18 / 12,
      letterSpacing: 0,
      fontWeight: 700,
    },
    allVariants: {
      scrollMarginTop: 'calc(var(--MuiDocs-header-height) + 32px)',
    },
  },
});