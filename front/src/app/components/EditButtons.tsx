import { Button } from "@mui/material";
import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';
import { grey } from "@mui/material/colors";

interface IEditButtons {
    onClickEditButton?: (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
    onClickDeleteButton?: (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
    showEdit?: boolean;
    showDelete?: boolean;
}

export default function EditButtons({ onClickDeleteButton, onClickEditButton,
    showDelete = true, showEdit = true }: IEditButtons) {

    const buttonsStyle = {
        minWidth: "15px",
        padding: "0",
        margin: "0 5px 0 0",
        backgroundColor: "white",
        ".MuiSvgIcon-root:hover": {
            color: "white",
            backgroundColor: grey[500]
        },
        ".MuiButton-startIcon": {
            margin: "0"
        },

    }

    const iconStyle = {
        padding: "8px",
        margin: "0"
    }
    return (
        <>
            <Button variant="contained" sx={buttonsStyle}
                startIcon={<EditIcon color="info" sx={iconStyle} />}
                style={showEdit !== true ? { display: "none" } : {}}
                onClick={(e) => onClickEditButton ? onClickEditButton(e) : undefined}                
            />
            <Button variant="contained" sx={buttonsStyle}
                startIcon={<DeleteIcon color="warning" sx={iconStyle} />}
                style={showDelete !== true ? { display: "none" } : {}}
                onClick={(e) => onClickDeleteButton ? onClickDeleteButton(e) : undefined}
            />
        </>
    )
}