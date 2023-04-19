import { Record, Union } from "../../fable_modules/fable-library.4.0.6/Types.js";
import { record_type, union_type, uint8_type } from "../../fable_modules/fable-library.4.0.6/Reflection.js";
import { defaultOf, round, min as min_1, compare, max } from "../../fable_modules/fable-library.4.0.6/Util.js";
import { toText, trimStart, printf, toFail } from "../../fable_modules/fable-library.4.0.6/String.js";
import { Decode, Encode } from "./Hex.js";
import { equalsWith } from "../../fable_modules/fable-library.4.0.6/Array.js";

export class ColorComponent extends Union {
    constructor(tag, fields) {
        super();
        this.tag = tag;
        this.fields = fields;
    }
    cases() {
        return ["A", "R", "G", "B"];
    }
}

export function ColorComponent$reflection() {
    return union_type("FSharpAux.Colors.ColorComponent", [], ColorComponent, () => [[["Item", uint8_type]], [["Item", uint8_type]], [["Item", uint8_type]], [["Item", uint8_type]]]);
}

export function getValueFromCC(cc) {
    switch (cc.tag) {
        case 1:
            return cc.fields[0];
        case 2:
            return cc.fields[0];
        case 3:
            return cc.fields[0];
        default:
            return cc.fields[0];
    }
}

export class Color extends Record {
    constructor(A, R, G, B) {
        super();
        this.A = A;
        this.R = R;
        this.G = G;
        this.B = B;
    }
}

export function Color$reflection() {
    return record_type("FSharpAux.Colors.Color", [], Color, () => [["A", uint8_type], ["R", uint8_type], ["G", uint8_type], ["B", uint8_type]]);
}

export function maxRGB(c) {
    return max(compare, new ColorComponent(3, [c.B]), max(compare, new ColorComponent(1, [c.R]), new ColorComponent(2, [c.G])));
}

export function minRGB(c) {
    return min_1(compare, new ColorComponent(3, [c.B]), min_1(compare, new ColorComponent(1, [c.R]), new ColorComponent(2, [c.G])));
}

export function fromArgb(a, r, g, b) {
    const f = (v) => {
        if ((v < 0) ? true : (v > 255)) {
            return toFail(printf("Value for component needs to be between 0 and 255."));
        }
        else {
            return v & 0xFF;
        }
    };
    return new Color(f(a), f(r), f(g), f(b));
}

export function fromRgb(r, g, b) {
    return fromArgb(255, r, g, b);
}

export function getHue(c) {
    const min = getValueFromCC(minRGB(c));
    const matchValue = maxRGB(c);
    switch (matchValue.tag) {
        case 1:
            return (c.G - c.B) / (matchValue.fields[0] - min);
        case 2:
            return 2 + ((c.B - c.R) / (matchValue.fields[0] - min));
        case 3:
            return 4 + ((c.R - c.G) / (matchValue.fields[0] - min));
        default:
            return toFail(printf(""));
    }
}

export function getSaturation(col) {
    const minimum = minRGB(col);
    const maximum = maxRGB(col);
    return round((getValueFromCC(minimum) + getValueFromCC(maximum)) / 2);
}

export function toArgb(c) {
    return [~~c.A, ~~c.R, ~~c.G, ~~c.B];
}

export function toHex(prefix, c) {
    return Encode(prefix ? "0x" : "", new Uint8Array([c.R, c.G, c.B]));
}

export function fromHex(s) {
    const matchValue = Decode(s);
    if (!equalsWith((x, y) => (x === y), matchValue, defaultOf()) && (matchValue.length === 3)) {
        return fromRgb(~~matchValue[0], ~~matchValue[1], ~~matchValue[2]);
    }
    else {
        return toFail(printf("Invalid hex color format"));
    }
}

export function toWebColor(c) {
    return Encode("#", new Uint8Array([c.R, c.G, c.B]));
}

export function fromWebColor(s) {
    const matchValue = Decode(trimStart(s, "#"));
    if (!equalsWith((x, y) => (x === y), matchValue, defaultOf()) && (matchValue.length === 3)) {
        return fromRgb(~~matchValue[0], ~~matchValue[1], ~~matchValue[2]);
    }
    else {
        return toFail(printf("Invalid hex color format"));
    }
}

export function toString(c) {
    const patternInput = toArgb(c);
    return toText(printf("{Alpha: %i Red: %i Green: %i Blue: %i}"))(patternInput[0])(patternInput[1])(patternInput[2])(patternInput[3]);
}

export const Table_black = fromRgb(0, 0, 0);

export const Table_blackLite = fromRgb(89, 89, 89);

export const Table_white = fromRgb(255, 255, 255);

