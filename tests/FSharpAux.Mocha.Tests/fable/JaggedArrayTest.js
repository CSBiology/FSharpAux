import { Expect_notEqual, Test_testCase, Test_testList } from "./fable_modules/Fable.Mocha.2.16.0/Mocha.fs.js";
import { JaggedList_init, JaggedArray_init, JaggedArray_toIndexedArray } from "./src/FSharpAux.Core/JaggedArray.js";
import { equalsWith } from "./fable_modules/fable-library.4.0.6/Array.js";
import { equals as equals_1, structuralHash, assertEqual, equalArrays } from "./fable_modules/fable-library.4.0.6/Util.js";
import { singleton, ofArray, contains } from "./fable_modules/fable-library.4.0.6/List.js";
import { list_type, equals, class_type, decimal_type, float64_type, bool_type, array_type, tuple_type, string_type, int32_type } from "./fable_modules/fable-library.4.0.6/Reflection.js";
import { seqToString } from "./fable_modules/fable-library.4.0.6/Types.js";
import { printf, toText } from "./fable_modules/fable-library.4.0.6/String.js";

const testArr2d1_ofArray2D_Equal = [new Int32Array([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]), new Int32Array([99, 66, 44, 11, 22, 33, 55, 0, 11, 0])];

const testArr2d1_ofArray2D_NotEqual = [new Int32Array([5, 6, 7, 8, 9, 0, 1, 2, 3, 4]), new Int32Array([55, 0, 11, 99, 66, 44, 11, 22, 33, 0])];

const testJaggArr1 = [["I ", "am ", "Jesus!"], ["No", " you", "are", "not"]];

const testJaggArr1_init_Equal = [[[0, 0], [0, 3], [0, 6], [0, 9], [0, 12], [0, 15], [0, 18]], [[2, 0], [2, 3], [2, 6], [2, 9], [2, 12], [2, 15], [2, 18]], [[4, 0], [4, 3], [4, 6], [4, 9], [4, 12], [4, 15], [4, 18]]];

const testJaggArr1_init_NotEqual = [[[2, 0], [2, 3], [2, 6], [2, 9], [2, 12], [2, 15], [2, 18]], [[0, 0], [0, 3], [0, 6], [0, 9], [0, 12], [0, 15], [0, 18]], [[4, 0], [4, 3], [4, 6], [4, 9], [4, 12], [4, 15], [4, 18]]];

const testJaggArr1_toIndexedArray_Equal = [[0, 0, "I "], [0, 1, "am "], [0, 2, "Jesus!"], [1, 0, "No"], [1, 1, " you"], [1, 2, "are"], [1, 3, "not"]];

const testJaggArr1_toIndexedArray_NotEqual = [[1, 0, "No"], [1, 1, " you"], [1, 2, "are"], [1, 3, "not"], [0, 0, "I "], [0, 1, "am "], [0, 2, "Jesus!"]];

const jaggedArrayTests = Test_testList("JaggedArrayTests", ofArray([Test_testList("JaggedArray.toIndexedArray", ofArray([Test_testCase("returns correct array", () => {
    let copyOfStruct, arg, arg_1;
    const actual = JaggedArray_toIndexedArray(testJaggArr1);
    const expected = testJaggArr1_toIndexedArray_Equal;
    if (equalsWith(equalArrays, actual, expected) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual, expected, "JaggedArray.toIndexedArray did return correct array");
    }
    else {
        throw new Error(contains((copyOfStruct = actual, array_type(tuple_type(int32_type, int32_type, string_type))), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg = seqToString(expected), (arg_1 = seqToString(actual), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg)(arg_1)("JaggedArray.toIndexedArray did return correct array")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected)(actual)("JaggedArray.toIndexedArray did return correct array"));
    }
}), Test_testCase("does not return incorrect array", () => {
    Expect_notEqual(JaggedArray_toIndexedArray(testJaggArr1), testJaggArr1_toIndexedArray_NotEqual, "JaggedArray.toIndexedArray did not return incorrect array");
})])), Test_testList("JaggedArray.init", ofArray([Test_testCase("returns correct jagged array", () => {
    let copyOfStruct_1, arg_6, arg_1_1;
    const actual_1 = JaggedArray_init(3, 7, (i, j) => [i * 2, j * 3]);
    const expected_1 = testJaggArr1_init_Equal;
    if (equalsWith((x_2, y_2) => equalsWith(equalArrays, x_2, y_2), actual_1, expected_1) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_1, expected_1, "JaggedArray.init did return correct jagged array");
    }
    else {
        throw new Error(contains((copyOfStruct_1 = actual_1, array_type(array_type(tuple_type(int32_type, int32_type)))), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_6 = seqToString(expected_1), (arg_1_1 = seqToString(actual_1), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_6)(arg_1_1)("JaggedArray.init did return correct jagged array")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_1)(actual_1)("JaggedArray.init did return correct jagged array"));
    }
}), Test_testCase("does not return incorrect jagged array", () => {
    Expect_notEqual(JaggedArray_init(3, 7, (i_1, j_1) => [i_1 * 2, j_1 * 3]), testJaggArr1_init_NotEqual, "JaggedArray.init did not return incorrect jagged array");
})]))]));

const testJaggList1_init_Equal = ofArray([ofArray([[0, 0], [0, 3], [0, 6], [0, 9], [0, 12], [0, 15], [0, 18]]), ofArray([[2, 0], [2, 3], [2, 6], [2, 9], [2, 12], [2, 15], [2, 18]]), ofArray([[4, 0], [4, 3], [4, 6], [4, 9], [4, 12], [4, 15], [4, 18]])]);

const testJaggList1_init_NotEqual = ofArray([ofArray([[2, 0], [2, 3], [2, 6], [2, 9], [2, 12], [2, 15], [2, 18]]), ofArray([[0, 0], [0, 3], [0, 6], [0, 9], [0, 12], [0, 15], [0, 18]]), ofArray([[4, 0], [4, 3], [4, 6], [4, 9], [4, 12], [4, 15], [4, 18]])]);

const jaggedListTests = Test_testList("JaggedListTests", singleton(Test_testList("JaggedList.init", ofArray([Test_testCase("returns correct list", () => {
    let copyOfStruct, arg, arg_1;
    const actual = JaggedList_init(3, 7, (i, j) => [i * 2, j * 3]);
    const expected = testJaggList1_init_Equal;
    if (equals_1(actual, expected) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual, expected, "JaggedList.init did return correct list");
    }
    else {
        throw new Error(contains((copyOfStruct = actual, list_type(list_type(tuple_type(int32_type, int32_type)))), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg = seqToString(expected), (arg_1 = seqToString(actual), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg)(arg_1)("JaggedList.init did return correct list")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected)(actual)("JaggedList.init did return correct list"));
    }
}), Test_testCase("does not return incorrect list", () => {
    Expect_notEqual(JaggedList_init(3, 7, (i_1, j_1) => [i_1 * 2, j_1 * 3]), testJaggList1_init_NotEqual, "JaggedList.init did not return incorrect list");
})]))));

export const main = Test_testList("JaggedArray", ofArray([jaggedArrayTests, jaggedListTests]));

