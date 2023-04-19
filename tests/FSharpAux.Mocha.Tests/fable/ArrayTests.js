import { Expect_notEqual, Test_testCase, Test_testList } from "./fable_modules/Fable.Mocha.2.16.0/Mocha.fs.js";
import { intersect, groupWhen, skipNth, takeNth, findIndicesBack, findIndices, choosei, countiByPredicate, countByPredicate, filteri } from "./src/FSharpAux.Core/Array.js";
import { equalsWith } from "./fable_modules/fable-library.4.0.6/Array.js";
import { equals as equals_1, int32ToString, structuralHash, assertEqual } from "./fable_modules/fable-library.4.0.6/Util.js";
import { ofArray, contains } from "./fable_modules/fable-library.4.0.6/List.js";
import { obj_type, equals, class_type, decimal_type, string_type, float64_type, bool_type, array_type, int32_type } from "./fable_modules/fable-library.4.0.6/Reflection.js";
import { seqToString } from "./fable_modules/fable-library.4.0.6/Types.js";
import { printf, toText } from "./fable_modules/fable-library.4.0.6/String.js";
import { singleton, append, delay, toList } from "./fable_modules/fable-library.4.0.6/Seq.js";

const testArray1 = new Int32Array([1337, 14, 23, 23, 69, 1, 2, 3, 1000, 9001, 23]);

const testArray2 = new Int32Array([3, 3, 2, 4, 2, 1]);

const testArray3 = new Int32Array([6, 6, 2, 4, 2, 8]);

const testArray1_filteri_Equal = new Int32Array([14, 23, 23, 69]);

const testArray1_filteri_NotEqual = new Int32Array([1337, 14, 23]);

const testArray1_choosei_Equal = new Float64Array([14, 23, 23, 69]);

const testArray1_choosei_NotEqual = new Float64Array([1337, 14, 23]);

const testArray1_findIndices_Equal = new Int32Array([1, 2, 3, 4, 5, 6, 7, 10]);

const testArray1_findIndices_NotEqual = new Int32Array([5, 6, 7, 10]);

const testArray1_findIndicesBack_Equal = new Int32Array([10, 7, 6, 5, 4, 3, 2, 1]);

const testArray1_findIndicesBack_NotEqual = new Int32Array([5, 6, 7, 10]);

const testArray1_takeNth_Equal = new Int32Array([23, 1, 1000]);

const testArray1_takeNth_NotEqual = new Int32Array([5, 6, 7, 10]);

const testArray1_skipNth_Equal = new Int32Array([1337, 14, 23, 69, 2, 3, 9001, 23]);

const testArray1_skipNth_NotEqual = new Int32Array([5, 6, 7, 10]);

const testArray1_groupWhen_Equal = [new Int32Array([1337, 14]), new Int32Array([23]), new Int32Array([23]), new Int32Array([69]), new Int32Array([1, 2]), new Int32Array([3, 1000]), new Int32Array([9001]), new Int32Array([23])];

const testArray1_groupWhen_NotEqual = [new Int32Array([1337, 14]), new Int32Array([23]), new Int32Array([23]), new Int32Array([69]), new Int32Array([1, 2]), new Int32Array([3, 1000]), new Int32Array([9001, 23])];

