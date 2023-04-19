import { isNullOrEmpty, split, getCharAtIndex, join, substring } from "../../fable_modules/fable-library.4.0.6/String.js";
import { forAll, iterate, singleton, empty, collect, append, delay } from "../../fable_modules/fable-library.4.0.6/Seq.js";
import { StringBuilder__Clear, StringBuilder__get_Length, StringBuilder__Append_244C7CD6, StringBuilder_$ctor } from "../../fable_modules/fable-library.4.0.6/System.Text.js";
import { FSharpRef, toString } from "../../fable_modules/fable-library.4.0.6/Types.js";
import { isLower, isUpper, isLetterOrDigit, isWhiteSpace } from "../../fable_modules/fable-library.4.0.6/Char.js";
import { defaultArg, some } from "../../fable_modules/fable-library.4.0.6/Option.js";
import { tryParse } from "../../fable_modules/fable-library.4.0.6/Boolean.js";
import { tryParse as tryParse_1 } from "../../fable_modules/fable-library.4.0.6/Int32.js";
import { tryParse as tryParse_2 } from "../../fable_modules/fable-library.4.0.6/Long.js";
import { tryParse as tryParse_3 } from "../../fable_modules/fable-library.4.0.6/Double.js";
import { tryParse as tryParse_4 } from "../../fable_modules/fable-library.4.0.6/Guid.js";
import { tryParseEnum, string_type, float64_type, int64_type, int32_type, bool_type } from "../../fable_modules/fable-library.4.0.6/Reflection.js";
import { findIndex, findIndexBack, initialize } from "../../fable_modules/fable-library.4.0.6/Array.js";
import { findIndicesBack, findIndices } from "./Array.js";
import { defaultOf } from "../../fable_modules/fable-library.4.0.6/Util.js";

export function String_subString(startIndex, length, text) {
    return substring(text, startIndex, length);
}

export function String_isNewline(c) {
    if (c === "\r") {
        return true;
    }
    else {
        return c === "\n";
    }
}

export function String_splitBy(isDelimiter, str) {
    return delay(() => {
        const result = StringBuilder_$ctor();
        return append(collect((char) => {
            if (!isDelimiter(char)) {
                StringBuilder__Append_244C7CD6(result, char);
                return empty();
            }
            else {
                return (StringBuilder__get_Length(result) > 0) ? append(singleton(toString(result)), delay(() => {
                    StringBuilder__Clear(result);
                    return empty();
                })) : empty();
            }
        }, str.split("")), delay(() => ((StringBuilder__get_Length(result) > 0) ? singleton(toString(result)) : empty())));
    });
}

export function String_toLines(input) {
    return String_splitBy(String_isNewline, input);
}

export function String_joinLines(input) {
    return join("\n", input).trim();
}

export function String_toWords(input) {
    return String_splitBy(isWhiteSpace, input);
}

export function String_joinWords(input) {
    return join(" ", input).trim();
}

export function String_trim(str) {
    return str.trim();
}

export function String_implode(xs) {
    const sb = StringBuilder_$ctor();
    iterate((arg_1) => {
        StringBuilder__Append_244C7CD6(sb, arg_1);
    }, xs);
    return toString(sb);
}

export function String_op_AtQmark(s, i) {
    if (i >= s.length) {
        return void 0;
    }
    else {
        return s[i];
    }
}

export function String_$007CEOF$007C_$007C(_arg) {
    if (_arg != null) {
        return void 0;
    }
    else {
        return some(void 0);
    }
}

export const String_$007CLetterDigit$007C_$007C = (charOption) => {
    const charOption_1 = charOption;
    let matchResult;
    if (charOption_1 != null) {
        if (isLetterOrDigit(charOption_1)) {
            matchResult = 0;
        }
        else {
            matchResult = 1;
        }
    }
    else {
        matchResult = 1;
    }
    switch (matchResult) {
        case 0:
            return charOption_1;
        default:
            return void 0;
    }
};

export const String_$007CUpper$007C_$007C = (charOption) => {
    const charOption_1 = charOption;
    let matchResult;
    if (charOption_1 != null) {
        if (isUpper(charOption_1)) {
            matchResult = 0;
        }
        else {
            matchResult = 1;
        }
    }
    else {
        matchResult = 1;
    }
    switch (matchResult) {
        case 0:
            return charOption_1;
        default:
            return void 0;
    }
};

