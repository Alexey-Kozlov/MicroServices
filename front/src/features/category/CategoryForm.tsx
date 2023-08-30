import { Button, CircularProgress, Container, TextField, ThemeProvider, Typography } from "@mui/material";
import Grid2 from "@mui/material/Unstable_Grid2";
import { Controller, useForm } from "react-hook-form";
import InputTheme from "../../app/themes/InputTheme";
import { yupResolver } from '@hookform/resolvers/yup';
import * as Yup from 'yup';
import HeaderTheme from "../header/headerTheme";
import { useStore } from "../../app/stores/store";
import { Category, ICategory } from "../../app/models/icategory";
import { useNavigate, useParams } from "react-router-dom";
import { useEffect } from "react";
import { observer } from "mobx-react-lite";
import { grey } from "@mui/material/colors";

export default observer(function CategoryForm() {
    let { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const validSchema = Yup.object().shape({
        name: Yup.string().required('Необходимо указать наименование!'),
    });
    const { categoryStore: { addEditCategory, getCategoryById, isSubmitted } } = useStore();
    const { handleSubmit, control, formState: { errors }, reset } = useForm<ICategory>({
        defaultValues: new Category(0,""),
        resolver: yupResolver(validSchema)
    });
    const onSubmit = handleSubmit(data => {
        if (!id) {
            id = "0";
        } 
        addEditCategory(new Category(Number(id), data.name)).then(() => {
            navigate(`$/category`);
        })
    });
    const labelStyle = {
        width: "130px"
    }
    useEffect(() => {
        if (id) {
            getCategoryById(id).then((category) => {
                category && reset(category);
            });
        }
    }, [id, reset, getCategoryById]);
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
                                    name="name"
                                    control={control}
                                    render={({ field }) =>
                                        <>
                                            <TextField {...field}
                                                variant="outlined"
                                                label="Наименование"
                                                error={errors.name ? true : false} />
                                            <Typography variant="inherit" sx={HeaderTheme.typography.errorMessage}>
                                                {errors.name?.message}
                                            </Typography>
                                        </>
                                    }
                                />
                            </Grid2>
                        </Grid2>                                               
                        <Grid2 container direction="row" justifyContent="center">
                            <Grid2>
                                <Button variant="outlined" sx={{ marginRight: "10px" }}
                                    onClick={() => navigate('/category')}>
                                    Отмена
                                </Button>
                                <Button type="submit" variant="outlined"
                                    disabled={isSubmitted}>
                                    {id ? "Сохранить" : "Ввод"}
                                    {isSubmitted && (
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
})