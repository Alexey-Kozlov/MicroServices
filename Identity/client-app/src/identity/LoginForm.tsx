import { ThemeProvider } from "@emotion/react";
import { yupResolver } from "@hookform/resolvers/yup";
import { Button, CircularProgress, Container, TextField, Typography } from "@mui/material";
import Grid2 from "@mui/material/Unstable_Grid2";
import { Controller, useForm } from "react-hook-form";
import { ILogin, Login } from "./ilogin";
import InputTheme from "./InputTheme";
import * as Yup from 'yup';
import HeaderTheme from "./headerTheme";
import { grey } from "@mui/material/colors";
import agent from "./identityAgent";
import { useSearchParams } from "react-router-dom";

export default function LoginForm() {
    const [searchParams] = useSearchParams();
    const validSchema = Yup.object().shape({
        login: Yup.string().required('Необходимо указать логин!'),
        password: Yup.string().required('Необходимо указать пароль!')
    });
    const { handleSubmit, control, formState: { errors }, reset } = useForm<ILogin>({
        defaultValues: new Login ('', ''),
        resolver: yupResolver(validSchema)
    });
    const onSubmit = handleSubmit(data => {
        const returnUrl = searchParams.get('ReturnUrl');
        agent.Identity.login(data)
            .then((identity) => {
                let url = process.env.REACT_APP_FRONT! + '/token?token=' + identity.result.token;
                if (returnUrl) {
                    url = url + '&ReturnUrl=' + returnUrl;
                }
                window.location.href = url;
            })
            .catch(error => alert(error));
    });
    const labelStyle = {
        width: "130px"
    }

    return (
        <ThemeProvider theme={InputTheme}>
            <form onSubmit={onSubmit}>
                <Container maxWidth="lg">

                        <Grid2 container direction="column" rowSpacing={1} columnSpacing={1}>
                            <Grid2 container direction="row" justifyContent="center" alignItems="center">
                                <Grid2 style={labelStyle} >
                                    <label>Наименование</label>
                                </Grid2>
                                <Grid2 xs={6}>
                                <Controller
                                    name="login"
                                        control={control}
                                        render={({ field }) =>
                                            <>
                                                <TextField {...field}
                                                    variant="outlined"
                                                    label="Логин"
                                                    error={errors.login ? true : false} />
                                                <Typography variant="inherit" sx={HeaderTheme.typography.errorMessage}>
                                                    {errors.login?.message}
                                                </Typography>
                                            </>
                                        }
                                    />
                                </Grid2>
                            </Grid2>
                            <Grid2 container direction="row" justifyContent="center" alignItems="center">
                                <Grid2 style={labelStyle}>
                                    <label>Цена</label>
                                </Grid2>
                                <Grid2 xs={6}>
                                <Controller
                                    name="password"
                                        control={control}
                                        render={({ field }) =>
                                            <>
                                                <TextField {...field}
                                                    variant="outlined"
                                                    label="Пароль"
                                                    error={errors.password ? true : false} />
                                                <Typography variant="inherit" sx={HeaderTheme.typography.errorMessage}>
                                                    {errors.password?.message}
                                                </Typography>
                                            </>
                                        }
                                    />
                                </Grid2>
                            </Grid2>
                            
                            <Grid2 container direction="row" justifyContent="center">
                                <Grid2>
                                <Button variant="outlined" sx={{ marginRight: "10px" }}
                                    onClick={() => window.location.href = process.env.REACT_APP_FRONT!}>
                                        Отмена
                                    </Button>
                                    <Button type="submit" variant="outlined"
                                        disabled={false}>
                                        Ввод
                                        {false && (
                                            <CircularProgress
                                                size={24}
                                                sx={{
                                                    color: grey[500],
                                                    position: 'absolute',
                                                    top: '50%',
                                                    left: '50%',
                                                    marginTop: '-12px',
                                                    marginLeft: '-12px',
                                                }}
                                            />
                                        )}
                                    </Button>

                                </Grid2>
                            </Grid2>
                        </Grid2>                  
                </Container>
            </form>
        </ThemeProvider>
    )
}