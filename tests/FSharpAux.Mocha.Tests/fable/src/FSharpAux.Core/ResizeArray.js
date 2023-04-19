import { readOnly, map as map_1, collect, delay } from "../../fable_modules/fable-library.4.0.6/Seq.js";
import { rangeDouble } from "../../fable_modules/fable-library.4.0.6/Range.js";
import { tail, head, isEmpty as isEmpty_1, length as length_1, cons, empty } from "../../fable_modules/fable-library.4.0.6/List.js";
import { value, some } from "../../fable_modules/fable-library.4.0.6/Option.js";
import { disposeSafe, getEnumerator, compare } from "../../fable_modules/fable-library.4.0.6/Util.js";
import { HashSet } from "../../fable_modules/fable-library.4.0.6/MutableSet.js";
import { HashIdentity_Structural } from "../../fable_modules/fable-library.4.0.6/FSharp.Collections.js";
import { addToSet } from "../../fable_modules/fable-library.4.0.6/MapUtil.js";

export function length(arr) {
    return arr.length;
}

export function get$(arr, n) {
    return arr[n];
}

export function set$(arr, n, x) {
    arr[n] = x;
}

export function create(n, x) {
    const arr = [];
    for (let i = 0; i <= (n - 1); i++) {
        void (arr.push(x));
    }
    return arr;
}

export function init(n, f) {
    const arr = [];
    for (let i = 0; i <= (n - 1); i++) {
        void (arr.push(f(i)));
    }
    return arr;
}

export function blit(arr1, start1, arr2, start2, len) {
    if (start1 < 0) {
        throw new Error("index must be positive\\nParameter name: start1");
    }
    if (start2 < 0) {
        throw new Error("index must be positive\\nParameter name: start2");
    }
    if (len < 0) {
        throw new Error("length must be positive\\nParameter name: len");
    }
    if ((start1 + len) > length(arr1)) {
        throw new Error("(start1+len) out of range\\nParameter name: start1");
    }
    if ((start2 + len) > length(arr2)) {
        throw new Error("(start2+len) out of range\\nParameter name: start2");
    }
    for (let i = 0; i <= (len - 1); i++) {
        arr2[start2 + i] = arr1[start1 + i];
    }
}

export function concat(arrs) {
    return Array.from(delay(() => collect((arr) => map_1((x) => x, arr), arrs)));
}

export function append(arr1, arr2) {
    return concat([arr1, arr2]);
}

export function sub(arr, start, len) {
    if (start < 0) {
        throw new Error("index must be positive\\nParameter name: start");
    }
    if (len < 0) {
        throw new Error("length must be positive\\nParameter name: len");
    }
    if ((start + len) > length(arr)) {
        throw new Error("length must be positive\\nParameter name: len");
    }
    return Array.from(delay(() => map_1((i) => arr[i], rangeDouble(start, 1, (start + len) - 1))));
}

export function fill(arr, start, len, x) {
    if (start < 0) {
        throw new Error("index must be positive\\nParameter name: start");
    }
    if (len < 0) {
        throw new Error("length must be positive\\nParameter name: len");
    }
    if ((start + len) > length(arr)) {
        throw new Error("length must be positive\\nParameter name: len");
    }
    for (let i = start; i <= ((start + len) - 1); i++) {
        arr[i] = x;
    }
}

export function copy(arr) {
    return Array.from(arr);
}

export function toList(arr) {
    let res = empty();
    for (let i = length(arr) - 1; i >= 0; i--) {
        res = cons(arr[i], res);
    }
    return res;
}

export function ofList(l) {
    const len = length_1(l) | 0;
    const res = [];
    const add = (_arg_mut) => {
        add:
        while (true) {
            const _arg = _arg_mut;
            if (!isEmpty_1(_arg)) {
                void (res.push(head(_arg)));
                _arg_mut = tail(_arg);
                continue add;
            }
            break;
        }
    };
    add(l);
    return res;
}

export function ofSeq(s) {
    return Array.from(s);
}

export function iter(f, arr) {
    for (let i = 0; i <= (arr.length - 1); i++) {
        f(arr[i]);
    }
}

export function map(f, arr) {
    const len = length(arr) | 0;
    const res = [];
    for (let i = 0; i <= (len - 1); i++) {
        void (res.push(f(arr[i])));
    }
    return res;
}

