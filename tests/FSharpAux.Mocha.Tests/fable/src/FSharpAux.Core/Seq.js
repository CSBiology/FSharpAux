import { exists, filter, initialize, choose, ofList, foldBack, tryPick, map, empty as empty_1, toList, iterate, length as length_1, enumerateWhile, enumerateUsing, delay, concat, initializeInfinite, take, singleton, append } from "../../fable_modules/fable-library.4.0.6/Seq.js";
import { head, tail, ofArrayWithTail, isEmpty, empty, singleton as singleton_1, cons } from "../../fable_modules/fable-library.4.0.6/List.js";
import { defaultOf, structuralHash, equals, compare, disposeSafe, getEnumerator } from "../../fable_modules/fable-library.4.0.6/Util.js";
import { HashSet } from "../../fable_modules/fable-library.4.0.6/MutableSet.js";
import { HashIdentity_Structural } from "../../fable_modules/fable-library.4.0.6/FSharp.Collections.js";
import { addToDict, tryGetValue, addToSet } from "../../fable_modules/fable-library.4.0.6/MapUtil.js";
import { intersectWith } from "../../fable_modules/fable-library.4.0.6/Set.js";
import { FSharpRef } from "../../fable_modules/fable-library.4.0.6/Types.js";
import { FSharpMap__get_Item, FSharpMap__ContainsKey, ofSeq } from "../../fable_modules/fable-library.4.0.6/Map.js";
import { value as value_1, some } from "../../fable_modules/fable-library.4.0.6/Option.js";
import { Dictionary } from "../../fable_modules/fable-library.4.0.6/MutableMap.js";
import { rangeDouble } from "../../fable_modules/fable-library.4.0.6/Range.js";
import { isInfinity } from "../../fable_modules/fable-library.4.0.6/Double.js";

export function appendSingleton(s, value) {
    return append(s, singleton(value));
}

export function consSingleton(s, value) {
    return append(singleton(value), s);
}

export function initRepeatValue(length, value) {
    return take(length, initializeInfinite((_arg) => value));
}

export function initRepeatValues(length, values) {
    return concat(take(length, initializeInfinite((_arg) => values)));
}

export function groupAfter(f, input) {
    const group = (en_mut, cont_mut, acc_mut, c_mut) => {
        group:
        while (true) {
            const en = en_mut, cont = cont_mut, acc = acc_mut, c = c_mut;
            if (!f(en["System.Collections.Generic.IEnumerator`1.get_Current"]()) && en["System.Collections.IEnumerator.MoveNext"]()) {
                en_mut = en;
                cont_mut = ((l) => cont(cons(c, l)));
                acc_mut = acc;
                c_mut = en["System.Collections.Generic.IEnumerator`1.get_Current"]();
                continue group;
            }
            else {
                return cont(singleton_1(c));
            }
            break;
        }
    };
    return delay(() => enumerateUsing(getEnumerator(input), (en_1) => enumerateWhile(() => en_1["System.Collections.IEnumerator.MoveNext"](), delay(() => singleton(group(en_1, (x) => x, empty(), en_1["System.Collections.Generic.IEnumerator`1.get_Current"]()))))));
}

export function groupWhen(f, input) {
    let matchValue_2, t_2, h_2;
    const en = getEnumerator(input);
    try {
        let firstCase = false;
        const loop = (cont_mut) => {
            loop:
            while (true) {
                const cont = cont_mut;
                if (en["System.Collections.IEnumerator.MoveNext"]()) {
                    if (f(en["System.Collections.Generic.IEnumerator`1.get_Current"]())) {
                        const temp = en["System.Collections.Generic.IEnumerator`1.get_Current"]();
                        cont_mut = ((y) => {
                            let firstCase_1;
                            return cont((firstCase_1 = firstCase, isEmpty(y) ? ((firstCase = true, singleton_1(singleton_1(temp)))) : (firstCase_1 ? ((firstCase = false, ofArrayWithTail([empty(), singleton_1(temp)], y))) : ofArrayWithTail([empty(), cons(temp, head(y))], tail(y)))));
                        });
                        continue loop;
                    }
                    else {
                        const temp_1 = en["System.Collections.Generic.IEnumerator`1.get_Current"]();
                        cont_mut = ((y_1) => {
                            let firstCase_2;
                            return cont((firstCase_2 = firstCase, isEmpty(y_1) ? singleton_1(singleton_1(temp_1)) : (firstCase_2 ? ((firstCase = false, cons(singleton_1(temp_1), y_1))) : cons(cons(temp_1, head(y_1)), tail(y_1)))));
                        });
                        continue loop;
                    }
                }
                else {
                    return cont(empty());
                }
                break;
            }
        };
        return (matchValue_2 = loop((x) => x), isEmpty(matchValue_2) ? empty() : ((t_2 = tail(matchValue_2), (h_2 = head(matchValue_2), isEmpty(h_2) ? t_2 : cons(h_2, t_2)))));
    }
    finally {
        disposeSafe(en);
    }
}

