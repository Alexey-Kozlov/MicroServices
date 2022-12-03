import { Button, CircularProgress, Container, TextField, ThemeProvider, Typography } from "@mui/material";
import Grid2 from "@mui/material/Unstable_Grid2";
import { Controller, useForm } from "react-hook-form";
import InputTheme from "../../app/themes/InputTheme";
import { yupResolver } from '@hookform/resolvers/yup';
import * as Yup from 'yup';
import HeaderTheme from "../header/headerTheme";
import { store, useStore } from "../../app/stores/store";
import { IProduct, Product } from "../../app/models/iproduct";
import { useEffect } from "react";
import { useParams } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { observe } from "mobx";
import WaitingIndicator from "../../app/components/WaitingIndicator";
import { grey } from "@mui/material/colors";

export default observer(function ProductForm() {
    const validSchema = Yup.object().shape({
        name: Yup.string().required('Необходимо указать наименование!'),
        price: Yup.number().required('Необходимо указать цену!')
            .typeError('Необходимо указать цену!')
            .moreThan(0, 'Необходимо указать цену больше 0!'),
        categoryId: Yup.number().required('Необходимо указать категорию!')
            .typeError('Необходимо указать категорию!')
            .moreThan(0, 'Необходимо указать категорию больше 0!'),
        imageId: Yup.number().required('Необходимо указать изображение!')
            .typeError('Необходимо указать изображение!')
            .moreThan(0, 'Необходимо указать изображение больше 0!')
    });
    const { productStore } = useStore();
    const { addEditProduct, getProduct, isLoading, isSubmitted } = productStore;

    const { handleSubmit, control, formState: { errors }, reset } = useForm<IProduct>({
        defaultValues: new Product(0, "", 0, "", 0, 0),
        resolver: yupResolver(validSchema)
    });

    const onSubmit = handleSubmit(data => {
        if (!id) {
            id = "0";
        } 
        addEditProduct(new Product(Number(id), data.name, data.price, data.description, data.categoryId, data.imageId))
            .then(() =>{
                store.commonStore.navigation!('/products');
            });
    });
    const labelStyle = {
        width: "130px"
    }

    let { id } = useParams<{ id: string }>();
  
    //Вариант 1 - отлавливаем изменения через observer - обновляем форму редактирования, когда готовы данные
    //Здесь проблема - могут быть ложные срабатывания
   //const disposer = observe(productStore!, "selectedProduct", (change: any) => {
   //    if (change.newValue) {
   //        //отменяем лишние срабатывания
   //        disposer();
   //        //обновляем данные в форме редактирования
   //        reset(change.newValue);
   //    }
   // });

    //Вариант 2 - работаем с возвращенным результатом, эмуляция анонимной асинхронной функции
    //useEffect(() => {
    //    if (id) {
    //        (async () => {
    //            const item = await getProduct(id!);
    //            reset(item);
    //        })();
    //    }
    //}, [id, getProduct, reset]);

    //Вариант 3 - обычный вариант с промисом
    useEffect(() => {
        if (id) {
            getProduct(id!).then((item) => {
                reset(item);
            });
        }
    }, [id, getProduct, reset]);

    return (
        <ThemeProvider theme={InputTheme}>
        <form onSubmit={onSubmit}>
                <Container maxWidth="lg">
                    {isLoading && !isSubmitted && id ? (<WaitingIndicator text="Загрузка..." />) : (
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
                            <Grid2 container direction="row" justifyContent="center" alignItems="center">
                                <Grid2 style={labelStyle}>
                                    <label>Цена</label>
                                </Grid2>
                                <Grid2 xs={6}>
                                    <Controller
                                        name="price"
                                        control={control}
                                        render={({ field }) =>
                                            <>
                                                <TextField {...field}
                                                    variant="outlined"
                                                    type="number"
                                                    label="Цена"
                                                    error={errors.price ? true : false} />
                                                <Typography variant="inherit" sx={HeaderTheme.typography.errorMessage}>
                                                    {errors.price?.message}
                                                </Typography>
                                            </>
                                        }
                                    />
                                </Grid2>
                            </Grid2>
                            <Grid2 container direction="row" justifyContent="center" alignItems="center">
                                <Grid2 style={labelStyle}>
                                    <label>Категория</label>
                                </Grid2>
                                <Grid2 xs={6}>
                                    <Controller
                                        name="categoryId"
                                        control={control}
                                        render={({ field }) =>
                                            <>
                                                <TextField {...field}
                                                    variant="outlined"
                                                    type="number"
                                                    label="Категория"
                                                    error={errors.categoryId ? true : false} />
                                                <Typography variant="inherit" sx={HeaderTheme.typography.errorMessage}>
                                                    {errors.categoryId?.message}
                                                </Typography>
                                            </>
                                        }
                                    />
                                </Grid2>
                            </Grid2>
                            <Grid2 container direction="row" justifyContent="center" alignItems="center">
                                <Grid2 style={labelStyle}>
                                    <label>Изображение</label>
                                </Grid2>
                                <Grid2 xs={6}>
                                    <Controller
                                        name="imageId"
                                        control={control}
                                        render={({ field }) =>
                                            <>
                                                <TextField {...field}
                                                    variant="outlined"
                                                    type="number"
                                                    label="Изображение"
                                                    error={errors.imageId ? true : false} />
                                                <Typography variant="inherit" sx={HeaderTheme.typography.errorMessage}>
                                                    {errors.imageId?.message}
                                                </Typography>
                                            </>
                                        }
                                    />
                                </Grid2>
                            </Grid2>
                            <Grid2 container direction="row" justifyContent="center" alignItems="center">
                                <Grid2 style={labelStyle}>
                                    <label>Примечание</label>
                                </Grid2>
                                <Grid2 xs={6}>
                                    <Controller name="description" control={control}
                                        render={({ field }) =>
                                            <TextField {...field} variant="outlined"
                                                label="Примечание" />
                                        }
                                    />
                                </Grid2>
                            </Grid2>
                            <Grid2 container direction="row" justifyContent="center">
                                <Grid2>
                                    <Button variant="outlined" sx={{ marginRight: "10px" }}
                                        onClick={() => store.commonStore.navigation!('/products')}>
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
                    )}
            </Container>
            </form>
        </ThemeProvider>
    )
})