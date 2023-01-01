import { observer } from "mobx-react-lite";
import { Button, Container, Paper, Stack, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@mui/material";
import { useEffect, useState } from "react";
import { useStore } from "../../app/stores/store";
import EditButtons from "../../app/components/EditButtons";
import ConfirmDialog from "../../app/components/ConfirmDialog";
import WaitingIndicator from "../../app/components/WaitingIndicator";
import { Link, useNavigate } from "react-router-dom";
import dayjs from "dayjs";

export default observer(function OrdersMain() {
    const { orderStore: { ordersRegistry, getOrders, deleteOrder, isLoading } } = useStore();
    const navigate = useNavigate();
    const confirmObject = {
        text: "",
        id: 0
    }

    useEffect(() => {
        getOrders();
    }, [getOrders]);
    const [showConfirm, setShowConfirm] = useState(false);
    const [confirmObj, setConfirmObj] = useState<typeof confirmObject>(confirmObject);

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
                                        <TableCell >ID</TableCell>
                                        <TableCell >Дата</TableCell>
                                        <TableCell >Количество позиций</TableCell>
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
            </Container>
            <ConfirmDialog id="deleteItemConfirm" mainText={confirmObj!.text}
                titleText="Подтверждение удаления записи"
                open={showConfirm} onClose={handleConfirmClose} />
        </>
    )
})