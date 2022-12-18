import { createTheme } from '@mui/material/styles';

export default createTheme({
    components: {
        MuiTextField: {
            styleOverrides: {
                root: {
                    width: "100%"
                }
            }
        }       
    }
});