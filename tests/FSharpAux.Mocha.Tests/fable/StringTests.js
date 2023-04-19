import { Expect_throws, Expect_notEqual, Expect_isFalse, Expect_isTrue, Test_testCase, Test_testList } from "./fable_modules/Fable.Mocha.2.16.0/Mocha.fs.js";
import { String_skipWhile, String_takeWhile, String_findIndicesBack, String_findIndices, String_findIndex, String_findIndexBack, String_splitS, String_last, String_first, String_trim } from "./src/FSharpAux.Core/String.js";
import { int32ToString, structuralHash, assertEqual } from "./fable_modules/fable-library.4.0.6/Util.js";
import { singleton, ofArray, contains } from "./fable_modules/fable-library.4.0.6/List.js";
import { array_type, char_type, equals, class_type, decimal_type, float64_type, bool_type, int32_type, string_type } from "./fable_modules/fable-library.4.0.6/Reflection.js";
import { replace, printf, toText } from "./fable_modules/fable-library.4.0.6/String.js";
import { equalsWith } from "./fable_modules/fable-library.4.0.6/Array.js";
import { seqToString } from "./fable_modules/fable-library.4.0.6/Types.js";

export const stringTests = Test_testList("StringTests", ofArray([Test_testList("String.trim", singleton(Test_testCase("trims input correctly", () => {
    let copyOfStruct;
    const actual = String_trim("      \tHi there!\n     ");
    if ((actual === "Hi there!") ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual, "Hi there!", "String.trim did not trim correctly");
    }
    else {
        throw new Error(contains((copyOfStruct = actual, string_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))("Hi there!")(actual)("String.trim did not trim correctly") : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))("Hi there!")(actual)("String.trim did not trim correctly"));
    }
}))), Test_testList("String.contains", ofArray([Test_testCase("contains present substring", () => {
    Expect_isTrue("Hi there!".indexOf("Hi") >= 0)("String.contains did not return true for proper substring");
}), Test_testCase("does not contain missing substring", () => {
    Expect_isFalse("Hi there!".indexOf("soos") >= 0)("String.contains did not return false for improper substring");
})])), Test_testList("String.startsWith", ofArray([Test_testCase("does start with", () => {
    Expect_isTrue("Hi there!".indexOf("Hi ") === 0)("String.startsWith did not return true for proper substring");
}), Test_testCase("does not fail to start with", () => {
    Expect_isFalse("Hi there!".indexOf("soos") === 0)("String.startsWith did not return false for improper substring");
})])), Test_testList("String.replace", ofArray([Test_testCase("replaces", () => {
    let copyOfStruct_1;
    const actual_1 = replace("Hi there!", "Hi ", "Yo ");
    if ((actual_1 === "Yo there!") ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_1, "Yo there!", "String.replaces did return correct replacement");
    }
    else {
        throw new Error(contains((copyOfStruct_1 = actual_1, string_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))("Yo there!")(actual_1)("String.replaces did return correct replacement") : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))("Yo there!")(actual_1)("String.replaces did return correct replacement"));
    }
}), Test_testCase("does not fail to replace", () => {
    Expect_notEqual(replace("Hi there!", "soos", "Yo"), "Yo there!", "String.replaces did not return incorrect replacement");
})])), Test_testList("String.first", ofArray([Test_testCase("gives first", () => {
    let copyOfStruct_2;
    const actual_2 = String_first("Hi there!");
    if ((actual_2 === "H") ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_2, "H", "String.first did return correct Char");
    }
    else {
        throw new Error(contains((copyOfStruct_2 = actual_2, char_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))("H")(actual_2)("String.first did return correct Char") : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))("H")(actual_2)("String.first did return correct Char"));
    }
}), Test_testCase("does not give anything else", () => {
    Expect_notEqual(String_first("Hi there!"), "!", "String.first did not return incorrect Char");
}), Test_testCase("errors if empty", () => {
    Expect_throws(() => {
        String_first("");
    }, "String.first did throw an exception");
})])), Test_testList("String.last", ofArray([Test_testCase("gives last", () => {
    let copyOfStruct_3;
    const actual_3 = String_last("Hi there!");
    if ((actual_3 === "!") ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_3, "!", "String.first did return correct Char");
    }
    else {
        throw new Error(contains((copyOfStruct_3 = actual_3, char_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))("!")(actual_3)("String.first did return correct Char") : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))("!")(actual_3)("String.first did return correct Char"));
    }
}), Test_testCase("does not give anything else", () => {
    Expect_notEqual(String_last("Hi there!"), "u", "String.first did not return incorrect Char");
}), Test_testCase("errors if empty", () => {
    Expect_throws(() => {
        String_last("");
    }, "String.last did throw an exception");
})])), Test_testList("String.splitS", ofArray([Test_testCase("splits correctly", () => {
    let copyOfStruct_4, arg_9, arg_1_4;
    const actual_4 = String_splitS(" t", "Hi there!");
    const expected_4 = ["Hi", "here!"];
    if (equalsWith((x_4, y_4) => (x_4 === y_4), actual_4, expected_4) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_4, expected_4, "String.splitS did return correct string array");
    }
    else {
        throw new Error(contains((copyOfStruct_4 = actual_4, array_type(string_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_9 = seqToString(expected_4), (arg_1_4 = seqToString(actual_4), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_9)(arg_1_4)("String.splitS did return correct string array")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_4)(actual_4)("String.splitS did return correct string array"));
    }
}), Test_testCase("does not splits incorrectly", () => {
    Expect_notEqual(String_splitS(" t", "Hi there!"), ["Hi ", "there!"], "String.first did not return incorrect string array");
})])), Test_testList("String.findIndexBack", ofArray([Test_testCase("finds correct index", () => {
    let copyOfStruct_5, arg_10, arg_1_5;
    const actual_5 = String_findIndexBack(" ", "Hi there!") | 0;
    if ((actual_5 === 2) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_5, 2, "String.findIndexBack did return correct index");
    }
    else {
        throw new Error(contains((copyOfStruct_5 = actual_5, int32_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_10 = int32ToString(2), (arg_1_5 = int32ToString(actual_5), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_10)(arg_1_5)("String.findIndexBack did return correct index")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(2)(actual_5)("String.findIndexBack did return correct index"));
    }
}), Test_testCase("does not find incorrect index", () => {
    Expect_notEqual(String_findIndexBack(" ", "Hi there!"), 3, "String.findIndexBack did not return incorrect index");
})])), Test_testList("String.findIndex", ofArray([Test_testCase("finds correct index", () => {
    let copyOfStruct_6, arg_11, arg_1_6;
    const actual_6 = String_findIndex(" ", "Hi there!") | 0;
    if ((actual_6 === 2) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_6, 2, "String.findIndex did return correct index");
    }
    else {
        throw new Error(contains((copyOfStruct_6 = actual_6, int32_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_11 = int32ToString(2), (arg_1_6 = int32ToString(actual_6), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_11)(arg_1_6)("String.findIndex did return correct index")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(2)(actual_6)("String.findIndex did return correct index"));
    }
}), Test_testCase("does not find incorrect index", () => {
    Expect_notEqual(String_findIndex(" ", "Hi there!"), 3, "String.findIndex did not return incorrect index");
})])), Test_testList("String.findIndices", ofArray([Test_testCase("finds correct indices", () => {
    let copyOfStruct_7, arg_12, arg_1_7;
    const actual_7 = String_findIndices("e", "Hi there!");
    const expected_7 = new Int32Array([5, 7]);
    if (equalsWith((x_8, y_8) => (x_8 === y_8), actual_7, expected_7) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_7, expected_7, "String.findIndices did return correct indices");
    }
    else {
        throw new Error(contains((copyOfStruct_7 = actual_7, array_type(int32_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_12 = seqToString(expected_7), (arg_1_7 = seqToString(actual_7), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_12)(arg_1_7)("String.findIndices did return correct indices")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_7)(actual_7)("String.findIndices did return correct indices"));
    }
}), Test_testCase("does not find incorrect indices", () => {
    Expect_notEqual(String_findIndices("e", "Hi there!"), new Int32Array([0, 1]), "String.findIndices did not return incorrect indices");
})])), Test_testList("String.findIndicesBack", ofArray([Test_testCase("finds correct indices", () => {
    let copyOfStruct_8, arg_13, arg_1_8;
    const actual_8 = String_findIndicesBack("e", "Hi there!");
    const expected_8 = new Int32Array([7, 5]);
    if (equalsWith((x_10, y_10) => (x_10 === y_10), actual_8, expected_8) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_8, expected_8, "String.findIndicesBack did return correct indices");
    }
    else {
        throw new Error(contains((copyOfStruct_8 = actual_8, array_type(int32_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_13 = seqToString(expected_8), (arg_1_8 = seqToString(actual_8), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_13)(arg_1_8)("String.findIndicesBack did return correct indices")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_8)(actual_8)("String.findIndicesBack did return correct indices"));
    }
}), Test_testCase("does not find incorrect indices", () => {
    Expect_notEqual(String_findIndicesBack("e", "Hi there!"), new Int32Array([0, 1]), "String.findIndicesBack did not return incorrect indices");
})])), Test_testList("String.takeWhile", ofArray([Test_testCase("finds correct substring", () => {
    let copyOfStruct_9;
    const actual_9 = String_takeWhile((y_12) => ("H" === y_12), "Hi there!");
    if ((actual_9 === "H") ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_9, "H", "String.takeWhile did return correct substring");
    }
    else {
        throw new Error(contains((copyOfStruct_9 = actual_9, string_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))("H")(actual_9)("String.takeWhile did return correct substring") : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))("H")(actual_9)("String.takeWhile did return correct substring"));
    }
}), Test_testCase("does not find incorrect substring", () => {
    Expect_notEqual(String_takeWhile((y_14) => ("H" === y_14), "Hi there!"), "fdsfds", "String.takeWhile did not return incorrect substring");
})])), Test_testList("String.skipWhile", ofArray([Test_testCase("finds correct substring", () => {
    let copyOfStruct_10;
    const actual_10 = String_skipWhile((y_15) => ("H" === y_15), "Hi there!");
    if ((actual_10 === "i there!") ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_10, "i there!", "String.skipWhile did return correct substring");
    }
    else {
        throw new Error(contains((copyOfStruct_10 = actual_10, string_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))("i there!")(actual_10)("String.skipWhile did return correct substring") : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))("i there!")(actual_10)("String.skipWhile did return correct substring"));
    }
}), Test_testCase("does not find incorrect substring", () => {
    Expect_notEqual(String_skipWhile((y_17) => ("H" === y_17), "Hi there!"), "fdsfds", "String.skipWhile did not return incorrect substring");
})]))]));

