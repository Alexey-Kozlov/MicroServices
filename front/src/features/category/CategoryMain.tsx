import { Button, Container, Paper, Stack, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@mui/material";
import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { useStore } from "../../app/stores/store";

export default observer(function CategoryMain() {
    const { categoryStore: { getCategoryList, categoryRegistry } } = useStore();

    useEffect(() => {
        getCategoryList();
    }, [getCategoryList]);

    return (
        <>
            <Container maxWidth="md" sx={{ marginTop: "50px" }}>
                <Stack alignItems="center">
                    <h2>Категории</h2>
                </Stack>
                <Stack alignItems="flex-end">
                    <Button variant="outlined" component="a" href="/addCategory">Добавить</Button>
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
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>
            </Container>
        </>
    )
})