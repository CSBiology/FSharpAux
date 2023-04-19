import { add as add_1, tryFind as tryFind_1, empty } from "../../fable_modules/fable-library.4.0.6/Map.js";
import { compare } from "../../fable_modules/fable-library.4.0.6/Util.js";
import { cons, singleton } from "../../fable_modules/fable-library.4.0.6/List.js";
import { fold } from "../../fable_modules/fable-library.4.0.6/Seq.js";

export function emptyMultiMap() {
    return empty({
        Compare: compare,
    });
}

export function add(key, value, map) {
    const matchValue = tryFind_1(key, map);
    if (matchValue == null) {
        return add_1(key, singleton(value), map);
    }
    else {
        return add_1(key, cons(value, matchValue), map);
    }
}

export function tryFind(key, map) {
    return tryFind_1(key, map);
}

export function ofSeq(data) {
    return fold((map, tupledArg) => add(tupledArg[0], tupledArg[1], map), empty({
        Compare: compare,
    }), data);
}

