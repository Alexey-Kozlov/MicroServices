import { MenuItem, TextField } from "@mui/material";
import { Button, Container, FormControl, InputLabel, Paper, Select, SelectChangeEvent, Stack, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@mui/material";
import { observer } from "mobx-react-lite";
import { ChangeEvent, ChangeEventHandler } from "react";
import { useEffect } from "react";
import EditButtons from "../../app/components/EditButtons";
import { IOrder } from "../../app/models/iorder";
import { ProductItems } from "../../app/models/iproductItems";
import { useStore } from "../../app/stores/store";

interface prop {
    order?: IOrder;
}
export default observer(function ProductList({ order }: prop) {
    const { productStore: { getProductItems, productItems, addUpdateProductItem, getProducts,
        productRegistry, deleteProductItem } } = useStore();

    useEffect(() => {
        getProducts().then(() => {
            if (order) {
                getProductItems(order);
            }
        });
    }, [order]);

    const addProductItem = () => {
        addUpdateProductItem();
    };
    const handleProductSelectChange = (event: SelectChangeEvent, id: number) => {
        const item = productItems.get(id);
        addUpdateProductItem(new ProductItems(id, Number.parseInt(event.target.value), item!.quantity));
    };
    const handleProductQuantChange = (event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>, id: number) => {
        const item = productItems.get(id);
        addUpdateProductItem(new ProductItems(id, item!.productId, Number.parseInt(event.target.value)));

    };

    const handleDeleteButton = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>, id: number) => {
        deleteProductItem(id);
    }
    return (
        <Container sx={{ marginTop: "50px", width:"600px" }}>
                <>
                    <Stack alignItems="center">
                        <h2>Позиции</h2>
                    </Stack>
                    <Stack alignItems="flex-end">
                    <Button
                        variant="outlined"
                        onClick={() => { addProductItem() }}
                    >Добавить</Button>
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
                                    <TableCell>
                                            <FormControl size="small" variant="standard">
                                                <InputLabel id={"selector" + item.id}>Продукт</InputLabel>
                                                <Select
                                                    labelId={"selector" + item.id}
                                                    label="Продукт"
                                                    value={item.productId.toString()}
                                                    onChange={(e) => handleProductSelectChange(e , item.id)}
                                                >
                                                    {
                                                        Array.from(productRegistry.values()).map(prod => (
                                                            <MenuItem
                                                                key={item.id + '_' + prod.id}
                                                                value={prod.id}>{prod.name}</MenuItem>
                                                        ))
                                                    }
                                                </Select>
                                            </FormControl>
                                    </TableCell>
                                    <TableCell>
                                            <TextField variant="standard"
                                                type="number"
                                                label="Количество"
                                                InputLabelProps={{ shrink: true }}
                                                value={item.quantity}
                                                onChange={(e) => handleProductQuantChange(e, item.id)}
                                            />
                                    </TableCell>
                                        <TableCell>
                                            <EditButtons showEdit={ false }
                                                onClickDeleteButton={(e) => handleDeleteButton(e, item.id)}
                                            />
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