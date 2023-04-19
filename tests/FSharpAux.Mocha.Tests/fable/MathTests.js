import { Expect_notEqual, Test_testCase, Test_testList } from "./fable_modules/Fable.Mocha.2.16.0/Mocha.fs.js";
import { structuralHash, assertEqual } from "./fable_modules/fable-library.4.0.6/Util.js";
import { singleton, ofArray, contains } from "./fable_modules/fable-library.4.0.6/List.js";
import { equals, class_type, decimal_type, string_type, bool_type, int32_type, float64_type } from "./fable_modules/fable-library.4.0.6/Reflection.js";
import { printf, toText } from "./fable_modules/fable-library.4.0.6/String.js";

export const mathTests = Test_testList("MathTests", singleton(Test_testList("Math.nthRoot", ofArray([Test_testCase("returns correct float", () => {
    let copyOfStruct, arg, arg_1;
    const actual = Math.pow(27, 1 / 3);
    if ((actual === 3) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual, 3, "Math.nthRoot did return correct float");
    }
    else {
        throw new Error(contains((copyOfStruct = actual, float64_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg = (3).toString(), (arg_1 = actual.toString(), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg)(arg_1)("Math.nthRoot did return correct float")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(3)(actual)("Math.nthRoot did return correct float"));
    }
}), Test_testCase("does not return incorrect float", () => {
    Expect_notEqual(Math.pow(27, 1 / 3), 9, "Math.nthRoot did not return incorrect float");
})]))));

