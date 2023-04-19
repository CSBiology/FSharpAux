import { empty as empty_1, map, toList, singleton, append, delay } from "./fable_modules/fable-library.4.0.6/Seq.js";
import { Expect_notEqual, Test_testCase, Test_testList } from "./fable_modules/Fable.Mocha.2.16.0/Mocha.fs.js";
import { intersect, groupWhen } from "./src/FSharpAux.Core/Seq.js";
import { structuralHash, assertEqual, equals } from "./fable_modules/fable-library.4.0.6/Util.js";
import { empty, singleton as singleton_1, ofArray, contains } from "./fable_modules/fable-library.4.0.6/List.js";
import { obj_type, equals as equals_1, class_type, decimal_type, string_type, float64_type, bool_type, list_type, int32_type } from "./fable_modules/fable-library.4.0.6/Reflection.js";
import { seqToString } from "./fable_modules/fable-library.4.0.6/Types.js";
import { printf, toText } from "./fable_modules/fable-library.4.0.6/String.js";

const testSeq1 = delay(() => append(singleton(1337), delay(() => append(singleton(14), delay(() => append(singleton(23), delay(() => append(singleton(23), delay(() => append(singleton(69), delay(() => append(singleton(1), delay(() => append(singleton(2), delay(() => append(singleton(3), delay(() => append(singleton(1000), delay(() => append(singleton(9001), delay(() => singleton(23))))))))))))))))))))));

const testSeq2 = delay(() => append(singleton(3), delay(() => append(singleton(3), delay(() => append(singleton(2), delay(() => append(singleton(4), delay(() => append(singleton(1), delay(() => singleton(2))))))))))));

const testSeq3 = delay(() => append(singleton(3), delay(() => append(singleton(3), delay(() => append(singleton(2), delay(() => append(singleton(4), delay(() => append(singleton(1), delay(() => singleton(1))))))))))));

const testSeq4 = delay(() => append(singleton(3), delay(() => append(singleton(3), delay(() => append(singleton(2), delay(() => append(singleton(4), delay(() => append(singleton(2), delay(() => singleton(2))))))))))));

const testSeq5 = delay(() => append(singleton(3), delay(() => append(singleton(3), delay(() => append(singleton(2), delay(() => append(singleton(4), delay(() => append(singleton(2), delay(() => singleton(1))))))))))));

const testSeq6 = delay(() => append(singleton(6), delay(() => append(singleton(6), delay(() => append(singleton(2), delay(() => append(singleton(4), delay(() => append(singleton(2), delay(() => singleton(8))))))))))));

const testSeq2_groupWhen_Equal = delay(() => append(singleton(delay(() => singleton(3))), delay(() => append(singleton(delay(() => append(singleton(3), delay(() => append(singleton(2), delay(() => singleton(4))))))), delay(() => singleton(delay(() => append(singleton(1), delay(() => singleton(2))))))))));

const testSeq2_groupWhen_NotEqual = delay(() => append(singleton(delay(() => singleton(3))), delay(() => append(singleton(delay(() => append(singleton(3), delay(() => append(singleton(2), delay(() => singleton(4))))))), delay(() => append(singleton(delay(() => singleton(1))), delay(() => singleton(delay(() => singleton(2))))))))));

const testSeq3_groupWhen_Equal = delay(() => append(singleton(delay(() => singleton(3))), delay(() => append(singleton(delay(() => append(singleton(3), delay(() => append(singleton(2), delay(() => singleton(4))))))), delay(() => append(singleton(delay(() => singleton(1))), delay(() => singleton(delay(() => singleton(1))))))))));

const testSeq3_groupWhen_NotEqual = delay(() => append(singleton(delay(() => singleton(3))), delay(() => append(singleton(delay(() => append(singleton(3), delay(() => append(singleton(2), delay(() => singleton(4))))))), delay(() => singleton(delay(() => append(singleton(1), delay(() => singleton(1))))))))));

const testSeq4_groupWhen_Equal = delay(() => append(singleton(delay(() => singleton(3))), delay(() => singleton(delay(() => append(singleton(3), delay(() => append(singleton(2), delay(() => append(singleton(4), delay(() => append(singleton(2), delay(() => singleton(2))))))))))))));

const testSeq4_groupWhen_NotEqual = delay(() => append(singleton(delay(() => singleton(3))), delay(() => append(singleton(delay(() => append(singleton(3), delay(() => append(singleton(2), delay(() => append(singleton(4), delay(() => singleton(2))))))))), delay(() => singleton(delay(() => singleton(2))))))));

const testSeq5_groupWhen_Equal = delay(() => append(singleton(delay(() => singleton(3))), delay(() => append(singleton(delay(() => append(singleton(3), delay(() => append(singleton(2), delay(() => append(singleton(4), delay(() => singleton(2))))))))), delay(() => singleton(delay(() => singleton(1))))))));

