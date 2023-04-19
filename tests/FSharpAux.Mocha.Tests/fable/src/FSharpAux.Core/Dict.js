import { contains, toList as toList_1, toArray as toArray_1, map } from "../../fable_modules/fable-library.4.0.6/Seq.js";
import { Dictionary } from "../../fable_modules/fable-library.4.0.6/MutableMap.js";
import { HashIdentity_Structural } from "../../fable_modules/fable-library.4.0.6/FSharp.Collections.js";
import { safeHash, toIterator, count, structuralHash, equals, disposeSafe, getEnumerator } from "../../fable_modules/fable-library.4.0.6/Util.js";
import { value, some } from "../../fable_modules/fable-library.4.0.6/Option.js";
import { getItemFromDict } from "../../fable_modules/fable-library.4.0.6/MapUtil.js";
import { toSeq as toSeq_1 } from "../../fable_modules/fable-library.4.0.6/Map.js";

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

const notMutable = "This value may not be mutated";

export function ofSeq(l) {
    const t = new Dictionary([], HashIdentity_Structural());
    const enumerator = getEnumerator(l);
    try {
        while (enumerator["System.Collections.IEnumerator.MoveNext"]()) {
            const forLoopVar = enumerator["System.Collections.Generic.IEnumerator`1.get_Current"]();
            t.set(some(forLoopVar[0]), forLoopVar[1]);
        }
    }
    finally {
        disposeSafe(enumerator);
    }
    const d = t;
    const c = t;
    return {
        "System.Collections.Generic.IDictionary`2.get_Item2B595"(x) {
            return getItemFromDict(d, some(x));
        },
        "System.Collections.Generic.IDictionary`2.set_Item5BDDA1"(x_1, v_1) {
            throw new Error("This value may not be mutated");
        },
        "System.Collections.Generic.IDictionary`2.get_Keys"() {
            const keys = d.keys();
            return {
                "System.Collections.Generic.ICollection`1.Add2B595"(x_2) {
                    throw new Error(notMutable);
                },
                "System.Collections.Generic.ICollection`1.Clear"() {
                    throw new Error(notMutable);
                },
                "System.Collections.Generic.ICollection`1.Remove2B595"(x_3) {
                    throw new Error(notMutable);
                },
                "System.Collections.Generic.ICollection`1.Contains2B595"(x_4) {
                    return contains(some(x_4), keys, {
                        Equals: equals,
                        GetHashCode: structuralHash,
                    });
                },
                "System.Collections.Generic.ICollection`1.CopyToZ3B4C077E"(arr, i) {
                    let n = 0;
                    const enumerator_1 = getEnumerator(keys);
                    try {
                        while (enumerator_1["System.Collections.IEnumerator.MoveNext"]()) {
                            const k_1 = enumerator_1["System.Collections.Generic.IEnumerator`1.get_Current"]();
                            arr[i + n] = value(k_1);
                            n = ((n + 1) | 0);
                        }
                    }
                    finally {
                        disposeSafe(enumerator_1);
                    }
                },
                "System.Collections.Generic.ICollection`1.get_IsReadOnly"() {
                    return true;
                },
                "System.Collections.Generic.ICollection`1.get_Count"() {
                    return count(keys);
                },
                GetEnumerator() {
                    return getEnumerator(map(value, keys));
                },
                [Symbol.iterator]() {
                    return toIterator(getEnumerator(this));
                },
                "System.Collections.IEnumerable.GetEnumerator"() {
                    return getEnumerator(map(value, keys));
                },
            };
        },
        "System.Collections.Generic.IDictionary`2.get_Values"() {
            return d.values();
        },
        "System.Collections.Generic.IDictionary`2.Add5BDDA1"(k_2, v_4) {
            throw new Error(notMutable);
        },
        "System.Collections.Generic.IDictionary`2.ContainsKey2B595"(k_3) {
            return d.has(some(k_3));
        },
        "System.Collections.Generic.IDictionary`2.TryGetValue6DC89625"(k_4, r) {
            const key = some(k_4);
            if (d.has(key)) {
                r.contents = getItemFromDict(d, key);
                return true;
            }
            else {
                return false;
            }
        },
        "System.Collections.Generic.IDictionary`2.Remove2B595"(k_5) {
            throw new Error(notMutable);
        },
        "System.Collections.Generic.ICollection`1.Add2B595"(x_6) {
            throw new Error(notMutable);
        },
        "System.Collections.Generic.ICollection`1.Clear"() {
            throw new Error(notMutable);
        },
        "System.Collections.Generic.ICollection`1.Remove2B595"(x_7) {
            throw new Error(notMutable);
        },
        "System.Collections.Generic.ICollection`1.Contains2B595"(_arg) {
            const activePatternResult = _arg;
            return contains([some(activePatternResult[0]), activePatternResult[1]], c, {
                Equals: equals,
                GetHashCode: safeHash,
            });
        },
        "System.Collections.Generic.ICollection`1.CopyToZ3B4C077E"(arr_1, i_1) {
            let n_1 = 0;
            const enumerator_2 = getEnumerator(c);
            try {
                while (enumerator_2["System.Collections.IEnumerator.MoveNext"]()) {
                    const activePatternResult_1 = enumerator_2["System.Collections.Generic.IEnumerator`1.get_Current"]();
                    arr_1[i_1 + n_1] = [value(activePatternResult_1[0]), activePatternResult_1[1]];
                    n_1 = ((n_1 + 1) | 0);
                }
            }
            finally {
                disposeSafe(enumerator_2);
            }
        },
        "System.Collections.Generic.ICollection`1.get_IsReadOnly"() {
            return true;
        },
        "System.Collections.Generic.ICollection`1.get_Count"() {
            return count(c);
        },
        GetEnumerator() {
            return getEnumerator(map((_arg_1) => {
                const activePatternResult_2 = _arg_1;
                return [value(activePatternResult_2[0]), activePatternResult_2[1]];
            }, c));
        },
        [Symbol.iterator]() {
            return toIterator(getEnumerator(this));
        },
        "System.Collections.IEnumerable.GetEnumerator"() {
            return getEnumerator(map((_arg_2) => {
                const activePatternResult_3 = _arg_2;
                return [value(activePatternResult_3[0]), activePatternResult_3[1]];
            }, c));
        },
    };
}

export function ofMap(m) {
    return ofSeq(toSeq_1(m));
}

export function ofList(l) {
    return ofSeq(l);
}

export function ofArray(a) {
    return ofSeq(a);
}

