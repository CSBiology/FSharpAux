import { ofSeq, iterate, fold, collect, ofArray, concat, mapIndexed, filter, map, length, empty, reverse, toArray, cons, singleton, tail as tail_1, head as head_2, scan, isEmpty } from "../../fable_modules/fable-library.4.0.6/List.js";
import { Dictionary } from "../../fable_modules/fable-library.4.0.6/MutableMap.js";
import { HashIdentity_Structural } from "../../fable_modules/fable-library.4.0.6/FSharp.Collections.js";
import { curry2, defaultOf } from "../../fable_modules/fable-library.4.0.6/Util.js";
import { addToSet, tryGetValue } from "../../fable_modules/fable-library.4.0.6/MapUtil.js";
import { FSharpRef } from "../../fable_modules/fable-library.4.0.6/Types.js";
import { map as map_1, delay, toList } from "../../fable_modules/fable-library.4.0.6/Seq.js";
import { value as value_1 } from "../../fable_modules/fable-library.4.0.6/Option.js";
import { HashSet } from "../../fable_modules/fable-library.4.0.6/MutableSet.js";
import { intersectWith } from "../../fable_modules/fable-library.4.0.6/Set.js";

export function List_scanReduce(f, l) {
    if (!isEmpty(l)) {
        return scan(f, head_2(l), tail_1(l));
    }
    else {
        throw new Error("the input list is empty\\nParameter name: l");
    }
}

export function List_scanArraySubRight(f, arr, start, fin, initState) {
    let state = initState;
    let res = singleton(state);
    for (let i = fin; i >= start; i--) {
        state = f(arr[i], state);
        res = cons(state, res);
    }
    return res;
}

export function List_scanReduceBack(f, l) {
    if (isEmpty(l)) {
        throw new Error("the input list is empty\\nParameter name: l");
    }
    else {
        const f_1 = f;
        const arr = toArray(l);
        const arrn = arr.length | 0;
        return List_scanArraySubRight(f_1, arr, 0, arrn - 2, arr[arrn - 1]);
    }
}

export function List_cutAfterN(n, input) {
    const gencut = (cur_mut, acc_mut, _arg_mut) => {
        gencut:
        while (true) {
            const cur = cur_mut, acc = acc_mut, _arg = _arg_mut;
            let matchResult, hd_1, tl_1, rest;
            if (!isEmpty(_arg)) {
                if (cur < n) {
                    matchResult = 0;
                    hd_1 = head_2(_arg);
                    tl_1 = tail_1(_arg);
                }
                else {
                    matchResult = 1;
                    rest = _arg;
                }
            }
            else {
                matchResult = 1;
                rest = _arg;
            }
            switch (matchResult) {
                case 0: {
                    cur_mut = (cur + 1);
                    acc_mut = cons(hd_1, acc);
                    _arg_mut = tl_1;
                    continue gencut;
                }
                default:
                    return [reverse(acc), rest];
            }
            break;
        }
    };
    return gencut(0, empty(), input);
}

export function List_cut(input) {
    return List_cutAfterN(~~(length(input) / 2), input);
}

export function List_groupEquals(f, input) {
    const groupLoop = (first_mut, heap_mut, stack_mut, groupStack_mut) => {
        groupLoop:
        while (true) {
            const first = first_mut, heap = heap_mut, stack = stack_mut, groupStack = groupStack_mut;
            if (isEmpty(heap)) {
                return [stack, groupStack];
            }
            else {
                const rest = tail_1(heap);
                const head = head_2(heap);
                if (f(first, head)) {
                    first_mut = first;
                    heap_mut = rest;
                    stack_mut = stack;
                    groupStack_mut = cons(head, groupStack);
                    continue groupLoop;
                }
                else {
                    first_mut = first;
                    heap_mut = rest;
                    stack_mut = cons(head, stack);
                    groupStack_mut = groupStack;
                    continue groupLoop;
                }
            }
            break;
        }
    };
    const outerLoop = (heap_1_mut, stack_1_mut) => {
        outerLoop:
        while (true) {
            const heap_1 = heap_1_mut, stack_1 = stack_1_mut;
            if (isEmpty(heap_1)) {
                return stack_1;
            }
            else {
                const head_1 = head_2(heap_1);
                const patternInput = groupLoop(head_1, tail_1(heap_1), empty(), singleton(head_1));
                heap_1_mut = patternInput[0];
                stack_1_mut = cons(patternInput[1], stack_1);
                continue outerLoop;
            }
            break;
        }
    };
    return outerLoop(input, empty());
}

