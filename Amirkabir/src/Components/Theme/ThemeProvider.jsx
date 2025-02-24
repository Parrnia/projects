import { getDesignTokens } from './brandingTheme';
import React, { useMemo } from 'react';
import { AppProvider } from '@toolpad/core/AppProvider';
import { useColorScheme, createTheme } from '@mui/material';

const ThemeProvider = ({ children }) => {
    const { mode, systemMode } = useColorScheme();
    const calculatedMode = (mode === 'system' ? systemMode : mode) ?? 'light';
  
    const THEME = useMemo(() => {
      const brandingDesignTokens = getDesignTokens(calculatedMode);

      return createTheme({
          ...brandingDesignTokens,
          palette: {
              ...brandingDesignTokens.palette,
              mode: calculatedMode,
          },
      });
    }, [calculatedMode]); 

    return (
        <AppProvider theme={THEME}>
            {children}
        </AppProvider>
    );
};

export default ThemeProvider;