export const arrayTests = Test_testList("ArrayTests", ofArray([Test_testList("Array.filteri", ofArray([Test_testCase("returns correct array", () => {
    let copyOfStruct, arg, arg_1;
    const actual = filteri((i, t) => {
        if (i < 5) {
            return t < 100;
        }
        else {
            return false;
        }
    }, testArray1);
    const expected = testArray1_filteri_Equal;
    if (equalsWith((x, y) => (x === y), actual, expected) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual, expected, "Array.filteri did return correct array");
    }
    else {
        throw new Error(contains((copyOfStruct = actual, array_type(int32_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg = seqToString(expected), (arg_1 = seqToString(actual), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg)(arg_1)("Array.filteri did return correct array")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected)(actual)("Array.filteri did return correct array"));
    }
}), Test_testCase("does not return incorrect array", () => {
    Expect_notEqual(filteri((i_1, t_1) => {
        if (i_1 < 5) {
            return t_1 < 100;
        }
        else {
            return false;
        }
    }, testArray1), testArray1_filteri_NotEqual, "Array.filteri did not return incorrect array");
})])), Test_testList("Array.countByPredicate", ofArray([Test_testCase("returns correct array", () => {
    let copyOfStruct_1, arg_6, arg_1_1;
    const actual_1 = countByPredicate((t_2) => (t_2 < 100), testArray1) | 0;
    if ((actual_1 === 8) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_1, 8, "Array.countByPredicate did return correct integer");
    }
    else {
        throw new Error(contains((copyOfStruct_1 = actual_1, int32_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_6 = int32ToString(8), (arg_1_1 = int32ToString(actual_1), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_6)(arg_1_1)("Array.countByPredicate did return correct integer")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(8)(actual_1)("Array.countByPredicate did return correct integer"));
    }
}), Test_testCase("does not return incorrect array", () => {
    Expect_notEqual(countByPredicate((t_3) => (t_3 < 100), testArray1), 5, "Array.countByPredicate did not return incorrect integer");
})])), Test_testList("Array.countiByPredicate", ofArray([Test_testCase("returns correct array", () => {
    let copyOfStruct_2, arg_7, arg_1_2;
    const actual_2 = countiByPredicate((i_2, t_4) => {
        if (i_2 < 5) {
            return t_4 < 100;
        }
        else {
            return false;
        }
    }, testArray1) | 0;
    if ((actual_2 === 4) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_2, 4, "Array.countiByPredicate did return correct integer");
    }
    else {
        throw new Error(contains((copyOfStruct_2 = actual_2, int32_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_7 = int32ToString(4), (arg_1_2 = int32ToString(actual_2), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_7)(arg_1_2)("Array.countiByPredicate did return correct integer")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(4)(actual_2)("Array.countiByPredicate did return correct integer"));
    }
}), Test_testCase("does not return incorrect array", () => {
    Expect_notEqual(countiByPredicate((i_3, t_5) => {
        if (i_3 < 5) {
            return t_5 < 100;
        }
        else {
            return false;
        }
    }, testArray1), 1, "Array.countiByPredicate did not return incorrect integer");
})])), Test_testList("Array.choosei", ofArray([Test_testCase("returns correct array", () => {
    let copyOfStruct_3, arg_8, arg_1_3;
    const actual_3 = choosei((i_4, t_6) => {
        if ((i_4 < 5) && (t_6 < 100)) {
            return t_6;
        }
        else {
            return void 0;
        }
    }, testArray1);
    const expected_3 = testArray1_choosei_Equal;
    if (equalsWith((x_4, y_4) => (x_4 === y_4), actual_3, expected_3) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_3, expected_3, "Array.choosei did return correct array");
    }
    else {
        throw new Error(contains((copyOfStruct_3 = actual_3, array_type(float64_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_8 = seqToString(expected_3), (arg_1_3 = seqToString(actual_3), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_8)(arg_1_3)("Array.choosei did return correct array")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_3)(actual_3)("Array.choosei did return correct array"));
    }
}), Test_testCase("does not return incorrect array", () => {
    Expect_notEqual(choosei((i_5, t_7) => {
        if ((i_5 < 5) && (t_7 < 100)) {
            return t_7;
        }
        else {
            return void 0;
        }
    }, testArray1), testArray1_choosei_NotEqual, "Array.choosei did not return incorrect array");
})])), Test_testList("Array.findIndices", ofArray([Test_testCase("returns correct array", () => {
    let copyOfStruct_4, arg_9, arg_1_4;
    const actual_4 = findIndices((t_8) => (t_8 < 100), testArray1);
    const expected_4 = testArray1_findIndices_Equal;
    if (equalsWith((x_6, y_6) => (x_6 === y_6), actual_4, expected_4) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_4, expected_4, "Array.findIndices did return correct array");
    }
    else {
        throw new Error(contains((copyOfStruct_4 = actual_4, array_type(int32_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_9 = seqToString(expected_4), (arg_1_4 = seqToString(actual_4), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_9)(arg_1_4)("Array.findIndices did return correct array")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_4)(actual_4)("Array.findIndices did return correct array"));
    }
}), Test_testCase("does not return incorrect array", () => {
    Expect_notEqual(findIndices((t_9) => (t_9 < 100), testArray1), testArray1_findIndices_NotEqual, "Array.findIndices did not return incorrect array");
})])), Test_testList("Array.findIndicesBack", ofArray([Test_testCase("returns correct array", () => {
    let copyOfStruct_5, arg_10, arg_1_5;
    const actual_5 = findIndicesBack((t_10) => (t_10 < 100), testArray1);
    const expected_5 = testArray1_findIndicesBack_Equal;
    if (equalsWith((x_8, y_8) => (x_8 === y_8), actual_5, expected_5) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_5, expected_5, "Array.findIndicesBack did return correct array");
    }
    else {
        throw new Error(contains((copyOfStruct_5 = actual_5, array_type(int32_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_10 = seqToString(expected_5), (arg_1_5 = seqToString(actual_5), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_10)(arg_1_5)("Array.findIndicesBack did return correct array")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_5)(actual_5)("Array.findIndicesBack did return correct array"));
    }
}), Test_testCase("does not return incorrect array", () => {
    Expect_notEqual(findIndicesBack((t_11) => (t_11 < 100), testArray1), testArray1_findIndicesBack_NotEqual, "Array.findIndicesBack did not return incorrect array");
})])), Test_testList("Array.takeNth", ofArray([Test_testCase("returns correct array", () => {
    let copyOfStruct_6, arg_11, arg_1_6;
    const actual_6 = takeNth(3, testArray1);
    const expected_6 = testArray1_takeNth_Equal;
    if (equalsWith((x_10, y_10) => (x_10 === y_10), actual_6, expected_6) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_6, expected_6, "Array.takeNth did return correct array");
    }
    else {
        throw new Error(contains((copyOfStruct_6 = actual_6, array_type(int32_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_11 = seqToString(expected_6), (arg_1_6 = seqToString(actual_6), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_11)(arg_1_6)("Array.takeNth did return correct array")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_6)(actual_6)("Array.takeNth did return correct array"));
    }
}), Test_testCase("does not return incorrect array", () => {
    Expect_notEqual(takeNth(3, testArray1), testArray1_takeNth_NotEqual, "Array.takeNth did not return incorrect array");
})])), Test_testList("Array.skipNth", ofArray([Test_testCase("returns correct array", () => {
    let copyOfStruct_7, arg_12, arg_1_7;
    const actual_7 = skipNth(3, testArray1);
    const expected_7 = testArray1_skipNth_Equal;
    if (equalsWith((x_12, y_12) => (x_12 === y_12), actual_7, expected_7) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_7, expected_7, "Array.skipNth did return correct array");
    }
    else {
        throw new Error(contains((copyOfStruct_7 = actual_7, array_type(int32_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_12 = seqToString(expected_7), (arg_1_7 = seqToString(actual_7), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_12)(arg_1_7)("Array.skipNth did return correct array")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_7)(actual_7)("Array.skipNth did return correct array"));
    }
}), Test_testCase("does not return incorrect array", () => {
    Expect_notEqual(skipNth(3, testArray1), testArray1_skipNth_NotEqual, "Array.skipNth did not return incorrect array");
})])), Test_testList("Array.groupWhen", toList(delay(() => {
    const isOdd = (n_4) => ((n_4 % 2) !== 0);
    return append(singleton(Test_testCase("returns correct jagged array", () => {
        let copyOfStruct_8, arg_13, arg_1_8;
        const actual_8 = groupWhen(isOdd, testArray1);
        const expected_8 = testArray1_groupWhen_Equal;
        if (equalsWith((x_14, y_14) => equalsWith((x_15, y_15) => (x_15 === y_15), x_14, y_14), actual_8, expected_8) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
            assertEqual(actual_8, expected_8, "Array.groupWhen did return correct jagged array");
        }
        else {
            throw new Error(contains((copyOfStruct_8 = actual_8, array_type(array_type(int32_type))), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
                Equals: equals,
                GetHashCode: structuralHash,
            }) ? ((arg_13 = seqToString(expected_8), (arg_1_8 = seqToString(actual_8), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_13)(arg_1_8)("Array.groupWhen did return correct jagged array")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_8)(actual_8)("Array.groupWhen did return correct jagged array"));
        }
    })), delay(() => singleton(Test_testCase("does not return incorrect jagged array", () => {
        Expect_notEqual(groupWhen(isOdd, testArray1), testArray1_groupWhen_NotEqual, "Array.groupWhen did not return incorrect jagged array");
        Expect_notEqual(groupWhen(isOdd, testArray1), [], "Array.groupWhen did not return empty jagged array");
    }))));
}))), Test_testList("Array.intersect", ofArray([Test_testCase("returns correct array, case1: [||]", () => {
    let copyOfStruct_9, arg_14, arg_1_9;
    const actual_9 = intersect(new Array(0), new Array(0));
    const expected_9 = [];
    if (equalsWith(equals_1, actual_9, expected_9) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_9, expected_9, "Array.intersect did not return empty array");
    }
    else {
        throw new Error(contains((copyOfStruct_9 = actual_9, array_type(obj_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_14 = seqToString(expected_9), (arg_1_9 = seqToString(actual_9), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_14)(arg_1_9)("Array.intersect did not return empty array")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_9)(actual_9)("Array.intersect did not return empty array"));
    }
}), Test_testCase("returns correct array, case2: [||]", () => {
    let copyOfStruct_10, arg_15, arg_1_10;
    const actual_10 = intersect(new Int32Array(0), testArray3);
    const expected_10 = new Int32Array([]);
    if (equalsWith((x_19, y_19) => (x_19 === y_19), actual_10, expected_10) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_10, expected_10, "Array.intersect did not return empty array");
    }
    else {
        throw new Error(contains((copyOfStruct_10 = actual_10, array_type(int32_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_15 = seqToString(expected_10), (arg_1_10 = seqToString(actual_10), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_15)(arg_1_10)("Array.intersect did not return empty array")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_10)(actual_10)("Array.intersect did not return empty array"));
    }
}), Test_testCase("returns correct array, case3: [||]", () => {
    let copyOfStruct_11, arg_16, arg_1_11;
    const actual_11 = intersect(testArray2, new Int32Array(0));
    const expected_11 = new Int32Array([]);
    if (equalsWith((x_21, y_21) => (x_21 === y_21), actual_11, expected_11) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_11, expected_11, "Array.intersect did not return empty array");
    }
    else {
        throw new Error(contains((copyOfStruct_11 = actual_11, array_type(int32_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_16 = seqToString(expected_11), (arg_1_11 = seqToString(actual_11), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_16)(arg_1_11)("Array.intersect did not return empty array")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_11)(actual_11)("Array.intersect did not return empty array"));
    }
}), Test_testCase("returns correct array, case4: [|2; 4|]", () => {
    let copyOfStruct_12, arg_17, arg_1_12;
    const actual_12 = intersect(testArray2, testArray3);
    const expected_12 = new Int32Array([2, 4]);
    if (equalsWith((x_23, y_23) => (x_23 === y_23), actual_12, expected_12) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_12, expected_12, "Array.intersect did not return correct array");
    }
    else {
        throw new Error(contains((copyOfStruct_12 = actual_12, array_type(int32_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_17 = seqToString(expected_12), (arg_1_12 = seqToString(actual_12), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_17)(arg_1_12)("Array.intersect did not return correct array")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_12)(actual_12)("Array.intersect did not return correct array"));
    }
})]))]));