export function List_applyEachPairwise(f, l) {
    const innerLoop = (hh_mut, ll_mut, acc_mut) => {
        innerLoop:
        while (true) {
            const hh = hh_mut, ll = ll_mut, acc = acc_mut;
            if (isEmpty(ll)) {
                return acc;
            }
            else {
                hh_mut = hh;
                ll_mut = tail_1(ll);
                acc_mut = cons(f(hh, head_2(ll)), acc);
                continue innerLoop;
            }
            break;
        }
    };
    const loop = (l$0027_mut, acc_1_mut) => {
        loop:
        while (true) {
            const l$0027 = l$0027_mut, acc_1 = acc_1_mut;
            if (isEmpty(l$0027)) {
                return reverse(acc_1);
            }
            else {
                const tail = tail_1(l$0027);
                l$0027_mut = tail;
                acc_1_mut = innerLoop(head_2(l$0027), tail, acc_1);
                continue loop;
            }
            break;
        }
    };
    return loop(l, empty());
}

export function List_applyEachPairwiseWith(f, l) {
    const innerLoop = (hh_mut, ll_mut, acc_mut) => {
        innerLoop:
        while (true) {
            const hh = hh_mut, ll = ll_mut, acc = acc_mut;
            if (isEmpty(ll)) {
                return acc;
            }
            else {
                hh_mut = hh;
                ll_mut = tail_1(ll);
                acc_mut = cons(f(hh, head_2(ll)), acc);
                continue innerLoop;
            }
            break;
        }
    };
    const loop = (l$0027_mut, acc_1_mut) => {
        loop:
        while (true) {
            const l$0027 = l$0027_mut, acc_1 = acc_1_mut;
            if (isEmpty(l$0027)) {
                return reverse(acc_1);
            }
            else {
                l$0027_mut = tail_1(l$0027);
                acc_1_mut = innerLoop(head_2(l$0027), l$0027, acc_1);
                continue loop;
            }
            break;
        }
    };
    return loop(l, empty());
}

export function List_removeAt(index, input) {
    return map((tuple_1) => tuple_1[1], filter((tuple) => tuple[0], mapIndexed((i, el) => [i !== index, el], input)));
}

export function List_insertAt(index, newEl, input) {
    return concat(mapIndexed((i, el) => {
        if (i === index) {
            return ofArray([newEl, el]);
        }
        else {
            return singleton(el);
        }
    }, input));
}

export function List_foldi(f, acc, l) {
    const loop = (i_mut, acc_1_mut, l_1_mut) => {
        loop:
        while (true) {
            const i = i_mut, acc_1 = acc_1_mut, l_1 = l_1_mut;
            if (!isEmpty(l_1)) {
                i_mut = (i + 1);
                acc_1_mut = f(i, acc_1, head_2(l_1));
                l_1_mut = tail_1(l_1);
                continue loop;
            }
            else {
                return acc_1;
            }
            break;
        }
    };
    return loop(0, acc, l);
}

export function List_countDistinctBy(keyf, list) {
    const dict = new Dictionary([], HashIdentity_Structural());
    const loop = (list_1_mut) => {
        loop:
        while (true) {
            const list_1 = list_1_mut;
            if (!isEmpty(list_1)) {
                const key = keyf(head_2(list_1));
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
                list_1_mut = tail_1(list_1);
                continue loop;
            }
            break;
        }
    };
    loop(list);
    return toList(delay(() => map_1((v_2) => [v_2[0], v_2[1].contents], dict)));
}

export function List_powerSetOf(l) {
    const loop = (l_1) => {
        if (!isEmpty(l_1)) {
            return collect((subSet) => ofArray([subSet, cons(head_2(l_1), subSet)]), loop(tail_1(l_1)));
        }
        else {
            return singleton(empty());
        }
    };
    const matchValue = loop(l);
    if (!isEmpty(matchValue)) {
        return tail_1(matchValue);
    }
    else {
        return matchValue;
    }
}

export function List_drawExaustively(l) {
    const loop = (acc_mut, l_1_mut) => {
        loop:
        while (true) {
            const acc = acc_mut, l_1 = l_1_mut;
            if (!isEmpty(l_1)) {
                acc_mut = concat(map((x) => map((y) => cons(x, y), acc), head_2(l_1)));
                l_1_mut = tail_1(l_1);
                continue loop;
            }
            else {
                return acc;
            }
            break;
        }
    };
    if (!isEmpty(l)) {
        if (isEmpty(tail_1(l))) {
            return map(singleton, head_2(l));
        }
        else {
            return loop(map(singleton, head_2(l)), tail_1(l));
        }
    }
    else {
        return empty();
    }
}

export function List_filteri(predicate, list) {
    let i = -1;
    return filter((x) => {
        i = ((i + 1) | 0);
        return predicate(i, x);
    }, list);
}

