export default function important<T>(value: T): T {
    return (value + ' !important') as any;
}