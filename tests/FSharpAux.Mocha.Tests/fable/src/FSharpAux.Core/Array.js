import { map, toArray, empty, singleton, collect, delay } from "../../fable_modules/fable-library.4.0.6/Seq.js";
import { rangeDouble } from "../../fable_modules/fable-library.4.0.6/Range.js";
import { last, initialize, copyTo, mapIndexed, copy, fill } from "../../fable_modules/fable-library.4.0.6/Array.js";
import { copyToArray, defaultOf, equals, min, comparePrimitives, max } from "../../fable_modules/fable-library.4.0.6/Util.js";
import { Operators_NullArg } from "../../fable_modules/fable-library.4.0.6/FSharp.Core.js";
import { Dictionary } from "../../fable_modules/fable-library.4.0.6/MutableMap.js";
import { HashIdentity_Structural } from "../../fable_modules/fable-library.4.0.6/FSharp.Collections.js";
import { addToSet, tryGetValue } from "../../fable_modules/fable-library.4.0.6/MapUtil.js";
import { FSharpRef } from "../../fable_modules/fable-library.4.0.6/Types.js";
import { value as value_1 } from "../../fable_modules/fable-library.4.0.6/Option.js";
import { HashSet } from "../../fable_modules/fable-library.4.0.6/MutableSet.js";
import { intersectWith } from "../../fable_modules/fable-library.4.0.6/Set.js";

export function removeIndex(index, arr) {
    if (index < arr.length) {
        return Array.from(delay(() => collect((i) => ((i !== index) ? singleton(arr[i]) : empty()), rangeDouble(0, 1, arr.length - 1))));
    }
    else {
        throw new Error("index not present in array\\nParameter name: index");
    }
}

export function scanSubRight(f, arr, start, fin, initState) {
    let state = initState;
    const res = fill(new Array((2 + fin) - start), 0, (2 + fin) - start, initState);
    for (let i = fin; i >= start; i--) {
        state = f(arr[i], state);
        res[i - start] = state;
    }
    return res;
}

export function scanSubLeft(f, initState, arr, start, fin) {
    let state = initState;
    const res = fill(new Array((2 + fin) - start), 0, (2 + fin) - start, initState);
    for (let i = start; i <= fin; i++) {
        state = f(state, arr[i]);
        res[(i - start) + 1] = state;
    }
    return res;
}

export function scanReduce(f, arr) {
    const arrn = arr.length | 0;
    if (arrn === 0) {
        throw new Error("the input array is empty\\nParameter name: arr");
    }
    else {
        return scanSubLeft(f, arr[0], arr, 1, arrn - 1);
    }
}

export function scanReduceBack(f, arr) {
    const arrn = arr.length | 0;
    if (arrn === 0) {
        throw new Error("the input array is empty\\nParameter name: arr");
    }
    else {
        return scanSubRight(f, arr, 0, arrn - 2, arr[arrn - 1]);
    }
}

export function shuffleFisherYates(rnd, arr) {
    const tmpArr = copy(arr);
    for (let i = arr.length; i >= 1; i--) {
        const j = rnd.Next1(i) | 0;
        const tmp = tmpArr[j];
        tmpArr[j] = tmpArr[i - 1];
        tmpArr[i - 1] = tmp;
    }
    return tmpArr;
}

export function shuffleInPlace(rnd, arr) {
    for (let i = arr.length; i >= 1; i--) {
        const j = rnd.Next1(i) | 0;
        const tmp = arr[j];
        arr[j] = arr[i - 1];
        arr[i - 1] = tmp;
    }
    return arr;
}

export function tryFindDefault(arr, zero, index) {
    if (arr.length > index) {
        return arr[index];
    }
    else {
        return zero;
    }
}

export function centeredWindow(n, source) {
    if (n < 0) {
        throw new Error("n must be a positive integer\\nParameter name: n");
    }
    const lastIndex = (source.length - 1) | 0;
    return mapIndexed((i, _arg) => {
        const windowStartIndex = max(comparePrimitives, i - n, 0) | 0;
        const arrSize = ((min(comparePrimitives, i + n, lastIndex) - windowStartIndex) + 1) | 0;
        const target = fill(new Array(arrSize), 0, arrSize, null);
        copyTo(source, windowStartIndex, target, 0, arrSize);
        return target;
    }, source);
}

export function foldi(f, acc, arr) {
    const l = arr.length | 0;
    const loop = (i_mut, acc_1_mut) => {
        loop:
        while (true) {
            const i = i_mut, acc_1 = acc_1_mut;
            if (i === l) {
                return acc_1;
            }
            else {
                i_mut = (i + 1);
                acc_1_mut = f(i, acc_1, arr[i]);
                continue loop;
            }
            break;
        }
    };
    return loop(0, acc);
}