export function List_countByPredicate(predicate, list) {
    let counter = 0;
    const loop = (predicate$0027_mut, list$0027_mut) => {
        loop:
        while (true) {
            const predicate$0027 = predicate$0027_mut, list$0027 = list$0027_mut;
            if (!isEmpty(list$0027)) {
                const t = tail_1(list$0027);
                if (predicate$0027(head_2(list$0027))) {
                    counter = ((counter + 1) | 0);
                    predicate$0027_mut = predicate$0027;
                    list$0027_mut = t;
                    continue loop;
                }
                else {
                    predicate$0027_mut = predicate$0027;
                    list$0027_mut = t;
                    continue loop;
                }
            }
            break;
        }
    };
    loop(predicate, list);
    return counter | 0;
}

export function List_countiByPredicate(predicate, list) {
    let counter = 0;
    let i = -1;
    const loop = (predicate$0027_mut, list$0027_mut) => {
        loop:
        while (true) {
            const predicate$0027 = predicate$0027_mut, list$0027 = list$0027_mut;
            i = ((i + 1) | 0);
            if (!isEmpty(list$0027)) {
                const t = tail_1(list$0027);
                if (predicate$0027(i)(head_2(list$0027))) {
                    counter = ((counter + 1) | 0);
                    predicate$0027_mut = predicate$0027;
                    list$0027_mut = t;
                    continue loop;
                }
                else {
                    predicate$0027_mut = predicate$0027;
                    list$0027_mut = t;
                    continue loop;
                }
            }
            break;
        }
    };
    loop(curry2(predicate), list);
    return counter | 0;
}

export function List_choosei(predicate, list) {
    let i = -1;
    const loop = (predicate$0027_mut, list$0027_mut, outputList_mut) => {
        loop:
        while (true) {
            const predicate$0027 = predicate$0027_mut, list$0027 = list$0027_mut, outputList = outputList_mut;
            if (!isEmpty(list$0027)) {
                const t = tail_1(list$0027);
                i = ((i + 1) | 0);
                const matchValue = predicate$0027(i)(head_2(list$0027));
                if (matchValue == null) {
                    predicate$0027_mut = predicate$0027;
                    list$0027_mut = t;
                    outputList_mut = outputList;
                    continue loop;
                }
                else {
                    predicate$0027_mut = predicate$0027;
                    list$0027_mut = t;
                    outputList_mut = cons(value_1(matchValue), outputList);
                    continue loop;
                }
            }
            else {
                return outputList;
            }
            break;
        }
    };
    return reverse(loop(curry2(predicate), list, empty()));
}

export function List_findIndicesBack(predicate, list) {
    let i = -1;
    const loop = (predicate$0027_mut, list$0027_mut, outputList_mut) => {
        loop:
        while (true) {
            const predicate$0027 = predicate$0027_mut, list$0027 = list$0027_mut, outputList = outputList_mut;
            if (!isEmpty(list$0027)) {
                const t = tail_1(list$0027);
                i = ((i + 1) | 0);
                if (predicate$0027(head_2(list$0027))) {
                    predicate$0027_mut = predicate$0027;
                    list$0027_mut = t;
                    outputList_mut = cons(i, outputList);
                    continue loop;
                }
                else {
                    predicate$0027_mut = predicate$0027;
                    list$0027_mut = t;
                    outputList_mut = outputList;
                    continue loop;
                }
            }
            else {
                return outputList;
            }
            break;
        }
    };
    return loop(predicate, list, empty());
}

export function List_findIndices(predicate, list) {
    return reverse(List_findIndicesBack(predicate, list));
}

export function List_takeNth(n, list) {
    return List_filteri((i, _arg) => (((i + 1) % n) === 0), list);
}

export function List_skipNth(n, list) {
    return List_filteri((i, _arg) => (((i + 1) % n) !== 0), list);
}

export function List_groupWhen(f, list) {
    return reverse(map(reverse, fold((acc, e) => {
        const matchValue = f(e);
        if (matchValue) {
            return cons(singleton(e), acc);
        }
        else if (!isEmpty(acc)) {
            return cons(cons(e, head_2(acc)), tail_1(acc));
        }
        else {
            return singleton(singleton(e));
        }
    }, empty(), list)));
}

export function List_intersect(list1, list2) {
    const patternInput = (length(list1) >= length(list2)) ? [list2, list1] : [list1, list2];
    const hsSl = new HashSet([], HashIdentity_Structural());
    iterate((arg_1) => {
        addToSet(arg_1, hsSl);
    }, patternInput[0]);
    intersectWith(hsSl, patternInput[1]);
    return ofSeq(hsSl);
}

export function Microsoft_FSharp_Collections_FSharpList$1__List$1_iterWhile_Static(f, ls) {
    const iterLoop = (f_1_mut, ls_1_mut) => {
        iterLoop:
        while (true) {
            const f_1 = f_1_mut, ls_1 = ls_1_mut;
            if (!isEmpty(ls_1)) {
                if (f_1(head_2(ls_1))) {
                    f_1_mut = f_1;
                    ls_1_mut = tail_1(ls_1);
                    continue iterLoop;
                }
            }
            break;
        }
    };
    iterLoop(f, ls);
}

