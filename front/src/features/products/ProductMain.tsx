import { observer } from "mobx-react-lite";
import { Button, Container, Paper, Stack, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@mui/material";
import { useEffect, useState } from "react";
import { useStore } from "../../app/stores/store";
import EditButtons from "../../app/components/EditButtons";
import ConfirmDialog from "../../app/components/ConfirmDialog";
import WaitingIndicator from "../../app/components/WaitingIndicator";
import { useNavigate } from "react-router-dom";

export default observer(function ProductMain() {
    const { productStore: { productRegistry, getProducts, deleteProduct, isLoading } } = useStore();
    const navigate = useNavigate();
    const confirmObject = {
        text: "",
        id: 0
    }

    useEffect(() => {
        getProducts();
    }, [getProducts]);
    const [showConfirm, setShowConfirm] = useState(false);
    const [confirmObj, setConfirmObj] = useState<typeof confirmObject>(confirmObject);

    const handleEditButton = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>, id: number) => {
        navigate(`/product/${id}`);
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
            deleteProduct(confirmObj!.id);
        }        
    };

    return (
        <>
            <Container maxWidth="md" sx={{ marginTop: "50px" }}>
                {isLoading ? (<WaitingIndicator text="Загрузка..." />) : (
                    <>
                        <Stack alignItems="center">
                            <h2>Продукты</h2>
                        </Stack>
                        <Stack alignItems="flex-end">
                            <Button variant="outlined" component="a" href="/product">Добавить</Button>
                        </Stack>
                        <TableContainer component={Paper}>
                            <Table sx={{ minWidth: 650 }}  >
                                <TableHead>
                                    <TableRow sx={{
                                        "& th": { fontWeight: "bold" }
                                    }}>
                                        <TableCell >ID</TableCell>
                                        <TableCell >Наименование</TableCell>
                                        <TableCell >Цена</TableCell>
                                        <TableCell >Категория</TableCell>
                                        <TableCell >Изображение</TableCell>
                                        <TableCell >Примечание</TableCell>
                                        <TableCell ></TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {Array.from(productRegistry.values()).map(product => (
                                        <TableRow key={product.id} sx={{
                                            ":nth-of-type(odd)": { backgroundColor: "#F1F1F1" }
                                        }}>
                                            <TableCell>{product.id}</TableCell>
                                            <TableCell>{product.name}</TableCell>
                                            <TableCell>{product.price}</TableCell>
                                            <TableCell>{product.categoryId}</TableCell>
                                            <TableCell>{product.imageId}</TableCell>
                                            <TableCell>{product.description}</TableCell>
                                            <TableCell>
                                                <EditButtons
                                                    onClickEditButton={(e) => handleEditButton(e, product.id)}
                                                    onClickDeleteButton={(e) => handleDeleteButton(e, product.id, product.name)}
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