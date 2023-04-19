import { round as round_1 } from "../../fable_modules/fable-library.4.0.6/Util.js";

export function log2(x) {
    return Math.log(x) / Math.log(2);
}

export function revLog2(x) {
    return Math.pow(2, x);
}

export function arsinh(x) {
    let x_1;
    const value = x + Math.sqrt(((x_1 = x, x_1 * x_1)) + 1);
    return Math.log(value);
}

export function round(digits, x) {
    return round_1(x, digits);
}

