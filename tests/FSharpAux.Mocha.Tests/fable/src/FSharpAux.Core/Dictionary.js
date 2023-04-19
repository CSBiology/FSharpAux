import { toList as toList_1, toArray as toArray_1, map } from "../../fable_modules/fable-library.4.0.6/Seq.js";
import { Dictionary } from "../../fable_modules/fable-library.4.0.6/MutableMap.js";
import { curry3, uncurry2, disposeSafe, getEnumerator, count as count_1, compare, structuralHash, equals } from "../../fable_modules/fable-library.4.0.6/Util.js";
import { ofArray as ofArray_1, ofSeq as ofSeq_1, ofList as ofList_1 } from "../../fable_modules/fable-library.4.0.6/Map.js";
import { getItemFromDict, addToDict } from "../../fable_modules/fable-library.4.0.6/MapUtil.js";
import { some } from "../../fable_modules/fable-library.4.0.6/Option.js";

export function toSeq(d) {
    return map((_arg) => {
        const activePatternResult = _arg;
        return [activePatternResult[0], activePatternResult[1]];
    }, d);
}

export function toArray(d) {
    return toArray_1(toSeq(d));
}

export function toList(d) {
    return toList_1(toSeq(d));
}

export function ofMap(m) {
    return new Dictionary(m, {
        Equals: equals,
        GetHashCode: structuralHash,
    });
}

export function ofList(l) {
    return new Dictionary(ofList_1(l, {
        Compare: compare,
    }), {
        Equals: equals,
        GetHashCode: structuralHash,
    });
}

export function ofSeq(s) {
    return new Dictionary(ofSeq_1(s, {
        Compare: compare,
    }), {
        Equals: equals,
        GetHashCode: structuralHash,
    });
}

export function ofArray(a) {
    return new Dictionary(ofArray_1(a, {
        Compare: compare,
    }), {
        Equals: equals,
        GetHashCode: structuralHash,
    });
}

export function addInPlace(key, value, table) {
    addToDict(table, key, value);
    return table;
}

export function addOrUpdateInPlace(key, value, table) {
    if (table.has(key)) {
        table.set(key, value);
    }
    else {
        addToDict(table, key, value);
    }
    return table;
}

export function addOrUpdateInPlaceBy(f, key, value, table) {
    if (table.has(key)) {
        const value$0027 = getItemFromDict(table, key);
        table.set(key, f(value$0027, value));
    }
    else {
        addToDict(table, key, value);
    }
    return table;
}

export function isEmpty(table) {
    return count_1(table) < 1;
}

export function containsKey(key, table) {
    return table.has(key);
}

export function count(table) {
    return count_1(table);
}

export function item(key, table) {
    return getItemFromDict(table, key);
}

export function remove(key, table) {
    table.delete(key);
    return table;
}

export function tryFind(key, table) {
    if (table.has(key)) {
        return some(getItemFromDict(table, key));
    }
    else {
        return void 0;
    }
}

export function merge(a, b, f) {
    const dict = new Dictionary(a, {
        Equals: equals,
        GetHashCode: structuralHash,
    });
    const enumerator = getEnumerator(b);
    try {
        while (enumerator["System.Collections.IEnumerator.MoveNext"]()) {
            const kv = enumerator["System.Collections.Generic.IEnumerator`1.get_Current"]();
            addOrUpdateInPlaceBy(uncurry2(curry3(f)(kv[0])), kv[0], kv[1], dict);
        }
    }
    finally {
        disposeSafe(enumerator);
    }
    return dict;
}