export function mapi(f, arr) {
    const f_1 = f;
    const len = length(arr) | 0;
    const res = [];
    for (let i = 0; i <= (len - 1); i++) {
        void (res.push(f_1(i, arr[i])));
    }
    return res;
}

export function iteri(f, arr) {
    const f_1 = f;
    for (let i = 0; i <= (arr.length - 1); i++) {
        f_1(i, arr[i]);
    }
}

export function exists(f, arr) {
    const len = length(arr) | 0;
    const loop = (i_mut) => {
        loop:
        while (true) {
            const i = i_mut;
            if (i < len) {
                if (f(arr[i])) {
                    return true;
                }
                else {
                    i_mut = (i + 1);
                    continue loop;
                }
            }
            else {
                return false;
            }
            break;
        }
    };
    return loop(0);
}

export function forall(f, arr) {
    const len = length(arr) | 0;
    const loop = (i_mut) => {
        loop:
        while (true) {
            const i = i_mut;
            if (i >= len) {
                return true;
            }
            else if (f(arr[i])) {
                i_mut = (i + 1);
                continue loop;
            }
            else {
                return false;
            }
            break;
        }
    };
    return loop(0);
}

export function indexNotFound() {
    throw new Error("An index satisfying the predicate was not found in the collection");
}

export function find(f, arr) {
    const loop = (i_mut) => {
        loop:
        while (true) {
            const i = i_mut;
            if (i >= length(arr)) {
                return indexNotFound();
            }
            else if (f(arr[i])) {
                return arr[i];
            }
            else {
                i_mut = (i + 1);
                continue loop;
            }
            break;
        }
    };
    return loop(0);
}

export function tryPick(f, arr) {
    const loop = (i_mut) => {
        loop:
        while (true) {
            const i = i_mut;
            if (i >= length(arr)) {
                return void 0;
            }
            else {
                const matchValue = f(arr[i]);
                if (matchValue == null) {
                    i_mut = (i + 1);
                    continue loop;
                }
                else {
                    return matchValue;
                }
            }
            break;
        }
    };
    return loop(0);
}

export function tryFind(f, arr) {
    const loop = (i_mut) => {
        loop:
        while (true) {
            const i = i_mut;
            if (i >= length(arr)) {
                return void 0;
            }
            else if (f(arr[i])) {
                return some(arr[i]);
            }
            else {
                i_mut = (i + 1);
                continue loop;
            }
            break;
        }
    };
    return loop(0);
}

export function iter2(f, arr1, arr2) {
    const f_1 = f;
    const len1 = length(arr1) | 0;
    if (len1 !== length(arr2)) {
        throw new Error("the arrays have different lengths\\nParameter name: arr2");
    }
    for (let i = 0; i <= (len1 - 1); i++) {
        f_1(arr1[i], arr2[i]);
    }
}

export function map2(f, arr1, arr2) {
    const f_1 = f;
    const len1 = length(arr1) | 0;
    if (len1 !== length(arr2)) {
        throw new Error("the arrays have different lengths\\nParameter name: arr2");
    }
    const res = [];
    for (let i = 0; i <= (len1 - 1); i++) {
        void (res.push(f_1(arr1[i], arr2[i])));
    }
    return res;
}

export function choose(f, arr) {
    const res = [];
    for (let i = 0; i <= (length(arr) - 1); i++) {
        const matchValue = f(arr[i]);
        if (matchValue != null) {
            const b = value(matchValue);
            void (res.push(b));
        }
    }
    return res;
}

export function filter(f, arr) {
    const res = [];
    for (let i = 0; i <= (length(arr) - 1); i++) {
        const x = arr[i];
        if (f(x)) {
            void (res.push(x));
        }
    }
    return res;
}

export function partition(f, arr) {
    const res1 = [];
    const res2 = [];
    for (let i = 0; i <= (length(arr) - 1); i++) {
        const x = arr[i];
        if (f(x)) {
            void (res1.push(x));
        }
        else {
            void (res2.push(x));
        }
    }
    return [res1, res2];
}

export function rev(arr) {
    const len = length(arr) | 0;
    const res = [];
    for (let i = len - 1; i >= 0; i--) {
        void (res.push(arr[i]));
    }
    return res;
}

