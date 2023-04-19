import { Union } from "../../fable_modules/fable-library.4.0.6/Types.js";
import { union_type, int32_type } from "../../fable_modules/fable-library.4.0.6/Reflection.js";
import { sortInPlaceBy } from "../../fable_modules/fable-library.4.0.6/Array.js";
import { comparePrimitives, compare } from "../../fable_modules/fable-library.4.0.6/Util.js";
import { cons, empty, singleton, append } from "../../fable_modules/fable-library.4.0.6/List.js";
import { append as append_1, singleton as singleton_1, delay } from "../../fable_modules/fable-library.4.0.6/Seq.js";
import { printf, toText } from "../../fable_modules/fable-library.4.0.6/String.js";

export class KdTree$2 extends Union {
    constructor(tag, fields) {
        super();
        this.tag = tag;
        this.fields = fields;
    }
    cases() {
        return ["Node", "Leaf"];
    }
}

export function KdTree$2$reflection(gen0, gen1) {
    return union_type("FSharpAux.KdTree.KdTree`2", [gen0, gen1], KdTree$2, () => [[["Item1", KdTree$2$reflection(gen0, gen1)], ["Item2", KdTree$2$reflection(gen0, gen1)], ["Item3", gen1], ["Item4", int32_type], ["Item5", gen0]], []]);
}

export function ofArrayBy(converter, getCoordinateByPos, k, arr) {
    const loop = (data, d, k_1) => {
        if (data.length === 0) {
            return new KdTree$2(1, []);
        }
        else {
            const axis = (d % k_1) | 0;
            sortInPlaceBy((item) => getCoordinateByPos(converter(item), axis), data, {
                Compare: compare,
            });
            const halfIndex = ~~(data.length / 2) | 0;
            if (halfIndex === 0) {
                const dataPoint = data[halfIndex];
                return new KdTree$2(0, [new KdTree$2(1, []), new KdTree$2(1, []), converter(dataPoint), d, dataPoint]);
            }
            else {
                const dataPoint_1 = data[halfIndex];
                const matchValue_1 = data.slice(0, (halfIndex - 1) + 1);
                const matchValue_2 = data.slice(halfIndex + 1, data.length);
                return new KdTree$2(0, [loop(matchValue_1, d + 1, k_1), loop(matchValue_2, d + 1, k_1), converter(dataPoint_1), d, dataPoint_1]);
            }
        }
    };
    return loop(arr, 0, k);
}

export function toList(tree) {
    if (tree.tag === 0) {
        return append(toList(tree.fields[0]), append(singleton(tree.fields[4]), toList(tree.fields[1])));
    }
    else {
        return empty();
    }
}

export function print(kdTree) {
    const printLoop = (c) => delay(() => ((c.tag === 1) ? singleton_1(toText(printf(""))) : append_1(singleton_1(toText(printf("\"Depth\" : %i \"Position\" : %A \"Data\" : %A, \n"))(c.fields[3])(c.fields[2])(c.fields[4])), delay(() => append_1(printLoop(c.fields[0]), delay(() => printLoop(c.fields[1])))))));
    return printLoop(kdTree);
}

export function rangeSearch(distance, getCoordinateByPos, tree, radius, queryCoordinates) {
    const loop = (tree_1_mut, acc_mut) => {
        loop:
        while (true) {
            const tree_1 = tree_1_mut, acc = acc_mut;
            if (tree_1.tag === 0) {
                const cRight = tree_1.fields[1];
                const cLeft = tree_1.fields[0];
                const cK = tree_1.fields[3] | 0;
                const cCoor = tree_1.fields[2];
                const acc$0027 = (distance(cCoor, queryCoordinates) <= radius) ? cons(tree_1.fields[4], acc) : acc;
                const axis = getCoordinateByPos(queryCoordinates, cK);
                const cAxis = getCoordinateByPos(cCoor, cK);
                if (comparePrimitives(axis, cAxis) < 0) {
                    if (Math.abs(axis - cAxis) <= radius) {
                        tree_1_mut = cLeft;
                        acc_mut = loop(cRight, acc$0027);
                        continue loop;
                    }
                    else {
                        tree_1_mut = cLeft;
                        acc_mut = acc$0027;
                        continue loop;
                    }
                }
                else if (Math.abs(axis - cAxis) <= radius) {
                    tree_1_mut = cRight;
                    acc_mut = loop(cLeft, acc$0027);
                    continue loop;
                }
                else {
                    tree_1_mut = cRight;
                    acc_mut = acc$0027;
                    continue loop;
                }
            }
            else {
                return acc;
            }
            break;
        }
    };
    return loop(tree, empty());
}