export function foldSub(f, acc, arr, iFrom, iTo) {
    if (equals(arr, defaultOf())) {
        Operators_NullArg("array");
    }
    const f_1 = f;
    let state = acc;
    for (let i = iFrom; i <= iTo; i++) {
        state = f_1(state, arr[i]);
    }
    return state;
}

export function fold2Sub(f, acc, arr1, arr2, iFrom, iTo) {
    if (equals(arr1, defaultOf())) {
        Operators_NullArg("array1");
    }
    if (equals(arr2, defaultOf())) {
        Operators_NullArg("array2");
    }
    const f_1 = f;
    let state = acc;
    if (arr1.length !== arr2.length) {
        throw new Error("Arrays must have same size.\\nParameter name: array2");
    }
    for (let i = iFrom; i <= iTo; i++) {
        state = f_1(state, arr1[i], arr2[i]);
    }
    return state;
}

export function countDistinctBy(keyf, arr) {
    const dict = new Dictionary([], HashIdentity_Structural());
    for (let idx = 0; idx <= (arr.length - 1); idx++) {
        const key = keyf(arr[idx]);
        let matchValue;
        let outArg = defaultOf();
        matchValue = [tryGetValue(dict, key, new FSharpRef(() => outArg, (v_1) => {
            outArg = v_1;
        })), outArg];
        if (matchValue[0]) {
            const count = matchValue[1];
            count.contents = ((count.contents + 1) | 0);
        }
        else {
            dict.set(key, new FSharpRef(1));
        }
    }
    return toArray(delay(() => map((v_2) => [v_2[0], v_2[1].contents], dict)));
}

export function binarySearchIndexBy(compare, arr) {
    if (arr.length === 0) {
        throw new Error("Array cannot be empty.");
    }
    else {
        const loop = (lower_mut, upper_mut) => {
            loop:
            while (true) {
                const lower = lower_mut, upper = upper_mut;
                if (lower > upper) {
                    return ~lower | 0;
                }
                else {
                    const middle = (lower + ~~((upper - lower) / 2)) | 0;
                    const comparisonResult = compare(arr[middle]) | 0;
                    if (comparisonResult === 0) {
                        return middle | 0;
                    }
                    else if (comparisonResult < 0) {
                        lower_mut = lower;
                        upper_mut = (middle - 1);
                        continue loop;
                    }
                    else {
                        lower_mut = (middle + 1);
                        upper_mut = upper;
                        continue loop;
                    }
                }
                break;
            }
        };
        return loop(0, arr.length - 1) | 0;
    }
}

export function iterUntil(predicate, stepSize, startIdx, arr) {
    const loop = (arr_1_mut, currentIdx_mut) => {
        loop:
        while (true) {
            const arr_1 = arr_1_mut, currentIdx = currentIdx_mut;
            if (currentIdx <= 0) {
                return void 0;
            }
            else if (currentIdx >= (arr_1.length - 1)) {
                return void 0;
            }
            else if (predicate(arr_1[currentIdx])) {
                return currentIdx;
            }
            else {
                arr_1_mut = arr_1;
                currentIdx_mut = (currentIdx + stepSize);
                continue loop;
            }
            break;
        }
    };
    return loop(arr, startIdx);
}

export function iterUntili(predicate, stepSize, startIdx, arr) {
    const loop = (arr_1_mut, currentIdx_mut) => {
        loop:
        while (true) {
            const arr_1 = arr_1_mut, currentIdx = currentIdx_mut;
            if (currentIdx <= 0) {
                return void 0;
            }
            else if (currentIdx >= (arr_1.length - 1)) {
                return void 0;
            }
            else if (predicate(currentIdx, arr_1[currentIdx])) {
                return currentIdx;
            }
            else {
                arr_1_mut = arr_1;
                currentIdx_mut = (currentIdx + stepSize);
                continue loop;
            }
            break;
        }
    };
    return loop(arr, startIdx);
}

export function filteri(predicate, array) {
    let i = -1;
    return array.filter((x) => {
        i = ((i + 1) | 0);
        return predicate(i, x);
    });
}

export function countByPredicate(predicate, array) {
    let counter = 0;
    for (let i = 0; i <= (array.length - 1); i++) {
        if (predicate(array[i])) {
            counter = ((counter + 1) | 0);
        }
    }
    return counter | 0;
}

export function countiByPredicate(predicate, array) {
    let counter = 0;
    for (let i = 0; i <= (array.length - 1); i++) {
        if (predicate(i, array[i])) {
            counter = ((counter + 1) | 0);
        }
    }
    return counter | 0;
}

