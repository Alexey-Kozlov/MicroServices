import { observer } from "mobx-react-lite";
import { Box, Button, Container, Pagination, Paper, Stack, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, TableSortLabel } from "@mui/material";
import React, { useEffect, useState } from "react";
import { useStore } from "../../app/stores/store";
import EditButtons from "../../app/components/EditButtons";
import ConfirmDialog from "../../app/components/ConfirmDialog";
import WaitingIndicator from "../../app/components/WaitingIndicator";
import { Link, useNavigate } from "react-router-dom";
import dayjs from "dayjs";
import PagingParams from "../../app/models/pagingParams";
import { visuallyHidden } from '@mui/utils';

export default observer(function OrdersMain() {
    const { orderStore: { ordersRegistry, getOrders, deleteOrder, isLoading, setPredicate, predicate },
        paging: { pagination, setPagingParams } } = useStore();
    const navigate = useNavigate();
    const confirmObject = {
        text: "",
        id: 0
    }
    
    useEffect(() => {
        if (predicate.get('sort')) {
            const orderParam = predicate.get('sort')?.split(',');
            setOrderObject({ field: orderParam![0], order: orderParam![1] });
        }
        getOrders();
    }, [getOrders]);
    const [showConfirm, setShowConfirm] = useState(false);
    const [confirmObj, setConfirmObj] = useState<typeof confirmObject>(confirmObject);
    const [orderObject, setOrderObject] = useState<{ field: string, order: string }>({field:'',order:''});
    const [currentPageNumber, setCurrentPageNumber] = useState<number>(1);

    const handleEditButton = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>, id: number) => {
        navigate(`/order/${id}`);
    }
    const handleDeleteButton = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>, id: number, name: string) => {
        confirmObject.id = id;
        confirmObject.text = "Действительно удалить '" + name + "'?";
        setConfirmObj(confirmObject);
        setShowConfirm(true);
             
    }
    const handleConfirmClose = (confirm: boolean) => {
        setShowConfirm(false);
        if (confirm) {
            deleteOrder(confirmObj!.id);
        }        
    };

    const handlePageChange = (e: React.ChangeEvent<unknown>, page: number) => {
        setPagingParams(new PagingParams(page));
        setCurrentPageNumber(page);
        getOrders();
    }

    
    const handleSortClick = (ev: React.MouseEvent<HTMLSpanElement, MouseEvent>, field: string) => {
        setPagingParams(new PagingParams(1));
        orderObject.order === 'desc' ?
            setOrderObject({ field: field, order: 'asc' }) :
            setOrderObject({ field: field, order: 'desc' });
        setPredicate('sort', field + ',' + (orderObject.order === 'desc' ? 'asc' : 'desc'));          
        setCurrentPageNumber(1);
    };

    return (
        <>
            <Container maxWidth="md" sx={{ marginTop: "50px" }}>
                {isLoading ? (<WaitingIndicator text="Загрузка..." />) : (
                    <>
                        <Stack alignItems="center">
                            <h2>Заказы</h2>
                        </Stack>
                        <Stack alignItems="flex-end">
                            <Button variant="outlined" component="a" href="/order">Добавить</Button>
                        </Stack>
                        <TableContainer component={Paper}>
                            <Table sx={{ minWidth: 650 }}  >
                                <TableHead>
                                    <TableRow sx={{
                                        "& th": { fontWeight: "bold" }
                                    }}>
                                        <TableCell>
                                            <TableSortLabel
                                                active={orderObject?.field === 'Id'}
                                                direction={orderObject!.order !== 'asc' ? 'asc' : 'desc'}
                                                onClick={e => handleSortClick(e,'Id')}
                                            >ID</TableSortLabel>
                                        </TableCell>
                                        <TableCell>
                                            <TableSortLabel
                                                active={orderObject?.field === 'OrderDate'}
                                                direction={orderObject!.order !== 'asc' ? 'asc' : 'desc'}
                                                onClick={e => handleSortClick(e, 'OrderDate')}
                                            >Дата</TableSortLabel>
                                        </TableCell>
                                        <TableCell>
                                            <TableSortLabel
                                                active={orderObject?.field === 'Products'}
                                                direction={orderObject!.order !== 'asc' ? 'asc' : 'desc'}
                                                onClick={e => handleSortClick(e, 'Products')}
                                            >Количество позиций</TableSortLabel>
                                        </TableCell>
                                        <TableCell >Примечание</TableCell>
                                        <TableCell ></TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {Array.from(ordersRegistry.values()).map(order => (
                                        <TableRow key={order.id} sx={{
                                            ":nth-of-type(odd)": { backgroundColor: "#F1F1F1" }
                                        }}>
                                            <TableCell>{order.id}</TableCell>
                                            <TableCell>{dayjs(order.orderDate).locale('ru').format('DD.MM.YYYY')}</TableCell>
                                            <TableCell>{order.products.length.toString()}</TableCell>
                                            <TableCell>{order.description}</TableCell>
                                            <TableCell>
                                                <EditButtons
                                                    onClickEditButton={(e) => handleEditButton(e, order.id)}
                                                    onClickDeleteButton={(e) => handleDeleteButton(e, order.id, order.id.toString())}
                                                />
                                            </TableCell>
                                        </TableRow>
                                    ))}
                                </TableBody>
                            </Table>
                        </TableContainer>
                    </>
                )}
                {pagination?.totalPages && pagination?.totalPages > 1 &&
                    <Stack alignItems='end' sx={{ marginTop: "20px" }}>
                        <Pagination
                            count={pagination?.totalPages || 0}
                            showFirstButton
                            showLastButton
                            onChange={handlePageChange}
                            page={currentPageNumber}
                        />
                    </Stack>
                }
            </Container>
            <ConfirmDialog id="deleteItemConfirm" mainText={confirmObj!.text}
                titleText="Подтверждение удаления записи"
                open={showConfirm} onClose={handleConfirmClose} />
        </>
    )
})