import { FSharpMap__TryFind, empty, add, tryFind, fold } from "../../fable_modules/fable-library.4.0.6/Map.js";
import { value as value_1 } from "../../fable_modules/fable-library.4.0.6/Option.js";
import { fold as fold_1 } from "../../fable_modules/fable-library.4.0.6/Seq.js";
import { cons, singleton } from "../../fable_modules/fable-library.4.0.6/List.js";
import { compare } from "../../fable_modules/fable-library.4.0.6/Util.js";

export function Map_merge(a, b, f) {
    return fold((s, k, v) => {
        const matchValue = tryFind(k, s);
        return (matchValue == null) ? add(k, v, s) : add(k, f(k, [v, value_1(matchValue)]), s);
    }, a, b);
}

export function Map_compose(data) {
    return fold_1((map, tupledArg) => {
        const key = tupledArg[0];
        const value = tupledArg[1];
        const matchValue = tryFind(key, map);
        if (matchValue == null) {
            return add(key, singleton(value), map);
        }
        else {
            return add(key, cons(value, matchValue), map);
        }
    }, empty({
        Compare: compare,
    }), data);
}

export function Map_tryFindDefault(a, zero, key) {
    const matchValue = FSharpMap__TryFind(a, key);
    if (matchValue != null) {
        return value_1(matchValue);
    }
    else {
        return zero;
    }
}

export function Microsoft_FSharp_Collections_FSharpMap$2__Map$2_TryFindDefault(this$, zero, key) {
    const matchValue = FSharpMap__TryFind(this$, key);
    if (matchValue != null) {
        return value_1(matchValue);
    }
    else {
        return zero;
    }
}

