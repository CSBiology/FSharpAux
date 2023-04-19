import { intersect as intersect_1, union as union_1, difference } from "../../fable_modules/fable-library.4.0.6/Set.js";

export function symmetricDifference(set1, set2) {
    return difference(union_1(set1, set2), intersect_1(set1, set2));
}

