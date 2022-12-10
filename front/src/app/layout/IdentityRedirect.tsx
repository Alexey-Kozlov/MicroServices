import { useEffect } from "react";

export default function IdentityRedirect() {
    useEffect(() => {
        if (!window.localStorage.getItem('jwt')) {
            window.location.href = process.env.REACT_APP_IDENTITY! + '/login';
        }
    });
    return (
        <br></br>
    )
}