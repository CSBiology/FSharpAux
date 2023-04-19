import { indexed, collect, choose, fold, map3, map2, mapIndexed, copy, map as map_1, initialize } from "../../fable_modules/fable-library.4.0.6/Array.js";
import { initialize as initialize_1, choose as choose_1, filter, fold as fold_1, map3 as map3_1, item, map2 as map2_1, mapIndexed as mapIndexed_1, ofSeq, isEmpty, empty, cons, tail, head, ofArray, map, toArray } from "../../fable_modules/fable-library.4.0.6/List.js";
import { map as map_2 } from "../../fable_modules/fable-library.4.0.6/Seq.js";

export function JaggedArray_transpose(arr) {
    if (arr.length > 0) {
        return initialize(arr[0].length, (rowI) => initialize(arr.length, (colI) => arr[colI][rowI]));
    }
    else {
        return arr;
    }
}

export function JaggedArray_ofJaggedList(data) {
    return toArray(map(toArray, data));
}

export function JaggedArray_toJaggedList(arr) {
    return ofArray(map_1(ofArray, arr));
}

export function JaggedArray_ofJaggedSeq(data) {
    return Array.from(map_2((s) => Array.from(s), data));
}

export function JaggedArray_copy(arr) {
    return initialize(arr.length, (i) => copy(arr[i]));
}

export function JaggedArray_toJaggedSeq(arr) {
    return map_2((s) => s, arr);
}

export function JaggedArray_map(mapping, jArray) {
    return map_1((x) => map_1(mapping, x), jArray);
}

export function JaggedArray_map2(mapping, jArray1, jArray2) {
    return mapIndexed((index, x) => map2(mapping, x, jArray2[index]), jArray1);
}

export function JaggedArray_map3(mapping, jArray1, jArray2, jArray3) {
    return mapIndexed((index, x) => map3(mapping, x, jArray2[index], jArray3[index]), jArray1);
}

export function JaggedArray_mapi(mapping, jArray) {
    return map_1((x) => mapIndexed(mapping, x), jArray);
}

export function JaggedArray_innerFold(folder, state, jArray) {
    return map_1((x) => fold(folder, state, x), jArray);
}

export function JaggedArray_fold(innerFolder, outerFolder, innerState, outerState, jArray) {
    return fold(outerFolder, outerState, JaggedArray_innerFold(innerFolder, innerState, jArray));
}

export function JaggedArray_innerFilter(predicate, jArray) {
    return map_1((x) => x.filter(predicate), jArray);
}

export function JaggedArray_innerChoose(chooser, jArray) {
    return map_1((x) => choose(chooser, x), jArray);
}

export function JaggedArray_shuffleColumnWise(rnd, arr) {
    const tmpArr = JaggedArray_copy(arr);
    if (arr.length > 0) {
        const rowCount = arr.length | 0;
        const columnCount = arr[0].length | 0;
        for (let ci = columnCount - 1; ci >= 0; ci--) {
            for (let ri = rowCount; ri >= 1; ri--) {
                const rj = rnd.Next1(ri) | 0;
                const tmp = tmpArr[rj][ci];
                tmpArr[rj][ci] = tmpArr[ri - 1][ci];
                tmpArr[ri - 1][ci] = tmp;
            }
        }
        return tmpArr;
    }
    else {
        return tmpArr;
    }
}

export function JaggedArray_shuffleRowWise(rnd, arr) {
    const tmpArr = JaggedArray_copy(arr);
    if (arr.length > 0) {
        const rowCount = arr.length | 0;
        const columnCount = arr[0].length | 0;
        for (let ri = rowCount - 1; ri >= 0; ri--) {
            for (let ci = columnCount; ci >= 1; ci--) {
                const cj = rnd.Next1(ci) | 0;
                const tmp = tmpArr[ri][cj];
                tmpArr[ri][cj] = tmpArr[ri][ci - 1];
                tmpArr[ri][ci - 1] = tmp;
            }
        }
        return tmpArr;
    }
    else {
        return tmpArr;
    }
}

export function JaggedArray_shuffle(rnd, arr) {
    const tmpArr = JaggedArray_copy(arr);
    if (arr.length > 0) {
        const rowCount = arr.length | 0;
        const columnCount = arr[0].length | 0;
        for (let ri = rowCount; ri >= 1; ri--) {
            for (let ci = columnCount; ci >= 1; ci--) {
                const rj = rnd.Next1(ri) | 0;
                const cj = rnd.Next1(ci) | 0;
                const tmp = tmpArr[rj][cj];
                tmpArr[rj][cj] = tmpArr[ri - 1][ci - 1];
                tmpArr[ri - 1][ci - 1] = tmp;
            }
        }
        return tmpArr;
    }
    else {
        return tmpArr;
    }
}