export const String_$007CLower$007C_$007C = (charOption) => {
    const charOption_1 = charOption;
    let matchResult;
    if (charOption_1 != null) {
        if (isLower(charOption_1)) {
            matchResult = 0;
        }
        else {
            matchResult = 1;
        }
    }
    else {
        matchResult = 1;
    }
    switch (matchResult) {
        case 0:
            return charOption_1;
        default:
            return void 0;
    }
};

export function String_niceName(s) {
    if (s === s.toLocaleUpperCase()) {
        return s;
    }
    else {
        const restart = (i) => delay(() => {
            const matchValue = String_op_AtQmark(s, i);
            if (String_$007CEOF$007C_$007C(matchValue) != null) {
                return empty();
            }
            else {
                let matchResult;
                if (String_$007CLetterDigit$007C_$007C(matchValue) != null) {
                    if (String_$007CUpper$007C_$007C(matchValue) != null) {
                        matchResult = 0;
                    }
                    else {
                        matchResult = 1;
                    }
                }
                else {
                    matchResult = 1;
                }
                switch (matchResult) {
                    case 0:
                        return upperStart(i)(i + 1);
                    default:
                        return (String_$007CLetterDigit$007C_$007C(matchValue) != null) ? consume(i)(false)(i + 1) : restart(i + 1);
                }
            }
        });
        const upperStart = (from) => ((i_1) => delay(() => {
            const matchValue_1 = String_op_AtQmark(s, i_1);
            return (String_$007CUpper$007C_$007C(matchValue_1) != null) ? consume(from)(true)(i_1 + 1) : ((String_$007CLower$007C_$007C(matchValue_1) != null) ? consume(from)(false)(i_1 + 1) : restart(i_1 + 1));
        }));
        const consume = (from_1) => ((takeUpper) => ((i_2) => delay(() => {
            const matchValue_2 = String_op_AtQmark(s, i_2);
            let matchResult_1;
            if (String_$007CLower$007C_$007C(matchValue_2) != null) {
                if (!takeUpper) {
                    matchResult_1 = 0;
                }
                else {
                    matchResult_1 = 1;
                }
            }
            else {
                matchResult_1 = 1;
            }
            switch (matchResult_1) {
                case 0:
                    return consume(from_1)(takeUpper)(i_2 + 1);
                default: {
                    let matchResult_2;
                    if (String_$007CUpper$007C_$007C(matchValue_2) != null) {
                        if (takeUpper) {
                            matchResult_2 = 0;
                        }
                        else {
                            matchResult_2 = 1;
                        }
                    }
                    else {
                        matchResult_2 = 1;
                    }
                    switch (matchResult_2) {
                        case 0:
                            return consume(from_1)(takeUpper)(i_2 + 1);
                        default:
                            return append(singleton([from_1, i_2]), delay(() => restart(i_2)));
                    }
                }
            }
        })));
        return join("", delay(() => collect((matchValue_3) => {
            let copyOfStruct;
            const i1 = matchValue_3[0] | 0;
            const sub = substring(s, i1, matchValue_3[1] - i1);
            return forAll(isLetterOrDigit, sub.split("")) ? singleton(((copyOfStruct = sub[0], copyOfStruct)).toLocaleUpperCase() + substring(sub.toLocaleLowerCase(), 1)) : empty();
        }, restart(0))));
    }
}

export function String_tryParseBoolDefault(defaultValue, str) {
    let matchValue;
    let outArg = false;
    matchValue = [tryParse(str, new FSharpRef(() => outArg, (v) => {
        outArg = v;
    })), outArg];
    if (matchValue[0]) {
        return matchValue[1];
    }
    else {
        return defaultValue;
    }
}

export function String_tryParseIntDefault(defaultValue, str) {
    let matchValue;
    let outArg = 0;
    matchValue = [tryParse_1(str, 511, false, 32, new FSharpRef(() => outArg, (v) => {
        outArg = (v | 0);
    })), outArg];
    if (matchValue[0]) {
        return matchValue[1] | 0;
    }
    else {
        return defaultValue | 0;
    }
}

export function String_tryParseInt64Default(defaultValue, str) {
    let matchValue;
    let outArg = 0n;
    matchValue = [tryParse_2(str, 511, false, 64, new FSharpRef(() => outArg, (v) => {
        outArg = v;
    })), outArg];
    if (matchValue[0]) {
        return matchValue[1];
    }
    else {
        return defaultValue;
    }
}

export function String_tryParseFloatDefault(defaultValue, str) {
    let matchValue;
    let outArg = 0;
    matchValue = [tryParse_3(str, new FSharpRef(() => outArg, (v) => {
        outArg = v;
    })), outArg];
    if (matchValue[0]) {
        return matchValue[1];
    }
    else {
        return defaultValue;
    }
}