export function intersect(seq1, seq2) {
    const patternInput = (length_1(seq1) >= length_1(seq2)) ? [seq2, seq1] : [seq1, seq2];
    const hsSs = new HashSet([], HashIdentity_Structural());
    iterate((arg_1) => {
        addToSet(arg_1, hsSs);
    }, patternInput[0]);
    intersectWith(hsSs, patternInput[1]);
    return hsSs;
}

export function groupsOfAtMost(size, s) {
    return delay(() => {
        const en = getEnumerator(s);
        const more = new FSharpRef(true);
        return enumerateWhile(() => more.contents, delay(() => {
            const group = toList(delay(() => {
                const i = new FSharpRef(0);
                return enumerateWhile(() => ((i.contents < size) && en["System.Collections.IEnumerator.MoveNext"]()), delay(() => append(singleton(en["System.Collections.Generic.IEnumerator`1.get_Current"]()), delay(() => {
                    i.contents = ((i.contents + 1) | 0);
                    return empty_1();
                }))));
            }));
            if (isEmpty(group)) {
                more.contents = false;
                return empty_1();
            }
            else {
                return singleton(group);
            }
        }));
    });
}

export function countIf(f, input) {
    const loop = (en_1_mut, counter_mut) => {
        loop:
        while (true) {
            const en_1 = en_1_mut, counter = counter_mut;
            if (en_1["System.Collections.IEnumerator.MoveNext"]()) {
                if (f(en_1["System.Collections.Generic.IEnumerator`1.get_Current"]())) {
                    en_1_mut = en_1;
                    counter_mut = (counter + 1);
                    continue loop;
                }
                else {
                    en_1_mut = en_1;
                    counter_mut = counter;
                    continue loop;
                }
            }
            else {
                return counter | 0;
            }
            break;
        }
    };
    return loop(getEnumerator(input), 0) | 0;
}

export function pivotize(aggregation, defaultValue, keyList, valueList) {
    const m = ofSeq(valueList, {
        Compare: compare,
    });
    return map((k) => {
        if (FSharpMap__ContainsKey(m, k)) {
            return aggregation(FSharpMap__get_Item(m, k));
        }
        else {
            return defaultValue;
        }
    }, keyList);
}

export function tryHead(s) {
    return tryPick(some, s);
}

export function headOrDefault(defaultValue, s) {
    const matchValue = tryHead(s);
    if (matchValue == null) {
        return defaultValue;
    }
    else {
        return value_1(matchValue);
    }
}

export function unzip(input) {
    const patternInput = foldBack((tupledArg, tupledArg_1) => [cons(tupledArg[0], tupledArg_1[0]), cons(tupledArg[1], tupledArg_1[1])], input, [empty(), empty()]);
    return [ofList(patternInput[0]), ofList(patternInput[1])];
}

export function unzip3(input) {
    const patternInput = foldBack((tupledArg, tupledArg_1) => [cons(tupledArg[0], tupledArg_1[0]), cons(tupledArg[1], tupledArg_1[1]), cons(tupledArg[2], tupledArg_1[2])], input, [empty(), empty(), empty()]);
    return [ofList(patternInput[0]), ofList(patternInput[1]), ofList(patternInput[2])];
}

export function foldi(f, acc, sequence) {
    const en = getEnumerator(sequence);
    const loop = (i_mut, acc_1_mut) => {
        loop:
        while (true) {
            const i = i_mut, acc_1 = acc_1_mut;
            const matchValue = en["System.Collections.IEnumerator.MoveNext"]();
            if (matchValue) {
                i_mut = (i + 1);
                acc_1_mut = f(i, acc_1, en["System.Collections.Generic.IEnumerator`1.get_Current"]());
                continue loop;
            }
            else {
                return acc_1;
            }
            break;
        }
    };
    return loop(0, acc);
}

