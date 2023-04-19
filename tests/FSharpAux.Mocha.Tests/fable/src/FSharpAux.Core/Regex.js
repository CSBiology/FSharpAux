import { nextMatch, matches, create, match } from "../../fable_modules/fable-library.4.0.6/RegExp.js";
import { empty, tail } from "../../fable_modules/fable-library.4.0.6/List.js";
import { empty as empty_1, singleton, append, map, delay, toList } from "../../fable_modules/fable-library.4.0.6/Seq.js";

export function Active_$007CRegexMatchValue$007C_$007C(regex, input) {
    const m = match(regex, input);
    if (m != null) {
        return m[0];
    }
    else {
        return void 0;
    }
}

export function Active_$007CFirstRegexGroup$007C_$007C(pattern, input) {
    const m = match(create(pattern), input);
    if (m != null) {
        return tail(toList(delay(() => map((g) => g, m))));
    }
    else {
        return void 0;
    }
}

export function Active_$007CRegexGroups$007C_$007C(pattern, input) {
    const m = matches(create(pattern), input);
    if (m.length > 0) {
        return toList(delay(() => map((m$0027) => m$0027, m)));
    }
    else {
        return void 0;
    }
}

export function Active_$007CRegexValue$007C_$007C(pattern, input) {
    const m = match(create(pattern), input);
    if (m != null) {
        return tail(toList(delay(() => map((g) => (g || ""), m))));
    }
    else {
        return void 0;
    }
}

export function Active_$007CRegexValues$007C_$007C(pattern, input) {
    const m = matches(create(pattern), input);
    if (m.length > 0) {
        return toList(delay(() => map((g) => g[0], m)));
    }
    else {
        return void 0;
    }
}

export function Active_$007CRegexGroupValues$007C_$007C(pattern, input) {
    const m = matches(create(pattern), input);
    if (m.length > 0) {
        return toList(delay(() => map((m$0027) => toList(delay(() => map((g) => (g || ""), m$0027))), m)));
    }
    else {
        return void 0;
    }
}

export function tryParseValue(regexStr, line) {
    const m = match(create(regexStr), line);
    if (m != null) {
        return m[0];
    }
    else {
        return void 0;
    }
}

export function parse(regexStr, line) {
    const m = match(create(regexStr), line);
    if (m != null) {
        return tail(toList(delay(() => map((g) => (g || ""), m))));
    }
    else {
        return empty();
    }
}

export function parseAll(regexStr, line) {
    const loop = (m) => delay(() => {
        if (m != null) {
            return append(singleton(tail(toList(delay(() => map((g) => (g || ""), m))))), delay(() => loop(nextMatch(m))));
        }
        else {
            return empty_1();
        }
    });
    return loop(match(create(regexStr), line));
}

export function createRegex(regexOptions, pattern) {
    return create(pattern, regexOptions);
}

