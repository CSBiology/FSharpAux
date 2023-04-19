import { compareTo, toLocalTime, toUniversalTime, tryParse as tryParse_1, minValue, parse as parse_1, kind } from "../../fable_modules/fable-library.4.0.6/Date.js";
import { FSharpRef } from "../../fable_modules/fable-library.4.0.6/Types.js";

export function isLocal(d) {
    if (kind(d) === 2) {
        return true;
    }
    else {
        return false;
    }
}

export function isUtc(d) {
    if (kind(d) === 1) {
        return true;
    }
    else {
        return false;
    }
}

export function isUnspecified(d) {
    if (kind(d) === 0) {
        return true;
    }
    else {
        return false;
    }
}

export function parse(s) {
    return parse_1(s);
}

export function tryParse(s) {
    let matchValue;
    let outArg = minValue();
    matchValue = [tryParse_1(s, new FSharpRef(() => outArg, (v) => {
        outArg = v;
    })), outArg];
    if (matchValue[0]) {
        return matchValue[1];
    }
    else {
        return void 0;
    }
}

export function toUtc(d) {
    return toUniversalTime(d);
}

export function toLocal(d) {
    return toLocalTime(d);
}

export function compare(timeA, toTimeB) {
    return compareTo(timeA, toTimeB);
}

export function compareWithConversion(commonFormat, timeA, toTimeB) {
    switch (commonFormat) {
        case 1:
            return compare(toUtc(timeA), toUtc(toTimeB)) | 0;
        case 2:
            return compare(toLocal(timeA), toLocal(toTimeB)) | 0;
        default:
            return compare(timeA, toTimeB) | 0;
    }
}

export function isEarlier(timeA, toTimeB) {
    return compare(timeA, toTimeB) < 0;
}

export function isEarlierWithConversion(commonFormat, timeA, toTimeB) {
    switch (commonFormat) {
        case 1:
            return isEarlier(toUtc(timeA), toUtc(toTimeB));
        case 2:
            return isEarlier(toLocal(timeA), toLocal(toTimeB));
        default:
            return isEarlier(timeA, toTimeB);
    }
}

export function isLater(timeA, toTimeB) {
    return compare(timeA, toTimeB) > 0;
}

export function isLaterWithConversion(commonFormat, timeA, toTimeB) {
    switch (commonFormat) {
        case 1:
            return isLater(toUtc(timeA), toUtc(toTimeB));
        case 2:
            return isLater(toLocal(timeA), toLocal(toTimeB));
        default:
            return isLater(timeA, toTimeB);
    }
}

export function isSame(timeA, toTimeB) {
    return compare(timeA, toTimeB) === 0;
}

export function isSameWithConversion(commonFormat, timeA, toTimeB) {
    switch (commonFormat) {
        case 1:
            return isSame(toUtc(timeA), toUtc(toTimeB));
        case 2:
            return isSame(toLocal(timeA), toLocal(toTimeB));
        default:
            return isSame(timeA, toTimeB);
    }
}