export function countDistinctBy(keyf, sequence) {
    const dict = new Dictionary([], HashIdentity_Structural());
    const en = getEnumerator(sequence);
    while (en["System.Collections.IEnumerator.MoveNext"]()) {
        const key = keyf(en["System.Collections.Generic.IEnumerator`1.get_Current"]());
        let matchValue;
        let outArg = 0;
        matchValue = [tryGetValue(dict, key, new FSharpRef(() => outArg, (v) => {
            outArg = (v | 0);
        })), outArg];
        if (matchValue[0]) {
            dict.set(key, matchValue[1] + 1);
        }
        else {
            dict.set(key, 1);
        }
    }
    return delay(() => map((v_1) => [v_1[0], v_1[1]], dict));
}

export function joinCross(s) {
    return choose((v) => {
        let matchResult, left, right;
        if (v[0] != null) {
            if (v[1] != null) {
                matchResult = 0;
                left = value_1(v[0]);
                right = value_1(v[1]);
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
                return [left, right];
            default:
                return void 0;
        }
    }, s);
}

export function joinLeft(s) {
    return choose((v) => {
        let matchResult, left, right;
        if (v[0] != null) {
            if (v[1] != null) {
                matchResult = 0;
                left = value_1(v[0]);
                right = value_1(v[1]);
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
                return [left, right];
            default:
                return void 0;
        }
    }, s);
}

export function joinRight(s) {
    return choose((v) => {
        let matchResult, left, right;
        if (v[0] != null) {
            if (v[1] != null) {
                matchResult = 0;
                left = value_1(v[0]);
                right = value_1(v[1]);
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
                return [left, right];
            default:
                return void 0;
        }
    }, s);
}

export function joinBy(joinOption, keyf1, keyf2, s1, s2) {
    const dict = new Dictionary([], {
        Equals: equals,
        GetHashCode: structuralHash,
    });
    iterate((l) => {
        const key = keyf1(l);
        const lValue = l;
        let patternInput;
        let outArg = defaultOf();
        patternInput = [tryGetValue(dict, key, new FSharpRef(() => outArg, (v) => {
            outArg = v;
        })), outArg];
        if (patternInput[0]) {
            dict.set(key, [some(lValue), patternInput[1][1]]);
        }
        else {
            addToDict(dict, key, [some(lValue), void 0]);
        }
    }, s1);
    iterate((r) => {
        const key_1 = keyf2(r);
        const rValue = r;
        let patternInput_1;
        let outArg_1 = defaultOf();
        patternInput_1 = [tryGetValue(dict, key_1, new FSharpRef(() => outArg_1, (v_1) => {
            outArg_1 = v_1;
        })), outArg_1];
        if (patternInput_1[0]) {
            dict.set(key_1, [patternInput_1[1][0], some(rValue)]);
        }
        else {
            addToDict(dict, key_1, [void 0, some(rValue)]);
        }
    }, s2);
    return joinOption(map((group) => group[1], dict));
}

export function Double_seqInit(from, tto, length) {
    const stepWidth = (tto - from) / (length - 1);
    return initialize(~~length, (x) => ((x * stepWidth) + from));
}

export function Double_seqInitStepWidth(from, tto, stepWidth) {
    return rangeDouble(from, stepWidth, tto);
}

export function Double_filterNaN(sq) {
    return filter((x) => !Number.isNaN(x), sq);
}

export function Double_filterNanBy(f, sq) {
    return filter((x) => !Number.isNaN(f(x)), sq);
}

export function Double_filterInfinity(sq) {
    return filter((x) => !isInfinity(x), sq);
}

export function Double_filterInfinityBy(f, sq) {
    return filter((x) => !isInfinity(f(x)), sq);
}

export function Double_filterNanAndInfinity(sq) {
    return filter((x) => !(Number.isNaN(x) ? true : isInfinity(x)), sq);
}

export function Double_filterNanAndInfinityBy(f, sq) {
    return filter((v) => {
        const x = f(v);
        return !(Number.isNaN(x) ? true : isInfinity(x));
    }, sq);
}

export function Double_existsNaN(sq) {
    return exists((x) => Number.isNaN(x), sq);
}

export function Double_existsNanBy(f, sq) {
    return exists((x) => Number.isNaN(f(x)), sq);
}

