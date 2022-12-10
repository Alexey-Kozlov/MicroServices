import { Button, Container, TextField, ThemeProvider, Typography } from "@mui/material";
import Grid2 from "@mui/material/Unstable_Grid2";
import { Controller, useForm } from "react-hook-form";
import InputTheme from "../../app/themes/InputTheme";
import { yupResolver } from '@hookform/resolvers/yup';
import * as Yup from 'yup';
import HeaderTheme from "../header/headerTheme";
import { store, useStore } from "../../app/stores/store";
import { Category } from "../../app/models/icategory";

type FormData = {
    id: number;
    name: string;
}

export default function CategoryForm() {
    const validSchema = Yup.object().shape({
        name: Yup.string().required('Необходимо указать наименование!'),
    });
    const { categoryStore: { addCategory } } = useStore();
    const { handleSubmit, control, formState: { errors } } = useForm<FormData>({
        defaultValues: {
            name: ""
        },
        resolver: yupResolver(validSchema)
    });
    const onSubmit = handleSubmit(data => {
        addCategory(new Category(0, data.name));
        store.commonStore.navigation!('/category');
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
                                <Button type="submit" variant="outlined" >Ввод</Button>
                            </Grid2>
                        </Grid2>
                    </Grid2>
                </Container>
            </form>
        </ThemeProvider>
    )
}