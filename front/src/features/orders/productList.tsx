import { Button, Container, Paper, Stack, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@mui/material";
import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { IOrder } from "../../app/models/iorder";
import { useStore } from "../../app/stores/store";

interface prop {
    order: IOrder;
}
export default observer(function ProductList({ order }: prop) {
    const { productStore: { getProductItems, productItems } } = useStore();
    useEffect(() => {
        getProductItems(order);
    }, [getProductItems ]);
    return (
        <Container maxWidth="md" sx={{ marginTop: "50px" }}>
                <>
                    <Stack alignItems="center">
                        <h2>Позиции</h2>
                    </Stack>
                    <Stack alignItems="flex-end">
                        <Button variant="outlined" component="a" href="/order">Добавить</Button>
                    </Stack>
                    <TableContainer component={Paper}>
                        <Table  >
                            <TableHead>
                                <TableRow sx={{
                                    "& th": { fontWeight: "bold" }
                                }}>
                                    <TableCell>ID</TableCell>
                                    <TableCell>Наименование</TableCell>
                                    <TableCell>Количество</TableCell>
                                    <TableCell></TableCell>
                                </TableRow>
                            </TableHead>
                        <TableBody>
                            {
                                Array.from(productItems.values()).map(item => (
                                    <TableRow key={item.id} sx={{
                                        ":nth-of-type(odd)": { backgroundColor: "#F1F1F1" }
                                    }}>
                                    <TableCell>{item.id}</TableCell>
                                    <TableCell>{item.name}</TableCell>
                                    <TableCell>{item.quantity}</TableCell>
                                        <TableCell>

                                        </TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    </TableContainer>
                </>

        </Container>
    )
})