export function String_tryParseGuidDefault(defaultValue, str) {
    let matchValue;
    let outArg = "00000000-0000-0000-0000-000000000000";
    matchValue = [tryParse_4(str, new FSharpRef(() => outArg, (v) => {
        outArg = v;
    })), outArg];
    if (matchValue[0]) {
        return matchValue[1];
    }
    else {
        return defaultValue;
    }
}

export function String_isBool(s) {
    const l = s.toLocaleLowerCase();
    if (((l === "true") ? true : (l === "false")) ? true : (l === "yes")) {
        return true;
    }
    else {
        return l === "no";
    }
}

export function String_isInt(s) {
    let outArg;
    return ((outArg = 0, [tryParse_1(s, 511, false, 32, new FSharpRef(() => outArg, (v) => {
        outArg = (v | 0);
    })), outArg]))[0];
}

export function String_isInt64(s) {
    let outArg;
    return ((outArg = (0n), [tryParse_2(s, 511, false, 64, new FSharpRef(() => outArg, (v) => {
        outArg = v;
    })), outArg]))[0];
}

export function String_isFloat(s) {
    let outArg;
    return ((outArg = 0, [tryParse_3(s, new FSharpRef(() => outArg, (v) => {
        outArg = v;
    })), outArg]))[0];
}

export function String_inferType(values) {
    if (forAll(String_isBool, values)) {
        return bool_type;
    }
    else if (forAll(String_isInt, values)) {
        return int32_type;
    }
    else if (forAll(String_isInt64, values)) {
        return int64_type;
    }
    else if (forAll(String_isFloat, values)) {
        return float64_type;
    }
    else {
        return string_type;
    }
}

export function String_rev(str) {
    const len = str.length | 0;
    const input = initialize(len, (i) => str[(len - i) - 1]);
    return input.join('');
}

export function String_take(n, str) {
    if (n < 0) {
        throw new Error("Can\'t take a negative number of characters from string.");
    }
    else if (n > str.length) {
        throw new Error("The input string has an insufficient number of characters.");
    }
    else {
        return str.slice(0, (n - 1) + 1);
    }
}

export function String_skip(n, str) {
    if (n < 0) {
        throw new Error("Can\'t skip a negative number of characters from string.");
    }
    else if (n > str.length) {
        throw new Error("The input string has an insufficient number of characters.");
    }
    else {
        return str.slice(n, (str.length - 1) + 1);
    }
}

export function String_first(str) {
    if (str.length === 0) {
        throw new Error("The input string was empty.\\nParameter name: str");
    }
    else {
        return getCharAtIndex(str, 0);
    }
}

export function String_last(str) {
    if (str.length === 0) {
        throw new Error("The input string was empty.\\nParameter name: str");
    }
    else {
        return getCharAtIndex(str, str.length - 1);
    }
}

export function String_splitS(delimiter, str) {
    return split(str, [delimiter], void 0, 0);
}

export function String_findIndexBack(ch, str) {
    return findIndexBack((c) => (c === ch), str.split(""));
}

export function String_findIndex(ch, str) {
    return findIndex((c) => (c === ch), str.split(""));
}

export function String_findIndices(ch, str) {
    return findIndices((c) => (c === ch), str.split(""));
}

export function String_findIndicesBack(ch, str) {
    return findIndicesBack((c) => (c === ch), str.split(""));
}

export function String_takeWhile(predicate, str) {
    if (isNullOrEmpty(str)) {
        return str;
    }
    else {
        let i = 0;
        while ((i < str.length) && predicate(str[i])) {
            i = ((i + 1) | 0);
        }
        return String_take(i, str);
    }
}

export function String_skipWhile(predicate, str) {
    if (isNullOrEmpty(str)) {
        return str;
    }
    else {
        let i = 0;
        while ((i < str.length) && predicate(str[i])) {
            i = ((i + 1) | 0);
        }
        return String_skip(i, str);
    }
}

export function System_String__String_ToEnum_6FCE9E49(this$, ignoreCase) {
    let patternInput;
    let outArg = defaultOf();
    patternInput = [tryParseEnum(this$, defaultArg(ignoreCase, true), new FSharpRef(() => outArg, (v) => {
        outArg = v;
    })), outArg];
    if (patternInput[0]) {
        return some(patternInput[1]);
    }
    else {
        return void 0;
    }
}

