import { defaultOf, structuralHash, equals } from "../../fable_modules/fable-library.4.0.6/Util.js";
import { class_type } from "../../fable_modules/fable-library.4.0.6/Reflection.js";
import { Dictionary } from "../../fable_modules/fable-library.4.0.6/MutableMap.js";
import { addToDict, addToSet, tryGetValue } from "../../fable_modules/fable-library.4.0.6/MapUtil.js";
import { FSharpRef } from "../../fable_modules/fable-library.4.0.6/Types.js";
import { toArray, iterate, map, delay } from "../../fable_modules/fable-library.4.0.6/Seq.js";
import { printf, toFail } from "../../fable_modules/fable-library.4.0.6/String.js";
import { HashSet } from "../../fable_modules/fable-library.4.0.6/MutableSet.js";

export class StructuralEqualityComparer$1 {
    constructor() {
    }
    Equals(a, b) {
        return equals(a, b);
    }
    GetHashCode(a) {
        return structuralHash(a);
    }
}

export function StructuralEqualityComparer$1$reflection(gen0) {
    return class_type("FSharpAux.StructuralEqualityComparer`1", [gen0], StructuralEqualityComparer$1);
}

export function StructuralEqualityComparer$1_$ctor() {
    return new StructuralEqualityComparer$1();
}

export class BidirectionalDictionary$2 {
    constructor(forwardDictionary, reverseDictionary) {
        this._forwardDictionary = forwardDictionary;
        this._reverseDictionary = reverseDictionary;
    }
}

export function BidirectionalDictionary$2$reflection(gen0, gen1) {
    return class_type("FSharpAux.BidirectionalDictionary`2", [gen0, gen1], BidirectionalDictionary$2);
}

export function BidirectionalDictionary$2_$ctor_7EF8B0C0(forwardDictionary, reverseDictionary) {
    return new BidirectionalDictionary$2(forwardDictionary, reverseDictionary);
}

export function BidirectionalDictionary$2_$ctor() {
    return BidirectionalDictionary$2_$ctor_7EF8B0C0(new Dictionary([], StructuralEqualityComparer$1_$ctor()), new Dictionary([], StructuralEqualityComparer$1_$ctor()));
}

export function BidirectionalDictionary$2__Add(this$, key, value) {
    BidirectionalDictionary$2__internalAdd(this$, this$._forwardDictionary, key, value);
    BidirectionalDictionary$2__internalAdd(this$, this$._reverseDictionary, value, key);
}

export function BidirectionalDictionary$2__ContainsKey_2B595(this$, key) {
    return this$._forwardDictionary.has(key);
}

export function BidirectionalDictionary$2__ContainsValue_2B594(this$, value) {
    return this$._reverseDictionary.has(value);
}

export function BidirectionalDictionary$2__TryGetByKey_2B595(this$, key) {
    let matchValue;
    let outArg = defaultOf();
    matchValue = [tryGetValue(this$._forwardDictionary, key, new FSharpRef(() => outArg, (v) => {
        outArg = v;
    })), outArg];
    if (matchValue[0]) {
        return delay(() => map((i) => i, matchValue[1]));
    }
    else {
        return void 0;
    }
}

export function BidirectionalDictionary$2__TryGetByValue_2B594(this$, value) {
    let matchValue;
    let outArg = defaultOf();
    matchValue = [tryGetValue(this$._reverseDictionary, value, new FSharpRef(() => outArg, (v) => {
        outArg = v;
    })), outArg];
    if (matchValue[0]) {
        return delay(() => map((i) => i, matchValue[1]));
    }
    else {
        return void 0;
    }
}

export function BidirectionalDictionary$2__RemoveKey_2B595(this$, key) {
    const matchValue = BidirectionalDictionary$2__TryGetByKey_2B595(this$, key);
    if (matchValue == null) {
        toFail(printf("Cannot remove key %A. Key not found in dictionary"))(key);
    }
    else {
        const vals = matchValue;
        this$._forwardDictionary.delete(key);
        iterate((k) => {
            BidirectionalDictionary$2__internalRemoveConditional(this$, (_arg, v) => equals(v, key), this$._reverseDictionary, k);
        }, vals);
    }
}

export function BidirectionalDictionary$2__RemoveValue_2B594(this$, value) {
    const matchValue = BidirectionalDictionary$2__TryGetByValue_2B594(this$, value);
    if (matchValue == null) {
        toFail(printf("Cannot remove key %A. Key not found in dictionary"))(value);
    }
    else {
        const keys = matchValue;
        this$._reverseDictionary.delete(value);
        iterate((k_1) => {
            BidirectionalDictionary$2__internalRemoveConditional(this$, (_arg, k) => equals(k, value), this$._forwardDictionary, k_1);
        }, keys);
    }
}

export function BidirectionalDictionary$2__get_GetArrayOfKeys(this$) {
    return toArray(this$._forwardDictionary.keys());
}

export function BidirectionalDictionary$2__get_GetArrayOfValues(this$) {
    return toArray(this$._reverseDictionary.keys());
}

export function BidirectionalDictionary$2__internalAdd(this$, dict, k, v) {
    let matchValue;
    let outArg = defaultOf();
    matchValue = [tryGetValue(dict, k, new FSharpRef(() => outArg, (v_1) => {
        outArg = v_1;
    })), outArg];
    if (matchValue[0]) {
        addToSet(v, matchValue[1]);
    }
    else {
        const tmp = new HashSet([], StructuralEqualityComparer$1_$ctor());
        addToSet(v, tmp);
        addToDict(dict, k, tmp);
    }
}

export function BidirectionalDictionary$2__internalAddRange(this$, dict, k, vv) {
    let matchValue;
    let outArg = defaultOf();
    matchValue = [tryGetValue(dict, k, new FSharpRef(() => outArg, (v) => {
        outArg = v;
    })), outArg];
    if (matchValue[0]) {
        iterate((v_1) => {
            addToSet(v_1, matchValue[1]);
        }, vv);
    }
    else {
        const tmp = new HashSet([], StructuralEqualityComparer$1_$ctor());
        iterate((v_2) => {
            addToSet(v_2, tmp);
        }, vv);
        addToDict(dict, k, tmp);
    }
}

export function BidirectionalDictionary$2__internalRemove(this$, dict, k) {
    return dict.delete(k);
}

export function BidirectionalDictionary$2__internalRemoveConditional(this$, f, dict, k) {
    try {
        let matchValue;
        let outArg = defaultOf();
        matchValue = [tryGetValue(dict, k, new FSharpRef(() => outArg, (v) => {
            outArg = v;
        })), outArg];
        if (matchValue[0]) {
            const container = matchValue[1];
            const vals = toArray(container);
            vals.forEach((v_1) => {
                if (f(k, v_1)) {
                    container.delete(v_1);
                }
            });
            if (container.size === 0) {
                dict.delete(k);
            }
        }
    }
    catch (matchValue_1) {
    }
}

