import { ThemeProvider } from "@emotion/react";
import { yupResolver } from "@hookform/resolvers/yup";
import { Button, CircularProgress, Container, TextField, Typography } from "@mui/material";
import Grid2 from "@mui/material/Unstable_Grid2";
import { Controller, useForm } from "react-hook-form";
import { ILogin, Login } from "../../app/models/ilogin";
import InputTheme from "../../app/themes/InputTheme";
import * as Yup from 'yup';
import HeaderTheme from "../header/headerTheme";
import { store } from "../../app/stores/store";
import { grey } from "@mui/material/colors";
import { useEffect } from "react";

export default function LoginForm() {
    const validSchema = Yup.object().shape({
        login: Yup.string().required('Необходимо указать логин!'),
        password: Yup.string().required('Необходимо указать пароль!')
    });
    const { handleSubmit, control, formState: { errors }, reset } = useForm<ILogin>({
        defaultValues: new Login ('', ''),
        resolver: yupResolver(validSchema)
    });
    const onSubmit = handleSubmit(data => {
        store.identityStore.login(data)
            .then(() => store.commonStore.navigation!('/'))
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
                                        onClick={() => store.commonStore.navigation!('/')}>
                                        Отмена
                                    </Button>
                                    <Button type="submit" variant="outlined"
                                        disabled={false}>
                                        "Ввод"
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