const testSeq5_groupWhen_NotEqual = delay(() => append(singleton(delay(() => singleton(3))), delay(() => singleton(delay(() => append(singleton(3), delay(() => append(singleton(2), delay(() => append(singleton(4), delay(() => append(singleton(2), delay(() => singleton(1))))))))))))));

export function list(s) {
    return toList(s);
}

export function list2(s) {
    return toList(map(toList, s));
}

export const seqTests = Test_testList("SeqTests", toList(delay(() => {
    const isOdd = (n) => ((n % 2) !== 0);
    return append(singleton(Test_testList("Seq.groupWhen", ofArray([Test_testCase("returns correct jagged list, case1: [3; 3; 2; 4; 1; 2]", () => {
        let copyOfStruct, arg, arg_1;
        const actual = list2(groupWhen(isOdd, testSeq2));
        const expected = list2(testSeq2_groupWhen_Equal);
        if (equals(actual, expected) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
            assertEqual(actual, expected, "Seq.groupWhen did return correct jagged list");
        }
        else {
            throw new Error(contains((copyOfStruct = actual, list_type(list_type(int32_type))), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
                Equals: equals_1,
                GetHashCode: structuralHash,
            }) ? ((arg = seqToString(expected), (arg_1 = seqToString(actual), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg)(arg_1)("Seq.groupWhen did return correct jagged list")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected)(actual)("Seq.groupWhen did return correct jagged list"));
        }
    }), Test_testCase("does not return incorrect jagged list, case1: [3; 3; 2; 4; 1; 2]", () => {
        Expect_notEqual(list2(groupWhen(isOdd, testSeq2)), list2(testSeq2_groupWhen_NotEqual), "Seq.groupWhen did not return incorrect jagged list");
    }), Test_testCase("returns correct jagged list, case2: [3; 3; 2; 4; 1; 1]", () => {
        let copyOfStruct_1, arg_6, arg_1_1;
        const actual_1 = list2(groupWhen(isOdd, testSeq3));
        const expected_1 = list2(testSeq3_groupWhen_Equal);
        if (equals(actual_1, expected_1) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
            assertEqual(actual_1, expected_1, "Seq.groupWhen did return correct jagged list");
        }
        else {
            throw new Error(contains((copyOfStruct_1 = actual_1, list_type(list_type(int32_type))), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
                Equals: equals_1,
                GetHashCode: structuralHash,
            }) ? ((arg_6 = seqToString(expected_1), (arg_1_1 = seqToString(actual_1), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_6)(arg_1_1)("Seq.groupWhen did return correct jagged list")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_1)(actual_1)("Seq.groupWhen did return correct jagged list"));
        }
    }), Test_testCase("does not return incorrect jagged list, case2: [3; 3; 2; 4; 1; 1]", () => {
        Expect_notEqual(list2(groupWhen(isOdd, testSeq3)), list2(testSeq3_groupWhen_NotEqual), "Seq.groupWhen did not return incorrect jagged list");
    }), Test_testCase("returns correct jagged list, case3: [3; 3; 2; 4; 2; 2]", () => {
        let copyOfStruct_2, arg_7, arg_1_2;
        const actual_2 = list2(groupWhen(isOdd, testSeq4));
        const expected_2 = list2(testSeq4_groupWhen_Equal);
        if (equals(actual_2, expected_2) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
            assertEqual(actual_2, expected_2, "Seq.groupWhen did return correct jagged list");
        }
        else {
            throw new Error(contains((copyOfStruct_2 = actual_2, list_type(list_type(int32_type))), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
                Equals: equals_1,
                GetHashCode: structuralHash,
            }) ? ((arg_7 = seqToString(expected_2), (arg_1_2 = seqToString(actual_2), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_7)(arg_1_2)("Seq.groupWhen did return correct jagged list")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_2)(actual_2)("Seq.groupWhen did return correct jagged list"));
        }
    }), Test_testCase("does not return incorrect jagged list, case3: [3; 3; 2; 4; 2; 2]", () => {
        Expect_notEqual(list2(groupWhen(isOdd, testSeq4)), list2(testSeq4_groupWhen_NotEqual), "Seq.groupWhen did not return incorrect jagged list");
    }), Test_testCase("returns correct jagged list, case4: [3; 3; 2; 4; 2; 1]", () => {
        let copyOfStruct_3, arg_8, arg_1_3;
        const actual_3 = list2(groupWhen(isOdd, testSeq5));
        const expected_3 = list2(testSeq5_groupWhen_Equal);
        if (equals(actual_3, expected_3) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
            assertEqual(actual_3, expected_3, "Seq.groupWhen did return correct jagged list");
        }
        else {
            throw new Error(contains((copyOfStruct_3 = actual_3, list_type(list_type(int32_type))), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
                Equals: equals_1,
                GetHashCode: structuralHash,
            }) ? ((arg_8 = seqToString(expected_3), (arg_1_3 = seqToString(actual_3), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_8)(arg_1_3)("Seq.groupWhen did return correct jagged list")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_3)(actual_3)("Seq.groupWhen did return correct jagged list"));
        }
    }), Test_testCase("does not return incorrect jagged list, case4: [3; 3; 2; 4; 2; 1]", () => {
        Expect_notEqual(list2(groupWhen(isOdd, testSeq5)), list2(testSeq5_groupWhen_NotEqual), "Seq.groupWhen did not return incorrect jagged list");
    }), Test_testCase("returns correct jagged list, case4: [6; 6; 2; 4; 2; 8]", () => {
        let copyOfStruct_4, arg_9, arg_1_4;
        const actual_4 = list2(groupWhen(isOdd, testSeq6));
        const expected_4 = singleton_1(list(testSeq6));
        if (equals(actual_4, expected_4) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
            assertEqual(actual_4, expected_4, "Seq.groupWhen did return correct jagged list");
        }
        else {
            throw new Error(contains((copyOfStruct_4 = actual_4, list_type(list_type(int32_type))), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
                Equals: equals_1,
                GetHashCode: structuralHash,
            }) ? ((arg_9 = seqToString(expected_4), (arg_1_4 = seqToString(actual_4), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_9)(arg_1_4)("Seq.groupWhen did return correct jagged list")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_4)(actual_4)("Seq.groupWhen did return correct jagged list"));
        }
    }), Test_testCase("does not return incorrect jagged list, case4: [6; 6; 2; 4; 2; 8]", () => {
        Expect_notEqual(list2(groupWhen(isOdd, testSeq6)), empty(), "Seq.groupWhen did not return empty (jagged) list");
    })]))), delay(() => singleton(Test_testList("Seq.intersect", ofArray([Test_testCase("returns correct list, case1: []", () => {
        let copyOfStruct_5, arg_10, arg_1_5;
        const actual_5 = list(intersect(empty_1(), empty_1()));
        const expected_5 = empty();
        if (equals(actual_5, expected_5) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
            assertEqual(actual_5, expected_5, "Seq.intersect did return correct list");
        }
        else {
            throw new Error(contains((copyOfStruct_5 = actual_5, list_type(obj_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
                Equals: equals_1,
                GetHashCode: structuralHash,
            }) ? ((arg_10 = seqToString(expected_5), (arg_1_5 = seqToString(actual_5), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_10)(arg_1_5)("Seq.intersect did return correct list")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_5)(actual_5)("Seq.intersect did return correct list"));
        }
    }), Test_testCase("returns correct list, case2: []", () => {
        let copyOfStruct_6, arg_11, arg_1_6;
        const actual_6 = list(intersect(empty_1(), testSeq1));
        const expected_6 = empty();
        if (equals(actual_6, expected_6) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
            assertEqual(actual_6, expected_6, "Seq.intersect did return correct list");
        }
        else {
            throw new Error(contains((copyOfStruct_6 = actual_6, list_type(int32_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
                Equals: equals_1,
                GetHashCode: structuralHash,
            }) ? ((arg_11 = seqToString(expected_6), (arg_1_6 = seqToString(actual_6), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_11)(arg_1_6)("Seq.intersect did return correct list")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_6)(actual_6)("Seq.intersect did return correct list"));
        }
    }), Test_testCase("returns correct list, case3: []", () => {
        let copyOfStruct_7, arg_12, arg_1_7;
        const actual_7 = list(intersect(testSeq2, empty_1()));
        const expected_7 = empty();
        if (equals(actual_7, expected_7) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
            assertEqual(actual_7, expected_7, "Seq.intersect did return correct list");
        }
        else {
            throw new Error(contains((copyOfStruct_7 = actual_7, list_type(int32_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
                Equals: equals_1,
                GetHashCode: structuralHash,
            }) ? ((arg_12 = seqToString(expected_7), (arg_1_7 = seqToString(actual_7), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_12)(arg_1_7)("Seq.intersect did return correct list")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_7)(actual_7)("Seq.intersect did return correct list"));
        }
    }), Test_testCase("returns correct list, case4: [2; 4]", () => {
        let copyOfStruct_8, arg_13, arg_1_8;
        const actual_8 = list(intersect(testSeq5, testSeq6));
        const expected_8 = ofArray([2, 4]);
        if (equals(actual_8, expected_8) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
            assertEqual(actual_8, expected_8, "Seq.intersect did return correct list");
        }
        else {
            throw new Error(contains((copyOfStruct_8 = actual_8, list_type(int32_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
                Equals: equals_1,
                GetHashCode: structuralHash,
            }) ? ((arg_13 = seqToString(expected_8), (arg_1_8 = seqToString(actual_8), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_13)(arg_1_8)("Seq.intersect did return correct list")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_8)(actual_8)("Seq.intersect did return correct list"));
        }
    })])))));
})));