export function foldBack(f, arr, acc) {
    let res = acc;
    const len = length(arr) | 0;
    for (let i = len - 1; i >= 0; i--) {
        res = f(get$(arr, i), res);
    }
    return res;
}

export function fold(f, acc, arr) {
    let res = acc;
    const len = length(arr) | 0;
    for (let i = 0; i <= (len - 1); i++) {
        res = f(res, get$(arr, i));
    }
    return res;
}

export function toArray(arr) {
    return arr.slice();
}

export function ofArray(arr) {
    return Array.from(arr);
}

export function toSeq(arr) {
    return readOnly(arr);
}

export function sort(f, arr) {
    arr.sort(f);
}

export function sortBy(f, arr) {
    arr.sort((x, y) => compare(f(x), f(y)));
}

export function exists2(f, arr1, arr2) {
    const len1 = length(arr1) | 0;
    if (len1 !== length(arr2)) {
        throw new Error("the arrays have different lengths\\nParameter name: arr2");
    }
    const loop = (i_mut) => {
        loop:
        while (true) {
            const i = i_mut;
            if (i < len1) {
                if (f(arr1[i], arr2[i])) {
                    return true;
                }
                else {
                    i_mut = (i + 1);
                    continue loop;
                }
            }
            else {
                return false;
            }
            break;
        }
    };
    return loop(0);
}

export function findIndex(f, arr) {
    const go = (n_mut) => {
        go:
        while (true) {
            const n = n_mut;
            if (n >= length(arr)) {
                return indexNotFound() | 0;
            }
            else if (f(arr[n])) {
                return n | 0;
            }
            else {
                n_mut = (n + 1);
                continue go;
            }
            break;
        }
    };
    return go(0) | 0;
}

export function findIndexi(f, arr) {
    const go = (n_mut) => {
        go:
        while (true) {
            const n = n_mut;
            if (n >= length(arr)) {
                return indexNotFound() | 0;
            }
            else if (f(n, arr[n])) {
                return n | 0;
            }
            else {
                n_mut = (n + 1);
                continue go;
            }
            break;
        }
    };
    return go(0) | 0;
}

export function foldSub(f, acc, arr, start, fin) {
    let res = acc;
    for (let i = start; i <= fin; i++) {
        res = f(res, arr[i]);
    }
    return res;
}

export function foldBackSub(f, arr, start, fin, acc) {
    let res = acc;
    for (let i = fin; i >= start; i--) {
        res = f(arr[i], res);
    }
    return res;
}

export function reduce(f, arr) {
    const arrn = length(arr) | 0;
    if (arrn === 0) {
        throw new Error("the input array may not be empty\\nParameter name: arr");
    }
    else {
        return foldSub(f, arr[0], arr, 1, arrn - 1);
    }
}

export function reduceBack(f, arr) {
    const arrn = length(arr) | 0;
    if (arrn === 0) {
        throw new Error("the input array may not be empty\\nParameter name: arr");
    }
    else {
        return foldBackSub(f, arr, 0, arrn - 2, arr[arrn - 1]);
    }
}

export function fold2(f, acc, arr1, arr2) {
    const f_1 = f;
    let res = acc;
    const len = length(arr1) | 0;
    if (len !== length(arr2)) {
        throw new Error("the arrays have different lengths\\nParameter name: arr2");
    }
    for (let i = 0; i <= (len - 1); i++) {
        res = f_1(res, arr1[i], arr2[i]);
    }
    return res;
}

export function foldBack2(f, arr1, arr2, acc) {
    const f_1 = f;
    let res = acc;
    const len = length(arr1) | 0;
    if (len !== length(arr2)) {
        throw new Error("the arrays have different lengths\\nParameter name: arr2");
    }
    for (let i = len - 1; i >= 0; i--) {
        res = f_1(arr1[i], arr2[i], res);
    }
    return res;
}

export function forall2(f, arr1, arr2) {
    const len1 = length(arr1) | 0;
    if (len1 !== length(arr2)) {
        throw new Error("the arrays have different lengths\\nParameter name: arr2");
    }
    const loop = (i_mut) => {
        loop:
        while (true) {
            const i = i_mut;
            if (i >= len1) {
                return true;
            }
            else if (f(arr1[i], arr2[i])) {
                i_mut = (i + 1);
                continue loop;
            }
            else {
                return false;
            }
            break;
        }
    };
    return loop(0);
}

