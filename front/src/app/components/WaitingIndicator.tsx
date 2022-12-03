import { Box, CircularProgress, Stack } from "@mui/material";

interface WI {
    text: string
}

const waitStyle = {
    position: "absolute",
    top: "30%",
    textAlign: "center",
    left: "50%",
    transform: "translateX(-50%)"
}

export default function WaitingIndicator({text }: WI) {
    return (
        <Box sx={waitStyle}>
            <h4>{text}</h4>
            <CircularProgress />
        </Box>
    )
}