export function JaggedArray_shuffleColumnWiseInPlace(rnd, arr) {
    if (arr.length > 0) {
        const rowCount = arr.length | 0;
        const columnCount = arr[0].length | 0;
        for (let ci = columnCount - 1; ci >= 0; ci--) {
            for (let ri = rowCount; ri >= 1; ri--) {
                const rj = rnd.Next1(ri) | 0;
                const tmp = arr[rj][ci];
                arr[rj][ci] = arr[ri - 1][ci];
                arr[ri - 1][ci] = tmp;
            }
        }
        return arr;
    }
    else {
        return arr;
    }
}

export function JaggedArray_shuffleRowWiseInPlace(rnd, arr) {
    if (arr.length > 0) {
        const rowCount = arr.length | 0;
        const columnCount = arr[0].length | 0;
        for (let ri = rowCount - 1; ri >= 0; ri--) {
            for (let ci = columnCount; ci >= 1; ci--) {
                const cj = rnd.Next1(ci) | 0;
                const tmp = arr[ri][cj];
                arr[ri][cj] = arr[ri][ci - 1];
                arr[ri][ci - 1] = tmp;
            }
        }
        return arr;
    }
    else {
        return arr;
    }
}

export function JaggedArray_shuffleInPlace(rnd, arr) {
    if (arr.length > 0) {
        const rowCount = arr.length | 0;
        const columnCount = arr[0].length | 0;
        for (let ri = rowCount; ri >= 1; ri--) {
            for (let ci = columnCount; ci >= 1; ci--) {
                const rj = rnd.Next1(ri) | 0;
                const cj = rnd.Next1(ci) | 0;
                const tmp = arr[rj][cj];
                arr[rj][cj] = arr[ri - 1][ci - 1];
                arr[ri - 1][ci - 1] = tmp;
            }
        }
        return arr;
    }
    else {
        return arr;
    }
}

export function JaggedArray_init(count1, count2, initializer) {
    return initialize(count1, (i) => initialize(count2, (j) => initializer(i, j)));
}

export function JaggedArray_toIndexedArray(jArr) {
    return collect((tupledArg) => map_1((tupledArg_1) => [tupledArg[0], tupledArg_1[0], tupledArg_1[1]], tupledArg[1]), indexed(map_1(indexed, jArr)));
}

export function JaggedList_transpose(data) {
    const transpose = (_arg) => {
        let matchResult, M;
        if (!isEmpty(_arg)) {
            if (!isEmpty(head(_arg))) {
                matchResult = 0;
                M = _arg;
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
                return cons(map(head, M), transpose(map(tail, M)));
            default:
                return empty();
        }
    };
    return transpose(data);
}

export function JaggedList_toJaggedArray(data) {
    return toArray(map(toArray, data));
}

export function JaggedList_ofJaggedArray(arr) {
    return ofArray(map_1(ofArray, arr));
}

export function JaggedList_ofJaggedSeq(data) {
    return ofSeq(map_2(ofSeq, data));
}

export function JaggedList_toJaggedSeq(data) {
    return map_2((s) => s, data);
}

export function JaggedList_map(mapping, jlist) {
    return map((x) => map(mapping, x), jlist);
}

export function JaggedList_map2(mapping, jlist1, jlist2) {
    return mapIndexed_1((index, x) => map2_1(mapping, x, item(index, jlist2)), jlist1);
}

export function JaggedList_map3(mapping, jlist1, jlist2, jlist3) {
    return mapIndexed_1((index, x) => map3_1(mapping, x, item(index, jlist2), item(index, jlist3)), jlist1);
}

export function JaggedList_mapi(mapping, jlist) {
    return map((x) => mapIndexed_1(mapping, x), jlist);
}

export function JaggedList_innerFold(folder, state, jlist) {
    return map((x) => fold_1(folder, state, x), jlist);
}

export function JaggedList_fold(innerFolder, outerFolder, innerState, outerState, jlist) {
    return fold_1(outerFolder, outerState, JaggedList_innerFold(innerFolder, innerState, jlist));
}

export function JaggedList_innerFilter(predicate, jlist) {
    return map((x) => filter(predicate, x), jlist);
}

export function JaggedList_innerChoose(chooser, jlist) {
    return map((x) => choose_1(chooser, x), jlist);
}

export function JaggedList_init(count1, count2, initializer) {
    return initialize_1(count1, (i) => initialize_1(count2, (j) => initializer(i, j)));
}

