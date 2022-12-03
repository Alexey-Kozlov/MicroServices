import Button from '@mui/material/Button';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import Dialog from '@mui/material/Dialog';
import { FormLabel } from '@mui/material';

export interface ConfirmationDialogRawProps {
    id: string;
    open: boolean;
    onClose: (value: boolean) => void;
    titleText: string;
    mainText: string;
}

export default function ConfirmDialog(props: ConfirmationDialogRawProps) {
    const { onClose, open, titleText, mainText, ...other } = props;

    const handleCancel = () => {
        onClose(false);
    };

    const handleOk = () => {
        onClose(true);
    };

    return (
         <Dialog
      sx={{ '& .MuiDialog-paper': { width: '80%', maxHeight: 435 } }}
      maxWidth="xs"
      open={open}
      {...other}
        >
            <DialogTitle sx={{ textAlign:"center" }}>{titleText}</DialogTitle>
      <DialogContent dividers>
                <FormLabel>{mainText}</FormLabel>
      </DialogContent>
      <DialogActions>
        <Button autoFocus onClick={handleCancel}>
          Отмена
        </Button>
        <Button onClick={handleOk}>Да</Button>
      </DialogActions>
    </Dialog>
    )
}