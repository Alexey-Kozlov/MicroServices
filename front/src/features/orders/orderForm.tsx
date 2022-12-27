import { Button, CircularProgress, Container, TextField, ThemeProvider, Typography } from "@mui/material";
import Grid2 from "@mui/material/Unstable_Grid2";
import { Controller, useForm } from "react-hook-form";
import InputTheme from "../../app/themes/InputTheme";
import { yupResolver } from '@hookform/resolvers/yup';
import * as Yup from 'yup';
import HeaderTheme from "../header/headerTheme";
import { useStore } from "../../app/stores/store";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { observer } from "mobx-react-lite";
import WaitingIndicator from "../../app/components/WaitingIndicator";
import { grey } from "@mui/material/colors";
import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import 'dayjs/locale/ru';
import { Unstable_DateField as DateField } from '@mui/x-date-pickers/DateField';
import { IOrder, Order } from "../../app/models/iorder";
import ProductList from "./productList";

export default observer(function OrderForm() {
    const navigate = useNavigate();
    const validSchema = Yup.object().shape({
        orderDate: Yup.date().required('Необходимо указать дату!'),
        userId: Yup.number().required('Необходимо выбрать пользователя!'),
        productId: Yup.number().required('Необходимо указать продукт')
    });
    const { orderStore: { addEditOrder, getOrder, isLoading, isSubmitted } } = useStore();
    const [order,setOrder] = useState<IOrder>();
    const { handleSubmit, control, formState: { errors }, reset } = useForm<IOrder>({
        defaultValues: new Order(0, new Date(),"","",[]),
        resolver: yupResolver(validSchema)
    });

    const onSubmit = handleSubmit(data => {
        if (!id) {
            id = "0";
        }
        addEditOrder(new Order(Number(id), data.orderDate, data.userId, data.description, data.products))
            .then(() =>{
                navigate('/orders');
            });
    });
    const labelStyle = {
        width: "130px"
    }

    let { id } = useParams<{ id: string }>();
  
    useEffect(() => {
        if (id) {
            getOrder(id!).then((item) => {
                if (item) {
                    reset(item);
                    setOrder(item);

                } 
            });
        }
    }, [id, getOrder, reset]);

    return (
        <ThemeProvider theme={InputTheme}>
        <form onSubmit={onSubmit}>
                <Container maxWidth="lg">
                    {isLoading && !isSubmitted && id ? (<WaitingIndicator text="Загрузка..." />) : (
                        <Grid2 container direction="column" rowSpacing={1} columnSpacing={1}>
                            <Grid2 container direction="row" justifyContent="center" alignItems="center">
                                <Grid2 style={labelStyle} >
                                    <label>Дата заказа</label>
                                </Grid2>
                                <Grid2 xs={6}>
                                <Controller
                                    name="orderDate"
                                    control={control}
                                    render={({ field }) => (                                           
                                        <LocalizationProvider dateAdapter={AdapterDayjs}
                                            adapterLocale='ru'
                                          
                                        >
                                       <DateField
                                            placeholder='Дата заказа'
                                            variant="outlined"
                                                label="Дата заказа"
                                                value={field.value}                                          
                                            error={errors.orderDate ? true : false}
                                        />
                                                    <Typography variant="inherit" sx={HeaderTheme.typography.errorMessage}>
                                                        {errors.orderDate?.message}
                                                    </Typography>
                                    </LocalizationProvider>
                                        )}
                                    />
                                </Grid2>
                            </Grid2>

                            <Grid2 container direction="row" justifyContent="center" alignItems="center">
                                <Grid2 style={labelStyle}>
                                    <label>Примечание</label>
                                </Grid2>
                                <Grid2 xs={6}>
                                    <Controller
                                        name="description"
                                        control={control}
                                        render={({ field }) =>
                                            <TextField {...field}
                                                variant="outlined"
                                                label="Примечание" />
                                        }
                                    />
                                </Grid2>
                            </Grid2>
                            <Grid2 container direction="row" justifyContent="center" alignItems="center">
                                <Grid2 xs={6}>
                                    {order &&
                                        <ProductList order={order!} />
                                    }
                                </Grid2>
                            </Grid2>
                            <Grid2 container direction="row" justifyContent="center">
                                <Grid2>
                                    <Button variant="outlined" sx={{ marginRight: "10px" }}
                                        onClick={() => navigate('/orders')}>
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