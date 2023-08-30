import { Button, Container, Paper, Stack, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@mui/material";
import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import ConfirmDialog from "../../app/components/ConfirmDialog";
import EditButtons from "../../app/components/EditButtons";
import { useStore } from "../../app/stores/store";

export default observer(function CategoryMain() {
    const { categoryStore: { getCategoryList, categoryRegistry, deleteCategory } } = useStore();
    const navigate = useNavigate();
    const confirmObject = {
        text: "",
        id: 0
    }
    const [confirmObj, setConfirmObj] = useState<typeof confirmObject>(confirmObject);
    const [showConfirm, setShowConfirm] = useState(false);
    useEffect(() => {
        getCategoryList();
    }, [getCategoryList]);

    const handleEditButton = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>, id: number) => {
        navigate(`/category/${id}`);
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
            deleteCategory(confirmObj!.id);
        }
    };
    const addCategoryLink = "/addCategory";

    return (
        <>
            <Container maxWidth="md" sx={{ marginTop: "50px" }}>
                <Stack alignItems="center">
                    <h2>Категории</h2>
                </Stack>
                <Stack alignItems="flex-end">
                    <Button variant="outlined" component="a" href={ addCategoryLink } >Добавить</Button>
                </Stack>
                <TableContainer component={Paper}>
                    <Table sx={{ minWidth: 650 }}  >
                        <TableHead>
                            <TableRow sx={{
                                "& th": { fontWeight: "bold" }
                            }}>
                                <TableCell >ID</TableCell>
                                <TableCell >Наименование</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {Array.from(categoryRegistry.values()).map(category => (
                                <TableRow key={category.id} sx={{
                                    ":nth-of-type(odd)": { backgroundColor: "#F1F1F1" }
                                }}>
                                    <TableCell>{category.id}</TableCell>
                                    <TableCell>{category.name}</TableCell>
                                    <TableCell>
                                        <EditButtons
                                            onClickEditButton={(e) => handleEditButton(e, category.id)}
                                            onClickDeleteButton={(e) => handleDeleteButton(e, category.id, category.name)}
                                        />
                                    </TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>
            </Container>
            <ConfirmDialog id="deleteItemConfirm" mainText={confirmObj!.text}
                titleText="Подтверждение удаления записи"
                open={showConfirm} onClose={handleConfirmClose} />
        </>
    )
})