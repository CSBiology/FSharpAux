import { Mocha_runTests, Test_testList } from "./fable_modules/Fable.Mocha.2.16.0/Mocha.fs.js";
import { seqTests } from "./SeqTests.js";
import { arrayTests } from "./ArrayTests.js";
import { main as main_1 } from "./JaggedArrayTest.js";
import { listTests } from "./ListTests.js";
import { mathTests } from "./MathTests.js";
import { stringTests } from "./StringTests.js";
import { ofArray } from "./fable_modules/fable-library.4.0.6/List.js";

export const all = Test_testList("All", ofArray([seqTests, arrayTests, main_1, listTests, mathTests, stringTests]));

(function (argv) {
    return Mocha_runTests(all);
})(typeof process === 'object' ? process.argv.slice(2) : []);

