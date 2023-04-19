import { empty, contains, singleton, ofArray } from "./fable_modules/fable-library.4.0.6/List.js";
import { Expect_notEqual, Test_testCase, Test_testList } from "./fable_modules/Fable.Mocha.2.16.0/Mocha.fs.js";
import { List_intersect, List_groupWhen, List_skipNth, List_takeNth, List_findIndicesBack, List_findIndices, List_choosei, List_countiByPredicate, List_countByPredicate, List_filteri } from "./src/FSharpAux.Core/List.js";
import { int32ToString, structuralHash, assertEqual, equals } from "./fable_modules/fable-library.4.0.6/Util.js";
import { obj_type, equals as equals_1, class_type, decimal_type, string_type, float64_type, bool_type, list_type, int32_type } from "./fable_modules/fable-library.4.0.6/Reflection.js";
import { seqToString } from "./fable_modules/fable-library.4.0.6/Types.js";
import { printf, toText } from "./fable_modules/fable-library.4.0.6/String.js";
import { singleton as singleton_1, append, delay, toList } from "./fable_modules/fable-library.4.0.6/Seq.js";

const testList1 = ofArray([1337, 14, 23, 23, 69, 1, 2, 3, 1000, 9001, 23]);

const testList2 = ofArray([3, 3, 2, 4, 1, 2]);

const testList3 = ofArray([3, 3, 2, 4, 1, 1]);

const testList4 = ofArray([3, 3, 2, 4, 2, 2]);

const testList5 = ofArray([3, 3, 2, 4, 2, 1]);

const testList6 = ofArray([6, 6, 2, 4, 2, 8]);

const testList1_filteri_Equal = ofArray([14, 23, 23, 69]);

const testList1_filteri_NotEqual = ofArray([1337, 14, 23]);

const testList1_choosei_Equal = ofArray([14, 23, 23, 69]);

const testList1_choosei_NotEqual = ofArray([1337, 14, 23]);

const testList1_findIndices_Equal = ofArray([1, 2, 3, 4, 5, 6, 7, 10]);

const testList1_findIndices_NotEqual = ofArray([5, 6, 7, 10]);

const testList1_findIndicesBack_Equal = ofArray([10, 7, 6, 5, 4, 3, 2, 1]);

const testList1_findIndicesBack_NotEqual = ofArray([5, 6, 7, 10]);

const testList1_takeNth_Equal = ofArray([23, 1, 1000]);

const testList1_takeNth_NotEqual = ofArray([5, 6, 7, 10]);

const testList1_skipNth_Equal = ofArray([1337, 14, 23, 69, 2, 3, 9001, 23]);

const testList1_skipNth_NotEqual = ofArray([5, 6, 7, 10]);

const testList1_groupWhen_Equal = ofArray([ofArray([1337, 14]), singleton(23), singleton(23), singleton(69), ofArray([1, 2]), ofArray([3, 1000]), singleton(9001), singleton(23)]);

const testList1_groupWhen_NotEqual = ofArray([ofArray([1337, 14]), singleton(23), singleton(23), singleton(69), ofArray([1, 2]), ofArray([3, 1000]), ofArray([9001, 23])]);

const testList2_groupWhen_Equal = ofArray([singleton(3), ofArray([3, 2, 4]), ofArray([1, 2])]);

const testList2_groupWhen_NotEqual = ofArray([singleton(3), ofArray([3, 2, 4]), singleton(1), singleton(2)]);

const testList3_groupWhen_Equal = ofArray([singleton(3), ofArray([3, 2, 4]), singleton(1), singleton(1)]);

const testList3_groupWhen_NotEqual = ofArray([singleton(3), ofArray([3, 2, 4]), ofArray([1, 1])]);

const testList4_groupWhen_Equal = ofArray([singleton(3), ofArray([3, 2, 4, 2, 2])]);

