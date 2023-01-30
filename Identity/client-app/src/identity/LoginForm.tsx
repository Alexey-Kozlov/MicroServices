import { ThemeProvider } from "@emotion/react";
import { yupResolver } from "@hookform/resolvers/yup";
import { Box, Button, CircularProgress, Container, Modal, Stack, TextField, Typography } from "@mui/material";
import Grid2 from "@mui/material/Unstable_Grid2";
import { Controller, useForm } from "react-hook-form";
import { ILogin, Login } from "./ilogin";
import InputTheme from "./InputTheme";
import * as Yup from 'yup';
import HeaderTheme from "./headerTheme";
import { grey } from "@mui/material/colors";
import agent from "./identityAgent";
import { Link, useSearchParams } from "react-router-dom";
import { useEffect, useState } from "react";

export default function LoginForm() {
    const [searchParams] = useSearchParams();
    const [open, setOpen] = useState(false);
    const validSchema = Yup.object().shape({
        login: Yup.string().required('Необходимо указать логин!'),
        password: Yup.string().required('Необходимо указать пароль!')
    });
    const { handleSubmit, control, formState: { errors } } = useForm<ILogin>({
        defaultValues: new Login('', ''),
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
            .catch(error => {
                alert(error);

            });
    });
    const labelStyle = {
        width: "130px"
    }

    useEffect(() => {
        setOpen(true);
    }, [setOpen])

    const handleClose = () => {
        setOpen(false);
        window.location.href = process.env.REACT_APP_FRONT!;
    }
    const modalStyle = {
        position: 'absolute' as 'absolute',
        top: '50%',
        left: '50%',
        transform: 'translate(-50%, -50%)',
        width: '500px',
        minHeight: '200px',
        bgcolor: 'background.paper',
        border: '2px solid #000',
        boxShadow: 24
    }
    return (
        <Modal
            open={open}
            onClose={handleClose}
        >
            <Box sx={modalStyle}>
                <form onSubmit={onSubmit}>
                    <ThemeProvider theme={InputTheme}>
                        <Container maxWidth="lg" sx={{ marginTop: "20px" }}>
                            <h3 style={{ textAlign:"center" }}>Идентификация пользователя</h3>
                            <Grid2 container direction="column" rowSpacing={1} columnSpacing={1}>
                                <Grid2 container direction="row" justifyContent="center" alignItems="center">
                                    <Grid2 style={labelStyle} >
                                        <label>Логин</label>
                                    </Grid2>
                                    <Grid2 xs={6}>
                                        <Controller
                                            name="login"
                                            control={control}
                                            render={({ field }) =>
                                                <>
                                                    <TextField {...field}
                                                        size="small"
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
                                        <label>Пароль</label>
                                    </Grid2>
                                    <Grid2 xs={6}>
                                        <Controller
                                            name="password"
                                            control={control}
                                            render={({ field }) =>
                                                <>
                                                    <TextField {...field}
                                                        size="small"
                                                        variant="outlined"
                                                        label="Пароль"
                                                        type="password"
                                                        error={errors.password ? true : false} />
                                                    <Typography variant="inherit" sx={HeaderTheme.typography.errorMessage}>
                                                        {errors.password?.message}
                                                    </Typography>
                                                </>
                                            }
                                        />
                                    </Grid2>
                                </Grid2>
                            </Grid2>
                            <Box
                                display="flex"
                                justifyContent="center"
                                alignItems="center"
                                sx={{ marginTop: "20px", marginBottom:"20px" }}
                            >
                                <Stack spacing={2}>
                                    <Link to="/newaccount" >Создать нового пользователя</Link>
                                    <Box display="flex"
                                        justifyContent="center"
                                        alignItems="center">
                                        <Button variant="outlined" sx={{ marginRight: "10px" }}
                                            onClick={() => handleClose()}>
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
                                    </Box>  
                                </Stack>                                                                                                                                
                            </Box>                          
                        </Container>
                    </ThemeProvider>
                </form>

            </Box>
        </Modal >

    )
}