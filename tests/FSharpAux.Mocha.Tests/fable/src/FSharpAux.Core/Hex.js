import { fill } from "../../fable_modules/fable-library.4.0.6/Array.js";
import { concat } from "../../fable_modules/fable-library.4.0.6/String.js";
import { defaultOf } from "../../fable_modules/fable-library.4.0.6/Util.js";
import { Operators_NullArg } from "../../fable_modules/fable-library.4.0.6/FSharp.Core.js";

export function ToHexDigit(n) {
    if (n < 10) {
        return String.fromCharCode(n + 48);
    }
    else {
        return String.fromCharCode(n + 55);
    }
}

export function FromHexDigit(c) {
    if ((c >= "0") && (c <= "9")) {
        return (c.charCodeAt(0) - 48) | 0;
    }
    else if ((c >= "A") && (c <= "F")) {
        return ((c.charCodeAt(0) - 65) + 10) | 0;
    }
    else if ((c >= "a") && (c <= "f")) {
        return ((c.charCodeAt(0) - 97) + 10) | 0;
    }
    else {
        throw new Error();
    }
}

export function Encode(prefix, color) {
    const hex = fill(new Array(color.length * 2), 0, color.length * 2, "");
    let n = 0;
    for (let i = 0; i <= (color.length - 1); i++) {
        hex[n] = ToHexDigit((~~color[i] & 240) >> 4);
        n = ((n + 1) | 0);
        hex[n] = ToHexDigit(~~color[i] & 15);
        n = ((n + 1) | 0);
    }
    return concat(prefix, ...(hex.join('')));
}

export function Decode(s) {
    if (s === defaultOf()) {
        return Operators_NullArg("s");
    }
    else if (s.length === 0) {
        return new Uint8Array(0);
    }
    else {
        let len = s.length;
        let i = 0;
        if (((len >= 2) && (s[0] === "0")) && ((s[1] === "x") ? true : (s[1] === "X"))) {
            len = ((len - 2) | 0);
            i = ((i + 2) | 0);
        }
        if ((len % 2) !== 0) {
            throw new Error("Invalid hex format\\nParameter name: s");
        }
        else {
            const buf = new Uint8Array(~~(len / 2));
            let n = 0;
            while (i < s.length) {
                buf[n] = (((FromHexDigit(s[i]) << 4) | FromHexDigit(s[i + 1])) & 0xFF);
                i = ((i + 2) | 0);
                n = ((n + 1) | 0);
            }
            return buf;
        }
    }
}

