import { Record } from "../../fable_modules/fable-library.4.0.6/Types.js";
import { record_type, class_type, list_type } from "../../fable_modules/fable-library.4.0.6/Reflection.js";
import { reverse, iterate, map as map_1, exists, foldBack, cons, tail, head, isEmpty, empty as empty_1 } from "../../fable_modules/fable-library.4.0.6/List.js";
import { iterate as iterate_1, map, tryFind, add as add_1, empty as empty_2 } from "../../fable_modules/fable-library.4.0.6/Map.js";
import { equals, compare } from "../../fable_modules/fable-library.4.0.6/Util.js";
import { concat, singleton, append, collect, delay } from "../../fable_modules/fable-library.4.0.6/Seq.js";
import { printf, toConsole } from "../../fable_modules/fable-library.4.0.6/String.js";

export class GenericNode$2 extends Record {
    constructor(Member, Children) {
        super();
        this.Member = Member;
        this.Children = Children;
    }
}

export function GenericNode$2$reflection(gen0, gen1) {
    return record_type("FSharpAux.Tree.GenericNode`2", [gen0, gen1], GenericNode$2, () => [["Member", list_type(gen1)], ["Children", class_type("Microsoft.FSharp.Collections.FSharpMap`2", [gen0, GenericNode$2$reflection(gen0, gen1)])]]);
}

export function empty() {
    return new GenericNode$2(empty_1(), empty_2({
        Compare: compare,
    }));
}

export function add(keyPath, item, tree) {
    let matchValue;
    if (!isEmpty(keyPath)) {
        const k = head(keyPath);
        return new GenericNode$2(tree.Member, add_1(k, add(tail(keyPath), item, (matchValue = tryFind(k, tree.Children), (matchValue == null) ? empty() : matchValue)), tree.Children));
    }
    else {
        return new GenericNode$2(cons(item, tree.Member), tree.Children);
    }
}

export function createTree(keyItemList) {
    return foldBack((tupledArg, acc) => add(tupledArg[0], tupledArg[1], acc), keyItemList, empty());
}

export function contains(keyPath_mut, item_mut, tree_mut) {
    contains:
    while (true) {
        const keyPath = keyPath_mut, item = item_mut, tree = tree_mut;
        if (!isEmpty(keyPath)) {
            const matchValue = tryFind(head(keyPath), tree.Children);
            if (matchValue == null) {
                return false;
            }
            else {
                keyPath_mut = tail(keyPath);
                item_mut = item;
                tree_mut = matchValue;
                continue contains;
            }
        }
        else {
            return exists((i) => equals(i, item), tree.Member);
        }
        break;
    }
}

export function mapMember(f, tree) {
    const c = map((_arg, nodes) => mapMember(f, nodes), tree.Children);
    return new GenericNode$2(map_1(f, tree.Member), c);
}

export function toMemberSeq(tree) {
    return delay(() => (isEmpty(tree.Member) ? collect((child) => toMemberSeq(child[1]), tree.Children) : append(singleton(tree.Member), delay(() => collect((child_1) => toMemberSeq(child_1[1]), tree.Children)))));
}

export function getDeepMemberSeqByKeyPath(keyPath_mut, tree_mut) {
    getDeepMemberSeqByKeyPath:
    while (true) {
        const keyPath = keyPath_mut, tree = tree_mut;
        if (!isEmpty(keyPath)) {
            const matchValue = tryFind(head(keyPath), tree.Children);
            if (matchValue == null) {
                return [];
            }
            else {
                keyPath_mut = tail(keyPath);
                tree_mut = matchValue;
                continue getDeepMemberSeqByKeyPath;
            }
        }
        else {
            return concat(toMemberSeq(tree));
        }
        break;
    }
}

export function print(tree) {
    const printLoop = (depth, key, tree_1) => {
        iterate((data) => {
            for (let i = 0; i <= depth; i++) {
                toConsole(printf(" -> "));
            }
            const arg = reverse(key);
            toConsole(printf("%A : %A"))(arg)(data);
        }, tree_1.Member);
        iterate_1((key$0027, nodes) => {
            printLoop(depth + 1, cons(key$0027, key), nodes);
        }, tree_1.Children);
    };
    printLoop(0, empty_1(), tree);
}