const testList4_groupWhen_NotEqual = ofArray([singleton(3), ofArray([3, 2, 4, 2]), singleton(2)]);

const testList5_groupWhen_Equal = ofArray([singleton(3), ofArray([3, 2, 4, 2]), singleton(1)]);

const testList5_groupWhen_NotEqual = ofArray([singleton(3), ofArray([3, 2, 4, 2, 1])]);

export const listTests = Test_testList("ListTests", ofArray([Test_testList("List.filteri", ofArray([Test_testCase("returns correct list", () => {
    let copyOfStruct, arg, arg_1;
    const actual = List_filteri((i, t) => {
        if (i < 5) {
            return t < 100;
        }
        else {
            return false;
        }
    }, testList1);
    const expected = testList1_filteri_Equal;
    if (equals(actual, expected) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual, expected, "List.filteri did return correct List");
    }
    else {
        throw new Error(contains((copyOfStruct = actual, list_type(int32_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals_1,
            GetHashCode: structuralHash,
        }) ? ((arg = seqToString(expected), (arg_1 = seqToString(actual), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg)(arg_1)("List.filteri did return correct List")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected)(actual)("List.filteri did return correct List"));
    }
}), Test_testCase("does not return incorrect list", () => {
    Expect_notEqual(List_filteri((i_1, t_1) => {
        if (i_1 < 5) {
            return t_1 < 100;
        }
        else {
            return false;
        }
    }, testList1), testList1_filteri_NotEqual, "List.filteri did not return incorrect List");
})])), Test_testList("List.countByPredicate", ofArray([Test_testCase("returns correct list", () => {
    let copyOfStruct_1, arg_6, arg_1_1;
    const actual_1 = List_countByPredicate((t_2) => (t_2 < 100), testList1) | 0;
    if ((actual_1 === 8) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_1, 8, "List.countByPredicate did return correct integer");
    }
    else {
        throw new Error(contains((copyOfStruct_1 = actual_1, int32_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals_1,
            GetHashCode: structuralHash,
        }) ? ((arg_6 = int32ToString(8), (arg_1_1 = int32ToString(actual_1), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_6)(arg_1_1)("List.countByPredicate did return correct integer")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(8)(actual_1)("List.countByPredicate did return correct integer"));
    }
}), Test_testCase("does not return incorrect list", () => {
    Expect_notEqual(List_countByPredicate((t_3) => (t_3 < 100), testList1), 5, "List.countByPredicate did not return incorrect integer");
})])), Test_testList("List.countiByPredicate", ofArray([Test_testCase("returns correct list", () => {
    let copyOfStruct_2, arg_7, arg_1_2;
    const actual_2 = List_countiByPredicate((i_2, t_4) => {
        if (i_2 < 5) {
            return t_4 < 100;
        }
        else {
            return false;
        }
    }, testList1) | 0;
    if ((actual_2 === 4) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_2, 4, "List.countiByPredicate did return correct integer");
    }
    else {
        throw new Error(contains((copyOfStruct_2 = actual_2, int32_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals_1,
            GetHashCode: structuralHash,
        }) ? ((arg_7 = int32ToString(4), (arg_1_2 = int32ToString(actual_2), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_7)(arg_1_2)("List.countiByPredicate did return correct integer")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(4)(actual_2)("List.countiByPredicate did return correct integer"));
    }
}), Test_testCase("does not return incorrect list", () => {
    Expect_notEqual(List_countiByPredicate((i_3, t_5) => {
        if (i_3 < 5) {
            return t_5 < 100;
        }
        else {
            return false;
        }
    }, testList1), 1, "List.countiByPredicate did not return incorrect integer");
})])), Test_testList("List.choosei", ofArray([Test_testCase("returns correct list", () => {
    let copyOfStruct_3, arg_8, arg_1_3;
    const actual_3 = List_choosei((i_4, t_6) => {
        if ((i_4 < 5) && (t_6 < 100)) {
            return t_6;
        }
        else {
            return void 0;
        }
    }, testList1);
    const expected_3 = testList1_choosei_Equal;
    if (equals(actual_3, expected_3) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_3, expected_3, "List.choosei did return correct List");
    }
    else {
        throw new Error(contains((copyOfStruct_3 = actual_3, list_type(float64_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals_1,
            GetHashCode: structuralHash,
        }) ? ((arg_8 = seqToString(expected_3), (arg_1_3 = seqToString(actual_3), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_8)(arg_1_3)("List.choosei did return correct List")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_3)(actual_3)("List.choosei did return correct List"));
    }
}), Test_testCase("does not return incorrect list", () => {
    Expect_notEqual(List_choosei((i_5, t_7) => {
        if ((i_5 < 5) && (t_7 < 100)) {
            return t_7;
        }
        else {
            return void 0;
        }
    }, testList1), testList1_choosei_NotEqual, "List.choosei did not return incorrect List");
})])), Test_testList("List.findIndices", ofArray([Test_testCase("returns correct list", () => {
    let copyOfStruct_4, arg_9, arg_1_4;
    const actual_4 = List_findIndices((t_8) => (t_8 < 100), testList1);
    const expected_4 = testList1_findIndices_Equal;
    if (equals(actual_4, expected_4) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_4, expected_4, "List.findIndices did return correct List");
    }
    else {
        throw new Error(contains((copyOfStruct_4 = actual_4, list_type(int32_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals_1,
            GetHashCode: structuralHash,
        }) ? ((arg_9 = seqToString(expected_4), (arg_1_4 = seqToString(actual_4), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_9)(arg_1_4)("List.findIndices did return correct List")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_4)(actual_4)("List.findIndices did return correct List"));
    }
}), Test_testCase("does not return incorrect list", () => {
    Expect_notEqual(List_findIndices((t_9) => (t_9 < 100), testList1), testList1_findIndices_NotEqual, "List.findIndices did not return incorrect List");
})])), Test_testList("List.findIndicesBack", ofArray([Test_testCase("returns correct list", () => {
    let copyOfStruct_5, arg_10, arg_1_5;
    const actual_5 = List_findIndicesBack((t_10) => (t_10 < 100), testList1);
    const expected_5 = testList1_findIndicesBack_Equal;
    if (equals(actual_5, expected_5) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_5, expected_5, "List.findIndicesBack did return correct List");
    }
    else {
        throw new Error(contains((copyOfStruct_5 = actual_5, list_type(int32_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals_1,
            GetHashCode: structuralHash,
        }) ? ((arg_10 = seqToString(expected_5), (arg_1_5 = seqToString(actual_5), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_10)(arg_1_5)("List.findIndicesBack did return correct List")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_5)(actual_5)("List.findIndicesBack did return correct List"));
    }
}), Test_testCase("does not return incorrect list", () => {
    Expect_notEqual(List_findIndicesBack((t_11) => (t_11 < 100), testList1), testList1_findIndicesBack_NotEqual, "List.findIndicesBack did not return incorrect List");
})])), Test_testList("List.takeNth", ofArray([Test_testCase("returns correct list", () => {
    let copyOfStruct_6, arg_11, arg_1_6;
    const actual_6 = List_takeNth(3, testList1);
    const expected_6 = testList1_takeNth_Equal;
    if (equals(actual_6, expected_6) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_6, expected_6, "List.takeNth did return correct List");
    }
    else {
        throw new Error(contains((copyOfStruct_6 = actual_6, list_type(int32_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals_1,
            GetHashCode: structuralHash,
        }) ? ((arg_11 = seqToString(expected_6), (arg_1_6 = seqToString(actual_6), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_11)(arg_1_6)("List.takeNth did return correct List")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_6)(actual_6)("List.takeNth did return correct List"));
    }
}), Test_testCase("does not return incorrect list", () => {
    Expect_notEqual(List_takeNth(3, testList1), testList1_takeNth_NotEqual, "List.takeNth did not return incorrect List");
})])), Test_testList("List.skipNth", ofArray([Test_testCase("returns correct list", () => {
    let copyOfStruct_7, arg_12, arg_1_7;
    const actual_7 = List_skipNth(3, testList1);
    const expected_7 = testList1_skipNth_Equal;
    if (equals(actual_7, expected_7) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_7, expected_7, "List.skipNth did return correct List");
    }
    else {
        throw new Error(contains((copyOfStruct_7 = actual_7, list_type(int32_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals_1,
            GetHashCode: structuralHash,
        }) ? ((arg_12 = seqToString(expected_7), (arg_1_7 = seqToString(actual_7), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_12)(arg_1_7)("List.skipNth did return correct List")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_7)(actual_7)("List.skipNth did return correct List"));
    }
}), Test_testCase("does not return incorrect list", () => {
    Expect_notEqual(List_skipNth(3, testList1), testList1_skipNth_NotEqual, "List.skipNth did not return incorrect List");
})])), Test_testList("List.groupWhen", toList(delay(() => {
    const isOdd = (n_4) => ((n_4 % 2) !== 0);
    return append(singleton_1(Test_testCase("returns correct jagged list, case1: [1337; 14; 23; 23; 69; 1; 2; 3; 1000; 9001; 23]", () => {
        let copyOfStruct_8, arg_13, arg_1_8;
        const actual_8 = List_groupWhen(isOdd, testList1);
        const expected_8 = testList1_groupWhen_Equal;
        if (equals(actual_8, expected_8) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
            assertEqual(actual_8, expected_8, "List.groupWhen did return correct JaggedList");
        }
        else {
            throw new Error(contains((copyOfStruct_8 = actual_8, list_type(list_type(int32_type))), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
                Equals: equals_1,
                GetHashCode: structuralHash,
            }) ? ((arg_13 = seqToString(expected_8), (arg_1_8 = seqToString(actual_8), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_13)(arg_1_8)("List.groupWhen did return correct JaggedList")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_8)(actual_8)("List.groupWhen did return correct JaggedList"));
        }
    })), delay(() => append(singleton_1(Test_testCase("does not return incorrect jagged list, case1: [1337; 14; 23; 23; 69; 1; 2; 3; 1000; 9001; 23]", () => {
        Expect_notEqual(List_groupWhen(isOdd, testList1), testList1_groupWhen_NotEqual, "List.groupWhen did not return incorrect JaggedList");
    })), delay(() => append(singleton_1(Test_testCase("returns correct jagged list, case2: [3; 3; 2; 4; 1; 2]", () => {
        let copyOfStruct_9, arg_14, arg_1_9;
        const actual_9 = List_groupWhen(isOdd, testList2);
        const expected_9 = testList2_groupWhen_Equal;
        if (equals(actual_9, expected_9) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
            assertEqual(actual_9, expected_9, "List.groupWhen did return correct JaggedList");
        }
        else {
            throw new Error(contains((copyOfStruct_9 = actual_9, list_type(list_type(int32_type))), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
                Equals: equals_1,
                GetHashCode: structuralHash,
            }) ? ((arg_14 = seqToString(expected_9), (arg_1_9 = seqToString(actual_9), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_14)(arg_1_9)("List.groupWhen did return correct JaggedList")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_9)(actual_9)("List.groupWhen did return correct JaggedList"));
        }
    })), delay(() => append(singleton_1(Test_testCase("does not return incorrect jagged list, case2: [3; 3; 2; 4; 1; 2]", () => {
        Expect_notEqual(List_groupWhen(isOdd, testList2), testList2_groupWhen_NotEqual, "List.groupWhen did not return incorrect JaggedList");
    })), delay(() => append(singleton_1(Test_testCase("returns correct jagged list, case3: [3; 3; 2; 4; 1; 1]", () => {
        let copyOfStruct_10, arg_15, arg_1_10;
        const actual_10 = List_groupWhen(isOdd, testList3);
        const expected_10 = testList3_groupWhen_Equal;
        if (equals(actual_10, expected_10) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
            assertEqual(actual_10, expected_10, "List.groupWhen did return correct JaggedList");
        }
        else {
            throw new Error(contains((copyOfStruct_10 = actual_10, list_type(list_type(int32_type))), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
                Equals: equals_1,
                GetHashCode: structuralHash,
            }) ? ((arg_15 = seqToString(expected_10), (arg_1_10 = seqToString(actual_10), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_15)(arg_1_10)("List.groupWhen did return correct JaggedList")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_10)(actual_10)("List.groupWhen did return correct JaggedList"));
        }
    })), delay(() => append(singleton_1(Test_testCase("does not return incorrect jagged list, case3: [3; 3; 2; 4; 1; 1]", () => {
        Expect_notEqual(List_groupWhen(isOdd, testList3), testList3_groupWhen_NotEqual, "List.groupWhen did not return incorrect JaggedList");
    })), delay(() => append(singleton_1(Test_testCase("returns correct jagged list, case4: [3; 3; 2; 4; 2; 2]", () => {
        let copyOfStruct_11, arg_16, arg_1_11;
        const actual_11 = List_groupWhen(isOdd, testList4);
        const expected_11 = testList4_groupWhen_Equal;
        if (equals(actual_11, expected_11) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
            assertEqual(actual_11, expected_11, "List.groupWhen did return correct JaggedList");
        }
        else {
            throw new Error(contains((copyOfStruct_11 = actual_11, list_type(list_type(int32_type))), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
                Equals: equals_1,
                GetHashCode: structuralHash,
            }) ? ((arg_16 = seqToString(expected_11), (arg_1_11 = seqToString(actual_11), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_16)(arg_1_11)("List.groupWhen did return correct JaggedList")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_11)(actual_11)("List.groupWhen did return correct JaggedList"));
        }
    })), delay(() => append(singleton_1(Test_testCase("does not return incorrect jagged list, case4: [3; 3; 2; 4; 2; 2]", () => {
        Expect_notEqual(List_groupWhen(isOdd, testList4), testList4_groupWhen_NotEqual, "List.groupWhen did not return incorrect JaggedList");
    })), delay(() => append(singleton_1(Test_testCase("returns correct jagged list, case5: [3; 3; 2; 4; 2; 1]", () => {
        let copyOfStruct_12, arg_17, arg_1_12;
        const actual_12 = List_groupWhen(isOdd, testList5);
        const expected_12 = testList5_groupWhen_Equal;
        if (equals(actual_12, expected_12) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
            assertEqual(actual_12, expected_12, "List.groupWhen did return correct JaggedList");
        }
        else {
            throw new Error(contains((copyOfStruct_12 = actual_12, list_type(list_type(int32_type))), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
                Equals: equals_1,
                GetHashCode: structuralHash,
            }) ? ((arg_17 = seqToString(expected_12), (arg_1_12 = seqToString(actual_12), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_17)(arg_1_12)("List.groupWhen did return correct JaggedList")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_12)(actual_12)("List.groupWhen did return correct JaggedList"));
        }
    })), delay(() => append(singleton_1(Test_testCase("does not return incorrect jagged list, case5: [3; 3; 2; 4; 2; 1]", () => {
        Expect_notEqual(List_groupWhen(isOdd, testList5), testList5_groupWhen_NotEqual, "List.groupWhen did not return incorrect JaggedList");
    })), delay(() => append(singleton_1(Test_testCase("returns correct jagged list, case6: [6; 6; 2; 4; 2; 8]", () => {
        let copyOfStruct_13, arg_18, arg_1_13;
        const actual_13 = List_groupWhen(isOdd, testList6);
        const expected_13 = singleton(testList6);
        if (equals(actual_13, expected_13) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
            assertEqual(actual_13, expected_13, "List.groupWhen did return correct JaggedList");
        }
        else {
            throw new Error(contains((copyOfStruct_13 = actual_13, list_type(list_type(int32_type))), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
                Equals: equals_1,
                GetHashCode: structuralHash,
            }) ? ((arg_18 = seqToString(expected_13), (arg_1_13 = seqToString(actual_13), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_18)(arg_1_13)("List.groupWhen did return correct JaggedList")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_13)(actual_13)("List.groupWhen did return correct JaggedList"));
        }
    })), delay(() => singleton_1(Test_testCase("does not return incorrect jagged list, case6: [6; 6; 2; 4; 2; 8]", () => {
        Expect_notEqual(List_groupWhen(isOdd, testList6), singleton(empty()), "List.groupWhen did not return incorrect JaggedList");
    }))))))))))))))))))))))));
}))), Test_testList("List.intersect", ofArray([Test_testCase("does not return incorrect list, case1: []", () => {
    let copyOfStruct_14, arg_19, arg_1_14;
    const actual_14 = List_intersect(empty(), empty());
    const expected_14 = empty();
    if (equals(actual_14, expected_14) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_14, expected_14, "List.intersect did return correct list");
    }
    else {
        throw new Error(contains((copyOfStruct_14 = actual_14, list_type(obj_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals_1,
            GetHashCode: structuralHash,
        }) ? ((arg_19 = seqToString(expected_14), (arg_1_14 = seqToString(actual_14), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_19)(arg_1_14)("List.intersect did return correct list")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_14)(actual_14)("List.intersect did return correct list"));
    }
}), Test_testCase("does not return incorrect list, case2: []", () => {
    let copyOfStruct_15, arg_20, arg_1_15;
    const actual_15 = List_intersect(empty(), testList6);
    const expected_15 = empty();
    if (equals(actual_15, expected_15) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_15, expected_15, "List.intersect did return correct list");
    }
    else {
        throw new Error(contains((copyOfStruct_15 = actual_15, list_type(int32_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals_1,
            GetHashCode: structuralHash,
        }) ? ((arg_20 = seqToString(expected_15), (arg_1_15 = seqToString(actual_15), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_20)(arg_1_15)("List.intersect did return correct list")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_15)(actual_15)("List.intersect did return correct list"));
    }
}), Test_testCase("does not return incorrect list, case3: []", () => {
    let copyOfStruct_16, arg_21, arg_1_16;
    const actual_16 = List_intersect(testList5, empty());
    const expected_16 = empty();
    if (equals(actual_16, expected_16) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_16, expected_16, "List.intersect did return correct list");
    }
    else {
        throw new Error(contains((copyOfStruct_16 = actual_16, list_type(int32_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals_1,
            GetHashCode: structuralHash,
        }) ? ((arg_21 = seqToString(expected_16), (arg_1_16 = seqToString(actual_16), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_21)(arg_1_16)("List.intersect did return correct list")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_16)(actual_16)("List.intersect did return correct list"));
    }
}), Test_testCase("does not return incorrect list, case4: [2; 4]", () => {
    let copyOfStruct_17, arg_22, arg_1_17;
    const actual_17 = List_intersect(testList5, testList6);
    const expected_17 = ofArray([2, 4]);
    if (equals(actual_17, expected_17) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_17, expected_17, "List.intersect did return correct list");
    }
    else {
        throw new Error(contains((copyOfStruct_17 = actual_17, list_type(int32_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals_1,
            GetHashCode: structuralHash,
        }) ? ((arg_22 = seqToString(expected_17), (arg_1_17 = seqToString(actual_17), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_22)(arg_1_17)("List.intersect did return correct list")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_17)(actual_17)("List.intersect did return correct list"));
    }
})]))]));