export function isEmpty(arr) {
    return length(arr) === 0;
}

export function iteri2(f, arr1, arr2) {
    const f_1 = f;
    const len1 = length(arr1) | 0;
    if (len1 !== length(arr2)) {
        throw new Error("the arrays have different lengths\\nParameter name: arr2");
    }
    for (let i = 0; i <= (len1 - 1); i++) {
        f_1(i, arr1[i], arr2[i]);
    }
}

export function mapi2(f, arr1, arr2) {
    const f_1 = f;
    const len1 = length(arr1) | 0;
    if (len1 !== length(arr2)) {
        throw new Error("the arrays have different lengths\\nParameter name: arr2");
    }
    return init(len1, (i) => f_1(i, arr1[i], arr2[i]));
}

export function scanBackSub(f, arr, start, fin, acc) {
    const f_1 = f;
    let state = acc;
    const res = create((2 + fin) - start, acc);
    for (let i = fin; i >= start; i--) {
        state = f_1(arr[i], state);
        res[i - start] = state;
    }
    return res;
}

export function scanSub(f, acc, arr, start, fin) {
    const f_1 = f;
    let state = acc;
    const res = create((fin - start) + 2, acc);
    for (let i = start; i <= fin; i++) {
        state = f_1(state, arr[i]);
        res[(i - start) + 1] = state;
    }
    return res;
}

export function scan(f, acc, arr) {
    return scanSub(f, acc, arr, 0, length(arr) - 1);
}

export function scanBack(f, arr, acc) {
    return scanBackSub(f, arr, 0, length(arr) - 1, acc);
}

export function singleton(x) {
    const res = [];
    void (res.push(x));
    return res;
}

export function tryFindIndex(f, arr) {
    const go = (n_mut) => {
        go:
        while (true) {
            const n = n_mut;
            if (n >= length(arr)) {
                return void 0;
            }
            else if (f(arr[n])) {
                return n;
            }
            else {
                n_mut = (n + 1);
                continue go;
            }
            break;
        }
    };
    return go(0);
}

export function tryFindIndexi(f, arr) {
    const go = (n_mut) => {
        go:
        while (true) {
            const n = n_mut;
            if (n >= length(arr)) {
                return void 0;
            }
            else if (f(n, arr[n])) {
                return n;
            }
            else {
                n_mut = (n + 1);
                continue go;
            }
            break;
        }
    };
    return go(0);
}

export function zip(arr1, arr2) {
    const len1 = length(arr1) | 0;
    if (len1 !== length(arr2)) {
        throw new Error("the arrays have different lengths\\nParameter name: arr2");
    }
    return init(len1, (i) => [arr1[i], arr2[i]]);
}

export function unzip(arr) {
    const len = length(arr) | 0;
    const res1 = [];
    const res2 = [];
    for (let i = 0; i <= (len - 1); i++) {
        const patternInput = arr[i];
        void (res1.push(patternInput[0]));
        void (res2.push(patternInput[1]));
    }
    return [res1, res2];
}

export function distinctBy(keyf, array) {
    const temp = [];
    const hashSet = new HashSet([], HashIdentity_Structural());
    let enumerator = getEnumerator(array);
    try {
        while (enumerator["System.Collections.IEnumerator.MoveNext"]()) {
            const v = enumerator["System.Collections.Generic.IEnumerator`1.get_Current"]();
            if (addToSet(keyf(v), hashSet)) {
                void (temp.push(v));
            }
        }
    }
    finally {
        disposeSafe(enumerator);
    }
    return temp;
}

export function distinct(array) {
    const temp = [];
    const hashSet = new HashSet([], HashIdentity_Structural());
    let enumerator = getEnumerator(array);
    try {
        while (enumerator["System.Collections.IEnumerator.MoveNext"]()) {
            const v = enumerator["System.Collections.Generic.IEnumerator`1.get_Current"]();
            if (addToSet(v, hashSet)) {
                void (temp.push(v));
            }
        }
    }
    finally {
        disposeSafe(enumerator);
    }
    return temp;
}

export function combine(arr1, arr2) {
    return zip(arr1, arr2);
}

export function split(arr) {
    return unzip(arr);
}

export function to_list(arr) {
    return toList(arr);
}

export function of_list(l) {
    return ofList(l);
}

export function to_seq(arr) {
    return toSeq(arr);
}

