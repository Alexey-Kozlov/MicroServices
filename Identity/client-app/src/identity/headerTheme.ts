import { createTheme } from '@mui/material/styles';
import { grey } from "@mui/material/colors";

declare module "@mui/material/styles" {
    interface TypographyVariantsOptions {
        tab: React.CSSProperties;
        errorMessage: React.CSSProperties;
    }
    interface TypographyVariants {
        tab: React.CSSProperties;
        errorMessage: React.CSSProperties;
    }
}

export default createTheme({
    typography: {
        tab: {
            textTransform: "none",
            color: grey[500],
            fontSize: "1rem",
            minWidth: 10
        },
        errorMessage: {
            color:"red"
        }
    },
    components: {
        MuiAppBar: {
            styleOverrides: {
                root: {
                    backgroundColor: grey['A100'],
                    height: "64px"
                }
            }
        },
        MuiToolbar: {
            styleOverrides: {
                root: {
                    justifyContent: "center"
                }
            }
        },
        MuiTabs: {
            styleOverrides: {
                indicator: {
                    height: "5px",
                    transition: "none",
                    display: "",
                    backgroundColor: grey[800]
                }
            }
        },
        MuiTab: {
            styleOverrides: {
                root: {                   
                    '&.Mui-selected': {
                        color: "black",
                        fontWeight:"600"
                    }
                }
            }
        }
    }
});