export function choosei(chooser, array) {
    if (equals(array, defaultOf())) {
        Operators_NullArg("array");
    }
    let i = 0;
    let first = defaultOf();
    let found = false;
    while ((i < array.length) && !found) {
        const element = array[i];
        const matchValue_1 = chooser(i, element);
        if (matchValue_1 != null) {
            const b = value_1(matchValue_1);
            first = b;
            found = true;
        }
        else {
            i = ((i + 1) | 0);
        }
    }
    if (i !== array.length) {
        const chunk1 = fill(new Array((array.length >> 2) + 1), 0, (array.length >> 2) + 1, null);
        chunk1[0] = first;
        let count = 1;
        i = ((i + 1) | 0);
        while ((count < chunk1.length) && (i < array.length)) {
            const element_1 = array[i];
            const matchValue_2 = chooser(i, element_1);
            if (matchValue_2 != null) {
                const b_1 = value_1(matchValue_2);
                chunk1[count] = b_1;
                count = ((count + 1) | 0);
            }
            i = ((i + 1) | 0);
        }
        if (i < array.length) {
            const chunk2 = fill(new Array(array.length - i), 0, array.length - i, null);
            count = 0;
            while (i < array.length) {
                const element_2 = array[i];
                const matchValue_3 = chooser(i, element_2);
                if (matchValue_3 != null) {
                    const b_2 = value_1(matchValue_3);
                    chunk2[count] = b_2;
                    count = ((count + 1) | 0);
                }
                i = ((i + 1) | 0);
            }
            const res = fill(new Array(chunk1.length + count), 0, chunk1.length + count, null);
            copyToArray(chunk1, 0, res, 0, chunk1.length);
            copyToArray(chunk2, 0, res, chunk1.length, count);
            return res;
        }
        else {
            const count_1 = count | 0;
            const array_1 = chunk1;
            const res_1 = fill(new Array(count_1), 0, count_1, null);
            if (count_1 < 64) {
                for (let i_1 = 0; i_1 <= (res_1.length - 1); i_1++) {
                    res_1[i_1] = array_1[0 + i_1];
                }
            }
            else {
                copyToArray(array_1, 0, res_1, 0, count_1);
            }
            return res_1;
        }
    }
    else {
        return new Array(0);
    }
}

export function findIndices(predicate, array) {
    let counter = 0;
    for (let i = 0; i <= (array.length - 1); i++) {
        if (predicate(array[i])) {
            counter = ((counter + 1) | 0);
        }
    }
    let outputArr = new Int32Array(counter);
    counter = 0;
    for (let i_1 = 0; i_1 <= (array.length - 1); i_1++) {
        if (predicate(array[i_1])) {
            outputArr[counter] = (i_1 | 0);
            counter = ((counter + 1) | 0);
        }
    }
    return outputArr;
}

export function findIndicesBack(predicate, array) {
    let counter = 0;
    for (let i = 0; i <= (array.length - 1); i++) {
        if (predicate(array[i])) {
            counter = ((counter + 1) | 0);
        }
    }
    let outputArr = new Int32Array(counter);
    counter = 0;
    for (let i_1 = array.length - 1; i_1 >= 0; i_1--) {
        if (predicate(array[i_1])) {
            outputArr[counter] = (i_1 | 0);
            counter = ((counter + 1) | 0);
        }
    }
    return outputArr;
}

export function takeNth(n, array) {
    return filteri((i, _arg) => (((i + 1) % n) === 0), array);
}

export function skipNth(n, array) {
    return filteri((i, _arg) => (((i + 1) % n) !== 0), array);
}

export function groupWhen(f, array) {
    const inds = findIndices(f, array);
    if (inds.length === 0) {
        return [array];
    }
    else {
        return initialize(inds.length, (i) => (((i + 1) === inds.length) ? array.slice(last(inds), array.length) : array.slice(inds[i], (inds[i + 1] - 1) + 1)));
    }
}

export function intersect(arr1, arr2) {
    const patternInput = (arr1.length >= arr2.length) ? [arr2, arr1] : [arr1, arr2];
    const hsSa = new HashSet([], HashIdentity_Structural());
    patternInput[0].forEach((arg_1) => {
        addToSet(arg_1, hsSa);
    });
    intersectWith(hsSa, patternInput[1]);
    return Array.from(hsSa);
}

export function System_Array__$005B$005D$1_TryFindDefault(this$, arr, zero, index) {
    if (arr.length > index) {
        return arr[index];
    }
    else {
        return zero;
    }
}