export const Table_Office_blue = fromRgb(65, 113, 156);

export const Table_Office_lightBlue = fromRgb(189, 215, 238);

export const Table_Office_darkBlue = fromRgb(68, 114, 196);

export const Table_Office_red = fromRgb(241, 90, 96);

export const Table_Office_lightRed = fromRgb(252, 212, 214);

export const Table_Office_orange = fromRgb(237, 125, 49);

export const Table_Office_lightOrange = fromRgb(248, 203, 173);

export const Table_Office_yellow = fromRgb(255, 217, 102);

export const Table_Office_lightYellow = fromRgb(255, 230, 153);

export const Table_Office_darkYellow = fromRgb(255, 192, 0);

export const Table_Office_green = fromRgb(122, 195, 106);

export const Table_Office_lightGreen = fromRgb(197, 224, 180);

export const Table_Office_darkGreen = fromRgb(112, 173, 71);

export const Table_Office_grey = fromRgb(165, 165, 165);

export const Table_Office_lightGrey = fromRgb(217, 217, 217);

export const Table_StatisticalGraphics24_Blue1 = fromRgb(2, 63, 165);

export const Table_StatisticalGraphics24_Blue2 = fromRgb(125, 135, 185);

export const Table_StatisticalGraphics24_Blue3 = fromRgb(190, 193, 212);

export const Table_StatisticalGraphics24_Red1 = fromRgb(214, 188, 192);

export const Table_StatisticalGraphics24_Red2 = fromRgb(187, 119, 132);

export const Table_StatisticalGraphics24_Red3 = fromRgb(142, 6, 59);

export const Table_StatisticalGraphics24_LightBlue1 = fromRgb(74, 111, 227);

export const Table_StatisticalGraphics24_LightBlue2 = fromRgb(133, 149, 225);

export const Table_StatisticalGraphics24_LightBlue3 = fromRgb(181, 187, 227);

export const Table_StatisticalGraphics24_LightRed1 = fromRgb(230, 175, 185);

export const Table_StatisticalGraphics24_LightRed2 = fromRgb(224, 123, 145);

export const Table_StatisticalGraphics24_LightRed3 = fromRgb(211, 63, 106);

export const Table_StatisticalGraphics24_Green1 = fromRgb(17, 198, 56);

export const Table_StatisticalGraphics24_Green2 = fromRgb(141, 213, 147);

export const Table_StatisticalGraphics24_Green3 = fromRgb(198, 222, 199);

export const Table_StatisticalGraphics24_Orange1 = fromRgb(234, 211, 198);

export const Table_StatisticalGraphics24_Orange2 = fromRgb(240, 185, 141);

export const Table_StatisticalGraphics24_Orange3 = fromRgb(239, 151, 8);

export const Table_StatisticalGraphics24_Cyan1 = fromRgb(15, 207, 192);

export const Table_StatisticalGraphics24_Cyan2 = fromRgb(156, 222, 214);

export const Table_StatisticalGraphics24_Cyan3 = fromRgb(213, 234, 231);

export const Table_StatisticalGraphics24_Magenta1 = fromRgb(243, 225, 235);

export const Table_StatisticalGraphics24_Magenta2 = fromRgb(246, 196, 225);

export const Table_StatisticalGraphics24_Magenta3 = fromRgb(247, 156, 212);

const Table_StatisticalGraphics24_paletteArray = [Table_StatisticalGraphics24_Blue1, Table_StatisticalGraphics24_Blue2, Table_StatisticalGraphics24_Blue3, Table_StatisticalGraphics24_Red1, Table_StatisticalGraphics24_Red2, Table_StatisticalGraphics24_Red3, Table_StatisticalGraphics24_LightBlue1, Table_StatisticalGraphics24_LightBlue2, Table_StatisticalGraphics24_LightBlue3, Table_StatisticalGraphics24_LightRed1, Table_StatisticalGraphics24_LightRed2, Table_StatisticalGraphics24_LightRed3, Table_StatisticalGraphics24_Green1, Table_StatisticalGraphics24_Green2, Table_StatisticalGraphics24_Green3, Table_StatisticalGraphics24_Orange1, Table_StatisticalGraphics24_Orange2, Table_StatisticalGraphics24_Orange3, Table_StatisticalGraphics24_Cyan1, Table_StatisticalGraphics24_Cyan2, Table_StatisticalGraphics24_Cyan3, Table_StatisticalGraphics24_Magenta1, Table_StatisticalGraphics24_Magenta2, Table_StatisticalGraphics24_Magenta3];

export function Table_StatisticalGraphics24_getRandomColor(rnd) {
    const index = rnd.Next2(0, 23) | 0;
    return Table_StatisticalGraphics24_paletteArray[index];
}

